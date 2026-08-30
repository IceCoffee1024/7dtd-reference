using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UniLinq;

namespace Challenges
{
	// Token: 0x020018CC RID: 6348
	public class ChallengeGroup
	{
		// Token: 0x0600C35F RID: 50015 RVA: 0x0048302F File Offset: 0x0048122F
		public ChallengeGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600C360 RID: 50016 RVA: 0x00483064 File Offset: 0x00481264
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

		// Token: 0x0600C361 RID: 50017 RVA: 0x00483099 File Offset: 0x00481299
		public static ChallengeGroup GetGroup(string id)
		{
			if (!ChallengeGroup.s_ChallengeGroups.ContainsKey(id))
			{
				return null;
			}
			return ChallengeGroup.s_ChallengeGroups[id];
		}

		// Token: 0x0600C362 RID: 50018 RVA: 0x004830B5 File Offset: 0x004812B5
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

		// Token: 0x0600C363 RID: 50019 RVA: 0x004830ED File Offset: 0x004812ED
		public void AddChallenge(ChallengeClass challenge)
		{
			this.ChallengeClasses.Add(challenge);
		}

		// Token: 0x0600C364 RID: 50020 RVA: 0x004830FC File Offset: 0x004812FC
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

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x0600C365 RID: 50021 RVA: 0x0048338A File Offset: 0x0048158A
		public bool IsActive
		{
			get
			{
				return (!this.IsIntro || ChallengeJournal.IntroChallengesEnabled) && ChallengeCategory.s_ChallengeCategories[this.Category].IsActive;
			}
		}

		// Token: 0x0600C366 RID: 50022 RVA: 0x004833B4 File Offset: 0x004815B4
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

		// Token: 0x0600C367 RID: 50023 RVA: 0x00483433 File Offset: 0x00481633
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C368 RID: 50024 RVA: 0x0048343E File Offset: 0x0048163E
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null || !this.IsComplete)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C369 RID: 50025 RVA: 0x0048346C File Offset: 0x0048166C
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

		// Token: 0x040093B3 RID: 37811
		public static Dictionary<string, ChallengeGroup> s_ChallengeGroups = new CaseInsensitiveStringDictionary<ChallengeGroup>();

		// Token: 0x040093B4 RID: 37812
		public string Name;

		// Token: 0x040093B5 RID: 37813
		public string Title;

		// Token: 0x040093B6 RID: 37814
		public bool IsComplete;

		// Token: 0x040093B7 RID: 37815
		public string RewardEvent;

		// Token: 0x040093B8 RID: 37816
		public string RewardText;

		// Token: 0x040093B9 RID: 37817
		public string ObjectiveText;

		// Token: 0x040093BA RID: 37818
		public bool IsRandom;

		// Token: 0x040093BB RID: 37819
		public bool IsIntro;

		// Token: 0x040093BC RID: 37820
		public int ActiveChallengeCount = 10;

		// Token: 0x040093BD RID: 37821
		public int DayReset = -1;

		// Token: 0x040093BE RID: 37822
		public bool LinkChallenges;

		// Token: 0x040093BF RID: 37823
		public string Category;

		// Token: 0x040093C0 RID: 37824
		public string HiddenBy = "";

		// Token: 0x040093C1 RID: 37825
		public bool UIDirty;

		// Token: 0x040093C2 RID: 37826
		public List<ChallengeGroup.ChallengeCount> ChallengeCounts;

		// Token: 0x040093C3 RID: 37827
		public List<ChallengeClass> ChallengeClasses = new List<ChallengeClass>();

		// Token: 0x040093C4 RID: 37828
		public MinEffectController Effects;

		// Token: 0x020018CD RID: 6349
		public class ChallengeCount
		{
			// Token: 0x040093C5 RID: 37829
			public FastTags<TagGroup.Global> Tags;

			// Token: 0x040093C6 RID: 37830
			public int Count;
		}
	}
}
