using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018D1 RID: 6353
	[Preserve]
	public class RequirementBuff : BaseRequirement
	{
		// Token: 0x0600C420 RID: 50208 RVA: 0x00484DD0 File Offset: 0x00482FD0
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementBuff_keyword", false, null);
			base.Description = string.Format("{0} {1}", text, BuffManager.GetBuff(base.ID).Name);
		}

		// Token: 0x0600C421 RID: 50209 RVA: 0x00484E0B File Offset: 0x0048300B
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || base.OwnerQuest.OwnerJournal.OwnerPlayer.Buffs.HasBuff(base.ID);
		}

		// Token: 0x0600C422 RID: 50210 RVA: 0x00484E41 File Offset: 0x00483041
		public override BaseRequirement Clone()
		{
			return new RequirementBuff
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x0400948B RID: 38027
		[PublicizedFrom(EAccessModifier.Private)]
		public string name = "";
	}
}
