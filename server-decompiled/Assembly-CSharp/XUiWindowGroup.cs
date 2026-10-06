using System;
using System.Collections.Generic;
using Platform;

// Token: 0x0200119F RID: 4511
public class XUiWindowGroup : GUIWindow
{
	// Token: 0x17001182 RID: 4482
	// (get) Token: 0x06009048 RID: 36936 RVA: 0x00364ABA File Offset: 0x00362CBA
	// (set) Token: 0x06009049 RID: 36937 RVA: 0x00364AC2 File Offset: 0x00362CC2
	public bool Initialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600904A RID: 36938 RVA: 0x00364ACC File Offset: 0x00362CCC
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

	// Token: 0x0600904B RID: 36939 RVA: 0x00364B90 File Offset: 0x00362D90
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

	// Token: 0x0600904C RID: 36940 RVA: 0x00364C48 File Offset: 0x00362E48
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

	// Token: 0x0600904D RID: 36941 RVA: 0x00364CA8 File Offset: 0x00362EA8
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

	// Token: 0x0600904E RID: 36942 RVA: 0x00364D7C File Offset: 0x00362F7C
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

	// Token: 0x0600904F RID: 36943 RVA: 0x00364DFC File Offset: 0x00362FFC
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

	// Token: 0x06009050 RID: 36944 RVA: 0x00364E8C File Offset: 0x0036308C
	public override bool HasActionSet()
	{
		return this.hasActionSetThisOpen;
	}

	// Token: 0x06009051 RID: 36945 RVA: 0x00364E94 File Offset: 0x00363094
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

	// Token: 0x0400699A RID: 27034
	public readonly XUi xui;

	// Token: 0x0400699B RID: 27035
	public XUiController Controller;

	// Token: 0x0400699D RID: 27037
	public readonly bool LeftPanelVAlignTop = true;

	// Token: 0x0400699E RID: 27038
	public readonly bool RightPanelVAlignTop = true;

	// Token: 0x0400699F RID: 27039
	public readonly int StackPanelYOffset = 457;

	// Token: 0x040069A0 RID: 27040
	public readonly int StackPanelPadding = 9;

	// Token: 0x040069A1 RID: 27041
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool openBackpackOnOpen;

	// Token: 0x040069A2 RID: 27042
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool closeCompassOnOpen;

	// Token: 0x040069A3 RID: 27043
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string defaultSelectedView;

	// Token: 0x040069A4 RID: 27044
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiWindowGroup.EHasActionSetFor hasActionSetFor;

	// Token: 0x040069A5 RID: 27045
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasActionSetThisOpen = true;

	// Token: 0x040069A6 RID: 27046
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiV_Window> windows = new List<XUiV_Window>();

	// Token: 0x040069A7 RID: 27047
	public readonly IReadOnlyList<XUiV_Window> Windows;

	// Token: 0x040069A8 RID: 27048
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;

	// Token: 0x020011A0 RID: 4512
	public enum EHasActionSetFor
	{
		// Token: 0x040069AA RID: 27050
		Both,
		// Token: 0x040069AB RID: 27051
		OnlyController,
		// Token: 0x040069AC RID: 27052
		OnlyKeyboard,
		// Token: 0x040069AD RID: 27053
		None
	}
}
