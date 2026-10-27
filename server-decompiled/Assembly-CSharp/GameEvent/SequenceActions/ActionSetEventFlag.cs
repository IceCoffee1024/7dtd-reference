using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A12 RID: 6674
	[Preserve]
	public class ActionSetEventFlag : ActionBaseTargetAction
	{
		// Token: 0x0600CBD2 RID: 52178 RVA: 0x004A6EB1 File Offset: 0x004A50B1
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			GameEventManager.Current.SetGameEventFlag(this.eventFlag, this.enable, GameEventManager.GetFloatValue(target as EntityAlive, this.durationText, 0f), false);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CBD3 RID: 52179 RVA: 0x004A6EE1 File Offset: 0x004A50E1
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<GameEventManager.GameEventFlagTypes>(ActionSetEventFlag.PropEventFlag, ref this.eventFlag);
			properties.ParseBool(ActionSetEventFlag.PropEnable, ref this.enable);
			properties.ParseString(ActionSetEventFlag.PropDuration, ref this.durationText);
		}

		// Token: 0x0600CBD4 RID: 52180 RVA: 0x004A6F1D File Offset: 0x004A511D
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

		// Token: 0x04009A62 RID: 39522
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventManager.GameEventFlagTypes eventFlag = GameEventManager.GameEventFlagTypes.Invalid;

		// Token: 0x04009A63 RID: 39523
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool enable;

		// Token: 0x04009A64 RID: 39524
		[PublicizedFrom(EAccessModifier.Protected)]
		public string durationText;

		// Token: 0x04009A65 RID: 39525
		public static string PropEventFlag = "event_flag";

		// Token: 0x04009A66 RID: 39526
		public static string PropEnable = "enable";

		// Token: 0x04009A67 RID: 39527
		public static string PropDuration = "duration";
	}
}
