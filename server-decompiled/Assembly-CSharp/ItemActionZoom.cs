using System;
using System.Collections;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005E8 RID: 1512
[global::UnityEngine.Scripting.Preserve]
public class ItemActionZoom : ItemAction
{
	// Token: 0x06003161 RID: 12641 RVA: 0x0013F820 File Offset: 0x0013DA20
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

	// Token: 0x06003162 RID: 12642 RVA: 0x0013F89D File Offset: 0x0013DA9D
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionZoom.ItemActionDataZoom(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x0013F8A8 File Offset: 0x0013DAA8
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
			for (int i = 0; i < itemActionDataZoom.invData.itemValue.ModificationCount; i++)
			{
				ItemValue modification = itemActionDataZoom.invData.itemValue.GetModification(i);
				ItemClassModifier itemClassModifier = ((modification != null) ? modification.ItemClass : null) as ItemClassModifier;
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

	// Token: 0x06003164 RID: 12644 RVA: 0x0013FDA4 File Offset: 0x0013DFA4
	public override void StartHolding(ItemActionData _data)
	{
		base.StartHolding(_data);
		if (_data.invData.holdingEntity as EntityPlayerLocal != null)
		{
			GameManager.Instance.triggerEffectManager.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.LeftTrigger, TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.zoomTriggerEffectPullDualsense, this.zoomTriggerEffectPullXb)), false);
		}
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x0013FDF8 File Offset: 0x0013DFF8
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

	// Token: 0x06003166 RID: 12646 RVA: 0x0013FE70 File Offset: 0x0013E070
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

	// Token: 0x06003167 RID: 12647 RVA: 0x001400A4 File Offset: 0x0013E2A4
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

	// Token: 0x06003168 RID: 12648 RVA: 0x00140130 File Offset: 0x0013E330
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

	// Token: 0x06003169 RID: 12649 RVA: 0x001401E0 File Offset: 0x0013E3E0
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

	// Token: 0x0600316A RID: 12650 RVA: 0x0014030D File Offset: 0x0013E50D
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startEndZoomLater(ItemActionZoom.ItemActionDataZoom _actionData)
	{
		yield return new WaitForSecondsRealtime(0f);
		_actionData.invData.holdingEntity.AimingGun = _actionData.aimingValue;
		yield break;
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x0014031C File Offset: 0x0013E51C
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

	// Token: 0x0600316C RID: 12652 RVA: 0x0014035C File Offset: 0x0013E55C
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

	// Token: 0x0600316D RID: 12653 RVA: 0x00140458 File Offset: 0x0013E658
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionZoom.ItemActionDataZoom itemActionDataZoom = (ItemActionZoom.ItemActionDataZoom)_actionData;
		return itemActionDataZoom.bZoomInProgress && Time.time - itemActionDataZoom.timeZoomStarted < 0.3f;
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x0014048C File Offset: 0x0013E68C
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

	// Token: 0x0600316F RID: 12655 RVA: 0x001405E0 File Offset: 0x0013E7E0
	public override bool IsHUDDisabled(ItemActionData _data)
	{
		return base.ZoomOverlay != null && !_data.invData.holdingEntity.isEntityRemote && _data.invData.holdingEntity.AimingGun && !((ItemActionZoom.ItemActionDataZoom)_data).bZoomInProgress;
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x0014062F File Offset: 0x0013E82F
	public override void GetIronSights(ItemActionData _actionData, out float _fov)
	{
		_fov = ((base.ZoomOverlay == null) ? ((ItemActionZoom.ItemActionDataZoom)_actionData).CurrentZoom : 0f);
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x00140653 File Offset: 0x0013E853
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

	// Token: 0x06003172 RID: 12658 RVA: 0x0014068C File Offset: 0x0013E88C
	public override TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		return TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.zoomTriggerEffectPullDualsense, this.zoomTriggerEffectPullXb));
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x001406A4 File Offset: 0x0013E8A4
	public override TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		return TriggerEffectManager.GetTriggerEffect(new ValueTuple<string, string>(this.zoomTriggerEffectShootDualsense, this.zoomTriggerEffectShootXb));
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowConcurrentActions()
	{
		return true;
	}

	// Token: 0x0400261A RID: 9754
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectPullDualsense;

	// Token: 0x0400261B RID: 9755
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectShootDualsense;

	// Token: 0x0400261C RID: 9756
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectPullXb;

	// Token: 0x0400261D RID: 9757
	[PublicizedFrom(EAccessModifier.Private)]
	public string zoomTriggerEffectShootXb;

	// Token: 0x020005E9 RID: 1513
	public class ItemActionDataZoom : ItemActionData
	{
		// Token: 0x06003176 RID: 12662 RVA: 0x001406BC File Offset: 0x0013E8BC
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

		// Token: 0x0400261E RID: 9758
		public float CurrentZoom;

		// Token: 0x0400261F RID: 9759
		public Transform Scope;

		// Token: 0x04002620 RID: 9760
		public bool bZoomInProgress;

		// Token: 0x04002621 RID: 9761
		public float timeZoomStarted;

		// Token: 0x04002622 RID: 9762
		public int layerBeforeSwitch;

		// Token: 0x04002623 RID: 9763
		public bool HasScope;

		// Token: 0x04002624 RID: 9764
		public Vector3 SightsCameraOffset;

		// Token: 0x04002625 RID: 9765
		public Vector3 ScopeCameraOffset;

		// Token: 0x04002626 RID: 9766
		public Texture2D ZoomOverlay;

		// Token: 0x04002627 RID: 9767
		public string ZoomOverlayName;

		// Token: 0x04002628 RID: 9768
		public int MaxZoomIn;

		// Token: 0x04002629 RID: 9769
		public int MaxZoomOut;

		// Token: 0x0400262A RID: 9770
		public int BaseFOV;

		// Token: 0x0400262B RID: 9771
		public Coroutine aimingCoroutine;

		// Token: 0x0400262C RID: 9772
		public bool aimingValue;
	}
}
