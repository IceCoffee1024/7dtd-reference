using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001883 RID: 6275
	[Preserve]
	public class RequirementLevel : BaseRequirement
	{
		// Token: 0x0600C14F RID: 49487 RVA: 0x0047AB48 File Offset: 0x00478D48
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementLevel_keyword", false, null);
			this.expectedLevel = Convert.ToInt32(base.Value);
			base.Description = string.Format("{0} {1}", text, this.expectedLevel);
		}

		// Token: 0x0600C150 RID: 49488 RVA: 0x0047AB8F File Offset: 0x00478D8F
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || XUiM_Player.GetLevel(base.OwnerQuest.OwnerJournal.OwnerPlayer) >= this.expectedLevel;
		}

		// Token: 0x0600C151 RID: 49489 RVA: 0x0047ABC0 File Offset: 0x00478DC0
		public override BaseRequirement Clone()
		{
			return new RequirementLevel
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x040092A4 RID: 37540
		[PublicizedFrom(EAccessModifier.Private)]
		public int expectedLevel;
	}
}
