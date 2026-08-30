using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Challenges;

// Token: 0x02000CEC RID: 3308
public class ChallengesFromXml
{
	// Token: 0x06006521 RID: 25889 RVA: 0x002797E4 File Offset: 0x002779E4
	public static IEnumerator CreateChallenges(XmlFile xmlFile)
	{
		ChallengeClass.s_Challenges.Clear();
		ChallengeGroup.s_ChallengeGroups.Clear();
		ChallengeCategory.s_ChallengeCategories.Clear();
		XElement root = xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <challenges> found!");
		}
		ChallengesFromXml.ParseNode(root);
		ChallengesFromXml.LastGroupChallenge.Clear();
		ChallengeClass.InitChallenges();
		yield break;
	}

	// Token: 0x06006522 RID: 25890 RVA: 0x002797F4 File Offset: 0x002779F4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseNode(XElement root)
	{
		if (root.HasAttribute("default_reward"))
		{
			ChallengesFromXml.DefaultRewardEvent = root.GetAttribute("default_reward");
		}
		if (root.HasAttribute("default_reward_text_key"))
		{
			ChallengesFromXml.DefaultRewardText = Localization.Get(root.GetAttribute("default_reward_text_key"), false, null);
		}
		else if (root.HasAttribute("default_reward_text"))
		{
			ChallengesFromXml.DefaultRewardText = root.GetAttribute("default_reward_text");
		}
		foreach (XElement xelement in root.Elements())
		{
			if (xelement.Name == "challenge")
			{
				ChallengesFromXml.ParseChallenge(xelement);
			}
			else if (xelement.Name == "challenge_group")
			{
				ChallengesFromXml.ParseChallengeGroup(xelement);
			}
			else
			{
				if (!(xelement.Name == "challenge_category"))
				{
					string text = "Unrecognized xml element ";
					XName name = xelement.Name;
					throw new Exception(text + ((name != null) ? name.ToString() : null));
				}
				ChallengesFromXml.ParseChallengeCategory(xelement);
			}
		}
	}

	// Token: 0x06006523 RID: 25891 RVA: 0x00279940 File Offset: 0x00277B40
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseChallengeGroup(XElement e)
	{
		DynamicProperties dynamicProperties = null;
		if (!e.HasAttribute("name"))
		{
			throw new Exception("challenge group must have an name attribute");
		}
		string attribute = e.GetAttribute("name");
		ChallengeGroup challengeGroup = ChallengeGroup.NewClass(attribute);
		if (challengeGroup == null)
		{
			throw new Exception("Challenge group with an id of '" + attribute + "' already exists!");
		}
		challengeGroup.ParseElement(e);
		foreach (XElement xelement in e.Elements())
		{
			if (xelement.Name == "property")
			{
				if (dynamicProperties == null)
				{
					dynamicProperties = new DynamicProperties();
				}
				dynamicProperties.Add(xelement, false);
			}
			else if (xelement.Name == "challenge_count")
			{
				if (!xelement.HasAttribute("tags"))
				{
					throw new Exception("Challenge count for group '" + attribute + "' does not contain tags!");
				}
				if (!xelement.HasAttribute("count"))
				{
					throw new Exception("Challenge count for group '" + attribute + "' does not contain count!");
				}
				challengeGroup.AddChallengeCount(xelement.GetAttribute("tags"), StringParsers.ParseSInt32(xelement.GetAttribute("count"), 0, -1, NumberStyles.Integer));
			}
		}
		challengeGroup.Effects = MinEffectController.ParseXml(e, null, MinEffectController.SourceParentType.ChallengeGroup, challengeGroup.Name);
	}

	// Token: 0x06006524 RID: 25892 RVA: 0x00279AC4 File Offset: 0x00277CC4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseChallengeCategory(XElement e)
	{
		if (!e.HasAttribute("name"))
		{
			throw new Exception("challenge category must have an name attribute");
		}
		string attribute = e.GetAttribute("name");
		if (ChallengeCategory.s_ChallengeCategories.ContainsKey(attribute))
		{
			throw new Exception("Challenge group with an id of '" + attribute + "' already exists!");
		}
		ChallengeCategory challengeCategory = new ChallengeCategory(attribute);
		challengeCategory.ParseElement(e);
		ChallengeCategory.s_ChallengeCategories.Add(attribute, challengeCategory);
	}

	// Token: 0x06006525 RID: 25893 RVA: 0x00279B3C File Offset: 0x00277D3C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseChallenge(XElement e)
	{
		DynamicProperties dynamicProperties = null;
		if (!e.HasAttribute("name"))
		{
			throw new Exception("challenge must have an name attribute");
		}
		string attribute = e.GetAttribute("name");
		ChallengeClass challengeClass = ChallengeClass.NewClass(attribute);
		if (challengeClass == null)
		{
			throw new Exception("Challenge with an id of '" + attribute + "' already exists!");
		}
		challengeClass.ParseElement(e);
		ChallengeGroup challengeGroup = challengeClass.ChallengeGroup;
		if (challengeGroup.LinkChallenges)
		{
			if (ChallengesFromXml.LastGroupChallenge.ContainsKey(challengeGroup))
			{
				ChallengesFromXml.LastGroupChallenge[challengeGroup].NextChallenge = challengeClass;
			}
			ChallengesFromXml.LastGroupChallenge[challengeGroup] = challengeClass;
		}
		foreach (XElement xelement in e.Elements())
		{
			if (xelement.Name == "property")
			{
				if (dynamicProperties == null)
				{
					dynamicProperties = new DynamicProperties();
				}
				dynamicProperties.Add(xelement, false);
			}
			else if (xelement.Name == "objective")
			{
				ChallengesFromXml.ParseObjective(challengeClass, xelement);
			}
		}
		challengeClass.Effects = MinEffectController.ParseXml(e, null, MinEffectController.SourceParentType.ChallengeClass, challengeClass.Name);
	}

	// Token: 0x06006526 RID: 25894 RVA: 0x00279C78 File Offset: 0x00277E78
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ParseObjective(ChallengeClass Challenge, XElement e)
	{
		if (!e.HasAttribute("type"))
		{
			throw new Exception("Objective must have a type!");
		}
		BaseChallengeObjective baseChallengeObjective = null;
		string attribute = e.GetAttribute("type");
		try
		{
			baseChallengeObjective = (BaseChallengeObjective)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("Challenges.ChallengeObjective", attribute));
			Challenge.AddObjective(baseChallengeObjective);
		}
		catch (Exception ex)
		{
			throw new Exception("No objective class '" + attribute + " found!", ex);
		}
		if (baseChallengeObjective != null)
		{
			baseChallengeObjective.ParseElement(e);
			baseChallengeObjective.Init();
		}
	}

	// Token: 0x04004E5E RID: 20062
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<ChallengeGroup, ChallengeClass> LastGroupChallenge = new Dictionary<ChallengeGroup, ChallengeClass>();

	// Token: 0x04004E5F RID: 20063
	public static string DefaultRewardEvent;

	// Token: 0x04004E60 RID: 20064
	public static string DefaultRewardText;
}
