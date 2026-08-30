using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198F RID: 6543
	[Preserve]
	public class ActionSetEventFlag : ActionBaseTargetAction
	{
		// Token: 0x0600C7DF RID: 51167 RVA: 0x00498B69 File Offset: 0x00496D69
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			GameEventManager.Current.SetGameEventFlag(this.eventFlag, this.enable, GameEventManager.GetFloatValue(target as EntityAlive, this.durationText, 0f), false);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7E0 RID: 51168 RVA: 0x00498B99 File Offset: 0x00496D99
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<GameEventManager.GameEventFlagTypes>(ActionSetEventFlag.PropEventFlag, ref this.eventFlag);
			properties.ParseBool(ActionSetEventFlag.PropEnable, ref this.enable);
			properties.ParseString(ActionSetEventFlag.PropDuration, ref this.durationText);
		}

		// Token: 0x0600C7E1 RID: 51169 RVA: 0x00498BD5 File Offset: 0x00496DD5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetEventFlag
			{
				targetGroup = this.targetGroup,
				eventFlag = this.eventFlag,
				enable = this.enable,
				durationText = this.durationText
			};
		}

		// Token: 0x0400978A RID: 38794
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventManager.GameEventFlagTypes eventFlag = GameEventManager.GameEventFlagTypes.Invalid;

		// Token: 0x0400978B RID: 38795
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool enable;

		// Token: 0x0400978C RID: 38796
		[PublicizedFrom(EAccessModifier.Protected)]
		public string durationText;

		// Token: 0x0400978D RID: 38797
		public static string PropEventFlag = "event_flag";

		// Token: 0x0400978E RID: 38798
		public static string PropEnable = "enable";

		// Token: 0x0400978F RID: 38799
		public static string PropDuration = "duration";
	}
}
