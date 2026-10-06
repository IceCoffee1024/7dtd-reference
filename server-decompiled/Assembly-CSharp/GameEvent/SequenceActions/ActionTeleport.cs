using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F6 RID: 6646
	[Preserve]
	public class ActionTeleport : ActionBaseTeleport
	{
		// Token: 0x0600CB1F RID: 51999 RVA: 0x004A4AE4 File Offset: 0x004A2CE4
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			World world = GameManager.Instance.World;
			Vector3 vector = Vector3.zero;
			switch (this.offsetType)
			{
			case ActionTeleport.OffsetTypes.None:
				vector = this.target_position;
				break;
			case ActionTeleport.OffsetTypes.Relative:
				vector = target.position + target.transform.TransformDirection(this.target_position);
				break;
			case ActionTeleport.OffsetTypes.World:
				vector = target.position + this.target_position;
				break;
			}
			if (vector.y > 0f)
			{
				base.TeleportEntity(target, vector);
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600CB20 RID: 52000 RVA: 0x004A4B6F File Offset: 0x004A2D6F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(ActionTeleport.PropTargetPosition, ref this.target_position);
			properties.ParseEnum<ActionTeleport.OffsetTypes>(ActionTeleport.PropOffsetType, ref this.offsetType);
		}

		// Token: 0x0600CB21 RID: 52001 RVA: 0x004A4B9A File Offset: 0x004A2D9A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTeleport
			{
				targetGroup = this.targetGroup,
				target_position = this.target_position,
				offsetType = this.offsetType,
				teleportDelayText = this.teleportDelayText
			};
		}

		// Token: 0x040099CD RID: 39373
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 target_position;

		// Token: 0x040099CE RID: 39374
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleport.OffsetTypes offsetType;

		// Token: 0x040099CF RID: 39375
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetPosition = "target_position";

		// Token: 0x040099D0 RID: 39376
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOffsetType = "offset_type";

		// Token: 0x020019F7 RID: 6647
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OffsetTypes
		{
			// Token: 0x040099D2 RID: 39378
			None,
			// Token: 0x040099D3 RID: 39379
			Relative,
			// Token: 0x040099D4 RID: 39380
			World
		}
	}
}
