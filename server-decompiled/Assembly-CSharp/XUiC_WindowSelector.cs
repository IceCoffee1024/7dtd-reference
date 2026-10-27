using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000EC7 RID: 3783
[Preserve]
public class XUiC_WindowSelector : XUiController
{
	// Token: 0x140000BE RID: 190
	// (add) Token: 0x06007553 RID: 30035 RVA: 0x002C6454 File Offset: 0x002C4654
	// (remove) Token: 0x06007554 RID: 30036 RVA: 0x002C648C File Offset: 0x002C468C
	public event XUiC_WindowSelector.WindowSelectedDelegate WindowSelected;

	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x06007555 RID: 30037 RVA: 0x002C64C1 File Offset: 0x002C46C1
	// (set) Token: 0x06007556 RID: 30038 RVA: 0x002C64C9 File Offset: 0x002C46C9
	[XuiXmlAttribute("sound_on_open", false)]
	public string SoundOnOpen { get; set; }

	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x06007557 RID: 30039 RVA: 0x002C64D2 File Offset: 0x002C46D2
	// (set) Token: 0x06007558 RID: 30040 RVA: 0x002C64DA File Offset: 0x002C46DA
	[XuiXmlAttribute("sound_on_close", false)]
	public string SoundOnClose { get; set; }

	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x06007559 RID: 30041 RVA: 0x002C64E3 File Offset: 0x002C46E3
	// (set) Token: 0x0600755A RID: 30042 RVA: 0x002C64EB File Offset: 0x002C46EB
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

	// Token: 0x17000DF4 RID: 3572
	// (get) Token: 0x0600755B RID: 30043 RVA: 0x002C6522 File Offset: 0x002C4722
	// (set) Token: 0x0600755C RID: 30044 RVA: 0x002C652A File Offset: 0x002C472A
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

	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x0600755D RID: 30045 RVA: 0x002C6533 File Offset: 0x002C4733
	[XuiXmlBinding("open_window_key")]
	public string SelectedName
	{
		get
		{
			XUiC_ButtonSelectable xuiC_ButtonSelectable = this.SelectedButton;
			return ((xuiC_ButtonSelectable != null) ? xuiC_ButtonSelectable.ViewComponent.ID : null) ?? "";
		}
	}

	// Token: 0x17000DF6 RID: 3574
	// (get) Token: 0x0600755E RID: 30046 RVA: 0x002C6555 File Offset: 0x002C4755
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

	// Token: 0x0600755F RID: 30047 RVA: 0x002C6588 File Offset: 0x002C4788
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

	// Token: 0x06007560 RID: 30048 RVA: 0x002C65F0 File Offset: 0x002C47F0
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

	// Token: 0x06007561 RID: 30049 RVA: 0x002C6624 File Offset: 0x002C4824
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

	// Token: 0x06007562 RID: 30050 RVA: 0x002C66E0 File Offset: 0x002C48E0
	public void SetSelected(string _name)
	{
		XUiC_ButtonSelectable xuiC_ButtonSelectable;
		if (!this.buttonsByName.TryGetValue(_name, out xuiC_ButtonSelectable))
		{
			return;
		}
		this.SelectedButton = xuiC_ButtonSelectable;
	}

	// Token: 0x06007563 RID: 30051 RVA: 0x002C6705 File Offset: 0x002C4905
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

	// Token: 0x06007564 RID: 30052 RVA: 0x002C6744 File Offset: 0x002C4944
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

	// Token: 0x06007565 RID: 30053 RVA: 0x002C67F0 File Offset: 0x002C49F0
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

	// Token: 0x06007566 RID: 30054 RVA: 0x002C68F9 File Offset: 0x002C4AF9
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x06007567 RID: 30055 RVA: 0x002C6924 File Offset: 0x002C4B24
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

	// Token: 0x06007568 RID: 30056 RVA: 0x002C69C8 File Offset: 0x002C4BC8
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

	// Token: 0x06007569 RID: 30057 RVA: 0x002C6A3C File Offset: 0x002C4C3C
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

	// Token: 0x04005751 RID: 22353
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ButtonSelectable[] buttons;

	// Token: 0x04005752 RID: 22354
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_ButtonSelectable> buttonsByName = new CaseInsensitiveStringDictionary<XUiC_ButtonSelectable>();

	// Token: 0x04005755 RID: 22357
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ButtonSelectable selectedButton;

	// Token: 0x04005756 RID: 22358
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameBasedWindows = true;

	// Token: 0x02000EC8 RID: 3784
	// (Invoke) Token: 0x06007570 RID: 30064
	public delegate void WindowSelectedDelegate(XUiC_WindowSelector _sender, string _windowId);
}
