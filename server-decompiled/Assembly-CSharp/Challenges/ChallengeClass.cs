using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Platform;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200194C RID: 6476
	[Preserve]
	public class ChallengeClass
	{
		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x0600C73C RID: 51004 RVA: 0x00490959 File Offset: 0x0048EB59
		// (set) Token: 0x0600C73D RID: 51005 RVA: 0x00490961 File Offset: 0x0048EB61
		public int OrderIndex
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x0600C73E RID: 51006 RVA: 0x0049096A File Offset: 0x0048EB6A
		public bool HasNavType
		{
			get
			{
				return this.GetNavType() > ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x0600C73F RID: 51007 RVA: 0x00490978 File Offset: 0x0048EB78
		public ChallengeClass(string name)
		{
			this.Name = name;
			this.OrderIndex = ChallengeClass.nextIndex++;
		}

		// Token: 0x0600C740 RID: 51008 RVA: 0x004909D4 File Offset: 0x0048EBD4
		public static ChallengeClass NewClass(string id)
		{
			if (ChallengeClass.s_Challenges.ContainsKey(id))
			{
				return null;
			}
			ChallengeClass challengeClass = new ChallengeClass(id.ToLower());
			ChallengeClass.s_Challenges[id] = challengeClass;
			return challengeClass;
		}

		// Token: 0x0600C741 RID: 51009 RVA: 0x00490A09 File Offset: 0x0048EC09
		public static void Cleanup()
		{
			ChallengeClass.s_Challenges.Clear();
		}

		// Token: 0x0600C742 RID: 51010 RVA: 0x00490A18 File Offset: 0x0048EC18
		public static void InitChallenges()
		{
			foreach (string text in ChallengeClass.s_Challenges.Keys)
			{
				ChallengeClass.s_Challenges[text].Init();
			}
		}

		// Token: 0x0600C743 RID: 51011 RVA: 0x00490A78 File Offset: 0x0048EC78
		[PublicizedFrom(EAccessModifier.Private)]
		public void Init()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				this.ObjectiveList[i].BaseInit();
				if (this.ObjectiveList[i].NeedsConstantUIUpdate)
				{
					this.NeedsConstantUIUpdate = true;
				}
			}
		}

		// Token: 0x0600C744 RID: 51012 RVA: 0x00490AC6 File Offset: 0x0048ECC6
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C745 RID: 51013 RVA: 0x00490AD1 File Offset: 0x0048ECD1
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C746 RID: 51014 RVA: 0x00490AF4 File Offset: 0x0048ECF4
		public static ChallengeClass GetChallenge(string name)
		{
			if (ChallengeClass.s_Challenges.ContainsKey(name))
			{
				return ChallengeClass.s_Challenges[name];
			}
			return null;
		}

		// Token: 0x0600C747 RID: 51015 RVA: 0x00490B10 File Offset: 0x0048ED10
		public Challenge CreateChallenge(ChallengeJournal ownerJournal)
		{
			Challenge challenge = new Challenge();
			challenge.ChallengeClass = this;
			challenge.Owner = ownerJournal;
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				BaseChallengeObjective baseChallengeObjective = this.ObjectiveList[i];
				BaseChallengeObjective baseChallengeObjective2 = baseChallengeObjective.Clone();
				baseChallengeObjective2.Owner = challenge;
				baseChallengeObjective2.IsRequirement = baseChallengeObjective.IsRequirement;
				baseChallengeObjective2.MaxCount = baseChallengeObjective.MaxCount;
				baseChallengeObjective2.ShowRequirements = baseChallengeObjective.ShowRequirements;
				baseChallengeObjective2.Biome = baseChallengeObjective.Biome;
				baseChallengeObjective2.HandleOnCreated();
				challenge.ObjectiveList.Add(baseChallengeObjective2);
			}
			return challenge;
		}

		// Token: 0x0600C748 RID: 51016 RVA: 0x00490BA5 File Offset: 0x0048EDA5
		public string GetNextChallengeName()
		{
			if (this.NextChallenge != null)
			{
				return this.NextChallenge.Name;
			}
			return "";
		}

		// Token: 0x0600C749 RID: 51017 RVA: 0x00490BC0 File Offset: 0x0048EDC0
		public void AddObjective(BaseChallengeObjective objective)
		{
			this.ObjectiveList.Add(objective);
			objective.OwnerClass = this;
		}

		// Token: 0x0600C74A RID: 51018 RVA: 0x00490BD8 File Offset: 0x0048EDD8
		public bool ResetObjectives(Challenge challenge)
		{
			int count = challenge.ObjectiveList.Count;
			if (count > this.ObjectiveList.Count)
			{
				challenge.ObjectiveList.RemoveRange(this.ObjectiveList.Count, count - this.ObjectiveList.Count);
			}
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				BaseChallengeObjective baseChallengeObjective = this.ObjectiveList[i];
				if (i < count && baseChallengeObjective.GetType() != challenge.ObjectiveList[i].GetType())
				{
					return false;
				}
				BaseChallengeObjective baseChallengeObjective2 = baseChallengeObjective.Clone();
				baseChallengeObjective2.Owner = challenge;
				baseChallengeObjective2.IsRequirement = baseChallengeObjective.IsRequirement;
				baseChallengeObjective2.MaxCount = baseChallengeObjective.MaxCount;
				baseChallengeObjective2.ShowRequirements = baseChallengeObjective.ShowRequirements;
				baseChallengeObjective2.Biome = baseChallengeObjective.Biome;
				baseChallengeObjective2.HandleOnCreated();
				if (i < count)
				{
					BaseChallengeObjective baseChallengeObjective3 = challenge.ObjectiveList[i];
					baseChallengeObjective2.CopyValues(baseChallengeObjective3, baseChallengeObjective);
					challenge.ObjectiveList[i] = baseChallengeObjective2;
					baseChallengeObjective2.Current = Utils.FastMin(baseChallengeObjective2.Current, baseChallengeObjective2.MaxCount);
				}
				else
				{
					challenge.ObjectiveList.Add(baseChallengeObjective2);
				}
				if (!challenge.IsActive)
				{
					baseChallengeObjective2.Current = baseChallengeObjective2.MaxCount;
					baseChallengeObjective2.Complete = true;
				}
			}
			return true;
		}

		// Token: 0x0600C74B RID: 51019 RVA: 0x00490D1C File Offset: 0x0048EF1C
		public string GetHint(bool isPreReq)
		{
			if (this.ChallengeHint == null)
			{
				return "";
			}
			if (isPreReq)
			{
				if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
				{
					string text = this.PreReqChallengeHint + "_alt";
					if (Localization.Exists(text, false))
					{
						return Localization.Get(text, false, null);
					}
				}
				return Localization.Get(this.PreReqChallengeHint, false, null);
			}
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
			{
				string text2 = this.ChallengeHint + "_alt";
				if (Localization.Exists(text2, false))
				{
					return Localization.Get(text2, false, null);
				}
			}
			return Localization.Get(this.ChallengeHint, false, null);
		}

		// Token: 0x0600C74C RID: 51020 RVA: 0x00490DC0 File Offset: 0x0048EFC0
		public string GetDescription()
		{
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
			{
				string text = this.Description + "_alt";
				if (Localization.Exists(text, false))
				{
					return Localization.Get(text, false, null);
				}
			}
			return Localization.Get(this.Description, false, null);
		}

		// Token: 0x0600C74D RID: 51021 RVA: 0x00490E0F File Offset: 0x0048F00F
		public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
		{
			if (this.Effects != null)
			{
				this.Effects.FireEvent(_eventType, _params);
			}
		}

		// Token: 0x0600C74E RID: 51022 RVA: 0x00490E28 File Offset: 0x0048F028
		public void ParseElement(XElement e)
		{
			if (e.HasAttribute("icon"))
			{
				this.Icon = e.GetAttribute("icon");
			}
			if (e.HasAttribute("title_key"))
			{
				this.Title = Localization.Get(e.GetAttribute("title_key"), false, null);
			}
			else if (e.HasAttribute("title"))
			{
				this.Title = e.GetAttribute("title");
			}
			else
			{
				this.Title = this.Name;
			}
			if (e.HasAttribute("group"))
			{
				ChallengeGroup challengeGroup = ChallengeGroup.s_ChallengeGroups[e.GetAttribute("group")];
				this.ChallengeGroup = challengeGroup;
				challengeGroup.AddChallenge(this);
			}
			if (e.HasAttribute("prerequisite_hint"))
			{
				this.PreReqChallengeHint = e.GetAttribute("prerequisite_hint");
			}
			if (e.HasAttribute("hint"))
			{
				this.ChallengeHint = e.GetAttribute("hint");
			}
			if (e.HasAttribute("short_description_key"))
			{
				this.ShortDescription = Localization.Get(e.GetAttribute("short_description_key"), false, null);
			}
			else if (e.HasAttribute("short_description"))
			{
				this.ShortDescription = e.GetAttribute("short_description");
			}
			if (e.HasAttribute("description_key"))
			{
				this.Description = e.GetAttribute("description_key");
			}
			else if (e.HasAttribute("description"))
			{
				this.Description = e.GetAttribute("description");
			}
			if (e.HasAttribute("reward_event"))
			{
				this.RewardEvent = e.GetAttribute("reward_event");
			}
			else
			{
				this.RewardEvent = ChallengesFromXml.DefaultRewardEvent;
			}
			if (e.HasAttribute("reward_text_key"))
			{
				this.RewardText = Localization.Get(e.GetAttribute("reward_text_key"), false, null);
			}
			else if (e.HasAttribute("reward_text"))
			{
				this.RewardText = e.GetAttribute("reward_text");
			}
			else
			{
				this.RewardText = ChallengesFromXml.DefaultRewardText;
			}
			if (e.HasAttribute("tags"))
			{
				this.TagName = e.GetAttribute("tags");
				this.Tags = FastTags<TagGroup.Global>.Parse(this.TagName);
			}
			if (e.HasAttribute("redeem_always"))
			{
				this.RedeemAlways = StringParsers.ParseBool(e.GetAttribute("redeem_always"), 0, -1, true);
			}
			if (e.HasAttribute("harvest_required"))
			{
				string[] array = e.GetAttribute("harvest_required").Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					ChallengeClass.ResourceRequiredTypes resourceRequiredTypes = ChallengeClass.ResourceRequiredTypes.Resource;
					if (Enum.TryParse<ChallengeClass.ResourceRequiredTypes>(array[i], true, out resourceRequiredTypes))
					{
						if (this.RequiredResourceTypeList == null)
						{
							this.RequiredResourceTypeList = new List<ChallengeClass.ResourceRequiredTypes>();
						}
						this.RequiredResourceTypeList.Add(resourceRequiredTypes);
					}
				}
			}
		}

		// Token: 0x0600C74F RID: 51023 RVA: 0x00491168 File Offset: 0x0048F368
		public ChallengeClass.UINavTypes GetNavType()
		{
			for (int i = 0; i < this.ObjectiveList.Count; i++)
			{
				BaseChallengeObjective baseChallengeObjective = this.ObjectiveList[i];
				if (baseChallengeObjective.NavType != ChallengeClass.UINavTypes.None)
				{
					return baseChallengeObjective.NavType;
				}
			}
			return ChallengeClass.UINavTypes.None;
		}

		// Token: 0x0600C750 RID: 51024 RVA: 0x004911A8 File Offset: 0x0048F3A8
		public bool HandleResourceRequirement()
		{
			if (ItemActionAttack.BlockDamagePercent == 0f)
			{
				return false;
			}
			if (this.RequiredResourceTypeList == null)
			{
				return true;
			}
			for (int i = 0; i < this.RequiredResourceTypeList.Count; i++)
			{
				switch (this.RequiredResourceTypeList[i])
				{
				case ChallengeClass.ResourceRequiredTypes.Resource:
					if (XUiM_Recipes.HarvestingOutputModifier == 0f)
					{
						return false;
					}
					break;
				case ChallengeClass.ResourceRequiredTypes.Ore:
					if (XUiM_Recipes.MiningOutputModifier == 0f)
					{
						return false;
					}
					break;
				case ChallengeClass.ResourceRequiredTypes.Seed:
					if (XUiM_Recipes.SeedDropOutputModifier == 0f)
					{
						return false;
					}
					break;
				case ChallengeClass.ResourceRequiredTypes.Crop:
					if (XUiM_Recipes.CropOutputModifier == 0f)
					{
						return false;
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x0400966D RID: 38509
		public static Dictionary<string, ChallengeClass> s_Challenges = new CaseInsensitiveStringDictionary<ChallengeClass>();

		// Token: 0x0400966E RID: 38510
		public string Name;

		// Token: 0x0400966F RID: 38511
		public string Title;

		// Token: 0x04009670 RID: 38512
		public string Icon;

		// Token: 0x04009671 RID: 38513
		public ChallengeGroup ChallengeGroup;

		// Token: 0x04009672 RID: 38514
		public string ShortDescription;

		// Token: 0x04009673 RID: 38515
		public string Description;

		// Token: 0x04009674 RID: 38516
		public string PreReqChallengeHint;

		// Token: 0x04009675 RID: 38517
		public string ChallengeHint;

		// Token: 0x04009676 RID: 38518
		public string RewardEvent;

		// Token: 0x04009677 RID: 38519
		public string RewardText = "";

		// Token: 0x04009678 RID: 38520
		public string TagName = string.Empty;

		// Token: 0x04009679 RID: 38521
		public FastTags<TagGroup.Global> Tags = FastTags<TagGroup.Global>.none;

		// Token: 0x0400967A RID: 38522
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x0400967B RID: 38523
		public ChallengeClass NextChallenge;

		// Token: 0x0400967C RID: 38524
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextIndex = 0;

		// Token: 0x0400967E RID: 38526
		public bool RedeemAlways;

		// Token: 0x0400967F RID: 38527
		public bool NeedsConstantUIUpdate;

		// Token: 0x04009680 RID: 38528
		public MinEffectController Effects;

		// Token: 0x04009681 RID: 38529
		public List<ChallengeClass.ResourceRequiredTypes> RequiredResourceTypeList;

		// Token: 0x0200194D RID: 6477
		public enum UINavTypes
		{
			// Token: 0x04009683 RID: 38531
			None,
			// Token: 0x04009684 RID: 38532
			Crafting,
			// Token: 0x04009685 RID: 38533
			TwitchActions
		}

		// Token: 0x0200194E RID: 6478
		public enum ResourceRequiredTypes
		{
			// Token: 0x04009687 RID: 38535
			Resource,
			// Token: 0x04009688 RID: 38536
			Ore,
			// Token: 0x04009689 RID: 38537
			Seed,
			// Token: 0x0400968A RID: 38538
			Crop
		}
	}
}
