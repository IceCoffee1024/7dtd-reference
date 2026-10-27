using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001906 RID: 6406
	[Preserve]
	public class RequirementLevel : BaseRequirement
	{
		// Token: 0x0600C542 RID: 50498 RVA: 0x00488B68 File Offset: 0x00486D68
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementLevel_keyword", false, null);
			this.expectedLevel = Convert.ToInt32(base.Value);
			base.Description = string.Format("{0} {1}", text, this.expectedLevel);
		}

		// Token: 0x0600C543 RID: 50499 RVA: 0x00488BAF File Offset: 0x00486DAF
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || XUiM_Player.GetLevel(base.OwnerQuest.OwnerJournal.OwnerPlayer) >= this.expectedLevel;
		}

		// Token: 0x0600C544 RID: 50500 RVA: 0x00488BE0 File Offset: 0x00486DE0
		public override BaseRequirement Clone()
		{
			return new RequirementLevel
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x0400957C RID: 38268
		[PublicizedFrom(EAccessModifier.Private)]
		public int expectedLevel;
	}
}
