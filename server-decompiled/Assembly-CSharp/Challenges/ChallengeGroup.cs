using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UniLinq;

namespace Challenges
{
	// Token: 0x0200194F RID: 6479
	public class ChallengeGroup
	{
		// Token: 0x0600C752 RID: 51026 RVA: 0x00491253 File Offset: 0x0048F453
		public ChallengeGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600C753 RID: 51027 RVA: 0x00491288 File Offset: 0x0048F488
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

		// Token: 0x0600C754 RID: 51028 RVA: 0x004912BD File Offset: 0x0048F4BD
		public static ChallengeGroup GetGroup(string id)
		{
			if (!ChallengeGroup.s_ChallengeGroups.ContainsKey(id))
			{
				return null;
			}
			return ChallengeGroup.s_ChallengeGroups[id];
		}

		// Token: 0x0600C755 RID: 51029 RVA: 0x004912D9 File Offset: 0x0048F4D9
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

		// Token: 0x0600C756 RID: 51030 RVA: 0x00491311 File Offset: 0x0048F511
		public void AddChallenge(ChallengeClass challenge)
		{
			this.ChallengeClasses.Add(challenge);
		}

		// Token: 0x0600C757 RID: 51031 RVA: 0x00491320 File Offset: 0x0048F520
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

		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x0600C758 RID: 51032 RVA: 0x004915AE File Offset: 0x0048F7AE
		public bool IsActive
		{
			get
			{
				return (!this.IsIntro || ChallengeJournal.IntroChallengesEnabled) && ChallengeCategory.s_ChallengeCategories[this.Category].IsActive;
			}
		}

		// Token: 0x0600C759 RID: 51033 RVA: 0x004915D8 File Offset: 0x0048F7D8
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

		// Token: 0x0600C75A RID: 51034 RVA: 0x00491657 File Offset: 0x0048F857
		public bool HasEventsOrPassives()
		{
			return this.Effects != null;
		}

		// Token: 0x0600C75B RID: 51035 RVA: 0x00491662 File Offset: 0x0048F862
		public void ModifyValue(EntityAlive _ea, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
		{
			if (this.Effects == null || !this.IsComplete)
			{
				return;
			}
			this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, 0f, _tags, 1);
		}

		// Token: 0x0600C75C RID: 51036 RVA: 0x00491690 File Offset: 0x0048F890
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

		// Token: 0x0400968B RID: 38539
		public static Dictionary<string, ChallengeGroup> s_ChallengeGroups = new CaseInsensitiveStringDictionary<ChallengeGroup>();

		// Token: 0x0400968C RID: 38540
		public string Name;

		// Token: 0x0400968D RID: 38541
		public string Title;

		// Token: 0x0400968E RID: 38542
		public bool IsComplete;

		// Token: 0x0400968F RID: 38543
		public string RewardEvent;

		// Token: 0x04009690 RID: 38544
		public string RewardText;

		// Token: 0x04009691 RID: 38545
		public string ObjectiveText;

		// Token: 0x04009692 RID: 38546
		public bool IsRandom;

		// Token: 0x04009693 RID: 38547
		public bool IsIntro;

		// Token: 0x04009694 RID: 38548
		public int ActiveChallengeCount = 10;

		// Token: 0x04009695 RID: 38549
		public int DayReset = -1;

		// Token: 0x04009696 RID: 38550
		public bool LinkChallenges;

		// Token: 0x04009697 RID: 38551
		public string Category;

		// Token: 0x04009698 RID: 38552
		public string HiddenBy = "";

		// Token: 0x04009699 RID: 38553
		public bool UIDirty;

		// Token: 0x0400969A RID: 38554
		public List<ChallengeGroup.ChallengeCount> ChallengeCounts;

		// Token: 0x0400969B RID: 38555
		public List<ChallengeClass> ChallengeClasses = new List<ChallengeClass>();

		// Token: 0x0400969C RID: 38556
		public MinEffectController Effects;

		// Token: 0x02001950 RID: 6480
		public class ChallengeCount
		{
			// Token: 0x0400969D RID: 38557
			public FastTags<TagGroup.Global> Tags;

			// Token: 0x0400969E RID: 38558
			public int Count;
		}
	}
}
