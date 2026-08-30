using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001999 RID: 6553
	[Preserve]
	public class ActionPushEntity : ActionBaseTargetAction
	{
		// Token: 0x0600C8D8 RID: 51416 RVA: 0x0049BFE0 File Offset: 0x0049A1E0
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

		// Token: 0x0600C8D9 RID: 51417 RVA: 0x0049C0E4 File Offset: 0x0049A2E4
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

		// Token: 0x0600C8DA RID: 51418 RVA: 0x0049C214 File Offset: 0x0049A414
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionPushEntity.Direction>(ActionPushEntity.PropDirection, ref this.direction);
			properties.ParseFloat(ActionPushEntity.PropForce, ref this.force);
		}

		// Token: 0x0600C8DB RID: 51419 RVA: 0x0049C23F File Offset: 0x0049A43F
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

		// Token: 0x0400982D RID: 38957
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionPushEntity.Direction direction;

		// Token: 0x0400982E RID: 38958
		[PublicizedFrom(EAccessModifier.Protected)]
		public float force = 2f;

		// Token: 0x0400982F RID: 38959
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDirection = "direction";

		// Token: 0x04009830 RID: 38960
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropForce = "distance";

		// Token: 0x0200199A RID: 6554
		public enum Direction
		{
			// Token: 0x04009832 RID: 38962
			Random,
			// Token: 0x04009833 RID: 38963
			Forward,
			// Token: 0x04009834 RID: 38964
			Backward,
			// Token: 0x04009835 RID: 38965
			Left,
			// Token: 0x04009836 RID: 38966
			Right
		}
	}
}
