using System;
using System.Globalization;
using AssetManagement;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E3 RID: 1507
[Preserve]
public class ItemActionThrownWeapon : ItemActionThrowAway
{
	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x06003130 RID: 12592 RVA: 0x0013E295 File Offset: 0x0013C495
	// (set) Token: 0x06003131 RID: 12593 RVA: 0x0013E29D File Offset: 0x0013C49D
	public new ExplosionData Explosion { get; set; }

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x06003132 RID: 12594 RVA: 0x0013E2A6 File Offset: 0x0013C4A6
	// (set) Token: 0x06003133 RID: 12595 RVA: 0x0013E2AE File Offset: 0x0013C4AE
	public new int Velocity { get; set; }

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06003134 RID: 12596 RVA: 0x0013E2B7 File Offset: 0x0013C4B7
	// (set) Token: 0x06003135 RID: 12597 RVA: 0x0013E2BF File Offset: 0x0013C4BF
	public new float FlyTime { get; set; }

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06003136 RID: 12598 RVA: 0x0013E2C8 File Offset: 0x0013C4C8
	// (set) Token: 0x06003137 RID: 12599 RVA: 0x0013E2D0 File Offset: 0x0013C4D0
	public new float LifeTime { get; set; }

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06003138 RID: 12600 RVA: 0x0013E2D9 File Offset: 0x0013C4D9
	// (set) Token: 0x06003139 RID: 12601 RVA: 0x0013E2E1 File Offset: 0x0013C4E1
	public new float CollisionRadius { get; set; }

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x0600313A RID: 12602 RVA: 0x0013E2EA File Offset: 0x0013C4EA
	// (set) Token: 0x0600313B RID: 12603 RVA: 0x0013E2F2 File Offset: 0x0013C4F2
	public float Gravity { get; set; }

