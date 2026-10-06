using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AssetManagement;
using UnityEngine;

// Token: 0x02000606 RID: 1542
public class ProjectileMoveScript : MonoBehaviour
{
	// Token: 0x06003274 RID: 12916 RVA: 0x00146AC2 File Offset: 0x00144CC2
	[PublicizedFrom(EAccessModifier.Private)]
	static ProjectileMoveScript()
	{
		GameManager.Instance.OnWorldChanged += ProjectileMoveScript.OnWorldChanged;
		ProjectileMoveScript.OnWorldChanged(GameManager.Instance.World);
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x00146AEC File Offset: 0x00144CEC
	public void Fire(Vector3 _idealStartPos, Vector3 _dirOrPos, Entity _firingEntity, int _hmOverride = 0, float _radius = 0f, bool _isBallistic = false)
	{
		Transform transform = base.transform;
		this.isOnIdealPos = true;
		this.previousPosition = transform.position + Origin.position;
		if (_idealStartPos.y != 0f)
		{
			this.idealPosition = _idealStartPos;
			this.isOnIdealPos = false;
		}
		this.firingEntity = _firingEntity;
		this.hitMask = ((_hmOverride == 0) ? 80 : _hmOverride);
		this.radius = ((_radius >= 0f) ? _radius : this.itemActionProjectile.collisionRadius);
		if (this.itemActionProjectile.FlyTime < 0f)
		{
			EntityAlive entityAlive = _firingEntity as EntityAlive;
			if (entityAlive != null)
			{
				if (_isBallistic)
				{
					Vector3 vector = _dirOrPos - _idealStartPos;
					float magnitude = vector.magnitude;
					this.flyDirection = vector / magnitude;
					float num = Utils.FastLerp(0.4f, -this.itemActionProjectile.FlyTime, magnitude / this.itemActionProjectile.Velocity);
					this.velocity = vector / num;
					this.velocity.y = this.velocity.y + this.itemActionProjectile.Gravity * -0.5f * num;
					Vector3 vector2 = this.velocity * 0.015f;
					this.previousPosition += vector2;
					this.idealPosition += vector2;
				}
				else
				{
					this.flyDirection = entityAlive.GetForwardVector();
					this.velocity = this.flyDirection * 2f;
					this.explosionDisabled = true;
				}
			}
			this.gravity = this.itemActionProjectile.Gravity;
		}
		else
		{
			this.flyDirection = _dirOrPos.normalized;
			this.velocity = this.flyDirection * EffectManager.GetValue(PassiveEffects.ProjectileVelocity, this.itemValueLauncher, this.itemActionProjectile.Velocity, _firingEntity as EntityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			this.gravity = EffectManager.GetValue(PassiveEffects.ProjectileGravity, this.itemValueLauncher, this.itemActionProjectile.Gravity, _firingEntity as EntityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		this.waterCollisionParticles.Init(this.ProjectileOwnerID, this.itemProjectile.MadeOfMaterial.SurfaceCategory, "water", 16);
		OnActivateItemGameObjectReference component = transform.GetComponent<OnActivateItemGameObjectReference>();
		if (component)
		{
			component.ActivateItem(true);
		}
		if (transform.parent)
		{
			transform.SetParent(null);
			Utils.SetLayerRecursively(transform.gameObject, 0);
		}
		transform.position = this.previousPosition - Origin.position;
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.SetState(ProjectileMoveScript.State.Active);
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x00146DA0 File Offset: 0x00144FA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetState(ProjectileMoveScript.State _state)
	{
		this.state = _state;
		this.stateTime = 0f;
		if (this.state == ProjectileMoveScript.State.Dead)
		{
			Transform transform = base.transform;
			Transform transform2 = transform.Find("MeshExplode");
			if (transform2)
			{
				transform2.gameObject.SetActive(false);
			}
			Light componentInChildren = transform.GetComponentInChildren<Light>();
			if (componentInChildren)
			{
				componentInChildren.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x00146E08 File Offset: 0x00145008
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnWorldChanged(World _world)
	{
		if (ProjectileMoveScript.s_lastWorld == _world)
		{
			return;
		}
		if (ProjectileMoveScript.s_lastWorld != null)
		{
			ProjectileMoveScript.s_lastWorld.EntityUnloadedDelegates -= ProjectileMoveScript.OnEntityUnloaded;
		}
		if (_world != null)
		{
			_world.EntityUnloadedDelegates += ProjectileMoveScript.OnEntityUnloaded;
		}
		ProjectileMoveScript.s_lastWorld = _world;
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x00146E58 File Offset: 0x00145058
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FixedUpdate()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		this.stateTime += fixedDeltaTime;
		if (this.state == ProjectileMoveScript.State.Active)
		{
			GameManager instance = GameManager.Instance;
			if (!instance || instance.World == null)
			{
				return;
			}
			if (this.stateTime > this.itemActionProjectile.FlyTime)
			{
				this.velocity.y = this.velocity.y + this.gravity * fixedDeltaTime;
			}
			Transform transform = base.transform;
			Vector3 vector = transform.position;
			Vector3 vector2 = this.velocity * fixedDeltaTime;
			transform.LookAt(vector + vector2);
			vector += vector2;
			if (!this.isOnIdealPos)
			{
				this.idealPosition += vector2;
				vector = Vector3.Lerp(vector, this.idealPosition - Origin.position, this.stateTime * 5f);
				this.isOnIdealPos = this.stateTime >= 0.2f;
			}
			transform.position = vector;
			if (this.stateTime >= this.itemActionProjectile.LifeTime)
			{
				this.SetState(ProjectileMoveScript.State.Dead);
			}
		}
		else if (this.state == ProjectileMoveScript.State.Sticky)
		{
			GameManager instance2 = GameManager.Instance;
			World world = ((instance2 != null) ? instance2.World : null);
			if (this.stateTime >= 180f)
			{
				this.SetState(ProjectileMoveScript.State.StickyDestroyed);
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (world != null)
			{
				bool flag = this.stickyForceRevive;
				if (!flag && this.stickyRay != null && this.stickyRay.Value.direction != Vector3.zero && this.stickyRayDistance > 0f && (!Voxel.Raycast(world, this.stickyRay.Value, this.stickyRayDistance, -538751005, this.hitMask, this.radius) || (!GameUtils.IsBlockOrTerrain(Voxel.voxelRayHitInfo.tag) && !Voxel.voxelRayHitInfo.tag.StartsWith("E_"))))
				{
					flag = true;
				}
				if (flag)
				{
					this.DoRevive();
				}
			}
		}
		else if (this.state == ProjectileMoveScript.State.Dead && this.stateTime > this.itemActionProjectile.DeadTime)
		{
			this.SetState(ProjectileMoveScript.State.Destroyed);
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.state == ProjectileMoveScript.State.Active)
		{
			this.checkCollision();
		}
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x001470A4 File Offset: 0x001452A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoRevive()
	{
		if (this.ProjectileID == -1)
		{
			return;
		}
		GameManager instance = GameManager.Instance;
		GameRandom gameRandom;
		if (instance == null)
		{
			gameRandom = null;
		}
		else
		{
			World world = instance.World;
			gameRandom = ((world != null) ? world.GetGameRandom() : null);
		}
		GameRandom gameRandom2 = gameRandom;
		if (gameRandom2 == null)
		{
			return;
		}
		if (!ProjectileManager.RemoveProjectile(this.ProjectileID, true))
		{
			return;
		}
		Component transform = base.transform;
		this.stickyRay = null;
		this.stickyRayDistance = 0f;
		this.stickyForceRevive = false;
		Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, this.velocity.normalized);
		Vector3 normalized = new Vector3(gameRandom2.RandomGaussian * 0.4f, gameRandom2.RandomGaussian * 0.4f, 1f).normalized;
		float num = gameRandom2.RandomFloat * 0.9f;
		this.velocity = quaternion * normalized * num;
		if (this.velocity.y > 0f)
		{
			this.velocity.y = 0f;
		}
		Utils.SetLayerRecursively(transform.gameObject, 0);
		this.SetState(ProjectileMoveScript.State.Active);
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x001471A8 File Offset: 0x001453A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void checkCollision()
	{
		GameManager instance = GameManager.Instance;
		if (!instance)
		{
			return;
		}
		World world = instance.World;
		if (world == null)
		{
			return;
		}
		Vector3 vector;
		if (this.isOnIdealPos)
		{
			vector = base.transform.position + Origin.position;
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
		EntityAlive entityAlive = (EntityAlive)this.firingEntity;
		this.waterCollisionParticles.CheckCollision(ray.origin, ray.direction, magnitude, entityAlive ? entityAlive.entityId : (-1));
		int num = -1;
		if (entityAlive && entityAlive.emodel)
		{
			num = entityAlive.GetModelLayer();
			entityAlive.SetModelLayer(2, false, null);
		}
		float num2 = this.radius + this.collisionStartBack;
		ray.origin -= ray.direction * num2;
		bool flag = Voxel.Raycast(world, ray, magnitude + num2, -538751005, this.hitMask, this.radius);
		this.collisionStartBack = 0.1f;
		if (num >= 0)
		{
			entityAlive.SetModelLayer(num, false, null);
		}
		if (flag && (GameUtils.IsBlockOrTerrain(Voxel.voxelRayHitInfo.tag) || Voxel.voxelRayHitInfo.tag.StartsWith("E_")))
		{
			if (this.firingEntity && !this.firingEntity.isEntityRemote)
			{
				EntityAlive entityAlive2 = ItemActionAttack.FindHitEntity(Voxel.voxelRayHitInfo) as EntityAlive;
				EntityDrone entityDrone = entityAlive2 as EntityDrone;
				EntityAlive entityAlive3 = this.firingEntity as EntityAlive;
				if (entityDrone && entityAlive3 && entityDrone.isAlly(entityAlive3))
				{
					return;
				}
				entityAlive.MinEventContext.Other = entityAlive2;
				ItemActionAttack.AttackHitInfo attackHitInfo = new ItemActionAttack.AttackHitInfo
				{
					WeaponTypeTag = ItemActionAttack.RangedTag
				};
				MinEventParams.CachedEventParam.Self = entityAlive;
				MinEventParams.CachedEventParam.Position = Voxel.voxelRayHitInfo.hit.pos;
				MinEventParams.CachedEventParam.ItemValue = this.itemValueProjectile;
				MinEventParams.CachedEventParam.Other = entityAlive2;
				MinEventParams.CachedEventParam.ItemActionData = this.actionData;
				this.itemProjectile.FireEvent(MinEventTypes.onProjectilePreImpact, MinEventParams.CachedEventParam);
				float num3 = Utils.FastLerp(1f, this.itemActionProjectile.GetDamageBlock(this.itemValueLauncher, ItemActionAttack.GetBlockHit(world, Voxel.voxelRayHitInfo), entityAlive, 0), this.actionData.strainPercent);
				float num4 = Utils.FastLerp(1f, this.itemActionProjectile.GetDamageEntity(this.itemValueLauncher, entityAlive, 0), this.actionData.strainPercent);
				bool flag2;
				if (entityAlive3 is EntityPlayer)
				{
					num4 *= ItemActionAttack.RangedDamagePercent;
					flag2 = ItemActionAttack.RangedDamagePercent == 0f;
				}
				else
				{
					num4 *= ItemActionAttack.EntityPlayerDamagePercent;
					flag2 = ItemActionAttack.EntityPlayerDamagePercent == 0f;
				}
				ItemActionAttack.Hit(Voxel.voxelRayHitInfo, this.ProjectileOwnerID, EnumDamageTypes.Piercing, num3, num4, 1f, 1f, EffectManager.GetValue(PassiveEffects.CriticalChance, this.itemValueLauncher, this.itemProjectile.CritChance.Value, entityAlive, null, this.itemProjectile.ItemTags, true, true, true, true, true, 1, true, false), ItemAction.GetDismemberChance(this.actionData, Voxel.voxelRayHitInfo), this.itemProjectile.MadeOfMaterial.SurfaceCategory, this.itemActionProjectile.GetDamageMultiplier(), this.getBuffActions(), attackHitInfo, 1, this.itemActionProjectile.ActionExp, this.itemActionProjectile.ActionExpBonusMultiplier, null, null, ItemActionAttack.EnumAttackMode.RealNoHarvesting, null, -1, this.itemValueLauncher, false, flag2, true, null);
				if (!entityAlive2)
				{
					entityAlive.FireEvent(MinEventTypes.onSelfPrimaryActionMissEntity, true);
				}
				entityAlive.FireEvent(MinEventTypes.onProjectileImpact, false);
				MinEventParams.CachedEventParam.Self = entityAlive;
				MinEventParams.CachedEventParam.Position = Voxel.voxelRayHitInfo.hit.pos;
				MinEventParams.CachedEventParam.ItemValue = this.itemValueProjectile;
				MinEventParams.CachedEventParam.Other = entityAlive2;
				MinEventParams.CachedEventParam.ItemActionData = this.actionData;
				this.itemProjectile.FireEvent(MinEventTypes.onProjectileImpact, MinEventParams.CachedEventParam);
				if (this.itemActionProjectile.Explosion.ParticleIndex > 0 && !this.explosionDisabled)
				{
					Vector3 vector3 = Voxel.voxelRayHitInfo.hit.pos - vector2.normalized * 0.1f;
					Vector3i vector3i = World.worldToBlockPos(vector3);
					if (!world.GetBlock(vector3i).isair)
					{
						BlockFace blockFace;
						vector3i = Voxel.OneVoxelStep(vector3i, vector3, -vector2.normalized, out vector3, out blockFace);
					}
					instance.ExplosionServer(vector3, vector3i, Quaternion.identity, this.itemActionProjectile.Explosion, this.ProjectileOwnerID, 0f, false, this.itemValueLauncher);
					this.SetState(ProjectileMoveScript.State.Dead);
					return;
				}
				if (!(entityAlive2 is EntitySwarm))
				{
					if (!this.itemProjectile.IsSticky)
					{
						this.SetState(ProjectileMoveScript.State.Dead);
						return;
					}
					GameRandom gameRandom = world.GetGameRandom();
					if (GameUtils.IsBlockOrTerrain(Voxel.voxelRayHitInfo.tag))
					{
						float num5 = this.stickyRoll.GetValueOrDefault();
						float num6;
						if (this.stickyRoll == null)
						{
							num5 = gameRandom.RandomFloat;
							this.stickyRoll = new float?(num5);
							num6 = num5;
						}
						else
						{
							num6 = num5;
						}
						if (num6 < EffectManager.GetValue(PassiveEffects.ProjectileStickChance, this.itemValueLauncher, 0.5f, entityAlive, null, this.itemProjectile.ItemTags | FastTags<TagGroup.Global>.Parse(Voxel.voxelRayHitInfo.fmcHit.blockValue.Block.blockMaterial.SurfaceCategory), true, true, true, true, true, 1, true, false))
						{
							this.ProjectileID = ProjectileManager.AddProjectileItem(base.transform, -1, Voxel.voxelRayHitInfo.hit.pos, vector2.normalized, this.itemValueProjectile.type);
							this.stickyRay = new Ray?(ray);
							this.stickyRayDistance = magnitude + num2;
							this.SetState(ProjectileMoveScript.State.Sticky);
							return;
						}
						instance.SpawnParticleEffectServer(new ParticleEffect("impact_metal_on_wood", Voxel.voxelRayHitInfo.hit.pos, Utils.BlockFaceToRotation(Voxel.voxelRayHitInfo.fmcHit.blockFace), 1f, Color.white, string.Format("{0}hit{1}", Voxel.voxelRayHitInfo.fmcHit.blockValue.Block.blockMaterial.SurfaceCategory, this.itemProjectile.MadeOfMaterial.SurfaceCategory), null, 1f, ""), this.firingEntity.entityId, false, false);
						this.SetState(ProjectileMoveScript.State.Dead);
						return;
					}
					else
					{
						float num5 = this.stickyRoll.GetValueOrDefault();
						float num7;
						if (this.stickyRoll == null)
						{
							num5 = gameRandom.RandomFloat;
							this.stickyRoll = new float?(num5);
							num7 = num5;
						}
						else
						{
							num7 = num5;
						}
						if (num7 < EffectManager.GetValue(PassiveEffects.ProjectileStickChance, this.itemValueLauncher, 0.5f, entityAlive, null, this.itemProjectile.ItemTags, true, true, true, true, true, 1, true, false))
						{
							this.ProjectileID = ProjectileManager.AddProjectileItem(base.transform, -1, Voxel.voxelRayHitInfo.hit.pos, vector2.normalized, this.itemValueProjectile.type);
							Utils.SetLayerRecursively(ProjectileManager.GetProjectile(this.ProjectileID).gameObject, 14);
							this.SetState(ProjectileMoveScript.State.Sticky);
							return;
						}
						instance.SpawnParticleEffectServer(new ParticleEffect("impact_metal_on_wood", Voxel.voxelRayHitInfo.hit.pos, Utils.BlockFaceToRotation(Voxel.voxelRayHitInfo.fmcHit.blockFace), 1f, Color.white, "bullethitwood", null, 1f, ""), this.firingEntity.entityId, false, false);
						this.SetState(ProjectileMoveScript.State.Dead);
						return;
					}
				}
			}
			else
			{
				this.SetState(ProjectileMoveScript.State.Dead);
			}
		}
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x00147968 File Offset: 0x00145B68
	public static void OnEntityUnloaded(Entity _entity, EnumRemoveEntityReason _reason)
	{
		ProjectileMoveScript.<>c__DisplayClass40_0 CS$<>8__locals1;
		CS$<>8__locals1.toUnload = new List<ProjectileMoveScript>();
		ProjectileMoveScript.<OnEntityUnloaded>g__Search|40_0(_entity.RootTransform, ref CS$<>8__locals1);
		foreach (ProjectileMoveScript projectileMoveScript in CS$<>8__locals1.toUnload)
		{
			projectileMoveScript.OnUnloaded();
		}
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x001479D4 File Offset: 0x00145BD4
	public void OnUnloaded()
	{
		if (this.state == ProjectileMoveScript.State.Sticky)
		{
			this.idealPosition = (this.previousPosition = base.transform.position + Origin.position);
			base.transform.SetParent(null);
			base.transform.position = this.previousPosition - Origin.position;
			this.stickyForceRevive = true;
		}
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x00147A3C File Offset: 0x00145C3C
	public void OnDestroy()
	{
		this.assetHandle.Release();
		GameManager instance = GameManager.Instance;
		if (!instance || instance.World == null || !this.firingEntity || this.itemValueProjectile == null)
		{
			return;
		}
		if (this.state == ProjectileMoveScript.State.StickyDestroyed)
		{
			return;
		}
		if (this.ProjectileID != -1 && !this.firingEntity.isEntityRemote)
		{
			Vector3 position = base.transform.position;
			if (instance.World.IsChunkAreaLoaded(Mathf.CeilToInt(position.x + Origin.position.x), Mathf.CeilToInt(position.y + Origin.position.y), Mathf.CeilToInt(position.z + Origin.position.z)))
			{
				instance.ItemDropServer(new ItemStack(this.itemValueProjectile, 1), position + Origin.position, Vector3.zero, this.ProjectileOwnerID, 60f, false);
			}
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x00147B30 File Offset: 0x00145D30
	public bool TryCollect(EntityPlayerLocal _player)
	{
		if (!this.itemProjectile.IsSticky)
		{
			return false;
		}
		ItemStack itemStack = new ItemStack(this.itemValueProjectile, 1);
		if (_player.PlayerUI.xui.PlayerInventory.AddItem(itemStack))
		{
			this.ProjectileID = -1;
			global::UnityEngine.Object.Destroy(base.gameObject);
			return true;
		}
		GameManager.ShowTooltip(_player, Localization.Get("xuiInventoryFullForPickup", false, null), string.Empty, "ui_denied", null, false, false, 0f);
		return false;
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x00147BAA File Offset: 0x00145DAA
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<string> getBuffActions()
	{
		return this.itemActionProjectile.BuffActions;
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x00147BD4 File Offset: 0x00145DD4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <OnEntityUnloaded>g__Search|40_0(Transform parent, ref ProjectileMoveScript.<>c__DisplayClass40_0 A_1)
	{
		foreach (object obj in parent)
		{
			ProjectileMoveScript.<OnEntityUnloaded>g__Search|40_0((Transform)obj, ref A_1);
		}
		ProjectileMoveScript projectileMoveScript;
		if (parent.TryGetComponent<ProjectileMoveScript>(out projectileMoveScript))
		{
			A_1.toUnload.Add(projectileMoveScript);
		}
	}

	// Token: 0x040027B1 RID: 10161
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float ReviveSpread = 0.4f;

	// Token: 0x040027B2 RID: 10162
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float ReviveVelocity = 0.9f;

	// Token: 0x040027B3 RID: 10163
	public const int InvalidID = -1;

	// Token: 0x040027B4 RID: 10164
	public static World s_lastWorld;

	// Token: 0x040027B5 RID: 10165
	public int ProjectileID = -1;

	// Token: 0x040027B6 RID: 10166
	public int ProjectileOwnerID;

	// Token: 0x040027B7 RID: 10167
	public ItemActionProjectile itemActionProjectile;

	// Token: 0x040027B8 RID: 10168
	public ItemClass itemProjectile;

	// Token: 0x040027B9 RID: 10169
	public ItemValue itemValueProjectile;

	// Token: 0x040027BA RID: 10170
	public ItemValue itemValueLauncher;

	// Token: 0x040027BB RID: 10171
	public ItemActionLauncher.ItemActionDataLauncher actionData;

	// Token: 0x040027BC RID: 10172
	public InstanceHandle<GameObject> assetHandle;

	// Token: 0x040027BD RID: 10173
	public Vector3 FinalPosition;

	// Token: 0x040027BE RID: 10174
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 flyDirection;

	// Token: 0x040027BF RID: 10175
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 idealPosition;

	// Token: 0x040027C0 RID: 10176
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 velocity;

	// Token: 0x040027C1 RID: 10177
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public ProjectileMoveScript.State state;

	// Token: 0x040027C2 RID: 10178
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float stateTime;

	// Token: 0x040027C3 RID: 10179
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Entity firingEntity;

	// Token: 0x040027C4 RID: 10180
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 previousPosition;

	// Token: 0x040027C5 RID: 10181
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float gravity;

	// Token: 0x040027C6 RID: 10182
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool isOnIdealPos;

	// Token: 0x040027C7 RID: 10183
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int hitMask;

	// Token: 0x040027C8 RID: 10184
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float radius;

	// Token: 0x040027C9 RID: 10185
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float collisionStartBack;

	// Token: 0x040027CA RID: 10186
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool explosionDisabled;

	// Token: 0x040027CB RID: 10187
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CollisionParticleController waterCollisionParticles = new CollisionParticleController();

	// Token: 0x040027CC RID: 10188
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float? stickyRoll;

	// Token: 0x040027CD RID: 10189
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Ray? stickyRay;

	// Token: 0x040027CE RID: 10190
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float stickyRayDistance;

	// Token: 0x040027CF RID: 10191
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool stickyForceRevive;

	// Token: 0x02000607 RID: 1543
	public enum State
	{
		// Token: 0x040027D1 RID: 10193
		Idle,
		// Token: 0x040027D2 RID: 10194
		Active,
		// Token: 0x040027D3 RID: 10195
		Sticky,
		// Token: 0x040027D4 RID: 10196
		StickyDestroyed,
		// Token: 0x040027D5 RID: 10197
		Dead,
		// Token: 0x040027D6 RID: 10198
		Destroyed
	}
}
