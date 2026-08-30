using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x0200187F RID: 6271
	[Preserve]
	public class RequirementBuff : BaseRequirement
	{
		// Token: 0x0600C141 RID: 49473 RVA: 0x0047A768 File Offset: 0x00478968
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementBuff_keyword", false, null);
			base.Description = string.Format("{0} {1}", text, BuffManager.GetBuff(base.ID).Name);
		}

		// Token: 0x0600C142 RID: 49474 RVA: 0x0047A7A3 File Offset: 0x004789A3
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || base.OwnerQuest.OwnerJournal.OwnerPlayer.Buffs.HasBuff(base.ID);
		}

		// Token: 0x0600C143 RID: 49475 RVA: 0x0047A7D9 File Offset: 0x004789D9
		public override BaseRequirement Clone()
		{
			return new RequirementBuff
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x0400929C RID: 37532
		[PublicizedFrom(EAccessModifier.Private)]
		public string name = "";
	}
}
