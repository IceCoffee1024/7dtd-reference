using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000EBC RID: 3772
[Preserve]
public class XUiC_WindowSelector : XUiController
{
	// Token: 0x140000BE RID: 190
	// (add) Token: 0x0600752E RID: 29998 RVA: 0x002C4EE8 File Offset: 0x002C30E8
	// (remove) Token: 0x0600752F RID: 29999 RVA: 0x002C4F20 File Offset: 0x002C3120
	public event XUiC_WindowSelector.WindowSelectedDelegate WindowSelected;

	// Token: 0x17000DEB RID: 3563
	// (get) Token: 0x06007530 RID: 30000 RVA: 0x002C4F55 File Offset: 0x002C3155
	// (set) Token: 0x06007531 RID: 30001 RVA: 0x002C4F5D File Offset: 0x002C315D
	[XuiXmlAttribute("sound_on_open", false)]
	public string SoundOnOpen { get; set; }

	// Token: 0x17000DEC RID: 3564
	// (get) Token: 0x06007532 RID: 30002 RVA: 0x002C4F66 File Offset: 0x002C3166
	// (set) Token: 0x06007533 RID: 30003 RVA: 0x002C4F6E File Offset: 0x002C316E
	[XuiXmlAttribute("sound_on_close", false)]
	public string SoundOnClose { get; set; }

	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x06007534 RID: 30004 RVA: 0x002C4F77 File Offset: 0x002C3177
	// (set) Token: 0x06007535 RID: 30005 RVA: 0x002C4F7F File Offset: 0x002C317F
	public XUiC_ButtonSelectable SelectedButton
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.selectedButton;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.selectedButton != null)
			{
				this.selectedButton.IsSelected = false;
			}
			this.selectedButton = value;
			if (this.selectedButton != null)
			{
				this.selectedButton.IsSelected = true;
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x17000DEE RID: 3566
	// (get) Token: 0x06007536 RID: 30006 RVA: 0x002C4FB6 File Offset: 0x002C31B6
	// (set) Token: 0x06007537 RID: 30007 RVA: 0x002C4FBE File Offset: 0x002C31BE
	[XuiXmlAttribute("name_based_windows", false)]
	public bool NameBasedWindows
	{
		get
		{
			return this.nameBasedWindows;
		}
		set
		{
			this.nameBasedWindows = value;
		}
	}

	// Token: 0x17000DEF RID: 3567
	// (get) Token: 0x06007538 RID: 30008 RVA: 0x002C4FC7 File Offset: 0x002C31C7
	[XuiXmlBinding("open_window_key")]
	public string SelectedName
	{
		get
		{
			XUiC_ButtonSelectable xuiC_ButtonSelectable = this.SelectedButton;
			return ((xuiC_ButtonSelectable != null) ? xuiC_ButtonSelectable.ViewComponent.ID : null) ?? "";
		}
	}

	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x06007539 RID: 30009 RVA: 0x002C4FE9 File Offset: 0x002C31E9
	[XuiXmlBinding("open_window")]
	public string SelectedNameLocalized
	{
		get
		{
			if (this.SelectedButton == null)
			{
				return "";
			}
			return Localization.Get("xui" + this.SelectedButton.ViewComponent.ID, false, null);
		}
	}

	// Token: 0x0600753A RID: 30010 RVA: 0x002C501C File Offset: 0x002C321C
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.buttons.Length; i++)
		{
			XUiC_ButtonSelectable xuiC_ButtonSelectable = this.buttons[i];
			this.buttonsByName[xuiC_ButtonSelectable.ViewComponent.ID] = xuiC_ButtonSelectable;
		}
		if (this.buttons.Length != 0)
		{
			this.SetSelected(this.buttons[0].ViewComponent.ID);
		}
	}

	// Token: 0x0600753B RID: 30011 RVA: 0x002C5084 File Offset: 0x002C3284
	[XuiBindEvent("OnPress", "buttons")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnPress(XUiController _sender, int _mouseButton)
	{
		this.tryCloseCurrentWindow(delegate
		{
			this.SetSelected(_sender.ViewComponent.ID);
			this.OpenSelectedWindow();
		});
	}

	// Token: 0x0600753C RID: 30012 RVA: 0x002C50B8 File Offset: 0x002C32B8
	public void OpenSelectedWindow()
	{
		if (this.SelectedButton == null)
		{
			return;
		}
		if (!this.SelectedButton.ViewComponent.IsVisible || !this.SelectedButton.ViewComponent.Enabled)
		{
			this.toggleCategory(1);
			return;
		}
		string id = this.SelectedButton.ViewComponent.ID;
		XUiC_WindowSelector.WindowSelectedDelegate windowSelected = this.WindowSelected;
		if (windowSelected != null)
		{
			windowSelected(this, id);
		}
		if (!this.NameBasedWindows)
		{
			return;
		}
		if (!this.xui.playerUI.windowManager.IsWindowOpen(id))
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(this.windowGroup, false);
			this.xui.playerUI.windowManager.Open(id, true);
		}
	}

	// Token: 0x0600753D RID: 30013 RVA: 0x002C5174 File Offset: 0x002C3374
	public void SetSelected(string _name)
	{
		XUiC_ButtonSelectable xuiC_ButtonSelectable;
		if (!this.buttonsByName.TryGetValue(_name, out xuiC_ButtonSelectable))
		{
			return;
		}
		this.SelectedButton = xuiC_ButtonSelectable;
	}

	// Token: 0x0600753E RID: 30014 RVA: 0x002C5199 File Offset: 0x002C3399
	public override void OnOpen()
	{
		base.OnOpen();
		this.OpenSelectedWindow();
		if (this.xui.DragAndDropWindow != null)
		{
			this.xui.DragAndDropWindow.InMenu = true;
		}
		Manager.PlayInsidePlayerHead(this.SoundOnOpen, -1, 0f, false, false);
	}

	// Token: 0x0600753F RID: 30015 RVA: 0x002C51D8 File Offset: 0x002C33D8
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.DragAndDropWindow != null)
		{
			this.xui.DragAndDropWindow.InMenu = false;
		}
		Manager.PlayInsidePlayerHead(this.SoundOnClose, -1, 0f, false, false);
		if (this.xui.CurrentSelectedEntry != null)
		{
			this.xui.CurrentSelectedEntry.IsSelected = false;
		}
		if (!this.xui.playerUI.isPrimaryUI)
		{
			XUiWindowGroup xuiWindowGroup = this.xui.playerUI.windowManager.GetWindow("toolbelt") as XUiWindowGroup;
			if (xuiWindowGroup != null)
			{
				XUiC_Toolbelt childByType = xuiWindowGroup.Controller.GetChildByType<XUiC_Toolbelt>();
				if (childByType == null)
				{
					return;
				}
				childByType.ClearHoveredItems();
			}
		}
	}

	// Token: 0x06007540 RID: 30016 RVA: 0x002C5284 File Offset: 0x002C3484
	[PublicizedFrom(EAccessModifier.Private)]
	public void openSelectorAndWindow(string _selectedPage)
	{
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
		if (this.xui.playerUI.windowManager.IsWindowOpen(this.windowGroup) && this.SelectedButton.ViewComponent.ID.EqualsCaseInsensitive(_selectedPage))
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
			if (this.xui.playerUI.windowManager.IsWindowOpen(this.windowGroup))
			{
				this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			}
			return;
		}
		this.SetSelected(_selectedPage);
		if (this.xui.playerUI.windowManager.IsWindowOpen(this.windowGroup))
		{
			this.OpenSelectedWindow();
			return;
		}
		this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
		this.xui.playerUI.windowManager.Open(this.windowGroup, false);
	}

	// Token: 0x06007541 RID: 30017 RVA: 0x002C538D File Offset: 0x002C358D
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x06007542 RID: 30018 RVA: 0x002C53B8 File Offset: 0x002C35B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void toggleCategory(int _dir)
	{
		int num = 0;
		while (num < this.buttons.Length && this.buttons[num] != this.SelectedButton)
		{
			num++;
		}
		if (num >= this.buttons.Length)
		{
			num = -1;
		}
		XUiC_ButtonSelectable xuiC_ButtonSelectable;
		do
		{
			num = NGUIMath.RepeatIndex(num + _dir, this.buttons.Length);
			xuiC_ButtonSelectable = this.buttons[num];
		}
		while (!xuiC_ButtonSelectable.ViewComponent.IsVisible || !xuiC_ButtonSelectable.ViewComponent.Enabled);
		this.SetSelected(this.buttons[num].ViewComponent.ID);
		this.buttons[num].ViewComponent.PlayClickSound();
		this.OpenSelectedWindow();
	}

	// Token: 0x06007543 RID: 30019 RVA: 0x002C545C File Offset: 0x002C365C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
			if (guiactions.WindowPagingLeft.WasReleased)
			{
				ThreadManager.RunTaskAfterFrames(delegate
				{
					this.tryCloseCurrentWindow(delegate
					{
						this.toggleCategory(-1);
					});
				}, 1);
			}
			if (guiactions.WindowPagingRight.WasReleased)
			{
				ThreadManager.RunTaskAfterFrames(delegate
				{
					this.tryCloseCurrentWindow(delegate
					{
						this.toggleCategory(1);
					});
				}, 1);
			}
		}
	}

	// Token: 0x06007544 RID: 30020 RVA: 0x002C54D0 File Offset: 0x002C36D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void tryCloseCurrentWindow(Action _onDone)
	{
		XUiWindowGroup xuiWindowGroup = this.xui.playerUI.windowManager.GetModalWindow() as XUiWindowGroup;
		if (xuiWindowGroup == null)
		{
			this.xui.playerUI.windowManager.Close(this.xui.playerUI.windowManager.GetModalWindow(), false);
			_onDone();
			return;
		}
		IXUiWindowConditionalClosing ixuiWindowConditionalClosing = xuiWindowGroup.Controller as IXUiWindowConditionalClosing;
		if (ixuiWindowConditionalClosing != null)
		{
			ixuiWindowConditionalClosing.TryClose(_onDone, null);
			return;
		}
		foreach (XUiV_Window xuiV_Window in xuiWindowGroup.Windows)
		{
			IXUiWindowConditionalClosing ixuiWindowConditionalClosing2 = xuiV_Window.Controller as IXUiWindowConditionalClosing;
			if (ixuiWindowConditionalClosing2 != null)
			{
				ixuiWindowConditionalClosing2.TryClose(_onDone, null);
				return;
			}
		}
		_onDone();
	}

	// Token: 0x0400572C RID: 22316
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable[] buttons;

	// Token: 0x0400572D RID: 22317
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_ButtonSelectable> buttonsByName = new CaseInsensitiveStringDictionary<XUiC_ButtonSelectable>();

	// Token: 0x04005730 RID: 22320
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ButtonSelectable selectedButton;

	// Token: 0x04005731 RID: 22321
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameBasedWindows = true;

	// Token: 0x02000EBD RID: 3773
	// (Invoke) Token: 0x0600754B RID: 30027
	public delegate void WindowSelectedDelegate(XUiC_WindowSelector _sender, string _windowId);
}
