using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F58 RID: 3928
[Preserve]
public class XUiC_GamepadCalloutWindow : XUiController
{
	// Token: 0x06007AA2 RID: 31394 RVA: 0x002E697C File Offset: 0x002E4B7C
	public override void Init()
	{
		base.Init();
		this.xui.CalloutWindow = this;
		XUiC_GamepadCalloutWindow.Callout.calloutFont = this.xui.GetUIFontByName("ReferenceFont", true);
		for (int i = 0; i < 17; i++)
		{
			XUiC_GamepadCalloutWindow.CalloutType calloutType = (XUiC_GamepadCalloutWindow.CalloutType)i;
			this.calloutGroups.Add(calloutType, new List<XUiC_GamepadCalloutWindow.Callout>());
			this.typeVisible[i] = new XUiC_GamepadCalloutWindow.VisibilityData();
		}
		this.InitWorldCallouts();
		this.InitContextMenuCallouts();
		this.InitRWGCallouts();
		this.InitCharacterPreviewCallouts();
		this.InitCameraZoomCallouts();
		this.controllerStyle = PlatformManager.NativePlatform.Input.CurrentControllerInputStyle;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06007AA3 RID: 31395 RVA: 0x002E6A17 File Offset: 0x002E4C17
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.CalloutWindow = null;
	}

