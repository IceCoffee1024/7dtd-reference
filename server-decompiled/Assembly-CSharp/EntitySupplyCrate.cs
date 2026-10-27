using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004E1 RID: 1249
[Preserve]
public class EntitySupplyCrate : EntityAlive
{
	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x0600284B RID: 10315 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsValidAimAssistSnapTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x000FABED File Offset: 0x000F8DED
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.hasAI = false;
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000FABFC File Offset: 0x000F8DFC
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

	// Token: 0x0600284E RID: 10318 RVA: 0x000FAC6B File Offset: 0x000F8E6B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitLocalActivationCommands(Action<EntityActivationCommand> _addCallback)
	{
		_addCallback(new EntityActivationCommand("search", "search", null, null));
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x000FAC84 File Offset: 0x000F8E84
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "search"))
		{
			return this.bag != null && !this.IsDead();
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x000FACB0 File Offset: 0x000F8EB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityActivated(EntityActivationCommand _command, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_command.commandId, "search"))
		{
			LockManager.Instance.LockRequestLocal(this, 0);
		}
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x000FACD8 File Offset: 0x000F8ED8
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

	// Token: 0x06002852 RID: 10322 RVA: 0x000FADB4 File Offset: 0x000F8FB4
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

	// Token: 0x06002853 RID: 10323 RVA: 0x000FAEA1 File Offset: 0x000F90A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.startRotY = this.rotation.y;
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x000FAEBA File Offset: 0x000F90BA
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		if (this.unloadReason == EnumRemoveEntityReason.Killed && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.World.aiDirector.GetComponent<AIDirectorAirDropComponent>().RemoveSupplyCrate(this.entityId);
		}
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x000FAEF6 File Offset: 0x000F90F6
	public override EnumMapObjectType GetMapObjectType()
	{
		return EnumMapObjectType.SupplyDrop;
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetMotionMultiplier(float _motionMultiplier)
	{
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x000FAEFA File Offset: 0x000F90FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void fallHitGround(float _v, Vector3 _fallMotion)
	{
		base.fallHitGround(Mathf.Min(_v, 5f), new Vector3(_fallMotion.x, Mathf.Max(-0.75f, _fallMotion.y), _fallMotion.z));
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x000FAF30 File Offset: 0x000F9130
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

	// Token: 0x06002859 RID: 10329 RVA: 0x000FAF9E File Offset: 0x000F919E
	public bool RequiresChunkObserver()
	{
		return !this.onGround || this.isSmokeOn;
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x000FAFB0 File Offset: 0x000F91B0
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

	// Token: 0x0600285B RID: 10331 RVA: 0x000FB008 File Offset: 0x000F9208
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

	// Token: 0x0600285C RID: 10332 RVA: 0x000FB148 File Offset: 0x000F9348
	public override bool CanUpdateEntity()
	{
		return this.isEntityRemote || base.CanUpdateEntity();
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x000FB15C File Offset: 0x000F935C
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

	// Token: 0x0600285E RID: 10334 RVA: 0x000FB1DC File Offset: 0x000F93DC
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

	// Token: 0x0600285F RID: 10335 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isRadiationSensitive()
	{
		return false;
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canDespawn()
	{
		return false;
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsSavedToFile()
	{
		return true;
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWithBlocks()
	{
		return false;
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x000FB290 File Offset: 0x000F9490
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

	// Token: 0x06002865 RID: 10341 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanBePushed()
	{
		return false;
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWith(Entity _other)
	{
		return false;
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000FB2FB File Offset: 0x000F94FB
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

	// Token: 0x06002868 RID: 10344 RVA: 0x000FB32F File Offset: 0x000F952F
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		_bw.Write(this.wasOnGround);
		_bw.Write(this.closeParachuteInTicks);
		_bw.Write(this.showParachuteInTicks);
	}

	// Token: 0x04001E5E RID: 7774
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startRotY;

	// Token: 0x04001E5F RID: 7775
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public new bool wasOnGround;

	// Token: 0x04001E60 RID: 7776
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int showParachuteInTicks;

	// Token: 0x04001E61 RID: 7777
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int closeParachuteInTicks;

	// Token: 0x04001E62 RID: 7778
	public bool isSmokeOn = true;

	// Token: 0x04001E63 RID: 7779
	public float smokeTimeAfterLanding = 240f;

	// Token: 0x04001E64 RID: 7780
	public float smokeTimeOnGround;

	// Token: 0x04001E65 RID: 7781
	public float smokeTimer;

	// Token: 0x04001E66 RID: 7782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform crateT;

	// Token: 0x04001E67 RID: 7783
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform parachuteT;
}
