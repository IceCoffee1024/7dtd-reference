using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018A5 RID: 6309
	[Preserve]
	public class RequirementBuff : BaseRequirement
	{
		// Token: 0x0600C2C8 RID: 49864 RVA: 0x00480894 File Offset: 0x0047EA94
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementBuff_keyword", false, null);
			base.Description = string.Format("{0} {1}", text, BuffManager.GetBuff(base.ID).Name);
		}

		// Token: 0x0600C2C9 RID: 49865 RVA: 0x004808CF File Offset: 0x0047EACF
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || base.OwnerQuest.OwnerJournal.OwnerPlayer.Buffs.HasBuff(base.ID);
		}

		// Token: 0x0600C2CA RID: 49866 RVA: 0x00480905 File Offset: 0x0047EB05
		public override BaseRequirement Clone()
		{
			return new RequirementBuff
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x040093C5 RID: 37829
		[PublicizedFrom(EAccessModifier.Private)]
		public string name = "";
	}
}
