using System;
using System.Collections.Generic;
using Platform;

// Token: 0x020011AF RID: 4527
public class XUiWindowGroup : GUIWindow
{
	// Token: 0x1700118D RID: 4493
	// (get) Token: 0x0600909C RID: 37020 RVA: 0x00366BAA File Offset: 0x00364DAA
	// (set) Token: 0x0600909D RID: 37021 RVA: 0x00366BB2 File Offset: 0x00364DB2
	public bool Initialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600909E RID: 37022 RVA: 0x00366BBC File Offset: 0x00364DBC
	public XUiWindowGroup(XUi _xui, string _id, XUiWindowGroup.EHasActionSetFor _hasActionSetFor, string _defaultSelectedName, int _stackPanelYOffset, int _stackPanelPadding, bool _openBackpackOnOpen, bool _closeCompassOnOpen)
		: base(_id)
	{
		this.Windows = this.windows.AsReadOnly();
		this.xui = _xui;
		this.playerUI = this.xui.playerUI;
		this.windowManager = this.playerUI.windowManager;
		this.hasActionSetFor = _hasActionSetFor;
		this.defaultSelectedView = _defaultSelectedName;
		if (_stackPanelYOffset != -2147483648)
		{
			this.StackPanelYOffset = _stackPanelYOffset;
		}
		if (_stackPanelPadding != -2147483648)
		{
			this.StackPanelPadding = _stackPanelPadding;
		}
		this.openBackpackOnOpen = _openBackpackOnOpen;
		this.closeCompassOnOpen = _closeCompassOnOpen;
	}

	// Token: 0x0600909F RID: 37023 RVA: 0x00366C80 File Offset: 0x00364E80
	public void Init()
	{
		if (this.Initialized)
		{
			return;
		}
		this.Controller.AutoBindComponents();
		this.Controller.Init();
		this.Controller.AutoBindEvents();
		foreach (XUiController xuiController in this.Controller.Children)
		{
			if (xuiController.ViewComponent != null)
			{
				this.windows.Add((XUiV_Window)xuiController.ViewComponent);
				xuiController.ViewComponent.IsVisible = false;
			}
		}
		this.windowManager.Add(this);
		this.Initialized = true;
	}

	// Token: 0x060090A0 RID: 37024 RVA: 0x00366D38 File Offset: 0x00364F38
	public override void Cleanup()
	{
		if (this.cleanedUp)
		{
			return;
		}
		this.cleanedUp = true;
		CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
		if (cursorController.navigationTarget != null && cursorController.navigationTarget.Controller.IsChildOf(this.Controller))
		{
			cursorController.SetNavigationTarget(null);
		}
		this.Controller.Cleanup();
	}

	// Token: 0x060090A1 RID: 37025 RVA: 0x00366D98 File Offset: 0x00364F98
	public override void OnOpen()
	{
		base.OnOpen();
		this.Controller.OnOpen();
		this.applyDefaultSelectedView();
		if (this.closeCompassOnOpen)
		{
			this.windowManager.Close("compass");
		}
		if (this.openBackpackOnOpen && GameManager.Instance != null)
		{
			this.windowManager.Open("backpack", false);
		}
		this.xui.RecenterWindowGroup(this, false);
		bool flag;
		switch (this.hasActionSetFor)
		{
		case XUiWindowGroup.EHasActionSetFor.Both:
			flag = true;
			break;
		case XUiWindowGroup.EHasActionSetFor.OnlyController:
			flag = PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard;
			break;
		case XUiWindowGroup.EHasActionSetFor.OnlyKeyboard:
			flag = PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard;
			break;
		case XUiWindowGroup.EHasActionSetFor.None:
			flag = false;
			break;
		default:
			flag = this.hasActionSetThisOpen;
			break;
		}
		this.hasActionSetThisOpen = flag;
	}

	// Token: 0x060090A2 RID: 37026 RVA: 0x00366E6C File Offset: 0x0036506C
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyDefaultSelectedView()
	{
		if (string.IsNullOrEmpty(this.defaultSelectedView))
		{
			return;
		}
		if (this.defaultSelectedView.StartsWith("bp."))
		{
			XUiC_BackpackWindow.defaultSelectedElement = this.defaultSelectedView.Remove(0, 3);
			return;
		}
		XUiController childById = this.Controller.GetChildById(this.defaultSelectedView);
		if (childById != null)
		{
			childById.SelectCursorElement(true, false);
			return;
		}
		Log.Warning("Could not find selectable element " + this.defaultSelectedView + " in WindowGroup " + this.Id);
	}

	// Token: 0x060090A3 RID: 37027 RVA: 0x00366EEC File Offset: 0x003650EC
	public override void OnClose()
	{
		base.OnClose();
		XUiC_DragAndDropWindow dragAndDropWindow = this.xui.DragAndDropWindow;
		if (dragAndDropWindow != null)
		{
			dragAndDropWindow.PlaceItemBackInInventory();
		}
		this.Controller.OnClose();
		if (this.openBackpackOnOpen && GameManager.Instance != null)
		{
			this.windowManager.Close("backpack");
		}
		if (this.xui.ToolTipWindow != null)
		{
			this.xui.ToolTipWindow.ToolTip = "";
		}
		XUiC_PopupMenu popupMenuWindow = this.xui.PopupMenuWindow;
		if (popupMenuWindow == null)
		{
			return;
		}
		popupMenuWindow.Close();
	}

	// Token: 0x060090A4 RID: 37028 RVA: 0x00366F7C File Offset: 0x0036517C
	public override bool HasActionSet()
	{
		return this.hasActionSetThisOpen;
	}

	// Token: 0x060090A5 RID: 37029 RVA: 0x00366F84 File Offset: 0x00365184
	public bool HasStackPanelWindows()
	{
		if (this.Controller == null)
		{
			return false;
		}
		using (List<XUiV_Window>.Enumerator enumerator = this.windows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsInStackPanel)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040069DA RID: 27098
	public readonly XUi xui;

	// Token: 0x040069DB RID: 27099
	public XUiController Controller;

	// Token: 0x040069DD RID: 27101
	public readonly bool LeftPanelVAlignTop = true;

	// Token: 0x040069DE RID: 27102
	public readonly bool RightPanelVAlignTop = true;

	// Token: 0x040069DF RID: 27103
	public readonly int StackPanelYOffset = 457;

	// Token: 0x040069E0 RID: 27104
	public readonly int StackPanelPadding = 9;

	// Token: 0x040069E1 RID: 27105
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool openBackpackOnOpen;

	// Token: 0x040069E2 RID: 27106
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool closeCompassOnOpen;

	// Token: 0x040069E3 RID: 27107
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string defaultSelectedView;

	// Token: 0x040069E4 RID: 27108
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiWindowGroup.EHasActionSetFor hasActionSetFor;

	// Token: 0x040069E5 RID: 27109
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasActionSetThisOpen = true;

	// Token: 0x040069E6 RID: 27110
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiV_Window> windows = new List<XUiV_Window>();

	// Token: 0x040069E7 RID: 27111
	public readonly IReadOnlyList<XUiV_Window> Windows;

	// Token: 0x040069E8 RID: 27112
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;

	// Token: 0x020011B0 RID: 4528
	public enum EHasActionSetFor
	{
		// Token: 0x040069EA RID: 27114
		Both,
		// Token: 0x040069EB RID: 27115
		OnlyController,
		// Token: 0x040069EC RID: 27116
		OnlyKeyboard,
		// Token: 0x040069ED RID: 27117
		None
	}
}
