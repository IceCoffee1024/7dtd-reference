using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F23 RID: 3875
[Preserve]
public class XUiC_GamepadCalloutWindow : XUiController
{
	// Token: 0x060079A9 RID: 31145 RVA: 0x002DDD68 File Offset: 0x002DBF68
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

	// Token: 0x060079AA RID: 31146 RVA: 0x002DDE03 File Offset: 0x002DC003
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.CalloutWindow = null;
	}

	// Token: 0x060079AB RID: 31147 RVA: 0x002DDE18 File Offset: 0x002DC018
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

	// Token: 0x060079AC RID: 31148 RVA: 0x002DDE64 File Offset: 0x002DC064
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

	// Token: 0x060079AD RID: 31149 RVA: 0x002DDEF4 File Offset: 0x002DC0F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitContextMenuCallouts()
	{
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPageSelection", XUiC_GamepadCalloutWindow.CalloutType.MenuComboBox);
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPaging", XUiC_GamepadCalloutWindow.CalloutType.MenuPaging);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoCategoryLeft", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoCategoryRight", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x060079AE RID: 31150 RVA: 0x002DDF4C File Offset: 0x002DC14C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRWGCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igco_rwgCameraMode", XUiC_GamepadCalloutWindow.CalloutType.RWGEditor);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftStick, "igco_moveCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStick, "igco_pivotCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igco_rwgCameraSpeed", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
	}

	// Token: 0x060079AF RID: 31151 RVA: 0x002DDFBC File Offset: 0x002DC1BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCharacterPreviewCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igco_rwgRotateCharacterPreview", XUiC_GamepadCalloutWindow.CalloutType.CharacterPreview);
	}

	// Token: 0x060079B0 RID: 31152 RVA: 0x002DDFD7 File Offset: 0x002DC1D7
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCameraZoomCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoZoomInCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoZoomOutCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
	}

	// Token: 0x060079B1 RID: 31153 RVA: 0x002DE00C File Offset: 0x002DC20C
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

	// Token: 0x060079B2 RID: 31154 RVA: 0x002DE0C8 File Offset: 0x002DC2C8
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

	// Token: 0x060079B3 RID: 31155 RVA: 0x002DE118 File Offset: 0x002DC318
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

	// Token: 0x060079B4 RID: 31156 RVA: 0x002DE160 File Offset: 0x002DC360
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

	// Token: 0x060079B5 RID: 31157 RVA: 0x002DE1EC File Offset: 0x002DC3EC
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

	// Token: 0x060079B6 RID: 31158 RVA: 0x002DE234 File Offset: 0x002DC434
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

	// Token: 0x060079B7 RID: 31159 RVA: 0x002DE2C0 File Offset: 0x002DC4C0
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

	// Token: 0x060079B8 RID: 31160 RVA: 0x002DE330 File Offset: 0x002DC530
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

	// Token: 0x060079B9 RID: 31161 RVA: 0x002DE3A4 File Offset: 0x002DC5A4
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

	// Token: 0x060079BA RID: 31162 RVA: 0x002DE40C File Offset: 0x002DC60C
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

	// Token: 0x060079BB RID: 31163 RVA: 0x002DE44C File Offset: 0x002DC64C
	public void EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type, float _duration = 0f)
	{
		this.SetCalloutsEnabled(_type, true);
		XUiC_GamepadCalloutWindow.VisibilityData visibilityData = this.typeVisible[(int)_type];
		visibilityData.activeDuration = 0f;
		visibilityData.duration = _duration;
	}

	// Token: 0x060079BC RID: 31164 RVA: 0x002DE46F File Offset: 0x002DC66F
	public void DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		this.SetCalloutsEnabled(_type, false);
	}

	// Token: 0x060079BD RID: 31165 RVA: 0x002DE47C File Offset: 0x002DC67C
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

	// Token: 0x060079BE RID: 31166 RVA: 0x002DE4DC File Offset: 0x002DC6DC
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

	// Token: 0x060079BF RID: 31167 RVA: 0x002DE58C File Offset: 0x002DC78C
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

	// Token: 0x060079C0 RID: 31168 RVA: 0x002DE704 File Offset: 0x002DC904
	public void HideCallouts(bool _hideCallouts)
	{
		this.hideCallouts = _hideCallouts;
		this.ShowCallouts();
	}

	// Token: 0x040059CD RID: 22989
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Y_OFFSET = 5;

	// Token: 0x040059CE RID: 22990
	[PublicizedFrom(EAccessModifier.Private)]
	public const int X_OFFSET = 0;

	// Token: 0x040059CF RID: 22991
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle controllerStyle;

	// Token: 0x040059D0 RID: 22992
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_GamepadCalloutWindow.Callout> callouts = new List<XUiC_GamepadCalloutWindow.Callout>();

	// Token: 0x040059D1 RID: 22993
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>> calloutGroups = new EnumDictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>>();

	// Token: 0x040059D2 RID: 22994
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamepadCalloutWindow.VisibilityData[] typeVisible = new XUiC_GamepadCalloutWindow.VisibilityData[17];

	// Token: 0x040059D3 RID: 22995
	[PublicizedFrom(EAccessModifier.Private)]
	public bool localActionsEnabled;

	// Token: 0x040059D4 RID: 22996
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject stackObject;

	// Token: 0x040059D5 RID: 22997
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hideCallouts = true;

	// Token: 0x02000F24 RID: 3876
	public enum CalloutType
	{
		// Token: 0x040059D7 RID: 22999
		Menu,
		// Token: 0x040059D8 RID: 23000
		MenuLoot,
		// Token: 0x040059D9 RID: 23001
		MenuHoverItem,
		// Token: 0x040059DA RID: 23002
		MenuHoverAir,
		// Token: 0x040059DB RID: 23003
		SelectedOption,
		// Token: 0x040059DC RID: 23004
		MenuCategory,
		// Token: 0x040059DD RID: 23005
		MenuPaging,
		// Token: 0x040059DE RID: 23006
		MenuComboBox,
		// Token: 0x040059DF RID: 23007
		MenuShortcuts,
		// Token: 0x040059E0 RID: 23008
		World,
		// Token: 0x040059E1 RID: 23009
		Tabs,
		// Token: 0x040059E2 RID: 23010
		ColorPicker,
		// Token: 0x040059E3 RID: 23011
		CharacterEditor,
		// Token: 0x040059E4 RID: 23012
		RWGEditor,
		// Token: 0x040059E5 RID: 23013
		RWGCamera,
		// Token: 0x040059E6 RID: 23014
		CharacterPreview,
		// Token: 0x040059E7 RID: 23015
		CameraZoom,
		// Token: 0x040059E8 RID: 23016
		Count
	}

	// Token: 0x02000F25 RID: 3877
	[PublicizedFrom(EAccessModifier.Private)]
	public class VisibilityData
	{
		// Token: 0x040059E9 RID: 23017
		public bool isVisible;

		// Token: 0x040059EA RID: 23018
		public float duration;

		// Token: 0x040059EB RID: 23019
		public float activeDuration;
	}

	// Token: 0x02000F26 RID: 3878
	[PublicizedFrom(EAccessModifier.Private)]
	public class Callout : MonoBehaviour
	{
		// Token: 0x060079C3 RID: 31171 RVA: 0x002DE748 File Offset: 0x002DC948
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

		// Token: 0x060079C4 RID: 31172 RVA: 0x002DE8A8 File Offset: 0x002DCAA8
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

		// Token: 0x060079C5 RID: 31173 RVA: 0x002DE903 File Offset: 0x002DCB03
		public void SetAtlas(INGUIAtlas _atlas)
		{
			if (this.iconSprite != null)
			{
				this.iconSprite.atlas = _atlas;
				return;
			}
			this.atlasToSet = _atlas;
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x002DE927 File Offset: 0x002DCB27
		public void FreeCallout()
		{
			this.isFree = true;
			this.icon = UIUtils.ButtonIcon.Count;
			this.action = "";
		}

		// Token: 0x060079C7 RID: 31175 RVA: 0x002DE943 File Offset: 0x002DCB43
		public void RefreshIcon()
		{
			if (!this.isFree && this.icon != UIUtils.ButtonIcon.Count && this.iconSprite != null)
			{
				this.iconSprite.spriteName = UIUtils.GetSpriteName(this.icon);
			}
		}

		// Token: 0x040059EC RID: 23020
		public UIUtils.ButtonIcon icon;

		// Token: 0x040059ED RID: 23021
		public string action;

		// Token: 0x040059EE RID: 23022
		public static NGUIFont calloutFont;

		// Token: 0x040059EF RID: 23023
		public UISprite iconSprite;

		// Token: 0x040059F0 RID: 23024
		public UILabel actionLabel;

		// Token: 0x040059F1 RID: 23025
		public XUiC_GamepadCalloutWindow.CalloutType type;

		// Token: 0x040059F2 RID: 23026
		public bool bIsVisible = true;

		// Token: 0x040059F3 RID: 23027
		public bool isFree = true;

		// Token: 0x040059F4 RID: 23028
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public INGUIAtlas atlasToSet;
	}
}
