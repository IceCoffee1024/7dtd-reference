using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001902 RID: 6402
	[Preserve]
	public class RequirementBuff : BaseRequirement
	{
		// Token: 0x0600C534 RID: 50484 RVA: 0x0048877C File Offset: 0x0048697C
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementBuff_keyword", false, null);
			base.Description = string.Format("{0} {1}", text, BuffManager.GetBuff(base.ID).Name);
		}

		// Token: 0x0600C535 RID: 50485 RVA: 0x004887B7 File Offset: 0x004869B7
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || base.OwnerQuest.OwnerJournal.OwnerPlayer.Buffs.HasBuff(base.ID);
		}

		// Token: 0x0600C536 RID: 50486 RVA: 0x004887ED File Offset: 0x004869ED
		public override BaseRequirement Clone()
		{
			return new RequirementBuff
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x04009574 RID: 38260
		[PublicizedFrom(EAccessModifier.Private)]
		public string name = "";
	}
}
