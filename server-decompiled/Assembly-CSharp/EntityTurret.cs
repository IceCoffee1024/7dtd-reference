using System;
using System.IO;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004DB RID: 1243
[Preserve]
public class EntityTurret : EntityAlive
{
	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06002872 RID: 10354 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsValidAimAssistSnapTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06002873 RID: 10355 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsValidAimAssistSlowdownTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06002874 RID: 10356 RVA: 0x000EB306 File Offset: 0x000E9506
	public override string LocalizedEntityName
	{
		get
		{
			return Localization.Get(this.EntityName, false, null);
		}
	}

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06002875 RID: 10357 RVA: 0x000FAEBB File Offset: 0x000F90BB
	// (set) Token: 0x06002876 RID: 10358 RVA: 0x000FAEC8 File Offset: 0x000F90C8
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

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06002877 RID: 10359 RVA: 0x000FAED6 File Offset: 0x000F90D6
	public bool IsTurning
	{
		get
		{
			return this.IsOn && (this.YawController.IsTurning || this.PitchController.IsTurning);
		}
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06002878 RID: 10360 RVA: 0x000FAEFC File Offset: 0x000F90FC
	// (set) Token: 0x06002879 RID: 10361 RVA: 0x000FAF21 File Offset: 0x000F9121
	public override int Health
	{
		get
		{
			return (int)Mathf.Max((float)this.OriginalItemValue.MaxUseTimes - this.OriginalItemValue.UseTimes, 1f);
		}
		set
		{
			this.OriginalItemValue.UseTimes = (float)(this.OriginalItemValue.MaxUseTimes - value);
		}
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x000FAF3C File Offset: 0x000F913C
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		EntityClass entityClass = EntityClass.list[this.entityClass];
		base.transform.tag = "E_Vehicle";
		Transform transform = base.transform;
		this.thisRigidBody = transform.GetComponent<Rigidbody>();
		if (this.thisRigidBody)
		{
			this.thisRigidBody.centerOfMass = new Vector3(0f, 0.1f, 0f);
			this.thisRigidBody.sleepThreshold = this.thisRigidBody.mass * 0.01f * 0.01f * 0.5f;
			transform.gameObject.AddComponent<CollisionCallForward>().Entity = this;
			transform.gameObject.layer = 21;
			Utils.SetTagsRecursively(transform, "E_Vehicle");
		}
		this.alertEnabled = false;
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x000FB00A File Offset: 0x000F920A
	public override void Kill(DamageResponse _dmResponse)
	{
		_dmResponse.Fatal = false;
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetDead()
	{
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x000FB00A File Offset: 0x000F920A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ClientKill(DamageResponse _dmResponse)
	{
		_dmResponse.Fatal = false;
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x000FB014 File Offset: 0x000F9214
	[PublicizedFrom(EAccessModifier.Protected)]
	public override DamageResponse damageEntityLocal(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale)
	{
		DamageResponse damageResponse = base.damageEntityLocal(_damageSource, _strength, _criticalHit, impulseScale);
		damageResponse.Fatal = false;
		return damageResponse;
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000FB038 File Offset: 0x000F9238
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		this.IsOn = false;
		if (GameManager.Instance != null && GameManager.Instance.World != null && this.belongsPlayerId != -1)
		{
			EntityAlive entityAlive = (EntityAlive)GameManager.Instance.World.GetEntity(this.belongsPlayerId);
			if (entityAlive != null)
			{
				entityAlive.RemoveOwnedEntity(this.entityId);
			}
		}
		this.FireController.Update();
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddCharacterController()
	{
	}

	// Token: 0x06002881 RID: 10369 RVA: 0x000FB0B0 File Offset: 0x000F92B0
	public override void PostInit()
	{
		Transform transform = base.transform;
		transform.rotation = this.qrotation;
		this.StaticPosition = this.position;
		this.fallPos = this.position;
		this.YawController = transform.GetComponentInChildren<AutoTurretYawLerp>();
		this.PitchController = transform.GetComponentInChildren<AutoTurretPitchLerp>();
		this.FireController = transform.GetComponentInChildren<MiniTurretFireController>();
		this.Laser = transform.FindInChilds("turret_laser", false);
		this.Cone = transform.FindInChilds("turret_cone", false);
		PersistentPlayerData playerData = GameManager.Instance.GetPersistentPlayerList().GetPlayerData(this.OwnerID);
		if (playerData != null)
		{
			this.belongsPlayerId = playerData.EntityId;
		}
		this.HandleNavObject();
		this.InitTurret();
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x000FB161 File Offset: 0x000F9361
	public override void InitInventory()
	{
		this.inventory = new EntityTurret.TurretInventory(GameManager.Instance, this);
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x000FB174 File Offset: 0x000F9374
	public void InitTurret()
	{
		this.FireController.Init(base.EntityClass.Properties, this);
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000FB190 File Offset: 0x000F9390
	public override void OnAddedToWorld()
	{
		base.OnAddedToWorld();
		if (this.OriginalItemValue.HasMods())
		{
			for (int i = 0; i < this.OriginalItemValue.Modifications.Length; i++)
			{
				ItemValue itemValue = this.OriginalItemValue.Modifications[i];
				if (itemValue.ItemClass != null && itemValue.ItemClass.Name == "modMeleeClubBurningShaft")
				{
					Transform transform = base.transform.FindInChilds("mod_junk_sledge_flamePrefab", false);
					if (transform)
					{
						transform.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x000FB21C File Offset: 0x000F941C
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
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
			if (this.Owner != null)
			{
				this.Owner.AddOwnedEntity(this);
			}
		}
		if (this.uloam == null && this.OriginalItemValue.ItemClass != null)
		{
			this.uloam = base.gameObject.AddMissingComponent<UpdateLightOnAllMaterials>();
			this.uloam.AddRendererNameToIgnore("turret_laser");
			this.uloam.SetTintColorForItem(Vector3.one);
			if (this.OriginalItemValue.ItemClass.Properties.Values.ContainsKey(Block.PropTintColor))
			{
				this.uloam.SetTintColorForItem(Block.StringToVector3(this.OriginalItemValue.GetPropertyOverride(Block.PropTintColor, this.OriginalItemValue.ItemClass.Properties.Values[Block.PropTintColor])));
			}
			else
			{
				this.uloam.SetTintColorForItem(Block.StringToVector3(this.OriginalItemValue.GetPropertyOverride(Block.PropTintColor, "255,255,255")));
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.IsOn = this.OriginalItemValue.PercentUsesLeft > 0f;
			if ((int)EffectManager.GetValue(PassiveEffects.MagazineSize, this.OriginalItemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0)
			{
				this.IsOn &= this.OriginalItemValue.Meta > 0;
			}
			if (GameManager.Instance != null && GameManager.Instance.World != null && this.belongsPlayerId != -1)
			{
				this.IsOn &= this.Owner != null;
				if (this.Owner != null)
				{
					if (EffectManager.GetValue(PassiveEffects.DisableItem, this.OriginalItemValue, 0f, this.Owner, null, this.OriginalItemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
					{
						this.IsOn = false;
					}
					else
					{
						this.maxOwnerDistance = (int)EffectManager.GetValue(PassiveEffects.JunkTurretActiveRange, this.OriginalItemValue, 10f, this.Owner, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
						if (this.IsOn)
						{
							this.DistanceToOwner = base.GetDistanceSq(this.Owner);
							this.IsOn &= this.DistanceToOwner < (float)(this.maxOwnerDistance * this.maxOwnerDistance);
						}
						if (this.IsOn)
						{
							int num = (int)EffectManager.GetValue(PassiveEffects.JunkTurretActiveCount, this.OriginalItemValue, 1f, this.Owner, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
							int num2 = 0;
							for (int i = 0; i < this.Owner.ownedEntities.Count; i++)
							{
								EntityTurret entityTurret = GameManager.Instance.World.GetEntity(this.Owner.ownedEntities[i].Id) as EntityTurret;
								if (!(entityTurret == null) && entityTurret.entityId != this.entityId)
								{
									if (entityTurret.IsOn)
									{
										num2++;
									}
									this.IsOn &= num2 <= num || this.DistanceToOwner < entityTurret.DistanceToOwner || this.ForceOn;
									if (!this.IsOn)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
			else if (this.IsOn)
			{
				this.IsOn &= this.belongsPlayerId == -1 && this.OwnerID == null;
			}
			this.ForceOn = false;
			if (this.TargetEntityId != this.lastTargetEntityId || this.IsOn != this.lastIsOn || this.OriginalItemValue.Equals(this.lastOriginalItemValue))
			{
				this.lastOriginalItemValue = this.OriginalItemValue.Clone();
				this.lastTargetEntityId = this.TargetEntityId;
				this.lastIsOn = this.IsOn;
				NetPackageTurretSync netPackageTurretSync = NetPackageManager.GetPackage<NetPackageTurretSync>().Setup(this.entityId, this.TargetEntityId, this.IsOn, this.OriginalItemValue);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageTurretSync, true, -1, -1, -1, null, 192, false);
			}
		}
		if (this.Laser != null && this.IsOn != this.Laser.gameObject.activeSelf)
		{
			this.Laser.gameObject.SetActive(this.IsOn);
		}
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000027FC File Offset: 0x000009FC
	public override void MoveEntityHeaded(Vector3 _direction, bool _isDirAbsolute)
	{
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000FB6E8 File Offset: 0x000F98E8
	public void InitDynamicSpawn()
	{
		for (int i = 1; i < ItemClass.list.Length - 1; i++)
		{
			if (ItemClass.list[i] != null)
			{
				string name = ItemClass.list[i].Name;
				if (name == "gunBotT1JunkSledge" || name == "gunBotT2JunkTurret")
				{
					this.OwnerID = PlatformManager.InternalLocalUserIdentifier;
					this.OriginalItemValue = new ItemValue(ItemClass.list[i].Id, false);
					this.AmmoCount = ItemClass.GetForId(ItemClass.list[i].Id).GetInitialMetadata(this.OriginalItemValue);
					this.ForceOn = true;
					PersistentPlayerData playerData = GameManager.Instance.GetPersistentPlayerList().GetPlayerData(this.OwnerID);
					if (playerData != null)
					{
						(GameManager.Instance.World.GetEntity(playerData.EntityId) as EntityAlive).AddOwnedEntity(this);
					}
				}
			}
		}
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000FB7CC File Offset: 0x000F99CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTransform()
	{
		Vector3 vector = base.transform.position + Origin.position;
		this.position = vector;
		this.StaticPosition = vector;
		Chunk chunk = GameManager.Instance.World.GetChunkFromWorldPos((int)vector.x, (int)vector.y, (int)vector.z) as Chunk;
		if (chunk != null && chunk.IsCollisionMeshGenerated)
		{
			if (this.posCheckTimer <= 0f)
			{
				this.posCheckTimer = 0.5f;
				int modelLayer = base.GetModelLayer();
				this.SetModelLayer(2, false, null);
				float y = this.fallPos.y;
				this.fallPos = vector;
				Ray ray = new Ray(vector + Vector3.up * 0.375f, Vector3.down);
				if (Voxel.Raycast(GameManager.Instance.World, ray, 255f, 1082195969, 128, 0.25f))
				{
					this.groundUpDirection = Voxel.phyxRaycastHit.normal;
					this.fallPos.y = Voxel.voxelRayHitInfo.fmcHit.pos.y;
					if (Vector3.Dot(Vector3.up, this.groundUpDirection) < 0.7f)
					{
						this.fallPos.y = this.fallPos.y - 0.1f;
					}
					if (this.fallPos.y < y)
					{
						this.fallDelay = 5;
					}
				}
				this.SetModelLayer(modelLayer, false, null);
			}
			float deltaTime = Time.deltaTime;
			this.posCheckTimer -= deltaTime;
			this.isFalling = false;
			if (vector != this.fallPos)
			{
				this.posCheckTimer = 0f;
				int num = this.fallDelay - 1;
				this.fallDelay = num;
				if (num < 0)
				{
					this.isFalling = true;
					base.transform.position = Vector3.MoveTowards(base.transform.position, this.fallPos - Origin.position, 5f * deltaTime);
					return;
				}
			}
		}
		else
		{
			this.posCheckTimer = 0.5f;
		}
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000FB9CA File Offset: 0x000F9BCA
	public override void OnCollectServer(int _playerId)
	{
		this.OriginalItemValue = ItemValue.None;
		this.PickedUpWaitingToDelete = true;
		this.bPlayerStatsChanged = true;
		base.transform.gameObject.SetActive(false);
		this.world.RemoveEntity(this.entityId, EnumRemoveEntityReason.Killed);
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x000FBA0C File Offset: 0x000F9C0C
	public override void OnCollectLocal(int _playerId)
	{
		EntityPlayerLocal entityPlayerLocal = this.world.GetEntity(_playerId) as EntityPlayerLocal;
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		ItemStack itemStack = new ItemStack(this.OriginalItemValue, 1);
		if (!uiforPlayer.xui.PlayerInventory.AddItem(itemStack))
		{
			GameManager.Instance.ItemDropServer(itemStack, entityPlayerLocal.GetPosition(), Vector3.zero, _playerId, 60f, false);
		}
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x000FBA6D File Offset: 0x000F9C6D
	public bool CanInteract(int _interactingEntityId)
	{
		return !this.isFalling && !this.PickedUpWaitingToDelete && this.OriginalItemValue.type != 0 && (this.belongsPlayerId == _interactingEntityId || this.Health <= 1);
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x000FBAA8 File Offset: 0x000F9CA8
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
		PlayerActionsLocal playerInput = entityPlayerLocal2.playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		return string.Format(Localization.Get("turretPickUp", false, null), text, this.LocalizedEntityName);
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x000FBB28 File Offset: 0x000F9D28
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitLocalActivationCommands(Action<EntityActivationCommand> _addCallback)
	{
		_addCallback(new EntityActivationCommand("take", "hand", null, null));
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x000FBB41 File Offset: 0x000F9D41
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "take"))
		{
			return this.CanInteract(_playerFocusing.entityId);
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000FBB68 File Offset: 0x000F9D68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityActivated(EntityActivationCommand _command, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_command.commandId, "take"))
		{
			ItemStack itemStack = new ItemStack(this.OriginalItemValue, 1);
			if (_playerFocusing.inventory.CanTakeItem(itemStack) || _playerFocusing.bag.CanTakeItem(itemStack))
			{
				base.Collect(_playerFocusing.entityId);
				return;
			}
			GameManager.ShowTooltip(_playerFocusing, Localization.Get("xuiInventoryFullForPickup", false, null), string.Empty, "ui_denied", null, false, false, 0f);
		}
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDead()
	{
		return false;
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000FBBE7 File Offset: 0x000F9DE7
	public override void Write(BinaryWriter _bw, bool _bNetworkWrite)
	{
		base.Write(_bw, _bNetworkWrite);
		_bw.Write(1);
		this.OwnerID.ToStream(_bw, false);
		this.OriginalItemValue.Write(_bw);
		StreamUtils.Write(_bw, this.StaticPosition);
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000FBC20 File Offset: 0x000F9E20
	public override void Read(byte _version, BinaryReader _br)
	{
		base.Read(_version, _br);
		int num = _br.ReadInt32();
		this.OwnerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.OriginalItemValue = ItemValue.None;
		this.OriginalItemValue.Read(_br);
		if (num > 0)
		{
			this.StaticPosition = StreamUtils.ReadVector3(_br);
		}
	}

	// Token: 0x04001E4C RID: 7756
	public const int SaveVersion = 1;

	// Token: 0x04001E4D RID: 7757
	public const string JunkTurretSledgeItem = "gunBotT1JunkSledge";

	// Token: 0x04001E4E RID: 7758
	public const string JunkTurretRangedItem = "gunBotT2JunkTurret";

	// Token: 0x04001E4F RID: 7759
	public AutoTurretYawLerp YawController;

	// Token: 0x04001E50 RID: 7760
	public AutoTurretPitchLerp PitchController;

	// Token: 0x04001E51 RID: 7761
	public MiniTurretFireController FireController;

	// Token: 0x04001E52 RID: 7762
	public Transform Laser;

	// Token: 0x04001E53 RID: 7763
	public Transform Cone;

	// Token: 0x04001E54 RID: 7764
	public Material ConeMaterial;

	// Token: 0x04001E55 RID: 7765
	public Color ConeColor;

	// Token: 0x04001E56 RID: 7766
	public float CenteredYaw;

	// Token: 0x04001E57 RID: 7767
	public float CenteredPitch;

	// Token: 0x04001E58 RID: 7768
	public bool TargetOwner;

	// Token: 0x04001E59 RID: 7769
	public bool TargetAllies;

	// Token: 0x04001E5A RID: 7770
	public bool TargetStrangers = true;

	// Token: 0x04001E5B RID: 7771
	public bool TargetEnemies = true;

	// Token: 0x04001E5C RID: 7772
	public int maxOwnerDistance = 10;

	// Token: 0x04001E5D RID: 7773
	public ItemValue OriginalItemValue = ItemValue.None;

	// Token: 0x04001E5E RID: 7774
	public bool PickedUpWaitingToDelete;

	// Token: 0x04001E5F RID: 7775
	public PlatformUserIdentifierAbs OwnerID;

	// Token: 0x04001E60 RID: 7776
	public bool IsOn;

	// Token: 0x04001E61 RID: 7777
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody thisRigidBody;

	// Token: 0x04001E62 RID: 7778
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UpdateLightOnAllMaterials uloam;

	// Token: 0x04001E63 RID: 7779
	public EntityAlive Owner;

	// Token: 0x04001E64 RID: 7780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lastTargetEntityId = -2;

	// Token: 0x04001E65 RID: 7781
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lastIsOn;

	// Token: 0x04001E66 RID: 7782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ItemValue lastOriginalItemValue = ItemValue.None;

	// Token: 0x04001E67 RID: 7783
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPOSITION_UPDATE_CHECK_TIME = 0.5f;

	// Token: 0x04001E68 RID: 7784
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float posCheckTimer = 0.5f;

	// Token: 0x04001E69 RID: 7785
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int fallDelay;

	// Token: 0x04001E6A RID: 7786
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 fallPos;

	// Token: 0x04001E6B RID: 7787
	public int TargetEntityId = -1;

	// Token: 0x04001E6C RID: 7788
	public bool ForceOn;

	// Token: 0x04001E6D RID: 7789
	public float DistanceToOwner = float.MaxValue;

	// Token: 0x04001E6E RID: 7790
	public Vector3 groundPosition;

	// Token: 0x04001E6F RID: 7791
	public Vector3 groundUpDirection;

	// Token: 0x04001E70 RID: 7792
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isFalling;

	// Token: 0x04001E71 RID: 7793
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 StaticPosition;

	// Token: 0x04001E72 RID: 7794
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLerpTimeScale = 8f;

	// Token: 0x04001E73 RID: 7795
	public int tmpBelongsPlayerID;

	// Token: 0x020004DC RID: 1244
	public class TurretInventory : Inventory
	{
		// Token: 0x06002894 RID: 10388 RVA: 0x000FBCD4 File Offset: 0x000F9ED4
		public TurretInventory(IGameManager _gameManager, EntityAlive _entity)
			: base(_gameManager, _entity)
		{
			this.cSlotCount = base.PUBLIC_SLOTS + 1;
			this.SetupSlots();
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Execute(int _actionIdx, bool _bReleased, PlayerActionsLocal _playerActions = null)
		{
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000FBCF2 File Offset: 0x000F9EF2
		public void SetupSlots()
		{
			this.slots = new ItemInventoryData[this.cSlotCount];
			this.models = new Transform[this.cSlotCount];
			this.m_HoldingItemIdx = 0;
			base.Clear();
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void updateHoldingItem()
		{
		}

		// Token: 0x04001E74 RID: 7796
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int cSlotCount;
	}
}
