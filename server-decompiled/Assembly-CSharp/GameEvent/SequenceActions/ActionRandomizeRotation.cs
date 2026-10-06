using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C9 RID: 6601
	[Preserve]
	public class ActionRandomizeRotation : ActionBaseClientAction
	{
		// Token: 0x0600CA40 RID: 51776 RVA: 0x004A090C File Offset: 0x0049EB0C
		public override void OnClientPerform(Entity target)
		{
			if (target != null)
			{
				Entity attachedToEntity = target.AttachedToEntity;
				float num = (float)GameEventManager.Current.Random.RandomRange(45, 315);
				if (attachedToEntity)
				{
					Transform physicsTransform = attachedToEntity.PhysicsTransform;
					Quaternion quaternion = physicsTransform.rotation;
					quaternion = Quaternion.AngleAxis(num, physicsTransform.up) * quaternion;
					attachedToEntity.SetRotation(quaternion.eulerAngles);
					physicsTransform.rotation = quaternion;
					EntityVehicle entityVehicle = attachedToEntity as EntityVehicle;
					if (entityVehicle)
					{
						entityVehicle.CameraChangeRotation(num);
						return;
					}
				}
				else
				{
					Vector3 rotation = target.rotation;
					rotation.y += num;
					target.SetRotation(rotation);
				}
			}
		}

		// Token: 0x0600CA41 RID: 51777 RVA: 0x004A09B7 File Offset: 0x0049EBB7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRandomizeRotation
			{
				targetGroup = this.targetGroup
			};
		}
	}
}
