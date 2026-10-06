using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Platform;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200191B RID: 6427
	[Preserve]
	public class ChallengeClass
	{
		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x0600C628 RID: 50728 RVA: 0x0048CF65 File Offset: 0x0048B165
		// (set) Token: 0x0600C629 RID: 50729 RVA: 0x0048CF6D File Offset: 0x0048B16D
		public int OrderIndex
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x0600C62A RID: 50730 RVA: 0x0048CF76 File Offset: 0x0048B176
		public bool HasNavType
		{
			get
			{
				return this.GetNavType() > ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x0600C62B RID: 50731 RVA: 0x0048CF84 File Offset: 0x0048B184
		public ChallengeClass(string name)
		{
			this.Name = name;
			this.OrderIndex = ChallengeClass.nextIndex++;
		}

		// Token: 0x0600C62C RID: 50732 RVA: 0x0048CFE0 File Offset: 0x0048B1E0
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

		// Token: 0x0600C62D RID: 50733 RVA: 0x0048D015 File Offset: 0x0048B215
		public static void Cleanup()
		{
			ChallengeClass.s_Challenges.Clear();
		}

		// Token: 0x0600C62E RID: 50734 RVA: 0x0048D024 File Offset: 0x0048B224
		public static void InitChallenges()
		{
			foreach (string text in ChallengeClass.s_Challenges.Keys)
			{
				ChallengeClass.s_Challenges[text].Init();
			}
		}

		// Token: 0x0600C62F RID: 50735 RVA: 0x0048D084 File Offset: 0x0048B284
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

		// Token: 0x0600C630 RID: 50736 RVA: 0x0048D0D2 File Offset: 0x0048B2D2
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C631 RID: 50737 RVA: 0x0048D0DD File Offset: 0x0048B2DD
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C632 RID: 50738 RVA: 0x0048D100 File Offset: 0x0048B300
		public static ChallengeClass GetChallenge(string name)
		{
			if (ChallengeClass.s_Challenges.ContainsKey(name))
			{
				return ChallengeClass.s_Challenges[name];
			}
			return null;
		}

		// Token: 0x0600C633 RID: 50739 RVA: 0x0048D11C File Offset: 0x0048B31C
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

		// Token: 0x0600C634 RID: 50740 RVA: 0x0048D1B1 File Offset: 0x0048B3B1
		public string GetNextChallengeName()
		{
			if (this.NextChallenge != null)
			{
				return this.NextChallenge.Name;
			}
			return "";
		}

		// Token: 0x0600C635 RID: 50741 RVA: 0x0048D1CC File Offset: 0x0048B3CC
		public void AddObjective(BaseChallengeObjective objective)
		{
			this.ObjectiveList.Add(objective);
			objective.OwnerClass = this;
		}

		// Token: 0x0600C636 RID: 50742 RVA: 0x0048D1E4 File Offset: 0x0048B3E4
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

		// Token: 0x0600C637 RID: 50743 RVA: 0x0048D328 File Offset: 0x0048B528
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

		// Token: 0x0600C638 RID: 50744 RVA: 0x0048D3CC File Offset: 0x0048B5CC
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

		// Token: 0x0600C639 RID: 50745 RVA: 0x0048D41B File Offset: 0x0048B61B
		public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
		{
			if (this.Effects != null)
			{
				this.Effects.FireEvent(_eventType, _params);
			}
		}

		// Token: 0x0600C63A RID: 50746 RVA: 0x0048D434 File Offset: 0x0048B634
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

		// Token: 0x0600C63B RID: 50747 RVA: 0x0048D774 File Offset: 0x0048B974
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

		// Token: 0x0600C63C RID: 50748 RVA: 0x0048D7B4 File Offset: 0x0048B9B4
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

		// Token: 0x04009584 RID: 38276
		public static Dictionary<string, ChallengeClass> s_Challenges = new CaseInsensitiveStringDictionary<ChallengeClass>();

		// Token: 0x04009585 RID: 38277
		public string Name;

		// Token: 0x04009586 RID: 38278
		public string Title;

		// Token: 0x04009587 RID: 38279
		public string Icon;

		// Token: 0x04009588 RID: 38280
		public ChallengeGroup ChallengeGroup;

		// Token: 0x04009589 RID: 38281
		public string ShortDescription;

		// Token: 0x0400958A RID: 38282
		public string Description;

		// Token: 0x0400958B RID: 38283
		public string PreReqChallengeHint;

		// Token: 0x0400958C RID: 38284
		public string ChallengeHint;

		// Token: 0x0400958D RID: 38285
		public string RewardEvent;

		// Token: 0x0400958E RID: 38286
		public string RewardText = "";

		// Token: 0x0400958F RID: 38287
		public string TagName = string.Empty;

		// Token: 0x04009590 RID: 38288
		public FastTags<TagGroup.Global> Tags = FastTags<TagGroup.Global>.none;

		// Token: 0x04009591 RID: 38289
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x04009592 RID: 38290
		public ChallengeClass NextChallenge;

		// Token: 0x04009593 RID: 38291
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextIndex = 0;

		// Token: 0x04009595 RID: 38293
		public bool RedeemAlways;

		// Token: 0x04009596 RID: 38294
		public bool NeedsConstantUIUpdate;

		// Token: 0x04009597 RID: 38295
		public MinEffectController Effects;

		// Token: 0x04009598 RID: 38296
		public List<ChallengeClass.ResourceRequiredTypes> RequiredResourceTypeList;

		// Token: 0x0200191C RID: 6428
		public enum UINavTypes
		{
			// Token: 0x0400959A RID: 38298
			None,
			// Token: 0x0400959B RID: 38299
			Crafting,
			// Token: 0x0400959C RID: 38300
			TwitchActions
		}

		// Token: 0x0200191D RID: 6429
		public enum ResourceRequiredTypes
		{
			// Token: 0x0400959E RID: 38302
			Resource,
			// Token: 0x0400959F RID: 38303
			Ore,
			// Token: 0x040095A0 RID: 38304
			Seed,
			// Token: 0x040095A1 RID: 38305
			Crop
		}
	}
}
