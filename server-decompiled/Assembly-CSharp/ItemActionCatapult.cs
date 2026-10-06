using System;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200059B RID: 1435
[Preserve]
public class ItemActionCatapult : ItemActionLauncher
{
	// Token: 0x06002F54 RID: 12116 RVA: 0x0012B284 File Offset: 0x00129484
	public ItemActionCatapult()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.35f));
		texture2D.Apply();
		this.progressBarStyle = new GUIStyle();
		this.progressBarStyle.normal.background = texture2D;
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x0012B2E4 File Offset: 0x001294E4
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

	// Token: 0x06002F56 RID: 12118 RVA: 0x0012B348 File Offset: 0x00129548
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

	// Token: 0x06002F57 RID: 12119 RVA: 0x0012B432 File Offset: 0x00129632
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCatapult.ItemActionDataCatapult(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x0012B43C File Offset: 0x0012963C
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

	// Token: 0x06002F59 RID: 12121 RVA: 0x0012B4A8 File Offset: 0x001296A8
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
				GameManager.Instance.ItemReloadServer(itemActionDataCatapult.invData.holdingEntity.entityId);
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

	// Token: 0x06002F5A RID: 12122 RVA: 0x0012B680 File Offset: 0x00129880
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

	// Token: 0x06002F5B RID: 12123 RVA: 0x0012B6C0 File Offset: 0x001298C0
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

	// Token: 0x06002F5C RID: 12124 RVA: 0x0012B714 File Offset: 0x00129914
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
			if (itemActionDataCatapult.invData == itemActionDataCatapult.invData.holdingEntity.inventory.holdingItemData)
			{
				_actionData.invData.holdingEntity.FireEvent(MinEventTypes.onSelfPrimaryActionEnd, true);
			}
		}
	}

	// Token: 0x06002F5D RID: 12125 RVA: 0x0012B7D0 File Offset: 0x001299D0
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

	// Token: 0x06002F5E RID: 12126 RVA: 0x0012B828 File Offset: 0x00129A28
	public float GetStrainPercent(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = _actionData as ItemActionLauncher.ItemActionDataLauncher;
		if (itemActionDataLauncher != null)
		{
			return itemActionDataLauncher.lastAttackStrainPercent;
		}
		return 0f;
	}

	// Token: 0x06002F5F RID: 12127 RVA: 0x0012B84C File Offset: 0x00129A4C
	public override bool CanReload(ItemActionData _actionData)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		if (itemActionDataCatapult != null && itemActionDataCatapult.m_bActivated)
		{
			this.CancelAction(_actionData);
		}
		return base.CanReload(_actionData);
	}

	// Token: 0x04002479 RID: 9337
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIStyle progressBarStyle;

	// Token: 0x0400247A RID: 9338
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundDraw;

	// Token: 0x0400247B RID: 9339
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundCancel;

	// Token: 0x0200059C RID: 1436
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataCatapult : ItemActionLauncher.ItemActionDataLauncher
	{
		// Token: 0x06002F60 RID: 12128 RVA: 0x0012B879 File Offset: 0x00129A79
		public ItemActionDataCatapult(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400247C RID: 9340
		public bool m_bActivated;

		// Token: 0x0400247D RID: 9341
		public bool m_bCanceled;

		// Token: 0x0400247E RID: 9342
		public float m_ActivateTime;

		// Token: 0x0400247F RID: 9343
		public float m_MaxStrainTime;
	}
}
