using System;
using System.Collections.Generic;
using Platform;

// Token: 0x0200116D RID: 4461
public class XUiWindowGroup : GUIWindow
{
	// Token: 0x17001105 RID: 4357
	// (get) Token: 0x06008EC4 RID: 36548 RVA: 0x0035B852 File Offset: 0x00359A52
	// (set) Token: 0x06008EC5 RID: 36549 RVA: 0x0035B85A File Offset: 0x00359A5A
	public bool Initialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06008EC6 RID: 36550 RVA: 0x0035B864 File Offset: 0x00359A64
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

	// Token: 0x06008EC7 RID: 36551 RVA: 0x0035B928 File Offset: 0x00359B28
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

	// Token: 0x06008EC8 RID: 36552 RVA: 0x0035B9E0 File Offset: 0x00359BE0
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

	// Token: 0x06008EC9 RID: 36553 RVA: 0x0035BA40 File Offset: 0x00359C40
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

	// Token: 0x06008ECA RID: 36554 RVA: 0x0035BB14 File Offset: 0x00359D14
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

	// Token: 0x06008ECB RID: 36555 RVA: 0x0035BB94 File Offset: 0x00359D94
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

	// Token: 0x06008ECC RID: 36556 RVA: 0x0035BC24 File Offset: 0x00359E24
	public override bool HasActionSet()
	{
		return this.hasActionSetThisOpen;
	}

	// Token: 0x06008ECD RID: 36557 RVA: 0x0035BC2C File Offset: 0x00359E2C
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

	// Token: 0x04006865 RID: 26725
	public readonly XUi xui;

	// Token: 0x04006866 RID: 26726
	public XUiController Controller;

	// Token: 0x04006868 RID: 26728
	public readonly bool LeftPanelVAlignTop = true;

	// Token: 0x04006869 RID: 26729
	public readonly bool RightPanelVAlignTop = true;

	// Token: 0x0400686A RID: 26730
	public readonly int StackPanelYOffset = 457;

	// Token: 0x0400686B RID: 26731
	public readonly int StackPanelPadding = 9;

	// Token: 0x0400686C RID: 26732
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool openBackpackOnOpen;

	// Token: 0x0400686D RID: 26733
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool closeCompassOnOpen;

	// Token: 0x0400686E RID: 26734
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string defaultSelectedView;

	// Token: 0x0400686F RID: 26735
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiWindowGroup.EHasActionSetFor hasActionSetFor;

	// Token: 0x04006870 RID: 26736
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasActionSetThisOpen = true;

	// Token: 0x04006871 RID: 26737
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiV_Window> windows = new List<XUiV_Window>();

	// Token: 0x04006872 RID: 26738
	public readonly IReadOnlyList<XUiV_Window> Windows;

	// Token: 0x04006873 RID: 26739
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;

	// Token: 0x0200116E RID: 4462
	public enum EHasActionSetFor
	{
		// Token: 0x04006875 RID: 26741
		Both,
		// Token: 0x04006876 RID: 26742
		OnlyController,
		// Token: 0x04006877 RID: 26743
		OnlyKeyboard,
		// Token: 0x04006878 RID: 26744
		None
	}
}
