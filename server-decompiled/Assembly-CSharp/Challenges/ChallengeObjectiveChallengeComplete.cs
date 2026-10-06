using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001928 RID: 6440
	[Preserve]
	public class ChallengeObjectiveChallengeComplete : BaseChallengeObjective
	{
		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x0600C6A7 RID: 50855 RVA: 0x00269949 File Offset: 0x00267B49
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.ChallengeComplete;
			}
		}

		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x0600C6A8 RID: 50856 RVA: 0x0048EE38 File Offset: 0x0048D038
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

		// Token: 0x0600C6A9 RID: 50857 RVA: 0x0048EEE0 File Offset: 0x0048D0E0
		public override void BaseInit()
		{
			base.BaseInit();
			this.UpdateMax();
		}

		// Token: 0x0600C6AA RID: 50858 RVA: 0x0048EEEE File Offset: 0x0048D0EE
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C6AB RID: 50859 RVA: 0x0048EEFC File Offset: 0x0048D0FC
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

		// Token: 0x0600C6AC RID: 50860 RVA: 0x0048EF4F File Offset: 0x0048D14F
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupWindowOpen("Challenges"));
		}

		// Token: 0x0600C6AD RID: 50861 RVA: 0x0048EF6F File Offset: 0x0048D16F
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.ChallengeComplete += this.Current_ChallengeComplete;
		}

		// Token: 0x0600C6AE RID: 50862 RVA: 0x0048EF87 File Offset: 0x0048D187
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.ChallengeComplete -= this.Current_ChallengeComplete;
		}

		// Token: 0x0600C6AF RID: 50863 RVA: 0x0048EFA0 File Offset: 0x0048D1A0
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

		// Token: 0x0600C6B0 RID: 50864 RVA: 0x0048F0D8 File Offset: 0x0048D2D8
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

		// Token: 0x0600C6B1 RID: 50865 RVA: 0x0048F183 File Offset: 0x0048D383
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveChallengeComplete
			{
				ChallengeName = this.ChallengeName,
				IsRedeemed = this.IsRedeemed,
				IsGroup = this.IsGroup
			};
		}

		// Token: 0x040095F0 RID: 38384
		public string ChallengeName = "";

		// Token: 0x040095F1 RID: 38385
		public bool IsGroup;

		// Token: 0x040095F2 RID: 38386
		public bool IsRedeemed;
	}
}
