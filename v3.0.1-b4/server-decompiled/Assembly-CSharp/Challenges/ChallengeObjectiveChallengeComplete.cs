using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018D6 RID: 6358
	[Preserve]
	public class ChallengeObjectiveChallengeComplete : BaseChallengeObjective
	{
		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x0600C3C8 RID: 50120 RVA: 0x002635C1 File Offset: 0x002617C1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.ChallengeComplete;
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x0600C3C9 RID: 50121 RVA: 0x00484600 File Offset: 0x00482800
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

		// Token: 0x0600C3CA RID: 50122 RVA: 0x004846A8 File Offset: 0x004828A8
		public override void BaseInit()
		{
			base.BaseInit();
			this.UpdateMax();
		}

		// Token: 0x0600C3CB RID: 50123 RVA: 0x004846B6 File Offset: 0x004828B6
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C3CC RID: 50124 RVA: 0x004846C4 File Offset: 0x004828C4
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

		// Token: 0x0600C3CD RID: 50125 RVA: 0x00484717 File Offset: 0x00482917
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupWindowOpen("Challenges"));
		}

		// Token: 0x0600C3CE RID: 50126 RVA: 0x00484737 File Offset: 0x00482937
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ChallengeComplete += this.Current_ChallengeComplete;
		}

		// Token: 0x0600C3CF RID: 50127 RVA: 0x0048474F File Offset: 0x0048294F
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ChallengeComplete -= this.Current_ChallengeComplete;
		}

		// Token: 0x0600C3D0 RID: 50128 RVA: 0x00484768 File Offset: 0x00482968
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

		// Token: 0x0600C3D1 RID: 50129 RVA: 0x004848A0 File Offset: 0x00482AA0
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

		// Token: 0x0600C3D2 RID: 50130 RVA: 0x0048494B File Offset: 0x00482B4B
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveChallengeComplete
			{
				ChallengeName = this.ChallengeName,
				IsRedeemed = this.IsRedeemed,
				IsGroup = this.IsGroup
			};
		}

		// Token: 0x04009401 RID: 37889
		public string ChallengeName = "";

		// Token: 0x04009402 RID: 37890
		public bool IsGroup;

		// Token: 0x04009403 RID: 37891
		public bool IsRedeemed;
	}
}
