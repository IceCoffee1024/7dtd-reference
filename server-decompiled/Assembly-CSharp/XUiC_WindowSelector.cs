using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000EAD RID: 3757
[Preserve]
public class XUiC_WindowSelector : XUiController
{
	// Token: 0x140000BF RID: 191
	// (add) Token: 0x06007536 RID: 30006 RVA: 0x002C1B30 File Offset: 0x002BFD30
	// (remove) Token: 0x06007537 RID: 30007 RVA: 0x002C1B68 File Offset: 0x002BFD68
	public event XUiC_WindowSelector.WindowSelectedDelegate WindowSelected;

	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x06007538 RID: 30008 RVA: 0x002C1B9D File Offset: 0x002BFD9D
	// (set) Token: 0x06007539 RID: 30009 RVA: 0x002C1BA5 File Offset: 0x002BFDA5
	[XuiXmlAttribute("sound_on_open", false)]
	public string SoundOnOpen { get; set; }

	// Token: 0x17000DD1 RID: 3537
	// (get) Token: 0x0600753A RID: 30010 RVA: 0x002C1BAE File Offset: 0x002BFDAE
	// (set) Token: 0x0600753B RID: 30011 RVA: 0x002C1BB6 File Offset: 0x002BFDB6
	[XuiXmlAttribute("sound_on_close", false)]
	public string SoundOnClose { get; set; }

	// Token: 0x17000DD2 RID: 3538
	// (get) Token: 0x0600753C RID: 30012 RVA: 0x002C1BBF File Offset: 0x002BFDBF
	// (set) Token: 0x0600753D RID: 30013 RVA: 0x002C1BC7 File Offset: 0x002BFDC7
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

	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x0600753E RID: 30014 RVA: 0x002C1BFE File Offset: 0x002BFDFE
	// (set) Token: 0x0600753F RID: 30015 RVA: 0x002C1C06 File Offset: 0x002BFE06
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

	// Token: 0x17000DD4 RID: 3540
	// (get) Token: 0x06007540 RID: 30016 RVA: 0x002C1C0F File Offset: 0x002BFE0F
	[XuiXmlBinding("open_window_key")]
	public string SelectedName
	{
		get
		{
			XUiC_ButtonSelectable xuiC_ButtonSelectable = this.SelectedButton;
			return ((xuiC_ButtonSelectable != null) ? xuiC_ButtonSelectable.ViewComponent.ID : null) ?? "";
		}
	}

	// Token: 0x17000DD5 RID: 3541
	// (get) Token: 0x06007541 RID: 30017 RVA: 0x002C1C31 File Offset: 0x002BFE31
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

	// Token: 0x06007542 RID: 30018 RVA: 0x002C1C64 File Offset: 0x002BFE64
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

	// Token: 0x06007543 RID: 30019 RVA: 0x002C1CCC File Offset: 0x002BFECC
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

	// Token: 0x06007544 RID: 30020 RVA: 0x002C1D00 File Offset: 0x002BFF00
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

	// Token: 0x06007545 RID: 30021 RVA: 0x002C1DBC File Offset: 0x002BFFBC
	public void SetSelected(string _name)
	{
		XUiC_ButtonSelectable xuiC_ButtonSelectable;
		if (!this.buttonsByName.TryGetValue(_name, out xuiC_ButtonSelectable))
		{
			return;
		}
		this.SelectedButton = xuiC_ButtonSelectable;
	}

	// Token: 0x06007546 RID: 30022 RVA: 0x002C1DE1 File Offset: 0x002BFFE1
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

	// Token: 0x06007547 RID: 30023 RVA: 0x002C1E20 File Offset: 0x002C0020
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

	// Token: 0x06007548 RID: 30024 RVA: 0x002C1ECC File Offset: 0x002C00CC
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

	// Token: 0x06007549 RID: 30025 RVA: 0x002C1FD5 File Offset: 0x002C01D5
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x0600754A RID: 30026 RVA: 0x002C2000 File Offset: 0x002C0200
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

	// Token: 0x0600754B RID: 30027 RVA: 0x002C20A4 File Offset: 0x002C02A4
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

	// Token: 0x0600754C RID: 30028 RVA: 0x002C2118 File Offset: 0x002C0318
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

	// Token: 0x040056F5 RID: 22261
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable[] buttons;

	// Token: 0x040056F6 RID: 22262
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_ButtonSelectable> buttonsByName = new CaseInsensitiveStringDictionary<XUiC_ButtonSelectable>();

	// Token: 0x040056F9 RID: 22265
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ButtonSelectable selectedButton;

	// Token: 0x040056FA RID: 22266
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameBasedWindows = true;

	// Token: 0x02000EAE RID: 3758
	// (Invoke) Token: 0x06007553 RID: 30035
	public delegate void WindowSelectedDelegate(XUiC_WindowSelector _sender, string _windowId);
}
