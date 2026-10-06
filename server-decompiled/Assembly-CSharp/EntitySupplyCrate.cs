using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004DF RID: 1247
[Preserve]
public class EntitySupplyCrate : EntityAlive
{
	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06002844 RID: 10308 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsValidAimAssistSnapTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x000FA6B5 File Offset: 0x000F88B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.hasAI = false;
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x000FA6C4 File Offset: 0x000F88C4
	public override void PostInit()
	{
		base.PostInit();
		this.ValidateResources();
		base.gameObject.layer = 21;
		Collider component = base.GetComponent<Collider>();
		if (component)
		{
			component.enabled = false;
			component.enabled = true;
		}
		if (this.wasOnGround)
		{
			this.StopSmokeAndLights();
			if (this.parachuteT)
			{
				this.parachuteT.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06002847 RID: 10311 RVA: 0x000FA733 File Offset: 0x000F8933
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitLocalActivationCommands(Action<EntityActivationCommand> _addCallback)
	{
		_addCallback(new EntityActivationCommand("search", "search", null, null));
	}

	// Token: 0x06002848 RID: 10312 RVA: 0x000FA74C File Offset: 0x000F894C
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "search"))
		{
			return this.bag != null && !this.IsDead();
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x000FA778 File Offset: 0x000F8978
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityActivated(EntityActivationCommand _command, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_command.commandId, "search"))
		{
			LockManager.Instance.LockRequestLocal(this, 0);
		}
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x000FA7A0 File Offset: 0x000F89A0
	public override string GetActivationText()
	{
		if (this.bag == null)
		{
			return string.Empty;
		}
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
		string text2 = Localization.Get(this.LocalizedEntityName, false, null);
		if (!this.bag.Touched)
		{
			return string.Format(Localization.Get("lootTooltipNew", false, null), text, text2);
		}
		if (this.bag.IsEmpty())
		{
			return string.Format(Localization.Get("lootTooltipEmpty", false, null), text, text2);
		}
		return string.Format(Localization.Get("lootTooltipTouched", false, null), text, text2);
	}

	// Token: 0x0600284B RID: 10315 RVA: 0x000FA87C File Offset: 0x000F8A7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleNavObject()
	{
		if (GameStats.GetBool(EnumGameStats.AirDropMarker))
		{
			NavObjectManager.Instance.UnRegisterNavObjectByEntityID(this.entityId);
			if (EntityClass.list[this.entityClass].NavObject != "")
			{
				this.NavObject = NavObjectManager.Instance.RegisterNavObject(EntityClass.list[this.entityClass].NavObject, this, "", false);
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Vector3 vector = this.NavObject.GetPosition() + Origin.position;
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup(this.NavObject.NavObjectClass.NavObjectClassName, this.NavObject.DisplayName, vector, true, this.NavObject.usingLocalizationId, this.entityId), false, -1, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x000FA969 File Offset: 0x000F8B69
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.startRotY = this.rotation.y;
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000FA982 File Offset: 0x000F8B82
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		if (this.unloadReason == EnumRemoveEntityReason.Killed && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.World.aiDirector.GetComponent<AIDirectorAirDropComponent>().RemoveSupplyCrate(this.entityId);
		}
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x000FA9BE File Offset: 0x000F8BBE
	public override EnumMapObjectType GetMapObjectType()
	{
		return EnumMapObjectType.SupplyDrop;
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetMotionMultiplier(float _motionMultiplier)
	{
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x000FA9C2 File Offset: 0x000F8BC2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void fallHitGround(float _v, Vector3 _fallMotion)
	{
		base.fallHitGround(Mathf.Min(_v, 5f), new Vector3(_fallMotion.x, Mathf.Max(-0.75f, _fallMotion.y), _fallMotion.z));
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x000FA9F8 File Offset: 0x000F8BF8
	public override void MoveEntityHeaded(Vector3 _direction, bool _isDirAbsolute)
	{
		base.MoveEntityHeaded(_direction, _isDirAbsolute);
		if (this.AttachedToEntity != null)
		{
			return;
		}
		if (((EModelSupplyCrate)this.emodel).parachute.gameObject.activeSelf && !base.IsInWater())
		{
			this.motion.y = this.motion.y + base.ScalePhysicsAddConstant(this.world.Gravity * 0.95f);
		}
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x000FAA66 File Offset: 0x000F8C66
	public bool RequiresChunkObserver()
	{
		return !this.onGround || this.isSmokeOn;
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x000FAA78 File Offset: 0x000F8C78
	[PublicizedFrom(EAccessModifier.Private)]
	public void ValidateResources()
	{
		if (!this.crateT)
		{
			this.crateT = base.transform.FindInChilds("SupplyCrateEntityPrefab", false);
		}
		if (!this.parachuteT)
		{
			this.parachuteT = base.transform.FindInChilds("parachute_supplies", false);
		}
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x000FAAD0 File Offset: 0x000F8CD0
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		if (this.showParachuteInTicks > 0)
		{
			this.showParachuteInTicks--;
		}
		if (this.closeParachuteInTicks > 0)
		{
			this.closeParachuteInTicks--;
		}
		if (!this.onGround && this.wasOnGround)
		{
			this.showParachuteInTicks = 10;
		}
		if (this.onGround && !this.wasOnGround)
		{
			this.closeParachuteInTicks = 10;
		}
		if ((this.onGround || base.IsInWater()) && this.closeParachuteInTicks <= 0)
		{
			((EModelSupplyCrate)this.emodel).parachute.gameObject.SetActive(false);
		}
		if (this.onGround && !this.wasOnGround)
		{
			float lightBrightness = this.world.GetLightBrightness(base.GetBlockPosition());
			GameManager.Instance.SpawnParticleEffectClient(new ParticleEffect("supply_crate_impact", base.GetPosition(), Quaternion.identity, lightBrightness, Color.white), this.entityId, false, false);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				AIDirectorAirDropComponent component = GameManager.Instance.World.aiDirector.GetComponent<AIDirectorAirDropComponent>();
				component.SetSupplyCratePosition(this.entityId, World.worldToBlockPos(this.position));
				component.RefreshCrates(-1);
			}
		}
		this.wasOnGround = this.onGround;
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x000FAC10 File Offset: 0x000F8E10
	public override bool CanUpdateEntity()
	{
		return this.isEntityRemote || base.CanUpdateEntity();
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x000FAC24 File Offset: 0x000F8E24
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		float time = Time.time;
		if (!this.onGround)
		{
			Vector3 vector;
			vector.x = Mathf.Sin(time) * 8f - 4f;
			vector.y = Mathf.Sin(time + 0.3f) * 8f - 4f + this.startRotY;
			vector.z = 0f;
			this.ModelTransform.localEulerAngles = vector;
			this.SetRotation(vector);
		}
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x000FACA4 File Offset: 0x000F8EA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void StopSmokeAndLights()
	{
		this.isSmokeOn = false;
		Transform modelTransform = this.emodel.GetModelTransform();
		List<Transform> list = new List<Transform>();
		GameUtils.FindTagInChilds(modelTransform, "SupplySmoke", list);
		for (int i = list.Count - 1; i >= 0; i--)
		{
			ParticleSystem[] componentsInChildren = list[i].GetComponentsInChildren<ParticleSystem>();
			for (int j = componentsInChildren.Length - 1; j >= 0; j--)
			{
				componentsInChildren[j].main.loop = false;
			}
		}
		list.Clear();
		GameUtils.FindTagInChilds(modelTransform, "SupplyLit", list);
		for (int k = 0; k < list.Count; k++)
		{
			list[k].gameObject.SetActive(false);
		}
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isRadiationSensitive()
	{
		return false;
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canDespawn()
	{
		return false;
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsSavedToFile()
	{
		return true;
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWithBlocks()
	{
		return false;
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x000FAD58 File Offset: 0x000F8F58
	public override void OnEntityDeath()
	{
		base.OnEntityDeath();
		GameManager.Instance.World.ObjectOnMapRemove(EnumMapObjectType.SupplyDrop, this.entityId);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityMapMarkerRemove>().Setup(EnumMapObjectType.SupplyDrop, this.entityId), false, -1, -1, -1, null, 192, false);
			base.DropBagServer();
		}
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanBePushed()
	{
		return false;
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWith(Entity _other)
	{
		return false;
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x000FADC3 File Offset: 0x000F8FC3
	public override void Read(byte _version, PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_version, _br, _eStreamMode);
		if (_version > 11)
		{
			this.wasOnGround = _br.ReadBoolean();
			this.closeParachuteInTicks = _br.ReadInt32();
			this.showParachuteInTicks = _br.ReadInt32();
		}
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x000FADF7 File Offset: 0x000F8FF7
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		_bw.Write(this.wasOnGround);
		_bw.Write(this.closeParachuteInTicks);
		_bw.Write(this.showParachuteInTicks);
	}

	// Token: 0x04001E4E RID: 7758
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startRotY;

	// Token: 0x04001E4F RID: 7759
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public new bool wasOnGround;

	// Token: 0x04001E50 RID: 7760
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int showParachuteInTicks;

	// Token: 0x04001E51 RID: 7761
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int closeParachuteInTicks;

	// Token: 0x04001E52 RID: 7762
	public bool isSmokeOn = true;

	// Token: 0x04001E53 RID: 7763
	public float smokeTimeAfterLanding = 240f;

	// Token: 0x04001E54 RID: 7764
	public float smokeTimeOnGround;

	// Token: 0x04001E55 RID: 7765
	public float smokeTimer;

	// Token: 0x04001E56 RID: 7766
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform crateT;

	// Token: 0x04001E57 RID: 7767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform parachuteT;
}
