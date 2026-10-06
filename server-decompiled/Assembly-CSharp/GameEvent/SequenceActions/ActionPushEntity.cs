using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C5 RID: 6597
	[Preserve]
	public class ActionPushEntity : ActionBaseTargetAction
	{
		// Token: 0x0600CA30 RID: 51760 RVA: 0x004A04A4 File Offset: 0x0049E6A4
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

		// Token: 0x0600CA31 RID: 51761 RVA: 0x004A05A8 File Offset: 0x0049E7A8
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

		// Token: 0x0600CA32 RID: 51762 RVA: 0x004A06D8 File Offset: 0x0049E8D8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionPushEntity.Direction>(ActionPushEntity.PropDirection, ref this.direction);
			properties.ParseFloat(ActionPushEntity.PropForce, ref this.force);
		}

		// Token: 0x0600CA33 RID: 51763 RVA: 0x004A0703 File Offset: 0x0049E903
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

		// Token: 0x040098F3 RID: 39155
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionPushEntity.Direction direction;

		// Token: 0x040098F4 RID: 39156
		[PublicizedFrom(EAccessModifier.Protected)]
		public float force = 2f;

		// Token: 0x040098F5 RID: 39157
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDirection = "direction";

		// Token: 0x040098F6 RID: 39158
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropForce = "distance";

		// Token: 0x020019C6 RID: 6598
		public enum Direction
		{
			// Token: 0x040098F8 RID: 39160
			Random,
			// Token: 0x040098F9 RID: 39161
			Forward,
			// Token: 0x040098FA RID: 39162
			Backward,
			// Token: 0x040098FB RID: 39163
			Left,
			// Token: 0x040098FC RID: 39164
			Right
		}
	}
}
