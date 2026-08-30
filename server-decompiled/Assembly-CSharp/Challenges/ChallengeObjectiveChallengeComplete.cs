using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018FC RID: 6396
	[Preserve]
	public class ChallengeObjectiveChallengeComplete : BaseChallengeObjective
	{
		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x0600C54F RID: 50511 RVA: 0x002664A9 File Offset: 0x002646A9
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.ChallengeComplete;
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x0600C550 RID: 50512 RVA: 0x0048A948 File Offset: 0x00488B48
		public override string DescriptionText
		{
			get
			{
				string text = Localization.Get("challengeTargetAnyChallenge", false, null);
				if (this.ChallengeName != "")
				{
					if (this.IsGroup)
					{
						ChallengeGroup challengeGroup = ChallengeGroup.s_ChallengeGroups[this.ChallengeName];
						if (challengeGroup != null)
						{
							text = challengeGroup.Title;
						}
					}
					else
					{
						ChallengeClass challenge = ChallengeClass.GetChallenge(this.ChallengeName);
						if (challenge != null)
						{
							text = challenge.Title;
						}
					}
				}
				if (this.IsRedeemed)
				{
					return Localization.Get("challengeObjectiveRedeem", false, null) + " [DECEA3]" + text + "[-]:";
				}
				return Localization.Get("challengeObjectiveComplete", false, null) + " [DECEA3]" + text + "[-]:";
			}
		}

		// Token: 0x0600C551 RID: 50513 RVA: 0x0048A9F0 File Offset: 0x00488BF0
		public override void BaseInit()
		{
			base.BaseInit();
			this.UpdateMax();
		}

		// Token: 0x0600C552 RID: 50514 RVA: 0x0048A9FE File Offset: 0x00488BFE
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C553 RID: 50515 RVA: 0x0048AA0C File Offset: 0x00488C0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateMax()
		{
			if (this.IsGroup)
			{
				ChallengeGroup challengeGroup = ChallengeGroup.s_ChallengeGroups[this.ChallengeName];
				this.MaxCount = challengeGroup.ChallengeClasses.Count;
				if (this.OwnerClass.ChallengeGroup == challengeGroup)
				{
					this.MaxCount--;
				}
			}
		}

		// Token: 0x0600C554 RID: 50516 RVA: 0x0048AA5F File Offset: 0x00488C5F
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupWindowOpen("Challenges"));
		}

		// Token: 0x0600C555 RID: 50517 RVA: 0x0048AA7F File Offset: 0x00488C7F
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ChallengeComplete += this.Current_ChallengeComplete;
		}

		// Token: 0x0600C556 RID: 50518 RVA: 0x0048AA97 File Offset: 0x00488C97
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ChallengeComplete -= this.Current_ChallengeComplete;
		}

		// Token: 0x0600C557 RID: 50519 RVA: 0x0048AAB0 File Offset: 0x00488CB0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_ChallengeComplete(ChallengeClass _challenge, bool _isRedeemed)
		{
			if (this.IsGroup)
			{
				base.Current = 0;
				List<ChallengeClass> challengeClasses = ChallengeGroup.s_ChallengeGroups[this.ChallengeName].ChallengeClasses;
				int i = 0;
				while (i < challengeClasses.Count)
				{
					Challenge challenge = this.Owner.Owner.ChallengeDictionary[challengeClasses[i].Name];
					bool flag = false;
					if (challenge.ChallengeState != Challenge.ChallengeStates.Active)
					{
						goto IL_0065;
					}
					if (challenge == this.Owner)
					{
						flag = true;
						goto IL_0065;
					}
					IL_009C:
					i++;
					continue;
					IL_0065:
					if (!flag && (!this.IsRedeemed || challenge.ChallengeState == Challenge.ChallengeStates.Redeemed) && (this.IsRedeemed || challenge.ChallengeState == Challenge.ChallengeStates.Completed))
					{
						int num = base.Current;
						base.Current = num + 1;
						goto IL_009C;
					}
					goto IL_009C;
				}
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
					return;
				}
			}
			else if ((string.IsNullOrEmpty(this.ChallengeName) || string.Compare(_challenge.Name, this.ChallengeName, true) == 0) && _isRedeemed == this.IsRedeemed)
			{
				int num = base.Current;
				base.Current = num + 1;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C558 RID: 50520 RVA: 0x0048ABE8 File Offset: 0x00488DE8
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("challenge"))
			{
				this.ChallengeName = e.GetAttribute("challenge");
			}
			if (e.HasAttribute("is_group"))
			{
				this.IsGroup = StringParsers.ParseBool(e.GetAttribute("is_group"), 0, -1, true);
				if (this.IsGroup)
				{
					this.MaxCount = -1;
				}
			}
			if (e.HasAttribute("is_redeemed"))
			{
				this.IsRedeemed = StringParsers.ParseBool(e.GetAttribute("is_redeemed"), 0, -1, true);
			}
		}

		// Token: 0x0600C559 RID: 50521 RVA: 0x0048AC93 File Offset: 0x00488E93
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveChallengeComplete
			{
				ChallengeName = this.ChallengeName,
				IsRedeemed = this.IsRedeemed,
				IsGroup = this.IsGroup
			};
		}

		// Token: 0x0400952A RID: 38186
		public string ChallengeName = "";

		// Token: 0x0400952B RID: 38187
		public bool IsGroup;

		// Token: 0x0400952C RID: 38188
		public bool IsRedeemed;
	}
}
