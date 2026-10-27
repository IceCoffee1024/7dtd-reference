using System;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005A2 RID: 1442
[Preserve]
public class ItemActionCatapult : ItemActionLauncher
{
	// Token: 0x06002F62 RID: 12130 RVA: 0x0012BE78 File Offset: 0x0012A078
	public ItemActionCatapult()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.35f));
		texture2D.Apply();
		this.progressBarStyle = new GUIStyle();
		this.progressBarStyle.normal.background = texture2D;
	}

	// Token: 0x06002F63 RID: 12131 RVA: 0x0012BED8 File Offset: 0x0012A0D8
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

	// Token: 0x06002F64 RID: 12132 RVA: 0x0012BF3C File Offset: 0x0012A13C
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

	// Token: 0x06002F65 RID: 12133 RVA: 0x0012C026 File Offset: 0x0012A226
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionCatapult.ItemActionDataCatapult(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F66 RID: 12134 RVA: 0x0012C030 File Offset: 0x0012A230
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

	// Token: 0x06002F67 RID: 12135 RVA: 0x0012C09C File Offset: 0x0012A29C
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

	// Token: 0x06002F68 RID: 12136 RVA: 0x0012C274 File Offset: 0x0012A474
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

	// Token: 0x06002F69 RID: 12137 RVA: 0x0012C2B4 File Offset: 0x0012A4B4
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

	// Token: 0x06002F6A RID: 12138 RVA: 0x0012C308 File Offset: 0x0012A508
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

	// Token: 0x06002F6B RID: 12139 RVA: 0x0012C3C4 File Offset: 0x0012A5C4
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

	// Token: 0x06002F6C RID: 12140 RVA: 0x0012C41C File Offset: 0x0012A61C
	public float GetStrainPercent(ItemActionData _actionData)
	{
		ItemActionLauncher.ItemActionDataLauncher itemActionDataLauncher = _actionData as ItemActionLauncher.ItemActionDataLauncher;
		if (itemActionDataLauncher != null)
		{
			return itemActionDataLauncher.lastAttackStrainPercent;
		}
		return 0f;
	}

	// Token: 0x06002F6D RID: 12141 RVA: 0x0012C440 File Offset: 0x0012A640
	public override bool CanReload(ItemActionData _actionData)
	{
		ItemActionCatapult.ItemActionDataCatapult itemActionDataCatapult = (ItemActionCatapult.ItemActionDataCatapult)_actionData;
		if (itemActionDataCatapult != null && itemActionDataCatapult.m_bActivated)
		{
			this.CancelAction(_actionData);
		}
		return base.CanReload(_actionData);
	}

	// Token: 0x04002494 RID: 9364
	[PublicizedFrom(EAccessModifier.Private)]
	public GUIStyle progressBarStyle;

	// Token: 0x04002495 RID: 9365
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundDraw;

	// Token: 0x04002496 RID: 9366
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundCancel;

	// Token: 0x020005A3 RID: 1443
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataCatapult : ItemActionLauncher.ItemActionDataLauncher
	{
		// Token: 0x06002F6E RID: 12142 RVA: 0x0012C46D File Offset: 0x0012A66D
		public ItemActionDataCatapult(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x04002497 RID: 9367
		public bool m_bActivated;

		// Token: 0x04002498 RID: 9368
		public bool m_bCanceled;

		// Token: 0x04002499 RID: 9369
		public float m_ActivateTime;

		// Token: 0x0400249A RID: 9370
		public float m_MaxStrainTime;
	}
}
