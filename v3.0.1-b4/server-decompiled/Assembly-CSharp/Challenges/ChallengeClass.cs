using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Platform;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018C9 RID: 6345
	[Preserve]
	public class ChallengeClass
	{
		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x0600C349 RID: 49993 RVA: 0x00482735 File Offset: 0x00480935
		// (set) Token: 0x0600C34A RID: 49994 RVA: 0x0048273D File Offset: 0x0048093D
		public int OrderIndex
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x0600C34B RID: 49995 RVA: 0x00482746 File Offset: 0x00480946
		public bool HasNavType
		{
			get
			{
				return this.GetNavType() > ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x0600C34C RID: 49996 RVA: 0x00482754 File Offset: 0x00480954
		public ChallengeClass(string name)
		{
			this.Name = name;
			this.OrderIndex = ChallengeClass.nextIndex++;
		}

		// Token: 0x0600C34D RID: 49997 RVA: 0x004827B0 File Offset: 0x004809B0
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

		// Token: 0x0600C34E RID: 49998 RVA: 0x004827E5 File Offset: 0x004809E5
		public static void Cleanup()
		{
			ChallengeClass.s_Challenges.Clear();
		}

		// Token: 0x0600C34F RID: 49999 RVA: 0x004827F4 File Offset: 0x004809F4
		public static void InitChallenges()
		{
			foreach (string text in ChallengeClass.s_Challenges.Keys)
			{
				ChallengeClass.s_Challenges[text].Init();
			}
		}

		// Token: 0x0600C350 RID: 50000 RVA: 0x00482854 File Offset: 0x00480A54
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

		// Token: 0x0600C351 RID: 50001 RVA: 0x004828A2 File Offset: 0x00480AA2
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C352 RID: 50002 RVA: 0x004828AD File Offset: 0x00480AAD
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C353 RID: 50003 RVA: 0x004828D0 File Offset: 0x00480AD0
		public static ChallengeClass GetChallenge(string name)
		{
			if (ChallengeClass.s_Challenges.ContainsKey(name))
			{
				return ChallengeClass.s_Challenges[name];
			}
			return null;
		}

		// Token: 0x0600C354 RID: 50004 RVA: 0x004828EC File Offset: 0x00480AEC
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

		// Token: 0x0600C355 RID: 50005 RVA: 0x00482981 File Offset: 0x00480B81
		public string GetNextChallengeName()
		{
			if (this.NextChallenge != null)
			{
				return this.NextChallenge.Name;
			}
			return "";
		}

		// Token: 0x0600C356 RID: 50006 RVA: 0x0048299C File Offset: 0x00480B9C
		public void AddObjective(BaseChallengeObjective objective)
		{
			this.ObjectiveList.Add(objective);
			objective.OwnerClass = this;
		}

		// Token: 0x0600C357 RID: 50007 RVA: 0x004829B4 File Offset: 0x00480BB4
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

		// Token: 0x0600C358 RID: 50008 RVA: 0x00482AF8 File Offset: 0x00480CF8
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

		// Token: 0x0600C359 RID: 50009 RVA: 0x00482B9C File Offset: 0x00480D9C
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

		// Token: 0x0600C35A RID: 50010 RVA: 0x00482BEB File Offset: 0x00480DEB
		public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
		{
			if (this.Effects != null)
			{
				this.Effects.FireEvent(_eventType, _params);
			}
		}

		// Token: 0x0600C35B RID: 50011 RVA: 0x00482C04 File Offset: 0x00480E04
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

		// Token: 0x0600C35C RID: 50012 RVA: 0x00482F44 File Offset: 0x00481144
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

		// Token: 0x0600C35D RID: 50013 RVA: 0x00482F84 File Offset: 0x00481184
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

		// Token: 0x04009395 RID: 37781
		public static Dictionary<string, ChallengeClass> s_Challenges = new CaseInsensitiveStringDictionary<ChallengeClass>();

		// Token: 0x04009396 RID: 37782
		public string Name;

		// Token: 0x04009397 RID: 37783
		public string Title;

		// Token: 0x04009398 RID: 37784
		public string Icon;

		// Token: 0x04009399 RID: 37785
		public ChallengeGroup ChallengeGroup;

		// Token: 0x0400939A RID: 37786
		public string ShortDescription;

		// Token: 0x0400939B RID: 37787
		public string Description;

		// Token: 0x0400939C RID: 37788
		public string PreReqChallengeHint;

		// Token: 0x0400939D RID: 37789
		public string ChallengeHint;

		// Token: 0x0400939E RID: 37790
		public string RewardEvent;

		// Token: 0x0400939F RID: 37791
		public string RewardText = "";

		// Token: 0x040093A0 RID: 37792
		public string TagName = string.Empty;

		// Token: 0x040093A1 RID: 37793
		public FastTags<TagGroup.Global> Tags = FastTags<TagGroup.Global>.none;

		// Token: 0x040093A2 RID: 37794
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x040093A3 RID: 37795
		public ChallengeClass NextChallenge;

		// Token: 0x040093A4 RID: 37796
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextIndex = 0;

		// Token: 0x040093A6 RID: 37798
		public bool RedeemAlways;

		// Token: 0x040093A7 RID: 37799
		public bool NeedsConstantUIUpdate;

		// Token: 0x040093A8 RID: 37800
		public MinEffectController Effects;

		// Token: 0x040093A9 RID: 37801
		public List<ChallengeClass.ResourceRequiredTypes> RequiredResourceTypeList;

		// Token: 0x020018CA RID: 6346
		public enum UINavTypes
		{
			// Token: 0x040093AB RID: 37803
			None,
			// Token: 0x040093AC RID: 37804
			Crafting,
			// Token: 0x040093AD RID: 37805
			TwitchActions
		}

		// Token: 0x020018CB RID: 6347
		public enum ResourceRequiredTypes
		{
			// Token: 0x040093AF RID: 37807
			Resource,
			// Token: 0x040093B0 RID: 37808
			Ore,
			// Token: 0x040093B1 RID: 37809
			Seed,
			// Token: 0x040093B2 RID: 37810
			Crop
		}
	}
}
