using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001961 RID: 6497
	[Preserve]
	public class ActionFlipRotation : ActionBaseClientAction
	{
		// Token: 0x0600C70D RID: 50957 RVA: 0x004947B0 File Offset: 0x004929B0
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

		// Token: 0x0600C70E RID: 50958 RVA: 0x00494852 File Offset: 0x00492A52
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