	// Token: 0x0600313C RID: 12604 RVA: 0x0013E2FC File Offset: 0x0013C4FC
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.Explosion = new ExplosionData(this.Properties, this.item.Effects);
		if (this.Properties.Values.ContainsKey("Velocity"))
		{
			this.Velocity = (int)StringParsers.ParseFloat(this.Properties.Values["Velocity"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.Velocity = 1;
		}
		if (this.Properties.Values.ContainsKey("FlyTime"))
		{
			this.FlyTime = StringParsers.ParseFloat(this.Properties.Values["FlyTime"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.FlyTime = 20f;
		}
		if (this.Properties.Values.ContainsKey("LifeTime"))
		{
			this.LifeTime = StringParsers.ParseFloat(this.Properties.Values["LifeTime"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.LifeTime = 100f;
		}
		if (this.Properties.Values.ContainsKey("CollisionRadius"))
		{
			this.CollisionRadius = StringParsers.ParseFloat(this.Properties.Values["CollisionRadius"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.CollisionRadius = 0.05f;
		}
		if (this.Properties.Values.ContainsKey("Gravity"))
		{
			this.Gravity = StringParsers.ParseFloat(this.Properties.Values["Gravity"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.Gravity = -9.81f;
		}
		if (_props.Values.ContainsKey("DamageEntity"))
		{
			this.damageEntity = StringParsers.ParseFloat(_props.Values["DamageEntity"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.damageEntity = 0f;
		}
		if (_props.Values.ContainsKey("DamageBlock"))
		{
			this.damageBlock = StringParsers.ParseFloat(_props.Values["DamageBlock"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.damageBlock = 0f;
		}
		this.hitmaskOverride = Voxel.ToHitMask(_props.GetString("Hitmask_override"));
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x0013E538 File Offset: 0x0013C738
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionThrowAway.MyInventoryData)_actionData).m_bActivated;
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x0013E545 File Offset: 0x0013C745
	public override void StartHolding(ItemActionData _actionData)
	{
		base.StartHolding(_actionData);
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		myInventoryData.m_bActivated = false;
		myInventoryData.m_ActivateTime = 0f;
		myInventoryData.m_LastThrowTime = 0f;
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x0013E570 File Offset: 0x0013C770
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		float num = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient ? 0.1f : AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast);
		if (myInventoryData.m_LastThrowTime <= 0f || Time.time - myInventoryData.m_LastThrowTime < num)
		{
			return;
		}
		GameManager.Instance.ItemActionEffectsServer(myInventoryData.invData.holdingEntity.entityId, myInventoryData.invData.slotIdx, myInventoryData.indexInEntityOfAction, 0, Vector3.zero, Vector3.zero, 0);
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x0013E610 File Offset: 0x0013C810
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		_actionData.invData.holdingEntity.emodel.avatarController.TriggerEvent("WeaponFire");
		(_actionData as ItemActionThrowAway.MyInventoryData).m_LastThrowTime = 0f;
		if (!_actionData.invData.holdingEntity.isEntityRemote)
		{
			this.throwAway(_actionData as ItemActionThrowAway.MyInventoryData);
		}
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x0013E66C File Offset: 0x0013C86C
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		if (!myInventoryData.m_bActivated && Time.time - myInventoryData.m_LastThrowTime < this.Delay)
		{
			return;
		}
		if (_actionData.invData.itemValue.PercentUsesLeft == 0f)
		{
			if (_bReleased)
			{
				EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
				base.HandleJamSound(_actionData.invData.itemValue, this.item, entityPlayerLocal);
			}
			return;
		}
		if (!_bReleased)
		{
			if (!myInventoryData.m_bActivated && !myInventoryData.m_bCanceled)
			{
				myInventoryData.m_bActivated = true;
				myInventoryData.m_ActivateTime = Time.time;
				myInventoryData.invData.holdingEntity.emodel.avatarController.TriggerEvent("WeaponPreFire");
			}
			return;
		}
		if (myInventoryData.m_bCanceled)
		{
			myInventoryData.m_bCanceled = false;
			return;
		}
		if (!myInventoryData.m_bActivated)
		{
			return;
		}
		myInventoryData.m_ThrowStrength = Mathf.Min(this.maxStrainTime, Time.time - myInventoryData.m_ActivateTime) / this.maxStrainTime * this.maxThrowStrength;
		myInventoryData.m_LastThrowTime = Time.time;
		myInventoryData.m_bActivated = false;
		if (!myInventoryData.invData.holdingEntity.isEntityRemote)
		{
			myInventoryData.invData.holdingEntity.emodel.avatarController.TriggerEvent("WeaponFire");
		}
		if (this.soundStart != null)
		{
			myInventoryData.invData.holdingEntity.PlayOneShot(this.soundStart, false, false, false, null, 1f);
		}
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0013E7D4 File Offset: 0x0013C9D4
	public override void CancelAction(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		myInventoryData.invData.holdingEntity.emodel.avatarController.TriggerEvent("WeaponPreFireCancel");
		myInventoryData.m_bActivated = false;
		myInventoryData.m_bCanceled = true;
		myInventoryData.m_ActivateTime = 0f;
		myInventoryData.m_LastThrowTime = 0f;
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x0013E82C File Offset: 0x0013CA2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void throwAway(ItemActionThrowAway.MyInventoryData _actionData)
	{
		ThrownWeaponMoveScript thrownWeaponMoveScript = this.instantiateProjectile(_actionData);
		ItemInventoryData invData = _actionData.invData;
		Vector3 lookVector = _actionData.invData.holdingEntity.GetLookVector();
		_actionData.invData.holdingEntity.getHeadPosition();
		Vector3 origin = _actionData.invData.holdingEntity.GetLookRay().origin;
		if (_actionData.invData.holdingEntity as EntityPlayerLocal != null)
		{
			float num = EffectManager.GetValue(PassiveEffects.StaminaLoss, _actionData.invData.holdingEntity.inventory.holdingItemItemValue, 2f, _actionData.invData.holdingEntity, null, _actionData.invData.holdingEntity.inventory.holdingItem.ItemTags | FastTags<TagGroup.Global>.Parse((_actionData.indexInEntityOfAction == 0) ? "primary" : "secondary"), true, true, true, true, true, 1, true, false) * ItemActionAttack.StaminaUsageMultiplier;
			_actionData.invData.holdingEntity.Stats.Stamina.Value -= num;
		}
		thrownWeaponMoveScript.Fire(origin, lookVector, _actionData.invData.holdingEntity, this.hitmaskOverride, _actionData.m_ThrowStrength);
		_actionData.invData.holdingEntity.inventory.DecHoldingItem(1);
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x0013E968 File Offset: 0x0013CB68
	public ThrownWeaponMoveScript instantiateProjectile(ItemActionData _actionData)
	{
		ItemValue holdingItemItemValue = _actionData.invData.holdingEntity.inventory.holdingItemItemValue;
		int entityId = _actionData.invData.holdingEntity.entityId;
		new ItemValue(_actionData.invData.item.Id, false);
		InstanceHandle<GameObject> instanceHandle = _actionData.invData.CloneModel();
		Transform transform = instanceHandle.Instance.transform;
		Utils.ForceMaterialsInstance(transform.gameObject);
		transform.parent = null;
		transform.position = _actionData.invData.model.transform.position;
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		Utils.SetLayerRecursively(transform.gameObject, 0);
		ThrownWeaponMoveScript thrownWeaponMoveScript = transform.gameObject.AddMissingComponent<ThrownWeaponMoveScript>();
		thrownWeaponMoveScript.assetHandle = instanceHandle;
		thrownWeaponMoveScript.itemActionThrownWeapon = this;
		thrownWeaponMoveScript.itemWeapon = _actionData.invData.item;
		thrownWeaponMoveScript.itemValueWeapon = _actionData.invData.itemValue;
		thrownWeaponMoveScript.actionData = _actionData as ItemActionThrowAway.MyInventoryData;
		thrownWeaponMoveScript.ProjectileOwnerID = _actionData.invData.holdingEntity.entityId;
		transform.gameObject.SetActive(true);
		_actionData.invData.model.gameObject.SetActive(false);
		_actionData.invData.holdingEntity.MinEventContext.Self = _actionData.invData.holdingEntity;
		_actionData.invData.holdingEntity.MinEventContext.Transform = transform;
		_actionData.invData.holdingEntity.MinEventContext.Tags = this.usePassedInTransformTag;
		thrownWeaponMoveScript.itemValueWeapon.FireEvent(MinEventTypes.onSelfHoldingItemThrown, _actionData.invData.holdingEntity.MinEventContext);
		return thrownWeaponMoveScript;
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x0013EB0C File Offset: 0x0013CD0C
	public float GetDamageEntity(ItemValue _itemValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
	{
		this.tmpTag = ((actionIndex == 0) ? ItemActionAttack.PrimaryTag : ItemActionAttack.SecondaryTag);
		this.tmpTag |= ((_itemValue.ItemClass == null) ? ItemActionAttack.MeleeTag : _itemValue.ItemClass.ItemTags);
		if (_holdingEntity != null)
		{
			this.tmpTag |= _holdingEntity.CurrentStanceTag | _holdingEntity.CurrentMovementTag;
		}
		return EffectManager.GetValue(PassiveEffects.EntityDamage, _itemValue, this.damageEntity, _holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x0013EBA4 File Offset: 0x0013CDA4
	public float GetDamageBlock(ItemValue _itemValue, BlockValue _blockValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
	{
		this.tmpTag = ((actionIndex == 0) ? ItemActionAttack.PrimaryTag : ItemActionAttack.SecondaryTag);
		this.tmpTag |= ((_itemValue.ItemClass == null) ? ItemActionAttack.MeleeTag : _itemValue.ItemClass.ItemTags);
		if (_holdingEntity != null)
		{
			this.tmpTag |= _holdingEntity.CurrentStanceTag | _holdingEntity.CurrentMovementTag;
		}
		this.tmpTag |= _blockValue.Block.Tags;
		float value = EffectManager.GetValue(PassiveEffects.BlockDamage, _itemValue, this.damageBlock, _holdingEntity, null, this.tmpTag, true, true, true, true, true, 1, true, false);
		return Utils.FastMin((float)_blockValue.Block.blockMaterial.MaxIncomingDamage, value);
	}

	// Token: 0x04002602 RID: 9730
	public float damageEntity;

	// Token: 0x04002603 RID: 9731
	public float damageBlock;

	// Token: 0x04002604 RID: 9732
	[PublicizedFrom(EAccessModifier.Protected)]
	public int hitmaskOverride;

	// Token: 0x04002605 RID: 9733
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> usePassedInTransformTag = FastTags<TagGroup.Global>.Parse("usePassedInTransform");

	// Token: 0x04002606 RID: 9734
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tmpTag;
}
