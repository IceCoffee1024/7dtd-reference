using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UniLinq;

namespace Challenges
{
	// Token: 0x020018F2 RID: 6386
	public class ChallengeGroup
	{
		// Token: 0x0600C4E6 RID: 50406 RVA: 0x0048936F File Offset: 0x0048756F
		public ChallengeGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600C4E7 RID: 50407 RVA: 0x004893A4 File Offset: 0x004875A4
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

		// Token: 0x0600C4E8 RID: 50408 RVA: 0x004893D9 File Offset: 0x004875D9
		public static ChallengeGroup GetGroup(string id)
		{
			if (!ChallengeGroup.s_ChallengeGroups.ContainsKey(id))
			{
				return null;
			}
			return ChallengeGroup.s_ChallengeGroups[id];
		}

		// Token: 0x0600C4E9 RID: 50409 RVA: 0x004893F5 File Offset: 0x004875F5
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

		// Token: 0x0600C4EA RID: 50410 RVA: 0x0048942D File Offset: 0x0048762D
		public void AddChallenge(ChallengeClass challenge)
		{
			this.ChallengeClasses.Add(challenge);
		}

		// Token: 0x0600C4EB RID: 50411 RVA: 0x0048943C File Offset: 0x0048763C
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

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x0600C4EC RID: 50412 RVA: 0x004896CA File Offset: 0x004878CA
		public bool IsActive
		{
			get
			{
				return (!this.IsIntro || ChallengeJournal.IntroChallengesEnabled) && ChallengeCategory.s_ChallengeCategories[this.Category].IsActive;
			}
		}

		// Token: 0x0600C4ED RID: 50413 RVA: 0x004896F4 File Offset: 0x004878F4
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

		// Token: 0x0600C4EE RID: 50414 RVA: 0x00489773 File Offset: 0x00487973
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C4EF RID: 50415 RVA: 0x0048977E File Offset: 0x0048797E
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null || !this.IsComplete)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C4F0 RID: 50416 RVA: 0x004897AC File Offset: 0x004879AC
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

		// Token: 0x040094DC RID: 38108
		public static Dictionary<string, ChallengeGroup> s_ChallengeGroups = new CaseInsensitiveStringDictionary<ChallengeGroup>();

		// Token: 0x040094DD RID: 38109
		public string Name;

		// Token: 0x040094DE RID: 38110
		public string Title;

		// Token: 0x040094DF RID: 38111
		public bool IsComplete;

		// Token: 0x040094E0 RID: 38112
		public string RewardEvent;

		// Token: 0x040094E1 RID: 38113
		public string RewardText;

		// Token: 0x040094E2 RID: 38114
		public string ObjectiveText;

		// Token: 0x040094E3 RID: 38115
		public bool IsRandom;

		// Token: 0x040094E4 RID: 38116
		public bool IsIntro;

		// Token: 0x040094E5 RID: 38117
		public int ActiveChallengeCount = 10;

		// Token: 0x040094E6 RID: 38118
		public int DayReset = -1;

		// Token: 0x040094E7 RID: 38119
		public bool LinkChallenges;

		// Token: 0x040094E8 RID: 38120
		public string Category;

		// Token: 0x040094E9 RID: 38121
		public string HiddenBy = "";

		// Token: 0x040094EA RID: 38122
		public bool UIDirty;

		// Token: 0x040094EB RID: 38123
		public List<ChallengeGroup.ChallengeCount> ChallengeCounts;

		// Token: 0x040094EC RID: 38124
		public List<ChallengeClass> ChallengeClasses = new List<ChallengeClass>();

		// Token: 0x040094ED RID: 38125
		public MinEffectController Effects;

		// Token: 0x020018F3 RID: 6387
		public class ChallengeCount
		{
			// Token: 0x040094EE RID: 38126
			public FastTags<TagGroup.Global> Tags;

			// Token: 0x040094EF RID: 38127
			public int Count;
		}
	}
}
