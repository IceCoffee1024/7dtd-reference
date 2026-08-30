using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E2 RID: 1506
[Preserve]
public class ItemActionThrownWeapon : ItemActionThrowAway
{
	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x060030E9 RID: 12521 RVA: 0x0013D7CF File Offset: 0x0013B9CF
	// (set) Token: 0x060030EA RID: 12522 RVA: 0x0013D7D7 File Offset: 0x0013B9D7
	public new ExplosionData Explosion { get; set; }

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x060030EB RID: 12523 RVA: 0x0013D7E0 File Offset: 0x0013B9E0
	// (set) Token: 0x060030EC RID: 12524 RVA: 0x0013D7E8 File Offset: 0x0013B9E8
	public new int Velocity { get; set; }

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x060030ED RID: 12525 RVA: 0x0013D7F1 File Offset: 0x0013B9F1
	// (set) Token: 0x060030EE RID: 12526 RVA: 0x0013D7F9 File Offset: 0x0013B9F9
	public new float FlyTime { get; set; }

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x060030EF RID: 12527 RVA: 0x0013D802 File Offset: 0x0013BA02
	// (set) Token: 0x060030F0 RID: 12528 RVA: 0x0013D80A File Offset: 0x0013BA0A
	public new float LifeTime { get; set; }

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x060030F1 RID: 12529 RVA: 0x0013D813 File Offset: 0x0013BA13
	// (set) Token: 0x060030F2 RID: 12530 RVA: 0x0013D81B File Offset: 0x0013BA1B
	public new float CollisionRadius { get; set; }

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x060030F3 RID: 12531 RVA: 0x0013D824 File Offset: 0x0013BA24
	// (set) Token: 0x060030F4 RID: 12532 RVA: 0x0013D82C File Offset: 0x0013BA2C
	public float Gravity { get; set; }

	// Token: 0x060030F5 RID: 12533 RVA: 0x0013D838 File Offset: 0x0013BA38
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

	// Token: 0x060030F6 RID: 12534 RVA: 0x0013DA74 File Offset: 0x0013BC74
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionThrowAway.MyInventoryData)_actionData).m_bActivated;
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x0013DA81 File Offset: 0x0013BC81
	public override void StartHolding(ItemActionData _actionData)
	{
		base.StartHolding(_actionData);
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		myInventoryData.m_bActivated = false;
		myInventoryData.m_ActivateTime = 0f;
		myInventoryData.m_LastThrowTime = 0f;
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x0013DAAC File Offset: 0x0013BCAC
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

	// Token: 0x060030F9 RID: 12537 RVA: 0x0013DB4C File Offset: 0x0013BD4C
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		_actionData.invData.holdingEntity.emodel.avatarController.TriggerEvent("WeaponFire");
		(_actionData as ItemActionThrowAway.MyInventoryData).m_LastThrowTime = 0f;
		if (!_actionData.invData.holdingEntity.isEntityRemote)
		{
			this.throwAway(_actionData as ItemActionThrowAway.MyInventoryData);
		}
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x0013DBA8 File Offset: 0x0013BDA8
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

	// Token: 0x060030FB RID: 12539 RVA: 0x0013DD10 File Offset: 0x0013BF10
	public override void CancelAction(ItemActionData _actionData)
	{
		ItemActionThrowAway.MyInventoryData myInventoryData = (ItemActionThrowAway.MyInventoryData)_actionData;
		myInventoryData.invData.holdingEntity.emodel.avatarController.TriggerEvent("WeaponPreFireCancel");
		myInventoryData.m_bActivated = false;
		myInventoryData.m_bCanceled = true;
		myInventoryData.m_ActivateTime = 0f;
		myInventoryData.m_LastThrowTime = 0f;
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x0013DD68 File Offset: 0x0013BF68
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

	// Token: 0x060030FD RID: 12541 RVA: 0x0013DEA4 File Offset: 0x0013C0A4
	public ThrownWeaponMoveScript instantiateProjectile(ItemActionData _actionData)
	{
		ItemValue holdingItemItemValue = _actionData.invData.holdingEntity.inventory.holdingItemItemValue;
		int entityId = _actionData.invData.holdingEntity.entityId;
		new ItemValue(_actionData.invData.item.Id, false);
		Transform transform = global::UnityEngine.Object.Instantiate<GameObject>(_actionData.invData.holdingEntity.inventory.models[_actionData.invData.holdingEntity.inventory.holdingItemIdx].gameObject).transform;
		Utils.ForceMaterialsInstance(transform.gameObject);
		transform.parent = null;
		transform.position = _actionData.invData.model.transform.position;
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		Utils.SetLayerRecursively(transform.gameObject, 0);
		ThrownWeaponMoveScript thrownWeaponMoveScript = transform.gameObject.AddMissingComponent<ThrownWeaponMoveScript>();
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

	// Token: 0x060030FE RID: 12542 RVA: 0x0013E064 File Offset: 0x0013C264
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

	// Token: 0x060030FF RID: 12543 RVA: 0x0013E0FC File Offset: 0x0013C2FC
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

	// Token: 0x04002608 RID: 9736
	public float damageEntity;

	// Token: 0x04002609 RID: 9737
	public float damageBlock;

	// Token: 0x0400260A RID: 9738
	[PublicizedFrom(EAccessModifier.Protected)]
	public int hitmaskOverride;

	// Token: 0x0400260B RID: 9739
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> usePassedInTransformTag = FastTags<TagGroup.Global>.Parse("usePassedInTransform");

	// Token: 0x0400260C RID: 9740
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> tmpTag;
}
