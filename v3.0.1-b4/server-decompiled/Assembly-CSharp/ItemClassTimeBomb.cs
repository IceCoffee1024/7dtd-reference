using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005F8 RID: 1528
[Preserve]
public class ItemClassTimeBomb : ItemClass
{
	// Token: 0x060031C5 RID: 12741 RVA: 0x0014353C File Offset: 0x0014173C
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

	// Token: 0x060031C6 RID: 12742 RVA: 0x0014360D File Offset: 0x0014180D
	public override void StartHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		base.StartHolding(_data, _modelTransform);
		this.OnHoldingReset(_data);
		this.activateHolding(_data.itemValue, 0, _modelTransform);
		_data.Changed();
	}

	// Token: 0x060031C7 RID: 12743 RVA: 0x00143634 File Offset: 0x00141834
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

	// Token: 0x060031C8 RID: 12744 RVA: 0x00143730 File Offset: 0x00141930
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

	// Token: 0x060031C9 RID: 12745 RVA: 0x00143840 File Offset: 0x00141A40
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

	// Token: 0x060031CA RID: 12746 RVA: 0x001438DC File Offset: 0x00141ADC
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

	// Token: 0x060031CB RID: 12747 RVA: 0x00143920 File Offset: 0x00141B20
	public override void OnMeshCreated(ItemWorldData _data)
	{
		EntityItem entityItem = _data.entityItem;
		ItemValue itemValue = entityItem.itemStack.itemValue;
		this.setActivationTransformsActive(entityItem.GetModelTransform(), itemValue.Meta != 0);
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x00143955 File Offset: 0x00141B55
	public override bool CanDrop(ItemValue _iv = null)
	{
		return this.bCanDrop && (_iv == null || _iv.Meta == 0);
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x0014396F File Offset: 0x00141B6F
	public override void Deactivate(ItemValue _iv)
	{
		_iv.Meta = 0;
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x00143978 File Offset: 0x00141B78
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

	// Token: 0x060031CF RID: 12751 RVA: 0x00143B8C File Offset: 0x00141D8C
	public override void OnDamagedByExplosion(ItemWorldData _data)
	{
		_data.gameManager.ExplosionServer(_data.entityItem.GetPosition(), World.worldToBlockPos(_data.entityItem.GetPosition()), Quaternion.identity, this.explosion, _data.belongsEntityId, _data.entityItem.rand.RandomRange(0.1f, 0.3f), false, _data.entityItem.itemStack.itemValue.Clone());
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x00143C00 File Offset: 0x00141E00
	public override bool CanCollect(ItemValue _iv)
	{
		return _iv.Meta == 0;
	}

	// Token: 0x060031D1 RID: 12753 RVA: 0x00143C0C File Offset: 0x00141E0C
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

	// Token: 0x0400273A RID: 10042
	public bool bExplodeOnHitGround;

	// Token: 0x0400273B RID: 10043
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPrimed = -1;

	// Token: 0x0400273C RID: 10044
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosion;

	// Token: 0x0400273D RID: 10045
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dropStarts;

	// Token: 0x0400273E RID: 10046
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mustPrime;

	// Token: 0x0400273F RID: 10047
	[PublicizedFrom(EAccessModifier.Private)]
	public int explodeAfterTicks;

	// Token: 0x04002740 RID: 10048
	[PublicizedFrom(EAccessModifier.Private)]
	public float tickSoundDelay;

	// Token: 0x04002741 RID: 10049
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] activationTransformToHide;

	// Token: 0x04002742 RID: 10050
	[PublicizedFrom(EAccessModifier.Private)]
	public string activationEmissive;
}
