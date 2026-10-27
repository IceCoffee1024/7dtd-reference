using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A15 RID: 6677
	[Preserve]
	public class ActionSetHordeNight : BaseAction
	{
		// Token: 0x0600CBDC RID: 52188 RVA: 0x004A7051 File Offset: 0x004A5251
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (GameManager.Instance != null && GameManager.Instance.World != null)
			{
				GameManager.Instance.World.aiDirector.BloodMoonComponent.SetForToday(this.keepBMDay);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CBDD RID: 52189 RVA: 0x004A708D File Offset: 0x004A528D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionSetHordeNight.PropKeepBMDay, ref this.keepBMDay);
		}

		// Token: 0x0600CBDE RID: 52190 RVA: 0x004A70A7 File Offset: 0x004A52A7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetHordeNight
			{
				keepBMDay = this.keepBMDay
			};
		}

		// Token: 0x04009A6D RID: 39533
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool keepBMDay = true;

		// Token: 0x04009A6E RID: 39534
		public static string PropKeepBMDay = "keep_bm_day";
	}
}
