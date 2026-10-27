using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F6 RID: 6646
	[Preserve]
	public class ActionPushEntity : ActionBaseTargetAction
	{
		// Token: 0x0600CB44 RID: 52036 RVA: 0x004A3FA4 File Offset: 0x004A21A4
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				DamageResponse damageResponse = DamageResponse.New(false);
				damageResponse.StunDuration = 1f;
				damageResponse.Strength = (int)(this.force * 100f);
				Vector3 vector = Vector3.zero;
				switch (this.direction)
				{
				case ActionPushEntity.Direction.Random:
				{
					GameRandom random = GameEventManager.Current.Random;
					vector = new Vector3(random.RandomFloat, random.RandomFloat, random.RandomFloat);
					vector.Normalize();
					break;
				}
				case ActionPushEntity.Direction.Forward:
					vector = entityAlive.transform.forward;
					break;
				case ActionPushEntity.Direction.Backward:
					vector = entityAlive.transform.forward * -1f;
					break;
				case ActionPushEntity.Direction.Left:
					vector = entityAlive.transform.right * -1f;
					break;
				case ActionPushEntity.Direction.Right:
					vector = entityAlive.transform.right;
					break;
				}
				damageResponse.Source = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Bashing, vector);
				entityAlive.DoRagdoll(in damageResponse);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB45 RID: 52037 RVA: 0x004A40A8 File Offset: 0x004A22A8
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckValidPosition(ref Vector3 newPoint, EntityAlive target)
		{
			World world = GameManager.Instance.World;
			Ray ray = new Ray(target.position, (newPoint - target.position).normalized);
			if (Voxel.Raycast(world, ray, this.force, -538750989, 67, 0f))
			{
				newPoint = Voxel.voxelRayHitInfo.hit.pos - ray.direction * 0.1f;
			}
			BlockValue blockValue = world.GetBlock(new Vector3i(newPoint - ray.direction * 0.5f));
			if (blockValue.Block.IsCollideMovement || blockValue.Block.IsCollideArrows)
			{
				newPoint = Voxel.voxelRayHitInfo.hit.pos - ray.direction;
				blockValue = world.GetBlock(new Vector3i(newPoint - ray.direction * 0.5f));
				if (blockValue.Block.IsCollideMovement || blockValue.Block.IsCollideArrows)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CB46 RID: 52038 RVA: 0x004A41D8 File Offset: 0x004A23D8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionPushEntity.Direction>(ActionPushEntity.PropDirection, ref this.direction);
			properties.ParseFloat(ActionPushEntity.PropForce, ref this.force);
		}

		// Token: 0x0600CB47 RID: 52039 RVA: 0x004A4203 File Offset: 0x004A2403
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPushEntity
			{
				targetGroup = this.targetGroup,
				force = this.force,
				direction = this.direction
			};
		}

		// Token: 0x040099DC RID: 39388
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionPushEntity.Direction direction;

		// Token: 0x040099DD RID: 39389
		[PublicizedFrom(EAccessModifier.Protected)]
		public float force = 2f;

		// Token: 0x040099DE RID: 39390
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDirection = "direction";

		// Token: 0x040099DF RID: 39391
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropForce = "distance";

		// Token: 0x020019F7 RID: 6647
		public enum Direction
		{
			// Token: 0x040099E1 RID: 39393
			Random,
			// Token: 0x040099E2 RID: 39394
			Forward,
			// Token: 0x040099E3 RID: 39395
			Backward,
			// Token: 0x040099E4 RID: 39396
			Left,
			// Token: 0x040099E5 RID: 39397
			Right
		}
	}
}
