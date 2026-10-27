using System;
using System.Collections.Generic;
using Audio;
using GamePath;
using Platform;
using RaycastPathing;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004A1 RID: 1185
[Preserve]
public class EntityDrone : EntityNPC, ILockable
{
	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x0600245D RID: 9309 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsValidAimAssistSlowdownTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x0600245E RID: 9310 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsValidAimAssistSnapTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x0600245F RID: 9311 RVA: 0x000DF010 File Offset: 0x000DD210
	public DroneLightManager lightManager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this._lm)
			{
				this._lm = base.transform.GetComponentInChildren<DroneLightManager>();
			}
			return this._lm;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06002460 RID: 9312 RVA: 0x000DF036 File Offset: 0x000DD236
	public float TimeSinceCreation
	{
		get
		{
			if (this.creationTime == 0f)
			{
				return 0f;
			}
			return Time.time - this.creationTime;
		}
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x000DF058 File Offset: 0x000DD258
	public static bool IsValidForLocalPlayer()
	{
		PersistentPlayerData playerData = GameManager.Instance.GetPersistentPlayerList().GetPlayerData(PlatformManager.InternalLocalUserIdentifier);
		return playerData != null && EntityDrone.isValidForPlayer(GameManager.Instance.World.GetEntity(playerData.EntityId) as EntityPlayerLocal);
	}

	// Token: 0x06002462 RID: 9314 RVA: 0x000DF09E File Offset: 0x000DD29E
	public static bool isValidForPlayer(int entityId)
	{
		return GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(entityId) != null && EntityDrone.isValidForPlayer(GameManager.Instance.World.GetEntity(entityId) as EntityAlive);
	}

	// Token: 0x06002463 RID: 9315 RVA: 0x000DF0D0 File Offset: 0x000DD2D0
	public static bool isValidForEntity(int entityId)
	{
		EntityAlive entityAlive = GameManager.Instance.World.GetEntity(entityId) as EntityAlive;
		return entityAlive && EntityDrone.isValidForPlayer(entityAlive);
	}

	// Token: 0x06002464 RID: 9316 RVA: 0x000DF104 File Offset: 0x000DD304
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isValidForPlayer(EntityAlive entityAlive)
	{
		for (int i = 0; i < entityAlive.ownedEntities.Count; i++)
		{
			if (entityAlive.ownedEntities[i].ClassId == EntityClass.junkDroneClass)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x000DF144 File Offset: 0x000DD344
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetItemClassId()
	{
		for (int i = 1; i < ItemClass.list.Length - 1; i++)
		{
			ItemClass itemClass = ItemClass.list[i];
			if (itemClass != null && itemClass.Name == "gunBotT3JunkDrone")
			{
				return itemClass.Id;
			}
		}
		return -1;
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x000DF18A File Offset: 0x000DD38A
	public void PrepareToSpawn()
	{
		this.PlayWakeupAnim = true;
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x000DF194 File Offset: 0x000DD394
	public void OnApplyToEntity(int orderState)
	{
		if (orderState < 0)
		{
			return;
		}
		if (orderState != 0)
		{
			if (orderState != 1)
			{
				return;
			}
			this.SentryMode();
		}
		else
		{
			this.setOrders(EntityDrone.Orders.Follow);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.SendSyncData(16384);
				return;
			}
		}
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x000DF1D8 File Offset: 0x000DD3D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugDroneLog(EntityDrone.LoggingTypes logType, string format, params object[] args)
	{
		int num = 0;
		switch (logType)
		{
		case EntityDrone.LoggingTypes.Init:
			num = (int)EntityDrone.debugDroneInitLogPriority;
			break;
		case EntityDrone.LoggingTypes.Animation:
			num = (int)EntityDrone.debugDroneAnimationLogPriority;
			break;
		case EntityDrone.LoggingTypes.Shutdown:
			num = (int)EntityDrone.debugDroneShutdownLogPriority;
			break;
		}
		if (num == 1)
		{
			Log.Warning(format, args);
			return;
		}
		if (num != 2)
		{
			Log.Out(format, args);
			return;
		}
		Log.Error(format, args);
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x000DF234 File Offset: 0x000DD434
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugDroneLog(string format, params object[] args)
	{
		this.DebugDroneLog(EntityDrone.LoggingTypes.Any, format, args);
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000DF23F File Offset: 0x000DD43F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.steering = new EntityDrone.EntitySteering(this);
		this.isLocked = true;
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000DF25C File Offset: 0x000DD45C
	[PublicizedFrom(EAccessModifier.Private)]
	public new void LateUpdate()
	{
		if (DroneManager.Debug_LocalControl)
		{
			this.debugInputRotX += Input.GetAxis("Mouse X") * 30f * 0.05f;
			this.debugInputRotY += Input.GetAxis("Mouse Y") * 30f * 0.05f;
			this.debugInputRotY = Mathf.Clamp(this.debugInputRotY, -90f, 90f);
			this.reconCam.transform.localRotation = Quaternion.AngleAxis(this.debugInputRotX, Vector3.up);
			this.reconCam.transform.localRotation *= Quaternion.AngleAxis(this.debugInputRotY, Vector3.left);
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && RaycastPathUtils.IsPositionBlocked(this.reconCam.ScreenPointToRay(Input.mousePosition), out raycastHit, 65536, true, 100f))
			{
				RaycastPathUtils.DrawBounds(World.worldToBlockPos(raycastHit.point + Origin.position), Color.yellow, 1f, 1f);
				this.pathMan.CreatePath(this.Owner.position, raycastHit.point + Origin.position, this.currentSpeedFlying, false, this.FollowHoverHeight);
			}
		}
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000DF3AC File Offset: 0x000DD5AC
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x000DF3B8 File Offset: 0x000DD5B8
	public override void PostInit()
	{
		float num = 1f / base.transform.localScale.x;
		this.interactionCollider = base.gameObject.GetComponent<BoxCollider>();
		if (this.interactionCollider)
		{
			this.interactionCollider.center = new Vector3(0f, 0.5f, 0.25f);
			this.interactionCollider.size = new Vector3(2.5f, 2f, 2f);
		}
		this.sensors = new EntityDrone.DroneSensors(this);
		this.sensors.Init();
		this.initWorldValues(this.orderState == EntityDrone.Orders.Follow);
		this.IsFlyMode.Value = true;
		this.bCanClimbLadders = true;
		this.bCanClimbVertical = true;
		this.prefabColor = this.GetPaintColor();
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x000DF484 File Offset: 0x000DD684
	public override void OnAddedToWorld()
	{
		base.OnAddedToWorld();
		if (this.itemvalueToLoad != null)
		{
			this.OriginalItemValue = this.itemvalueToLoad;
		}
		if (this.OriginalItemValue == null)
		{
			this.isSystemSpawn = true;
			int itemClassId = this.GetItemClassId();
			if (itemClassId == -1)
			{
				Log.Warning("Failed to load junk drone from system spawn");
				return;
			}
			this.OriginalItemValue = new ItemValue(itemClassId, false);
		}
		this.isOwnerSyncPending = true;
		this.LoadMods();
		if (this.nativeCollider)
		{
			this.nativeCollider.enabled = true;
		}
		float value = EffectManager.GetValue(PassiveEffects.DegradationMax, this.OriginalItemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		base.Stats.Health.BaseMax = value;
		base.Stats.Health.OriginalMax = value;
		this.Health = Mathf.RoundToInt(value * (1f - this.OriginalItemValue.UseTimes / value));
		this.animator = base.GetComponentInChildren<Animator>();
		this.pathMan = new FloodFillEntityPathGenerator(this.world, this);
		Origin.OriginChanged = (Action<Vector3>)Delegate.Combine(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		this.creationTime = Time.time;
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000DF5B8 File Offset: 0x000DD7B8
	public override void OnEntityUnload()
	{
		Origin.OriginChanged = (Action<Vector3>)Delegate.Remove(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		this.unRegsiterMovingLights();
		this.registeredPartyMembers = null;
		EntityPlayer entityPlayer = this.Owner as EntityPlayer;
		if (entityPlayer)
		{
			Party party = entityPlayer.Party;
			if (party != null)
			{
				party.PartyMemberAdded -= this.onPartyMemberAdded;
				party.PartyMemberRemoved -= this.onPartyMemberRemoved;
			}
			entityPlayer.PlayerTeleportedDelegates -= this.TeleportIfFollowing;
		}
		base.OnEntityUnload();
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000DF64C File Offset: 0x000DD84C
	public override bool CanUpdateEntity()
	{
		return base.CanUpdateEntity();
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool CanNavigatePath()
	{
		return true;
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000DF654 File Offset: 0x000DD854
	public override void SetPosition(Vector3 _pos, bool _bUpdatePhysics = true)
	{
		base.SetPosition(_pos, _bUpdatePhysics);
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x000DF660 File Offset: 0x000DD860
	public override float GetEyeHeight()
	{
		if (this.head == null)
		{
			this.head = base.transform.FindInChilds("Head", false);
		}
		return this.head.position.y - base.transform.position.y;
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000DF6B4 File Offset: 0x000DD8B4
	public override Ray GetLookRay()
	{
		return new Ray(this.position + new Vector3(0f, this.GetEyeHeight(), 0f), (base.GetAttackTarget() == null) ? this.GetLookVector() : (base.GetAttackTarget().getChestPosition() - this.position).normalized);
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool CanBePushed()
	{
		return true;
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000DF71A File Offset: 0x000DD91A
	public override float GetWeight()
	{
		return base.GetWeight();
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDead()
	{
		return false;
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x000DF722 File Offset: 0x000DD922
	public override bool IsAttackValid()
	{
		return this.activeWeapon != null && this.activeWeapon.canFire();
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06002479 RID: 9337 RVA: 0x000D1EC9 File Offset: 0x000D00C9
	// (set) Token: 0x0600247A RID: 9338 RVA: 0x000DF73C File Offset: 0x000DD93C
	public override int Health
	{
		get
		{
			return (int)base.Stats.Health.Value;
		}
		set
		{
			float num = (float)Mathf.Max(value, 1);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && num == 1f && this.state != EntityDrone.State.Shutdown)
			{
				this.isShutdownPending = true;
			}
			base.Stats.Health.Value = num;
		}
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000DF788 File Offset: 0x000DD988
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float _impulseScale)
	{
		int num = Mathf.RoundToInt((float)_strength * this.armorDamageReduction);
		EntityAlive entityAlive = (EntityAlive)this.world.GetEntity(_damageSource.getEntityId());
		if (this.Owner && entityAlive && !this.debugFriendlyFire && entityAlive && this.isAlly(entityAlive))
		{
			num = 0;
		}
		return base.DamageEntity(_damageSource, num, _criticalHit, _impulseScale);
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x000DF7F8 File Offset: 0x000DD9F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleNavObject()
	{
		NavObjectManager instance = NavObjectManager.Instance;
		EntityClass eClass = EntityClass.list[this.entityClass];
		if (eClass.NavObject != "")
		{
			NavObject navObject = instance.NavObjectList.Find(delegate(NavObject n)
			{
				NavObjectClass navObjectClass = n.NavObjectClass;
				return ((navObjectClass != null) ? navObjectClass.NavObjectClassName : null) == eClass.NavObject;
			});
			if (navObject != null)
			{
				instance.UnRegisterNavObject(navObject);
			}
			this.NavObject = instance.RegisterNavObject(eClass.NavObject, this, "", false);
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			primaryPlayer.Waypoints.UpdateEntityDroneWayPoint(this, this.OrderState == EntityDrone.Orders.Follow, false);
		}
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x000DF8B0 File Offset: 0x000DDAB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddCharacterController()
	{
		base.AddCharacterController();
		if (this.PhysicsTransform == null)
		{
			return;
		}
		if (this.m_characterController == null)
		{
			return;
		}
		this.RootMotion = false;
		this.m_characterController.SetSize(Vector3.zero, this.physColHeight, this.physColHeight * 0.5f);
		this.setNoClip(true);
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x00041024 File Offset: 0x0003F224
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float GetPushBoundsVertical()
	{
		return 1f;
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x000027FC File Offset: 0x000009FC
	public override void PlayStepSound(float _volume)
	{
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateStepSound(float _distX, float _distZ, float _rotYDelta)
	{
	}

	// Token: 0x06002481 RID: 9345 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsIgnoredByAI()
	{
		return true;
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000DF90B File Offset: 0x000DDB0B
	public override string ToString()
	{
		return string.Format("[type={0}, id={1}, belongsPlayerId={2}]", base.GetType(), this.entityId, this.belongsPlayerId);
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x000DF934 File Offset: 0x000DDB34
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		_bw.Write(1);
		this.OwnerID.ToStream(_bw, false);
		this.OriginalItemValue = this.GetUpdatedItemValue();
		this.OriginalItemValue.Write(_bw);
		ushort num = 49515;
		_bw.Write(num);
		this.WriteSyncData(_bw, num);
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000DF98C File Offset: 0x000DDB8C
	public override void Read(byte _version, PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_version, _br, _eStreamMode);
		_br.ReadInt32();
		this.OwnerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.OriginalItemValue = ItemValue.None;
		this.OriginalItemValue.Read(_br);
		ushort num = _br.ReadUInt16();
		this.ReadSyncData(_br, num, 0);
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000DF9E0 File Offset: 0x000DDBE0
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		if (DroneManager.Debug_LocalControl)
		{
			return;
		}
		this.SyncOwnerData();
		this.updateTransitionState();
		this.updateAnimStates();
		if (this.isShutdownPending || (!this.Owner && this.state != EntityDrone.State.Shutdown))
		{
			this.performShutdown();
		}
		this.updateShutdownState();
		if (!this.isQuietMode && this.idleLoop == null && this.state == EntityDrone.State.Idle && !GameManager.IsDedicatedServer && GameManager.Instance.World.IsLocalPlayer(this.belongsPlayerId))
		{
			this.idleLoop = this.playSoundLoop("drone_idle_hover", 0.2f);
		}
		if ((this.state == EntityDrone.State.Idle || this.state == EntityDrone.State.Sentry || this.state == EntityDrone.State.Follow) && this.areaScanTimer > 0f)
		{
			this.areaScanTimer -= Time.deltaTime;
			if (this.areaScanTimer <= 0f)
			{
				this.isInConfinedSpace = this.pathMan.IsConfinedSpace(this.position, 3f, false);
				this.areaScanTimer = this.areaScanTime;
			}
		}
		this.updatePartyBuffs();
		this.updateDroneSystems();
		EntityPlayerLocal entityPlayerLocal = this.Owner as EntityPlayerLocal;
		if (entityPlayerLocal && this.state == EntityDrone.State.Idle && this.initSuppressVOTimer <= 0f)
		{
			if (this.focusBoxNode == null)
			{
				if (entityPlayerLocal.MoveController.FocusBoxPosition == World.worldToBlockPos(this.position))
				{
					RaycastNode raycastNode = RaycastPathWorldUtils.FindNodeType(RaycastPathWorldUtils.ScanVolume(this.world, this.position, false, false, false, 0f), cPathNodeType.Air);
					if (raycastNode != null)
					{
						this.focusBoxNode = raycastNode;
					}
				}
			}
			else
			{
				Vector3 vector = this.focusBoxNode.Center - this.position;
				RaycastPathUtils.DrawLine(this.position, this.focusBoxNode.Center, Color.yellow, 1f);
				if (this.isOutOfRange(this.focusBoxNode.Center, 0.25f))
				{
					this.move(vector.normalized, false);
				}
				else
				{
					this.focusBoxNode = null;
				}
			}
		}
		else if (this.state != EntityDrone.State.Idle && this.focusBoxNode != null)
		{
			this.focusBoxNode = null;
		}
		this.updateDroneServiceMenu();
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x000DFC0C File Offset: 0x000DDE0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTasks()
	{
		if (DroneManager.Debug_LocalControl)
		{
			float num = this.debugInputSpeed;
			if (InputUtils.ShiftKeyPressed)
			{
				num *= 10f;
			}
			this.debugInputFwd = this.reconCam.transform.forward;
			this.debugInputFwd.y = 0f;
			if (Input.GetKey(KeyCode.W))
			{
				this.move(this.debugInputFwd, num, false);
			}
			if (Input.GetKey(KeyCode.S))
			{
				this.move(-this.debugInputFwd, num, false);
			}
			this.debugInputRgt = this.reconCam.transform.right;
			this.debugInputRgt.y = 0f;
			if (Input.GetKey(KeyCode.A))
			{
				this.move(-this.debugInputRgt, num, false);
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.move(this.debugInputRgt, num, false);
			}
			this.debugInputUp = this.reconCam.transform.up;
			this.debugInputUp.x = 0f;
			this.debugInputUp.z = 0f;
			if (Input.GetKey(KeyCode.Space))
			{
				this.move(this.debugInputUp, num * 0.5f, false);
			}
			if (Input.GetKey(KeyCode.C))
			{
				this.move(-this.debugInputUp, num * 0.5f, false);
			}
			RaycastPathUtils.DrawBounds(this.Owner.GetBlockPosition().ToVector3CenterXZ() - new Vector3(0.5f, 0f, 0.5f), Color.cyan, 1f, 1f);
			return;
		}
		base.GetEntitySenses().ClearIfExpired();
		if (this.Owner != null)
		{
			this.updateState();
			this.debugUpdate();
		}
		if (PathFinderThread.Instance != null)
		{
			base.updateTasks();
		}
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x000DFDD4 File Offset: 0x000DDFD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitLocalActivationCommands(Action<EntityActivationCommand> _addCallback)
	{
		_addCallback(new EntityActivationCommand("talk", "talk", null, null));
		_addCallback(new EntityActivationCommand("service", "service", null, null));
		_addCallback(new EntityActivationCommand("repair", "wrench", null, null));
		_addCallback(new EntityActivationCommand("lock", "lock", null, null));
		_addCallback(new EntityActivationCommand("unlock", "unlock", null, null));
		_addCallback(new EntityActivationCommand("storage", "loot_sack", null, null));
		_addCallback(new EntityActivationCommand("keypad", "keypad", null, null));
		_addCallback(new EntityActivationCommand("take", "hand", null, null));
		_addCallback(new EntityActivationCommand("drone_command_stay", "run_and_gun", null, null));
		_addCallback(new EntityActivationCommand("drone_command_follow", "run", null, null));
		_addCallback(new EntityActivationCommand("drone_dont_heal_allies", "player", null, null));
		_addCallback(new EntityActivationCommand("drone_heal_allies", "allies", null, null));
		_addCallback(new EntityActivationCommand("drone_light_on", "lightbulb", null, null));
		_addCallback(new EntityActivationCommand("drone_light_off", "electric_switch", null, null));
		_addCallback(new EntityActivationCommand("drone_silent_on", "stealth", null, "drone_silent"));
		_addCallback(new EntityActivationCommand("drone_silent_off", "sight", null, "drone_silent"));
		_addCallback(new EntityActivationCommand("drone_command_heal", "cardio", null, null));
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x000DFF70 File Offset: 0x000DE170
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ReorderActivationCommands(List<EntityActivationCommand> _commands)
	{
		if (this.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			Entity.MoveActivationCommandAfter(_commands, "storage", "heal");
		}
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x000DFF90 File Offset: 0x000DE190
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (this.IsDead())
		{
			return false;
		}
		if (this.belongsToPlayerId(_playerFocusing.entityId))
		{
			if (base.CommandIs(_commandName, "talk"))
			{
				return this.state != EntityDrone.State.Shutdown;
			}
			if (base.CommandIs(_commandName, "service"))
			{
				return true;
			}
			if (base.CommandIs(_commandName, "repair"))
			{
				return (float)this.Health < base.Stats.Health.Max;
			}
			if (base.CommandIs(_commandName, "lock"))
			{
				return !this.isLocked;
			}
			if (base.CommandIs(_commandName, "unlock"))
			{
				return this.isLocked;
			}
			if (base.CommandIs(_commandName, "keypad"))
			{
				return true;
			}
			if (base.CommandIs(_commandName, "take"))
			{
				return true;
			}
			if (base.CommandIs(_commandName, "drone_command_stay"))
			{
				return this.OrderState != EntityDrone.Orders.Stay && this.state != EntityDrone.State.Shutdown;
			}
			if (base.CommandIs(_commandName, "drone_command_follow"))
			{
				return this.OrderState != EntityDrone.Orders.Follow && this.state != EntityDrone.State.Shutdown;
			}
			if (base.CommandIs(_commandName, "drone_command_heal"))
			{
				return this.state != EntityDrone.State.Shutdown && this.TargetCanBeHealed(_playerFocusing);
			}
			if (base.CommandIs(_commandName, "storage"))
			{
				return this.bag != null;
			}
			if (base.CommandIs(_commandName, "drone_silent_on"))
			{
				return !this.isQuietMode;
			}
			if (base.CommandIs(_commandName, "drone_silent_off"))
			{
				return this.isQuietMode;
			}
			if (base.CommandIs(_commandName, "drone_light_on"))
			{
				return this.IsFlashlightAttached && !this.IsFlashlightOn;
			}
			if (base.CommandIs(_commandName, "drone_light_off"))
			{
				return this.IsFlashlightAttached && this.IsFlashlightOn;
			}
			if (base.CommandIs(_commandName, "drone_dont_heal_allies"))
			{
				return this.IsHealModAttached && this.allyHealMode == EntityDrone.AllyHealMode.HealAllies;
			}
			if (base.CommandIs(_commandName, "drone_heal_allies"))
			{
				return this.IsHealModAttached && this.allyHealMode == EntityDrone.AllyHealMode.DoNotHeal;
			}
			if (base.CommandIs(_commandName, "drone_attack_mode_passive"))
			{
				return this.IsWeaponAttached && this.attackMode == EntityDrone.AttackMode.Aggressive && GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
			}
			if (base.CommandIs(_commandName, "drone_attack_mode_aggressive"))
			{
				return this.IsWeaponAttached && this.attackMode == EntityDrone.AttackMode.Passive && GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
			}
			return base.AllowActivationCommand(_commandName, _playerFocusing);
		}
		else
		{
			bool flag = this.isLocked && !this.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier) && this.HasPassword();
			bool flag2 = (float)this.Health < base.Stats.Health.Max;
			if (base.CommandIs(_commandName, "storage"))
			{
				return this.bag != null;
			}
			if (base.CommandIs(_commandName, "keypad"))
			{
				return flag;
			}
			return base.CommandIs(_commandName, "repair") && flag2;
		}
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x000E0260 File Offset: 0x000DE460
	public override string GetActivationText()
	{
		GameManager instance = GameManager.Instance;
		EntityPlayerLocal entityPlayerLocal;
		if (instance == null)
		{
			entityPlayerLocal = null;
		}
		else
		{
			World world = instance.World;
			entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		}
		EntityPlayerLocal entityPlayerLocal2 = entityPlayerLocal;
		if (entityPlayerLocal2 == null)
		{
			return string.Empty;
		}
		string text = entityPlayerLocal2.playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + entityPlayerLocal2.playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		string text2 = string.Format(Localization.Get("npcTooltipTalk", false, null), text, this.LocalizedEntityName);
		if (this.IsLocked() && !this.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			text2 = Localization.Get("ttLocked", false, null) + "\n" + text2;
		}
		return text2;
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x000E0314 File Offset: 0x000DE514
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityActivated(EntityActivationCommand _command, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_command.commandId, "storage") && this.isLocked && !this.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			this.playSound("locked", 1f);
			return;
		}
		this.transientLockCommand = _command.commandId.ToString();
		LockManager.Instance.LockRequestLocal(this, 0);
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000E037C File Offset: 0x000DE57C
	public void StopUIInteraction()
	{
		this.stopInteraction(234);
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000E038C File Offset: 0x000DE58C
	[PublicizedFrom(EAccessModifier.Private)]
	public void startInteraction(ReadOnlySpan<char> _commandName)
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (base.CommandIs(_commandName, "talk"))
		{
			this.startDialog(primaryPlayer);
			return;
		}
		if (base.CommandIs(_commandName, "service"))
		{
			((XUiC_DroneWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow(XUiC_DroneWindowGroup.ID)).Controller).CurrentVehicleEntity = this;
			uiforPrimaryPlayer.windowManager.Open(XUiC_DroneWindowGroup.ID, true);
			Manager.BroadcastPlayByLocalPlayer(this.position, "UseActions/service_vehicle");
			this.playVO("drone_command", true, 1f);
			return;
		}
		if (base.CommandIs(_commandName, "repair"))
		{
			this.DoRepairAction(uiforPrimaryPlayer);
			this.stopInteraction(0);
			return;
		}
		if (base.CommandIs(_commandName, "lock"))
		{
			this.playSound("locking", 1f);
			this.isLocked = !this.isLocked;
			this.stopInteraction(2);
			return;
		}
		if (base.CommandIs(_commandName, "unlock"))
		{
			this.playSound("unlocking", 1f);
			this.isLocked = !this.isLocked;
			this.stopInteraction(2);
			return;
		}
		if (base.CommandIs(_commandName, "keypad"))
		{
			this.doKeypadAction(uiforPrimaryPlayer);
			return;
		}
		if (base.CommandIs(_commandName, "take") || base.CommandIs(_commandName, "force_pickup"))
		{
			this.pickup(primaryPlayer);
			return;
		}
		if (base.CommandIs(_commandName, "drone_command_stay") || base.CommandIs(_commandName, "drone_command_follow"))
		{
			this.ToggleOrderState();
			this.stopInteraction(0);
			return;
		}
		if (base.CommandIs(_commandName, "drone_command_heal"))
		{
			this.HealRequest();
			this.stopInteraction(0);
			return;
		}
		if (base.CommandIs(_commandName, "storage"))
		{
			this.openStorageWindow(uiforPrimaryPlayer);
			this.playVO("drone_command", true, 1f);
			return;
		}
		if (base.CommandIs(_commandName, "drone_silent_on") || base.CommandIs(_commandName, "drone_silent_off"))
		{
			this.ToggleQuietMode();
			return;
		}
		if (base.CommandIs(_commandName, "drone_light_on") || base.CommandIs(_commandName, "drone_light_off"))
		{
			this.ToggleLightAction();
			this.stopInteraction(0);
			return;
		}
		if (base.CommandIs(_commandName, "drone_heal_allies") || base.CommandIs(_commandName, "drone_dont_heal_allies"))
		{
			this.ToggleHealAllies();
			this.stopInteraction(0);
			return;
		}
		if (base.CommandIs(_commandName, "drone_attack_mode_passive") || base.CommandIs(_commandName, "drone_attack_mode_aggressive"))
		{
			this.ToggleAttackMode();
			this.stopInteraction(0);
			return;
		}
		this.stopInteraction(0);
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x000E0601 File Offset: 0x000DE801
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopInteraction(ushort syncFlags = 0)
	{
		if (syncFlags != 0)
		{
			this.SendSyncData(syncFlags);
		}
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x000E0618 File Offset: 0x000DE818
	public void OpenStorageFromDialog(Entity _entityFocusing)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_entityFocusing as EntityPlayerLocal);
		this.openStorageWindow(uiforPlayer);
		this.transientLockCommand = "storage";
		LockManager.Instance.LockRequestLocal(this, 0);
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x000E0650 File Offset: 0x000DE850
	public void ToggleOrderState()
	{
		EntityDrone.Orders orders = this.orderState;
		if (orders == EntityDrone.Orders.Follow)
		{
			this.SentryMode();
			return;
		}
		if (orders != EntityDrone.Orders.Stay)
		{
			return;
		}
		this.FollowMode();
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x000E0679 File Offset: 0x000DE879
	public void ToggleHealAllies()
	{
		this.playVO("drone_command", true, 1f);
		this.setHealAllies(!this.IsHealingAllies);
		this.SendSyncData(256);
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x000E06A8 File Offset: 0x000DE8A8
	public void ToggleAttackMode()
	{
		this.playVO("drone_command", true, 1f);
		EntityDrone.AttackMode attackMode = this.attackMode;
		if (attackMode == EntityDrone.AttackMode.Passive)
		{
			this.SetAttacKMode(EntityDrone.AttackMode.Aggressive);
			return;
		}
		if (attackMode != EntityDrone.AttackMode.Aggressive)
		{
			return;
		}
		this.SetAttacKMode(EntityDrone.AttackMode.Passive);
	}

	// Token: 0x06002493 RID: 9363 RVA: 0x000E06E4 File Offset: 0x000DE8E4
	public void ToggleLightAction()
	{
		this.IsFlashlightOn = !this.IsFlashlightOn;
		this.setFlashlightOn(this.IsFlashlightOn);
		this.SendSyncData(64);
	}

	// Token: 0x06002494 RID: 9364 RVA: 0x000E070C File Offset: 0x000DE90C
	public void ToggleQuietMode()
	{
		this.isQuietMode = !this.isQuietMode;
		this.playVO("drone_command", true, 1f);
		Handle handle = this.idleLoop;
		if (handle != null)
		{
			handle.Stop(this.entityId);
		}
		this.idleLoop = null;
		this.stopInteraction(32);
	}

	// Token: 0x06002495 RID: 9365 RVA: 0x000E0760 File Offset: 0x000DE960
	public void DoRepairAction(LocalPlayerUI playerUI)
	{
		string text = "resourceRepairKit";
		if (this.HasStoredItem(playerUI.entityPlayer, text, EntityDrone.repairKitTags))
		{
			if (this.GetRepairAmountNeeded() > 0)
			{
				playerUI.xui.CollectedItemList.RemoveItemStack(new ItemStack(ItemClass.GetItem(text, false), 1));
				this.playSound("craft_repair_item", 1f);
				this.TakeStoredItem(playerUI.entityPlayer, text, EntityDrone.repairKitTags);
				this.performRepair();
				this.SendSyncData(16);
				return;
			}
		}
		else
		{
			Manager.PlayInsidePlayerHead("misc/missingitemtorepair", -1, 0f, false, false);
		}
	}

	// Token: 0x06002496 RID: 9366 RVA: 0x000E07F4 File Offset: 0x000DE9F4
	public void HealRequest()
	{
		if (!this.healWeapon.hasHealingItem())
		{
			GameManager.ShowTooltip(this.Owner as EntityPlayerLocal, Localization.Get("xuiDroneNeedsHealItemsStored", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			this.playSound("drone_empty", 1f);
			return;
		}
		if (this.state != EntityDrone.State.Heal && this.healWeapon.canFire())
		{
			this.userRequestedHeal = true;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.healTargetServer(this.Owner, this.userRequestedHeal);
			}
			else
			{
				this.healRequestClient();
			}
			this.userRequestedHeal = false;
		}
	}

	// Token: 0x06002497 RID: 9367 RVA: 0x000E0897 File Offset: 0x000DEA97
	public void ProcessDialog(string _dialogId)
	{
		if (!string.IsNullOrEmpty(_dialogId) && _dialogId.EqualsCaseInsensitive("trader_response_nevermind"))
		{
			this.playVO("drone_command", true, 1f);
		}
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x000E08C0 File Offset: 0x000DEAC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void startDialog(Entity _entityFocusing)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_entityFocusing as EntityPlayerLocal);
		uiforPlayer.xui.Dialog.Respondent = this;
		uiforPlayer.windowManager.CloseAllOpenModalWindows(null, false);
		XUiC_DialogWindowGroup.Open(uiforPlayer.xui, new Action(this.StopUIInteraction));
		this.playVO("drone_greeting", false, 1f);
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x000E0920 File Offset: 0x000DEB20
	[PublicizedFrom(EAccessModifier.Private)]
	public void openStorageWindow(LocalPlayerUI playerUI)
	{
		XUiC_BagStorageWindowGroup.Open(playerUI.xui, this, this.bag, LootContainer.GetLootContainer("roboticDrone", true), Localization.Get("xuiStorage", false, null), delegate
		{
			this.SendSyncData(8);
		}, new Action(this.StopUIInteraction), delegate
		{
			float distanceSq = this.GetDistanceSq(playerUI.entityPlayer);
			float num = Constants.cDigAndBuildDistance + 1f;
			return distanceSq <= num * num;
		}, false);
	}

	// Token: 0x0600249A RID: 9370 RVA: 0x000E0994 File Offset: 0x000DEB94
	[PublicizedFrom(EAccessModifier.Private)]
	public void doKeypadAction(LocalPlayerUI playerUI)
	{
		XUiC_KeypadWindow.Open(playerUI, this, new Action(this.StopUIInteraction), delegate
		{
			float distanceSq = this.GetDistanceSq(playerUI.entityPlayer);
			float num = Constants.cDigAndBuildDistance + 1f;
			return distanceSq <= num * num;
		});
	}

	// Token: 0x0600249B RID: 9371 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnWakeUp()
	{
	}

	// Token: 0x0600249C RID: 9372 RVA: 0x000E09D9 File Offset: 0x000DEBD9
	public void SetItemValueToLoad(ItemValue itemValue)
	{
		this.itemvalueToLoad = itemValue.Clone();
	}

	// Token: 0x0600249D RID: 9373 RVA: 0x000E09E8 File Offset: 0x000DEBE8
	public void LoadMods()
	{
		Vector2i size = LootContainer.GetLootContainer("roboticDrone", true).size;
		int num = size.x * size.y;
		this.lightManager.DisableMaterials("junkDroneLamp");
		GameObject gameObject = base.transform.FindInChilds("freightBox", false).gameObject;
		GameObject gameObject2 = base.transform.FindInChilds("armor", false).gameObject;
		GameObject gameObject3 = base.transform.FindInChilds("machineGun", false).gameObject;
		GameObject gameObject4 = base.transform.FindInChilds("teddyBear", false).gameObject;
		GameObject gameObject5 = base.transform.FindInChilds("junkDroneArmRight", false).gameObject;
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
		}
		if (gameObject3 != null)
		{
			gameObject3.SetActive(false);
		}
		if (gameObject4 != null)
		{
			gameObject4.SetActive(false);
		}
		if (gameObject5 != null)
		{
			gameObject5.SetActive(true);
		}
		for (int i = 0; i < this.installedWeapons.Count; i++)
		{
			DroneWeapons.Weapon weapon = this.installedWeapons[i];
			this.installedWeapons.Remove(weapon);
			weapon.Unequip();
		}
		this.stunWeapon = null;
		this.healWeapon = null;
		if (this.state == EntityDrone.State.Attack)
		{
			this.SetState(EntityDrone.State.Idle, true);
		}
		this.IsFlashlightAttached = false;
		this.setFlashlightOn(false);
		if (this.OriginalItemValue.HasMods())
		{
			for (int j = 0; j < this.OriginalItemValue.ModificationCount; j++)
			{
				ItemValue modification = this.OriginalItemValue.GetModification(j);
				if (((modification != null) ? modification.ItemClass : null) != null)
				{
					string name = modification.ItemClass.Name;
					uint num2 = <PrivateImplementationDetails>.ComputeStringHash(name);
					if (num2 <= 2400030839U)
					{
						if (num2 != 1912183181U)
						{
							if (num2 != 2266484491U)
							{
								if (num2 == 2400030839U)
								{
									if (name == "modRoboticDroneWeaponMod")
									{
										if (gameObject3)
										{
											gameObject3.SetActive(true);
										}
									}
								}
							}
							else if (name == "modRoboticDroneMoraleBoosterMod")
							{
								this.isSupportModAttached = true;
								if (gameObject4)
								{
									gameObject4.SetActive(true);
								}
							}
						}
						else if (name == "modRoboticDroneCargoMod")
						{
							num += 8;
							if (gameObject)
							{
								gameObject.SetActive(true);
							}
						}
					}
					else if (num2 <= 3474526689U)
					{
						if (num2 != 2404831999U)
						{
							if (num2 == 3474526689U)
							{
								if (name == "modRoboticDroneArmorPlatingMod")
								{
									this.armorDamageReduction = 0.5f;
									if (gameObject2)
									{
										gameObject2.SetActive(true);
									}
								}
							}
						}
						else if (name == "modRoboticDroneMedicMod")
						{
							this.healWeapon = new DroneWeapons.HealBeamWeapon(this);
							this.healWeapon.Init();
							this.healWeapon.Equip(modification);
							this.installedWeapons.Add(this.healWeapon);
						}
					}
					else if (num2 != 3914512375U)
					{
						if (num2 == 4027736419U)
						{
							if (name == "modRoboticDroneStunWeaponMod")
							{
								this.stunWeapon = new DroneWeapons.StunBeamWeapon(this);
								this.stunWeapon.Init();
								this.stunWeapon.Equip(modification);
								this.installedWeapons.Add(this.stunWeapon);
								if (gameObject3)
								{
									gameObject3.SetActive(true);
								}
								if (gameObject5)
								{
									gameObject5.SetActive(false);
								}
							}
						}
					}
					else if (name == "modRoboticDroneHeadlampMod")
					{
						this.IsFlashlightAttached = true;
						DroneLightManager.LightEffect[] lightEffects = this.lightManager.LightEffects;
						if (lightEffects.Length != 0)
						{
							LightManager.RegisterMovingLight(this, lightEffects[0].linkedObjects[0].GetComponent<Light>());
						}
						if (this.IsFlashlightOn)
						{
							this.setFlashlightOn(true);
						}
					}
				}
			}
		}
		if (this.bag.SlotCount != num)
		{
			this.bag.Resize(new Vector2i(num, 1));
		}
		Color color = this.prefabColor;
		ItemValue cosmeticMod = this.OriginalItemValue.GetCosmeticMod(0);
		if (this.OriginalItemValue.CosmeticModCount > 0 && cosmeticMod != null && !cosmeticMod.IsEmpty())
		{
			Vector3 vector = Block.StringToVector3(this.OriginalItemValue.GetPropertyOverride(Block.PropTintColor, "255,255,255"));
			color.r = vector.x;
			color.g = vector.y;
			color.b = vector.z;
		}
		for (int k = 0; k < this.paintableParts.Length; k++)
		{
			this.SetPaint(this.paintableParts[k], color);
		}
	}

	// Token: 0x0600249E RID: 9374 RVA: 0x000E0EA7 File Offset: 0x000DF0A7
	[PublicizedFrom(EAccessModifier.Private)]
	public void initWorldValues(bool value)
	{
		this.bWillRespawn = value;
	}

	// Token: 0x0600249F RID: 9375 RVA: 0x000E0EB0 File Offset: 0x000DF0B0
	public void SyncOwnerData()
	{
		if (this.isOwnerSyncPending)
		{
			this.notifySyncOwner();
			this.isOwnerSyncPending = false;
		}
		if (this.belongsPlayerId == -1)
		{
			PersistentPlayerList persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
			if (persistentPlayerList != null)
			{
				PersistentPlayerData playerData = persistentPlayerList.GetPlayerData(this.OwnerID);
				if (playerData != null)
				{
					this.belongsPlayerId = playerData.EntityId;
				}
			}
		}
		if (!this.Owner)
		{
			this.Owner = (EntityAlive)GameManager.Instance.World.GetEntity(this.belongsPlayerId);
			if (this.Owner)
			{
				this.Owner.AddOwnedEntity(this);
				if (!this.hasNavObjectsEnabled && GameManager.Instance.World.IsLocalPlayer(this.belongsPlayerId))
				{
					this.HandleNavObject();
					this.hasNavObjectsEnabled = true;
				}
			}
		}
	}

	// Token: 0x060024A0 RID: 9376 RVA: 0x000E0F78 File Offset: 0x000DF178
	[PublicizedFrom(EAccessModifier.Private)]
	public void notifySyncOwner()
	{
		PersistentPlayerData playerData = GameManager.Instance.GetPersistentPlayerList().GetPlayerData(this.OwnerID);
		if (playerData != null)
		{
			this.belongsPlayerId = playerData.EntityId;
			this.Owner = GameManager.Instance.World.GetEntity(this.belongsPlayerId) as EntityAlive;
		}
		if (this.Owner)
		{
			this.rotation = Quaternion.LookRotation(this.Owner.position - this.position).eulerAngles;
			if (GameManager.Instance.World.IsLocalPlayer(this.belongsPlayerId))
			{
				this.HandleNavObject();
				this.hasNavObjectsEnabled = true;
				this.SetOwner(this.OwnerID);
				this.SendSyncData(3);
			}
		}
	}

	// Token: 0x060024A1 RID: 9377 RVA: 0x000E1037 File Offset: 0x000DF237
	[PublicizedFrom(EAccessModifier.Private)]
	public bool belongsToPlayerId(int id)
	{
		return this.belongsPlayerId == id;
	}

	// Token: 0x060024A2 RID: 9378 RVA: 0x000E1042 File Offset: 0x000DF242
	public Color GetPaintColor()
	{
		return base.transform.FindRecursive("BaseMesh").GetComponentInChildren<Renderer>().sharedMaterial.color;
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x000E1064 File Offset: 0x000DF264
	public void SetPaint(string childName, Color color)
	{
		Transform transform = base.transform.FindRecursive(childName);
		if (transform && transform.gameObject.activeSelf)
		{
			Renderer[] componentsInChildren = transform.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.color = color;
			}
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x060024A4 RID: 9380 RVA: 0x000E10B6 File Offset: 0x000DF2B6
	// (set) Token: 0x060024A5 RID: 9381 RVA: 0x000E10BE File Offset: 0x000DF2BE
	public bool PlayWakeupAnim { get; set; }

	// Token: 0x060024A6 RID: 9382 RVA: 0x000E10C8 File Offset: 0x000DF2C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void playWakeupAnim()
	{
		this.animator = base.GetComponentInChildren<Animator>();
		if (this.animator)
		{
			this.animator.enabled = true;
			this.animator.Play("Base Layer.SpawnIn");
			this.WakeupAnimTime = 2.5f;
		}
	}

	// Token: 0x060024A7 RID: 9383 RVA: 0x000E1118 File Offset: 0x000DF318
	[PublicizedFrom(EAccessModifier.Private)]
	public void playIdleAnim()
	{
		this.animator = base.GetComponentInChildren<Animator>();
		if (this.animator)
		{
			this.animator.enabled = true;
			this.animator.Play("Base Layer.Idle", 0, 0f);
			this.animator.Update(0f);
		}
	}

	// Token: 0x060024A8 RID: 9384 RVA: 0x000E1170 File Offset: 0x000DF370
	[PublicizedFrom(EAccessModifier.Private)]
	public void setShutdownAnim()
	{
		this.animator = base.GetComponentInChildren<Animator>();
		if (this.animator)
		{
			this.animator.Play("Base Layer.SpawnIn", 0, 0f);
			this.animator.Update(0f);
			this.animator.StopPlayback();
			this.animator.enabled = false;
		}
	}

	// Token: 0x060024A9 RID: 9385 RVA: 0x000E11D4 File Offset: 0x000DF3D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAnimStates()
	{
		if (!this.isAnimationStateSet)
		{
			if (this.Health > 1 && this.Owner)
			{
				if (this.PlayWakeupAnim)
				{
					this.playWakeupAnim();
					this.PlayWakeupAnim = false;
				}
				else
				{
					this.playIdleAnim();
					this.WakeupAnimTime = 2.5f;
				}
			}
			else
			{
				this.setShutdownAnim();
			}
			this.isAnimationStateSet = true;
		}
		if (this.WakeupAnimTime > 0f)
		{
			this.WakeupAnimTime -= 0.05f;
			if (this.WakeupAnimTime <= 0f && !GameManager.IsDedicatedServer)
			{
				if (this.Owner)
				{
					Manager.Stop(this.Owner.entityId, "drone_take");
				}
				this.playVO("drone_wakeup", false, 1f);
			}
		}
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x000E129C File Offset: 0x000DF49C
	[PublicizedFrom(EAccessModifier.Private)]
	public void unRegsiterMovingLights()
	{
		DroneLightManager.LightEffect[] lightEffects = this.lightManager.LightEffects;
		if (lightEffects.Length != 0)
		{
			LightManager.UnRegisterMovingLight(this, lightEffects[0].linkedObjects[0].GetComponent<Light>());
		}
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x000E12CE File Offset: 0x000DF4CE
	[PublicizedFrom(EAccessModifier.Private)]
	public void setFlashlightOn(bool value)
	{
		if (value)
		{
			this.lightManager.InitMaterials("junkDroneLamp");
			return;
		}
		this.lightManager.DisableMaterials("junkDroneLamp");
	}

	// Token: 0x060024AC RID: 9388 RVA: 0x000E12F4 File Offset: 0x000DF4F4
	public void BroadcastPlayVO(string sound_path, bool _hasPriority = false, float _vol = 1f)
	{
		if (!this.isQuietMode && this.initSuppressVOTimer <= 0f && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageAudioPlayInHead>().Setup(sound_path, true), false, this.Owner.entityId, -1, -1, null, 192, false);
			if (!GameManager.IsDedicatedServer)
			{
				this.playVO(sound_path, _hasPriority, _vol);
			}
		}
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x000E1365 File Offset: 0x000DF565
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(string sound_path, float _vol = 1f)
	{
		this.playSound(this, sound_path, false, false, _vol);
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x000E1374 File Offset: 0x000DF574
	[PublicizedFrom(EAccessModifier.Private)]
	public void playSound(Entity entity, string sound_path, bool _isVO = false, bool _hasPriority = false, float _vol = 1f)
	{
		if (!this.isQuietMode)
		{
			if (_isVO)
			{
				if (_hasPriority)
				{
					Handle handle = this.voHandle;
					if (handle != null)
					{
						handle.Stop(this.entityId);
					}
				}
				this.voHandle = Manager.Play(entity, sound_path, _vol, true);
				return;
			}
			Manager.Play(entity, sound_path, _vol, false);
		}
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x000E13C3 File Offset: 0x000DF5C3
	[PublicizedFrom(EAccessModifier.Private)]
	public Handle playSoundLoop(string sound_path, float _vol = 1f)
	{
		return Manager.Play(this, sound_path, _vol, true);
	}

	// Token: 0x060024B0 RID: 9392 RVA: 0x000E13CE File Offset: 0x000DF5CE
	[PublicizedFrom(EAccessModifier.Private)]
	public void playVO(string sound_path, bool _hasPriority = false, float _vol = 1f)
	{
		if (GameManager.Instance.World.IsLocalPlayer(this.belongsPlayerId))
		{
			this.playSound(this, sound_path, true, _hasPriority, _vol);
		}
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x000E13F4 File Offset: 0x000DF5F4
	public bool isAlly(EntityAlive _target)
	{
		if (this.debugFriendlyFire)
		{
			return false;
		}
		if (this.Owner && this.Owner == _target)
		{
			return true;
		}
		PersistentPlayerList persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
		PersistentPlayerData playerData = persistentPlayerList.GetPlayerData(this.OwnerID);
		if (playerData != null && persistentPlayerList.EntityToPlayerMap.ContainsKey(_target.entityId))
		{
			PersistentPlayerData persistentPlayerData = persistentPlayerList.EntityToPlayerMap[_target.entityId];
			if (persistentPlayerData != null && playerData.IsAlly(persistentPlayerData))
			{
				return true;
			}
			EntityPlayer entityPlayer = this.Owner as EntityPlayer;
			EntityPlayer entityPlayer2 = _target as EntityPlayer;
			if (entityPlayer && entityPlayer2 && entityPlayer.Party != null && entityPlayer.Party.ContainsMember(entityPlayer2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x000E14B5 File Offset: 0x000DF6B5
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPartyMemberAdded(EntityPlayer player)
	{
		this.registeredPartyMembers.Add(player.entityId);
	}

	// Token: 0x060024B3 RID: 9395 RVA: 0x000E14C8 File Offset: 0x000DF6C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPartyMemberRemoved(EntityPlayer player)
	{
		this.registeredPartyMembers.Remove(player.entityId);
		this.removeSupportBuff(player);
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x000E14E4 File Offset: 0x000DF6E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void removePartyBuffs(EntityPlayer owner)
	{
		if (owner.Party != null)
		{
			for (int i = 0; i < owner.Party.MemberList.Count; i++)
			{
				EntityAlive entityAlive = owner.Party.MemberList[i];
				this.removeSupportBuff(entityAlive);
			}
		}
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000E1530 File Offset: 0x000DF730
	[PublicizedFrom(EAccessModifier.Private)]
	public void procBuffRange(EntityAlive entity)
	{
		if (entity)
		{
			if ((this.position - entity.position).magnitude < 32f)
			{
				this.addSupportBuff(entity);
				return;
			}
			this.removeSupportBuff(entity);
		}
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000E1574 File Offset: 0x000DF774
	[PublicizedFrom(EAccessModifier.Private)]
	public void addSupportBuff(EntityAlive entity)
	{
		if (this.state != EntityDrone.State.Shutdown && !entity.Buffs.HasBuff("buffJunkDroneSupportEffect"))
		{
			entity.Buffs.AddBuff("buffJunkDroneSupportEffect", -1, true, false, -1f);
		}
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x000E15AA File Offset: 0x000DF7AA
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeSupportBuff(EntityAlive entity)
	{
		if (entity && entity.Buffs.HasBuff("buffJunkDroneSupportEffect") && !this.doesEntityHaveSupport(entity))
		{
			entity.Buffs.RemoveBuff("buffJunkDroneSupportEffect", -1, true);
		}
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000E15E4 File Offset: 0x000DF7E4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool doesEntityHaveSupport(EntityAlive entity)
	{
		for (int i = 0; i < entity.ownedEntities.Count; i++)
		{
			EntityDrone entityDrone = this.world.GetEntity(entity.ownedEntities[i].Id) as EntityDrone;
			if (entityDrone && entityDrone.isSupportModAttached)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x000E163C File Offset: 0x000DF83C
	[PublicizedFrom(EAccessModifier.Private)]
	public void buffAllies()
	{
		EntityPlayer entityPlayer = this.Owner as EntityPlayer;
		if (entityPlayer)
		{
			if (entityPlayer.Party != null)
			{
				this.knownPartyMembers = entityPlayer.Party.GetMemberIdArray();
				for (int i = 0; i < this.knownPartyMembers.Length; i++)
				{
					EntityAlive entityAlive = this.world.GetEntity(this.knownPartyMembers[i]) as EntityAlive;
					this.procBuffRange(entityAlive);
				}
				return;
			}
			if (this.knownPartyMembers != null && this.knownPartyMembers.Length != 0)
			{
				for (int j = 0; j < this.knownPartyMembers.Length; j++)
				{
					EntityAlive entityAlive2 = this.world.GetEntity(this.knownPartyMembers[j]) as EntityAlive;
					this.removeSupportBuff(entityAlive2);
				}
				this.knownPartyMembers = null;
			}
			this.procBuffRange(entityPlayer);
		}
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000E1701 File Offset: 0x000DF901
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePartyBuffs()
	{
		if (this.Owner && this.isSupportModAttached && !this.isEntityRemote)
		{
			this.buffAllies();
		}
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000E1728 File Offset: 0x000DF928
	public bool HasStoredItem(EntityAlive entity, string itemGroupOrName, FastTags<TagGroup.Global> fastTags)
	{
		ItemValue item = ItemClass.GetItem(itemGroupOrName, false);
		bool itemClass = item.ItemClass != null;
		int num = 0;
		int num2 = 0;
		if (itemClass)
		{
			num = entity.bag.GetItemCount(item, -1, -1, true);
			num2 = entity.inventory.GetItemCount(item, false, -1, -1, true);
		}
		return num + num2 > 0;
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x000E1770 File Offset: 0x000DF970
	public ItemStack TakeStoredItem(EntityAlive entity, string itemGroupOrName, FastTags<TagGroup.Global> fastTags)
	{
		ItemValue item = ItemClass.GetItem(itemGroupOrName, false);
		if (item.ItemClass != null)
		{
			entity.bag.GetItemCount(item, -1, -1, true);
			if (entity.inventory.GetItemCount(item, false, -1, -1, true) > 0)
			{
				entity.inventory.DecItem(item, 1, false, null);
			}
			else
			{
				entity.bag.DecItem(item, 1, false, null);
			}
			return new ItemStack(item.Clone(), 1);
		}
		return null;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x000E17E0 File Offset: 0x000DF9E0
	public ItemValue GetUpdatedItemValue()
	{
		this.OriginalItemValue.UseTimes = (float)this.OriginalItemValue.MaxUseTimes * (1f - (float)this.Health / base.Stats.Health.BaseMax);
		return this.OriginalItemValue;
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x000E1820 File Offset: 0x000DFA20
	public override void OnCollectServer(int _playerId)
	{
		this.OriginalItemValue = this.GetUpdatedItemValue();
		base.transform.gameObject.SetActive(false);
		if (this.Owner)
		{
			this.Owner.RemoveOwnedEntity(this.entityId);
			if (DroneManager.Instance != null)
			{
				DroneManager.Instance.RemoveTrackedDrone(this, EnumRemoveEntityReason.Despawned);
			}
		}
		this.world.RemoveEntity(this.entityId, EnumRemoveEntityReason.Killed);
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x000E1890 File Offset: 0x000DFA90
	public override void OnCollectLocal(int _playerId)
	{
		EntityPlayerLocal entityPlayerLocal = this.world.GetEntity(_playerId) as EntityPlayerLocal;
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		ItemStack itemStack = new ItemStack(this.GetUpdatedItemValue(), 1);
		if (!uiforPlayer.xui.PlayerInventory.Toolbelt.AddItem(itemStack) && !uiforPlayer.xui.PlayerInventory.AddItem(itemStack))
		{
			GameManager.Instance.ItemDropServer(itemStack, entityPlayerLocal.GetPosition(), Vector3.zero, _playerId, 60f, false);
		}
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x000E190C File Offset: 0x000DFB0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void pickup(Entity _entityFocusing)
	{
		if (!this.bag.IsEmpty())
		{
			this.playVO("drone_takefail", true, 1f);
			GameManager.ShowTooltip(this.Owner as EntityPlayerLocal, Localization.Get("ttEmptyDroneBeforePickup", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			this.stopInteraction(0);
			return;
		}
		ItemStack itemStack = new ItemStack(this.GetUpdatedItemValue(), 1);
		EntityPlayer entityPlayer = _entityFocusing as EntityPlayer;
		if (entityPlayer.inventory.CanTakeItem(itemStack) || entityPlayer.bag.CanTakeItem(itemStack))
		{
			this.isBeingPickedUp = true;
			this.playSound(entityPlayer, "drone_take", true, true, 1f);
			this.initWorldValues(false);
			this.nativeCollider.enabled = false;
			base.Collect(entityPlayer.entityId);
			if (entityPlayer.Buffs.HasBuff("buffJunkDroneSupportEffect"))
			{
				entityPlayer.Buffs.RemoveBuff("buffJunkDroneSupportEffect", -1, true);
			}
			this.removePartyBuffs(entityPlayer);
			this.unRegsiterMovingLights();
		}
		else
		{
			GameManager.ShowTooltip(entityPlayer as EntityPlayerLocal, Localization.Get("xuiInventoryFullForPickup", false, null), string.Empty, "ui_denied", null, false, false, 0f);
		}
		this.stopInteraction(0);
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x060024C1 RID: 9409 RVA: 0x000E1A39 File Offset: 0x000DFC39
	public int StorageCapacity
	{
		get
		{
			return this.bag.SlotCount;
		}
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x000E1A46 File Offset: 0x000DFC46
	public int GetStoredItemCount()
	{
		return this.bag.GetUsedSlotCount();
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x000E1A53 File Offset: 0x000DFC53
	public bool CanRemoveExtraStorage()
	{
		return this.GetStoredItemCount() < this.StorageCapacity - 8;
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x000E1A68 File Offset: 0x000DFC68
	public void NotifyToManyStoredItems()
	{
		if (this.overItemLimitCooldown > 0f)
		{
			return;
		}
		this.overItemLimitCooldown = 5f;
		if (!this.CanRemoveExtraStorage())
		{
			GameManager.ShowTooltip(this.Owner as EntityPlayerLocal, Localization.Get("ttJunkDroneEmptySomeStorage", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x000E1AC5 File Offset: 0x000DFCC5
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateDroneServiceMenu()
	{
		if (this.overItemLimitCooldown > 0f)
		{
			this.overItemLimitCooldown -= 0.05f;
		}
	}

	// Token: 0x060024C6 RID: 9414 RVA: 0x000E1AE6 File Offset: 0x000DFCE6
	public int GetRepairAmountNeeded()
	{
		return this.GetMaxHealth() - this.Health;
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x000E1AF5 File Offset: 0x000DFCF5
	public void RepairParts(int _amount)
	{
		this.Health += _amount;
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x000E1B08 File Offset: 0x000DFD08
	[PublicizedFrom(EAccessModifier.Private)]
	public void performRepair()
	{
		this.Health = (int)base.Stats.Health.Max;
		this.OriginalItemValue.UseTimes = 0f;
		this.setShutdown(false);
		this.playWakeupAnim();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.SendSyncData(16);
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x060024C9 RID: 9417 RVA: 0x000E1B5D File Offset: 0x000DFD5D
	public float EnemyDetectionRadius
	{
		get
		{
			return this.sensors.EnemyDetectionRadius;
		}
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x000E1B6A File Offset: 0x000DFD6A
	public EntityAlive GetNearestEnemyInRange(Vector3 targetPos)
	{
		return this.sensors.GetNearestEnemyInRange(targetPos);
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x000E1B78 File Offset: 0x000DFD78
	public bool IsOwnerSneaking()
	{
		return this.Owner && this.Owner.IsCrouching && !this.sensors.IsOwnerAttackTarget();
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x000E1BA4 File Offset: 0x000DFDA4
	public EntityDrone.State GetState()
	{
		return this.state;
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000E1BAC File Offset: 0x000DFDAC
	public void SetState(EntityDrone.State next, bool sync = false)
	{
		this.setState(next);
		if (sync)
		{
			this.SendSyncData(32768);
		}
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x000E1BC4 File Offset: 0x000DFDC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setState(EntityDrone.State next)
	{
		this.lastState = this.state;
		this.state = next;
		this.stateTime = 0f;
		switch (this.state)
		{
		case EntityDrone.State.Idle:
		case EntityDrone.State.Sentry:
			break;
		case EntityDrone.State.Follow:
			if (this.lastState == EntityDrone.State.Sentry && this.Owner && this.Owner.HasOwnedEntity(this.entityId))
			{
				this.Owner.GetOwnedEntity(this.entityId).ClearLastKnownPostition();
				return;
			}
			break;
		case EntityDrone.State.Heal:
			this.clearNeedsHealItemCheck();
			break;
		default:
			return;
		}
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x000E1C54 File Offset: 0x000DFE54
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateState()
	{
		this.stateTime += 0.05f;
		switch (this.state)
		{
		case EntityDrone.State.Idle:
			this.idleState();
			return;
		case EntityDrone.State.Sentry:
			this.sentryState();
			break;
		case EntityDrone.State.Follow:
			this.followState();
			return;
		case EntityDrone.State.Heal:
			this.healState();
			return;
		case EntityDrone.State.Attack:
			this.attackState();
			return;
		case EntityDrone.State.Shutdown:
		case EntityDrone.State.NoClip:
			break;
		default:
			return;
		}
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x000E1CC0 File Offset: 0x000DFEC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTransitionState()
	{
		if (this.transitionState != EntityDrone.State.None)
		{
			if (this.state == this.transitionState)
			{
				this.transitionState = EntityDrone.State.None;
				return;
			}
			if ((this.state == EntityDrone.State.Attack || this.state == EntityDrone.State.Heal) && this.transitionState == EntityDrone.State.Idle)
			{
				for (int i = 0; i < this.installedWeapons.Count; i++)
				{
					this.installedWeapons[i].RefreshCooldown();
				}
			}
			EntityDrone.State state = this.transitionState;
			if (state != EntityDrone.State.Idle)
			{
				if (state != EntityDrone.State.Heal)
				{
					if (state == EntityDrone.State.Shutdown)
					{
						this.isShutdownPending = true;
					}
					else
					{
						this.setState(this.transitionState);
					}
				}
				else
				{
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						this.healTargetServer(base.GetAttackTarget(), this.userRequestedHeal);
					}
					else
					{
						this.setState(this.transitionState);
						this.initSuppressVOTimer = 0f;
					}
					this.userRequestedHeal = false;
				}
			}
			else if (this.state == EntityDrone.State.Shutdown)
			{
				this.setShutdown(false);
			}
			else
			{
				this.setState(this.transitionState);
			}
			this.transitionState = EntityDrone.State.None;
		}
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000E1DC0 File Offset: 0x000DFFC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void idleState()
	{
		EntityAlive owner = this.Owner;
		if (owner)
		{
			Vector3 chestPosition = owner.getChestPosition();
			if (this.onUnderWaterState(chestPosition))
			{
				return;
			}
			bool flag = this.sensors.IsEnemyInRange();
			if (flag && !this.steering.IsInRange(chestPosition, this.sensors.EnemyDetectionRadius))
			{
				this.setState(EntityDrone.State.Follow);
				return;
			}
			if (!this.steering.IsInRange(chestPosition, this.FollowDistance + 2f) && !flag)
			{
				this.setState(EntityDrone.State.Follow);
				return;
			}
			if (!this.isEntityAboveOrBelow(owner))
			{
				this.rotateTo(this.steering.GetDir2D(this.position, chestPosition));
				float num = 0f;
				if (this.position.y - chestPosition.y > num || this.position.y - chestPosition.y < num)
				{
					Vector3 position = this.position;
					position.y = chestPosition.y;
					this.move(this.steering.Seek(this.position, position, this.SpeedFlying * 0.5f), this.SpeedFlying, false);
				}
			}
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x060024D2 RID: 9426 RVA: 0x000E1EDA File Offset: 0x000E00DA
	public EntityDrone.Orders OrderState
	{
		get
		{
			return this.orderState;
		}
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x000E1EE4 File Offset: 0x000E00E4
	public void SentryMode()
	{
		this.playVO("drone_command", true, 1f);
		this.SentryPos = this.position;
		this.setOrders(EntityDrone.Orders.Stay);
		this.setState(EntityDrone.State.Sentry);
		this.SendSyncData(49152);
		if (this.Owner && this.Owner.HasOwnedEntity(this.entityId))
		{
			this.Owner.GetOwnedEntity(this.entityId).SetLastKnownPosition(this.position);
		}
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x000E1F63 File Offset: 0x000E0163
	[PublicizedFrom(EAccessModifier.Private)]
	public void setOrders(EntityDrone.Orders orders)
	{
		this.orderState = orders;
		this.initWorldValues(this.orderState == EntityDrone.Orders.Follow);
		if (GameManager.Instance.World.IsLocalPlayer(this.belongsPlayerId))
		{
			this.HandleNavObject();
		}
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x000E1F98 File Offset: 0x000E0198
	[PublicizedFrom(EAccessModifier.Private)]
	public void sentryState()
	{
		Vector3 sentryPos = this.SentryPos;
		if (this.world.IsChunkAreaLoaded(sentryPos))
		{
			if ((sentryPos - this.position).magnitude > 5f)
			{
				if (!this.DoMoveIntoFollowPos(sentryPos, 1.414f, base.transform.forward, 0.1f, true, 10f))
				{
					return;
				}
				this.clearCurrentPath();
			}
			if (!this.steering.IsInRange(sentryPos, 0.25f))
			{
				Vector3 vector = this.steering.Seek(this.position, sentryPos, 0.25f);
				this.rotateTo((sentryPos - this.position).normalized);
				this.move(vector, false);
				return;
			}
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x060024D6 RID: 9430 RVA: 0x00020119 File Offset: 0x0001E319
	public bool CanInterruptFollow
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x000E2053 File Offset: 0x000E0253
	public void FollowMode()
	{
		this.playVO("drone_command", true, 1f);
		this.setOrders(EntityDrone.Orders.Follow);
		this.setState(EntityDrone.State.Follow);
		this.SendSyncData(49152);
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x000E207F File Offset: 0x000E027F
	public bool IsAttachedToVehicle(Entity entity)
	{
		return entity && entity.AttachedToEntity as EntityVehicle != null;
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000E209C File Offset: 0x000E029C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInteractionFocusOwner()
	{
		if (this.Owner)
		{
			Ray lookRay = this.Owner.GetLookRay();
			RaycastHit raycastHit;
			if (Physics.Raycast(lookRay.origin - Origin.position, lookRay.direction, out raycastHit, 1000f, 16384) && raycastHit.transform && raycastHit.transform.tag != "Physics")
			{
				Entity component = raycastHit.transform.GetComponent<Entity>();
				if (component && component.entityClass == EntityClass.junkDroneClass)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000E213B File Offset: 0x000E033B
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onInterruptState()
	{
		if (this.isInteractionFocusOwner())
		{
			this.ownerFocusTimer += 0.05f;
			if (this.ownerFocusTimer >= 0.2f)
			{
				this.ownerFocusTimer = 0f;
				this.setState(EntityDrone.State.Idle);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000E217C File Offset: 0x000E037C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onVehicleState(EntityAlive entity, Vector3 followPoint)
	{
		if (this.IsAttachedToVehicle(entity))
		{
			if (!this.ownerIsOnVehicle)
			{
				this.ownerIsOnVehicle = true;
				this.setNoClip(true);
				this.clearCurrentPath();
			}
			Entity attachedToEntity = entity.AttachedToEntity;
			Vector3 vector = attachedToEntity.position - attachedToEntity.transform.forward * 5f * 2f + Vector3.up * 5f * 2f;
			this.steerFollow(entity, vector);
			return true;
		}
		if (this.ownerIsOnVehicle)
		{
			this.ownerIsOnVehicle = false;
			this.SetPosition(followPoint, true);
		}
		return false;
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000E2220 File Offset: 0x000E0420
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onUnderWaterState(Vector3 chestPos)
	{
		if (this.isTargetUnderWater(chestPos))
		{
			if (this.currentPath.Count > 0)
			{
				this.clearCurrentPath();
			}
			Vector3 vector = this.steering.Seek(this.position, this.findOpenBlockAbove(chestPos, 256), 0.2f);
			this.rotateTo(vector);
			this.move(vector, false);
			return true;
		}
		return false;
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x000E2280 File Offset: 0x000E0480
	[PublicizedFrom(EAccessModifier.Private)]
	public bool DoMoveIntoFollowPos(Vector3 targetPos, float seekDist, Vector3 seekForward, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		Utils.DrawCircleLinesHorzontal(this.position - Origin.position, seekDist, Color.white, Color.red, 24, 0.05f);
		Vector3 normalized = (targetPos - this.position).normalized;
		float magnitude = (targetPos - this.position).magnitude;
		if (this.currentPath.Count == 0)
		{
			EntityDrone.GetPath(this.currentPath, this, this.position, targetPos, this.SpeedFlying, null, seekDist, pointRadius, debugDraw, duration);
			if (this.currentPath.Count > 0)
			{
				this.currentPathDest = this.currentPath[this.currentPath.Count - 1];
			}
			else
			{
				this.currentPathDest = targetPos;
			}
		}
		else if (EntityDrone.IsPositionBlocked(this.position, targetPos, 1073807360, false, 0f) || (float)this.currentPath.Count > seekDist + 1f || (this.currentPath.Count > 0 && magnitude > seekDist + 1.414f))
		{
			this.followPlannedPath(this.SpeedFlying, pointRadius, debugDraw, duration);
		}
		else if (magnitude >= seekDist)
		{
			this.RotateTo(normalized);
			this.MoveTo(targetPos, pointRadius);
		}
		return !EntityDrone.IsPositionBlocked(this.position, targetPos, 1073807360, false, 0f) && magnitude <= seekDist;
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x000E23D4 File Offset: 0x000E05D4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool DoMoveIntoFollowPos(EntityAlive avaliableTarget, float seekDist, Vector3 seekForward, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		Vector3 chestPosition = avaliableTarget.getChestPosition();
		return this.DoMoveIntoFollowPos(chestPosition, seekDist, seekForward, pointRadius, debugDraw, duration);
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x000E23F8 File Offset: 0x000E05F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void steerFollow(EntityAlive entity, Vector3 followPoint)
	{
		if (entity)
		{
			if (!this.steering.IsInRange(entity.position, 10f))
			{
				if (this.decelerationTime > 0f)
				{
					this.decelerationTime = 0f;
				}
				this.accelerationTime += 0.05f;
				this.currentSpeedFlying = Mathf.Lerp(this.currentSpeedFlying, Mathf.Max(15f, (entity.position - this.position).magnitude), Mathf.Clamp01(this.accelerationTime / this.SpeedFlying));
			}
			else
			{
				if (this.accelerationTime > 0f)
				{
					this.accelerationTime = 0f;
				}
				this.decelerationTime += 0.05f;
				this.currentSpeedFlying = Mathf.Lerp(this.currentSpeedFlying, this.SpeedFlying, Mathf.Clamp01(this.decelerationTime / (this.SpeedFlying * 0.5f)));
			}
		}
		Vector3 vector = this.steering.Seek(this.position, followPoint, this.SpeedFlying);
		if (!this.steering.IsInRange(followPoint, 0.1f))
		{
			Vector3 chestPosition = entity.getChestPosition();
			float magnitude = (chestPosition - this.position).magnitude;
			if (magnitude > 5f && magnitude < 24f && !RaycastPathUtils.IsPointBlocked(this.position, chestPosition, 1073807360, false, 0f) && Vector3.Angle(entity.GetLookVector(), this.position - chestPosition) < 45f)
			{
				float num = 0.5f;
				Vector3 vector2 = this.steering.Flee(this.position, chestPosition, this.SpeedFlying);
				if (!RaycastPathUtils.IsPositionBlocked(this.position, this.position + (vector + vector2), 1073807360, false))
				{
					vector += vector2 * num;
				}
			}
			if (this.steering.GetAltitude(this.position) < magnitude * 0.33f && !RaycastPathUtils.IsPositionBlocked(this.position, this.position + Vector3.up, 1073807360, false))
			{
				float num2 = 0.75f;
				Vector3 vector3 = this.steering.Seek(this.position, this.position + Vector3.up, this.SpeedFlying);
				vector += vector3 * num2;
			}
			this.rotateTo((chestPosition - this.position).normalized);
			this.move(vector.normalized, followPoint, this.currentSpeedFlying, false);
		}
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000E2690 File Offset: 0x000E0890
	[PublicizedFrom(EAccessModifier.Private)]
	public void followState()
	{
		EntityAlive owner = this.Owner;
		if (!owner)
		{
			return;
		}
		if (this.onInterruptState())
		{
			return;
		}
		Vector3 chestPosition = owner.getChestPosition();
		if (this.onUnderWaterState(chestPosition))
		{
			return;
		}
		Vector3[] groupPositions = EntityDrone.GetGroupPositions(owner, 5f, false, 0f);
		Array.Sort<Vector3>(groupPositions, (Vector3 x, Vector3 y) => Vector3.Distance(this.position, x).CompareTo(Vector3.Distance(this.position, y)));
		Vector3 vector = groupPositions[0];
		if (this.onVehicleState(owner, vector))
		{
			return;
		}
		if (PathFinderThread.Instance != null)
		{
			if (!this.DoMoveIntoFollowPos(owner, 5f, base.transform.forward, 0.1f, true, 10f))
			{
				return;
			}
			this.clearCurrentPath();
		}
		this.steerFollow(owner, vector);
		if (this.steering.IsInRange(vector, 0.5f) || this.steering.IsInRange(chestPosition, this.FollowDistance))
		{
			this.setState(EntityDrone.State.Idle);
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x060024E1 RID: 9441 RVA: 0x000E2766 File Offset: 0x000E0966
	public bool IsWeaponAttached
	{
		get
		{
			return this.stunWeapon != null;
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x060024E2 RID: 9442 RVA: 0x000E2771 File Offset: 0x000E0971
	public EntityDrone.AttackMode AttackState
	{
		get
		{
			return this.attackMode;
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x060024E3 RID: 9443 RVA: 0x000E2779 File Offset: 0x000E0979
	public bool CanAttack
	{
		get
		{
			return this.state != EntityDrone.State.Shutdown && this.state != EntityDrone.State.Heal && this.state != EntityDrone.State.Attack && this.WakeupAnimTime <= 0f;
		}
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x000E27A8 File Offset: 0x000E09A8
	public void SetAttacKMode(EntityDrone.AttackMode mode)
	{
		this.attackMode = mode;
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x000E27B1 File Offset: 0x000E09B1
	public List<DroneWeapons.Weapon> GetInstalledWeapons()
	{
		return this.installedWeapons;
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x000E27BC File Offset: 0x000E09BC
	public DroneWeapons.Weapon GetInstalledWeapon(string itemKey)
	{
		for (int i = 0; i < this.installedWeapons.Count; i++)
		{
			DroneWeapons.Weapon weapon = this.installedWeapons[i];
			if (weapon.ItemName.Equals(itemKey))
			{
				return weapon;
			}
		}
		return null;
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x000E27FD File Offset: 0x000E09FD
	public void SetActiveWeapon(DroneWeapons.Weapon _weapon)
	{
		this.activeWeapon = _weapon;
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void attackState()
	{
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void exitAttackState()
	{
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x060024EA RID: 9450 RVA: 0x000E2806 File Offset: 0x000E0A06
	public bool IsHealModAttached
	{
		get
		{
			return this.healWeapon != null;
		}
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x000E2811 File Offset: 0x000E0A11
	public Vector3 GetHealArmPosition()
	{
		if (this.healWeapon != null)
		{
			return this.healWeapon.WeaponJoint.position + Origin.position;
		}
		return this.position + Origin.position;
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x060024EC RID: 9452 RVA: 0x000E2846 File Offset: 0x000E0A46
	public EntityDrone.AllyHealMode HealAllyMode
	{
		get
		{
			return this.allyHealMode;
		}
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x000E284E File Offset: 0x000E0A4E
	public bool TargetCanBeHealed(EntityAlive entity)
	{
		return this.healWeapon != null && this.healWeapon.targetCanBeHealed(entity) && this.healWeapon.hasHealingItem();
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x000E2873 File Offset: 0x000E0A73
	public bool IsTargetInNeedOfMedical(EntityAlive target)
	{
		return this.healWeapon != null && this.healWeapon.isTargetInNeedOfMedical(target);
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x000E288C File Offset: 0x000E0A8C
	public EntityAlive GetNearestHealTargetInRange(float range)
	{
		if (!this.IsHealingAllies || this.registeredPartyMembers == null)
		{
			return this.Owner;
		}
		List<EntityAlive> healingTargetsInRange = this.getHealingTargetsInRange(this.registeredPartyMembers, range);
		if (healingTargetsInRange.Count > 0)
		{
			if (healingTargetsInRange.Count > 1)
			{
				healingTargetsInRange.Sort((EntityAlive x, EntityAlive y) => x.Health.CompareTo(y.Health));
			}
			return healingTargetsInRange[0];
		}
		return null;
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x000E28FE File Offset: 0x000E0AFE
	[PublicizedFrom(EAccessModifier.Private)]
	public void setHealAllies(bool value)
	{
		this.IsHealingAllies = value;
		this.allyHealMode = (value ? EntityDrone.AllyHealMode.HealAllies : EntityDrone.AllyHealMode.DoNotHeal);
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x000E2914 File Offset: 0x000E0B14
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> getHealingTargetsInRange(List<int> playerIds, float range)
	{
		EntityDrone.healTargetsInRange.Clear();
		for (int i = 0; i < playerIds.Count; i++)
		{
			int num = playerIds[i];
			EntityAlive entityAlive = this.world.GetEntity(num) as EntityAlive;
			if (entityAlive)
			{
				float magnitude = (this.position - entityAlive.position).magnitude;
				bool flag = !RaycastPathUtils.IsPositionBlocked(this.position, entityAlive.getHeadPosition(), 65536, false);
				if (magnitude < range && this.healWeapon.targetNeedsHealing(entityAlive) && flag)
				{
					EntityDrone.healTargetsInRange.Add(entityAlive);
				}
			}
		}
		return EntityDrone.healTargetsInRange;
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x000E29BC File Offset: 0x000E0BBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void healTargetServer(EntityAlive target, bool healReq = false)
	{
		if (this.state != EntityDrone.State.Heal && this.healWeapon.canFire() && (healReq || this.healWeapon.isTargetInNeedOfMedical(target)))
		{
			this.healTarget(target);
		}
	}

	// Token: 0x060024F3 RID: 9459 RVA: 0x000E29EC File Offset: 0x000E0BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void healRequestClient()
	{
		this.SetState(EntityDrone.State.Heal, true);
		this.SetState(EntityDrone.State.Idle, false);
	}

	// Token: 0x060024F4 RID: 9460 RVA: 0x000E29FE File Offset: 0x000E0BFE
	[PublicizedFrom(EAccessModifier.Private)]
	public void healTarget(EntityAlive target)
	{
		base.SetAttackTarget(target, 1200);
		if (base.GetAttackTarget())
		{
			this.SetActiveWeapon(this.healWeapon);
			this.SetState(EntityDrone.State.Heal, true);
		}
	}

	// Token: 0x060024F5 RID: 9461 RVA: 0x000E2A30 File Offset: 0x000E0C30
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkNotifityNeedsHealItem()
	{
		if (!this.healWeapon.hasHealingItem())
		{
			GameManager.ShowTooltip(this.Owner as EntityPlayerLocal, Localization.Get("xuiDroneNeedsHealItemsStored", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			this.playSound("drone_empty", 1f);
			return true;
		}
		return false;
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x000E2A8C File Offset: 0x000E0C8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateNeedsHealItemCheck()
	{
		if (this.needsHealItemTimer > 0f)
		{
			this.needsHealItemTimer -= 0.05f;
		}
		if (this.needsHealItemTimer <= 0f && this.needsHealNotifyCount < 2 && this.checkNotifityNeedsHealItem())
		{
			this.needsHealItemTimer = 30f;
			this.needsHealNotifyCount++;
		}
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x000E2AEF File Offset: 0x000E0CEF
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearNeedsHealItemCheck()
	{
		this.needsHealItemTimer = 0f;
		this.needsHealNotifyCount = 0;
	}

	// Token: 0x060024F8 RID: 9464 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void healState()
	{
	}

	// Token: 0x060024F9 RID: 9465 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void onHealDone()
	{
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x000E2B03 File Offset: 0x000E0D03
	public bool IsOnTeleportCooldown()
	{
		return this.teleportAtkCooldownTimer > 0f;
	}

	// Token: 0x060024FB RID: 9467 RVA: 0x000E2B12 File Offset: 0x000E0D12
	public void TeleportIfFollowing()
	{
		if (this.orderState == EntityDrone.Orders.Follow && !this.isShutdown)
		{
			this.teleportState();
		}
	}

	// Token: 0x060024FC RID: 9468 RVA: 0x000E2B2A File Offset: 0x000E0D2A
	public void TeleportOutOfRange()
	{
		if (this.state == EntityDrone.State.Attack)
		{
			this.exitAttackState();
		}
		if (this.state == EntityDrone.State.Heal)
		{
			this.onHealDone();
		}
		this.teleportState();
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x000E2B50 File Offset: 0x000E0D50
	public void TeleportToPosition(Vector3 telePos)
	{
		this.teleportToPosition(telePos);
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x000E2B59 File Offset: 0x000E0D59
	[PublicizedFrom(EAccessModifier.Private)]
	public void teleportToPosition(Vector3 telePos)
	{
		this.motion = Vector3.zero;
		this.SetPosition(telePos, true);
	}

	// Token: 0x060024FF RID: 9471 RVA: 0x000E2B6E File Offset: 0x000E0D6E
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkTeleportPos(Vector3 target)
	{
		if (this.Owner)
		{
			if (this.isOutOfRange(this.Owner.position, 32f))
			{
				Log.Out("teleport failed");
				return;
			}
			Log.Out("teleport success!");
		}
	}

	// Token: 0x06002500 RID: 9472 RVA: 0x000E2BAC File Offset: 0x000E0DAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void teleportState()
	{
		this.teleportAtkCooldownTimer = 5f;
		this.setState(EntityDrone.State.Teleport);
		this.clearCurrentPath();
		this.motion = Vector3.zero;
		Vector3 chestPosition = this.Owner.getChestPosition();
		Vector3 lookVector = this.Owner.GetLookVector();
		Vector3 vector = chestPosition - new Vector3(lookVector.x, 0f, lookVector.z) * 5f;
		Vector3[] groupPositions = EntityDrone.GetGroupPositions(this.Owner, 5f, false, 0f);
		Array.Sort<Vector3>(groupPositions, (Vector3 x, Vector3 y) => Vector3.Distance(this.position, x).CompareTo(Vector3.Distance(this.position, y)));
		foreach (Vector3 vector2 in groupPositions)
		{
			RaycastHit raycastHit;
			bool flag = EntityDrone.IsPositionBlocked(chestPosition, vector2, out raycastHit, 1073807360, false, 0f);
			bool flag2 = EntityDrone.IsPositionBlocked(vector2, chestPosition, 1073807360, false, 0f);
			if (!flag && !flag2)
			{
				vector = vector2;
				break;
			}
			if (flag)
			{
				vector = World.worldToBlockPos(raycastHit.point + Origin.position).ToVector3Center();
				break;
			}
		}
		this.SetPosition(vector, true);
		this.ModelTransform.position = vector - Origin.position;
		this.setState(EntityDrone.State.Idle);
		this.checkTeleportPos(vector);
	}

	// Token: 0x06002501 RID: 9473 RVA: 0x000E2CEC File Offset: 0x000E0EEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void performShutdown()
	{
		this.DebugDroneLog(EntityDrone.LoggingTypes.Shutdown, "performShutdown() {0}", new object[] { this });
		if (this.Owner)
		{
			Manager.Stop(this.Owner.entityId, "drone_take");
		}
		this.playVO("drone_shutdown", true, 1f);
		if (this.Owner && this.Owner.HasOwnedEntity(this.entityId))
		{
			this.Owner.GetOwnedEntity(this.entityId).SetLastKnownPosition(this.position);
		}
		this.setShutdown(true);
		this.isShutdownPending = false;
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x000E2D8C File Offset: 0x000E0F8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setShutdown(bool value)
	{
		this.DebugDroneLog(EntityDrone.LoggingTypes.Shutdown, "setShutdown({0}) {1}", new object[] { value, this });
		this.animator = base.GetComponentInChildren<Animator>();
		if (this.animator)
		{
			this.animator.enabled = !value;
		}
		this.PhysicsTransform.gameObject.SetActive(!value);
		this.IsNoCollisionMode.Value = value;
		this.setShutdownDestruction(value);
		this.isShutdown = value;
		if (value)
		{
			base.SetRevengeTarget(null);
			base.SetAttackTarget(null, 0);
			this.setShutdownAnim();
			this.setState(EntityDrone.State.Shutdown);
			Handle handle = this.idleLoop;
			if (handle != null)
			{
				handle.Stop(this.entityId);
			}
			this.idleLoop = null;
		}
		else
		{
			this.isGrounded = value;
			if (this.orderState == EntityDrone.Orders.Stay)
			{
				this.setState(EntityDrone.State.Sentry);
			}
			else
			{
				this.setState(EntityDrone.State.Idle);
			}
			if (this.Owner && this.Owner.HasOwnedEntity(this.entityId))
			{
				this.Owner.GetOwnedEntity(this.entityId).ClearLastKnownPostition();
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.SendSyncData(32768);
		}
	}

	// Token: 0x06002503 RID: 9475 RVA: 0x000E2EBC File Offset: 0x000E10BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setShutdownDestruction(bool value)
	{
		if (value && this.Health > 1)
		{
			return;
		}
		Transform transform = base.transform.FindInChilds("p_smokeLeft", false);
		if (transform)
		{
			transform.gameObject.SetActive(value);
		}
		Transform transform2 = base.transform.FindInChilds("p_smokeRight", false);
		if (transform2)
		{
			transform2.gameObject.SetActive(value);
		}
	}

	// Token: 0x06002504 RID: 9476 RVA: 0x000E2F24 File Offset: 0x000E1124
	[PublicizedFrom(EAccessModifier.Private)]
	public void processShutdown()
	{
		if (this.isGrounded)
		{
			return;
		}
		this.fallBlockPos.RoundToInt(this.position - EntityDrone.blockHeightOffset);
		RaycastHit raycastHit;
		if ((!this.hasFallPoint || this.world.GetBlock(this.fallBlockPos).isair) && Physics.Raycast(this.position - Origin.position + EntityDrone.blockHeightOffset, Vector3.down, out raycastHit, 999f, 268500992))
		{
			this.fallPoint = raycastHit.point;
			this.isGrounded = false;
			this.hasFallPoint = true;
		}
		if (this.isShutdown)
		{
			Vector3 position = this.position;
			float num = Vector3.Distance(this.position, this.fallPoint + Origin.position);
			if (num < 0.01f)
			{
				this.isGrounded = true;
				return;
			}
			position.y -= num * this.SpeedFlying * 0.05f;
			position.y = Mathf.Max(position.y, this.fallPoint.y);
			this.SetPosition(position, true);
		}
	}

	// Token: 0x06002505 RID: 9477 RVA: 0x000E3040 File Offset: 0x000E1240
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateShutdownState()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (this.Owner)
			{
				if (this.Owner.Health <= 0 && this.state != EntityDrone.State.Shutdown && this.state != EntityDrone.State.Sentry)
				{
					this.performShutdown();
				}
				if (this.Health > 1 && this.Owner.Health > 1 && Vector3.Distance(this.position, this.Owner.position) < 10f && this.state == EntityDrone.State.Shutdown)
				{
					this.setShutdown(false);
				}
			}
			if (this.state == EntityDrone.State.Shutdown)
			{
				this.processShutdown();
			}
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06002506 RID: 9478 RVA: 0x000E30E2 File Offset: 0x000E12E2
	// (set) Token: 0x06002507 RID: 9479 RVA: 0x000E30EF File Offset: 0x000E12EF
	public int AmmoCount
	{
		get
		{
			return this.OriginalItemValue.Meta;
		}
		set
		{
			this.OriginalItemValue.Meta = value;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06002508 RID: 9480 RVA: 0x000E30FD File Offset: 0x000E12FD
	// (set) Token: 0x06002509 RID: 9481 RVA: 0x000E3108 File Offset: 0x000E1308
	public bool IsVisible
	{
		get
		{
			return this.isVisible;
		}
		set
		{
			if (value != this.isVisible)
			{
				Renderer[] componentsInChildren = this.emodel.gameObject.GetComponentsInChildren<Renderer>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = value;
				}
				this.isVisible = value;
			}
		}
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x000E3150 File Offset: 0x000E1350
	public void SetRenderersEnabled(bool value)
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = value;
		}
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000E317B File Offset: 0x000E137B
	[PublicizedFrom(EAccessModifier.Private)]
	public void setNoClip(bool value)
	{
		this.IsNoCollisionMode.Value = value;
		this.PhysicsTransform.gameObject.layer = (value ? 14 : 15);
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000E31A4 File Offset: 0x000E13A4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOutOfRange(Vector3 _target, float _distance)
	{
		return (this.position - _target).sqrMagnitude > _distance * _distance;
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x000E31CC File Offset: 0x000E13CC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsAnyPlayerWithingDist(float dist)
	{
		PersistentPlayerList persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
		if (((persistentPlayerList != null) ? persistentPlayerList.Players : null) != null)
		{
			foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in persistentPlayerList.Players)
			{
				EntityPlayer entityPlayer = this.world.GetEntity(keyValuePair.Value.EntityId) as EntityPlayer;
				if (entityPlayer && (entityPlayer.getChestPosition() - this.position).magnitude <= dist)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000E3278 File Offset: 0x000E1478
	[PublicizedFrom(EAccessModifier.Private)]
	public float getTargetView(EntityAlive target, float degrees, float weight)
	{
		Vector3 lookVector = target.GetLookVector();
		Vector3 vector = this.position - target.position;
		float num = Vector3.Angle(lookVector, vector);
		if (num < degrees * 0.5f)
		{
			return (1f - num / degrees * 0.5f) * weight;
		}
		return 0f;
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x000027FC File Offset: 0x000009FC
	public void NotifyOffTheWorld()
	{
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnOriginChanged(Vector3 _origin)
	{
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000E32C8 File Offset: 0x000E14C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateDroneSystems()
	{
		if (this.initSuppressVOTimer > 0f)
		{
			this.initSuppressVOTimer -= 0.05f;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.Owner && !this.registeredOwnerHandlers)
		{
			EntityPlayer entityPlayer = this.Owner as EntityPlayer;
			if (entityPlayer)
			{
				entityPlayer.PlayerTeleportedDelegates += this.TeleportIfFollowing;
				this.registeredOwnerHandlers = true;
			}
		}
		if (this.registeredPartyMembers == null)
		{
			EntityPlayer entityPlayer2 = this.Owner as EntityPlayer;
			if (entityPlayer2)
			{
				Party party = entityPlayer2.Party;
				if (party != null)
				{
					this.registeredPartyMembers = new List<int>();
					int[] memberIdArray = party.GetMemberIdArray();
					for (int i = 0; i < memberIdArray.Length; i++)
					{
						this.registeredPartyMembers.Add(memberIdArray[i]);
					}
					party.PartyMemberAdded += this.onPartyMemberAdded;
					party.PartyMemberRemoved += this.onPartyMemberRemoved;
				}
			}
		}
		if (this.sensors != null)
		{
			this.sensors.Update();
			if (this.sensors.IsEnemyInRange())
			{
				this.FollowDistance = 10f;
			}
			else
			{
				this.FollowDistance = 5f;
			}
		}
		bool flag = this.state != EntityDrone.State.Shutdown && this.state != EntityDrone.State.Sentry && this.state != EntityDrone.State.Attack && this.state != EntityDrone.State.Heal;
		if (this.healWeapon != null && flag && this.healWeapon.targetCanBeHealed(this.Owner) && this.initSuppressVOTimer <= 0f)
		{
			this.updateNeedsHealItemCheck();
		}
		if (this.teleportAtkCooldownTimer >= 0f)
		{
			this.teleportAtkCooldownTimer -= 0.05f;
		}
		for (int j = 0; j < this.installedWeapons.Count; j++)
		{
			this.installedWeapons[j].Update();
		}
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x000E34A3 File Offset: 0x000E16A3
	public bool IsInRange(Vector3 target, float range)
	{
		return this.steering.IsInRange(target, range);
	}

	// Token: 0x06002513 RID: 9491 RVA: 0x000E34B4 File Offset: 0x000E16B4
	public void MoveTo(Vector3 targetPos, float pointRadius)
	{
		Vector3 vector = this.steering.Seek(this.position, targetPos, pointRadius);
		this.move(vector, this.SpeedFlying, false);
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x000E34E3 File Offset: 0x000E16E3
	public void RotateTo(Vector3 dir)
	{
		this.rotateTo(dir);
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x000E34EC File Offset: 0x000E16EC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canMove(Vector3 dir)
	{
		Vector3 vector = this.position + dir.normalized * this.physColHeight;
		return !RaycastPathUtils.IsPositionBlocked(this.position, vector, 1073807360, false);
	}

	// Token: 0x06002516 RID: 9494 RVA: 0x000E352C File Offset: 0x000E172C
	[PublicizedFrom(EAccessModifier.Private)]
	public void move(Vector3 dir, float speedFlying, bool ignoreObsticles = false)
	{
		Vector3 vector = this.position + dir.normalized * this.physColHeight;
		if (this.ownerIsOnVehicle || !EntityDrone.IsPositionBlocked(this.position, vector, 1073807360, true, 0f) || ignoreObsticles)
		{
			this.motion += dir * speedFlying * 0.05f;
		}
	}

	// Token: 0x06002517 RID: 9495 RVA: 0x000E35A4 File Offset: 0x000E17A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void move(Vector3 dir, Vector3 target, float speedFlying, bool ignoreObsticles = false)
	{
		Vector3 vector = this.position + dir.normalized * this.physColHeight;
		if (this.ownerIsOnVehicle || !EntityDrone.IsPositionBlocked(this.position, vector, 1073807360, true, 0f) || ignoreObsticles)
		{
			this.motion += Vector3.ClampMagnitude(dir * speedFlying * 0.05f, (target - this.position).magnitude);
		}
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x000E3634 File Offset: 0x000E1834
	[PublicizedFrom(EAccessModifier.Private)]
	public void move(Vector3 dir, bool ignoreObsticles = false)
	{
		this.move(dir, this.currentSpeedFlying, ignoreObsticles);
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x000E3644 File Offset: 0x000E1844
	public override void MoveEntityHeadedFlying(Vector3 _direction, bool _isDirAbsolute)
	{
		if (!this.IsNoCollisionMode.Value)
		{
			this.entityCollision(this.motion);
			this.motion *= base.ConditionalScalePhysicsMulConstant(0.546f);
			return;
		}
		this.SetPosition(this.position + this.motion, true);
		this.motion = Vector3.zero;
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x000E36AA File Offset: 0x000E18AA
	[PublicizedFrom(EAccessModifier.Private)]
	public void rotateTo(Vector3 dir)
	{
		if (dir != Vector3.zero)
		{
			this.rotation = this.rotateToDir(dir);
		}
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x000E36C8 File Offset: 0x000E18C8
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 rotateToDir(Vector3 dir)
	{
		return Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(dir), (1f - Vector3.Angle(base.transform.forward, dir) / 180f) * this.RotationSpeed * 0.05f).eulerAngles;
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000E3720 File Offset: 0x000E1920
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 rotateToEuler(Vector3 rot)
	{
		return Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(rot), (1f - Vector3.Angle(base.transform.forward, (rot - base.transform.eulerAngles).normalized) / 180f) * this.RotationSpeed * 0.05f).eulerAngles;
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x000E3790 File Offset: 0x000E1990
	public static bool GetPath(List<Vector3> currentPath, EntityAlive entity, Vector3 start, Vector3 end, float speed, EAIBase aiTask = null, float seekDist = 0f, float pointRadius = 0.1f, bool debugDraw = false, float duration = 0f)
	{
		RaycastPathUtils.DrawBounds(start, Color.yellow, duration, 1f);
		RaycastPathUtils.DrawBounds(end, Color.green, duration, 1f);
		Vector3 projectedGroundPoint = EntityDrone.GetProjectedGroundPoint(start, debugDraw, duration);
		Vector3 projectedGroundPoint2 = EntityDrone.GetProjectedGroundPoint(end, debugDraw, duration);
		if (EntityDrone.GetProjectedPath(currentPath, entity, projectedGroundPoint, projectedGroundPoint2, speed, aiTask, false, duration))
		{
			Utils.DrawCircleLinesHorzontal(end - Origin.position, seekDist, Color.white, Color.green, 12, duration);
			for (int i = 1; i < currentPath.Count; i++)
			{
				Vector3 vector = currentPath[i];
				vector += EntityDrone.blockHeightOffset;
				vector.y += 1f;
				currentPath[i] = vector;
			}
			Vector3 vector2 = projectedGroundPoint2;
			vector2.y += 1f;
			currentPath[currentPath.Count - 1] = vector2;
			for (int j = 1; j < currentPath.Count - 1; j++)
			{
				if (EntityDrone.IsPositionBlocked(currentPath[j], currentPath[j + 1], 1073807360, false, 0f))
				{
					if (debugDraw)
					{
						Utils.DrawLine(currentPath[j] - Origin.position, currentPath[j + 1] - Origin.position, Color.white, Color.magenta, 2, duration);
						Utils.DrawCircleLinesHorzontal(currentPath[j] - Origin.position, pointRadius, Color.white, Color.green, 12, duration);
					}
					Vector3 vector3 = currentPath[j];
					vector3.y -= 1f;
					Vector3 vector4 = currentPath[j + 1];
					vector4.y -= 1f;
					if (!EntityDrone.IsPositionBlocked(currentPath[j], vector3, 1073807360, false, 0f) && !EntityDrone.IsPositionBlocked(vector3, vector4, 1073807360, false, 0f))
					{
						Utils.DrawLine(vector3 - Origin.position, vector4 - Origin.position, Color.white, Color.yellow, 2, duration);
						currentPath[j] = vector3;
						currentPath[j + 1] = vector4;
						if (j + 2 < currentPath.Count - 1 && !EntityDrone.IsPositionBlocked(vector4, currentPath[j + 2] - EntityDrone.blockHeightOffset, 1073807360, false, 0f))
						{
							currentPath[j + 2] = currentPath[j + 2] - EntityDrone.blockHeightOffset;
						}
						j++;
					}
					if (j + 2 >= currentPath.Count - 1 || EntityDrone.IsPositionBlocked(currentPath[j], currentPath[j + 2], 0, false, 0f))
					{
						currentPath.RemoveRange(j + 1, currentPath.Count - (j + 1));
						break;
					}
					Utils.DrawLine(currentPath[j] - Origin.position, currentPath[j + 2] - Origin.position, Color.white, Color.blue, 2, duration);
					currentPath.RemoveAt(j + 1);
				}
				else if (debugDraw)
				{
					Utils.DrawLine(currentPath[j] - Origin.position, currentPath[j + 1] - Origin.position, Color.white, Color.cyan, 2, duration);
					Utils.DrawCircleLinesHorzontal(currentPath[j] - Origin.position, pointRadius, Color.white, Color.cyan, 12, duration);
				}
			}
			currentPath.RemoveAt(0);
			return true;
		}
		return false;
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000E3B18 File Offset: 0x000E1D18
	public static bool GetProjectedPath(List<Vector3> projectedPath, EntityAlive entity, Vector3 start, Vector3 end, float speed, EAIBase aiTask = null, bool debugDraw = false, float duration = 0f)
	{
		projectedPath.Clear();
		int entityId = entity.entityId;
		PathFinderThread instance = PathFinderThread.Instance;
		PathInfo path = instance.GetPath(entityId);
		PathEntity pathEntity = ((path != null) ? path.path : null);
		if (pathEntity == null && !instance.IsCalculatingPath(entityId))
		{
			instance.FindPath(entity, start, end, speed, false, aiTask);
			return false;
		}
		if (pathEntity == null)
		{
			return false;
		}
		for (int i = 0; i < pathEntity.points.Length; i++)
		{
			Vector3 projectedLocation = pathEntity.points[i].projectedLocation;
			projectedPath.Add(projectedLocation);
		}
		if (debugDraw)
		{
			for (int j = 0; j < projectedPath.Count - 1; j++)
			{
				Utils.DrawLine(projectedPath[j] - Origin.position, projectedPath[j + 1] - Origin.position, Color.white, Color.cyan, 2, duration);
			}
		}
		return true;
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x000E3BEC File Offset: 0x000E1DEC
	public static Vector3 GetProjectedGroundPoint(Vector3 currentPos, bool debugDraw = false, float duration = 0f)
	{
		Vector3 vector = currentPos;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector - Origin.position + EntityDrone.blockHeightOffset, Vector3.down, out raycastHit, 100f, 1073807360))
		{
			vector = raycastHit.point + EntityDrone.blockHeightOffset + Origin.position;
			if (debugDraw)
			{
				RaycastPathUtils.DrawBounds(vector, Color.white, duration, 1f);
			}
		}
		else
		{
			vector -= EntityDrone.blockHeightOffset;
			vector.y -= 1f;
			if (debugDraw)
			{
				RaycastPathUtils.DrawBounds(vector, Color.white, duration, 1f);
			}
		}
		return vector;
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x000E3C8C File Offset: 0x000E1E8C
	public static Vector3[] GetGroupPositions(EntityAlive _entity, float followDist, bool debugDraw = false, float duration = 0f)
	{
		Vector3[] array = new Vector3[5];
		float num = 1f;
		Vector3 chestPosition = _entity.getChestPosition();
		Vector3 lookVector = _entity.GetLookVector();
		lookVector.y = 0f;
		array[0] = chestPosition - lookVector * followDist;
		Vector3 normalized = (_entity.transform.right - lookVector).normalized;
		normalized.y = 0f;
		array[1] = chestPosition + normalized * followDist * num;
		Vector3 normalized2 = (_entity.transform.right + lookVector).normalized;
		normalized2.y = 0f;
		array[2] = chestPosition - normalized2 * followDist * num;
		Vector3 normalized3 = (normalized - lookVector).normalized;
		normalized3.y = 0f;
		array[3] = chestPosition + normalized3 * followDist * num;
		Vector3 normalized4 = (normalized2 + lookVector).normalized;
		normalized4.y = 0f;
		array[4] = chestPosition - normalized4 * followDist * num;
		World world = GameManager.Instance.World;
		foreach (Vector3 vector in array)
		{
			bool flag = EntityDrone.IsPositionBlocked(chestPosition, vector, 1073807360, false, 0f);
			bool flag2 = EntityDrone.IsPositionBlocked(vector, chestPosition, 1073807360, false, 0f);
			Vector3i vector3i = World.worldToBlockPos(vector);
			if (!flag && !flag2)
			{
				if (RaycastPathWorldUtils.FindNodeType(RaycastPathWorldUtils.ScanVolume(world, vector3i.ToVector3Center(), false, false, false, 0f), cPathNodeType.Air) != null && debugDraw)
				{
					RaycastPathUtils.DrawNode(new RaycastNode(vector3i.ToVector3Center(), 1f, 0), Color.yellow, duration);
				}
			}
			else if (debugDraw)
			{
				RaycastPathUtils.DrawNode(new RaycastNode(vector3i.ToVector3Center(), 1f, 0), Color.red, duration);
			}
		}
		return array;
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x000E3E94 File Offset: 0x000E2094
	public static bool IsPositionBlocked(Vector3 start, Vector3 end, int layerMask = 0, bool debugDraw = false, float duration = 0f)
	{
		RaycastHit raycastHit;
		return EntityDrone.IsPositionBlocked(start, end, out raycastHit, layerMask, debugDraw, duration);
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x000E3EB0 File Offset: 0x000E20B0
	public static bool IsPositionBlocked(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask = 0, bool debugDraw = false, float duration = 0f)
	{
		Vector3 vector = end - start;
		return EntityDrone.IsPositionBlocked(new Ray(start - Origin.position, vector), out hit, layerMask, vector.magnitude, debugDraw, duration);
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x000E3EE8 File Offset: 0x000E20E8
	public static bool IsPositionBlocked(Ray ray, out RaycastHit hit, int layerMask = 0, float maxDist = 100f, bool debugDraw = false, float duration = 0f)
	{
		bool flag = Physics.Raycast(ray, out hit, maxDist, layerMask);
		if (debugDraw)
		{
			if (flag)
			{
				Utils.DrawLine(ray.origin, hit.point, Color.magenta, Color.magenta, 2, duration);
			}
			else
			{
				Utils.DrawLine(ray.origin, ray.origin + ray.direction * maxDist, Color.cyan, Color.cyan, 2, duration);
			}
		}
		return flag;
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x000E3F5C File Offset: 0x000E215C
	public static EntityDrone FindCollisionEntity(Transform t)
	{
		if (t)
		{
			EntityDrone component = t.GetComponent<EntityDrone>();
			if (component)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x000E3F84 File Offset: 0x000E2184
	public bool IgnoreCollisionEntity(Ray ray, float seeDist)
	{
		bool flag = false;
		int layer = base.gameObject.layer;
		GameObject gameObject = this.PhysicsTransform.gameObject;
		int layer2 = gameObject.layer;
		Utils.SetLayerRecursively(base.gameObject, 2);
		Utils.SetLayerRecursively(gameObject, 2);
		if (Voxel.Raycast(this.world, ray, seeDist, -1612492829, 64, 0f))
		{
			flag = true;
		}
		Utils.SetLayerRecursively(base.gameObject, layer);
		Utils.SetLayerRecursively(gameObject, layer2);
		return flag;
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000E3FF4 File Offset: 0x000E21F4
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i getBlockPosition(Vector3 worldPos)
	{
		Vector3i vector3i = new Vector3i(worldPos);
		Vector3 vector = worldPos - vector3i.ToVector3Center();
		return vector3i + Vector3i.FromVector3Rounded(vector);
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000E4024 File Offset: 0x000E2224
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 findOpenBlockAbove(Vector3 targetPosition, int maxHeight = 256)
	{
		Vector3i vector3i = this.getBlockPosition(targetPosition);
		vector3i += Vector3i.up;
		int num = 1;
		BlockValue blockValue = this.world.GetBlock(vector3i);
		while (!blockValue.isair && num < maxHeight)
		{
			num++;
			vector3i += Vector3i.up;
			blockValue = this.world.GetBlock(vector3i);
		}
		return vector3i.ToVector3Center();
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000E4088 File Offset: 0x000E2288
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEntityAboveOrBelow(Entity entity)
	{
		bool flag = false;
		Vector3 chestPosition = entity.getChestPosition();
		float num = this.position.x - chestPosition.x;
		float num2 = this.position.z - chestPosition.z;
		float num3 = this.position.y - chestPosition.y;
		if (num > -0.85f && num < 0.85f && num2 > -0.85f && num2 < 0.85f && (num3 < -1.2f || num3 > 1.2f))
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000E4110 File Offset: 0x000E2310
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTargetUnderWater(Vector3 targetPosition)
	{
		Vector3i blockPosition = this.getBlockPosition(targetPosition);
		return this.world.GetBlock(blockPosition).type == 240;
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x000E4140 File Offset: 0x000E2340
	[PublicizedFrom(EAccessModifier.Private)]
	public void followPlannedPath(float speed, float pointRadius = 0.1f, bool debugDraw = false, float duration = 0f)
	{
		if (this.currentPath.Count > 0)
		{
			this.currentPathTarget = this.currentPath[0];
			this.RotateTo((this.currentPathTarget - this.position).normalized);
			this.MoveTo(this.currentPathTarget, pointRadius);
			if (this.IsInRange(this.currentPathTarget, pointRadius))
			{
				this.currentPath.RemoveAt(0);
				return;
			}
			if (debugDraw && this.currentPath.Count > 1)
			{
				RaycastPathUtils.DrawLine(this.currentPath[0], this.currentPath[1], Color.green, 1f);
			}
			if (this.pathTracker.IsStuck(this.position, this.currentPath[0], 0.5f, false))
			{
				if (this.currentPath.Count > 1)
				{
					this.TeleportToPosition(this.currentPath[1]);
					this.currentPath.RemoveRange(0, 2);
					return;
				}
				this.TeleportToPosition(this.currentPath[0]);
				this.currentPath.RemoveAt(0);
				return;
			}
		}
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x000E4262 File Offset: 0x000E2462
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearCurrentPath()
	{
		this.currentPath.Clear();
		this.OnPathInterupted();
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000E4275 File Offset: 0x000E2475
	public void OnPathInterupted()
	{
		this.moveHelper.Stop();
		this.navigator.clearPath();
		if (PathFinderThread.Instance != null)
		{
			PathFinderThread.Instance.RemovePathsFor(this.entityId);
		}
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000E42A4 File Offset: 0x000E24A4
	public override string MakeDebugNameInfo()
	{
		return string.Format("\nState: {0}", this.state.ToStringCached<EntityDrone.State>());
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000E42BB File Offset: 0x000E24BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateDebugName()
	{
		this.aiManager.UpdateDebugName();
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000E42C8 File Offset: 0x000E24C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void debugUpdate()
	{
		this.updateDebugName();
		if (this.debugCamera)
		{
			if (this.Owner && this.currentPath.Count == 0)
			{
				this.debugCamera.transform.LookAt(this.Owner.getHeadPosition() - Origin.position);
				return;
			}
			this.debugCamera.transform.forward = base.transform.forward;
		}
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000E4343 File Offset: 0x000E2543
	public void DebugTeleportUnstuck()
	{
		this.teleportState();
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000E434B File Offset: 0x000E254B
	public void DebugTeleportTo(Vector3 pos)
	{
		this.clearCurrentPath();
		this.motion = Vector3.zero;
		this.SetPosition(pos, true);
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06002532 RID: 9522 RVA: 0x000E4366 File Offset: 0x000E2566
	public bool DebugFrendlyFireEnabled
	{
		get
		{
			return this.debugFriendlyFire;
		}
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000E436E File Offset: 0x000E256E
	public void DebugToggleFriendlyFire()
	{
		this.debugFriendlyFire = !this.debugFriendlyFire;
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06002534 RID: 9524 RVA: 0x000E437F File Offset: 0x000E257F
	public bool IsDebugCameraEnabled
	{
		get
		{
			return this.debugShowCamera;
		}
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x000E4387 File Offset: 0x000E2587
	public void DebugToggleDebugCamera()
	{
		this._prepareDebugCamera();
		this.debugShowCamera = !this.debugShowCamera;
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x000E439E File Offset: 0x000E259E
	public void SetDebugCameraEnabled(bool value)
	{
		this._prepareDebugCamera();
		this.debugShowCamera = value;
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x000E43B0 File Offset: 0x000E25B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void _prepareDebugCamera()
	{
		if (this.debugShowCamera && this.debugCamera)
		{
			global::UnityEngine.Object.Destroy(this.debugCamera);
			return;
		}
		this.debugCamera = new GameObject("Camera");
		this.debugCamera.transform.SetParent(base.transform);
		this.debugCamera.transform.localPosition = Vector3.zero;
		this.debugCamera.transform.localRotation = Quaternion.identity;
		Camera camera = this.debugCamera.AddComponent<Camera>();
		Rect rect = camera.rect;
		float num = 0.35f;
		rect.width = num;
		rect.height = num;
		float num2 = 1f - num;
		rect.x = num2;
		rect.y = num2;
		camera.rect = rect;
		camera.farClipPlane = 64f;
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000E4480 File Offset: 0x000E2680
	public void Debug_ToggleReconMode()
	{
		this._prepareReconCam();
		DroneManager.Debug_LocalControl = !DroneManager.Debug_LocalControl;
		EntityPlayerLocal entityPlayerLocal = this.Owner as EntityPlayerLocal;
		entityPlayerLocal.PlayerUI.windowManager.SetHUDEnabled(DroneManager.Debug_LocalControl ? GUIWindowManager.HudEnabledStates.FullHide : GUIWindowManager.HudEnabledStates.Enabled);
		entityPlayerLocal.bEntityAliveFlagsChanged = true;
		entityPlayerLocal.IsGodMode.Value = DroneManager.Debug_LocalControl;
		entityPlayerLocal.IsNoCollisionMode.Value = DroneManager.Debug_LocalControl;
		entityPlayerLocal.IsFlyMode.Value = DroneManager.Debug_LocalControl;
		if (entityPlayerLocal.IsGodMode.Value)
		{
			entityPlayerLocal.Buffs.AddBuff("god", -1, true, false, -1f);
		}
		else if (!GameManager.Instance.World.IsEditor() && !GameModeCreative.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode)))
		{
			entityPlayerLocal.Buffs.RemoveBuff("god", -1, true);
		}
		entityPlayerLocal.IsSpectator = DroneManager.Debug_LocalControl;
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000E4568 File Offset: 0x000E2768
	[PublicizedFrom(EAccessModifier.Private)]
	public void _prepareReconCam()
	{
		if (DroneManager.Debug_LocalControl && this.reconCam)
		{
			global::UnityEngine.Object.Destroy(this.reconCam.gameObject);
			return;
		}
		GameObject gameObject = new GameObject(this.Owner.EntityName + "-Drone|Recon");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		this.reconCam = gameObject.AddComponent<Camera>();
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000E45F4 File Offset: 0x000E27F4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool procEnemiesInRange()
	{
		if (this.DebugEnemiesInRange && this.sensors.IsEnemyInRange() && this.Owner)
		{
			EntityPlayerLocal entityPlayerLocal = this.Owner as EntityPlayerLocal;
			if (entityPlayerLocal)
			{
				this.rotateTo((this.Owner.position - this.position).normalized);
				Vector3 vector = entityPlayerLocal.vp_FPCamera.transform.position + Origin.position;
				if (entityPlayerLocal.bFirstPersonView)
				{
					vector = this.Owner.getHeadPosition() - this.Owner.transform.forward;
					vector.y = Mathf.Max(vector.y, this.Owner.getHeadPosition().y) + 0.5f;
				}
				else
				{
					vector.y = Mathf.Max(vector.y, this.Owner.getHeadPosition().y) + 1f;
				}
				float magnitude = (this.position - vector).magnitude;
				this.move(this.steering.Seek(this.position, vector, 1f), magnitude * 15f, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x000E473A File Offset: 0x000E293A
	public ushort GetSyncFlagsReplicated(ushort syncFlags)
	{
		return syncFlags & 2;
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x000E4740 File Offset: 0x000E2940
	public void SendSyncData(ushort syncFlags)
	{
		int primaryPlayerId = GameManager.Instance.World.GetPrimaryPlayerId();
		this.SendSyncData(syncFlags, primaryPlayerId);
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000E4768 File Offset: 0x000E2968
	[PublicizedFrom(EAccessModifier.Private)]
	public void SendSyncData(ushort syncFlags, int playerId)
	{
		EntityDrone.NetPackageDroneDataSync netPackageDroneDataSync = NetPackageManager.GetPackage<EntityDrone.NetPackageDroneDataSync>().Setup(this, playerId, syncFlags);
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageDroneDataSync, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageDroneDataSync, false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000E47BC File Offset: 0x000E29BC
	public void WriteSyncData(PooledBinaryWriter _bw, ushort syncFlags)
	{
		_bw.Write(3);
		if ((syncFlags & 1) > 0)
		{
			this.OwnerID.ToStream(_bw, false);
			_bw.Write(this.Health);
		}
		if ((syncFlags & 16384) > 0)
		{
			_bw.Write((byte)this.OrderState);
			if (this.OrderState == EntityDrone.Orders.Stay)
			{
				float[] array = new float[]
				{
					this.SentryPos.x,
					this.SentryPos.y,
					this.SentryPos.z
				};
				for (int i = 0; i < array.Length; i++)
				{
					_bw.Write(array[i]);
				}
			}
		}
		if ((syncFlags & 32768) > 0)
		{
			_bw.Write((byte)this.state);
			if (this.state == EntityDrone.State.Heal)
			{
				_bw.Write(this.userRequestedHeal);
				this.userRequestedHeal = false;
			}
		}
		if ((syncFlags & 2) > 0)
		{
			byte b = 0;
			if (this.isLocked)
			{
				b |= 2;
			}
			_bw.Write(b);
			this.ownerSteamId.ToStream(_bw, false);
			_bw.Write(this.passwordHash);
			_bw.Write((byte)this.allowedUsers.Count);
			for (int j = 0; j < this.allowedUsers.Count; j++)
			{
				this.allowedUsers[j].ToStream(_bw, false);
			}
		}
		if ((syncFlags & 8) > 0)
		{
			this.bag.Write(_bw, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? StreamModeWrite.ToClient : StreamModeWrite.ToServer);
		}
		if ((syncFlags & 32) > 0)
		{
			_bw.Write(this.isQuietMode);
		}
		if ((syncFlags & 64) > 0)
		{
			_bw.Write(this.IsFlashlightOn);
		}
		if ((syncFlags & 256) > 0)
		{
			_bw.Write(this.IsHealingAllies);
		}
		if ((syncFlags & 128) > 0)
		{
			this.OriginalItemValue.Write(_bw);
		}
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x000E4970 File Offset: 0x000E2B70
	public void ReadSyncData(PooledBinaryReader _br, ushort syncFlags, int senderId)
	{
		byte b = _br.ReadByte();
		if ((syncFlags & 1) > 0)
		{
			this.OwnerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			this.Health = _br.ReadInt32();
		}
		if ((syncFlags & 16384) > 0)
		{
			EntityDrone.Orders orders = (EntityDrone.Orders)_br.ReadByte();
			if (orders == EntityDrone.Orders.Stay)
			{
				this.SentryPos.x = _br.ReadSingle();
				this.SentryPos.y = _br.ReadSingle();
				this.SentryPos.z = _br.ReadSingle();
			}
			this.setOrders(orders);
			if (GameManager.IsDedicatedServer)
			{
				this.SendSyncData(16384, senderId);
			}
		}
		if ((syncFlags & 32768) > 0)
		{
			byte b2 = _br.ReadByte();
			this.transitionState = (EntityDrone.State)b2;
			if (b >= 1 && this.transitionState == EntityDrone.State.Heal)
			{
				this.userRequestedHeal = _br.ReadBoolean();
			}
			this.DebugDroneLog("{0} read transition {1} > {2}", new object[] { this, this.state, this.transitionState });
		}
		if ((syncFlags & 2) > 0)
		{
			byte b3 = _br.ReadByte();
			this.isLocked = (b3 & 2) > 0;
			this.ownerSteamId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			if (b > 1)
			{
				this.passwordHash = _br.ReadString();
			}
			else
			{
				this.passwordHash = _br.ReadInt32().ToString();
			}
			this.allowedUsers.Clear();
			int num = (int)_br.ReadByte();
			for (int i = 0; i < num; i++)
			{
				this.allowedUsers.Add(PlatformUserIdentifierAbs.FromStream(_br, true, false));
			}
		}
		if ((syncFlags & 8) > 0)
		{
			if (b >= 3)
			{
				this.bag = Bag.Read(_br, XUiC_ItemStack.StackLocationTypes.Vehicle, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? StreamModeRead.FromClient : StreamModeRead.FromServer, this);
			}
			else
			{
				int num2 = (int)_br.ReadByte();
				ItemStack[] array = new ItemStack[num2];
				for (int j = 0; j < num2; j++)
				{
					ItemStack itemStack = new ItemStack();
					array[j] = itemStack.Read(_br);
				}
				this.bag.SetSlots(array, true);
			}
		}
		if ((syncFlags & 16) > 0)
		{
			this.performRepair();
		}
		if ((syncFlags & 32) > 0)
		{
			this.isQuietMode = _br.ReadBoolean();
			if (this.isQuietMode)
			{
				Handle handle = this.idleLoop;
				if (handle != null)
				{
					handle.Stop(this.entityId);
				}
				this.idleLoop = null;
			}
		}
		if ((syncFlags & 64) > 0)
		{
			this.IsFlashlightOn = _br.ReadBoolean();
			this.setFlashlightOn(this.IsFlashlightOn);
		}
		if ((syncFlags & 256) > 0)
		{
			this.setHealAllies(_br.ReadBoolean());
		}
		if ((syncFlags & 128) > 0)
		{
			this.OriginalItemValue.Read(_br);
			this.LoadMods();
		}
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x000E4BF2 File Offset: 0x000E2DF2
	public override void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
		this.bag.ItemGrid.Write(_bwServerContext, StreamModeWrite.ToClient);
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000E4C08 File Offset: 0x000E2E08
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		if (!_success)
		{
			GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), Localization.Get("ttVehicleInUse", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		this.bag.ItemGrid.ReadInto(_brServerContext, StreamModeRead.FromServer);
		this.startInteraction(this.transientLockCommand);
		return true;
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06002542 RID: 9538 RVA: 0x000E4C70 File Offset: 0x000E2E70
	// (set) Token: 0x06002543 RID: 9539 RVA: 0x000E4C78 File Offset: 0x000E2E78
	public int EntityId
	{
		get
		{
			return this.entityId;
		}
		set
		{
			this.entityId = value;
		}
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000E4C81 File Offset: 0x000E2E81
	public bool IsLocked()
	{
		return this.isLocked;
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x000E4C89 File Offset: 0x000E2E89
	public void SetLocked(bool _isLocked)
	{
		this.isLocked = _isLocked;
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000E4C92 File Offset: 0x000E2E92
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerSteamId;
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x000E4C9A File Offset: 0x000E2E9A
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerSteamId = _userIdentifier;
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x000E4CA3 File Offset: 0x000E2EA3
	public bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier)
	{
		return (_userIdentifier != null && _userIdentifier.Equals(this.ownerSteamId)) || this.allowedUsers.Contains(_userIdentifier) || this.IsOwner(_userIdentifier);
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x000E4CCD File Offset: 0x000E2ECD
	public List<PlatformUserIdentifierAbs> GetUsers()
	{
		return new List<PlatformUserIdentifierAbs>();
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x000E4CD4 File Offset: 0x000E2ED4
	public bool LocalPlayerIsOwner()
	{
		return this.IsOwner(PlatformManager.InternalLocalUserIdentifier);
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x000E4CE1 File Offset: 0x000E2EE1
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		if (this.ownerSteamId == null && this.OwnerID != null)
		{
			return this.OwnerID.Equals(_userIdentifier);
		}
		return this.ownerSteamId != null && this.ownerSteamId.Equals(_userIdentifier);
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x000E4D16 File Offset: 0x000E2F16
	public bool HasPassword()
	{
		return !string.IsNullOrEmpty(this.passwordHash);
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x000E4D26 File Offset: 0x000E2F26
	public string GetHashForPassword(string _password)
	{
		return Utils.HashString(_password);
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x000E4D2E File Offset: 0x000E2F2E
	public bool SetPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier)
	{
		if (this.LocalPlayerIsOwner() && _passwordHash != null)
		{
			if (_passwordHash != this.passwordHash)
			{
				this.passwordHash = _passwordHash;
				this.allowedUsers.Clear();
				if (this.ownerSteamId == null)
				{
					this.SetOwner(_userIdentifier);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x000E4D6D File Offset: 0x000E2F6D
	public bool CheckPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier)
	{
		if (this.LocalPlayerIsOwner() || !this.HasPassword())
		{
			this.SendSyncData(2);
			return true;
		}
		if (_passwordHash == this.passwordHash)
		{
			this.allowedUsers.Add(_userIdentifier);
			this.SendSyncData(2);
			return true;
		}
		return false;
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x000E4DAC File Offset: 0x000E2FAC
	public string GetPasswordHash()
	{
		return this.passwordHash;
	}

	// Token: 0x04001B25 RID: 6949
	public const string ClassName = "entityJunkDrone";

	// Token: 0x04001B26 RID: 6950
	public const string ItemName = "gunBotT3JunkDrone";

	// Token: 0x04001B27 RID: 6951
	public const int SaveVersion = 1;

	// Token: 0x04001B28 RID: 6952
	public const string cSupportModBuff = "buffJunkDroneSupportEffect";

	// Token: 0x04001B29 RID: 6953
	public static readonly FastTags<TagGroup.Global> StorageModifierTags = FastTags<TagGroup.Global>.Parse("droneStorage");

	// Token: 0x04001B2A RID: 6954
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cIdleAnimName = "Base Layer.Idle";

	// Token: 0x04001B2B RID: 6955
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cSpawnAnimName = "Base Layer.SpawnIn";

	// Token: 0x04001B2C RID: 6956
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static FastTags<TagGroup.Global> repairKitTags = FastTags<TagGroup.Global>.Parse("junk");

	// Token: 0x04001B2D RID: 6957
	public static bool DebugModeEnabled;

	// Token: 0x04001B2E RID: 6958
	public ItemValue OriginalItemValue;

	// Token: 0x04001B2F RID: 6959
	public PlatformUserIdentifierAbs OwnerID;

	// Token: 0x04001B30 RID: 6960
	public EntityAlive Owner;

	// Token: 0x04001B31 RID: 6961
	public const float cBaseFollowDistance = 5f;

	// Token: 0x04001B32 RID: 6962
	public const float cCombatFollowRange = 10f;

	// Token: 0x04001B33 RID: 6963
	public float FollowDistance = 5f;

	// Token: 0x04001B34 RID: 6964
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cAvoidRange = 2.5f;

	// Token: 0x04001B35 RID: 6965
	public float FollowHoverHeight = 1f;

	// Token: 0x04001B36 RID: 6966
	public float StayHoverHeight = 2f;

	// Token: 0x04001B37 RID: 6967
	public float SpeedPathing = 2f;

	// Token: 0x04001B38 RID: 6968
	public float SpeedFlying = 3f;

	// Token: 0x04001B39 RID: 6969
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cMaxSpeedFlying = 15f;

	// Token: 0x04001B3A RID: 6970
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cVehicleInventorySize = 45;

	// Token: 0x04001B3B RID: 6971
	public float RotationSpeed = 30f;

	// Token: 0x04001B3C RID: 6972
	public float AttackActionTime = 3f;

	// Token: 0x04001B3D RID: 6973
	public float HealActionTime = 7f;

	// Token: 0x04001B3E RID: 6974
	public DroneWeapons.HealBeamWeapon healWeapon;

	// Token: 0x04001B3F RID: 6975
	public DroneWeapons.StunBeamWeapon stunWeapon;

	// Token: 0x04001B40 RID: 6976
	public DroneWeapons.Weapon activeWeapon;

	// Token: 0x04001B41 RID: 6977
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<DroneWeapons.Weapon> installedWeapons = new List<DroneWeapons.Weapon>();

	// Token: 0x04001B42 RID: 6978
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool weaponDischarged;

	// Token: 0x04001B43 RID: 6979
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cAttackEnterTime = 1f;

	// Token: 0x04001B44 RID: 6980
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cAttackExitTime = 1.5f;

	// Token: 0x04001B45 RID: 6981
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float attackEnterTimer = 1f;

	// Token: 0x04001B46 RID: 6982
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float attackExitTimer = 1.5f;

	// Token: 0x04001B47 RID: 6983
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float accelerationTime;

	// Token: 0x04001B48 RID: 6984
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float decelerationTime;

	// Token: 0x04001B49 RID: 6985
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float armorDamageReduction = 1f;

	// Token: 0x04001B4A RID: 6986
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentSpeedFlying = 3f;

	// Token: 0x04001B4B RID: 6987
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone.State state;

	// Token: 0x04001B4C RID: 6988
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone.State lastState;

	// Token: 0x04001B4D RID: 6989
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone.State transitionState = EntityDrone.State.None;

	// Token: 0x04001B4E RID: 6990
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float stateTime;

	// Token: 0x04001B4F RID: 6991
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float stateMaxTime;

	// Token: 0x04001B50 RID: 6992
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 lastPosition;

	// Token: 0x04001B51 RID: 6993
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeSpentAtLocation;

	// Token: 0x04001B52 RID: 6994
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isVisible = true;

	// Token: 0x04001B53 RID: 6995
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive currentTarget;

	// Token: 0x04001B54 RID: 6996
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> currentPath = new List<Vector3>();

	// Token: 0x04001B55 RID: 6997
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone.EntitySteering steering;

	// Token: 0x04001B56 RID: 6998
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone.DroneSensors sensors;

	// Token: 0x04001B57 RID: 6999
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public FloodFillEntityPathGenerator pathMan;

	// Token: 0x04001B58 RID: 7000
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cInitSuppressVOTime = 5f;

	// Token: 0x04001B59 RID: 7001
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float initSuppressVOTimer = 5f;

	// Token: 0x04001B5A RID: 7002
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform head;

	// Token: 0x04001B5B RID: 7003
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color prefabColor;

	// Token: 0x04001B5C RID: 7004
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public DroneLightManager _lm;

	// Token: 0x04001B5D RID: 7005
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float creationTime;

	// Token: 0x04001B5E RID: 7006
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasNavObjectsEnabled;

	// Token: 0x04001B5F RID: 7007
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isOwnerSyncPending;

	// Token: 0x04001B60 RID: 7008
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ItemValue itemvalueToLoad;

	// Token: 0x04001B61 RID: 7009
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isBeingPickedUp;

	// Token: 0x04001B62 RID: 7010
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BoxCollider interactionCollider;

	// Token: 0x04001B63 RID: 7011
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string[] paintableParts = new string[] { "BaseMesh", "junkDroneArmRight", "armor" };

	// Token: 0x04001B64 RID: 7012
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool debugFriendlyFire;

	// Token: 0x04001B65 RID: 7013
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool debugShowCamera;

	// Token: 0x04001B66 RID: 7014
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject debugCamera;

	// Token: 0x04001B67 RID: 7015
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera reconCam;

	// Token: 0x04001B68 RID: 7016
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isQuietMode;

	// Token: 0x04001B69 RID: 7017
	public bool IsFlashlightAttached;

	// Token: 0x04001B6A RID: 7018
	public bool IsFlashlightOn;

	// Token: 0x04001B6B RID: 7019
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isSupportModAttached;

	// Token: 0x04001B6C RID: 7020
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Handle voHandle;

	// Token: 0x04001B6D RID: 7021
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Handle idleLoop;

	// Token: 0x04001B6E RID: 7022
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool partyEventsSet;

	// Token: 0x04001B6F RID: 7023
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int[] knownPartyMembers;

	// Token: 0x04001B70 RID: 7024
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float areaScanTime = 0.5f;

	// Token: 0x04001B71 RID: 7025
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float areaScanTimer = 0.5f;

	// Token: 0x04001B72 RID: 7026
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isInConfinedSpace;

	// Token: 0x04001B73 RID: 7027
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float debugInputRotX;

	// Token: 0x04001B74 RID: 7028
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float debugInputRotY;

	// Token: 0x04001B75 RID: 7029
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 debugInputFwd;

	// Token: 0x04001B76 RID: 7030
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 debugInputRgt;

	// Token: 0x04001B77 RID: 7031
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 debugInputUp;

	// Token: 0x04001B78 RID: 7032
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float debugInputSpeed = 3f;

	// Token: 0x04001B79 RID: 7033
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i debugOwnerPos;

	// Token: 0x04001B7A RID: 7034
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float overItemLimitCooldown;

	// Token: 0x04001B7B RID: 7035
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float retryPathTime = 0.5f;

	// Token: 0x04001B7C RID: 7036
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isTryingToFindPath;

	// Token: 0x04001B7D RID: 7037
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool userRequestedHeal;

	// Token: 0x04001B7E RID: 7038
	public bool isSystemSpawn;

	// Token: 0x04001B7F RID: 7039
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte debugDroneInitLogPriority = 0;

	// Token: 0x04001B80 RID: 7040
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte debugDroneAnimationLogPriority = 0;

	// Token: 0x04001B81 RID: 7041
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static byte debugDroneShutdownLogPriority = 0;

	// Token: 0x04001B82 RID: 7042
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float physColHeight = 0.6f;

	// Token: 0x04001B83 RID: 7043
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RaycastNode focusBoxNode;

	// Token: 0x04001B84 RID: 7044
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationStay = "drone_command_stay";

	// Token: 0x04001B85 RID: 7045
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationFollow = "drone_command_follow";

	// Token: 0x04001B86 RID: 7046
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationDontHealAllies = "drone_dont_heal_allies";

	// Token: 0x04001B87 RID: 7047
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationHealAllies = "drone_heal_allies";

	// Token: 0x04001B88 RID: 7048
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationLightOn = "drone_light_on";

	// Token: 0x04001B89 RID: 7049
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationLightOff = "drone_light_off";

	// Token: 0x04001B8A RID: 7050
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationSilentOn = "drone_silent_on";

	// Token: 0x04001B8B RID: 7051
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationSilentOff = "drone_silent_off";

	// Token: 0x04001B8C RID: 7052
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationHealMe = "drone_command_heal";

	// Token: 0x04001B8D RID: 7053
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationModPassive = "drone_attack_mode_passive";

	// Token: 0x04001B8E RID: 7054
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cActivationModAgressive = "drone_attack_mode_aggressive";

	// Token: 0x04001B8F RID: 7055
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;

	// Token: 0x04001B90 RID: 7056
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isAnimationStateSet;

	// Token: 0x04001B91 RID: 7057
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cWakeUpTime = 2.5f;

	// Token: 0x04001B92 RID: 7058
	public float WakeupAnimTime;

	// Token: 0x04001B94 RID: 7060
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool registeredOwnerHandlers;

	// Token: 0x04001B95 RID: 7061
	public List<int> registeredPartyMembers;

	// Token: 0x04001B96 RID: 7062
	public Vector3 SentryPos;

	// Token: 0x04001B97 RID: 7063
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone.Orders orderState;

	// Token: 0x04001B98 RID: 7064
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cOwnerFocusTime = 0.2f;

	// Token: 0x04001B99 RID: 7065
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float ownerFocusTimer;

	// Token: 0x04001B9A RID: 7066
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool ownerIsOnVehicle;

	// Token: 0x04001B9B RID: 7067
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone.AttackMode attackMode;

	// Token: 0x04001B9C RID: 7068
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cNotifyNeedsHealItemCooldown = 30f;

	// Token: 0x04001B9D RID: 7069
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cNotifyNeedsHealMaxNotifyCount = 2;

	// Token: 0x04001B9E RID: 7070
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float needsHealItemTimer;

	// Token: 0x04001B9F RID: 7071
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int needsHealNotifyCount;

	// Token: 0x04001BA0 RID: 7072
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool IsHealingAllies;

	// Token: 0x04001BA1 RID: 7073
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone.AllyHealMode allyHealMode;

	// Token: 0x04001BA2 RID: 7074
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<EntityAlive> healTargetsInRange = new List<EntityAlive>();

	// Token: 0x04001BA3 RID: 7075
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cTeleportAtkCooldownTime = 5f;

	// Token: 0x04001BA4 RID: 7076
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float teleportAtkCooldownTimer;

	// Token: 0x04001BA5 RID: 7077
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isShutdown;

	// Token: 0x04001BA6 RID: 7078
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isGrounded;

	// Token: 0x04001BA7 RID: 7079
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 fallPoint;

	// Token: 0x04001BA8 RID: 7080
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasFallPoint;

	// Token: 0x04001BA9 RID: 7081
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i fallBlockPos;

	// Token: 0x04001BAA RID: 7082
	public const int cPathLayer = 1073807360;

	// Token: 0x04001BAB RID: 7083
	public const float cFollowHoverHeight = 1f;

	// Token: 0x04001BAC RID: 7084
	public const float cAddPathDist = 1.414f;

	// Token: 0x04001BAD RID: 7085
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 blockHeightOffset = new Vector3(0f, 0.5f, 0f);

	// Token: 0x04001BAE RID: 7086
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityDrone.PathTracker pathTracker = new EntityDrone.PathTracker();

	// Token: 0x04001BAF RID: 7087
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currentPathTarget;

	// Token: 0x04001BB0 RID: 7088
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 currentPathDest;

	// Token: 0x04001BB1 RID: 7089
	public bool DebugEnemiesInRange;

	// Token: 0x04001BB2 RID: 7090
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncReplicate = 2;

	// Token: 0x04001BB3 RID: 7091
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const byte cSyncVersion = 3;

	// Token: 0x04001BB4 RID: 7092
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncOwnerKey = 1;

	// Token: 0x04001BB5 RID: 7093
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncInteractAndSecurity = 2;

	// Token: 0x04001BB6 RID: 7094
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncAction = 4;

	// Token: 0x04001BB7 RID: 7095
	public const ushort cSyncStorage = 8;

	// Token: 0x04001BB8 RID: 7096
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncOrderState = 16384;

	// Token: 0x04001BB9 RID: 7097
	public const ushort cSyncState = 32768;

	// Token: 0x04001BBA RID: 7098
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const byte cSyncInteractAndSecurityFLocked = 2;

	// Token: 0x04001BBB RID: 7099
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncRepairAction = 16;

	// Token: 0x04001BBC RID: 7100
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncQuietMode = 32;

	// Token: 0x04001BBD RID: 7101
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncLightMod = 64;

	// Token: 0x04001BBE RID: 7102
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncService = 128;

	// Token: 0x04001BBF RID: 7103
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const ushort cSyncHealAllies = 256;

	// Token: 0x04001BC0 RID: 7104
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isShutdownPending;

	// Token: 0x04001BC1 RID: 7105
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string transientLockCommand;

	// Token: 0x04001BC2 RID: 7106
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isLocked;

	// Token: 0x04001BC3 RID: 7107
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string passwordHash = string.Empty;

	// Token: 0x04001BC4 RID: 7108
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<PlatformUserIdentifierAbs> allowedUsers = new List<PlatformUserIdentifierAbs>();

	// Token: 0x04001BC5 RID: 7109
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public PlatformUserIdentifierAbs ownerSteamId;

	// Token: 0x020004A2 RID: 1186
	public class SoundKeys
	{
		// Token: 0x04001BC6 RID: 7110
		public const string cIdleHover = "drone_idle_hover";

		// Token: 0x04001BC7 RID: 7111
		public const string cFly = "drone_fly";

		// Token: 0x04001BC8 RID: 7112
		public const string cCommand = "drone_command";

		// Token: 0x04001BC9 RID: 7113
		public const string cEmpty = "drone_empty";

		// Token: 0x04001BCA RID: 7114
		public const string cEnemySense = "drone_enemy_sense";

		// Token: 0x04001BCB RID: 7115
		public const string cEnemyEngauge = "drone_enemy_engauge";

		// Token: 0x04001BCC RID: 7116
		public const string cDroneOther = "drone_other";

		// Token: 0x04001BCD RID: 7117
		public const string cShutDown = "drone_shutdown";

		// Token: 0x04001BCE RID: 7118
		public const string cTake = "drone_take";

		// Token: 0x04001BCF RID: 7119
		public const string cTakeFail = "drone_takefail";

		// Token: 0x04001BD0 RID: 7120
		public const string cWakeUp = "drone_wakeup";

		// Token: 0x04001BD1 RID: 7121
		public const string cGreeting = "drone_greeting";
	}

	// Token: 0x020004A3 RID: 1187
	[PublicizedFrom(EAccessModifier.Private)]
	public class ModKeys
	{
		// Token: 0x04001BD2 RID: 7122
		public const string cStorageMod = "modRoboticDroneCargoMod";

		// Token: 0x04001BD3 RID: 7123
		public const string cArmorMod = "modRoboticDroneArmorPlatingMod";

		// Token: 0x04001BD4 RID: 7124
		public const string cHealMod = "modRoboticDroneMedicMod";

		// Token: 0x04001BD5 RID: 7125
		public const string cStunMod = "modRoboticDroneStunWeaponMod";

		// Token: 0x04001BD6 RID: 7126
		public const string cGunMod = "modRoboticDroneWeaponMod";

		// Token: 0x04001BD7 RID: 7127
		public const string cMoraleMod = "modRoboticDroneMoraleBoosterMod";

		// Token: 0x04001BD8 RID: 7128
		public const string cHeadlampMod = "modRoboticDroneHeadlampMod";

		// Token: 0x04001BD9 RID: 7129
		public const string cHeadlampLightName = "junkDroneLamp";
	}

	// Token: 0x020004A4 RID: 1188
	public enum State
	{
		// Token: 0x04001BDB RID: 7131
		Idle,
		// Token: 0x04001BDC RID: 7132
		Sentry,
		// Token: 0x04001BDD RID: 7133
		Follow,
		// Token: 0x04001BDE RID: 7134
		Heal,
		// Token: 0x04001BDF RID: 7135
		Attack,
		// Token: 0x04001BE0 RID: 7136
		Shutdown,
		// Token: 0x04001BE1 RID: 7137
		NoClip,
		// Token: 0x04001BE2 RID: 7138
		Teleport,
		// Token: 0x04001BE3 RID: 7139
		None
	}

	// Token: 0x020004A5 RID: 1189
	[PublicizedFrom(EAccessModifier.Private)]
	public enum LoggingTypes
	{
		// Token: 0x04001BE5 RID: 7141
		Any,
		// Token: 0x04001BE6 RID: 7142
		Init,
		// Token: 0x04001BE7 RID: 7143
		Animation,
		// Token: 0x04001BE8 RID: 7144
		Shutdown
	}

	// Token: 0x020004A6 RID: 1190
	public enum Orders
	{
		// Token: 0x04001BEA RID: 7146
		Follow,
		// Token: 0x04001BEB RID: 7147
		Stay
	}

	// Token: 0x020004A7 RID: 1191
	public enum AttackMode
	{
		// Token: 0x04001BED RID: 7149
		Passive,
		// Token: 0x04001BEE RID: 7150
		Aggressive
	}

	// Token: 0x020004A8 RID: 1192
	public enum AllyHealMode
	{
		// Token: 0x04001BF0 RID: 7152
		DoNotHeal,
		// Token: 0x04001BF1 RID: 7153
		HealAllies
	}

	// Token: 0x020004A9 RID: 1193
	[Preserve]
	public class PathTracker
	{
		// Token: 0x06002557 RID: 9559 RVA: 0x000E4FB5 File Offset: 0x000E31B5
		public bool IsStuck(Vector3 pos, Vector3 target, float time = 0.5f, bool debugDraw = false)
		{
			return this.IsStuckInBlock(pos, time, debugDraw) || this.IsNotAbleToReachTarget(target);
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000E4FD4 File Offset: 0x000E31D4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsStuckInBlock(Vector3 position, float time = 0.5f, bool debugDraw = false)
		{
			this.timeInBlock += Time.deltaTime;
			this.currentBlockPosition.FloorToInt(position);
			if (debugDraw)
			{
				RaycastPathUtils.DrawBounds(this.currentBlockPosition, Color.green, 0.05f, 1f);
			}
			if (this.currentBlockPosition != this.lastBlockPosition)
			{
				this.lastBlockPosition = this.currentBlockPosition;
				this.timeInBlock = 0f;
			}
			return this.timeInBlock > time;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000E5054 File Offset: 0x000E3254
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsNotAbleToReachTarget(Vector3 currentTarget)
		{
			this.timeSpentToNextTarget += Time.deltaTime;
			if (this.targetDestination != currentTarget)
			{
				this.targetDestination = currentTarget;
				this.timeSpentToNextTarget = 0f;
			}
			if (this.timeSpentToNextTarget > 1f)
			{
				this.timeSpentToNextTarget = 0f;
				return true;
			}
			return false;
		}

		// Token: 0x04001BF2 RID: 7154
		public Vector3i currentBlockPosition;

		// Token: 0x04001BF3 RID: 7155
		public Vector3i lastBlockPosition;

		// Token: 0x04001BF4 RID: 7156
		public float timeInBlock;

		// Token: 0x04001BF5 RID: 7157
		[PublicizedFrom(EAccessModifier.Private)]
		public float timeSpentToNextTarget;

		// Token: 0x04001BF6 RID: 7158
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 targetDestination;
	}

	// Token: 0x020004AA RID: 1194
	[Preserve]
	public class NetPackageDroneDataSync : NetPackage
	{
		// Token: 0x0600255B RID: 9563 RVA: 0x000E50B0 File Offset: 0x000E32B0
		public EntityDrone.NetPackageDroneDataSync Setup(EntityDrone _ev, int _senderId, ushort _syncFlags)
		{
			this.senderId = _senderId;
			this.vehicleId = _ev.entityId;
			this.syncFlags = _syncFlags;
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(this.entityData);
				_ev.WriteSyncData(pooledBinaryWriter, _syncFlags);
			}
			return this;
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000E5114 File Offset: 0x000E3314
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~NetPackageDroneDataSync()
		{
			MemoryPools.poolMemoryStream.FreeSync(this.entityData);
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000E514C File Offset: 0x000E334C
		public override void read(PooledBinaryReader _br)
		{
			this.senderId = _br.ReadInt32();
			this.vehicleId = _br.ReadInt32();
			this.syncFlags = _br.ReadUInt16();
			int num = (int)_br.ReadUInt16();
			StreamUtils.StreamCopy(_br.BaseStream, this.entityData, num, null, true);
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000E5198 File Offset: 0x000E3398
		public override void write(PooledBinaryWriter _bw)
		{
			base.write(_bw);
			_bw.Write(this.senderId);
			_bw.Write(this.vehicleId);
			_bw.Write(this.syncFlags);
			_bw.Write((ushort)this.entityData.Length);
			this.entityData.WriteTo(_bw.BaseStream);
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000E51F4 File Offset: 0x000E33F4
		public override void ProcessPackage(World _world, GameManager _callbacks)
		{
			if (_world == null)
			{
				return;
			}
			EntityDrone entityDrone = GameManager.Instance.World.GetEntity(this.vehicleId) as EntityDrone;
			if (entityDrone == null)
			{
				return;
			}
			if (this.entityData.Length > 0L)
			{
				PooledExpandableMemoryStream pooledExpandableMemoryStream = this.entityData;
				lock (pooledExpandableMemoryStream)
				{
					this.entityData.Position = 0L;
					try
					{
						using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader.SetBaseStream(this.entityData);
							entityDrone.ReadSyncData(pooledBinaryReader, this.syncFlags, this.senderId);
						}
					}
					catch (Exception ex)
					{
						Log.Exception(ex);
						string text = "Error syncing data for entity ";
						EntityDrone entityDrone2 = entityDrone;
						Log.Error(text + ((entityDrone2 != null) ? entityDrone2.ToString() : null) + "; Sender id = " + this.senderId.ToString());
					}
				}
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				ushort syncFlagsReplicated = entityDrone.GetSyncFlagsReplicated(this.syncFlags);
				if (syncFlagsReplicated != 0)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<EntityDrone.NetPackageDroneDataSync>().Setup(entityDrone, this.senderId, syncFlagsReplicated), false, -1, this.senderId, -1, null, 192, false);
				}
			}
		}

		// Token: 0x04001BF7 RID: 7159
		[PublicizedFrom(EAccessModifier.Private)]
		public int senderId;

		// Token: 0x04001BF8 RID: 7160
		[PublicizedFrom(EAccessModifier.Private)]
		public int vehicleId;

		// Token: 0x04001BF9 RID: 7161
		[PublicizedFrom(EAccessModifier.Private)]
		public ushort syncFlags;

		// Token: 0x04001BFA RID: 7162
		[PublicizedFrom(EAccessModifier.Private)]
		public PooledExpandableMemoryStream entityData = MemoryPools.poolMemoryStream.AllocSync(true);
	}

	// Token: 0x020004AB RID: 1195
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class DroneSensors
	{
		// Token: 0x06002561 RID: 9569 RVA: 0x000E5365 File Offset: 0x000E3565
		public DroneSensors(EntityDrone _drone)
		{
			this.drone = _drone;
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init()
		{
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000E53A0 File Offset: 0x000E35A0
		public void Update()
		{
			if (this.enemyDetectedBarkTimer > 0f)
			{
				this.enemyDetectedBarkTimer -= 0.05f;
				if (this.enemyDetectedBarkTimer <= 0f)
				{
					this.canBarkEnemyDetected = true;
				}
			}
			if (this.IsEnemyInRange() && this.canBarkEnemyDetected)
			{
				this.barkEnemyDetected();
			}
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000E53F6 File Offset: 0x000E35F6
		public bool IsEnemyInRange()
		{
			return this.drone && this.GetNearestEnemyInRange(this.drone.position) != null;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000E541E File Offset: 0x000E361E
		public EntityAlive GetNearestEnemyInRange(Vector3 targetPos)
		{
			return this.GetNearestEnemyInRange(targetPos, this.EnemyDetectionRadius);
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000E5430 File Offset: 0x000E3630
		public EntityAlive GetNearestEnemyInRange(Vector3 targetPos, float weaponRange)
		{
			EntityAlive revengeTarget = this.drone.GetRevengeTarget();
			if (revengeTarget && !revengeTarget.Buffs.HasBuff("buffShocked"))
			{
				return revengeTarget;
			}
			EntityEnemy entityEnemy = null;
			float num = float.MaxValue;
			this.entitiesInRange.Clear();
			GameManager.Instance.World.GetEntitiesAround(EntityFlags.Player | EntityFlags.Zombie | EntityFlags.Animal | EntityFlags.Bandit, targetPos, this.EnemyDetectionRadius, this.entitiesInRange);
			for (int i = 0; i < this.entitiesInRange.Count; i++)
			{
				EntityEnemy entityEnemy2 = this.entitiesInRange[i] as EntityEnemy;
				if (entityEnemy2 && entityEnemy2.EntityClass != null && entityEnemy2.EntityClass.bIsEnemyEntity && this.canAttackTarget(entityEnemy2))
				{
					float sqrMagnitude = (targetPos - entityEnemy2.position).sqrMagnitude;
					if (sqrMagnitude < num && sqrMagnitude < weaponRange * weaponRange)
					{
						num = sqrMagnitude;
						entityEnemy = entityEnemy2;
					}
				}
			}
			return entityEnemy;
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000E5514 File Offset: 0x000E3714
		public bool IsOwnerAttackTarget()
		{
			if (this.drone && this.drone.Owner)
			{
				this.entitiesInRange.Clear();
				GameManager.Instance.World.GetEntitiesAround(EntityFlags.Player | EntityFlags.Zombie | EntityFlags.Animal | EntityFlags.Bandit, this.drone.position, this.EnemyDetectionRadius, this.entitiesInRange);
				for (int i = 0; i < this.entitiesInRange.Count; i++)
				{
					EntityEnemy entityEnemy = this.entitiesInRange[i] as EntityEnemy;
					if (entityEnemy && this.canAttackTarget(entityEnemy) && entityEnemy.GetAttackTarget() == this.drone.Owner)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000E55D0 File Offset: 0x000E37D0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool canAttackTarget(EntityAlive enemy)
		{
			return !enemy.IsDead() && (!enemy.IsSleeper || !enemy.IsSleeping) && !enemy.Buffs.HasBuff("buffShocked") && !EntityDrone.IsPositionBlocked(this.drone.position, enemy.getChestPosition(), 1073807360, false, 0f);
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000E5634 File Offset: 0x000E3834
		[PublicizedFrom(EAccessModifier.Private)]
		public void barkEnemyDetected()
		{
			if (this.drone)
			{
				if (this.drone.Owner)
				{
					Manager.Stop(this.drone.Owner.entityId, "drone_take");
				}
				if (this.drone.state == EntityDrone.State.Shutdown)
				{
					return;
				}
				this.drone.playVO("drone_enemy_sense", true, 1f);
				this.enemyDetectedBarkTimer = this.EnemyDetectedBarkCooldown;
				this.canBarkEnemyDetected = false;
			}
		}

		// Token: 0x04001BFB RID: 7163
		public float EnemyDetectionRadius = 22f;

		// Token: 0x04001BFC RID: 7164
		public float EnemyDetectedBarkCooldown = 90f;

		// Token: 0x04001BFD RID: 7165
		[PublicizedFrom(EAccessModifier.Private)]
		public float enemyDetectedBarkTimer = 10f;

		// Token: 0x04001BFE RID: 7166
		[PublicizedFrom(EAccessModifier.Private)]
		public bool canBarkEnemyDetected;

		// Token: 0x04001BFF RID: 7167
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityDrone drone;

		// Token: 0x04001C00 RID: 7168
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> entitiesInRange = new List<Entity>();
	}

	// Token: 0x020004AC RID: 1196
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class SteeringMan
	{
		// Token: 0x0600256A RID: 9578 RVA: 0x000E56B2 File Offset: 0x000E38B2
		public Vector3 Seek(Vector3 pos, Vector3 target, float slowingRadius)
		{
			return this.doSeek(pos, target, slowingRadius);
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000E56BD File Offset: 0x000E38BD
		public Vector3 Seek2D(Vector3 pos, Vector3 target, float slowingRadius)
		{
			return this.doSeek2D(pos, target, slowingRadius);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000E56C8 File Offset: 0x000E38C8
		public Vector3 Flee(Vector3 pos, Vector3 target, float avoidRadius)
		{
			return this.doFlee(pos, target, avoidRadius);
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000E56D3 File Offset: 0x000E38D3
		public Vector3 Flee2D(Vector3 pos, Vector3 target, float avoidRadius)
		{
			return this.doFlee2D(pos, target, avoidRadius);
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000E56DE File Offset: 0x000E38DE
		public Vector3 GetDir(Vector3 from, Vector3 to)
		{
			return this.getDirVector(from, to);
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000E56E8 File Offset: 0x000E38E8
		public Vector3 GetDir2D(Vector3 from, Vector3 to)
		{
			Vector3 vector = new Vector3(from.x, 0f, from.z);
			Vector3 vector2 = new Vector3(to.x, 0f, to.z);
			return this.getDirVector(vector, vector2);
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000E572D File Offset: 0x000E392D
		public bool IsInRange(Vector3 from, Vector3 to, float dist)
		{
			return this.isInRange(from, to, dist);
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000E5738 File Offset: 0x000E3938
		public bool IsInRange2D(Vector3 from, Vector3 to, float dist)
		{
			return this.isInRange2D(from, to, dist);
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000E5743 File Offset: 0x000E3943
		public Vector3 GetPointAround(Vector3 lhs, Vector3 rhs, float radius)
		{
			return this.getPointAround(lhs, rhs, radius);
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000E574E File Offset: 0x000E394E
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 getVec(Vector3 pos, Vector3 target)
		{
			return target - pos;
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000E5758 File Offset: 0x000E3958
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 getDirVector(Vector3 pos, Vector3 target)
		{
			return this.getVec(pos, target).normalized;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000E5778 File Offset: 0x000E3978
		[PublicizedFrom(EAccessModifier.Private)]
		public float getDist(Vector3 pos, Vector3 target)
		{
			return this.getVec(pos, target).magnitude;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000E5798 File Offset: 0x000E3998
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isInRange(Vector3 from, Vector3 to, float dist)
		{
			return (from - to).sqrMagnitude < dist * dist;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000E57BC File Offset: 0x000E39BC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isInRange2D(Vector3 from, Vector3 to, float dist)
		{
			Vector3 vector = new Vector3(from.x, 0f, from.z);
			Vector3 vector2 = new Vector3(to.x, 0f, to.z);
			return this.isInRange(vector, vector2, dist);
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000E5802 File Offset: 0x000E3A02
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 getPointAround(Vector3 lhs, Vector3 rhs, float radius)
		{
			return Vector3.Cross(lhs, rhs) * radius * 0.5f;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000E581C File Offset: 0x000E3A1C
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 doSeek(Vector3 pos, Vector3 target, float radius)
		{
			float dist = this.getDist(pos, target);
			if (dist < radius)
			{
				return this.getDirVector(pos, target) * (dist / radius);
			}
			return this.getDirVector(pos, target);
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000E5850 File Offset: 0x000E3A50
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 doSeek2D(Vector3 pos, Vector3 target, float radius)
		{
			Vector3 vector = this.doSeek(pos, target, radius);
			vector.y = 0f;
			return vector;
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000E5874 File Offset: 0x000E3A74
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 doFlee(Vector3 pos, Vector3 target, float radius)
		{
			return -this.doSeek(pos, target, radius);
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000E5884 File Offset: 0x000E3A84
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 doFlee2D(Vector3 pos, Vector3 target, float radius)
		{
			Vector3 vector = this.doFlee(pos, target, radius);
			vector.y = 0f;
			return vector;
		}

		// Token: 0x04001C01 RID: 7169
		[PublicizedFrom(EAccessModifier.Protected)]
		public const int kMaxDistance = 1000;
	}

	// Token: 0x020004AD RID: 1197
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class EntitySteering : EntityDrone.SteeringMan
	{
		// Token: 0x0600257E RID: 9598 RVA: 0x000E58A8 File Offset: 0x000E3AA8
		public EntitySteering(EntityAlive _entity)
		{
			this.entity = _entity;
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000E58B7 File Offset: 0x000E3AB7
		public Vector3 Hover(float height, float slowingRadius = 1f)
		{
			return this.doHover(this.entity.position, height, slowingRadius);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000E58CC File Offset: 0x000E3ACC
		public Vector3 FollowPlayer(Vector3 playerPos, Vector3 playerLookDir, float followDist, float degrees = 90f, float maxDist = 15f)
		{
			return this.followTarget(this.entity.position, playerPos, playerLookDir, followDist, degrees, maxDist);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000E58E6 File Offset: 0x000E3AE6
		public Vector3 AvoidArc(Vector3 fromPos, Vector3 toPos, Vector3 dir, Vector3 up, bool subtract, float degrees, float maxDist = 1000f)
		{
			return this.doAvoidArc(fromPos, toPos, dir, up, subtract, degrees, maxDist);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000E58F9 File Offset: 0x000E3AF9
		public Vector3 AvoidArc2D(Vector3 fromPos, Vector3 toPos, Vector3 dir, bool subtract, float degrees, float maxDist = 1000f)
		{
			return this.doAvoidArc2D(fromPos, toPos, dir, subtract, degrees, maxDist);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000E590A File Offset: 0x000E3B0A
		public Vector3 AvoidTargetView(EntityAlive target, float followDist, bool subtract, float degrees = 90f, float maxDist = 15f)
		{
			return this.avoidTargetView(this.entity.position, target.getHeadPosition(), target.GetLookVector(), followDist, subtract, degrees, maxDist);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000E5930 File Offset: 0x000E3B30
		public Vector3 FollowTarget(EntityAlive target, Vector3 viewDir, float followDist, bool subtract, float degrees = 90f, float maxDist = 15f)
		{
			return this.pursueAvoidOwnerView(this.entity.position, target.getHeadPosition(), viewDir, Vector3.zero, followDist, subtract, degrees, maxDist);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000E5961 File Offset: 0x000E3B61
		public bool IsInRange(Vector3 target, float dist)
		{
			return base.IsInRange(this.entity.position, target, dist);
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000E5976 File Offset: 0x000E3B76
		public bool IsInRange2D(Vector3 target, float dist)
		{
			return base.IsInRange2D(this.entity.position, target, dist);
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000E598B File Offset: 0x000E3B8B
		public bool IsInRange2D(EntityAlive target, float dist)
		{
			return base.IsInRange2D(this.entity.position, target.position, dist);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000E59A5 File Offset: 0x000E3BA5
		public float GetYPos(float height)
		{
			return this.getYPos(this.entity.position, height);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000E59B9 File Offset: 0x000E3BB9
		public float GetAltitude(Vector3 pos)
		{
			return this.getAltitude(pos);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000E59C2 File Offset: 0x000E3BC2
		public bool IsAboveGround(Vector3 pos)
		{
			return this.getAltitude(pos) > -1f;
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000E59D2 File Offset: 0x000E3BD2
		public float GetCeiling(Vector3 pos)
		{
			return this.getCeiling(pos);
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000E59DB File Offset: 0x000E3BDB
		public bool IsBelowCeiling(Vector3 pos)
		{
			return this.getCeiling(pos) > -1f;
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000E59EC File Offset: 0x000E3BEC
		[PublicizedFrom(EAccessModifier.Private)]
		public float getAltitude(Vector3 pos)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(pos - Origin.position, Vector3.down, out raycastHit, 1000f, 65536))
			{
				return raycastHit.distance;
			}
			return -1f;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000E5A2C File Offset: 0x000E3C2C
		[PublicizedFrom(EAccessModifier.Private)]
		public float getCeiling(Vector3 pos)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(pos - Origin.position, Vector3.up, out raycastHit, 1000f, 65536))
			{
				return raycastHit.distance;
			}
			return -1f;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000E5A6C File Offset: 0x000E3C6C
		[PublicizedFrom(EAccessModifier.Private)]
		public float getYPos(Vector3 pos, float height)
		{
			float altitude = this.getAltitude(pos);
			if (altitude >= 0f)
			{
				return pos.y - altitude + height;
			}
			return -1f;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000E5A9C File Offset: 0x000E3C9C
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 doHover(Vector3 pos, float height, float radius)
		{
			float altitude = this.getAltitude(pos);
			if (altitude <= 0f)
			{
				return Vector3.zero;
			}
			Vector3 vector = ((altitude < height) ? Vector3.up : Vector3.down);
			float num = Mathf.Abs(height - altitude);
			if (num < radius)
			{
				return vector * (num / radius);
			}
			return vector;
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000E5AE8 File Offset: 0x000E3CE8
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 followTarget(Vector3 pos, Vector3 target, Vector3 lookDir, float followDist, float degrees, float maxDist)
		{
			return base.Seek(pos, target, followDist).normalized;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000E5B08 File Offset: 0x000E3D08
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 avoidTargetView(Vector3 pos, Vector3 target, Vector3 lookDir, float followDist, bool subtract, float degrees, float maxDist)
		{
			return this.AvoidArc2D(pos, target, lookDir, subtract, degrees, maxDist).normalized;
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000E5B2C File Offset: 0x000E3D2C
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 pursueAvoidOwnerView(Vector3 pos, Vector3 target, Vector3 lookDir, Vector3 offSet, float followDist, bool subtract, float degrees, float maxDist)
		{
			Vector3 vector = base.Seek(pos, target, followDist);
			Vector3 vector2 = this.AvoidArc2D(pos, target, lookDir, subtract, degrees, maxDist);
			return (vector + vector2).normalized;
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000E5B64 File Offset: 0x000E3D64
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 doAvoidArc(Vector3 from, Vector3 to, Vector3 dir, Vector3 up, bool subtract, float degrees, float maxDist)
		{
			Vector3 vector = from - to;
			if (Vector3.Angle(dir, vector) < degrees * 0.5f)
			{
				Vector3 vector2 = base.GetPointAround((to - from).normalized, up, maxDist);
				vector2 = (subtract ? (to - vector2) : (to + vector2));
				if (base.IsInRange(from, vector2, maxDist))
				{
					return base.Flee(from, vector2 + dir * vector.magnitude, 0f);
				}
			}
			return Vector3.zero;
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000E5BEC File Offset: 0x000E3DEC
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 doAvoidArc2D(Vector3 from, Vector3 to, Vector3 dir, bool subtract, float degrees, float maxDist)
		{
			Vector3 vector = this.doAvoidArc(from, to, dir, Vector3.up, subtract, degrees, maxDist);
			vector.y = 0f;
			return vector;
		}

		// Token: 0x04001C02 RID: 7170
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityAlive entity;
	}
}
