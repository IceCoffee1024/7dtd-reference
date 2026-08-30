using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A4 RID: 6564
	[Preserve]
	public class ActionTeleport : ActionBaseTeleport
	{
		// Token: 0x0600C840 RID: 51264 RVA: 0x0049A2E8 File Offset: 0x004984E8
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

		// Token: 0x0600C841 RID: 51265 RVA: 0x0049A373 File Offset: 0x00498573
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseVec(ActionTeleport.PropTargetPosition, ref this.target_position);
			properties.ParseEnum<ActionTeleport.OffsetTypes>(ActionTeleport.PropOffsetType, ref this.offsetType);
		}

		// Token: 0x0600C842 RID: 51266 RVA: 0x0049A39E File Offset: 0x0049859E
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

		// Token: 0x040097DE RID: 38878
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 target_position;

		// Token: 0x040097DF RID: 38879
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleport.OffsetTypes offsetType;

		// Token: 0x040097E0 RID: 38880
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetPosition = "target_position";

		// Token: 0x040097E1 RID: 38881
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOffsetType = "offset_type";

		// Token: 0x020019A5 RID: 6565
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OffsetTypes
		{
			// Token: 0x040097E3 RID: 38883
			None,
			// Token: 0x040097E4 RID: 38884
			Relative,
			// Token: 0x040097E5 RID: 38885
			World
		}
	}
}
