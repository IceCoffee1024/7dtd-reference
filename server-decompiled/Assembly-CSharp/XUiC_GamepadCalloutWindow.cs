using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F49 RID: 3913
[Preserve]
public class XUiC_GamepadCalloutWindow : XUiController
{
	// Token: 0x06007A5F RID: 31327 RVA: 0x002E508C File Offset: 0x002E328C
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

	// Token: 0x06007A60 RID: 31328 RVA: 0x002E5127 File Offset: 0x002E3327
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.CalloutWindow = null;
	}

	// Token: 0x06007A61 RID: 31329 RVA: 0x002E513C File Offset: 0x002E333C
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

	// Token: 0x06007A62 RID: 31330 RVA: 0x002E5188 File Offset: 0x002E3388
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

	// Token: 0x06007A63 RID: 31331 RVA: 0x002E5218 File Offset: 0x002E3418
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitContextMenuCallouts()
	{
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPageSelection", XUiC_GamepadCalloutWindow.CalloutType.MenuComboBox);
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPaging", XUiC_GamepadCalloutWindow.CalloutType.MenuPaging);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoCategoryLeft", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoCategoryRight", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x06007A64 RID: 31332 RVA: 0x002E5270 File Offset: 0x002E3470
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRWGCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igco_rwgCameraMode", XUiC_GamepadCalloutWindow.CalloutType.RWGEditor);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftStick, "igco_moveCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStick, "igco_pivotCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igco_rwgCameraSpeed", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
	}

	// Token: 0x06007A65 RID: 31333 RVA: 0x002E52E0 File Offset: 0x002E34E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCharacterPreviewCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igco_rwgRotateCharacterPreview", XUiC_GamepadCalloutWindow.CalloutType.CharacterPreview);
	}

	// Token: 0x06007A66 RID: 31334 RVA: 0x002E52FB File Offset: 0x002E34FB
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCameraZoomCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoZoomInCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoZoomOutCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
	}

	// Token: 0x06007A67 RID: 31335 RVA: 0x002E5330 File Offset: 0x002E3530
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

	// Token: 0x06007A68 RID: 31336 RVA: 0x002E53EC File Offset: 0x002E35EC
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

	// Token: 0x06007A69 RID: 31337 RVA: 0x002E543C File Offset: 0x002E363C
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

	// Token: 0x06007A6A RID: 31338 RVA: 0x002E5484 File Offset: 0x002E3684
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

	// Token: 0x06007A6B RID: 31339 RVA: 0x002E5510 File Offset: 0x002E3710
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

	// Token: 0x06007A6C RID: 31340 RVA: 0x002E5558 File Offset: 0x002E3758
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

	// Token: 0x06007A6D RID: 31341 RVA: 0x002E55E4 File Offset: 0x002E37E4
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

	// Token: 0x06007A6E RID: 31342 RVA: 0x002E5654 File Offset: 0x002E3854
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

	// Token: 0x06007A6F RID: 31343 RVA: 0x002E56C8 File Offset: 0x002E38C8
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

	// Token: 0x06007A70 RID: 31344 RVA: 0x002E5730 File Offset: 0x002E3930
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

	// Token: 0x06007A71 RID: 31345 RVA: 0x002E5770 File Offset: 0x002E3970
	public void EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type, float _duration = 0f)
	{
		this.SetCalloutsEnabled(_type, true);
		XUiC_GamepadCalloutWindow.VisibilityData visibilityData = this.typeVisible[(int)_type];
		visibilityData.activeDuration = 0f;
		visibilityData.duration = _duration;
	}

	// Token: 0x06007A72 RID: 31346 RVA: 0x002E5793 File Offset: 0x002E3993
	public void DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		this.SetCalloutsEnabled(_type, false);
	}

	// Token: 0x06007A73 RID: 31347 RVA: 0x002E57A0 File Offset: 0x002E39A0
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

	// Token: 0x06007A74 RID: 31348 RVA: 0x002E5800 File Offset: 0x002E3A00
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

	// Token: 0x06007A75 RID: 31349 RVA: 0x002E58B0 File Offset: 0x002E3AB0
	public void UpdateCalloutsForItemStack(GameObject _stackObject, ItemStack _itemStack, bool _isHovered, bool _canSwap = true, bool _canRemove = true, bool _canPlaceOne = true)
	{
		XUiC_DragAndDropWindow dragAndDropWindow = this.xui.DragAndDropWindow;
		this.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuHoverItem);
		if (_isHovered)
		{
			this.stackObject = _stackObject;
			if (_itemStack.itemValue.ItemClass != null)
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

	// Token: 0x06007A76 RID: 31350 RVA: 0x002E5A28 File Offset: 0x002E3C28
	public void HideCallouts(bool _hideCallouts)
	{
		this.hideCallouts = _hideCallouts;
		this.ShowCallouts();
	}

	// Token: 0x04005AB8 RID: 23224
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Y_OFFSET = 5;

	// Token: 0x04005AB9 RID: 23225
	[PublicizedFrom(EAccessModifier.Private)]
	public const int X_OFFSET = 0;

	// Token: 0x04005ABA RID: 23226
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle controllerStyle;

	// Token: 0x04005ABB RID: 23227
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_GamepadCalloutWindow.Callout> callouts = new List<XUiC_GamepadCalloutWindow.Callout>();

	// Token: 0x04005ABC RID: 23228
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>> calloutGroups = new EnumDictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>>();

	// Token: 0x04005ABD RID: 23229
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamepadCalloutWindow.VisibilityData[] typeVisible = new XUiC_GamepadCalloutWindow.VisibilityData[17];

	// Token: 0x04005ABE RID: 23230
	[PublicizedFrom(EAccessModifier.Private)]
	public bool localActionsEnabled;

	// Token: 0x04005ABF RID: 23231
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject stackObject;

	// Token: 0x04005AC0 RID: 23232
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hideCallouts = true;

	// Token: 0x02000F4A RID: 3914
	public enum CalloutType
	{
		// Token: 0x04005AC2 RID: 23234
		Menu,
		// Token: 0x04005AC3 RID: 23235
		MenuLoot,
		// Token: 0x04005AC4 RID: 23236
		MenuHoverItem,
		// Token: 0x04005AC5 RID: 23237
		MenuHoverAir,
		// Token: 0x04005AC6 RID: 23238
		SelectedOption,
		// Token: 0x04005AC7 RID: 23239
		MenuCategory,
		// Token: 0x04005AC8 RID: 23240
		MenuPaging,
		// Token: 0x04005AC9 RID: 23241
		MenuComboBox,
		// Token: 0x04005ACA RID: 23242
		MenuShortcuts,
		// Token: 0x04005ACB RID: 23243
		World,
		// Token: 0x04005ACC RID: 23244
		Tabs,
		// Token: 0x04005ACD RID: 23245
		ColorPicker,
		// Token: 0x04005ACE RID: 23246
		CharacterEditor,
		// Token: 0x04005ACF RID: 23247
		RWGEditor,
		// Token: 0x04005AD0 RID: 23248
		RWGCamera,
		// Token: 0x04005AD1 RID: 23249
		CharacterPreview,
		// Token: 0x04005AD2 RID: 23250
		CameraZoom,
		// Token: 0x04005AD3 RID: 23251
		Count
	}

	// Token: 0x02000F4B RID: 3915
	[PublicizedFrom(EAccessModifier.Private)]
	public class VisibilityData
	{
		// Token: 0x04005AD4 RID: 23252
		public bool isVisible;

		// Token: 0x04005AD5 RID: 23253
		public float duration;

		// Token: 0x04005AD6 RID: 23254
		public float activeDuration;
	}

	// Token: 0x02000F4C RID: 3916
	[PublicizedFrom(EAccessModifier.Private)]
	public class Callout : MonoBehaviour
	{
		// Token: 0x06007A79 RID: 31353 RVA: 0x002E5A6C File Offset: 0x002E3C6C
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

		// Token: 0x06007A7A RID: 31354 RVA: 0x002E5BCC File Offset: 0x002E3DCC
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

		// Token: 0x06007A7B RID: 31355 RVA: 0x002E5C27 File Offset: 0x002E3E27
		public void SetAtlas(INGUIAtlas _atlas)
		{
			if (this.iconSprite != null)
			{
				this.iconSprite.atlas = _atlas;
				return;
			}
			this.atlasToSet = _atlas;
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x002E5C4B File Offset: 0x002E3E4B
		public void FreeCallout()
		{
			this.isFree = true;
			this.icon = UIUtils.ButtonIcon.Count;
			this.action = "";
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x002E5C67 File Offset: 0x002E3E67
		public void RefreshIcon()
		{
			if (!this.isFree && this.icon != UIUtils.ButtonIcon.Count && this.iconSprite != null)
			{
				this.iconSprite.spriteName = UIUtils.GetSpriteName(this.icon);
			}
		}

		// Token: 0x04005AD7 RID: 23255
		public UIUtils.ButtonIcon icon;

		// Token: 0x04005AD8 RID: 23256
		public string action;

		// Token: 0x04005AD9 RID: 23257
		public static NGUIFont calloutFont;

		// Token: 0x04005ADA RID: 23258
		public UISprite iconSprite;

		// Token: 0x04005ADB RID: 23259
		public UILabel actionLabel;

		// Token: 0x04005ADC RID: 23260
		public XUiC_GamepadCalloutWindow.CalloutType type;

		// Token: 0x04005ADD RID: 23261
		public bool bIsVisible = true;

		// Token: 0x04005ADE RID: 23262
		public bool isFree = true;

		// Token: 0x04005ADF RID: 23263
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public INGUIAtlas atlasToSet;
	}
}
