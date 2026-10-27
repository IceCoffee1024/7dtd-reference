using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;

// Token: 0x02000D3B RID: 3387
public class NPCsFromXml
{
	// Token: 0x06006691 RID: 26257 RVA: 0x00289B95 File Offset: 0x00287D95
	public static IEnumerator LoadNPCInfo(XmlFile xmlFile)
	{
		XElement root = xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <npcs> found!");
		}
		NPCsFromXml.ParseNode(root);
		yield break;
	}

	// Token: 0x06006692 RID: 26258 RVA: 0x00289BA4 File Offset: 0x00287DA4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseNode(XElement root)
	{
		foreach (XElement xelement in root.Elements())
		{
			if (xelement.Name == "npc_info")
			{
				NPCsFromXml.ParseNPCInfo(xelement);
			}
			else if (!(xelement.Name == "voice_sets") && xelement.Name == "factions")
			{
				NPCsFromXml.ParseFactionInfo(xelement);
			}
		}
	}

	// Token: 0x06006693 RID: 26259 RVA: 0x00289C40 File Offset: 0x00287E40
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseFactionInfo(XElement root)
	{
		FactionManager.Init();
		foreach (XElement xelement in root.Elements("faction"))
		{
			string text = "";
			if (xelement.HasAttribute("name"))
			{
				text = xelement.GetAttribute("name");
			}
			FactionManager.Instance.CreateFaction(text, false, "");
		}
		foreach (XElement xelement2 in root.Elements("faction"))
		{
			string text2 = "";
			if (xelement2.HasAttribute("name"))
			{
				text2 = xelement2.GetAttribute("name");
			}
			NPCsFromXml.ParseFactionStandings(text2, xelement2);
		}
	}

	// Token: 0x06006694 RID: 26260 RVA: 0x00289D44 File Offset: 0x00287F44
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseFactionStandings(string factionName, XElement root)
	{
		foreach (XElement xelement in root.Elements("relationship"))
		{
			Faction factionByName = FactionManager.Instance.GetFactionByName(factionName);
			string text = "";
			string text2 = "";
			if (xelement.HasAttribute("name"))
			{
				text = xelement.GetAttribute("name");
			}
			if (xelement.HasAttribute("value"))
			{
				text2 = xelement.GetAttribute("value");
			}
			if (text == "*")
			{
				for (byte b = 0; b < 255; b += 1)
				{
					if (factionByName.ID != b)
					{
						FactionManager.Relationship relationshipTier = NPCsFromXml.getRelationshipTier(factionByName.GetRelationship(b));
						FactionManager.Relationship relationshipTier2 = NPCsFromXml.getRelationshipTier(NPCsFromXml.getRelationshipFromTier(text2));
						if (relationshipTier != relationshipTier2)
						{
							factionByName.SetRelationship(b, NPCsFromXml.getRelationshipFromTier(text2));
						}
					}
				}
			}
		}
		foreach (XElement xelement2 in root.Elements("relationship"))
		{
			Faction factionByName2 = FactionManager.Instance.GetFactionByName(factionName);
			string text3 = "";
			string text4 = "";
			if (xelement2.HasAttribute("name"))
			{
				text3 = xelement2.GetAttribute("name");
			}
			if (!(text3 == "*"))
			{
				if (xelement2.HasAttribute("value"))
				{
					text4 = xelement2.GetAttribute("value");
				}
				factionByName2.SetRelationship(FactionManager.Instance.GetFactionByName(text3).ID, NPCsFromXml.getRelationshipFromTier(text4));
			}
		}
	}

	// Token: 0x06006695 RID: 26261 RVA: 0x00289F30 File Offset: 0x00288130
	public static FactionManager.Relationship getRelationshipTier(float rel)
	{
		if (rel < 200f)
		{
			return FactionManager.Relationship.Hate;
		}
		if (rel < 400f)
		{
			return FactionManager.Relationship.Dislike;
		}
		if (rel < 600f)
		{
			return FactionManager.Relationship.Neutral;
		}
		if (rel < 800f)
		{
			return FactionManager.Relationship.Like;
		}
		if (rel < 1000f)
		{
			return FactionManager.Relationship.Love;
		}
		return FactionManager.Relationship.Leader;
	}

	// Token: 0x06006696 RID: 26262 RVA: 0x00289F84 File Offset: 0x00288184
	public static float getRelationshipFromTier(string _rel)
	{
		return (float)EnumUtils.Parse<FactionManager.Relationship>(_rel, true);
	}

	// Token: 0x06006697 RID: 26263 RVA: 0x00289F90 File Offset: 0x00288190
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseNPCInfo(XElement e)
	{
		if (!e.HasAttribute("id"))
		{
			throw new Exception("npc must have an id attribute");
		}
		string attribute = e.GetAttribute("id");
		if (NPCInfo.npcInfoList.ContainsKey(attribute))
		{
			throw new Exception("Duplicate npc entry with id " + attribute);
		}
		NPCInfo npcinfo = new NPCInfo();
		npcinfo.Id = attribute;
		NPCInfo.npcInfoList.Add(attribute, npcinfo);
		if (e.HasAttribute("faction"))
		{
			npcinfo.Faction = e.GetAttribute("faction");
		}
		if (e.HasAttribute("portrait"))
		{
			npcinfo.Portrait = e.GetAttribute("portrait");
		}
		if (e.HasAttribute("trader_id"))
		{
			int num = 0;
			if (int.TryParse(e.GetAttribute("trader_id"), out num))
			{
				npcinfo.TraderID = num;
			}
		}
		if (e.HasAttribute("dialog_id"))
		{
			npcinfo.DialogID = e.GetAttribute("dialog_id");
		}
		if (e.HasAttribute("quest_list"))
		{
			npcinfo.QuestListName = e.GetAttribute("quest_list");
		}
		if (e.HasAttribute("voice_set"))
		{
			npcinfo.VoiceSet = e.GetAttribute("voice_set");
		}
		if (e.HasAttribute("stance"))
		{
			npcinfo.CurrentStance = EnumUtils.Parse<NPCInfo.StanceTypes>(e.GetAttribute("stance"), true);
		}
		if (e.HasAttribute("quest_faction"))
		{
			npcinfo.QuestFaction = Convert.ToByte(e.GetAttribute("quest_faction"));
		}
		if (e.HasAttribute("localization_id"))
		{
			npcinfo.LocalizationID = e.GetAttribute("localization_id");
		}
		if (e.HasAttribute("dms_section_type"))
		{
			npcinfo.DmsSectionType = EnumUtils.Parse<SectionType>(e.GetAttribute("dms_section_type"), true);
		}
	}
}
