using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CA RID: 6602
	[Preserve]
	public class ActionTeleport : ActionBaseTeleport
	{
		// Token: 0x0600C9C7 RID: 51655 RVA: 0x004A0640 File Offset: 0x0049E840
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

		// Token: 0x0600C9C8 RID: 51656 RVA: 0x004A06CB File Offset: 0x0049E8CB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(ActionTeleport.PropTargetPosition, ref this.target_position);
			properties.ParseEnum<ActionTeleport.OffsetTypes>(ActionTeleport.PropOffsetType, ref this.offsetType);
		}

		// Token: 0x0600C9C9 RID: 51657 RVA: 0x004A06F6 File Offset: 0x0049E8F6
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

		// Token: 0x04009907 RID: 39175
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 target_position;

		// Token: 0x04009908 RID: 39176
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleport.OffsetTypes offsetType;

		// Token: 0x04009909 RID: 39177
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetPosition = "target_position";

		// Token: 0x0400990A RID: 39178
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOffsetType = "offset_type";

		// Token: 0x020019CB RID: 6603
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OffsetTypes
		{
			// Token: 0x0400990C RID: 39180
			None,
			// Token: 0x0400990D RID: 39181
			Relative,
			// Token: 0x0400990E RID: 39182
			World
		}
	}
}
