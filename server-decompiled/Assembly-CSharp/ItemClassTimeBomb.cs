using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005FF RID: 1535
[Preserve]
public class ItemClassTimeBomb : ItemClass
{
	// Token: 0x06003214 RID: 12820 RVA: 0x00145380 File Offset: 0x00143580
	public override void Init()
	{
		base.Init();
		this.explosion = new ExplosionData(this.Properties, this.Effects);
		this.Properties.ParseBool("ExplodeOnHit", ref this.bExplodeOnHitGround);
		this.Properties.ParseBool("FuseStartOnDrop", ref this.dropStarts);
		this.Properties.ParseBool("FusePrimeOnActivate", ref this.mustPrime);
		string @string = this.Properties.GetString("ActivationTransformToHide");
		if (@string.Length > 0)
		{
			this.activationTransformToHide = @string.Split(';', StringSplitOptions.None);
		}
		this.activationEmissive = this.Properties.GetString("ActivationEmissive");
		float num = 2f;
		this.Properties.ParseFloat("FuseTime", ref num);
		this.explodeAfterTicks = (int)(num * 20f);
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x00145451 File Offset: 0x00143651
	public override void StartHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		base.StartHolding(_data, _modelTransform);
		this.OnHoldingReset(_data);
		this.activateHolding(_data.itemValue, 0, _modelTransform);
		_data.Changed();
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x00145478 File Offset: 0x00143678
	public override void OnHoldingItemActivated(ItemInventoryData _data)
	{
		ItemValue itemValue = _data.itemValue;
		if (itemValue.Meta != 0)
		{
			return;
		}
		if (_data.holdingEntity.emodel.avatarController != null)
		{
			_data.holdingEntity.emodel.avatarController.CancelEvent("WeaponPreFireCancel");
		}
		this.setActivationTransformsActive(_data.holdingEntity.inventory.models[_data.holdingEntity.inventory.holdingItemIdx], true);
		_data.holdingEntity.RightArmAnimationUse = true;
		int num = this.explodeAfterTicks;
		if (this.mustPrime)
		{
			num = -1;
		}
		this.activateHolding(itemValue, num, _data.model);
		_data.Changed();
		AudioSource audioSource = ((_data.model != null) ? _data.model.GetComponentInChildren<AudioSource>() : null);
		if (audioSource)
		{
			audioSource.Play();
		}
		if (!_data.holdingEntity.isEntityRemote)
		{
			_data.gameManager.SimpleRPC(_data.holdingEntity.entityId, SimpleRPCType.OnActivateItem, false, false);
		}
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x00145574 File Offset: 0x00143774
	public override void OnHoldingUpdate(ItemInventoryData _data)
	{
		base.OnHoldingUpdate(_data);
		if (_data.holdingEntity.isEntityRemote)
		{
			return;
		}
		ItemValue itemValue = _data.itemValue;
		if (itemValue.Meta > 0)
		{
			itemValue.Meta--;
			if (itemValue.Meta == 0)
			{
				Vector3 vector = ((_data.model != null) ? (_data.model.position + Origin.position) : _data.holdingEntity.GetPosition());
				MinEventParams.CachedEventParam.Self = _data.holdingEntity;
				MinEventParams.CachedEventParam.Position = vector;
				MinEventParams.CachedEventParam.ItemValue = itemValue;
				itemValue.FireEvent(MinEventTypes.onProjectileImpact, MinEventParams.CachedEventParam);
				_data.gameManager.ExplosionServer(vector, World.worldToBlockPos(vector), Quaternion.identity, this.explosion, _data.holdingEntity.entityId, 0.1f, false, itemValue.Clone());
				this.activateHolding(itemValue, 0, _data.model);
				_data.holdingEntity.inventory.DecHoldingItem(1);
				return;
			}
			_data.Changed();
		}
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x00145684 File Offset: 0x00143884
	public override void OnHoldingReset(ItemInventoryData _data)
	{
		_data.itemValue.Meta = 0;
		this.setActivationTransformsActive(_data.holdingEntity.inventory.models[_data.holdingEntity.inventory.holdingItemIdx], false);
		if (!_data.holdingEntity.isEntityRemote)
		{
			_data.gameManager.SimpleRPC(_data.holdingEntity.entityId, SimpleRPCType.OnResetItem, false, false);
		}
		if (_data.holdingEntity.emodel.avatarController != null)
		{
			_data.holdingEntity.emodel.avatarController.TriggerEvent("WeaponPreFireCancel");
		}
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x00145720 File Offset: 0x00143920
	[PublicizedFrom(EAccessModifier.Private)]
	public void activateHolding(ItemValue iv, int ticks, Transform _t)
	{
		iv.Meta = ticks;
		if (_t != null)
		{
			OnActivateItemGameObjectReference component = _t.GetComponent<OnActivateItemGameObjectReference>();
			if (component != null)
			{
				bool flag = ticks != 0;
				if (component.IsActivated() != flag)
				{
					component.ActivateItem(flag);
				}
			}
		}
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x00145764 File Offset: 0x00143964
	public override void OnMeshCreated(ItemWorldData _data)
	{
		EntityItem entityItem = _data.entityItem;
		ItemValue itemValue = entityItem.itemStack.itemValue;
		this.setActivationTransformsActive(entityItem.GetModelTransform(), itemValue.Meta != 0);
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x00145799 File Offset: 0x00143999
	public override bool CanDrop(ItemValue _iv = null)
	{
		return this.bCanDrop && (_iv == null || _iv.Meta == 0);
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x001457B3 File Offset: 0x001439B3
	public override void Deactivate(ItemValue _iv)
	{
		_iv.Meta = 0;
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x001457BC File Offset: 0x001439BC
	public override void OnDroppedUpdate(ItemWorldData _data)
	{
		EntityItem entityItem = _data.entityItem;
		bool flag = _data.world.IsRemote();
		ItemValue itemValue = entityItem.itemStack.itemValue;
		if (itemValue.Meta == 65535)
		{
			itemValue.Meta = -1;
		}
		Vector3 vector = entityItem.PhysicsMasterGetFinalPosition();
		if (!flag && this.bExplodeOnHitGround && (!this.mustPrime || itemValue.Meta > 0) && (entityItem.isCollided || _data.world.IsWater(vector)))
		{
			itemValue.FireEvent(MinEventTypes.onProjectileImpact, new MinEventParams
			{
				Self = (_data.world.GetEntity(_data.belongsEntityId) as EntityAlive),
				IsLocal = true,
				Position = vector,
				ItemValue = itemValue
			});
			itemValue.Meta = 1;
		}
		if (!flag && ((this.dropStarts && itemValue.Meta == 0) || (this.mustPrime && itemValue.Meta <= -1)))
		{
			Animator componentInChildren = entityItem.gameObject.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				componentInChildren.SetBool("PinPulled", true);
			}
			itemValue.Meta = this.explodeAfterTicks;
		}
		if (itemValue.Meta > 0)
		{
			OnActivateItemGameObjectReference onActivateItemGameObjectReference = ((entityItem.GetModelTransform() != null) ? entityItem.GetModelTransform().GetComponent<OnActivateItemGameObjectReference>() : null);
			if (onActivateItemGameObjectReference != null && !onActivateItemGameObjectReference.IsActivated())
			{
				onActivateItemGameObjectReference.ActivateItem(true);
				this.setActivationTransformsActive(entityItem.GetModelTransform(), true);
			}
			if (flag)
			{
				return;
			}
			this.tickSoundDelay -= 0.05f;
			if (this.tickSoundDelay <= 0f)
			{
				this.tickSoundDelay = entityItem.itemClass.SoundTickDelay;
				entityItem.PlayOneShot(entityItem.itemClass.SoundTick, false, false, false, null, 1f);
			}
			itemValue.Meta--;
			if (itemValue.Meta == 0)
			{
				entityItem.SetDead();
				_data.gameManager.ExplosionServer(vector, World.worldToBlockPos(vector), Quaternion.identity, this.explosion, _data.belongsEntityId, 0f, false, itemValue.Clone());
				return;
			}
			entityItem.itemStack.itemValue = itemValue;
		}
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x001459D0 File Offset: 0x00143BD0
	public override void OnDamagedByExplosion(ItemWorldData _data)
	{
		_data.gameManager.ExplosionServer(_data.entityItem.GetPosition(), World.worldToBlockPos(_data.entityItem.GetPosition()), Quaternion.identity, this.explosion, _data.belongsEntityId, _data.entityItem.rand.RandomRange(0.1f, 0.3f), false, _data.entityItem.itemStack.itemValue.Clone());
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x00145A44 File Offset: 0x00143C44
	public override bool CanCollect(ItemValue _iv)
	{
		return _iv.Meta == 0;
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x00145A50 File Offset: 0x00143C50
	[PublicizedFrom(EAccessModifier.Private)]
	public void setActivationTransformsActive(Transform item, bool isActive)
	{
		if (this.activationTransformToHide != null)
		{
			foreach (string text in this.activationTransformToHide)
			{
				Transform transform = item.FindInChilds(text, false);
				if (transform)
				{
					transform.gameObject.SetActive(isActive);
				}
			}
		}
		if (!string.IsNullOrEmpty(this.activationEmissive))
		{
			float num = (float)(isActive ? 1 : 0);
			foreach (Renderer renderer in item.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer.CompareTag(this.activationEmissive))
				{
					renderer.material.SetFloat("_EmissionMultiply", num);
				}
			}
		}
	}

	// Token: 0x0400279D RID: 10141
	public bool bExplodeOnHitGround;

	// Token: 0x0400279E RID: 10142
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPrimed = -1;

	// Token: 0x0400279F RID: 10143
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosion;

	// Token: 0x040027A0 RID: 10144
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dropStarts;

	// Token: 0x040027A1 RID: 10145
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mustPrime;

	// Token: 0x040027A2 RID: 10146
	[PublicizedFrom(EAccessModifier.Private)]
	public int explodeAfterTicks;

	// Token: 0x040027A3 RID: 10147
	[PublicizedFrom(EAccessModifier.Private)]
	public float tickSoundDelay;

	// Token: 0x040027A4 RID: 10148
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] activationTransformToHide;

	// Token: 0x040027A5 RID: 10149
	[PublicizedFrom(EAccessModifier.Private)]
	public string activationEmissive;
}