	// Token: 0x06007AA4 RID: 31396 RVA: 0x002E6A2C File Offset: 0x002E4C2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		for (int i = 0; i < this.callouts.Count; i++)
		{
			this.callouts[i].RefreshIcon();
		}
		this.HideCallouts(base.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard);
	}

	// Token: 0x06007AA5 RID: 31397 RVA: 0x002E6A78 File Offset: 0x002E4C78
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitWorldCallouts()
	{
		this.AddCallout(UIUtils.ButtonIcon.FaceButtonWest, "igcoInventory", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.FaceButtonWest, "igcoRadialMenu", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoJump", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.DPadUp, "igcoQuickSlot1", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.DPadRight, "igcoQuickSlot2", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.DPadDown, "igcoQuickSlot3", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.DPadLeft, "igcoToggleLight", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoActivate", XUiC_GamepadCalloutWindow.CalloutType.World);
		this.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoReload", XUiC_GamepadCalloutWindow.CalloutType.World);
	}

	// Token: 0x06007AA6 RID: 31398 RVA: 0x002E6B08 File Offset: 0x002E4D08
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitContextMenuCallouts()
	{
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPageSelection", XUiC_GamepadCalloutWindow.CalloutType.MenuComboBox);
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPaging", XUiC_GamepadCalloutWindow.CalloutType.MenuPaging);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoCategoryLeft", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoCategoryRight", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x06007AA7 RID: 31399 RVA: 0x002E6B60 File Offset: 0x002E4D60
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRWGCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igco_rwgCameraMode", XUiC_GamepadCalloutWindow.CalloutType.RWGEditor);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftStick, "igco_moveCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStick, "igco_pivotCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igco_rwgCameraSpeed", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
	}

	// Token: 0x06007AA8 RID: 31400 RVA: 0x002E6BD0 File Offset: 0x002E4DD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCharacterPreviewCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igco_rwgRotateCharacterPreview", XUiC_GamepadCalloutWindow.CalloutType.CharacterPreview);
	}

	// Token: 0x06007AA9 RID: 31401 RVA: 0x002E6BEB File Offset: 0x002E4DEB
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCameraZoomCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoZoomInCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoZoomOutCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
	}

	// Token: 0x06007AAA RID: 31402 RVA: 0x002E6C20 File Offset: 0x002E4E20
	public override void Update(float _dt)
	{
		base.Update(_dt);
		bool flag = this.xui.playerUI != null && this.xui.playerUI.playerInput != null && this.xui.playerUI.playerInput.Enabled;
		if (this.localActionsEnabled != flag)
		{
			this.localActionsEnabled = flag;
			if (flag)
			{
				this.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
			}
		}
		this.UpdateVisibility(_dt);
		if (this.stackObject != null && !this.stackObject.activeInHierarchy)
		{
			this.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
			this.stackObject = null;
		}
		if (this.IsDirty)
		{
			this.ResetFreeCallouts();
			this.ShowCallouts();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007AAB RID: 31403 RVA: 0x002E6CDC File Offset: 0x002E4EDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetFreeCallouts()
	{
		for (int i = 0; i < this.callouts.Count; i++)
		{
			XUiC_GamepadCalloutWindow.Callout callout = this.callouts[i];
			if (callout != null && callout.isFree)
			{
				callout.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06007AAC RID: 31404 RVA: 0x002E6D2C File Offset: 0x002E4F2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowCallouts()
	{
		int num = 0;
		for (int i = 0; i < 17; i++)
		{
			XUiC_GamepadCalloutWindow.VisibilityData visibilityData = this.typeVisible[i];
			if (!this.hideCallouts)
			{
				this.ShowCallouts((XUiC_GamepadCalloutWindow.CalloutType)i, visibilityData.isVisible, ref num);
			}
			else
			{
				this.ShowCallouts((XUiC_GamepadCalloutWindow.CalloutType)i, false, ref num);
			}
		}
	}

	// Token: 0x06007AAD RID: 31405 RVA: 0x002E6D74 File Offset: 0x002E4F74
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowCallouts(XUiC_GamepadCalloutWindow.CalloutType _type, bool _visible, ref int _currentOffset)
	{
		List<XUiC_GamepadCalloutWindow.Callout> list = this.calloutGroups[_type];
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				XUiC_GamepadCalloutWindow.Callout callout = list[i];
				bool flag = _visible && callout.bIsVisible;
				if (flag)
				{
					callout.transform.localPosition = new Vector2(0f, (float)_currentOffset);
					_currentOffset -= 5 + callout.iconSprite.height;
				}
				if (callout != null)
				{
					callout.gameObject.SetActive(flag);
				}
			}
		}
	}

	// Token: 0x06007AAE RID: 31406 RVA: 0x002E6E00 File Offset: 0x002E5000
	public void AddCallout(UIUtils.ButtonIcon _button, string _action, XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		if (this.ContainsCallout(_button, _action))
		{
			return;
		}
		XUiC_GamepadCalloutWindow.Callout callout = this.GetCallout(_type);
		callout.SetupCallout(_button, _action);
		List<XUiC_GamepadCalloutWindow.Callout> list = this.calloutGroups[_type];
		if (list != null)
		{
			list.Add(callout);
		}
		this.IsDirty = true;
	}

	// Token: 0x06007AAF RID: 31407 RVA: 0x002E6E48 File Offset: 0x002E5048
	public void RemoveCallout(UIUtils.ButtonIcon _button, string _action, XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		XUiC_GamepadCalloutWindow.Callout callout = null;
		foreach (XUiC_GamepadCalloutWindow.Callout callout2 in this.callouts)
		{
			if (callout2 != null && callout2.icon == _button && callout2.action == _action && callout2.type == _type)
			{
				callout = callout2;
				break;
			}
		}
		if (callout != null)
		{
			callout.FreeCallout();
		}
	}

	// Token: 0x06007AB0 RID: 31408 RVA: 0x002E6ED4 File Offset: 0x002E50D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowCallout(UIUtils.ButtonIcon _button, XUiC_GamepadCalloutWindow.CalloutType _type, bool _visible)
	{
		List<XUiC_GamepadCalloutWindow.Callout> list = this.calloutGroups[_type];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != null && list[i].iconSprite.spriteName == UIUtils.GetSpriteName(_button))
			{
				list[i].bIsVisible = _visible;
				break;
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x06007AB1 RID: 31409 RVA: 0x002E6F44 File Offset: 0x002E5144
	public bool ContainsCallout(UIUtils.ButtonIcon _button, string _action)
	{
		foreach (XUiC_GamepadCalloutWindow.Callout callout in this.callouts)
		{
			if (callout != null && callout.icon == _button && callout.action == _action)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007AB2 RID: 31410 RVA: 0x002E6FB8 File Offset: 0x002E51B8
	public void ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		List<XUiC_GamepadCalloutWindow.Callout> list;
		if (this.calloutGroups.TryGetValue(_type, out list) && list.Count > 0)
		{
			for (int i = 0; i < this.callouts.Count; i++)
			{
				XUiC_GamepadCalloutWindow.Callout callout = this.callouts[i];
				if (callout.type == _type)
				{
					callout.FreeCallout();
				}
			}
			list.Clear();
			this.IsDirty = true;
		}
	}

	// Token: 0x06007AB3 RID: 31411 RVA: 0x002E7020 File Offset: 0x002E5220
	public void SetCalloutsEnabled(XUiC_GamepadCalloutWindow.CalloutType _type, bool _enabled)
	{
		XUiC_GamepadCalloutWindow.VisibilityData visibilityData = this.typeVisible[(int)_type];
		if (visibilityData != null && visibilityData.isVisible != _enabled)
		{
			visibilityData.isVisible = _enabled;
			this.IsDirty = true;
		}
		if (_enabled && _type != XUiC_GamepadCalloutWindow.CalloutType.World)
		{
			this.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.World);
		}
	}

	// Token: 0x06007AB4 RID: 31412 RVA: 0x002E7060 File Offset: 0x002E5260
	public void EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type, float _duration = 0f)
	{
		this.SetCalloutsEnabled(_type, true);
		XUiC_GamepadCalloutWindow.VisibilityData visibilityData = this.typeVisible[(int)_type];
		visibilityData.activeDuration = 0f;
		visibilityData.duration = _duration;
	}

	// Token: 0x06007AB5 RID: 31413 RVA: 0x002E7083 File Offset: 0x002E5283
	public void DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		this.SetCalloutsEnabled(_type, false);
	}

	// Token: 0x06007AB6 RID: 31414 RVA: 0x002E7090 File Offset: 0x002E5290
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateVisibility(float _dt)
	{
		for (int i = 0; i < 17; i++)
		{
			XUiC_GamepadCalloutWindow.VisibilityData visibilityData = this.typeVisible[i];
			if (visibilityData.isVisible && visibilityData.duration != 0f)
			{
				visibilityData.activeDuration += Time.unscaledDeltaTime;
				if (visibilityData.activeDuration > visibilityData.duration)
				{
					this.DisableCallouts((XUiC_GamepadCalloutWindow.CalloutType)i);
				}
			}
		}
	}

	// Token: 0x06007AB7 RID: 31415 RVA: 0x002E70F0 File Offset: 0x002E52F0
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamepadCalloutWindow.Callout GetCallout(XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		XUiC_GamepadCalloutWindow.Callout callout = null;
		bool flag = false;
		for (int i = 0; i < this.callouts.Count; i++)
		{
			XUiC_GamepadCalloutWindow.Callout callout2 = this.callouts[i];
			if (callout2 != null && callout2.isFree)
			{
				callout = callout2;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			callout = this.viewComponent.UiTransform.gameObject.AddChild<XUiC_GamepadCalloutWindow.Callout>();
			if (callout.iconSprite == null)
			{
				callout.iconSprite = callout.gameObject.AddChild<UISprite>();
			}
			this.callouts.Add(callout);
			callout.SetAtlas(UIUtils.IconAtlas);
		}
		callout.type = _type;
		callout.isFree = false;
		callout.bIsVisible = true;
		return callout;
	}

	// Token: 0x06007AB8 RID: 31416 RVA: 0x002E71A0 File Offset: 0x002E53A0
	public void UpdateCalloutsForItemStack(GameObject _stackObject, ItemStack _itemStack, bool _isHovered, bool _canSwap = true, bool _canRemove = true, bool _canPlaceOne = true)
	{
		XUiC_DragAndDropWindow dragAndDropWindow = this.xui.DragAndDropWindow;
		this.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
		if (_isHovered)
		{
			this.stackObject = _stackObject;
			bool flag;
			if (_itemStack == null)
			{
				flag = null != null;
			}
			else
			{
				ItemValue itemValue = _itemStack.itemValue;
				flag = ((itemValue != null) ? itemValue.ItemClass : null) != null;
			}
			if (flag)
			{
				if (dragAndDropWindow.CurrentStack.IsEmpty())
				{
					if (_canRemove)
					{
						if (_itemStack.itemValue.ItemClass.CanStack() && _itemStack.count > 1)
						{
							this.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoTakeAll", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
							this.AddCallout(UIUtils.ButtonIcon.FaceButtonWest, "igcoTakeHalf", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
							this.ShowCallout(UIUtils.ButtonIcon.FaceButtonSouth, XUiC_GamepadCalloutWindow.CalloutType.Menu, false);
						}
						else
						{
							this.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoTake", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
							this.ShowCallout(UIUtils.ButtonIcon.FaceButtonSouth, XUiC_GamepadCalloutWindow.CalloutType.Menu, false);
						}
						this.AddCallout(UIUtils.ButtonIcon.RightStick, "igcoQuickMove", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
					}
				}
				else if (_itemStack.CanStackWith(dragAndDropWindow.CurrentStack, false))
				{
					this.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoPlaceAll", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
					if (_canPlaceOne)
					{
						this.AddCallout(UIUtils.ButtonIcon.FaceButtonWest, "igcoPlaceOne", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
					}
				}
				else if (_canSwap)
				{
					this.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSwap", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
				}
			}
			else if (!dragAndDropWindow.CurrentStack.IsEmpty())
			{
				ItemClass itemClass = dragAndDropWindow.CurrentStack.itemValue.ItemClass;
				if (itemClass != null && _canSwap)
				{
					if (itemClass.CanStack())
					{
						this.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoPlaceAll", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
						this.AddCallout(UIUtils.ButtonIcon.FaceButtonWest, "igcoPlaceOne", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
					}
					else
					{
						this.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoPlace", XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
					}
				}
			}
			this.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem, 0f);
			return;
		}
		this.stackObject = null;
		this.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
		this.ShowCallout(UIUtils.ButtonIcon.FaceButtonSouth, XUiC_GamepadCalloutWindow.CalloutType.Menu, true);
	}

	// Token: 0x06007AB9 RID: 31417 RVA: 0x002E7325 File Offset: 0x002E5525
	public void HideCallouts(bool _hideCallouts)
	{
		this.hideCallouts = _hideCallouts;
		this.ShowCallouts();
	}

	// Token: 0x04005AEB RID: 23275
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Y_OFFSET = 5;

	// Token: 0x04005AEC RID: 23276
	[PublicizedFrom(EAccessModifier.Private)]
	public const int X_OFFSET = 0;

	// Token: 0x04005AED RID: 23277
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle controllerStyle;

	// Token: 0x04005AEE RID: 23278
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_GamepadCalloutWindow.Callout> callouts = new List<XUiC_GamepadCalloutWindow.Callout>();

	// Token: 0x04005AEF RID: 23279
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>> calloutGroups = new EnumDictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>>();

	// Token: 0x04005AF0 RID: 23280
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamepadCalloutWindow.VisibilityData[] typeVisible = new XUiC_GamepadCalloutWindow.VisibilityData[17];

	// Token: 0x04005AF1 RID: 23281
	[PublicizedFrom(EAccessModifier.Private)]
	public bool localActionsEnabled;

	// Token: 0x04005AF2 RID: 23282
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject stackObject;

	// Token: 0x04005AF3 RID: 23283
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hideCallouts = true;

	// Token: 0x02000F59 RID: 3929
	public enum CalloutType
	{
		// Token: 0x04005AF5 RID: 23285
		Menu,
		// Token: 0x04005AF6 RID: 23286
		MenuLoot,
		// Token: 0x04005AF7 RID: 23287
		MenuHoverItem,
		// Token: 0x04005AF8 RID: 23288
		MenuHoverAir,
		// Token: 0x04005AF9 RID: 23289
		SelectedOption,
		// Token: 0x04005AFA RID: 23290
		MenuCategory,
		// Token: 0x04005AFB RID: 23291
		MenuPaging,
		// Token: 0x04005AFC RID: 23292
		MenuComboBox,
		// Token: 0x04005AFD RID: 23293
		MenuShortcuts,
		// Token: 0x04005AFE RID: 23294
		World,
		// Token: 0x04005AFF RID: 23295
		Tabs,
		// Token: 0x04005B00 RID: 23296
		ColorPicker,
		// Token: 0x04005B01 RID: 23297
		CharacterEditor,
		// Token: 0x04005B02 RID: 23298
		RWGEditor,
		// Token: 0x04005B03 RID: 23299
		RWGCamera,
		// Token: 0x04005B04 RID: 23300
		CharacterPreview,
		// Token: 0x04005B05 RID: 23301
		CameraZoom,
		// Token: 0x04005B06 RID: 23302
		Count
	}

	// Token: 0x02000F5A RID: 3930
	[PublicizedFrom(EAccessModifier.Private)]
	public class VisibilityData
	{
		// Token: 0x04005B07 RID: 23303
		public bool isVisible;

		// Token: 0x04005B08 RID: 23304
		public float duration;

		// Token: 0x04005B09 RID: 23305
		public float activeDuration;
	}

	// Token: 0x02000F5B RID: 3931
	[PublicizedFrom(EAccessModifier.Private)]
	public class Callout : MonoBehaviour
	{
		// Token: 0x06007ABC RID: 31420 RVA: 0x002E7368 File Offset: 0x002E5568
		public void Awake()
		{
			this.bIsVisible = true;
			base.gameObject.SetActive(false);
			if (this.iconSprite == null)
			{
				this.iconSprite = base.gameObject.AddChild<UISprite>();
			}
			this.iconSprite.pivot = UIWidget.Pivot.TopLeft;
			this.iconSprite.height = 35;
			this.iconSprite.width = 35;
			this.iconSprite.transform.localPosition = Vector3.zero;
			if (this.atlasToSet != null)
			{
				this.iconSprite.atlas = this.atlasToSet;
				this.atlasToSet = null;
			}
			this.iconSprite.fixedAspect = true;
			this.actionLabel = base.gameObject.AddChild<UILabel>();
			this.actionLabel.font = XUiC_GamepadCalloutWindow.Callout.calloutFont;
			this.actionLabel.fontSize = 32;
			this.actionLabel.pivot = UIWidget.Pivot.TopLeft;
			this.actionLabel.overflowMethod = UILabel.Overflow.ResizeFreely;
			this.actionLabel.alignment = NGUIText.Alignment.Left;
			this.actionLabel.transform.localPosition = new Vector2(40f, 0f);
			this.actionLabel.effectStyle = UILabel.Effect.Outline;
			this.actionLabel.effectColor = new Color32(0, 0, 0, byte.MaxValue);
			this.actionLabel.effectDistance = new Vector2(1.5f, 1.5f);
		}

		// Token: 0x06007ABD RID: 31421 RVA: 0x002E74C8 File Offset: 0x002E56C8
		public void SetupCallout(UIUtils.ButtonIcon _icon, string _action)
		{
			if (this.iconSprite != null && this.actionLabel != null)
			{
				this.icon = _icon;
				this.action = _action;
				this.iconSprite.spriteName = UIUtils.GetSpriteName(_icon);
				this.actionLabel.text = Localization.Get(_action, false, null);
			}
		}

		// Token: 0x06007ABE RID: 31422 RVA: 0x002E7523 File Offset: 0x002E5723
		public void SetAtlas(INGUIAtlas _atlas)
		{
			if (this.iconSprite != null)
			{
				this.iconSprite.atlas = _atlas;
				return;
			}
			this.atlasToSet = _atlas;
		}

		// Token: 0x06007ABF RID: 31423 RVA: 0x002E7547 File Offset: 0x002E5747
		public void FreeCallout()
		{
			this.isFree = true;
			this.icon = UIUtils.ButtonIcon.Count;
			this.action = "";
		}

		// Token: 0x06007AC0 RID: 31424 RVA: 0x002E7563 File Offset: 0x002E5763
		public void RefreshIcon()
		{
			if (!this.isFree && this.icon != UIUtils.ButtonIcon.Count && this.iconSprite != null)
			{
				this.iconSprite.spriteName = UIUtils.GetSpriteName(this.icon);
			}
		}

		// Token: 0x04005B0A RID: 23306
		public UIUtils.ButtonIcon icon;

		// Token: 0x04005B0B RID: 23307
		public string action;

		// Token: 0x04005B0C RID: 23308
		public static NGUIFont calloutFont;

		// Token: 0x04005B0D RID: 23309
		public UISprite iconSprite;

		// Token: 0x04005B0E RID: 23310
		public UILabel actionLabel;

		// Token: 0x04005B0F RID: 23311
		public XUiC_GamepadCalloutWindow.CalloutType type;

		// Token: 0x04005B10 RID: 23312
		public bool bIsVisible = true;

		// Token: 0x04005B11 RID: 23313
		public bool isFree = true;

		// Token: 0x04005B12 RID: 23314
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public INGUIAtlas atlasToSet;
	}
}
