using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E1 RID: 6625
	[Preserve]
	public class ActionSetEventFlag : ActionBaseTargetAction
	{
		// Token: 0x0600CABE RID: 51902 RVA: 0x004A33B1 File Offset: 0x004A15B1
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			GameEventManager.Current.SetGameEventFlag(this.eventFlag, this.enable, GameEventManager.GetFloatValue(target as EntityAlive, this.durationText, 0f), false);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CABF RID: 51903 RVA: 0x004A33E1 File Offset: 0x004A15E1
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<GameEventManager.GameEventFlagTypes>(ActionSetEventFlag.PropEventFlag, ref this.eventFlag);
			properties.ParseBool(ActionSetEventFlag.PropEnable, ref this.enable);
			properties.ParseString(ActionSetEventFlag.PropDuration, ref this.durationText);
		}

		// Token: 0x0600CAC0 RID: 51904 RVA: 0x004A341D File Offset: 0x004A161D
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

		// Token: 0x04009979 RID: 39289
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventManager.GameEventFlagTypes eventFlag = GameEventManager.GameEventFlagTypes.Invalid;

		// Token: 0x0400997A RID: 39290
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool enable;

		// Token: 0x0400997B RID: 39291
		[PublicizedFrom(EAccessModifier.Protected)]
		public string durationText;

		// Token: 0x0400997C RID: 39292
		public static string PropEventFlag = "event_flag";

		// Token: 0x0400997D RID: 39293
		public static string PropEnable = "enable";

		// Token: 0x0400997E RID: 39294
		public static string PropDuration = "duration";
	}
}
