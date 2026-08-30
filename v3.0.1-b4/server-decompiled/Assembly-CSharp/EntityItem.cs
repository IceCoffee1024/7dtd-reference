using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004B5 RID: 1205
[Preserve]
public class EntityItem : Entity
{
	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x0600262A RID: 9770 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override Entity.EnumPositionUpdateMovementType positionUpdateMovementType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return Entity.EnumPositionUpdateMovementType.Instant;
		}
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x000E963B File Offset: 0x000E783B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitLocalActivationCommands(Action<EntityActivationCommand> _addCallback)
	{
		_addCallback(new EntityActivationCommand("take", "hand", null, null));
		_addCallback(new EntityActivationCommand("search", "search", null, null));
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x000E966C File Offset: 0x000E786C
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "take"))
		{
			return this.CanCollect() && this.onGround;
		}
		if (base.CommandIs(_commandName, "search"))
		{
			return this.bag != null;
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x000E96B8 File Offset: 0x000E78B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityActivated(EntityActivationCommand _command, EntityPlayerLocal _playerFocusing)
	{
		if (!base.CommandIs(_command.commandId, "take"))
		{
			if (base.CommandIs(_command.commandId, "search"))
			{
				LockManager.Instance.LockRequestLocal(this, new Entity.EntityLockContext(_command.commandId.ToString(), this.bag), 0);
			}
			return;
		}
		if (_playerFocusing.inventory.CanTakeItem(this.itemStack) || _playerFocusing.bag.CanTakeItem(this.itemStack))
		{
			base.Collect(_playerFocusing.entityId);
			return;
		}
		GameManager.ShowTooltip(_playerFocusing, Localization.Get("xuiInventoryFullForPickup", false, null), string.Empty, "ui_denied", null, false, false, 0f);
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x000E9770 File Offset: 0x000E7970
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
		if (this.CanCollect() && this.onGround)
		{
			string localizedItemName = ItemClass.GetForId(this.itemStack.itemValue.type).GetLocalizedItemName();
			if (this.itemStack.count > 1)
			{
				return string.Format(Localization.Get("itemTooltipFocusedSeveral", false, null), text, localizedItemName, this.itemStack.count);
			}
			return string.Format(Localization.Get("itemTooltipFocusedOne", false, null), text, localizedItemName);
		}
		else
		{
			if (this.bag == null)
			{
				return string.Empty;
			}
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
	}

	// Token: 0x0600262F RID: 9775 RVA: 0x000E98C0 File Offset: 0x000E7AC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		this.usePhysicsMaster = true;
		this.isPhysicsMaster = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
		base.Awake();
		this.yOffset = 0.15f;
		EntityItem.ItemInstanceCount++;
		Collider component = base.GetComponent<Collider>();
		if (component)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06002630 RID: 9776 RVA: 0x000E9918 File Offset: 0x000E7B18
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~EntityItem()
	{
		EntityItem.ItemInstanceCount--;
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x000E994C File Offset: 0x000E7B4C
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		this.itemRB = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06002632 RID: 9778 RVA: 0x000E9964 File Offset: 0x000E7B64
	public override void PostInit()
	{
		base.PostInit();
		base.PhysicsSetRB(this.itemRB);
		base.transform.eulerAngles = this.rotation;
		if (this.itemClass != null)
		{
			this.stickPercent = this.itemClass.Properties.GetFloat("StickPercent");
			if (this.itemStack != null)
			{
				this.itemWorldData = this.itemClass.CreateWorldData(GameManager.Instance, this, this.itemStack.itemValue, this.belongsPlayerId);
			}
		}
	}

	// Token: 0x06002633 RID: 9779 RVA: 0x000E99E8 File Offset: 0x000E7BE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleNavObject()
	{
		if (!this.bWasThrown)
		{
			return;
		}
		if (this.itemClass != null && this.OwnerId != -1 && this.world.GetEntity(this.OwnerId) as EntityPlayerLocal != null && this.itemClass.NavObject != "")
		{
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(this.itemClass.NavObject, base.transform, "", false);
		}
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x000E9A6C File Offset: 0x000E7C6C
	public void SetItemStack(ItemStack _itemStack)
	{
		if (this.itemStack == null)
		{
			this.itemStack = ItemStack.Empty;
		}
		this.lastCachedItemStack = this.itemStack.Clone();
		this.itemStack = _itemStack;
		this.itemClass = ItemClass.GetForId(this.itemStack.itemValue.type);
		this.distractionRadiusSq = EffectManager.GetValue(PassiveEffects.DistractionRadius, this.itemStack.itemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.distractionRadiusSq *= this.distractionRadiusSq;
		this.distractionLifetime = Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.DistractionLifetime, this.itemStack.itemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		this.distractionEatTicks = Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.DistractionEatTicks, this.itemStack.itemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		this.distractionStrength = EffectManager.GetValue(PassiveEffects.DistractionStrength, this.itemStack.itemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public new void FixedUpdate()
	{
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x000E9B9C File Offset: 0x000E7D9C
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		if (!this.bMeshCreated)
		{
			this.createMesh();
		}
		if (this.itemWorldData != null)
		{
			this.itemClass.OnDroppedUpdate(this.itemWorldData);
		}
		if (Utils.FastAbs(this.position.y - this.prevPos.y) < 0.1f)
		{
			this.onGroundCounter++;
			if (this.onGroundCounter > 10)
			{
				this.onGround = true;
			}
		}
		if (this.isPhysicsMaster && !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && (this.ticksExisted & 1) != 0)
		{
			base.PhysicsMasterSendToServer(base.transform);
		}
		this.checkGravitySetting(this.isPhysicsMaster);
		if (this.isEntityRemote)
		{
			return;
		}
		if (!this.itemTransform)
		{
			this.lifetime = 0f;
		}
		this.lifetime -= 0.05f;
		if (this.lifetime <= 0f)
		{
			this.SetDead();
		}
		if (this.itemClass != null && this.itemClass.IsEatDistraction && this.distractionLifetime > 0 && this.distractionEatTicks <= 0)
		{
			this.SetDead();
		}
		if (base.transform.position.y + Origin.position.y < 0f)
		{
			this.SetDead();
		}
		if (!this.IsDead())
		{
			this.tickDistraction();
		}
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x000E9CF7 File Offset: 0x000E7EF7
	[PublicizedFrom(EAccessModifier.Private)]
	public void playThrowSound(string _name)
	{
		Manager.Play(this, "throw" + _name, 1f, false);
		Manager.Play(this, "throwdefault", 1f, false);
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x000E9D23 File Offset: 0x000E7F23
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale = 1f)
	{
		if (_strength >= 99999)
		{
			this.lifetime = 0f;
		}
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x000E9D44 File Offset: 0x000E7F44
	public override void OnDamagedByExplosion()
	{
		if (this.itemWorldData != null)
		{
			ItemClass forId = ItemClass.GetForId(this.itemStack.itemValue.type);
			if (forId != null)
			{
				forId.OnDamagedByExplosion(this.itemWorldData);
			}
		}
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x000E9D80 File Offset: 0x000E7F80
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createMesh()
	{
		if (this.itemStack.itemValue.type == 0 || this.itemClass == null)
		{
			Log.Error(string.Format("Could not create item with id {0}", this.itemStack.itemValue.type));
			this.SetDead();
			return;
		}
		if (this.meshGameObject != null && this.lastCachedItemStack.itemValue.type != 0)
		{
			global::UnityEngine.Object.Destroy(this.meshGameObject);
			this.meshGameObject = null;
		}
		this.itemTransform = null;
		float num = 0f;
		Vector3 zero = Vector3.zero;
		if (this.itemClass.IsBlock())
		{
			BlockValue blockValue = this.itemStack.itemValue.ToBlockValue(false);
			if (this.itemTransform == null)
			{
				this.itemTransform = this.itemClass.CloneModel(this.meshGameObject, this.world, blockValue, null, Vector3.zero, base.transform, BlockShape.MeshPurpose.Drop, default(TextureFullArray));
			}
			Block block = blockValue.Block;
			if (block.Properties.Values.ContainsKey("DropScale"))
			{
				num = StringParsers.ParseFloat(block.Properties.Values["DropScale"], 0, -1, NumberStyles.Any);
			}
		}
		else
		{
			if (this.itemTransform == null)
			{
				this.itemTransform = this.itemClass.CloneModel(this.world, this.itemStack.itemValue, Vector3.zero, base.transform, BlockShape.MeshPurpose.Drop, default(TextureFullArray));
			}
			if (this.itemClass.Properties.Values.ContainsKey("DropScale"))
			{
				num = StringParsers.ParseFloat(this.itemClass.Properties.Values["DropScale"], 0, -1, NumberStyles.Any);
			}
		}
		if (num != 0f)
		{
			this.itemTransform.localScale = new Vector3(num, num, num);
		}
		this.itemTransform.localEulerAngles = this.itemClass.GetDroppedCorrectionRotation();
		this.itemTransform.localPosition = zero;
		bool flag = true;
		Collider[] componentsInChildren = this.itemTransform.GetComponentsInChildren<Collider>();
		int num2 = 0;
		while (componentsInChildren != null && num2 < componentsInChildren.Length)
		{
			Collider collider = componentsInChildren[num2];
			Rigidbody component = collider.gameObject.GetComponent<Rigidbody>();
			if ((component && component.isKinematic) || (collider is MeshCollider && !((MeshCollider)collider).convex))
			{
				collider.enabled = false;
			}
			else
			{
				collider.gameObject.layer = 13;
				collider.enabled = true;
				flag = false;
				collider.gameObject.AddMissingComponent<RootTransformRefEntity>();
			}
			num2++;
		}
		base.transform.GetComponent<Collider>().enabled = flag;
		this.meshGameObject = this.itemTransform.gameObject;
		this.meshGameObject.SetActive(true);
		if (this.itemWorldData != null)
		{
			this.itemClass.OnMeshCreated(this.itemWorldData);
		}
		this.bMeshCreated = true;
		this.meshRenderers = this.itemTransform.GetComponentsInChildren<Renderer>(true);
		this.VisiblityCheck(0f, false);
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x000EA08C File Offset: 0x000E828C
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkGravitySetting(bool hasGravity)
	{
		bool flag = hasGravity && !this.stickT;
		if (this.useGravity != flag)
		{
			this.useGravity = flag;
			Rigidbody rigidbody = this.itemRB;
			if (flag)
			{
				rigidbody.useGravity = true;
				rigidbody.isKinematic = false;
				rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				return;
			}
			rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x000EA0F4 File Offset: 0x000E82F4
	public override void AddVelocity(Vector3 _vel)
	{
		this.bWasThrown = true;
		base.SetAirBorne(true);
		if (!this.bMeshCreated)
		{
			this.createMesh();
		}
		this.checkGravitySetting(this.isPhysicsMaster);
		if (this.isPhysicsMaster)
		{
			this.itemRB.angularVelocity = this.rand.RandomOnUnitSphere * (1f + _vel.magnitude * this.rand.RandomFloat * 8f);
			this.itemRB.AddForce(_vel * 6f, ForceMode.Impulse);
		}
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x000EA184 File Offset: 0x000E8384
	public override void VisiblityCheck(float _distanceSqr, bool _masterIsZooming)
	{
		if (this.itemTransform && this.meshRenderers != null)
		{
			bool flag = _distanceSqr < (float)(_masterIsZooming ? 8100 : 3600);
			if (this.ticksExisted < 3 && _distanceSqr < 0.64000005f)
			{
				flag = false;
			}
			for (int i = 0; i < this.meshRenderers.Length; i++)
			{
				this.meshRenderers[i].enabled = flag;
			}
		}
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool CanCollideWith(Entity _other)
	{
		return true;
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x000EA1EE File Offset: 0x000E83EE
	public virtual bool CanCollect()
	{
		return this.itemClass != null && this.itemClass.CanCollect(this.itemStack.itemValue);
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x000EA210 File Offset: 0x000E8410
	public override void OnCollectServer(int _playerId)
	{
		this.world.RemoveEntity(this.entityId, EnumRemoveEntityReason.Killed);
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000EA228 File Offset: 0x000E8428
	public override void OnCollectLocal(int _playerId)
	{
		EntityPlayerLocal entityPlayerLocal = this.world.GetEntity(_playerId) as EntityPlayerLocal;
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(entityPlayerLocal);
		bool flag = entityPlayerLocal.inventory.IsHoldingItemActionRunning();
		int num = (flag ? uiforPlayer.xui.PlayerInventory.CountAvailableSpaceForItem(this.itemStack.itemValue, false) : (-1));
		if ((!flag || num - this.itemStack.itemValue.ItemClass.Stacknumber.Value > this.itemStack.count) && uiforPlayer.xui.PlayerInventory.AddItem(this.itemStack))
		{
			return;
		}
		GameManager.Instance.ItemDropServer(this.itemStack, base.GetPosition(), Vector3.zero, _playerId, 60f, false);
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000EA2E0 File Offset: 0x000E84E0
	public override void OnLoadedFromEntityCache(EntityCreationData _ecd)
	{
		this.markedForUnload = false;
		base.transform.name = "Item_" + _ecd.id.ToString();
		this.SetItemStack(_ecd.itemStack);
		this.bMeshCreated = false;
		if (this.meshRenderers != null)
		{
			for (int i = 0; i < this.meshRenderers.Length; i++)
			{
				this.meshRenderers[i].enabled = false;
			}
		}
		UpdateLightOnChunkMesh component;
		if (this.meshGameObject != null && (component = this.meshGameObject.GetComponent<UpdateLightOnChunkMesh>()) != null)
		{
			component.Reset();
		}
		this.itemWorldData = null;
		this.bDead = false;
		this.motion = Vector3.zero;
		this.addedToChunk = false;
		this.fallDistance = 0f;
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000EA3A6 File Offset: 0x000E85A6
	public override Transform GetModelTransform()
	{
		return this.itemTransform;
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x000EA3AE File Offset: 0x000E85AE
	public override void PhysicsMasterBecome()
	{
		this.checkGravitySetting(true);
		base.PhysicsMasterBecome();
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x000EA3C0 File Offset: 0x000E85C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTransform()
	{
		if (!this.isPhysicsMaster)
		{
			float deltaTime = Time.deltaTime;
			base.transform.position = Vector3.Lerp(base.transform.position, this.position - Origin.position, deltaTime * 7f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.qrotation, deltaTime * 3f);
			return;
		}
		Vector3 position = base.transform.position;
		if (!float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsNaN(position.z) && !float.IsInfinity(position.x) && !float.IsInfinity(position.y) && !float.IsInfinity(position.z))
		{
			this.SetPosition(position + Origin.position, true);
		}
		Quaternion rotation = base.transform.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		if (!float.IsNaN(eulerAngles.x) && !float.IsNaN(eulerAngles.y) && !float.IsNaN(eulerAngles.z) && !float.IsInfinity(eulerAngles.x) && !float.IsInfinity(eulerAngles.y) && !float.IsInfinity(eulerAngles.z))
		{
			this.SetRotation(eulerAngles);
			this.qrotation = rotation;
		}
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x000EA514 File Offset: 0x000E8714
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.stickT)
		{
			Vector3 vector = this.stickT.TransformPoint(this.stickRelativePos);
			base.transform.position = vector;
			base.transform.rotation = this.stickT.rotation * this.stickRot;
		}
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000EA570 File Offset: 0x000E8770
	[PublicizedFrom(EAccessModifier.Private)]
	public void tickDistraction()
	{
		if (this.itemClass == null || this.distractionLifetime <= 0)
		{
			return;
		}
		if ((this.isCollided || !this.itemClass.IsRequireContactDistraction) && this.distractionRadiusSq > 0f)
		{
			int num = this.nextDistractionTick + 1;
			this.nextDistractionTick = num;
			if (num > 20)
			{
				this.nextDistractionTick = 0;
				Vector3 position = this.position;
				Bounds bounds = new Bounds(position, new Vector3(this.distractionRadiusSq, this.distractionRadiusSq, this.distractionRadiusSq));
				this.world.GetEntitiesInBounds(typeof(EntityAlive), bounds, EntityItem.distractionTargets);
				for (int i = 0; i < EntityItem.distractionTargets.Count; i++)
				{
					EntityAlive entityAlive = (EntityAlive)EntityItem.distractionTargets[i];
					if (!entityAlive.IsSleeping && entityAlive.distraction == null)
					{
						EntityClass entityClass = EntityClass.list[entityAlive.entityClass];
						if (this.itemClass.DistractionTags.IsEmpty || this.itemClass.DistractionTags.Test_AnySet(entityClass.Tags))
						{
							float distanceSq = base.GetDistanceSq(entityAlive);
							if (distanceSq <= this.distractionRadiusSq && (entityAlive.pendingDistraction == null || distanceSq < entityAlive.pendingDistractionDistanceSq))
							{
								float num2 = entityAlive.distractionResistance - this.distractionStrength;
								if (num2 <= 0f || num2 < this.rand.RandomFloat * 100f)
								{
									entityAlive.pendingDistraction = this;
									entityAlive.pendingDistractionDistanceSq = distanceSq;
								}
							}
						}
					}
				}
				EntityItem.distractionTargets.Clear();
				if (this.distractionLifetime > 0)
				{
					this.distractionLifetime--;
				}
			}
		}
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x000EA730 File Offset: 0x000E8930
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCollisionEnter(Collision collision)
	{
		if (!this.CanCollide(collision))
		{
			return;
		}
		this.CheckStick(collision);
		if (!this.isCollided)
		{
			this.isCollided = true;
			if (this.isPhysicsMaster && !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				base.PhysicsMasterSendToServer(base.transform);
			}
		}
		if (this.contactPoints == null)
		{
			this.contactPoints = new List<ContactPoint>(2);
		}
		for (int i = collision.GetContacts(this.contactPoints) - 1; i >= 0; i--)
		{
			ContactPoint contactPoint = this.contactPoints[i];
			if (Utils.FastAbs(Vector3.Dot(collision.relativeVelocity, contactPoint.normal)) >= 1f)
			{
				Entity hitRootEntity = GameUtils.GetHitRootEntity(contactPoint.otherCollider.transform.tag, contactPoint.otherCollider.transform);
				if (hitRootEntity != null)
				{
					string value = EntityClass.list[hitRootEntity.entityClass].Properties.GetValue("SurfaceCategory");
					if (!string.IsNullOrEmpty(value) && this.itemClass != null && this.itemClass.MadeOfMaterial != null)
					{
						this.playThrowSound(this.itemClass.MadeOfMaterial.id + "hit" + value);
						return;
					}
					break;
				}
				else
				{
					Vector3i vector3i = World.worldToBlockPos(contactPoint.point - 0.25f * contactPoint.normal + Origin.position);
					BlockValue blockValue = this.world.GetBlock(vector3i);
					if (blockValue.isair)
					{
						WorldRayHitInfo worldRayHitInfo = new WorldRayHitInfo();
						GameUtils.FindMasterBlockForEntityModelBlock(this.world, -contactPoint.normal, contactPoint.otherCollider.transform.tag, contactPoint.point + Origin.position, contactPoint.otherCollider.transform, worldRayHitInfo);
						vector3i = worldRayHitInfo.hit.blockPos;
						blockValue = this.world.GetBlock(vector3i);
						if (blockValue.isair)
						{
							goto IL_02FB;
						}
					}
					float num = Utils.FastAbs(contactPoint.normal.x);
					float num2 = Utils.FastAbs(contactPoint.normal.y);
					float num3 = Utils.FastAbs(contactPoint.normal.z);
					BlockFace blockFace = BlockFace.Top;
					if (num >= num2 && num >= num3)
					{
						if (contactPoint.normal.x < 0f)
						{
							blockFace = BlockFace.East;
						}
						else if (contactPoint.normal.x > 0f)
						{
							blockFace = BlockFace.West;
						}
					}
					else if (num3 >= num && num3 >= num2)
					{
						if (contactPoint.normal.z < 0f)
						{
							blockFace = BlockFace.North;
						}
						else if (contactPoint.normal.z > 0f)
						{
							blockFace = BlockFace.South;
						}
					}
					else if (contactPoint.normal.y < 0f)
					{
						blockFace = BlockFace.Bottom;
					}
					string surfaceCategory = blockValue.Block.GetMaterialForSide(blockValue, blockFace).SurfaceCategory;
					if (this.itemClass != null && this.itemClass.MadeOfMaterial != null)
					{
						this.playThrowSound(this.itemClass.MadeOfMaterial.id + "hit" + surfaceCategory);
						return;
					}
					break;
				}
			}
			IL_02FB:;
		}
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x000EAA44 File Offset: 0x000E8C44
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CanCollide(Collision collision)
	{
		if (GameManager.Instance.World == null)
		{
			return false;
		}
		if (!this.bWasThrown && this.itemClass is ItemClassTimeBomb)
		{
			return false;
		}
		Transform transform = collision.transform;
		if (!transform)
		{
			return false;
		}
		string tag = transform.tag;
		if (tag != null && tag.StartsWith("E_"))
		{
			Transform hitRootTransform = GameUtils.GetHitRootTransform(tag, transform);
			if (hitRootTransform != null)
			{
				Entity component = hitRootTransform.GetComponent<Entity>();
				if (component != null && component.entityId == this.belongsPlayerId)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000EAAD0 File Offset: 0x000E8CD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckStick(Collision collision)
	{
		if (this.stickPercent <= 0f)
		{
			return;
		}
		float num = 1f - this.stickPercent;
		this.itemRB.velocity *= num;
		this.itemRB.angularVelocity *= num;
		if (this.stickPercent >= 1f && !this.stickT)
		{
			this.stickT = collision.transform;
			this.stickRelativePos = this.stickT.InverseTransformPoint(base.transform.position);
			this.stickRot = Quaternion.Inverse(this.stickT.rotation) * base.transform.rotation;
			Collider[] componentsInChildren = this.itemRB.GetComponentsInChildren<Collider>();
			for (int i = componentsInChildren.Length - 1; i >= 0; i--)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			this.checkGravitySetting(this.isPhysicsMaster);
			this.PlayOneShot(this.itemClass.SoundStick, false, false, false, null, 1f);
		}
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x000EABE4 File Offset: 0x000E8DE4
	public override string ToString()
	{
		if (this.itemStack.itemValue.HasQuality)
		{
			return string.Format("[type={0}, name={1}, cnt={2}, quality={3}]", new object[]
			{
				base.GetType().Name,
				this.itemClass.Name,
				this.itemStack.count,
				this.itemStack.itemValue.Quality
			});
		}
		return string.Format("[type={0}, name={1}, cnt={2}]", base.GetType().Name, (this.itemClass != null) ? this.itemClass.Name : string.Empty, this.itemStack.count);
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsQRotationUsed()
	{
		return true;
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x0600264D RID: 9805 RVA: 0x000EAC9A File Offset: 0x000E8E9A
	public bool IsDistractionActive
	{
		get
		{
			return this.distractionLifetime > 0;
		}
	}

	// Token: 0x04001C56 RID: 7254
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody itemRB;

	// Token: 0x04001C57 RID: 7255
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool useGravity;

	// Token: 0x04001C58 RID: 7256
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer[] meshRenderers;

	// Token: 0x04001C59 RID: 7257
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject meshGameObject;

	// Token: 0x04001C5A RID: 7258
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform itemTransform;

	// Token: 0x04001C5B RID: 7259
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04001C5C RID: 7260
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ItemStack lastCachedItemStack = ItemStack.Empty;

	// Token: 0x04001C5D RID: 7261
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMeshCreated;

	// Token: 0x04001C5E RID: 7262
	public ItemClass itemClass;

	// Token: 0x04001C5F RID: 7263
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float stickPercent;

	// Token: 0x04001C60 RID: 7264
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform stickT;

	// Token: 0x04001C61 RID: 7265
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 stickRelativePos;

	// Token: 0x04001C62 RID: 7266
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion stickRot;

	// Token: 0x04001C63 RID: 7267
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ItemWorldData itemWorldData;

	// Token: 0x04001C64 RID: 7268
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bWasThrown;

	// Token: 0x04001C65 RID: 7269
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int onGroundCounter;

	// Token: 0x04001C66 RID: 7270
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int distractionLifetime;

	// Token: 0x04001C67 RID: 7271
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float distractionStrength;

	// Token: 0x04001C68 RID: 7272
	public int distractionEatTicks;

	// Token: 0x04001C69 RID: 7273
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int nextDistractionTick;

	// Token: 0x04001C6A RID: 7274
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float distractionRadiusSq;

	// Token: 0x04001C6B RID: 7275
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<Entity> distractionTargets = new List<Entity>();

	// Token: 0x04001C6C RID: 7276
	public int OwnerId = -1;

	// Token: 0x04001C6D RID: 7277
	public static int ItemInstanceCount;

	// Token: 0x04001C6E RID: 7278
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<ContactPoint> contactPoints;
}
