using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000E98 RID: 3736
[Preserve]
public class XUiC_WindowSelector : XUiController
{
	// Token: 0x140000BE RID: 190
	// (add) Token: 0x06007496 RID: 29846 RVA: 0x002BE604 File Offset: 0x002BC804
	// (remove) Token: 0x06007497 RID: 29847 RVA: 0x002BE63C File Offset: 0x002BC83C
	public event XUiC_WindowSelector.WindowSelectedDelegate WindowSelected;

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x06007498 RID: 29848 RVA: 0x002BE671 File Offset: 0x002BC871
	// (set) Token: 0x06007499 RID: 29849 RVA: 0x002BE679 File Offset: 0x002BC879
	[XuiXmlAttribute("sound_on_open", false)]
	public string SoundOnOpen { get; set; }

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x0600749A RID: 29850 RVA: 0x002BE682 File Offset: 0x002BC882
	// (set) Token: 0x0600749B RID: 29851 RVA: 0x002BE68A File Offset: 0x002BC88A
	[XuiXmlAttribute("sound_on_close", false)]
	public string SoundOnClose { get; set; }

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x0600749C RID: 29852 RVA: 0x002BE693 File Offset: 0x002BC893
	// (set) Token: 0x0600749D RID: 29853 RVA: 0x002BE69B File Offset: 0x002BC89B
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

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x0600749E RID: 29854 RVA: 0x002BE6D2 File Offset: 0x002BC8D2
	// (set) Token: 0x0600749F RID: 29855 RVA: 0x002BE6DA File Offset: 0x002BC8DA
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

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x060074A0 RID: 29856 RVA: 0x002BE6E3 File Offset: 0x002BC8E3
	[XuiXmlBinding("open_window_key")]
	public string SelectedName
	{
		get
		{
			XUiC_ButtonSelectable xuiC_ButtonSelectable = this.SelectedButton;
			return ((xuiC_ButtonSelectable != null) ? xuiC_ButtonSelectable.ViewComponent.ID : null) ?? "";
		}
	}

	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x060074A1 RID: 29857 RVA: 0x002BE705 File Offset: 0x002BC905
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

	// Token: 0x060074A2 RID: 29858 RVA: 0x002BE738 File Offset: 0x002BC938
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

	// Token: 0x060074A3 RID: 29859 RVA: 0x002BE7A0 File Offset: 0x002BC9A0
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

	// Token: 0x060074A4 RID: 29860 RVA: 0x002BE7D4 File Offset: 0x002BC9D4
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

	// Token: 0x060074A5 RID: 29861 RVA: 0x002BE890 File Offset: 0x002BCA90
	public void SetSelected(string _name)
	{
		XUiC_ButtonSelectable xuiC_ButtonSelectable;
		if (!this.buttonsByName.TryGetValue(_name, out xuiC_ButtonSelectable))
		{
			return;
		}
		this.SelectedButton = xuiC_ButtonSelectable;
	}

	// Token: 0x060074A6 RID: 29862 RVA: 0x002BE8B5 File Offset: 0x002BCAB5
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

	// Token: 0x060074A7 RID: 29863 RVA: 0x002BE8F4 File Offset: 0x002BCAF4
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

	// Token: 0x060074A8 RID: 29864 RVA: 0x002BE9A0 File Offset: 0x002BCBA0
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

	// Token: 0x060074A9 RID: 29865 RVA: 0x002BEAA9 File Offset: 0x002BCCA9
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x060074AA RID: 29866 RVA: 0x002BEAD4 File Offset: 0x002BCCD4
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

	// Token: 0x060074AB RID: 29867 RVA: 0x002BEB78 File Offset: 0x002BCD78
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

	// Token: 0x060074AC RID: 29868 RVA: 0x002BEBEC File Offset: 0x002BCDEC
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

	// Token: 0x04005651 RID: 22097
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable[] buttons;

	// Token: 0x04005652 RID: 22098
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_ButtonSelectable> buttonsByName = new CaseInsensitiveStringDictionary<XUiC_ButtonSelectable>();

	// Token: 0x04005655 RID: 22101
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ButtonSelectable selectedButton;

	// Token: 0x04005656 RID: 22102
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameBasedWindows = true;

	// Token: 0x02000E99 RID: 3737
	// (Invoke) Token: 0x060074B3 RID: 29875
	public delegate void WindowSelectedDelegate(XUiC_WindowSelector _sender, string _windowId);
}
