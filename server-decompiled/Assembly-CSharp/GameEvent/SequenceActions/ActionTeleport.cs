using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A27 RID: 6695
	[Preserve]
	public class ActionTeleport : ActionBaseTeleport
	{
		// Token: 0x0600CC33 RID: 52275 RVA: 0x004A85E0 File Offset: 0x004A67E0
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

		// Token: 0x0600CC34 RID: 52276 RVA: 0x004A866B File Offset: 0x004A686B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(ActionTeleport.PropTargetPosition, ref this.target_position);
			properties.ParseEnum<ActionTeleport.OffsetTypes>(ActionTeleport.PropOffsetType, ref this.offsetType);
		}

		// Token: 0x0600CC35 RID: 52277 RVA: 0x004A8696 File Offset: 0x004A6896
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

		// Token: 0x04009AB6 RID: 39606
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 target_position;

		// Token: 0x04009AB7 RID: 39607
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleport.OffsetTypes offsetType;

		// Token: 0x04009AB8 RID: 39608
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetPosition = "target_position";

		// Token: 0x04009AB9 RID: 39609
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOffsetType = "offset_type";

		// Token: 0x02001A28 RID: 6696
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OffsetTypes
		{
			// Token: 0x04009ABB RID: 39611
			None,
			// Token: 0x04009ABC RID: 39612
			Relative,
			// Token: 0x04009ABD RID: 39613
			World
		}
	}
}
