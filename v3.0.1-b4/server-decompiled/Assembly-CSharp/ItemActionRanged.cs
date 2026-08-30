using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005BF RID: 1471
[Preserve]
public class ItemActionRanged : ItemActionAttack
{
	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x00132A59 File Offset: 0x00130C59
	// (set) Token: 0x06002FC4 RID: 12228 RVA: 0x00132A61 File Offset: 0x00130C61
	public IntRange aiBurstShot
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x00132A6A File Offset: 0x00130C6A
	// (set) Token: 0x06002FC6 RID: 12230 RVA: 0x00132A72 File Offset: 0x00130C72
	public FloatRange aiBurstDelay
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x00132AA0 File Offset: 0x00130CA0
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionRanged.ItemActionDataRanged(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x00132AAC File Offset: 0x00130CAC
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.bulletMaterialName = "bullet";
		_props.ParseString("bullet_material", ref this.bulletMaterialName);
		_props.ParseBool("SupportHarvesting", ref this.bSupportHarvesting);
		_props.ParseBool("UseMeleeCrosshair", ref this.bUseMeleeCrosshair);
		this.EntityPenetrationCount = 0;
		_props.ParseInt("EntityPenetrationCount", ref this.EntityPenetrationCount);
		_props.ParseInt("BlockPenetrationFactor", ref this.BlockPenetrationFactor);
		_props.ParseBool("AutoReload", ref this.AutoReload);
		if (_props.Values.ContainsKey("triggerEffectShootDualsense"))
		{
			this.triggerEffectShootDualsense = _props.Values["triggerEffectShootDualsense"];
		}
		else
		{
			this.triggerEffectShootDualsense = string.Empty;
		}
		if (_props.Values.ContainsKey("triggerEffectTriggerPullDualsense"))
		{
			this.triggerEffectTriggerPullDualsense = _props.Values["triggerEffectTriggerPullDualsense"];
		}
		else
		{
			this.triggerEffectTriggerPullDualsense = string.Empty;
		}
		if (_props.Values.ContainsKey("triggerEffectShootXbox"))
		{
			this.triggerEffectShootXbox = _props.Values["triggerEffectShootXbox"];
		}
		else
		{
			this.triggerEffectShootXbox = string.Empty;
		}
		if (_props.Values.ContainsKey("triggerEffectTriggerPullXbox"))
		{
			this.triggerEffectTriggerPullXbox = _props.Values["triggerEffectTriggerPullXbox"];
		}
		else
		{
			this.triggerEffectTriggerPullXbox = string.Empty;
		}
		_props.ParseFloat("SpreadVerticalOffset", ref this.spreadVerticalOffset);
		_props.ParseBool("RapidTrigger", ref this.rapidTrigger);
		this.aiBurstShot = _props.TryParseRange("aiBurstShot", new IntRange(1, 1));
		this.aiBurstDelay = _props.TryParseRange("aiBurstDelay", new FloatRange(1f, 2f));
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x00132C66 File Offset: 0x00130E66
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canShowOverlay(ItemActionData actionData)
	{
		return this.bSupportHarvesting;
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x00132C6E File Offset: 0x00130E6E
	public bool IsSingleMagazineUsage()
	{
		return this.AmmoIsPerMagazine;
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x00132C78 File Offset: 0x00130E78
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal == null)
		{
			return ItemClass.EnumCrosshairType.None;
		}
		if (entityPlayerLocal.TPCameraCheckResult == eTPCameraCheckResult.Pass)
		{
			if (!this.bUseMeleeCrosshair)
			{
				return ItemClass.EnumCrosshairType.Crosshair;
			}
			return ItemClass.EnumCrosshairType.Plus;
		}
		else
		{
			if (entityPlayerLocal.TPCameraCheckResult == eTPCameraCheckResult.LineOfSightCheckFailed)
			{
				return ItemClass.EnumCrosshairType.Blocked;
			}
			return ItemClass.EnumCrosshairType.None;
		}
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x00010E62 File Offset: 0x0000F062
	public override RenderCubeType GetFocusType(ItemActionData _actionData)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x00132CC4 File Offset: 0x00130EC4
	public override void OnModificationsChanged(ItemActionData _data)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = _data as ItemActionRanged.ItemActionDataRanged;
		if (itemActionDataRanged.OriginalDelay == -1f)
		{
			this.Properties.ParseFloat("Delay", ref itemActionDataRanged.OriginalDelay);
		}
		ItemValue itemValue = itemActionDataRanged.invData.itemValue;
		string text = "";
		this.Properties.ParseString("Sound_start", ref text);
		itemActionDataRanged.SoundStart = itemValue.GetPropertyOverride("Sound_start", text);
		text = "";
		this.Properties.ParseString("Sound_loop", ref text);
		itemActionDataRanged.SoundLoop = itemValue.GetPropertyOverride("Sound_loop", text);
		text = "";
		this.Properties.ParseString("Sound_end", ref text);
		itemActionDataRanged.SoundEnd = itemValue.GetPropertyOverride("Sound_end", text);
		if (this.soundStart != null && this.soundStart.Contains("silenced"))
		{
			itemActionDataRanged.IsFlashSuppressed = true;
		}
		itemActionDataRanged.Laser = ((itemActionDataRanged.invData.model != null) ? ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(itemActionDataRanged.invData, "laser") : null);
		itemActionDataRanged.bReleased = true;
		if (itemActionDataRanged.ScopeTransform)
		{
			string text2 = "0,0,0";
			this.Properties.ParseString("ScopeOffset", ref text2);
			Vector3 vector = StringParsers.ParseVector3(itemValue.GetPropertyOverride("ScopeOffset", text2), 0, -1);
			if (itemActionDataRanged.ScopeTransform.localPosition != vector)
			{
				itemActionDataRanged.ScopeTransform.localPosition = vector;
			}
			text2 = "0,0,0";
			this.Properties.ParseString("ScopeRotation", ref text2);
			Vector3 vector2 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("ScopeRotation", text2), 0, -1);
			if (itemActionDataRanged.ScopeTransform.localRotation.eulerAngles != vector2)
			{
				itemActionDataRanged.ScopeTransform.localRotation = Quaternion.Euler(vector2);
			}
			text2 = "1,1,1";
			this.Properties.ParseString("ScopeScale", ref text2);
			Vector3 vector3 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("ScopeScale", text2), 0, -1);
			if (itemActionDataRanged.ScopeTransform.localScale != vector3)
			{
				itemActionDataRanged.ScopeTransform.localScale = vector3;
			}
		}
		if (itemActionDataRanged.SideTransform)
		{
			string text3 = "0,0,0";
			this.Properties.ParseString("SideOffset", ref text3);
			Vector3 vector4 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("SideOffset", text3), 0, -1);
			if (itemActionDataRanged.SideTransform.localPosition != vector4)
			{
				itemActionDataRanged.SideTransform.localPosition = vector4;
			}
			text3 = "0,0,0";
			this.Properties.ParseString("SideRotation", ref text3);
			Vector3 vector5 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("SideRotation", text3), 0, -1);
			if (itemActionDataRanged.SideTransform.localRotation.eulerAngles != vector5)
			{
				itemActionDataRanged.SideTransform.localRotation = Quaternion.Euler(vector5);
			}
			text3 = "1,1,1";
			this.Properties.ParseString("SideScale", ref text3);
			Vector3 vector6 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("SideScale", text3), 0, -1);
			if (itemActionDataRanged.SideTransform.localScale != vector6)
			{
				itemActionDataRanged.SideTransform.localScale = vector6;
			}
		}
		if (itemActionDataRanged.BarrelTransform)
		{
			string text4 = "0,0,0";
			this.Properties.ParseString("BarrelOffset", ref text4);
			Vector3 vector7 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("BarrelOffset", text4), 0, -1);
			if (itemActionDataRanged.BarrelTransform.localPosition != vector7)
			{
				itemActionDataRanged.BarrelTransform.localPosition = vector7;
			}
			text4 = "0,0,0";
			this.Properties.ParseString("BarrelRotation", ref text4);
			Vector3 vector8 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("BarrelRotation", text4), 0, -1);
			if (itemActionDataRanged.BarrelTransform.localRotation.eulerAngles != vector8)
			{
				itemActionDataRanged.BarrelTransform.localRotation = Quaternion.Euler(vector8);
			}
			text4 = "1,1,1";
			this.Properties.ParseString("BarrelScale", ref text4);
			Vector3 vector9 = StringParsers.ParseVector3(itemValue.GetPropertyOverride("BarrelScale", text4), 0, -1);
			if (itemActionDataRanged.BarrelTransform.localScale != vector9)
			{
				itemActionDataRanged.BarrelTransform.localScale = vector9;
			}
		}
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x00133100 File Offset: 0x00131300
	public override void StartHolding(ItemActionData _data)
	{
		base.StartHolding(_data);
		if (_data.invData.holdingEntity as EntityPlayerLocal != null)
		{
			TriggerEffectManager.ControllerTriggerEffect triggerEffect = TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.triggerEffectTriggerPullDualsense, this.triggerEffectTriggerPullXbox));
			GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, triggerEffect, false);
		}
	}

	// Token: 0x06002FD0 RID: 12240 RVA: 0x00133158 File Offset: 0x00131358
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_data;
		if (itemActionDataRanged.state != ItemActionFiringState.Off)
		{
			itemActionDataRanged.state = ItemActionFiringState.Off;
			this.ItemActionEffects(GameManager.Instance, itemActionDataRanged, 0, Vector3.zero, Vector3.forward, 0);
		}
		this.ResetBurstShot(itemActionDataRanged);
		itemActionDataRanged.bReleased = true;
		itemActionDataRanged.lastAccuracy = 1f;
		if (_data.invData.holdingEntity as EntityPlayerLocal != null)
		{
			GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, TriggerEffectManager.NoneEffect, false);
		}
		this.stopParticles(itemActionDataRanged.muzzle);
		this.stopParticles(itemActionDataRanged.muzzle2);
		if (Manager.IsASequence(itemActionDataRanged.invData.holdingEntity, itemActionDataRanged.SoundStart))
		{
			Manager.StopSequence(itemActionDataRanged.invData.holdingEntity, itemActionDataRanged.SoundStart);
		}
	}

	// Token: 0x06002FD1 RID: 12241 RVA: 0x00133228 File Offset: 0x00131428
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopParticles(Transform t)
	{
		if (t != null)
		{
			ParticleSystem[] componentsInChildren = t.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				global::UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
		}
	}

	// Token: 0x06002FD2 RID: 12242 RVA: 0x0013325C File Offset: 0x0013145C
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		if (ItemActionRanged.Reloading(itemActionDataRanged))
		{
			return true;
		}
		float num = Time.time - itemActionDataRanged.m_LastShotTime;
		return (!this.rapidTrigger || !itemActionDataRanged.bReleased || num <= 0.25f) && num < itemActionDataRanged.Delay;
	}

	// Token: 0x06002FD3 RID: 12243 RVA: 0x001332B0 File Offset: 0x001314B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeCurrentlyLoadedAmmunition(ItemValue _gun, ItemValue _ammo, EntityAlive _entity)
	{
		ItemStack itemStack = new ItemStack(_ammo, _gun.Meta);
		int itemCount = _entity.bag.GetItemCount(_ammo, -1, -1, true);
		int itemCount2 = _entity.inventory.GetItemCount(_ammo, false, -1, -1, true);
		EntityPlayerLocal entityPlayerLocal = _entity as EntityPlayerLocal;
		if (itemStack.count > 0)
		{
			if (itemCount > 0)
			{
				if (!entityPlayerLocal.bag.AddItem(itemStack) && !entityPlayerLocal.inventory.AddItem(itemStack))
				{
					GameManager.Instance.ItemDropServer(itemStack, entityPlayerLocal.GetPosition(), new Vector3(0.5f, 0f, 0.5f), entityPlayerLocal.entityId, 60f, false);
					entityPlayerLocal.PlayOneShot("itemdropped", false, false, false, null, 1f);
				}
			}
			else if (itemCount2 > 0)
			{
				if (!entityPlayerLocal.inventory.AddItem(itemStack) && !entityPlayerLocal.bag.AddItem(itemStack))
				{
					GameManager.Instance.ItemDropServer(itemStack, entityPlayerLocal.GetPosition(), new Vector3(0.5f, 0f, 0.5f), entityPlayerLocal.entityId, 60f, false);
					entityPlayerLocal.PlayOneShot("itemdropped", false, false, false, null, 1f);
				}
			}
			else if (!entityPlayerLocal.bag.AddItem(itemStack) && !entityPlayerLocal.inventory.AddItem(itemStack))
			{
				GameManager.Instance.ItemDropServer(itemStack, entityPlayerLocal.GetPosition(), new Vector3(0.5f, 0f, 0.5f), entityPlayerLocal.entityId, 60f, false);
				entityPlayerLocal.PlayOneShot("itemdropped", false, false, false, null, 1f);
			}
		}
		_gun.Meta = 0;
		_entity.inventory.CallOnToolbeltChangedInternal();
	}

	// Token: 0x06002FD4 RID: 12244 RVA: 0x0013344E File Offset: 0x0013164E
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadNewAmmunition(ItemValue _gun, ItemValue _ammo, EntityAlive _entity)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_entity.inventory.holdingItemData.actionData[0];
		if ((int)_gun.SelectedAmmoTypeIndex == this.MagazineItemNames.Length)
		{
			_gun.SelectedAmmoTypeIndex = 0;
		}
		itemActionDataRanged.isChangingAmmoType = true;
	}

	// Token: 0x06002FD5 RID: 12245 RVA: 0x00133488 File Offset: 0x00131688
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSelectedAmmoById(int _ammoItemId, ItemValue _gun)
	{
		for (int i = 0; i < this.MagazineItemNames.Length; i++)
		{
			if (ItemClass.GetItem(this.MagazineItemNames[i], false).type == _ammoItemId)
			{
				_gun.SelectedAmmoTypeIndex = (byte)i;
				return;
			}
		}
	}

	// Token: 0x06002FD6 RID: 12246 RVA: 0x001334C8 File Offset: 0x001316C8
	public virtual void SetAmmoType(EntityAlive _entity, ref ItemValue _gun, int _lastSelectedIndex, int _newSelectedIndex)
	{
		_gun.SelectedAmmoTypeIndex = (byte)_newSelectedIndex;
		if (_gun.Equals(_entity.inventory.holdingItemItemValue))
		{
			this.SwapAmmoType(_entity, ItemClass.GetItem(this.MagazineItemNames[_newSelectedIndex], false).type);
			return;
		}
		ItemValue item = ItemClass.GetItem(this.MagazineItemNames[_lastSelectedIndex], false);
		ItemClass.GetItem(this.MagazineItemNames[_newSelectedIndex], false);
		this.removeCurrentlyLoadedAmmunition(_gun, item, _entity);
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = _entity.inventory.holdingItemData.actionData[0] as ItemActionRanged.ItemActionDataRanged;
		this.requestReload(itemActionDataRanged);
	}

	// Token: 0x06002FD7 RID: 12247 RVA: 0x0013355C File Offset: 0x0013175C
	public void CompleteReload(ItemActionRanged.ItemActionDataRanged _adr)
	{
		if (_adr == null)
		{
			return;
		}
		EntityAlive holdingEntity = _adr.invData.holdingEntity;
		ItemValue item = ItemClass.GetItem(this.MagazineItemNames[(int)_adr.invData.itemValue.SelectedAmmoTypeIndex], false);
		int num = (int)EffectManager.GetValue(PassiveEffects.MagazineSize, _adr.invData.itemValue, (float)this.BulletsPerMagazine, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (this.HasInfiniteAmmo(_adr))
		{
			_adr.reloadAmount = (this.AmmoIsPerMagazine ? num : (num - _adr.invData.itemValue.Meta));
		}
		else if (this.AmmoIsPerMagazine)
		{
			if (holdingEntity.bag.GetItemCount(item, -1, -1, true) > 0)
			{
				_adr.reloadAmount = num * holdingEntity.bag.DecItem(item, 1, false, null);
			}
			else if (holdingEntity.inventory.GetItemCount(item, false, -1, -1, true) > 0)
			{
				_adr.reloadAmount = num * holdingEntity.inventory.DecItem(item, 1, false, null);
			}
		}
		else
		{
			int num2 = num - _adr.invData.itemValue.Meta;
			_adr.reloadAmount = holdingEntity.bag.DecItem(item, num2, false, null);
			if (_adr.reloadAmount < num2)
			{
				_adr.reloadAmount += holdingEntity.inventory.DecItem(item, num2 - _adr.reloadAmount, false, null);
			}
		}
		if (_adr.reloadAmount > 0)
		{
			_adr.invData.itemValue.Meta = Utils.FastMin(_adr.invData.itemValue.Meta + _adr.reloadAmount, num);
			if (_adr.invData.item.Properties.GetValue(ItemClass.PropSoundIdle) != null)
			{
				_adr.invData.holdingEntitySoundID = -1;
			}
		}
		_adr.invData.holdingEntity.inventory.holdingItemItemValue.RemoveMetaData(ItemActionRanged.scGunIsJammed);
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x0013372E File Offset: 0x0013192E
	[PublicizedFrom(EAccessModifier.Private)]
	public void requestReload(ItemActionRanged.ItemActionDataRanged _adr)
	{
		if (_adr != null)
		{
			_adr.isReloadRequested = true;
			GameManager.Instance.ItemReloadServer(_adr.invData.holdingEntity.entityId);
		}
	}

	// Token: 0x06002FD9 RID: 12249 RVA: 0x00133754 File Offset: 0x00131954
	public void SwapSelectedAmmo(EntityAlive _entity, int _ammoIndex)
	{
		if (_ammoIndex == (int)_entity.inventory.holdingItemItemValue.SelectedAmmoTypeIndex)
		{
			ItemActionRanged.ItemActionDataRanged itemActionDataRanged = _entity.inventory.holdingItemData.actionData[0] as ItemActionRanged.ItemActionDataRanged;
			if (itemActionDataRanged != null && _entity.inventory.GetHoldingGun().CanReload(itemActionDataRanged))
			{
				this.requestReload(itemActionDataRanged);
			}
			return;
		}
		ItemClass itemClass = ItemClass.GetItemClass(this.MagazineItemNames[_ammoIndex], false);
		if (itemClass != null)
		{
			this.SwapAmmoType(_entity, itemClass.Id);
		}
	}

	// Token: 0x06002FDA RID: 12250 RVA: 0x001337D0 File Offset: 0x001319D0
	public override void SwapAmmoType(EntityAlive _entity, int _ammoItemId = -1)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_entity.inventory.holdingItemData.actionData[0];
		this.CancelReload(itemActionDataRanged, true);
		ItemValue itemValue = itemActionDataRanged.invData.itemValue;
		EntityAlive holdingEntity = itemActionDataRanged.invData.holdingEntity;
		ItemValue item = ItemClass.GetItem(this.MagazineItemNames[(int)itemValue.SelectedAmmoTypeIndex], false);
		itemActionDataRanged.reloadAmount = 0;
		this.removeCurrentlyLoadedAmmunition(itemValue, item, holdingEntity);
		if (_ammoItemId == -1)
		{
			for (int i = 0; i < this.MagazineItemNames.Length; i++)
			{
				ItemValue itemValue2 = itemValue;
				itemValue2.SelectedAmmoTypeIndex += 1;
				if ((int)itemValue.SelectedAmmoTypeIndex == this.MagazineItemNames.Length)
				{
					itemValue.SelectedAmmoTypeIndex = 0;
				}
				ItemValue item2 = ItemClass.GetItem(this.MagazineItemNames[(int)itemValue.SelectedAmmoTypeIndex], false);
				if (itemActionDataRanged.invData.holdingEntity.inventory.GetItemCount(item2, false, -1, -1, true) + itemActionDataRanged.invData.holdingEntity.bag.GetItemCount(item2, -1, -1, true) + itemActionDataRanged.invData.itemValue.Meta > 0)
				{
					break;
				}
			}
		}
		else
		{
			this.setSelectedAmmoById(_ammoItemId, itemValue);
		}
		ItemValue item3 = ItemClass.GetItem(this.MagazineItemNames[(int)itemValue.SelectedAmmoTypeIndex], false);
		_entity.inventory.CallOnToolbeltChangedInternal();
		this.loadNewAmmunition(itemValue, item3, holdingEntity);
		EntityPlayerLocal entityPlayerLocal = itemActionDataRanged.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			itemActionDataRanged.invData.holdingEntity.ForceHoldingWeaponUpdate();
		}
		this.requestReload(itemActionDataRanged);
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.HolsterWeapon(false);
		}
	}

	// Token: 0x06002FDB RID: 12251 RVA: 0x00133960 File Offset: 0x00131B60
	[PublicizedFrom(EAccessModifier.Private)]
	public void CycleAmmoType(ItemActionData _actionData, bool excludeNonUnderwaterAmmoTypes)
	{
		if (this.MagazineItemNames.Length <= 1)
		{
			return;
		}
		int num = (int)_actionData.invData.holdingEntity.inventory.holdingItemItemValue.SelectedAmmoTypeIndex;
		int num2 = num;
		num--;
		while (num != num2)
		{
			ItemValue item = ItemClass.GetItem(this.MagazineItemNames[num], false);
			if (excludeNonUnderwaterAmmoTypes && !item.ItemClass.UsableUnderwater)
			{
				num--;
			}
			else
			{
				if (_actionData.invData.holdingEntity.bag.GetItemCount(item, -1, -1, true) + _actionData.invData.holdingEntity.inventory.GetItemCount(item, false, -1, -1, true) > 0)
				{
					break;
				}
				num--;
				if (num < 0)
				{
					num = this.MagazineItemNames.Length - 1;
				}
			}
		}
		this.SwapSelectedAmmo(_actionData.invData.holdingEntity, num);
	}

	// Token: 0x06002FDC RID: 12252 RVA: 0x00133A24 File Offset: 0x00131C24
	public virtual bool IsAmmoUsableUnderwater(EntityAlive holdingEntity)
	{
		if (!this.UsesMagazines)
		{
			return true;
		}
		int selectedAmmoTypeIndex = (int)holdingEntity.inventory.holdingItemItemValue.SelectedAmmoTypeIndex;
		return ItemClass.GetItem(this.MagazineItemNames[selectedAmmoTypeIndex], false).ItemClass.UsableUnderwater;
	}

	// Token: 0x06002FDD RID: 12253 RVA: 0x00133A64 File Offset: 0x00131C64
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		itemActionDataRanged.Delay = 60f / EffectManager.GetValue(PassiveEffects.RoundsPerMinute, itemActionDataRanged.invData.itemValue, 60f / itemActionDataRanged.OriginalDelay, itemActionDataRanged.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (itemActionDataRanged.burstShotStarted && !itemActionDataRanged.burstShotFinished)
		{
			this.TryExecuteAction(_actionData);
		}
		float lastUseTime = itemActionDataRanged.lastUseTime;
		itemActionDataRanged.lastUseTime = Time.time;
		float num = itemActionDataRanged.lastUseTime - lastUseTime;
		if (_actionData.invData.holdingEntity.isHeadUnderwater && _actionData.invData.itemValue.ItemClass != null && this.MagazineItemNames != null && !ItemClass.GetItemClass(this.MagazineItemNames[(int)_actionData.invData.itemValue.SelectedAmmoTypeIndex], false).UsableUnderwater)
		{
			this.CycleAmmoType(_actionData, true);
			return;
		}
		if (itemActionDataRanged.state != ItemActionFiringState.Off && itemActionDataRanged.m_LastShotTime > 0f && Time.time > itemActionDataRanged.m_LastShotTime + itemActionDataRanged.Delay * 2f && !itemActionDataRanged.burstShotStarted)
		{
			this.triggerReleased(itemActionDataRanged, _actionData.indexInEntityOfAction);
		}
		this.updateAccuracy(_actionData, _actionData.invData.holdingEntity.AimingGun, num);
		if (itemActionDataRanged.SideTransform && itemActionDataRanged.Laser == null && itemActionDataRanged.SideTransform.childCount > 0)
		{
			itemActionDataRanged.Laser = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_actionData.invData, "laser");
		}
		if (ItemAction.ShowDistanceDebugInfo || (_actionData as ItemActionRanged.ItemActionDataRanged).Laser != null)
		{
			this.GetExecuteActionTarget(_actionData);
		}
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x00133C08 File Offset: 0x00131E08
	public static bool ReloadCancelled(ItemActionRanged.ItemActionDataRanged actionData)
	{
		return actionData.isReloadCancelled || actionData.isWeaponReloadCancelled;
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x00133C1A File Offset: 0x00131E1A
	public static bool NotReloadCancelled(ItemActionRanged.ItemActionDataRanged actionData)
	{
		return !actionData.isReloadCancelled || !actionData.isWeaponReloadCancelled;
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x00133C2F File Offset: 0x00131E2F
	public static bool Reloading(ItemActionRanged.ItemActionDataRanged actionData)
	{
		return actionData.isReloading || actionData.isWeaponReloading || actionData.isReloadRequested;
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x00133C49 File Offset: 0x00131E49
	public static bool NotReloading(ItemActionRanged.ItemActionDataRanged actionData)
	{
		return !actionData.isReloading && !actionData.isWeaponReloading && !actionData.isReloadRequested;
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x00133C68 File Offset: 0x00131E68
	public override void CancelReload(ItemActionData _data, bool holsterWeapon)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_data;
		bool flag = ItemActionRanged.NotReloading(itemActionDataRanged);
		if (flag)
		{
			itemActionDataRanged.invData.holdingEntity.emodel.avatarController.SetReloadBool(false);
		}
		if (flag || ItemActionRanged.ReloadCancelled(itemActionDataRanged))
		{
			return;
		}
		base.CancelReload(_data, holsterWeapon);
		itemActionDataRanged.isReloadCancelled = true;
		itemActionDataRanged.isWeaponReloadCancelled = itemActionDataRanged.invData.item.HasReloadAnim;
		itemActionDataRanged.isChangingAmmoType = false;
		itemActionDataRanged.invData.holdingEntity.emodel.avatarController.SetReloadBool(false);
		if (itemActionDataRanged.state != ItemActionFiringState.Off)
		{
			itemActionDataRanged.state = ItemActionFiringState.Off;
			this.ItemActionEffects(GameManager.Instance, itemActionDataRanged, 0, Vector3.zero, Vector3.forward, 0);
		}
	}

	// Token: 0x06002FE3 RID: 12259 RVA: 0x00133D1C File Offset: 0x00131F1C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isJammed(ItemValue iv)
	{
		int num;
		return iv.TryGetMetadata(ItemActionRanged.scGunIsJammed, out num);
	}

	// Token: 0x06002FE4 RID: 12260 RVA: 0x00133D38 File Offset: 0x00131F38
	public ItemActionRanged.ReloadFlags GetReloadFlags(ItemActionData _actionData)
	{
		ItemActionRanged.ReloadFlags reloadFlags = ItemActionRanged.ReloadFlags.InvalidItem;
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = _actionData as ItemActionRanged.ItemActionDataRanged;
		if (itemActionDataRanged != null && itemActionDataRanged.invData.holdingEntity != null)
		{
			EntityAlive holdingEntity = itemActionDataRanged.invData.holdingEntity;
			ItemValue holdingItemItemValue = holdingEntity.inventory.holdingItemItemValue;
			ItemValue item = ItemClass.GetItem(this.MagazineItemNames[(int)holdingItemItemValue.SelectedAmmoTypeIndex], false);
			int num = (int)EffectManager.GetValue(PassiveEffects.MagazineSize, holdingItemItemValue, (float)this.BulletsPerMagazine, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			if (!ItemActionRanged.NotReloading(itemActionDataRanged))
			{
				reloadFlags |= ItemActionRanged.ReloadFlags.AlreadyReloading;
			}
			EntityPlayerLocal entityPlayerLocal = holdingEntity as EntityPlayerLocal;
			if (entityPlayerLocal && entityPlayerLocal.CancellingInventoryActions)
			{
				reloadFlags |= ItemActionRanged.ReloadFlags.PendingInventoryActions;
			}
			if (this.isJammed(holdingItemItemValue))
			{
				reloadFlags |= ItemActionRanged.ReloadFlags.Jammed;
			}
			if (_actionData.invData.itemValue.Meta == 0)
			{
				reloadFlags |= ItemActionRanged.ReloadFlags.EmptyClip;
			}
			else if (_actionData.invData.itemValue.Meta > 0)
			{
				reloadFlags |= ItemActionRanged.ReloadFlags.PartialClip;
			}
			else if (_actionData.invData.itemValue.Meta == num)
			{
				reloadFlags |= ItemActionRanged.ReloadFlags.FullClip;
			}
			if (holdingEntity.inventory.GetItemCount(item, false, -1, -1, true) == 0)
			{
				Bag bag = holdingEntity.bag;
				if (bag != null && bag.GetItemCount(item, -1, -1, true) == 0 && !this.HasInfiniteAmmo(_actionData))
				{
					reloadFlags |= ItemActionRanged.ReloadFlags.OutOfAmmo;
				}
			}
			reloadFlags &= ~ItemActionRanged.ReloadFlags.InvalidItem;
		}
		return reloadFlags;
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x00133E88 File Offset: 0x00132088
	public override bool CanReload(ItemActionData _actionData)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue holdingItemItemValue = holdingEntity.inventory.holdingItemItemValue;
		ItemValue item = ItemClass.GetItem(this.MagazineItemNames[(int)holdingItemItemValue.SelectedAmmoTypeIndex], false);
		int num = (int)EffectManager.GetValue(PassiveEffects.MagazineSize, holdingItemItemValue, (float)this.BulletsPerMagazine, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		EntityPlayerLocal entityPlayerLocal = holdingEntity as EntityPlayerLocal;
		if (ItemActionRanged.NotReloading(itemActionDataRanged) && (!entityPlayerLocal || !entityPlayerLocal.CancellingInventoryActions) && (this.isJammed(holdingItemItemValue) || _actionData.invData.itemValue.Meta < num))
		{
			if (holdingEntity.inventory.GetItemCount(item, false, -1, -1, true) <= 0)
			{
				Bag bag = holdingEntity.bag;
				if (bag == null || bag.GetItemCount(item, -1, -1, true) <= 0)
				{
					return this.HasInfiniteAmmo(_actionData);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002FE6 RID: 12262 RVA: 0x00133F64 File Offset: 0x00132164
	public override void ReloadGun(ItemActionData _actionData)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = _actionData as ItemActionRanged.ItemActionDataRanged;
		if (itemActionDataRanged != null)
		{
			itemActionDataRanged.isReloadRequested = false;
			this.ResetBurstShot(itemActionDataRanged);
			if (!itemActionDataRanged.invData.holdingEntity.isEntityRemote)
			{
				Manager.StopSequence(itemActionDataRanged.invData.holdingEntity, itemActionDataRanged.SoundStart);
				itemActionDataRanged.invData.holdingEntity.emodel.avatarController.CancelEvent("WeaponFire");
				itemActionDataRanged.invData.holdingEntity.OnReloadStart();
			}
		}
	}

	// Token: 0x06002FE7 RID: 12263 RVA: 0x00133FE0 File Offset: 0x001321E0
	public override bool IsAimingGunPossible(ItemActionData _actionData)
	{
		return ItemActionRanged.NotReloading((ItemActionRanged.ItemActionDataRanged)_actionData);
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x00133FED File Offset: 0x001321ED
	public override EnumCameraShake GetCameraShakeType(ItemActionData _actionData)
	{
		if (!_actionData.invData.holdingEntity.AimingGun)
		{
			return EnumCameraShake.Small;
		}
		return EnumCameraShake.None;
	}

	// Token: 0x06002FE9 RID: 12265 RVA: 0x00134004 File Offset: 0x00132204
	public override TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		return TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.triggerEffectTriggerPullDualsense, this.triggerEffectTriggerPullXbox));
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x0013401C File Offset: 0x0013221C
	public override TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		return TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.triggerEffectShootDualsense, this.triggerEffectShootXbox));
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x00134034 File Offset: 0x00132234
	public override bool AllowItemLoopingSound(ItemActionData _actionData)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		int burstCount = this.GetBurstCount(_actionData);
		return _actionData.invData.itemValue.Meta > 0 && burstCount > 0 && (int)itemActionDataRanged.curBurstCount <= burstCount && !string.IsNullOrEmpty(this.soundRepeat) && itemActionDataRanged.state == ItemActionFiringState.Loop;
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x00134088 File Offset: 0x00132288
	public override void ItemActionEffects(GameManager _gameManager, ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
		if (GameManager.Instance.IsPaused())
		{
			return;
		}
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = _actionData as ItemActionRanged.ItemActionDataRanged;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		EntityPlayerLocal entityPlayerLocal = holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			ItemActionFiringState state = itemActionDataRanged.state;
			if (state != ItemActionFiringState.Off)
			{
				if (state - ItemActionFiringState.Start > 1)
				{
				}
				TriggerEffectManager.ControllerTriggerEffect controllerTriggerEffect = TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.triggerEffectShootDualsense, this.triggerEffectShootXbox));
				GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, controllerTriggerEffect, false);
			}
			else
			{
				TriggerEffectManager.ControllerTriggerEffect controllerTriggerEffect = TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.triggerEffectTriggerPullDualsense, this.triggerEffectTriggerPullXbox));
				GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, controllerTriggerEffect, false);
			}
		}
		bool flag = false;
		if (itemActionDataRanged.state != ItemActionFiringState.Off || holdingEntity.isEntityRemote)
		{
			if (_firingState == 0 && itemActionDataRanged.invData.itemValue.Meta != 0)
			{
				if (!Manager.IsASequence(holdingEntity, itemActionDataRanged.SoundStart))
				{
					if (itemActionDataRanged.state != ItemActionFiringState.Off)
					{
						Manager.Play(holdingEntity, itemActionDataRanged.SoundEnd, 1f, false);
					}
				}
				else if (itemActionDataRanged.state != ItemActionFiringState.Off || holdingEntity.isEntityRemote)
				{
					Manager.StopSequence(holdingEntity, itemActionDataRanged.SoundStart);
				}
			}
			else if (_firingState != 0 && itemActionDataRanged.invData.itemValue.Meta == 0)
			{
				if (!Manager.IsASequence(holdingEntity, itemActionDataRanged.SoundStart))
				{
					if (itemActionDataRanged.state != ItemActionFiringState.Off)
					{
						Manager.Play(holdingEntity, itemActionDataRanged.SoundStart, 1f, false);
						flag = true;
					}
				}
				else if (itemActionDataRanged.state != ItemActionFiringState.Off || holdingEntity.isEntityRemote)
				{
					Manager.StopSequence(holdingEntity, itemActionDataRanged.SoundStart);
				}
			}
			else if (itemActionDataRanged.invData.itemValue.Meta == 0 && Manager.IsASequence(holdingEntity, itemActionDataRanged.SoundStart) && (itemActionDataRanged.state != ItemActionFiringState.Off || holdingEntity.isEntityRemote))
			{
				Manager.StopSequence(holdingEntity, itemActionDataRanged.SoundStart);
			}
		}
		if (_firingState != 0)
		{
			this.onHoldingEntityFired(_actionData);
			string text = ((_firingState == 1) ? itemActionDataRanged.SoundStart : itemActionDataRanged.SoundLoop);
			if (!string.IsNullOrEmpty(text))
			{
				if (!Manager.IsASequence(holdingEntity, text))
				{
					if (!flag || _firingState != 1)
					{
						Manager.Play(holdingEntity, text, 1f, false);
					}
				}
				else
				{
					Manager.PlaySequence(holdingEntity, text);
				}
			}
			if (holdingEntity.inventory.IsHUDDisabled())
			{
				return;
			}
			if (!itemActionDataRanged.IsFlashSuppressed && itemActionDataRanged.muzzle)
			{
				bool flag2 = entityPlayerLocal && entityPlayerLocal.bFirstPersonView;
				if (this.particlesMuzzleFire != null)
				{
					ParticleEffect particleEffect = new ParticleEffect((flag2 && this.particlesMuzzleFireFpv != null) ? this.particlesMuzzleFireFpv : this.particlesMuzzleFire, Vector3.zero, 1f, Color.clear, null, itemActionDataRanged.muzzle, false, 1f, "");
					Transform transform = _gameManager.SpawnParticleEffectClientForceCreation(particleEffect, holdingEntity.entityId, false);
					if (transform)
					{
						if (itemActionDataRanged.IsDoubleBarrel && itemActionDataRanged.invData.itemValue.Meta == 0)
						{
							transform.SetParent(itemActionDataRanged.muzzle2, false);
						}
						else
						{
							transform.SetParent(itemActionDataRanged.muzzle, false);
						}
						if (transform.GetComponentsInChildren<ParticleSystem>().Length != 0 && entityPlayerLocal == GameManager.Instance.World.GetPrimaryPlayer() && entityPlayerLocal.vp_FPCamera.OnValue_IsFirstPerson)
						{
							Utils.SetLayerRecursively(transform.gameObject, 10);
						}
					}
				}
				if (this.particlesMuzzleSmoke != null)
				{
					float num = _gameManager.World.GetLightBrightness(World.worldToBlockPos(itemActionDataRanged.muzzle.position)) / 2f;
					ParticleEffect particleEffect2 = new ParticleEffect((flag2 && this.particlesMuzzleSmokeFpv != null) ? this.particlesMuzzleSmokeFpv : this.particlesMuzzleSmoke, Vector3.zero, num, Color.clear, null, null, false, 1f, "");
					Transform transform2 = _gameManager.SpawnParticleEffectClientForceCreation(particleEffect2, holdingEntity.entityId, false);
					if (transform2 && entityPlayerLocal == GameManager.Instance.World.GetPrimaryPlayer() && entityPlayerLocal.vp_FPCamera.OnValue_IsFirstPerson)
					{
						transform2.gameObject.layer = 10;
					}
				}
			}
		}
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x0013446C File Offset: 0x0013266C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onHoldingEntityFired(ItemActionData _actionData)
	{
		if (!_actionData.invData.holdingEntity.isEntityRemote)
		{
			_actionData.invData.holdingEntity.emodel.avatarController.SetMeleeAttackSpeed(1f / ((ItemActionRanged.ItemActionDataRanged)_actionData).Delay);
			_actionData.invData.holdingEntity.OnFired();
		}
		(_actionData as ItemActionRanged.ItemActionDataRanged).lastAccuracy *= EffectManager.GetValue(PassiveEffects.IncrementalSpreadMultiplier, _actionData.invData.itemValue, 1f, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		(_actionData as ItemActionRanged.ItemActionDataRanged).lastAccuracy = Mathf.Min((_actionData as ItemActionRanged.ItemActionDataRanged).lastAccuracy, 5f);
		if (_actionData.invData.holdingEntity as EntityPlayerLocal != null)
		{
			GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.triggerEffectShootDualsense, this.triggerEffectShootXbox)), false);
		}
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x0013456C File Offset: 0x0013276C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void triggerReleased(ItemActionRanged.ItemActionDataRanged myActionData, int _idx)
	{
		if (myActionData.burstShotStarted && !myActionData.burstShotFinished)
		{
			return;
		}
		myActionData.bPressed = false;
		myActionData.bReleased = true;
		myActionData.invData.gameManager.ItemActionEffectsServer(myActionData.invData.holdingEntity.entityId, myActionData.invData.slotIdx, _idx, 0, Vector3.zero, Vector3.zero, 0);
		myActionData.state = ItemActionFiringState.Off;
		if (myActionData.invData.holdingEntity as EntityPlayerLocal != null)
		{
			GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.triggerEffectTriggerPullDualsense, this.triggerEffectTriggerPullXbox)), false);
		}
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x00134616 File Offset: 0x00132816
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ResetBurstShot(ItemActionRanged.ItemActionDataRanged _actionDataRanged)
	{
		_actionDataRanged.burstShotStarted = false;
		_actionDataRanged.burstShotFinished = false;
		_actionDataRanged.curBurstCount = 0;
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual int getUserData(ItemActionData _actionData)
	{
		return 0;
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x0013462D File Offset: 0x0013282D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void ConsumeAmmo(ItemActionData _actionData)
	{
		_actionData.invData.itemValue.Meta--;
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x00134648 File Offset: 0x00132848
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		itemActionDataRanged.bInitialPress = !_bReleased && !itemActionDataRanged.bPressed;
		itemActionDataRanged.bPressed = !_bReleased;
		if (_bReleased)
		{
			itemActionDataRanged.bReleased = true;
			if (Manager.IsASequence(_actionData.invData.holdingEntity, itemActionDataRanged.SoundStart))
			{
				Manager.StopSequence(_actionData.invData.holdingEntity, itemActionDataRanged.SoundStart);
			}
			this.triggerReleased(itemActionDataRanged, _actionData.indexInEntityOfAction);
			if (itemActionDataRanged.burstShotStarted && itemActionDataRanged.burstShotFinished)
			{
				this.ResetBurstShot(itemActionDataRanged);
			}
			return;
		}
		this.TryExecuteAction(_actionData);
	}

	// Token: 0x06002FF3 RID: 12275 RVA: 0x001346E0 File Offset: 0x001328E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TryExecuteAction(ItemActionData _actionData)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.StartTPCameraLockTimer();
			if (!entityPlayerLocal.TPCameraCheckPassed)
			{
				return;
			}
		}
		if (itemActionDataRanged.burstShotFinished)
		{
			return;
		}
		int burstCount = this.GetBurstCount(_actionData);
		bool bInitialPress = itemActionDataRanged.bInitialPress;
		bool flag = bInitialPress && (this.rapidTrigger || burstCount == 1);
		bool flag2 = burstCount == 0 || (int)itemActionDataRanged.curBurstCount < burstCount;
		if (burstCount > 0)
		{
			itemActionDataRanged.burstShotStarted = true;
		}
		if (itemActionDataRanged.burstShotStarted && (int)itemActionDataRanged.curBurstCount >= burstCount)
		{
			itemActionDataRanged.burstShotFinished = true;
			this.ResetBurstShot(itemActionDataRanged);
			if (burstCount != 1)
			{
				return;
			}
		}
		if (!flag && !flag2 && !itemActionDataRanged.bReleased)
		{
			return;
		}
		bool bReleased = itemActionDataRanged.bReleased;
		itemActionDataRanged.bReleased = false;
		float time = Time.time;
		if (ItemActionRanged.Reloading(itemActionDataRanged))
		{
			itemActionDataRanged.m_LastShotTime = time;
			return;
		}
		if (burstCount != 1 && !flag && time - itemActionDataRanged.m_LastShotTime < itemActionDataRanged.Delay)
		{
			return;
		}
		itemActionDataRanged.m_LastShotTime = time;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		holdingEntity.MinEventContext.Other = null;
		if (this.isJammed(holdingEntity.inventory.holdingItemItemValue))
		{
			itemActionDataRanged.m_LastShotTime = time + 1f;
			if (bInitialPress)
			{
				holdingEntity.FireEvent(MinEventTypes.onSelfItemJammedUse, true);
			}
			return;
		}
		if (EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, itemActionDataRanged.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			itemActionDataRanged.m_LastShotTime = time + 1f;
			if (bInitialPress)
			{
				Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			}
			return;
		}
		if (holdingEntity.isHeadUnderwater && !this.IsAmmoUsableUnderwater(holdingEntity))
		{
			if (bInitialPress)
			{
				GameManager.ShowTooltip(holdingEntity as EntityPlayerLocal, "ttCannotUseAtThisTime", false, false, 0f);
			}
			return;
		}
		if (itemActionDataRanged.invData.itemValue.PercentUsesLeft <= 0f)
		{
			itemActionDataRanged.m_LastShotTime = -1f;
			if (bInitialPress)
			{
				EntityPlayerLocal entityPlayerLocal2 = holdingEntity as EntityPlayerLocal;
				base.HandleJamSound(_actionData.invData.itemValue, this.item, entityPlayerLocal2);
			}
			return;
		}
		itemActionDataRanged.invData.holdingEntity.MinEventContext.ItemValue = itemActionDataRanged.invData.holdingEntity.inventory.holdingItemItemValue;
		itemActionDataRanged.invData.holdingEntity.MinEventContext.ItemActionData = itemActionDataRanged.invData.actionData[0];
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged2 = itemActionDataRanged;
		itemActionDataRanged2.curBurstCount += 1;
		if (!this.checkAmmo(itemActionDataRanged))
		{
			if (bReleased)
			{
				holdingEntity.PlayOneShot(this.soundEmpty, false, false, false, null, 1f);
				if (itemActionDataRanged.state != ItemActionFiringState.Off)
				{
					itemActionDataRanged.invData.gameManager.ItemActionEffectsServer(itemActionDataRanged.invData.holdingEntity.entityId, itemActionDataRanged.invData.slotIdx, itemActionDataRanged.indexInEntityOfAction, 0, Vector3.zero, Vector3.zero, 0);
				}
				itemActionDataRanged.state = ItemActionFiringState.Off;
				if (this.CanReload(itemActionDataRanged))
				{
					this.requestReload(itemActionDataRanged);
					itemActionDataRanged.invData.holdingEntitySoundID = -2;
				}
			}
			return;
		}
		itemActionDataRanged.invData.holdingEntity.FireEvent(MinEventTypes.onSelfRangedBurstShotEnd, true);
		itemActionDataRanged.invData.holdingEntity.FireEvent(MinEventTypes.onSelfRangedBurstShotStart, true);
		if (itemActionDataRanged.state == ItemActionFiringState.Off)
		{
			itemActionDataRanged.state = ItemActionFiringState.Start;
		}
		else
		{
			itemActionDataRanged.state = ItemActionFiringState.Loop;
		}
		if (!this.InfiniteAmmo)
		{
			this.ConsumeAmmo(_actionData);
		}
		int modelLayer = holdingEntity.GetModelLayer();
		holdingEntity.SetModelLayer(2, false, null);
		Vector3 vector = Vector3.zero;
		int num = (int)EffectManager.GetValue(PassiveEffects.RoundRayCount, itemActionDataRanged.invData.itemValue, 1f, itemActionDataRanged.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		bool flag3 = false;
		for (int i = 0; i < num; i++)
		{
			bool flag4 = false;
			vector = this.fireShot(i, itemActionDataRanged, ref flag4);
			if (flag4)
			{
				flag3 = true;
			}
		}
		if (!flag3 && holdingEntity != null)
		{
			holdingEntity.FireEvent((_actionData.indexInEntityOfAction == 0) ? MinEventTypes.onSelfPrimaryActionMissEntity : MinEventTypes.onSelfSecondaryActionMissEntity, true);
		}
		holdingEntity.SetModelLayer(modelLayer, false, null);
		Vector3 vector2;
		Vector3 vector3;
		int actionEffectsValues = this.GetActionEffectsValues(_actionData, out vector2, out vector3);
		itemActionDataRanged.invData.gameManager.ItemActionEffectsServer(holdingEntity.entityId, itemActionDataRanged.invData.slotIdx, itemActionDataRanged.indexInEntityOfAction, (int)itemActionDataRanged.state, vector2, vector3, actionEffectsValues | this.getUserData(_actionData));
		if (itemActionDataRanged.invData.itemValue.MaxUseTimes > 0)
		{
			_actionData.invData.itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, itemActionDataRanged.invData.itemValue, 1f, itemActionDataRanged.invData.holdingEntity, null, _actionData.invData.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
			if (itemActionDataRanged.invData.itemValue.PercentUsesLeft == 0f)
			{
				itemActionDataRanged.state = ItemActionFiringState.Off;
			}
		}
		if (this.GetMaxAmmoCount(itemActionDataRanged) == 1 && itemActionDataRanged.invData.itemValue.Meta == 0)
		{
			if (itemActionDataRanged.state != ItemActionFiringState.Off)
			{
				itemActionDataRanged.invData.gameManager.ItemActionEffectsServer(holdingEntity.entityId, itemActionDataRanged.invData.slotIdx, itemActionDataRanged.indexInEntityOfAction, 0, Vector3.zero, Vector3.zero, 0);
			}
			itemActionDataRanged.state = ItemActionFiringState.Off;
			this.item.StopHoldingAudio(itemActionDataRanged.invData);
			itemActionDataRanged.burstShotFinished = true;
			if (this.AutoReload && this.CanReload(itemActionDataRanged))
			{
				this.requestReload(itemActionDataRanged);
			}
		}
		Vector3 kickbackForce = base.GetKickbackForce(vector);
		holdingEntity.motion += kickbackForce * (holdingEntity.AimingGun ? 0.2f : 0.5f);
		holdingEntity.inventory.CallOnToolbeltChangedInternal();
		if (itemActionDataRanged.burstShotStarted && (int)itemActionDataRanged.curBurstCount >= burstCount)
		{
			itemActionDataRanged.burstShotFinished = true;
			bool bPressed = itemActionDataRanged.bPressed;
			this.triggerReleased(itemActionDataRanged, _actionData.indexInEntityOfAction);
			if (!bPressed)
			{
				this.ResetBurstShot(itemActionDataRanged);
			}
		}
		base.HandleItemBreak(_actionData);
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x00134CD1 File Offset: 0x00132ED1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool checkAmmo(ItemActionData _actionData)
	{
		return this.InfiniteAmmo || _actionData.invData.itemValue.Meta > 0;
	}

	// Token: 0x06002FF5 RID: 12277 RVA: 0x00134CF0 File Offset: 0x00132EF0
	public bool HasInfiniteAmmo(ItemActionData _actionData)
	{
		return EffectManager.GetValue(PassiveEffects.InfiniteAmmo, _actionData.invData.itemValue, 0f, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0f;
	}

	// Token: 0x06002FF6 RID: 12278 RVA: 0x00134D3C File Offset: 0x00132F3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float updateAccuracy(ItemActionData _actionData, bool _isAimingGun, float deltaTickTime)
	{
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue itemValue = _actionData.invData.itemValue;
		float num;
		if (_isAimingGun)
		{
			num = EffectManager.GetValue(PassiveEffects.SpreadMultiplierAiming, itemValue, 0.1f, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		else
		{
			num = EffectManager.GetValue(PassiveEffects.SpreadMultiplierHip, itemValue, 1f, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		if (holdingEntity.moveDirection == Vector3.zero)
		{
			num *= EffectManager.GetValue(PassiveEffects.SpreadMultiplierIdle, itemValue, 0.1f, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		else if (!holdingEntity.MovementRunning)
		{
			num *= EffectManager.GetValue(PassiveEffects.SpreadMultiplierWalking, itemValue, 1f, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		else
		{
			num *= EffectManager.GetValue(PassiveEffects.SpreadMultiplierRunning, itemValue, 1f, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		if (holdingEntity.IsCrouching)
		{
			num *= EffectManager.GetValue(PassiveEffects.SpreadMultiplierCrouching, itemValue, 1f, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		double num2 = (double)Mathf.Clamp01(EffectManager.GetValue(PassiveEffects.WeaponHandling, itemValue, 0.1f, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		itemActionDataRanged.lastAccuracy = ItemActionRanged.<updateAccuracy>g__AccuracyExpDecay|73_0(itemActionDataRanged.lastAccuracy, num, (double)ItemActionRanged.AccuracyUpdateDecayConstant * num2, deltaTickTime);
		return itemActionDataRanged.lastAccuracy;
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x00134EAE File Offset: 0x001330AE
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void ResetOldAccuracy()
	{
		ItemActionRanged._oldAccuracy = 1f;
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x00134EBC File Offset: 0x001330BC
	public virtual float GetRange(ItemActionData _actionData)
	{
		return EffectManager.GetValue(PassiveEffects.MaxRange, _actionData.invData.itemValue, this.Range, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x00134F00 File Offset: 0x00133100
	public virtual int GetMaxAmmoCount(ItemActionData _actionData)
	{
		return (int)EffectManager.GetValue(PassiveEffects.MagazineSize, _actionData.invData.itemValue, (float)this.BulletsPerMagazine, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x00134F44 File Offset: 0x00133144
	public virtual int GetBurstCount(ItemActionData _actionData)
	{
		return (int)EffectManager.GetValue(PassiveEffects.BurstRoundCount, _actionData.invData.itemValue, 1f, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x00134F88 File Offset: 0x00133188
	public virtual float GetIdealRangeForAI(ItemActionData _actionData)
	{
		return EffectManager.GetValue(PassiveEffects.MaxRange, _actionData.invData.itemValue, this.Range, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x00134FCC File Offset: 0x001331CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 getDirectionOffset(ItemActionRanged.ItemActionDataRanged _actionData, Vector3 _forward, int _shotOffset = 0)
	{
		float num = EffectManager.GetValue(PassiveEffects.SpreadDegreesHorizontal, _actionData.invData.itemValue, 45f, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		num *= _actionData.lastAccuracy;
		num *= (float)_actionData.MeanderNoise.Noise((double)Time.time, 0.0, (double)_shotOffset) * 0.66f;
		float num2 = EffectManager.GetValue(PassiveEffects.SpreadDegreesVertical, _actionData.invData.itemValue, 45f, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) * _actionData.lastAccuracy * ((float)_actionData.MeanderNoise.Noise(0.0, (double)Time.time, (double)_shotOffset) * 0.66f) + this.spreadVerticalOffset;
		Quaternion quaternion = Quaternion.LookRotation(_forward, Vector3.up);
		Vector3 vector = Quaternion.Euler(num2, num, 0f) * Vector3.forward;
		return quaternion * vector;
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x001350CC File Offset: 0x001332CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 getDirectionRandomOffset(ItemActionRanged.ItemActionDataRanged _actionData, Vector3 _forward)
	{
		float num = EffectManager.GetValue(PassiveEffects.SpreadDegreesHorizontal, _actionData.invData.itemValue, 45f, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		num *= _actionData.lastAccuracy;
		num *= _actionData.rand.RandomFloat * 2f - 1f;
		float num2 = EffectManager.GetValue(PassiveEffects.SpreadDegreesVertical, _actionData.invData.itemValue, 45f, _actionData.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) * _actionData.lastAccuracy * (_actionData.rand.RandomFloat * 2f - 1f) + this.spreadVerticalOffset;
		Quaternion quaternion = Quaternion.LookRotation(_forward, Vector3.up);
		Vector3 vector = Quaternion.Euler(num2, num, 0f) * Vector3.forward;
		return quaternion * vector;
	}

	// Token: 0x06002FFE RID: 12286 RVA: 0x001351B4 File Offset: 0x001333B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 fireShot(int _shotIdx, ItemActionRanged.ItemActionDataRanged _actionData, ref bool hitEntityFound)
	{
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		ItemValue itemValue = _actionData.invData.itemValue;
		float range = this.GetRange(_actionData);
		Ray lookRay = holdingEntity.GetLookRay();
		lookRay.direction = this.getDirectionOffset(_actionData, lookRay.direction, _shotIdx);
		_actionData.waterCollisionParticles.Reset();
		_actionData.waterCollisionParticles.CheckCollision(lookRay.origin, lookRay.direction, range, holdingEntity.entityId);
		int num = ((this.hitmaskOverride == 0) ? 8 : this.hitmaskOverride);
		int num2 = Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.EntityPenetrationCount, itemValue, (float)this.EntityPenetrationCount, holdingEntity, null, itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false));
		num2++;
		int num3 = Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.BlockPenetrationFactor, itemValue, (float)this.BlockPenetrationFactor, holdingEntity, null, itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false));
		EntityAlive entityAlive = null;
		hitEntityFound = false;
		for (int i = 0; i < num2; i++)
		{
			if (Voxel.Raycast(_actionData.invData.world, lookRay, range, -538751005, num, 0f))
			{
				WorldRayHitInfo worldRayHitInfo = Voxel.voxelRayHitInfo.Clone();
				if (worldRayHitInfo.hit.distanceSq > range * range)
				{
					return lookRay.direction;
				}
				lookRay.origin = worldRayHitInfo.hit.pos;
				if (worldRayHitInfo.tag.StartsWith("E_"))
				{
					EntityDrone component = worldRayHitInfo.transform.GetComponent<EntityDrone>();
					if (component && component.isAlly(holdingEntity as EntityPlayer))
					{
						lookRay.origin = worldRayHitInfo.hit.pos + lookRay.direction * 0.1f;
						i--;
						goto IL_0501;
					}
					string text;
					EntityAlive entityAlive2 = ItemActionAttack.FindHitEntityNoTagCheck(worldRayHitInfo, out text) as EntityAlive;
					if (entityAlive == entityAlive2)
					{
						lookRay.origin = worldRayHitInfo.hit.pos + lookRay.direction * 0.1f;
						i--;
						goto IL_0501;
					}
					holdingEntity.MinEventContext.Other = entityAlive2;
					entityAlive = entityAlive2;
					hitEntityFound = true;
				}
				else
				{
					BlockValue blockHit = ItemActionAttack.GetBlockHit(_actionData.invData.world, worldRayHitInfo);
					i += Mathf.FloorToInt((float)blockHit.Block.MaxDamage / (float)num3);
					holdingEntity.MinEventContext.BlockValue = blockHit;
				}
				float num4 = 1f;
				float value = EffectManager.GetValue(PassiveEffects.DamageFalloffRange, itemValue, range, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				if (worldRayHitInfo.hit.distanceSq > value * value)
				{
					num4 = 1f - (worldRayHitInfo.hit.distanceSq - value * value) / (range * range - value * value);
				}
				_actionData.attackDetails.isCriticalHit = holdingEntity.AimingGun;
				_actionData.attackDetails.WeaponTypeTag = ItemActionAttack.RangedTag;
				holdingEntity.FireEvent((_actionData.indexInEntityOfAction == 0) ? MinEventTypes.onSelfPrimaryActionRayHit : MinEventTypes.onSelfSecondaryActionRayHit, true);
				float num5 = 1f;
				World world = GameManager.Instance.World;
				Vector3i vector3i = World.worldToBlockPos(worldRayHitInfo.hit.pos);
				WaterValue water = world.GetWater(vector3i);
				if (water.HasMass())
				{
					Vector3i vector3i2 = new Vector3i(vector3i.x, vector3i.y + 1, vector3i.z);
					if (world.GetWater(vector3i2).GetMassPercent() > 0f)
					{
						num5 = 0.25f;
					}
					else
					{
						float num6 = worldRayHitInfo.hit.pos.y - (float)vector3i.y;
						float num7 = water.GetMassPercent() * 0.6f - num6;
						if (num7 > 0f)
						{
							num5 = 1f - 0.75f * num7;
						}
					}
				}
				float num8 = base.GetDamageEntity(itemValue, holdingEntity, 0) * num4 * num5;
				bool flag;
				if (holdingEntity is EntityPlayer)
				{
					float num9 = (itemValue.ItemClass.ItemTags.Test_AnySet(ItemActionAttack.MeleeTag) ? ItemActionAttack.MeleeDamagePercent : ItemActionAttack.RangedDamagePercent);
					num8 *= num9;
					flag = num9 == 0f;
				}
				else
				{
					num8 *= ItemActionAttack.EntityPlayerDamagePercent;
					flag = ItemActionAttack.EntityPlayerDamagePercent == 0f;
				}
				WorldRayHitInfo worldRayHitInfo2 = worldRayHitInfo;
				int entityId = holdingEntity.entityId;
				EnumDamageTypes enumDamageTypes = ((this.DamageType == EnumDamageTypes.None) ? EnumDamageTypes.Piercing : this.DamageType);
				float num10 = base.GetDamageBlock(itemValue, ItemActionAttack.GetBlockHit(_actionData.invData.world, worldRayHitInfo), holdingEntity, 0) * num4 * num5;
				float num11 = num8;
				float num12 = 1f;
				float percentUsesLeft = itemValue.PercentUsesLeft;
				float value2 = _actionData.invData.item.CritChance.Value;
				float dismemberChance = ItemAction.GetDismemberChance(_actionData, worldRayHitInfo);
				string text2 = this.bulletMaterialName;
				DamageMultiplier damageMultiplier = this.damageMultiplier;
				List<string> buffActions = this.getBuffActions(_actionData);
				ItemActionAttack.AttackHitInfo attackDetails = _actionData.attackDetails;
				int num13 = 0;
				int actionExp = this.ActionExp;
				float actionExpBonusMultiplier = this.ActionExpBonusMultiplier;
				ItemActionAttack itemActionAttack = null;
				Dictionary<string, ItemActionAttack.Bonuses> toolBonuses = this.ToolBonuses;
				ItemActionAttack.EnumAttackMode enumAttackMode = (this.bSupportHarvesting ? ItemActionAttack.EnumAttackMode.RealAndHarvesting : ItemActionAttack.EnumAttackMode.RealNoHarvesting);
				Dictionary<string, string> dictionary = null;
				int num14 = -1;
				bool flag2 = flag;
				Dictionary<string, float> soundImpactVolumeScale = this.soundImpactVolumeScale;
				ItemActionAttack.Hit(worldRayHitInfo2, entityId, enumDamageTypes, num10, num11, num12, percentUsesLeft, value2, dismemberChance, text2, damageMultiplier, buffActions, attackDetails, num13, actionExp, actionExpBonusMultiplier, itemActionAttack, toolBonuses, enumAttackMode, dictionary, num14, itemValue, false, flag2, true, soundImpactVolumeScale);
				if (this.bSupportHarvesting)
				{
					GameUtils.HarvestOnAttack(_actionData, this.ToolBonuses);
				}
			}
			else
			{
				holdingEntity.FireEvent((_actionData.indexInEntityOfAction == 0) ? MinEventTypes.onSelfPrimaryActionRayMiss : MinEventTypes.onSelfSecondaryActionRayMiss, true);
			}
			IL_0501:;
		}
		return lookRay.direction;
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x001356D8 File Offset: 0x001338D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public float getDamageBlock(ItemActionRanged.ItemActionDataRanged _actionData)
	{
		return base.GetDamageBlock(_actionData.invData.itemValue, BlockValue.Air, _actionData.invData.holdingEntity, _actionData.indexInEntityOfAction);
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x00135701 File Offset: 0x00133901
	[PublicizedFrom(EAccessModifier.Protected)]
	public float getDamageEntity(ItemActionRanged.ItemActionDataRanged _actionData)
	{
		return base.GetDamageEntity(_actionData.invData.itemValue, _actionData.invData.holdingEntity, _actionData.indexInEntityOfAction);
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x00135725 File Offset: 0x00133925
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual int GetActionEffectsValues(ItemActionData _actionData, out Vector3 _startPos, out Vector3 _direction)
	{
		_startPos = Vector3.zero;
		_direction = Vector3.zero;
		return 0;
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x00135740 File Offset: 0x00133940
	public override int GetInitialMeta(ItemValue _itemValue)
	{
		return (int)EffectManager.GetValue(PassiveEffects.MagazineSize, _itemValue, (float)this.BulletsPerMagazine, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x00135770 File Offset: 0x00133970
	public override WorldRayHitInfo GetExecuteActionTarget(ItemActionData _actionData)
	{
		ItemActionRanged.ItemActionDataRanged itemActionDataRanged = (ItemActionRanged.ItemActionDataRanged)_actionData;
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		float range = this.GetRange(_actionData);
		itemActionDataRanged.distance = range;
		int modelLayer = holdingEntity.GetModelLayer();
		holdingEntity.SetModelLayer(2, false, null);
		int num = ((this.hitmaskOverride == 0) ? 8 : this.hitmaskOverride);
		Ray lookRay = holdingEntity.GetLookRay();
		bool flag = ItemActionRanged.Reloading(itemActionDataRanged);
		EntityPlayer entityPlayer = holdingEntity as EntityPlayer;
		if (entityPlayer != null && itemActionDataRanged.Laser != null)
		{
			if (itemActionDataRanged.Laser.gameObject.activeInHierarchy)
			{
				EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
				if (entityPlayerLocal != null && !entityPlayerLocal.TPCameraCheckPassed)
				{
					lookRay = new Ray(_actionData.invData.holdingEntity.inventory.GetHoldingItemTransform().position + Origin.position, _actionData.invData.holdingEntity.inventory.GetHoldingItemTransform().forward);
				}
				if (Voxel.Raycast(_actionData.invData.world, lookRay, range, -538751005, num, 0f))
				{
					WorldRayHitInfo worldRayHitInfo = _actionData.GetUpdatedHitInfo();
					entityPlayer.SetLaserSightData(!flag, worldRayHitInfo.hit.pos);
				}
				else
				{
					entityPlayer.SetLaserSightData(false, lookRay.origin);
				}
			}
			else
			{
				entityPlayer.SetLaserSightData(false, lookRay.origin);
			}
		}
		lookRay.direction = this.getDirectionOffset(itemActionDataRanged, lookRay.direction, 0);
		bool flag2 = Voxel.Raycast(_actionData.invData.world, lookRay, range, -538751005, num, 0f);
		holdingEntity.SetModelLayer(modelLayer, false, null);
		if (flag2)
		{
			WorldRayHitInfo worldRayHitInfo = _actionData.GetUpdatedHitInfo();
			itemActionDataRanged.distance = Mathf.Sqrt(worldRayHitInfo.hit.distanceSq);
			itemActionDataRanged.damageFalloffPercent = 1f;
			if (itemActionDataRanged.Laser != null)
			{
				itemActionDataRanged.Laser.position = worldRayHitInfo.hit.pos - Origin.position;
				itemActionDataRanged.Laser.gameObject.SetActive(!flag);
			}
			float value = EffectManager.GetValue(PassiveEffects.DamageFalloffRange, _actionData.invData.itemValue, range, holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			if (worldRayHitInfo.hit.distanceSq > value * value)
			{
				itemActionDataRanged.damageFalloffPercent = 1f - (worldRayHitInfo.hit.distanceSq - value * value) / (range * range - value * value);
			}
			return worldRayHitInfo;
		}
		return null;
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x001359E8 File Offset: 0x00133BE8
	public override void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
		base.GetItemValueActionInfo(ref _infoList, _itemValue, _xui, 0);
		_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblHandling", false, null), EffectManager.GetValue(PassiveEffects.WeaponHandling, _itemValue, 0.1f, _xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false)));
		_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblRPM", false, null), EffectManager.GetValue(PassiveEffects.RoundsPerMinute, _itemValue, 60f / this.originalDelay, _xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false)));
		_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblAttributeFalloffRange", false, null), string.Format("{0} / {1} {2}", EffectManager.GetValue(PassiveEffects.DamageFalloffRange, _itemValue, 0f, _xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false).ToCultureInvariantString(), EffectManager.GetValue(PassiveEffects.MaxRange, _itemValue, this.Range, _xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false).ToCultureInvariantString(), Localization.Get("lblAttributeFalloffRangeText", false, null))));
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x00135B38 File Offset: 0x00133D38
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static float <updateAccuracy>g__AccuracyExpDecay|73_0(float initial, float target, double decay, float dt)
	{
		dt = Mathf.Clamp01(dt);
		float num = (float)((double)target + (double)(initial - target) * Math.Exp(-decay * (double)dt));
		if (Math.Abs(num - initial) < 1E-05f)
		{
			return target;
		}
		return num;
	}

	// Token: 0x040024EC RID: 9452
	public static string scGunIsJammed = "GunIsJammed";

	// Token: 0x040024ED RID: 9453
	public const float cUnderwaterDamageReductionMultipler = 0.25f;

	// Token: 0x040024EE RID: 9454
	public const float cUnderwaterDamagePenalty = 0.75f;

	// Token: 0x040024EF RID: 9455
	[PublicizedFrom(EAccessModifier.Private)]
	public string bulletMaterialName;

	// Token: 0x040024F0 RID: 9456
	public bool bSupportHarvesting;

	// Token: 0x040024F1 RID: 9457
	public bool bUseMeleeCrosshair;

	// Token: 0x040024F2 RID: 9458
	[PublicizedFrom(EAccessModifier.Private)]
	public float originalDelay = -1f;

	// Token: 0x040024F3 RID: 9459
	[PublicizedFrom(EAccessModifier.Private)]
	public string triggerEffectShootDualsense;

	// Token: 0x040024F4 RID: 9460
	[PublicizedFrom(EAccessModifier.Private)]
	public string triggerEffectTriggerPullDualsense;

	// Token: 0x040024F5 RID: 9461
	[PublicizedFrom(EAccessModifier.Private)]
	public string triggerEffectShootXbox;

	// Token: 0x040024F6 RID: 9462
	[PublicizedFrom(EAccessModifier.Private)]
	public string triggerEffectTriggerPullXbox;

	// Token: 0x040024F7 RID: 9463
	[PublicizedFrom(EAccessModifier.Private)]
	public bool rapidTrigger;

	// Token: 0x040024FA RID: 9466
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool AutoReload = true;

	// Token: 0x040024FB RID: 9467
	public int EntityPenetrationCount;

	// Token: 0x040024FC RID: 9468
	public int BlockPenetrationFactor = 251;

	// Token: 0x040024FD RID: 9469
	[PublicizedFrom(EAccessModifier.Private)]
	public float spreadVerticalOffset;

	// Token: 0x040024FE RID: 9470
	public static float AccuracyUpdateDecayConstant = 9.1f;

	// Token: 0x040024FF RID: 9471
	public static bool LogOldAccuracy;

	// Token: 0x04002500 RID: 9472
	[PublicizedFrom(EAccessModifier.Private)]
	public static float _oldAccuracy;

	// Token: 0x020005C0 RID: 1472
	public class ItemActionDataRanged : ItemActionAttackData
	{
		// Token: 0x06003007 RID: 12295 RVA: 0x00135B74 File Offset: 0x00133D74
		public ItemActionDataRanged(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
			this.IsDoubleBarrel = _invData.item.ItemTags.Test_Bit(FastTags<TagGroup.Global>.GetBit("dBarrel"));
			if (this.IsDoubleBarrel)
			{
				this.muzzle2 = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "Muzzle_R");
				this.muzzle = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "Muzzle_L");
			}
			else
			{
				this.muzzle = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "Muzzle");
			}
			this.Laser = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "laser");
			this.ScopeTransform = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "Attachments/Scope");
			this.SideTransform = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "Attachments/Side");
			this.BarrelTransform = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "Attachments/Barrel");
			this.hasScopeMod = this.ScopeTransform != null && this.ScopeTransform.childCount > 0;
			this.hasSideMod = this.SideTransform != null && this.SideTransform.childCount > 0;
			this.hasBarrelMod = this.BarrelTransform != null && this.BarrelTransform.childCount > 0;
			Transform transform = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "ironsight");
			if (transform == null)
			{
				transform = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "ironsights");
			}
			if (transform != null)
			{
				transform.gameObject.SetActive(!this.hasScopeMod);
			}
			Transform modelChildTransformByName = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "scope_rail");
			if (modelChildTransformByName != null)
			{
				modelChildTransformByName.gameObject.SetActive(this.hasScopeMod);
			}
			Transform modelChildTransformByName2 = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "side_rail");
			if (modelChildTransformByName2 != null)
			{
				modelChildTransformByName2.gameObject.SetActive(this.hasSideMod);
			}
			Transform modelChildTransformByName3 = ItemActionRanged.ItemActionDataRanged.getModelChildTransformByName(_invData, "barrel_rail");
			if (modelChildTransformByName3 != null)
			{
				modelChildTransformByName3.gameObject.SetActive(this.hasBarrelMod);
			}
			this.m_LastShotTime = -1f;
			this.MeanderNoise = new PerlinNoise(_invData.holdingEntity.entityId + _invData.item.Id);
			this.rand = _invData.holdingEntity.rand;
			this.waterCollisionParticles = new CollisionParticleController();
			this.waterCollisionParticles.Init(_invData.holdingEntity.entityId, _invData.item.MadeOfMaterial.SurfaceCategory, "water", 16);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x00135DD5 File Offset: 0x00133FD5
		public static Transform getModelChildTransformByName(ItemInventoryData _invData, string _name)
		{
			if (_invData.model == null)
			{
				return null;
			}
			if (_name.Contains("/"))
			{
				return _invData.model.Find(_name);
			}
			return _invData.model.FindInChilds(_name, false);
		}

		// Token: 0x04002501 RID: 9473
		public float m_LastShotTime;

		// Token: 0x04002502 RID: 9474
		public int reloadAmount;

		// Token: 0x04002503 RID: 9475
		public bool IsDoubleBarrel;

		// Token: 0x04002504 RID: 9476
		public Transform muzzle;

		// Token: 0x04002505 RID: 9477
		public Transform muzzle2;

		// Token: 0x04002506 RID: 9478
		public Transform Laser;

		// Token: 0x04002507 RID: 9479
		public ItemActionFiringState state;

		// Token: 0x04002508 RID: 9480
		public float lastTimeTriggerPressed;

		// Token: 0x04002509 RID: 9481
		public Vector3i currentDiggingLocation;

		// Token: 0x0400250A RID: 9482
		public float curBlockDamagePerHit;

		// Token: 0x0400250B RID: 9483
		public float curBlockDamage;

		// Token: 0x0400250C RID: 9484
		public bool bReleased;

		// Token: 0x0400250D RID: 9485
		public bool bPressed;

		// Token: 0x0400250E RID: 9486
		public GameRandom rand;

		// Token: 0x0400250F RID: 9487
		public PerlinNoise MeanderNoise;

		// Token: 0x04002510 RID: 9488
		public byte curBurstCount;

		// Token: 0x04002511 RID: 9489
		public float lastAccuracy;

		// Token: 0x04002512 RID: 9490
		public float distance;

		// Token: 0x04002513 RID: 9491
		public float damageFalloffPercent;

		// Token: 0x04002514 RID: 9492
		public bool isReloadRequested;

		// Token: 0x04002515 RID: 9493
		public bool isReloading;

		// Token: 0x04002516 RID: 9494
		public bool isWeaponReloading;

		// Token: 0x04002517 RID: 9495
		public bool isReloadCancelled;

		// Token: 0x04002518 RID: 9496
		public bool isWeaponReloadCancelled;

		// Token: 0x04002519 RID: 9497
		public bool wasReloadCancelled;

		// Token: 0x0400251A RID: 9498
		public bool wasWeaponReloadCancelled;

		// Token: 0x0400251B RID: 9499
		public bool wasAiming;

		// Token: 0x0400251C RID: 9500
		public bool isChangingAmmoType;

		// Token: 0x0400251D RID: 9501
		public Transform ScopeTransform;

		// Token: 0x0400251E RID: 9502
		public Transform SideTransform;

		// Token: 0x0400251F RID: 9503
		public Transform BarrelTransform;

		// Token: 0x04002520 RID: 9504
		public bool hasScopeMod;

		// Token: 0x04002521 RID: 9505
		public bool hasSideMod;

		// Token: 0x04002522 RID: 9506
		public bool hasBarrelMod;

		// Token: 0x04002523 RID: 9507
		public bool IsFlashSuppressed;

		// Token: 0x04002524 RID: 9508
		public string SoundStart;

		// Token: 0x04002525 RID: 9509
		public string SoundLoop;

		// Token: 0x04002526 RID: 9510
		public string SoundEnd;

		// Token: 0x04002527 RID: 9511
		public float Delay;

		// Token: 0x04002528 RID: 9512
		public float OriginalDelay = -1f;

		// Token: 0x04002529 RID: 9513
		public bool burstShotStarted;

		// Token: 0x0400252A RID: 9514
		public bool burstShotFinished;

		// Token: 0x0400252B RID: 9515
		public bool bInitialPress;

		// Token: 0x0400252C RID: 9516
		public CollisionParticleController waterCollisionParticles = new CollisionParticleController();
	}

	// Token: 0x020005C1 RID: 1473
	[Flags]
	public enum ReloadFlags
	{
		// Token: 0x0400252E RID: 9518
		None = 0,
		// Token: 0x0400252F RID: 9519
		InvalidItem = 1,
		// Token: 0x04002530 RID: 9520
		AlreadyReloading = 2,
		// Token: 0x04002531 RID: 9521
		PendingInventoryActions = 4,
		// Token: 0x04002532 RID: 9522
		Jammed = 8,
		// Token: 0x04002533 RID: 9523
		OutOfAmmo = 16,
		// Token: 0x04002534 RID: 9524
		EmptyClip = 32,
		// Token: 0x04002535 RID: 9525
		PartialClip = 64,
		// Token: 0x04002536 RID: 9526
		FullClip = 128,
		// Token: 0x04002537 RID: 9527
		CompletelyOutOfAmmo = 48
	}
}
