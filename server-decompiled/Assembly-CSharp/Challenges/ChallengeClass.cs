using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Platform;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018EF RID: 6383
	[Preserve]
	public class ChallengeClass
	{
		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x0600C4D0 RID: 50384 RVA: 0x00488A75 File Offset: 0x00486C75
		// (set) Token: 0x0600C4D1 RID: 50385 RVA: 0x00488A7D File Offset: 0x00486C7D
		public int OrderIndex
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x0600C4D2 RID: 50386 RVA: 0x00488A86 File Offset: 0x00486C86
		public bool HasNavType
		{
			get
			{
				return this.GetNavType() > ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x0600C4D3 RID: 50387 RVA: 0x00488A94 File Offset: 0x00486C94
		public ChallengeClass(string name)
		{
			this.Name = name;
			this.OrderIndex = ChallengeClass.nextIndex++;
		}

		// Token: 0x0600C4D4 RID: 50388 RVA: 0x00488AF0 File Offset: 0x00486CF0
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

		// Token: 0x0600C4D5 RID: 50389 RVA: 0x00488B25 File Offset: 0x00486D25
		public static void Cleanup()
		{
			ChallengeClass.s_Challenges.Clear();
		}

		// Token: 0x0600C4D6 RID: 50390 RVA: 0x00488B34 File Offset: 0x00486D34
		public static void InitChallenges()
		{
			foreach (string text in ChallengeClass.s_Challenges.Keys)
			{
				ChallengeClass.s_Challenges[text].Init();
			}
		}

		// Token: 0x0600C4D7 RID: 50391 RVA: 0x00488B94 File Offset: 0x00486D94
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

		// Token: 0x0600C4D8 RID: 50392 RVA: 0x00488BE2 File Offset: 0x00486DE2
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C4D9 RID: 50393 RVA: 0x00488BED File Offset: 0x00486DED
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C4DA RID: 50394 RVA: 0x00488C10 File Offset: 0x00486E10
		public static ChallengeClass GetChallenge(string name)
		{
			if (ChallengeClass.s_Challenges.ContainsKey(name))
			{
				return ChallengeClass.s_Challenges[name];
			}
			return null;
		}

		// Token: 0x0600C4DB RID: 50395 RVA: 0x00488C2C File Offset: 0x00486E2C
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

		// Token: 0x0600C4DC RID: 50396 RVA: 0x00488CC1 File Offset: 0x00486EC1
		public string GetNextChallengeName()
		{
			if (this.NextChallenge != null)
			{
				return this.NextChallenge.Name;
			}
			return "";
		}

		// Token: 0x0600C4DD RID: 50397 RVA: 0x00488CDC File Offset: 0x00486EDC
		public void AddObjective(BaseChallengeObjective objective)
		{
			this.ObjectiveList.Add(objective);
			objective.OwnerClass = this;
		}

		// Token: 0x0600C4DE RID: 50398 RVA: 0x00488CF4 File Offset: 0x00486EF4
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

		// Token: 0x0600C4DF RID: 50399 RVA: 0x00488E38 File Offset: 0x00487038
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

		// Token: 0x0600C4E0 RID: 50400 RVA: 0x00488EDC File Offset: 0x004870DC
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

		// Token: 0x0600C4E1 RID: 50401 RVA: 0x00488F2B File Offset: 0x0048712B
		public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
		{
			if (this.Effects != null)
			{
				this.Effects.FireEvent(_eventType, _params);
			}
		}

		// Token: 0x0600C4E2 RID: 50402 RVA: 0x00488F44 File Offset: 0x00487144
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

		// Token: 0x0600C4E3 RID: 50403 RVA: 0x00489284 File Offset: 0x00487484
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

		// Token: 0x0600C4E4 RID: 50404 RVA: 0x004892C4 File Offset: 0x004874C4
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

		// Token: 0x040094BE RID: 38078
		public static Dictionary<string, ChallengeClass> s_Challenges = new CaseInsensitiveStringDictionary<ChallengeClass>();

		// Token: 0x040094BF RID: 38079
		public string Name;

		// Token: 0x040094C0 RID: 38080
		public string Title;

		// Token: 0x040094C1 RID: 38081
		public string Icon;

		// Token: 0x040094C2 RID: 38082
		public ChallengeGroup ChallengeGroup;

		// Token: 0x040094C3 RID: 38083
		public string ShortDescription;

		// Token: 0x040094C4 RID: 38084
		public string Description;

		// Token: 0x040094C5 RID: 38085
		public string PreReqChallengeHint;

		// Token: 0x040094C6 RID: 38086
		public string ChallengeHint;

		// Token: 0x040094C7 RID: 38087
		public string RewardEvent;

		// Token: 0x040094C8 RID: 38088
		public string RewardText = "";

		// Token: 0x040094C9 RID: 38089
		public string TagName = string.Empty;

		// Token: 0x040094CA RID: 38090
		public FastTags<TagGroup.Global> Tags = FastTags<TagGroup.Global>.none;

		// Token: 0x040094CB RID: 38091
		public List<BaseChallengeObjective> ObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x040094CC RID: 38092
		public ChallengeClass NextChallenge;

		// Token: 0x040094CD RID: 38093
		[PublicizedFrom(EAccessModifier.Private)]
		public static int nextIndex = 0;

		// Token: 0x040094CF RID: 38095
		public bool RedeemAlways;

		// Token: 0x040094D0 RID: 38096
		public bool NeedsConstantUIUpdate;

		// Token: 0x040094D1 RID: 38097
		public MinEffectController Effects;

		// Token: 0x040094D2 RID: 38098
		public List<ChallengeClass.ResourceRequiredTypes> RequiredResourceTypeList;

		// Token: 0x020018F0 RID: 6384
		public enum UINavTypes
		{
			// Token: 0x040094D4 RID: 38100
			None,
			// Token: 0x040094D5 RID: 38101
			Crafting,
			// Token: 0x040094D6 RID: 38102
			TwitchActions
		}

		// Token: 0x020018F1 RID: 6385
		public enum ResourceRequiredTypes
		{
			// Token: 0x040094D8 RID: 38104
			Resource,
			// Token: 0x040094D9 RID: 38105
			Ore,
			// Token: 0x040094DA RID: 38106
			Seed,
			// Token: 0x040094DB RID: 38107
			Crop
		}
	}
}
