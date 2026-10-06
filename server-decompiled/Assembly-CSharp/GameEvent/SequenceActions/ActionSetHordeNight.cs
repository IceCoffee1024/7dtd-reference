using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E4 RID: 6628
	[Preserve]
	public class ActionSetHordeNight : BaseAction
	{
		// Token: 0x0600CAC8 RID: 51912 RVA: 0x004A3551 File Offset: 0x004A1751
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (GameManager.Instance != null && GameManager.Instance.World != null)
			{
				GameManager.Instance.World.aiDirector.BloodMoonComponent.SetForToday(this.keepBMDay);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAC9 RID: 51913 RVA: 0x004A358D File Offset: 0x004A178D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionSetHordeNight.PropKeepBMDay, ref this.keepBMDay);
		}

		// Token: 0x0600CACA RID: 51914 RVA: 0x004A35A7 File Offset: 0x004A17A7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetHordeNight
			{
				keepBMDay = this.keepBMDay
			};
		}

		// Token: 0x04009984 RID: 39300
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool keepBMDay = true;

		// Token: 0x04009985 RID: 39301
		public static string PropKeepBMDay = "keep_bm_day";
	}
}
