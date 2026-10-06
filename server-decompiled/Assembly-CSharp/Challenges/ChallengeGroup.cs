using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UniLinq;

namespace Challenges
{
	// Token: 0x0200191E RID: 6430
	public class ChallengeGroup
	{
		// Token: 0x0600C63E RID: 50750 RVA: 0x0048D85F File Offset: 0x0048BA5F
		public ChallengeGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600C63F RID: 50751 RVA: 0x0048D894 File Offset: 0x0048BA94
		public static ChallengeGroup NewClass(string id)
		{
			if (ChallengeGroup.s_ChallengeGroups.ContainsKey(id))
			{
				return null;
			}
			ChallengeGroup challengeGroup = new ChallengeGroup(id.ToLower());
			ChallengeGroup.s_ChallengeGroups[id] = challengeGroup;
			return challengeGroup;
		}

		// Token: 0x0600C640 RID: 50752 RVA: 0x0048D8C9 File Offset: 0x0048BAC9
		public static ChallengeGroup GetGroup(string id)
		{
			if (!ChallengeGroup.s_ChallengeGroups.ContainsKey(id))
			{
				return null;
			}
			return ChallengeGroup.s_ChallengeGroups[id];
		}

		// Token: 0x0600C641 RID: 50753 RVA: 0x0048D8E5 File Offset: 0x0048BAE5
		public void AddChallengeCount(string tag, int count)
		{
			if (this.ChallengeCounts == null)
			{
				this.ChallengeCounts = new List<ChallengeGroup.ChallengeCount>();
			}
			this.ChallengeCounts.Add(new ChallengeGroup.ChallengeCount
			{
				Tags = FastTags<TagGroup.Global>.Parse(tag),
				Count = count
			});
		}

		// Token: 0x0600C642 RID: 50754 RVA: 0x0048D91D File Offset: 0x0048BB1D
		public void AddChallenge(ChallengeClass challenge)
		{
			this.ChallengeClasses.Add(challenge);
		}

		// Token: 0x0600C643 RID: 50755 RVA: 0x0048D92C File Offset: 0x0048BB2C
		public void ParseElement(XElement e)
		{
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
			if (e.HasAttribute("category"))
			{
				this.Category = e.GetAttribute("category");
			}
			if (e.HasAttribute("reward_event"))
			{
				this.RewardEvent = e.GetAttribute("reward_event");
			}
			if (e.HasAttribute("reward_text_key"))
			{
				this.RewardText = Localization.Get(e.GetAttribute("reward_text_key"), false, null);
			}
			else if (e.HasAttribute("reward_text"))
			{
				this.RewardText = e.GetAttribute("reward_text");
			}
			if (e.HasAttribute("objective_text_key"))
			{
				this.ObjectiveText = Localization.Get(e.GetAttribute("objective_text_key"), false, null);
			}
			else if (e.HasAttribute("objective_text"))
			{
				this.ObjectiveText = e.GetAttribute("objective_text");
			}
			if (e.HasAttribute("active_challenge_count"))
			{
				this.ActiveChallengeCount = StringParsers.ParseSInt32(e.GetAttribute("active_challenge_count"), 0, -1, NumberStyles.Integer);
			}
			if (e.HasAttribute("day_reset"))
			{
				this.DayReset = StringParsers.ParseSInt32(e.GetAttribute("day_reset"), 0, -1, NumberStyles.Integer);
			}
			if (e.HasAttribute("is_random"))
			{
				this.IsRandom = StringParsers.ParseBool(e.GetAttribute("is_random"), 0, -1, true);
			}
			if (e.HasAttribute("link_challenges"))
			{
				this.LinkChallenges = StringParsers.ParseBool(e.GetAttribute("link_challenges"), 0, -1, true);
			}
			if (e.HasAttribute("hidden_by"))
			{
				this.HiddenBy = e.GetAttribute("hidden_by");
			}
			if (e.HasAttribute("is_intro"))
			{
				this.IsIntro = StringParsers.ParseBool(e.GetAttribute("is_intro"), 0, -1, true);
			}
		}

		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x0600C644 RID: 50756 RVA: 0x0048DBBA File Offset: 0x0048BDBA
		public bool IsActive
		{
			get
			{
				return (!this.IsIntro || ChallengeJournal.IntroChallengesEnabled) && ChallengeCategory.s_ChallengeCategories[this.Category].IsActive;
			}
		}

