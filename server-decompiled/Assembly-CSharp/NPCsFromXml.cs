using System;
using System.Collections;
using System.Xml.Linq;
using MusicUtils.Enums;

// Token: 0x02000D30 RID: 3376
public class NPCsFromXml
{
	// Token: 0x0600666C RID: 26220 RVA: 0x0028866D File Offset: 0x0028686D
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

	// Token: 0x0600666D RID: 26221 RVA: 0x0028867C File Offset: 0x0028687C
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

	// Token: 0x0600666E RID: 26222 RVA: 0x00288718 File Offset: 0x00286918
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

	// Token: 0x0600666F RID: 26223 RVA: 0x0028881C File Offset: 0x00286A1C
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

	// Token: 0x06006670 RID: 26224 RVA: 0x00288A08 File Offset: 0x00286C08
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

	// Token: 0x06006671 RID: 26225 RVA: 0x00288A5C File Offset: 0x00286C5C
	public static float getRelationshipFromTier(string _rel)
	{
		return (float)EnumUtils.Parse<FactionManager.Relationship>(_rel, true);
	}

	// Token: 0x06006672 RID: 26226 RVA: 0x00288A68 File Offset: 0x00286C68
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
