using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000607 RID: 1543
public class ThrownWeaponMoveScript : MonoBehaviour
{
	// Token: 0x06003240 RID: 12864 RVA: 0x001472A4 File Offset: 0x001454A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (ThrownWeaponMoveScript.gameManager == null)
		{
			ThrownWeaponMoveScript.gameManager = global::UnityEngine.Object.FindAnyObjectByType<GameManager>();
		}
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x001472C0 File Offset: 0x001454C0
	public void Fire(Vector3 _idealStartPosition, Vector3 _flyDirection, Entity _firingEntity, int _hmOverride = 0, float _velocity = -1f)
	{
		this.flyDirection = _flyDirection.normalized;
		this.idealPosition = _idealStartPosition;
		this.firingEntity = _firingEntity;
		this.velocity = this.flyDirection.normalized * EffectManager.GetValue(PassiveEffects.ProjectileVelocity, this.itemValueWeapon, (_velocity == -1f) ? ((float)this.itemActionThrownWeapon.Velocity) : _velocity, _firingEntity as EntityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.bArmed = true;
		this.hmOverride = _hmOverride;
		this.waterCollisionParticles.Init(this.ProjectileOwnerID, this.itemWeapon.MadeOfMaterial.SurfaceCategory, "water", 16);
		if (_idealStartPosition == Vector3.zero)
		{
			this.previousPosition = base.transform.position + Origin.position;
		}
		else
		{
			this.previousPosition = _idealStartPosition;
		}
		CapsuleCollider component = base.transform.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.gravity = Vector3.up * EffectManager.GetValue(PassiveEffects.ProjectileGravity, this.itemValueWeapon, this.itemActionThrownWeapon.Gravity, _firingEntity as EntityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (base.transform.GetComponent<OnActivateItemGameObjectReference>() != null)
		{
			base.transform.GetComponent<OnActivateItemGameObjectReference>().ActivateItem(true);
		}
		this.timeShotStarted = Time.time;
		if (base.transform.parent != null)
		{
			base.transform.parent = null;
			Utils.SetLayerRecursively(base.transform.gameObject, 0);
		}
		base.transform.position = _idealStartPosition - Origin.position;
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x00147490 File Offset: 0x00145690
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FixedUpdate()
	{
		if (!this.bArmed)
		{
			if (base.transform.lossyScale.x < 0.01f)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!base.transform.gameObject.activeInHierarchy)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!this.stuckInEntity)
			{
				base.transform.position = this.FinalPosition - Origin.position;
				return;
			}
		}
		else
		{
			if (ThrownWeaponMoveScript.gameManager == null || ThrownWeaponMoveScript.gameManager.World == null)
			{
				return;
			}
			if (Time.time - this.timeShotStarted >= this.itemActionThrownWeapon.FlyTime)
			{
				this.velocity += this.gravity * Time.fixedDeltaTime;
			}
			Vector3 vector = this.velocity * Time.fixedDeltaTime;
			if (!ThrownWeaponMoveScript.gameManager.World.IsChunkAreaCollidersLoaded(base.transform.position + vector + Origin.position))
			{
				vector = this.gravity * Time.fixedDeltaTime;
			}
			base.transform.LookAt(base.transform.position + vector);
			base.transform.Rotate(Vector3.right, 90f);
			this.checkCollision(vector);
			if (this.bArmed)
			{
				base.transform.position = base.transform.position + vector;
				if (Time.time - this.timeShotStarted >= this.itemActionThrownWeapon.LifeTime)
				{
					global::UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x00147634 File Offset: 0x00145834
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void checkCollision(Vector3 _amountToMove)
	{
		if (ThrownWeaponMoveScript.gameManager == null || ThrownWeaponMoveScript.gameManager.World == null)
		{
			return;
		}
		Vector3 vector = _amountToMove;
		float num = vector.magnitude;
		if (num <= 0f)
		{
			return;
		}
		Vector3 vector2 = vector * (1f / num);
		num += 0.5f;
		EntityAlive entityAlive = (EntityAlive)this.firingEntity;
		Ray ray = new Ray(base.transform.position + Origin.position + vector2 * -0.2f, vector2);
		this.waterCollisionParticles.CheckCollision(ray.origin, ray.direction, num, (entityAlive != null) ? entityAlive.entityId : (-1));
		int num2 = 0;
		if (entityAlive != null && entityAlive.emodel != null)
		{
			num2 = entityAlive.GetModelLayer();
			entityAlive.SetModelLayer(2, false, null);
		}
		bool flag = Voxel.Raycast(ThrownWeaponMoveScript.gameManager.World, ray, num, -538750989, (this.hmOverride == 0) ? 8 : this.hmOverride, 0f);
		if (entityAlive != null && entityAlive.emodel != null)
		{
			entityAlive.SetModelLayer(num2, false, null);
		}
		if (flag && (GameUtils.IsBlockOrTerrain(Voxel.voxelRayHitInfo.tag) || Voxel.voxelRayHitInfo.tag.StartsWith("E_")))
		{
			if (this.firingEntity != null && !this.firingEntity.isEntityRemote)
			{
				this.stuckInEntity = ItemActionAttack.FindHitEntity(Voxel.voxelRayHitInfo) as EntityAlive;
				entityAlive.MinEventContext.Other = this.stuckInEntity;
				entityAlive.FireEvent(MinEventTypes.onProjectileImpact, false);
				MinEventParams.CachedEventParam.Self = entityAlive;
				MinEventParams.CachedEventParam.Position = Voxel.voxelRayHitInfo.hit.pos;
				MinEventParams.CachedEventParam.ItemValue = this.itemValueWeapon;
				this.itemWeapon.FireEvent(MinEventTypes.onProjectileImpact, MinEventParams.CachedEventParam);
				ItemActionAttack.AttackHitInfo attackHitInfo = new ItemActionAttack.AttackHitInfo
				{
					WeaponTypeTag = ItemActionAttack.ThrownTag
				};
				if (this.itemValueWeapon.MaxUseTimes > 0)
				{
					this.itemValueWeapon.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, this.itemValueWeapon, 1f, this.firingEntity as EntityAlive, null, this.itemValueWeapon.ItemClass.ItemTags | FastTags<TagGroup.Global>.Parse("Secondary"), true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
				}
				ItemActionAttack.Hit(Voxel.voxelRayHitInfo, this.ProjectileOwnerID, EnumDamageTypes.Piercing, this.itemActionThrownWeapon.GetDamageBlock(this.itemValueWeapon, ItemActionAttack.GetBlockHit(ThrownWeaponMoveScript.gameManager.World, Voxel.voxelRayHitInfo), entityAlive, 1), this.itemActionThrownWeapon.GetDamageEntity(this.itemValueWeapon, entityAlive, 1), 1f, 1f, EffectManager.GetValue(PassiveEffects.CriticalChance, this.itemValueWeapon, this.itemWeapon.CritChance.Value, entityAlive, null, this.itemWeapon.ItemTags, true, true, true, true, true, 1, true, false), ItemAction.GetDismemberChance(this.actionData, Voxel.voxelRayHitInfo), this.itemWeapon.MadeOfMaterial.SurfaceCategory, null, this.getBuffActions(), attackHitInfo, 1, this.itemActionThrownWeapon.ActionExp, this.itemActionThrownWeapon.ActionExpBonusMultiplier, null, null, ItemActionAttack.EnumAttackMode.RealNoHarvesting, null, -1, this.itemValueWeapon, false, false, true, null);
				this.NavObject = NavObjectManager.Instance.RegisterNavObject(this.itemWeapon.NavObject, base.transform, "", false);
				if (GameUtils.IsBlockOrTerrain(Voxel.voxelRayHitInfo.tag))
				{
					this.ProjectileID = ProjectileManager.AddProjectileItem(base.transform, -1, Voxel.voxelRayHitInfo.hit.pos, vector.normalized, this.itemValueWeapon.type);
				}
				else
				{
					this.ProjectileID = ProjectileManager.AddProjectileItem(base.transform, -1, Voxel.voxelRayHitInfo.hit.pos, vector.normalized, this.itemValueWeapon.type);
				}
				Utils.SetLayerRecursively(ProjectileManager.GetProjectile(this.ProjectileID).gameObject, 14);
			}
			else
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			this.bArmed = false;
		}
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x00147A47 File Offset: 0x00145C47
	public void DropStuckProjectile()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x00147A54 File Offset: 0x00145C54
	public void OnDestroy()
	{
		if (GameManager.Instance == null || GameManager.Instance.World == null || this.firingEntity == null || base.transform == null || this.itemValueWeapon == null)
		{
			return;
		}
		if (this.stuckInEntity != null)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this.NavObject);
			if (this.ProjectileID != -1 && this.firingEntity != null && !this.firingEntity.isEntityRemote)
			{
				Vector3 bellyPosition = this.stuckInEntity.getBellyPosition();
				if (GameManager.Instance.World.IsChunkAreaLoaded(Mathf.CeilToInt(bellyPosition.x), Mathf.CeilToInt(bellyPosition.y), Mathf.CeilToInt(bellyPosition.z)))
				{
					GameManager.Instance.ItemDropServer(new ItemStack(this.itemValueWeapon, 1), bellyPosition, Vector3.zero, this.ProjectileOwnerID, 1000f, false);
					return;
				}
			}
		}
		else if (this.ProjectileID != -1 && this.firingEntity != null && !this.firingEntity.isEntityRemote && GameManager.Instance.World.IsChunkAreaLoaded(Mathf.CeilToInt(base.transform.position.x + Origin.position.x), Mathf.CeilToInt(base.transform.position.y + Origin.position.y), Mathf.CeilToInt(base.transform.position.z + Origin.position.z)))
		{
			GameManager.Instance.ItemDropServer(new ItemStack(this.itemValueWeapon, 1), base.transform.position + Origin.position + Vector3.up, Vector3.zero, this.ProjectileOwnerID, 1000f, false);
		}
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x00147C3C File Offset: 0x00145E3C
	public bool TryCollect(EntityPlayerLocal _player)
	{
		if (!this.itemWeapon.IsSticky)
		{
			return false;
		}
		ItemStack itemStack = new ItemStack(this.itemValueWeapon, 1);
		if (_player.PlayerUI.xui.PlayerInventory.AddItem(itemStack))
		{
			this.ProjectileID = -1;
			global::UnityEngine.Object.Destroy(base.gameObject);
			return true;
		}
		GameManager.ShowTooltip(_player, Localization.Get("xuiInventoryFullForPickup", false, null), string.Empty, "ui_denied", null, false, false, 0f);
		return false;
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x00147CB6 File Offset: 0x00145EB6
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<string> getBuffActions()
	{
		return this.itemActionThrownWeapon.BuffActions;
	}

	// Token: 0x040027E0 RID: 10208
	public const int InvalidID = -1;

	// Token: 0x040027E1 RID: 10209
	public int ProjectileID = -1;

	// Token: 0x040027E2 RID: 10210
	public int ProjectileOwnerID;

	// Token: 0x040027E3 RID: 10211
	public ItemActionThrownWeapon itemActionThrownWeapon;

	// Token: 0x040027E4 RID: 10212
	public ItemClass itemWeapon;

	// Token: 0x040027E5 RID: 10213
	public ItemValue itemValueWeapon;

	// Token: 0x040027E6 RID: 10214
	public ItemActionThrowAway.MyInventoryData actionData;

	// Token: 0x040027E7 RID: 10215
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static GameManager gameManager;

	// Token: 0x040027E8 RID: 10216
	public Vector3 flyDirection;

	// Token: 0x040027E9 RID: 10217
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 idealPosition;

	// Token: 0x040027EA RID: 10218
	public Vector3 velocity;

	// Token: 0x040027EB RID: 10219
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float timeShotStarted;

	// Token: 0x040027EC RID: 10220
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bArmed;

	// Token: 0x040027ED RID: 10221
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Entity firingEntity;

	// Token: 0x040027EE RID: 10222
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 previousPosition;

	// Token: 0x040027EF RID: 10223
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 gravity;

	// Token: 0x040027F0 RID: 10224
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bOnIdealPos;

	// Token: 0x040027F1 RID: 10225
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int hmOverride;

	// Token: 0x040027F2 RID: 10226
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public NavObject NavObject;

	// Token: 0x040027F3 RID: 10227
	public Vector3 FinalPosition;

	// Token: 0x040027F4 RID: 10228
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CollisionParticleController waterCollisionParticles = new CollisionParticleController();

	// Token: 0x040027F5 RID: 10229
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive stuckInEntity;
}
