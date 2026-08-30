using System;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200059A RID: 1434
[Preserve]
public class ItemActionCatapult : ItemActionLauncher
{
	// Token: 0x06002F12 RID: 12050 RVA: 0x0012A764 File Offset: 0x00128964
	public ItemActionCatapult()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.35f));
		texture2D.Apply();
		this.progressBarStyle = new GUIStyle();
		this.progressBarStyle.normal.background = texture2D;
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x0012A7C4 File Offset: 0x001289C4
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

	// Token: 0x06002F14 RID: 12052 RVA: 0x0012A828 File Offset: 0x00128A28
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

	// Token: 0x06002F15 RID: 12053 RVA: 0x0012A912 File Offset: 0x00128B12
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCatapult.ItemActionDataCatapult(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x0012A91C File Offset: 0x00128B1C
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

	// Token: 0x06002F17 RID: 12055 RVA: 0x0012A988 File Offset: 0x00128B88
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

	// Token: 0x06002F18 RID: 12056 RVA: 0x0012AB68 File Offset: 0x00128D68
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

	// Token: 0x06002F19 RID: 12057 RVA: 0x0012ABA8 File Offset: 0x00128DA8
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

	// Token: 0x06002F1A RID: 12058 RVA: 0x0012ABFC File Offset: 0x00128DFC
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

	// Token: 0x06002F1B RID: 12059 RVA: 0x0012ACE0 File Offset: 0x00128EE0
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

	// Token: 0x06002F1C RID: 12060 RVA: 0x0012AD38 File Offset: 0x00128F38
	public float GetStrainPercent(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = _actionData as ItemActionLauncher.ItemActionDataLauncher;
		if (itemActionDataLauncher != null)
		{
			return itemActionDataLauncher.lastAttackStrainPercent;
		}
		return 0f;
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x0012AD5C File Offset: 0x00128F5C
	public override bool CanReload(ItemActionData _actionData)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		if (itemActionDataCatapult != null && itemActionDataCatapult.m_bActivated)
		{
			this.CancelAction(_actionData);
		}
		return base.CanReload(_actionData);
	}

	// Token: 0x04002480 RID: 9344
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIStyle progressBarStyle;

	// Token: 0x04002481 RID: 9345
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundDraw;

	// Token: 0x04002482 RID: 9346
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundCancel;

	// Token: 0x0200059B RID: 1435
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataCatapult : ItemActionLauncher.ItemActionDataLauncher
	{
		// Token: 0x06002F1E RID: 12062 RVA: 0x0012AD89 File Offset: 0x00128F89
		public ItemActionDataCatapult(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002483 RID: 9347
		public bool m_bActivated;

		// Token: 0x04002484 RID: 9348
		public bool m_bCanceled;

		// Token: 0x04002485 RID: 9349
		public float m_ActivateTime;

		// Token: 0x04002486 RID: 9350
		public float m_MaxStrainTime;
	}
}
