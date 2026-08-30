using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B8 RID: 6584
	[Preserve]
	public class ActionSetHordeNight : BaseAction
	{
		// Token: 0x0600C970 RID: 51568 RVA: 0x0049F061 File Offset: 0x0049D261
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (GameManager.Instance != null && GameManager.Instance.World != null)
			{
				GameManager.Instance.World.aiDirector.BloodMoonComponent.SetForToday(this.keepBMDay);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C971 RID: 51569 RVA: 0x0049F09D File Offset: 0x0049D29D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionSetHordeNight.PropKeepBMDay, ref this.keepBMDay);
		}

		// Token: 0x0600C972 RID: 51570 RVA: 0x0049F0B7 File Offset: 0x0049D2B7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetHordeNight
			{
				keepBMDay = this.keepBMDay
			};
		}

		// Token: 0x040098BE RID: 39102
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool keepBMDay = true;

		// Token: 0x040098BF RID: 39103
		public static string PropKeepBMDay = "keep_bm_day";
	}
}
