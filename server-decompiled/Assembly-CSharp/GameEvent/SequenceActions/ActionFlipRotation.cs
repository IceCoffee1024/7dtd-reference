using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001987 RID: 6535
	[Preserve]
	public class ActionFlipRotation : ActionBaseClientAction
	{
		// Token: 0x0600C894 RID: 51348 RVA: 0x0049AAF0 File Offset: 0x00498CF0
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

		// Token: 0x0600C895 RID: 51349 RVA: 0x0049AB92 File Offset: 0x00498D92
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
