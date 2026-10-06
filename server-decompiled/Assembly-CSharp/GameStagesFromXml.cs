using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x02000D1F RID: 3359
public class GameStagesFromXml
{
	// Token: 0x06006614 RID: 26132 RVA: 0x00283320 File Offset: 0x00281520
	public static IEnumerator Load(XmlFile _xmlFile)
	{
		GameStageGroup.Clear();
		List<GameStagesFromXml.Group> list = new List<GameStagesFromXml.Group>();
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new XmlLoadException("gamestages.xml", root, "Missing root element!");
		}
		foreach (XElement xelement in root.Elements())
		{
			if (xelement.Name == "spawner")
			{
				GameStagesFromXml.ParseGameStageDef(xelement);
			}
			else if (xelement.Name == "group")
			{
				GameStagesFromXml.Group group = GameStagesFromXml.ParseGameStageGroup(xelement);
				list.Add(group);
			}
			else if (xelement.Name == "config")
			{
				if (xelement.HasAttribute("startingWeight"))
				{
					GameStageDefinition.StartingWeight = StringParsers.ParseFloat(xelement.GetAttribute("startingWeight"), 0, -1, NumberStyles.Any);
				}
				if (xelement.HasAttribute("difficultyBonus"))
				{
					GameStageDefinition.DifficultyBonus = StringParsers.ParseFloat(xelement.GetAttribute("difficultyBonus"), 0, -1, NumberStyles.Any);
				}
				if (xelement.HasAttribute("daysAliveChangeWhenKilled"))
				{
					GameStageDefinition.DaysAliveChangeWhenKilled = long.Parse(xelement.GetAttribute("daysAliveChangeWhenKilled"));
				}
				if (xelement.HasAttribute("diminishingReturns"))
				{
					GameStageDefinition.DiminishingReturns = StringParsers.ParseFloat(xelement.GetAttribute("diminishingReturns"), 0, -1, NumberStyles.Any);
				}
				if (xelement.HasAttribute("lootBonusEvery"))
				{
					GameStageDefinition.LootBonusEvery = int.Parse(xelement.GetAttribute("lootBonusEvery"));
				}
				if (xelement.HasAttribute("lootBonusMaxCount"))
				{
					GameStageDefinition.LootBonusMaxCount = int.Parse(xelement.GetAttribute("lootBonusMaxCount"));
				}
				if (xelement.HasAttribute("lootBonusScale"))
				{
					GameStageDefinition.LootBonusScale = StringParsers.ParseFloat(xelement.GetAttribute("lootBonusScale"), 0, -1, NumberStyles.Any);
				}
				string text;
				if ((text = xelement.GetAttribute("lootWanderingBonusEvery")).Length > 0)
				{
					GameStageDefinition.LootWanderingBonusEvery = int.Parse(text);
				}
				if ((text = xelement.GetAttribute("lootWanderingBonusScale")).Length > 0)
				{
					GameStageDefinition.LootWanderingBonusScale = StringParsers.ParseFloat(text, 0, -1, NumberStyles.Any);
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			GameStagesFromXml.Group group2 = list[i];
			string text2 = group2.spawnerName;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "SleeperGSList";
			}
			GameStageDefinition gameStageDefinition;
			if (!GameStageDefinition.TryGetGameStage(text2, out gameStageDefinition))
			{
				throw new XmlLoadException("gamestages.xml", group2.element, string.Concat(new string[] { "Group '", group2.name, "': Spawner '", text2, "' not found!" }));
			}
			GameStageGroup gameStageGroup = new GameStageGroup(gameStageDefinition);
			GameStageGroup.AddGameStageGroup(group2.name, gameStageGroup);
		}
		yield break;
	}

	// Token: 0x06006615 RID: 26133 RVA: 0x00283330 File Offset: 0x00281530
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameStagesFromXml.Group ParseGameStageGroup(XElement root)
	{
		string attribute = root.GetAttribute("name");
		if (attribute.Length == 0)
		{
			throw new XmlLoadException("gamestages.xml", root, "<group> missing name!");
		}
		string attribute2 = root.GetAttribute("spawner");
		return new GameStagesFromXml.Group(attribute, attribute2, root);
	}

	// Token: 0x06006616 RID: 26134 RVA: 0x00283380 File Offset: 0x00281580
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseGameStageDef(XElement root)
	{
		string attribute = root.GetAttribute("name");
		if (attribute.Length == 0)
		{
			throw new XmlLoadException("gamestages.xml", root, "<spawner> missing name!");
		}
		GameStageDefinition gameStageDefinition = new GameStageDefinition(attribute);
		foreach (XElement xelement in root.Elements("gamestage"))
		{
			GameStagesFromXml.ParseStage(gameStageDefinition, xelement);
		}
		GameStageDefinition.AddGameStage(gameStageDefinition);
	}

	// Token: 0x06006617 RID: 26135 RVA: 0x0028340C File Offset: 0x0028160C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseStage(GameStageDefinition gsd, XElement root)
	{
		string attribute = root.GetAttribute("stage");
		if (attribute.Length == 0)
		{
			throw new XmlLoadException("gamestages.xml", root, "GameStage " + gsd.name + " sub element is missing stage!");
		}
		GameStageDefinition.Stage stage = new GameStageDefinition.Stage(int.Parse(attribute));
		foreach (XElement xelement in root.Elements("spawn"))
		{
			GameStagesFromXml.ParseSpawn(gsd, stage, xelement);
		}
		if (stage.Count > 0)
		{
			gsd.AddStage(stage);
		}
	}

	// Token: 0x06006618 RID: 26136 RVA: 0x002834B8 File Offset: 0x002816B8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseSpawn(GameStageDefinition gsd, GameStageDefinition.Stage stage, XElement root)
	{
		string attribute = root.GetAttribute("group");
		if (attribute.Length == 0)
		{
			throw new XmlLoadException("gamestages.xml", root, "<spawn> is missing group!");
		}
		if (!EntityGroups.list.ContainsKey(attribute))
		{
			throw new XmlLoadException("gamestages.xml", root, string.Format("Spawner '{0}', gamestage {1}: EntityGroup '{2}' unknown!", gsd.name, stage.stageNum, attribute));
		}
		int num = 1;
		root.ParseAttribute("num", ref num);
		int num2 = 1;
		root.ParseAttribute("maxAlive", ref num2);
		int num3 = 2;
		root.ParseAttribute("interval", ref num3);
		int num4 = 0;
		root.ParseAttribute("duration", ref num4);
		GameStageDefinition.SpawnGroup spawnGroup = new GameStageDefinition.SpawnGroup(attribute, num, num2, num3, num4);
		stage.AddSpawnGroup(spawnGroup);
	}

	// Token: 0x04004F5E RID: 20318
	[PublicizedFrom(EAccessModifier.Private)]
	public const string XMLName = "gamestages.xml";

	// Token: 0x02000D20 RID: 3360
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Group
	{
		// Token: 0x0600661A RID: 26138 RVA: 0x0028358E File Offset: 0x0028178E
		public Group(string _name, string _spawnerName, XElement _element)
		{
			this.name = _name;
			this.spawnerName = _spawnerName;
			this.element = _element;
		}

		// Token: 0x04004F5F RID: 20319
		public readonly string name;

		// Token: 0x04004F60 RID: 20320
		public readonly string spawnerName;

		// Token: 0x04004F61 RID: 20321
		public readonly XElement element;
	}
}
