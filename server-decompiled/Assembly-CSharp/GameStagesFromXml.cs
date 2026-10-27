using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x02000D2A RID: 3370
public class GameStagesFromXml
{
	// Token: 0x06006639 RID: 26169 RVA: 0x00284848 File Offset: 0x00282A48
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

	// Token: 0x0600663A RID: 26170 RVA: 0x00284858 File Offset: 0x00282A58
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

	// Token: 0x0600663B RID: 26171 RVA: 0x002848A8 File Offset: 0x00282AA8
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

	// Token: 0x0600663C RID: 26172 RVA: 0x00284934 File Offset: 0x00282B34
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

	// Token: 0x0600663D RID: 26173 RVA: 0x002849E0 File Offset: 0x00282BE0
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

	// Token: 0x04004F83 RID: 20355
	[PublicizedFrom(EAccessModifier.Private)]
	public const string XMLName = "gamestages.xml";

	// Token: 0x02000D2B RID: 3371
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Group
	{
		// Token: 0x0600663F RID: 26175 RVA: 0x00284AB6 File Offset: 0x00282CB6
		public Group(string _name, string _spawnerName, XElement _element)
		{
			this.name = _name;
			this.spawnerName = _spawnerName;
			this.element = _element;
		}

		// Token: 0x04004F84 RID: 20356
		public readonly string name;

		// Token: 0x04004F85 RID: 20357
		public readonly string spawnerName;

		// Token: 0x04004F86 RID: 20358
		public readonly XElement element;
	}
}
