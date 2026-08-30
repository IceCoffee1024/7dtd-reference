using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018A9 RID: 6313
	[Preserve]
	public class RequirementLevel : BaseRequirement
	{
		// Token: 0x0600C2D6 RID: 49878 RVA: 0x00480C74 File Offset: 0x0047EE74
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementLevel_keyword", false, null);
			this.expectedLevel = Convert.ToInt32(base.Value);
			base.Description = string.Format("{0} {1}", text, this.expectedLevel);
		}

		// Token: 0x0600C2D7 RID: 49879 RVA: 0x00480CBB File Offset: 0x0047EEBB
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || XUiM_Player.GetLevel(base.OwnerQuest.OwnerJournal.OwnerPlayer) >= this.expectedLevel;
		}

		// Token: 0x0600C2D8 RID: 49880 RVA: 0x00480CEC File Offset: 0x0047EEEC
		public override BaseRequirement Clone()
		{
			return new RequirementLevel
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x040093CD RID: 37837
		[PublicizedFrom(EAccessModifier.Private)]
		public int expectedLevel;
	}
}
