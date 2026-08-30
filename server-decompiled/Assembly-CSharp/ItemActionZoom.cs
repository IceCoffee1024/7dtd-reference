using System;
using System.Collections;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E7 RID: 1511
[global::UnityEngine.Scripting.Preserve]
public class ItemActionZoom : ItemAction
{
	// Token: 0x0600311A RID: 12570 RVA: 0x0013ED8C File Offset: 0x0013CF8C
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("zoomTriggerEffectPullDualsense"))
		{
			this.zoomTriggerEffectPullDualsense = _props.Values["zoomTriggerEffectPullDualsense"];
		}
		else
		{
			this.zoomTriggerEffectPullDualsense = string.Empty;
		}
		if (_props.Values.ContainsKey("zoomTriggerEffectPullXb"))
		{
			this.zoomTriggerEffectPullXb = _props.Values["zoomTriggerEffectPullXb"];
			return;
		}
		this.zoomTriggerEffectPullXb = string.Empty;
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x0013EE09 File Offset: 0x0013D009
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionZoom.ItemActionDataZoom(_invData, _indexInEntityOfAction);
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x0013EE14 File Offset: 0x0013D014
	public override void OnModificationsChanged(ItemActionData _data)
	{
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_data;
		if (this.Properties != null && this.Properties.Values.ContainsKey("Zoom_overlay"))
		{
			itemActionDataZoom.ZoomOverlayName = _data.invData.itemValue.GetPropertyOverride("Zoom_overlay", this.Properties.Values["Zoom_overlay"]);
		}
		else
		{
			itemActionDataZoom.ZoomOverlayName = _data.invData.itemValue.GetPropertyOverride("Zoom_overlay", "");
		}
		if (itemActionDataZoom.ZoomOverlayName != "")
		{
			itemActionDataZoom.ZoomOverlay = DataLoader.LoadAsset<Texture2D>(itemActionDataZoom.ZoomOverlayName, false);
		}
		if (itemActionDataZoom.invData.holdingEntity as EntityPlayerLocal != null)
		{
			itemActionDataZoom.BaseFOV = (int)(itemActionDataZoom.invData.holdingEntity as EntityPlayerLocal).playerCamera.fieldOfView;
			itemActionDataZoom.MaxZoomOut = itemActionDataZoom.BaseFOV;
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("Zoom_max_out"))
		{
			itemActionDataZoom.MaxZoomOut = StringParsers.ParseSInt32(_data.invData.itemValue.GetPropertyOverride("Zoom_max_out", this.Properties.Values["Zoom_max_out"]), 0, -1, NumberStyles.Integer);
		}
		else
		{
			itemActionDataZoom.MaxZoomOut = StringParsers.ParseSInt32(_data.invData.itemValue.GetPropertyOverride("Zoom_max_out", itemActionDataZoom.MaxZoomOut.ToString()), 0, -1, NumberStyles.Integer);
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("Zoom_max_in"))
		{
			itemActionDataZoom.MaxZoomIn = StringParsers.ParseSInt32(_data.invData.itemValue.GetPropertyOverride("Zoom_max_in", this.Properties.Values["Zoom_max_in"]), 0, -1, NumberStyles.Integer);
		}
		else
		{
			itemActionDataZoom.MaxZoomIn = StringParsers.ParseSInt32(_data.invData.itemValue.GetPropertyOverride("Zoom_max_in", itemActionDataZoom.MaxZoomOut.ToString()), 0, -1, NumberStyles.Integer);
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("SightsCameraOffset"))
		{
			itemActionDataZoom.SightsCameraOffset = StringParsers.ParseVector3(itemActionDataZoom.invData.itemValue.GetPropertyOverride("SightsCameraOffset", this.Properties.Values["SightsCameraOffset"]), 0, -1);
		}
		else
		{
			itemActionDataZoom.SightsCameraOffset = StringParsers.ParseVector3(itemActionDataZoom.invData.itemValue.GetPropertyOverride("SightsCameraOffset", "0,0,0"), 0, -1);
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("ScopeCameraOffset"))
		{
			itemActionDataZoom.ScopeCameraOffset = StringParsers.ParseVector3(itemActionDataZoom.invData.itemValue.GetPropertyOverride("ScopeCameraOffset", this.Properties.Values["ScopeCameraOffset"]), 0, -1);
		}
		else
		{
			itemActionDataZoom.ScopeCameraOffset = StringParsers.ParseVector3(itemActionDataZoom.invData.itemValue.GetPropertyOverride("ScopeCameraOffset", "0,0,0"), 0, -1);
		}
		itemActionDataZoom.CurrentZoom = (float)itemActionDataZoom.MaxZoomOut;
		if (itemActionDataZoom.invData.model != null && itemActionDataZoom.Scope != null)
		{
			itemActionDataZoom.HasScope = itemActionDataZoom.Scope.childCount > 0;
		}
		else if (itemActionDataZoom.invData.model != null)
		{
			itemActionDataZoom.Scope = itemActionDataZoom.invData.model.FindInChilds("Attachments", false);
			itemActionDataZoom.Scope = itemActionDataZoom.Scope.Find("Scope");
			itemActionDataZoom.HasScope = itemActionDataZoom.Scope.childCount > 0;
		}
		if (!itemActionDataZoom.HasScope)
		{
			foreach (ItemValue itemValue in itemActionDataZoom.invData.itemValue.Modifications)
			{
				ItemClassModifier itemClassModifier = ((itemValue != null) ? itemValue.ItemClass : null) as ItemClassModifier;
				if (itemClassModifier != null && itemClassModifier.ModifierTags.Test_AllSet(FastTags<TagGroup.Global>.Parse("scope")))
				{
					itemActionDataZoom.HasScope = true;
					break;
				}
			}
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("zoomTriggerEffectPullDualsense"))
		{
			this.zoomTriggerEffectPullDualsense = _data.invData.itemValue.GetPropertyOverride("zoomTriggerEffectPullDualsense", "NoEffect");
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("zoomTriggerEffectPullXb"))
		{
			this.zoomTriggerEffectPullXb = _data.invData.itemValue.GetPropertyOverride("zoomTriggerEffectPullXb", "NoEffect");
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("zoomTriggerEffectShootDualsense"))
		{
			this.zoomTriggerEffectShootDualsense = _data.invData.itemValue.GetPropertyOverride("zoomTriggerEffectShootDualsense", "NoEffect");
		}
		if (this.Properties != null && this.Properties.Values.ContainsKey("zoomTriggerEffectShootXb"))
		{
			this.zoomTriggerEffectShootXb = _data.invData.itemValue.GetPropertyOverride("zoomTriggerEffectShootXb", "NoEffect");
		}
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x0013F308 File Offset: 0x0013D508
	public override void StartHolding(ItemActionData _data)
	{
		base.StartHolding(_data);
		if (_data.invData.holdingEntity as EntityPlayerLocal != null)
		{
			GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.LeftTrigger, TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.zoomTriggerEffectPullDualsense, this.zoomTriggerEffectPullXb)), false);
		}
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x0013F35C File Offset: 0x0013D55C
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		EntityPlayerLocal entityPlayerLocal = _data.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.LeftTrigger, TriggerEffectManager.NoneEffect, false);
			ItemActionZoom.ItemActionDataZoom itemActionDataZoom = _data as ItemActionZoom.ItemActionDataZoom;
			if (itemActionDataZoom != null && itemActionDataZoom.invData.holdingEntity.AimingGun)
			{
				entityPlayerLocal.cameraTransform.GetComponent<Camera>().fieldOfView = (float)itemActionDataZoom.BaseFOV;
			}
		}
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x0013F3D4 File Offset: 0x0013D5D4
	public override void OnScreenOverlay(ItemActionData _actionData)
	{
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		EntityPlayerLocal entityPlayerLocal = itemActionDataZoom.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			if (itemActionDataZoom.ZoomOverlay != null && !itemActionDataZoom.bZoomInProgress && _actionData.invData.holdingEntity.AimingGun)
			{
				if (itemActionDataZoom.Scope != null && entityPlayerLocal.playerCamera)
				{
					entityPlayerLocal.playerCamera.cullingMask = entityPlayerLocal.playerCamera.cullingMask & -1025;
					if (itemActionDataZoom.invData.holdingEntity.GetModelLayer() != 10)
					{
						itemActionDataZoom.layerBeforeSwitch = itemActionDataZoom.invData.holdingEntity.GetModelLayer();
						itemActionDataZoom.invData.holdingEntity.SetModelLayer(10, false, Utils.ExcludeLayerZoom);
						return;
					}
				}
				float num = (float)itemActionDataZoom.ZoomOverlay.width;
				float num2 = (float)Screen.height * 0.95f;
				num *= num2 / (float)itemActionDataZoom.ZoomOverlay.height;
				int num3 = (int)(((float)Screen.width - num) / 2f);
				int num4 = (int)(((float)Screen.height - num2) / 2f);
				GUIUtils.DrawFilledRect(new Rect(0f, 0f, (float)Screen.width, (float)num4), Color.black, false, Color.black);
				GUIUtils.DrawFilledRect(new Rect(0f, 0f, (float)num3, (float)Screen.height), Color.black, false, Color.black);
				GUIUtils.DrawFilledRect(new Rect((float)num3 + num, 0f, (float)Screen.width, (float)num4 + num2), Color.black, false, Color.black);
				GUIUtils.DrawFilledRect(new Rect(0f, (float)num4 + num2, (float)Screen.width, (float)Screen.height), Color.black, false, Color.black);
				Graphics.DrawTexture(new Rect((float)num3, (float)num4, num, num2), itemActionDataZoom.ZoomOverlay);
				if (entityPlayerLocal != null && !entityPlayerLocal.bFirstPersonView && entityPlayerLocal.emodel.visible)
				{
					entityPlayerLocal.emodel.SetVisible(false, true);
					return;
				}
			}
			else if (!entityPlayerLocal.bFirstPersonView && !entityPlayerLocal.emodel.visible)
			{
				entityPlayerLocal.emodel.SetVisible(true, true);
			}
		}
	}

	// Token: 0x06003120 RID: 12576 RVA: 0x0013F608 File Offset: 0x0013D808
	public override bool ConsumeScrollWheel(ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		if (!_actionData.invData.holdingEntity.AimingGun)
		{
			return false;
		}
		if (_scrollWheelInput == 0f)
		{
			return false;
		}
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		if (!itemActionDataZoom.bZoomInProgress)
		{
			itemActionDataZoom.CurrentZoom = Utils.FastClamp(itemActionDataZoom.CurrentZoom + _scrollWheelInput * -25f, (float)itemActionDataZoom.MaxZoomIn, (float)itemActionDataZoom.MaxZoomOut);
			((EntityPlayerLocal)_actionData.invData.holdingEntity).cameraTransform.GetComponent<Camera>().fieldOfView = (float)((int)itemActionDataZoom.CurrentZoom);
		}
		return true;
	}

	// Token: 0x06003121 RID: 12577 RVA: 0x0013F694 File Offset: 0x0013D894
	public override bool ConsumeCameraFunction(ItemActionData _actionData)
	{
		if (!_actionData.invData.holdingEntity.AimingGun)
		{
			return false;
		}
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		if (itemActionDataZoom.MaxZoomIn == itemActionDataZoom.MaxZoomOut)
		{
			return false;
		}
		if (!itemActionDataZoom.bZoomInProgress)
		{
			if (itemActionDataZoom.CurrentZoom == (float)itemActionDataZoom.MaxZoomIn)
			{
				itemActionDataZoom.CurrentZoom = (float)itemActionDataZoom.MaxZoomOut;
			}
			else
			{
				itemActionDataZoom.CurrentZoom = Utils.FastClamp(itemActionDataZoom.CurrentZoom - 5f, (float)itemActionDataZoom.MaxZoomIn, (float)itemActionDataZoom.MaxZoomOut);
			}
			((EntityPlayerLocal)_actionData.invData.holdingEntity).cameraTransform.GetComponent<Camera>().fieldOfView = (float)((int)itemActionDataZoom.CurrentZoom);
			return true;
		}
		return false;
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x0013F744 File Offset: 0x0013D944
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		bool flag = !_bReleased && itemActionDataZoom.invData.holdingEntity.IsAimingGunPossible();
		EntityPlayerLocal entityPlayerLocal = itemActionDataZoom.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal)
		{
			if (!entityPlayerLocal.IsCameraAttachedToPlayerOrScope() && entityPlayerLocal.bFirstPersonView)
			{
				return;
			}
			if (entityPlayerLocal.movementInput.running && !_bReleased)
			{
				entityPlayerLocal.MoveController.ForceStopRunning();
			}
			bool flag2 = ((entityPlayerLocal.playerInput.LastDeviceClass == InputDeviceClass.Controller) ? GamePrefs.GetBool(EnumGamePrefs.OptionsControllerWeaponAiming) : GamePrefs.GetBool(EnumGamePrefs.OptionsWeaponAiming));
			if (_bReleased && flag2 && ((itemActionDataZoom.aimingCoroutine != null && itemActionDataZoom.aimingValue) || entityPlayerLocal.bLerpCameraFlag))
			{
				return;
			}
		}
		if (itemActionDataZoom.aimingCoroutine != null)
		{
			GameManager.Instance.StopCoroutine(itemActionDataZoom.aimingCoroutine);
			itemActionDataZoom.aimingCoroutine = null;
		}
		if (itemActionDataZoom.invData.holdingEntity.AimingGun == flag)
		{
			return;
		}
		itemActionDataZoom.aimingValue = flag;
		itemActionDataZoom.aimingCoroutine = GameManager.Instance.StartCoroutine(this.startEndZoomLater(itemActionDataZoom));
		if (!_bReleased && entityPlayerLocal && entityPlayerLocal.movementInput.lastInputController)
		{
			entityPlayerLocal.MoveController.FindCameraSnapTarget(eCameraSnapMode.Zoom, 50f);
		}
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x0013F871 File Offset: 0x0013DA71
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startEndZoomLater(ItemActionZoom.ItemActionDataZoom _actionData)
	{
		yield return new WaitForSecondsRealtime(0f);
		_actionData.invData.holdingEntity.AimingGun = _actionData.aimingValue;
		yield break;
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x0013F880 File Offset: 0x0013DA80
	public override void AimingSet(ItemActionData _actionData, bool _isAiming, bool _wasAiming)
	{
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		if (itemActionDataZoom.aimingCoroutine != null)
		{
			GameManager.Instance.StopCoroutine(itemActionDataZoom.aimingCoroutine);
			itemActionDataZoom.aimingCoroutine = null;
		}
		if (_isAiming != _wasAiming)
		{
			this.startEndZoom(itemActionDataZoom, _isAiming);
		}
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x0013F8C0 File Offset: 0x0013DAC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void startEndZoom(ItemActionZoom.ItemActionDataZoom _actionData, bool _isAiming)
	{
		if (_isAiming)
		{
			if (!_actionData.bZoomInProgress && !string.IsNullOrEmpty(_actionData.invData.item.soundSightIn))
			{
				Manager.BroadcastPlay(_actionData.invData.item.soundSightIn);
			}
			_actionData.timeZoomStarted = Time.time;
			_actionData.bZoomInProgress = true;
			return;
		}
		if (_actionData.layerBeforeSwitch != -1)
		{
			_actionData.invData.holdingEntity.SetModelLayer(_actionData.layerBeforeSwitch, false, null);
			_actionData.layerBeforeSwitch = -1;
		}
		EntityPlayerLocal entityPlayerLocal = (EntityPlayerLocal)_actionData.invData.holdingEntity;
		if (_actionData.Scope != null && entityPlayerLocal.playerCamera)
		{
			entityPlayerLocal.playerCamera.cullingMask = entityPlayerLocal.playerCamera.cullingMask | 1024;
		}
		if (!_actionData.bZoomInProgress && !string.IsNullOrEmpty(_actionData.invData.item.soundSightOut))
		{
			Manager.BroadcastPlay(_actionData.invData.item.soundSightOut);
		}
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x0013F9BC File Offset: 0x0013DBBC
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		return itemActionDataZoom.bZoomInProgress && Time.time - itemActionDataZoom.timeZoomStarted < 0.3f;
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x0013F9F0 File Offset: 0x0013DBF0
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		EntityPlayerLocal entityPlayerLocal = itemActionDataZoom.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			bool flag = ((itemActionDataZoom.aimingCoroutine != null) ? itemActionDataZoom.aimingValue : entityPlayerLocal.AimingGun);
			if (!entityPlayerLocal.movementInput.running && !flag && !entityPlayerLocal.bLerpCameraFlag)
			{
				itemActionDataZoom.HasExecuted = false;
			}
			vp_FPWeapon vp_FPWeapon = entityPlayerLocal.vp_FPWeapon;
			if (vp_FPWeapon != null)
			{
				if (_actionData.invData.holdingEntity.AimingGun)
				{
					if (itemActionDataZoom.HasScope)
					{
						vp_FPWeapon.AimingPositionOffset = itemActionDataZoom.ScopeCameraOffset;
					}
					else
					{
						vp_FPWeapon.AimingPositionOffset = itemActionDataZoom.SightsCameraOffset;
					}
					vp_FPWeapon.RenderingFieldOfView = (float)StringParsers.ParseSInt32(_actionData.invData.itemValue.GetPropertyOverride("WeaponCameraFOV", vp_FPWeapon.originalRenderingFieldOfView.ToCultureInvariantString()), 0, -1, NumberStyles.Integer);
				}
				else
				{
					vp_FPWeapon.AimingPositionOffset = Vector3.zero;
					vp_FPWeapon.RenderingFieldOfView = vp_FPWeapon.originalRenderingFieldOfView;
				}
				vp_FPWeapon.Refresh();
			}
		}
		if (!itemActionDataZoom.bZoomInProgress || Time.time - itemActionDataZoom.timeZoomStarted < 0.15f)
		{
			return;
		}
		itemActionDataZoom.bZoomInProgress = false;
		if (_actionData.invData.holdingEntity.AimingGun && entityPlayerLocal)
		{
			entityPlayerLocal.cameraTransform.GetComponent<Camera>().fieldOfView = (float)((int)itemActionDataZoom.CurrentZoom);
		}
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x0013FB44 File Offset: 0x0013DD44
	public override bool IsHUDDisabled(ItemActionData _data)
	{
		return base.ZoomOverlay != null && !_data.invData.holdingEntity.isEntityRemote && _data.invData.holdingEntity.AimingGun && !((ItemActionZoom.ItemActionDataZoom)_data).bZoomInProgress;
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x0013FB93 File Offset: 0x0013DD93
	public override void GetIronSights(ItemActionData _actionData, out float _fov)
	{
		_fov = ((base.ZoomOverlay == null) ? ((ItemActionZoom.ItemActionDataZoom)_actionData).CurrentZoom : 0f);
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x0013FBB7 File Offset: 0x0013DDB7
	public override EnumCameraShake GetCameraShakeType(ItemActionData _actionData)
	{
		if (base.ZoomOverlay != null && _actionData.invData.holdingEntity.AimingGun)
		{
			return EnumCameraShake.Big;
		}
		if (_actionData.invData.holdingEntity.AimingGun)
		{
			return EnumCameraShake.Tiny;
		}
		return EnumCameraShake.Small;
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x0013FBF0 File Offset: 0x0013DDF0
	public override TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		return TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.zoomTriggerEffectPullDualsense, this.zoomTriggerEffectPullXb));
	}

	// Token: 0x0600312C RID: 12588 RVA: 0x0013FC08 File Offset: 0x0013DE08
	public override TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		return TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.zoomTriggerEffectShootDualsense, this.zoomTriggerEffectShootXb));
	}

	// Token: 0x0600312D RID: 12589 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowConcurrentActions()
	{
		return true;
	}

	// Token: 0x04002620 RID: 9760
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectPullDualsense;

	// Token: 0x04002621 RID: 9761
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectShootDualsense;

	// Token: 0x04002622 RID: 9762
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectPullXb;

	// Token: 0x04002623 RID: 9763
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectShootXb;

	// Token: 0x020005E8 RID: 1512
	public class ItemActionDataZoom : ItemActionData
	{
		// Token: 0x0600312F RID: 12591 RVA: 0x0013FC20 File Offset: 0x0013DE20
		public ItemActionDataZoom(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
			if (_invData.model != null)
			{
				this.Scope = _invData.model.FindInChilds("Attachments", false);
				if (this.Scope == null)
				{
					Log.Error("Transform 'Attachments' not found in weapon prefab for {0}.", new object[] { _invData.model.name });
				}
				else
				{
					this.Scope = this.Scope.Find("Scope");
					this.HasScope = this.Scope.childCount > 0;
				}
			}
			this.layerBeforeSwitch = -1;
		}

		// Token: 0x04002624 RID: 9764
		public float CurrentZoom;

		// Token: 0x04002625 RID: 9765
		public Transform Scope;

		// Token: 0x04002626 RID: 9766
		public bool bZoomInProgress;

		// Token: 0x04002627 RID: 9767
		public float timeZoomStarted;

		// Token: 0x04002628 RID: 9768
		public int layerBeforeSwitch;

		// Token: 0x04002629 RID: 9769
		public bool HasScope;

		// Token: 0x0400262A RID: 9770
		public Vector3 SightsCameraOffset;

		// Token: 0x0400262B RID: 9771
		public Vector3 ScopeCameraOffset;

		// Token: 0x0400262C RID: 9772
		public Texture2D ZoomOverlay;

		// Token: 0x0400262D RID: 9773
		public string ZoomOverlayName;

		// Token: 0x0400262E RID: 9774
		public int MaxZoomIn;

		// Token: 0x0400262F RID: 9775
		public int MaxZoomOut;

		// Token: 0x04002630 RID: 9776
		public int BaseFOV;

		// Token: 0x04002631 RID: 9777
		public Coroutine aimingCoroutine;

		// Token: 0x04002632 RID: 9778
		public bool aimingValue;
	}
}
