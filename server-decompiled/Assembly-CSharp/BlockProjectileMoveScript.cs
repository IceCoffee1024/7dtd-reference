using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class BlockProjectileMoveScript : ProjectileMoveScript
{
	// Token: 0x060007D3 RID: 2003 RVA: 0x000374F4 File Offset: 0x000356F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void checkCollision()
	{
		GameManager instance = GameManager.Instance;
		if (!instance)
		{
			return;
		}
		if (instance.World == null)
		{
			return;
		}
		Vector3 vector;
		if (this.isOnIdealPos)
		{
			vector = base.transform.position;
		}
		else
		{
			vector = this.idealPosition;
		}
		Vector3 vector2 = vector - this.previousPosition;
		float magnitude = vector2.magnitude;
		if (magnitude < 0.04f)
		{
			return;
		}
		Ray ray = new Ray(this.previousPosition, vector2);
		this.previousPosition = vector;
		int num = 0;
		EntityAlive entityAlive = (EntityAlive)this.firingEntity;
		if (entityAlive != null && entityAlive.emodel != null)
		{
			num = entityAlive.GetModelLayer();
			entityAlive.SetModelLayer(2, false, null);
		}
		this.hitMask = 32;
		bool flag = Voxel.Raycast(instance.World, ray, magnitude, -538750989, this.hitMask, 0f);
		if (entityAlive != null && entityAlive.emodel != null)
		{
			entityAlive.SetModelLayer(num, false, null);
		}
		if (flag && (GameUtils.IsBlockOrTerrain(Voxel.voxelRayHitInfo.tag) || Voxel.voxelRayHitInfo.tag.StartsWith("E_")))
		{
			base.enabled = false;
			global::UnityEngine.Object.Destroy(base.transform.gameObject);
			Transform transform = Voxel.voxelRayHitInfo.transform;
			string text = null;
			if (Voxel.voxelRayHitInfo.tag.StartsWith("E_BP_"))
			{
				text = Voxel.voxelRayHitInfo.tag.Substring("E_BP_".Length).ToLower();
				transform = GameUtils.GetHitRootTransform(Voxel.voxelRayHitInfo.tag, Voxel.voxelRayHitInfo.transform);
			}
			if (Voxel.voxelRayHitInfo.tag.StartsWith("E_"))
			{
				Entity component = transform.GetComponent<Entity>();
				if (component == null)
				{
					return;
				}
				DamageSourceEntity damageSourceEntity = new DamageSourceEntity(EnumDamageSource.External, EnumDamageTypes.Piercing, this.ProjectileOwnerID)
				{
					AttackingItem = this.itemValueProjectile,
					bIgnorePartyShare = true,
					bTrapKillXP = true
				};
				int num2 = (int)this.GetProjectileDamageEntity();
				component.DamageEntity(damageSourceEntity, num2, false, 1f);
				if (this.itemActionProjectile.BuffActions != null)
				{
					EntityAlive entityAlive2 = component as EntityAlive;
					if (entityAlive2 != null)
					{
						string text2 = ((text != null) ? GameUtils.GetChildTransformPath(entityAlive2.transform, Voxel.voxelRayHitInfo.transform) : null);
						ItemAction.ExecuteBuffActions(this.itemActionProjectile.BuffActions, -1, entityAlive2, false, damageSourceEntity.GetEntityDamageBodyPart(entityAlive2), text2);
					}
				}
			}
			if (this.itemActionProjectile.Explosion.ParticleIndex > 0)
			{
				Vector3 vector3 = Voxel.voxelRayHitInfo.hit.pos - vector2.normalized * 0.1f;
				Vector3i vector3i = World.worldToBlockPos(vector3);
				if (!instance.World.GetBlock(vector3i).isair)
				{
					BlockFace blockFace;
					vector3i = Voxel.OneVoxelStep(vector3i, vector3, -vector2.normalized, out vector3, out blockFace);
				}
				instance.ExplosionServer(vector3, vector3i, Quaternion.identity, this.itemActionProjectile.Explosion, this.ProjectileOwnerID, 0f, false, this.itemValueProjectile);
			}
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00037803 File Offset: 0x00035A03
	public float GetProjectileDamageEntity()
	{
		return this.itemActionProjectile.GetDamageEntity(this.itemValueProjectile, null, 0);
	}
}
