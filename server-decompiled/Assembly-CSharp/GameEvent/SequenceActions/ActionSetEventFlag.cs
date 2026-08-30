using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B5 RID: 6581
	[Preserve]
	public class ActionSetEventFlag : ActionBaseTargetAction
	{
		// Token: 0x0600C966 RID: 51558 RVA: 0x0049EEC1 File Offset: 0x0049D0C1
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			GameEventManager.Current.SetGameEventFlag(this.eventFlag, this.enable, GameEventManager.GetFloatValue(target as EntityAlive, this.durationText, 0f), false);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C967 RID: 51559 RVA: 0x0049EEF1 File Offset: 0x0049D0F1
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<GameEventManager.GameEventFlagTypes>(ActionSetEventFlag.PropEventFlag, ref this.eventFlag);
			properties.ParseBool(ActionSetEventFlag.PropEnable, ref this.enable);
			properties.ParseString(ActionSetEventFlag.PropDuration, ref this.durationText);
		}

		// Token: 0x0600C968 RID: 51560 RVA: 0x0049EF2D File Offset: 0x0049D12D
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

		// Token: 0x040098B3 RID: 39091
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventManager.GameEventFlagTypes eventFlag = GameEventManager.GameEventFlagTypes.Invalid;

		// Token: 0x040098B4 RID: 39092
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool enable;

		// Token: 0x040098B5 RID: 39093
		[PublicizedFrom(EAccessModifier.Protected)]
		public string durationText;

		// Token: 0x040098B6 RID: 39094
		public static string PropEventFlag = "event_flag";

		// Token: 0x040098B7 RID: 39095
		public static string PropEnable = "enable";

		// Token: 0x040098B8 RID: 39096
		public static string PropDuration = "duration";
	}
}
