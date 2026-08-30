using System;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000595 RID: 1429
[Preserve]
public class ItemActionCatapult : ItemActionLauncher
{
	// Token: 0x06002EE4 RID: 12004 RVA: 0x001296F4 File Offset: 0x001278F4
	public ItemActionCatapult()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.35f));
		texture2D.Apply();
		this.progressBarStyle = new GUIStyle();
		this.progressBarStyle.normal.background = texture2D;
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x00129754 File Offset: 0x00127954
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Sound_draw"))
		{
			this.soundDraw = _props.Values["Sound_draw"];
		}
		if (_props.Values.ContainsKey("Sound_cancel"))
		{
			this.soundCancel = _props.Values["Sound_cancel"];
		}
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x001297B8 File Offset: 0x001279B8
	public override void OnModificationsChanged(ItemActionData _data)
	{
		base.OnModificationsChanged(_data);
		if (this.Properties.Values.ContainsKey("Max_strain_time"))
		{
			((ItemActionCatapult.ItemActionDataCatapult)_data).m_MaxStrainTime = StringParsers.ParseFloat(_data.invData.itemValue.GetPropertyOverride("Max_strain_time", this.Properties.Values["Max_strain_time"]), 0, -1, NumberStyles.Any);
		}
		else
		{
			((ItemActionCatapult.ItemActionDataCatapult)_data).m_MaxStrainTime = StringParsers.ParseFloat(_data.invData.itemValue.GetPropertyOverride("Max_strain_time", "2"), 0, -1, NumberStyles.Any);
		}
		((ItemActionCatapult.ItemActionDataCatapult)_data).m_MaxStrainTime = 60f / EffectManager.GetValue(PassiveEffects.RoundsPerMinute, _data.invData.itemValue, ((ItemActionCatapult.ItemActionDataCatapult)_data).m_MaxStrainTime, _data.invData.holdingEntity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x001298A2 File Offset: 0x00127AA2
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCatapult.ItemActionDataCatapult(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x001298AC File Offset: 0x00127AAC
	public override void OnScreenOverlay(ItemActionData _actionData)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		EntityPlayerLocal entityPlayerLocal = itemActionDataCatapult.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			LocalPlayerUI playerUI = entityPlayerLocal.PlayerUI;
			float num = (Time.time - itemActionDataCatapult.m_ActivateTime) / itemActionDataCatapult.m_MaxStrainTime;
			if (itemActionDataCatapult.m_bActivated)
			{
				XUiC_ThrowPower.Status(playerUI, Mathf.Clamp01(num));
				return;
			}
			XUiC_ThrowPower.Status(playerUI, -1f);
		}
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x00129918 File Offset: 0x00127B18
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		EntityPlayerLocal entityPlayerLocal = itemActionDataCatapult.invData.holdingEntity as EntityPlayerLocal;
		if (_bReleased)
		{
			itemActionDataCatapult.m_bCanceled = false;
			itemActionDataCatapult.invData.holdingEntity.SpecialAttack = false;
		}
		if (ItemActionRanged.Reloading(itemActionDataCatapult))
		{
			itemActionDataCatapult.m_LastShotTime = Time.time;
			return;
		}
		if (Time.time - itemActionDataCatapult.m_LastShotTime < itemActionDataCatapult.Delay)
		{
			return;
		}
		if (!this.InfiniteAmmo && itemActionDataCatapult.invData.itemValue.Meta == 0)
		{
			if (this.AutoReload && this.CanReload(itemActionDataCatapult))
			{
				itemActionDataCatapult.invData.gameManager.ItemReloadServer(itemActionDataCatapult.invData.holdingEntity.entityId);
				itemActionDataCatapult.invData.holdingEntitySoundID = -2;
			}
			return;
		}
		if (!_bReleased)
		{
			if (!itemActionDataCatapult.m_bActivated)
			{
				itemActionDataCatapult.m_bActivated = true;
				itemActionDataCatapult.m_ActivateTime = Time.time;
				itemActionDataCatapult.invData.holdingEntity.SpecialAttack = true;
				if (this.soundDraw != null)
				{
					_actionData.invData.holdingEntity.PlayOneShot(this.soundDraw, false, false, false, null, 1f);
				}
				if (entityPlayerLocal != null)
				{
					entityPlayerLocal.StartTPCameraLockTimer();
				}
			}
			return;
		}
		if (!itemActionDataCatapult.m_bActivated)
		{
			return;
		}
		itemActionDataCatapult.strainPercent = (Time.time - itemActionDataCatapult.m_ActivateTime) / itemActionDataCatapult.m_MaxStrainTime;
		if ((itemActionDataCatapult.invData.itemValue.MaxUseTimes > 0 && itemActionDataCatapult.invData.itemValue.UseTimes >= (float)itemActionDataCatapult.invData.itemValue.MaxUseTimes) || (itemActionDataCatapult.invData.itemValue.UseTimes == 0f && itemActionDataCatapult.invData.itemValue.MaxUseTimes == 0) || (entityPlayerLocal != null && !entityPlayerLocal.TPCameraCheckPassed))
		{
			this.CancelAction(_actionData);
			itemActionDataCatapult.m_bCanceled = false;
		}
		itemActionDataCatapult.m_bActivated = false;
		base.ExecuteAction(_actionData, false);
		base.ExecuteAction(_actionData, true);
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x00129AF8 File Offset: 0x00127CF8
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		base.OnHoldingUpdate(_actionData);
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		if (itemActionDataCatapult.m_bActivated)
		{
			EntityPlayerLocal entityPlayerLocal = itemActionDataCatapult.invData.holdingEntity as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.StartTPCameraLockTimer();
			}
		}
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x00129B38 File Offset: 0x00127D38
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		this.CancelAction(_data);
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_data;
		itemActionDataCatapult.m_bCanceled = false;
		EntityPlayerLocal entityPlayerLocal = itemActionDataCatapult.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			XUiC_ThrowPower.Status(entityPlayerLocal.PlayerUI, -1f);
		}
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x00129B8C File Offset: 0x00127D8C
	public override void CancelAction(ItemActionData _actionData)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		if (itemActionDataCatapult.m_bActivated)
		{
			itemActionDataCatapult.m_bActivated = false;
			itemActionDataCatapult.m_bCanceled = true;
			itemActionDataCatapult.bReleased = false;
			itemActionDataCatapult.invData.holdingEntity.SpecialAttack = false;
			itemActionDataCatapult.invData.holdingEntity.SpecialAttack2 = true;
			if (this.soundCancel != null)
			{
				_actionData.invData.holdingEntity.PlayOneShot(this.soundCancel, false, false, false, null, 1f);
			}
			base.triggerReleased(itemActionDataCatapult, _actionData.indexInEntityOfAction);
			if (itemActionDataCatapult.invData.slotIdx == itemActionDataCatapult.invData.holdingEntity.inventory.holdingItemIdx && itemActionDataCatapult.invData.item == itemActionDataCatapult.invData.holdingEntity.inventory.holdingItem)
			{
				_actionData.invData.holdingEntity.FireEvent(MinEventTypes.onSelfPrimaryActionEnd, true);
			}
		}
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x00129C70 File Offset: 0x00127E70
	public override void ReloadGun(ItemActionData _actionData)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		if (itemActionDataCatapult != null)
		{
			itemActionDataCatapult.isReloadRequested = false;
			Manager.StopSequence(itemActionDataCatapult.invData.holdingEntity, itemActionDataCatapult.SoundStart);
			if (!itemActionDataCatapult.invData.holdingEntity.isEntityRemote)
			{
				itemActionDataCatapult.invData.holdingEntity.OnReloadStart();
			}
		}
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x00129CC8 File Offset: 0x00127EC8
	public float GetStrainPercent(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = _actionData as ItemActionLauncher.ItemActionDataLauncher;
		if (itemActionDataLauncher != null)
		{
			return itemActionDataLauncher.lastAttackStrainPercent;
		}
		return 0f;
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x00129CEC File Offset: 0x00127EEC
	public override bool CanReload(ItemActionData _actionData)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		if (itemActionDataCatapult != null && itemActionDataCatapult.m_bActivated)
		{
			this.CancelAction(_actionData);
		}
		return base.CanReload(_actionData);
	}

	// Token: 0x0400244C RID: 9292
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIStyle progressBarStyle;

	// Token: 0x0400244D RID: 9293
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundDraw;

	// Token: 0x0400244E RID: 9294
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundCancel;

	// Token: 0x02000596 RID: 1430
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataCatapult : ItemActionLauncher.ItemActionDataLauncher
	{
		// Token: 0x06002EF0 RID: 12016 RVA: 0x00129D19 File Offset: 0x00127F19
		public ItemActionDataCatapult(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400244F RID: 9295
		public bool m_bActivated;

		// Token: 0x04002450 RID: 9296
		public bool m_bCanceled;

		// Token: 0x04002451 RID: 9297
		public float m_ActivateTime;

		// Token: 0x04002452 RID: 9298
		public float m_MaxStrainTime;
	}
}
