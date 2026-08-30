using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001973 RID: 6515
	[Preserve]
	public class ActionPushEntity : ActionBaseTargetAction
	{
		// Token: 0x0600C751 RID: 51025 RVA: 0x00495C9C File Offset: 0x00493E9C
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

		// Token: 0x0600C752 RID: 51026 RVA: 0x00495DA0 File Offset: 0x00493FA0
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

		// Token: 0x0600C753 RID: 51027 RVA: 0x00495ED0 File Offset: 0x004940D0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionPushEntity.Direction>(ActionPushEntity.PropDirection, ref this.direction);
			properties.ParseFloat(ActionPushEntity.PropForce, ref this.force);
		}

		// Token: 0x0600C754 RID: 51028 RVA: 0x00495EFB File Offset: 0x004940FB
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

		// Token: 0x04009704 RID: 38660
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionPushEntity.Direction direction;

		// Token: 0x04009705 RID: 38661
		[PublicizedFrom(EAccessModifier.Protected)]
		public float force = 2f;

		// Token: 0x04009706 RID: 38662
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDirection = "direction";

		// Token: 0x04009707 RID: 38663
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropForce = "distance";

		// Token: 0x02001974 RID: 6516
		public enum Direction
		{
			// Token: 0x04009709 RID: 38665
			Random,
			// Token: 0x0400970A RID: 38666
			Forward,
			// Token: 0x0400970B RID: 38667
			Backward,
			// Token: 0x0400970C RID: 38668
			Left,
			// Token: 0x0400970D RID: 38669
			Right
		}
	}
}
