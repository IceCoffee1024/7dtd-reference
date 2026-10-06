using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018D5 RID: 6357
	[Preserve]
	public class RequirementLevel : BaseRequirement
	{
		// Token: 0x0600C42E RID: 50222 RVA: 0x004851BC File Offset: 0x004833BC
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementLevel_keyword", false, null);
			this.expectedLevel = Convert.ToInt32(base.Value);
			base.Description = string.Format("{0} {1}", text, this.expectedLevel);
		}

		// Token: 0x0600C42F RID: 50223 RVA: 0x00485203 File Offset: 0x00483403
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || XUiM_Player.GetLevel(base.OwnerQuest.OwnerJournal.OwnerPlayer) >= this.expectedLevel;
		}

		// Token: 0x0600C430 RID: 50224 RVA: 0x00485234 File Offset: 0x00483434
		public override BaseRequirement Clone()
		{
			return new RequirementLevel
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x04009493 RID: 38035
		[PublicizedFrom(EAccessModifier.Private)]
		public int expectedLevel;
	}
}