		// Token: 0x0600C645 RID: 50757 RVA: 0x0048DBE4 File Offset: 0x0048BDE4
		public bool IsVisible(EntityPlayer player)
		{
			if (!ChallengeJournal.IntroChallengesEnabled && this.IsIntro)
			{
				return false;
			}
			if (this.HiddenBy == "")
			{
				return true;
			}
			if (!ChallengeCategory.s_ChallengeCategories[this.Category].CanShow(player))
			{
				return false;
			}
			if (ChallengeGroup.s_ChallengeGroups.ContainsKey(this.HiddenBy))
			{
				ChallengeGroup challengeGroup = ChallengeGroup.s_ChallengeGroups[this.HiddenBy];
				if (challengeGroup.IsActive)
				{
					return challengeGroup.IsComplete;
				}
			}
			return true;
		}

		// Token: 0x0600C646 RID: 50758 RVA: 0x0048DC63 File Offset: 0x0048BE63
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C647 RID: 50759 RVA: 0x0048DC6E File Offset: 0x0048BE6E
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null || !this.IsComplete)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C648 RID: 50760 RVA: 0x0048DC9C File Offset: 0x0048BE9C
		public List<ChallengeClass> GetChallengeClassesForCreate()
		{
			List<ChallengeClass> list = new List<ChallengeClass>();
			for (int i = 0; i < this.ChallengeClasses.Count; i++)
			{
				list.Add(this.ChallengeClasses[i]);
			}
			GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
			for (int j = 0; j < list.Count * 2; j++)
			{
				int num = gameRandom.RandomRange(list.Count);
				int num2 = gameRandom.RandomRange(list.Count);
				ChallengeClass challengeClass = list[num];
				list[num] = list[num2];
				list[num2] = challengeClass;
			}
			if (this.ChallengeCounts != null)
			{
				for (int k = 0; k < this.ChallengeCounts.Count; k++)
				{
					ChallengeGroup.ChallengeCount challengeCount = this.ChallengeCounts[k];
					int num3 = challengeCount.Count;
					for (int l = list.Count - 1; l >= 0; l--)
					{
						if (list[l].Tags.Test_AnySet(challengeCount.Tags))
						{
							if (num3 == 0)
							{
								list.RemoveAt(l);
							}
							else
							{
								num3--;
							}
						}
					}
				}
				list = list.OrderBy<ChallengeClass, string>((ChallengeClass c) => c.TagName).ToList<ChallengeClass>();
			}
			return list;
		}

		// Token: 0x040095A2 RID: 38306
		public static Dictionary<string, ChallengeGroup> s_ChallengeGroups = new CaseInsensitiveStringDictionary<ChallengeGroup>();

		// Token: 0x040095A3 RID: 38307
		public string Name;

		// Token: 0x040095A4 RID: 38308
		public string Title;

		// Token: 0x040095A5 RID: 38309
		public bool IsComplete;

		// Token: 0x040095A6 RID: 38310
		public string RewardEvent;

		// Token: 0x040095A7 RID: 38311
		public string RewardText;

		// Token: 0x040095A8 RID: 38312
		public string ObjectiveText;

		// Token: 0x040095A9 RID: 38313
		public bool IsRandom;

		// Token: 0x040095AA RID: 38314
		public bool IsIntro;

		// Token: 0x040095AB RID: 38315
		public int ActiveChallengeCount = 10;

		// Token: 0x040095AC RID: 38316
		public int DayReset = -1;

		// Token: 0x040095AD RID: 38317
		public bool LinkChallenges;

		// Token: 0x040095AE RID: 38318
		public string Category;

		// Token: 0x040095AF RID: 38319
		public string HiddenBy = "";

		// Token: 0x040095B0 RID: 38320
		public bool UIDirty;

		// Token: 0x040095B1 RID: 38321
		public List<ChallengeGroup.ChallengeCount> ChallengeCounts;

		// Token: 0x040095B2 RID: 38322
		public List<ChallengeClass> ChallengeClasses = new List<ChallengeClass>();

		// Token: 0x040095B3 RID: 38323
		public MinEffectController Effects;

		// Token: 0x0200191F RID: 6431
		public class ChallengeCount
		{
			// Token: 0x040095B4 RID: 38324
			public FastTags<TagGroup.Global> Tags;

			// Token: 0x040095B5 RID: 38325
			public int Count;
		}
	}
}
