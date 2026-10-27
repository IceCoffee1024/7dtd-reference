using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E4 RID: 6628
	[Preserve]
	public class ActionFlipRotation : ActionBaseClientAction
	{
		// Token: 0x0600CB00 RID: 51968 RVA: 0x004A2AB4 File Offset: 0x004A0CB4
		public override void OnClientPerform(Entity target)
		{
			if (target != null)
			{
				Entity attachedToEntity = target.AttachedToEntity;
				if (attachedToEntity)
				{
					Transform physicsTransform = attachedToEntity.PhysicsTransform;
					Quaternion quaternion = physicsTransform.rotation;
					quaternion = Quaternion.AngleAxis(180f, physicsTransform.up) * quaternion;
					attachedToEntity.SetRotation(quaternion.eulerAngles);
					physicsTransform.rotation = quaternion;
					EntityVehicle entityVehicle = attachedToEntity as EntityVehicle;
					if (entityVehicle)
					{
						entityVehicle.CameraChangeRotation(180f);
						entityVehicle.VelocityFlip();
						return;
					}
				}
				else
				{
					Vector3 rotation = target.rotation;
					rotation.y += 180f;
					target.SetRotation(rotation);
				}
			}
		}

		// Token: 0x0600CB01 RID: 51969 RVA: 0x004A2B56 File Offset: 0x004A0D56
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionFlipRotation
			{
				targetGroup = this.targetGroup
			};
		}
	}
}
