using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F3A RID: 3898
[Preserve]
public class XUiC_GamepadCalloutWindow : XUiController
{
	// Token: 0x06007A62 RID: 31330 RVA: 0x002E1D1C File Offset: 0x002DFF1C
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

	// Token: 0x06007A63 RID: 31331 RVA: 0x002E1DB7 File Offset: 0x002DFFB7
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.CalloutWindow = null;
	}

	// Token: 0x06007A64 RID: 31332 RVA: 0x002E1DCC File Offset: 0x002DFFCC
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

	// Token: 0x06007A65 RID: 31333 RVA: 0x002E1E18 File Offset: 0x002E0018
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

	// Token: 0x06007A66 RID: 31334 RVA: 0x002E1EA8 File Offset: 0x002E00A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitContextMenuCallouts()
	{
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPageSelection", XUiC_GamepadCalloutWindow.CalloutType.MenuComboBox);
		this.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igcoPaging", XUiC_GamepadCalloutWindow.CalloutType.MenuPaging);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoCategoryLeft", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoCategoryRight", XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x06007A67 RID: 31335 RVA: 0x002E1F00 File Offset: 0x002E0100
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitRWGCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igco_rwgCameraMode", XUiC_GamepadCalloutWindow.CalloutType.RWGEditor);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftStick, "igco_moveCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStick, "igco_pivotCamera", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igco_rwgCameraSpeed", XUiC_GamepadCalloutWindow.CalloutType.RWGCamera);
	}

	// Token: 0x06007A68 RID: 31336 RVA: 0x002E1F70 File Offset: 0x002E0170
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCharacterPreviewCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStickLeftRight, "igco_rwgRotateCharacterPreview", XUiC_GamepadCalloutWindow.CalloutType.CharacterPreview);
	}

	// Token: 0x06007A69 RID: 31337 RVA: 0x002E1F8B File Offset: 0x002E018B
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitCameraZoomCallouts()
	{
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoZoomInCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoZoomOutCamera", XUiC_GamepadCalloutWindow.CalloutType.CameraZoom);
	}

	// Token: 0x06007A6A RID: 31338 RVA: 0x002E1FC0 File Offset: 0x002E01C0
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

	// Token: 0x06007A6B RID: 31339 RVA: 0x002E207C File Offset: 0x002E027C
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

	// Token: 0x06007A6C RID: 31340 RVA: 0x002E20CC File Offset: 0x002E02CC
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

	// Token: 0x06007A6D RID: 31341 RVA: 0x002E2114 File Offset: 0x002E0314
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

	// Token: 0x06007A6E RID: 31342 RVA: 0x002E21A0 File Offset: 0x002E03A0
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

	// Token: 0x06007A6F RID: 31343 RVA: 0x002E21E8 File Offset: 0x002E03E8
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

	// Token: 0x06007A70 RID: 31344 RVA: 0x002E2274 File Offset: 0x002E0474
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

	// Token: 0x06007A71 RID: 31345 RVA: 0x002E22E4 File Offset: 0x002E04E4
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

	// Token: 0x06007A72 RID: 31346 RVA: 0x002E2358 File Offset: 0x002E0558
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

	// Token: 0x06007A73 RID: 31347 RVA: 0x002E23C0 File Offset: 0x002E05C0
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

	// Token: 0x06007A74 RID: 31348 RVA: 0x002E2400 File Offset: 0x002E0600
	public void EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type, float _duration = 0f)
	{
		this.SetCalloutsEnabled(_type, true);
		XUiC_GamepadCalloutWindow.VisibilityData visibilityData = this.typeVisible[(int)_type];
		visibilityData.activeDuration = 0f;
		visibilityData.duration = _duration;
	}

	// Token: 0x06007A75 RID: 31349 RVA: 0x002E2423 File Offset: 0x002E0623
	public void DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType _type)
	{
		this.SetCalloutsEnabled(_type, false);
	}

	// Token: 0x06007A76 RID: 31350 RVA: 0x002E2430 File Offset: 0x002E0630
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

	// Token: 0x06007A77 RID: 31351 RVA: 0x002E2490 File Offset: 0x002E0690
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

	// Token: 0x06007A78 RID: 31352 RVA: 0x002E2540 File Offset: 0x002E0740
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

	// Token: 0x06007A79 RID: 31353 RVA: 0x002E26B8 File Offset: 0x002E08B8
	public void HideCallouts(bool _hideCallouts)
	{
		this.hideCallouts = _hideCallouts;
		this.ShowCallouts();
	}

	// Token: 0x04005A80 RID: 23168
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Y_OFFSET = 5;

	// Token: 0x04005A81 RID: 23169
	[PublicizedFrom(EAccessModifier.Private)]
	public const int X_OFFSET = 0;

	// Token: 0x04005A82 RID: 23170
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle controllerStyle;

	// Token: 0x04005A83 RID: 23171
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_GamepadCalloutWindow.Callout> callouts = new List<XUiC_GamepadCalloutWindow.Callout>();

	// Token: 0x04005A84 RID: 23172
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>> calloutGroups = new EnumDictionary<XUiC_GamepadCalloutWindow.CalloutType, List<XUiC_GamepadCalloutWindow.Callout>>();

	// Token: 0x04005A85 RID: 23173
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamepadCalloutWindow.VisibilityData[] typeVisible = new XUiC_GamepadCalloutWindow.VisibilityData[17];

	// Token: 0x04005A86 RID: 23174
	[PublicizedFrom(EAccessModifier.Private)]
	public bool localActionsEnabled;

	// Token: 0x04005A87 RID: 23175
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject stackObject;

	// Token: 0x04005A88 RID: 23176
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hideCallouts = true;

	// Token: 0x02000F3B RID: 3899
	public enum CalloutType
	{
		// Token: 0x04005A8A RID: 23178
		Menu,
		// Token: 0x04005A8B RID: 23179
		MenuLoot,
		// Token: 0x04005A8C RID: 23180
		MenuHoverItem,
		// Token: 0x04005A8D RID: 23181
		MenuHoverAir,
		// Token: 0x04005A8E RID: 23182
		SelectedOption,
		// Token: 0x04005A8F RID: 23183
		MenuCategory,
		// Token: 0x04005A90 RID: 23184
		MenuPaging,
		// Token: 0x04005A91 RID: 23185
		MenuComboBox,
		// Token: 0x04005A92 RID: 23186
		MenuShortcuts,
		// Token: 0x04005A93 RID: 23187
		World,
		// Token: 0x04005A94 RID: 23188
		Tabs,
		// Token: 0x04005A95 RID: 23189
		ColorPicker,
		// Token: 0x04005A96 RID: 23190
		CharacterEditor,
		// Token: 0x04005A97 RID: 23191
		RWGEditor,
		// Token: 0x04005A98 RID: 23192
		RWGCamera,
		// Token: 0x04005A99 RID: 23193
		CharacterPreview,
		// Token: 0x04005A9A RID: 23194
		CameraZoom,
		// Token: 0x04005A9B RID: 23195
		Count
	}

	// Token: 0x02000F3C RID: 3900
	[PublicizedFrom(EAccessModifier.Private)]
	public class VisibilityData
	{
		// Token: 0x04005A9C RID: 23196
		public bool isVisible;

		// Token: 0x04005A9D RID: 23197
		public float duration;

		// Token: 0x04005A9E RID: 23198
		public float activeDuration;
	}

	// Token: 0x02000F3D RID: 3901
	[PublicizedFrom(EAccessModifier.Private)]
	public class Callout : MonoBehaviour
	{
		// Token: 0x06007A7C RID: 31356 RVA: 0x002E26FC File Offset: 0x002E08FC
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

		// Token: 0x06007A7D RID: 31357 RVA: 0x002E285C File Offset: 0x002E0A5C
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

		// Token: 0x06007A7E RID: 31358 RVA: 0x002E28B7 File Offset: 0x002E0AB7
		public void SetAtlas(INGUIAtlas _atlas)
		{
			if (this.iconSprite != null)
			{
				this.iconSprite.atlas = _atlas;
				return;
			}
			this.atlasToSet = _atlas;
		}

		// Token: 0x06007A7F RID: 31359 RVA: 0x002E28DB File Offset: 0x002E0ADB
		public void FreeCallout()
		{
			this.isFree = true;
			this.icon = UIUtils.ButtonIcon.Count;
			this.action = "";
		}

		// Token: 0x06007A80 RID: 31360 RVA: 0x002E28F7 File Offset: 0x002E0AF7
		public void RefreshIcon()
		{
			if (!this.isFree && this.icon != UIUtils.ButtonIcon.Count && this.iconSprite != null)
			{
				this.iconSprite.spriteName = UIUtils.GetSpriteName(this.icon);
			}
		}

		// Token: 0x04005A9F RID: 23199
		public UIUtils.ButtonIcon icon;

		// Token: 0x04005AA0 RID: 23200
		public string action;

		// Token: 0x04005AA1 RID: 23201
		public static NGUIFont calloutFont;

		// Token: 0x04005AA2 RID: 23202
		public UISprite iconSprite;

		// Token: 0x04005AA3 RID: 23203
		public UILabel actionLabel;

		// Token: 0x04005AA4 RID: 23204
		public XUiC_GamepadCalloutWindow.CalloutType type;

		// Token: 0x04005AA5 RID: 23205
		public bool bIsVisible = true;

		// Token: 0x04005AA6 RID: 23206
		public bool isFree = true;

		// Token: 0x04005AA7 RID: 23207
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public INGUIAtlas atlasToSet;
	}
}
