using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199D RID: 6557
	[Preserve]
	public class ActionRandomizeRotation : ActionBaseClientAction
	{
		// Token: 0x0600C8E8 RID: 51432 RVA: 0x0049C448 File Offset: 0x0049A648
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

		// Token: 0x0600C8E9 RID: 51433 RVA: 0x0049C4F3 File Offset: 0x0049A6F3
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
