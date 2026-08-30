using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001992 RID: 6546
	[Preserve]
	public class ActionSetHordeNight : BaseAction
	{
		// Token: 0x0600C7E9 RID: 51177 RVA: 0x00498D09 File Offset: 0x00496F09
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (GameManager.Instance != null && GameManager.Instance.World != null)
			{
				GameManager.Instance.World.aiDirector.BloodMoonComponent.SetForToday(this.keepBMDay);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7EA RID: 51178 RVA: 0x00498D45 File Offset: 0x00496F45
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionSetHordeNight.PropKeepBMDay, ref this.keepBMDay);
		}

		// Token: 0x0600C7EB RID: 51179 RVA: 0x00498D5F File Offset: 0x00496F5F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetHordeNight
			{
				keepBMDay = this.keepBMDay
			};
		}

		// Token: 0x04009795 RID: 38805
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool keepBMDay = true;

		// Token: 0x04009796 RID: 38806
		public static string PropKeepBMDay = "keep_bm_day";
	}
}
