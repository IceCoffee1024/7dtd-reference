using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020010CE RID: 4302
[Preserve]
public class XUiC_TwitchWindowSelector : XUiController
{
	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x060088BE RID: 35006 RVA: 0x00345DFA File Offset: 0x00343FFA
	// (set) Token: 0x060088BF RID: 35007 RVA: 0x00345E02 File Offset: 0x00344002
	public new XUiV_Button Selected
	{
		get
		{
			return this.selected;
		}
		set
		{
			if (this.selected != null)
			{
				this.selected.Selected = false;
			}
			this.selected = value;
			if (this.selected != null)
			{
				this.selected.Selected = true;
				this.HandleSelectedChange();
			}
		}
	}

	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x060088C0 RID: 35008 RVA: 0x00345E39 File Offset: 0x00344039
	public string SelectedName
	{
		get
		{
			if (this.selected == null)
			{
				return "";
			}
			return this.selected.ID;
		}
	}

	// Token: 0x060088C1 RID: 35009 RVA: 0x00345E54 File Offset: 0x00344054
	public override void Init()
	{
		base.Init();
		XUiC_TwitchWindowSelector.ID = base.WindowGroup.Id;
		XUiController childById = base.GetChildById("lblWindowName");
		if (childById != null)
		{
			this.lblWindowName = (XUiV_Label)childById.ViewComponent;
		}
		this.categories.Clear();
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiController xuiController = this.children[i];
			if (xuiController.ViewComponent.EventOnPress)
			{
				xuiController.OnPress += this.HandleOnPress;
				this.categories.Add(xuiController.ViewComponent.ID.ToLower());
			}
		}
		this.SetSelected("actions");
	}

	// Token: 0x060088C2 RID: 35010 RVA: 0x00345F0A File Offset: 0x0034410A
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnPress(XUiController _sender, int _mouseButton)
	{
		this.Selected = (XUiV_Button)_sender.ViewComponent;
		this.OpenSelectedWindow();
	}

	// Token: 0x060088C3 RID: 35011 RVA: 0x00345F23 File Offset: 0x00344123
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleSelectedChange()
	{
		this.updateWindowTitle();
	}

	// Token: 0x060088C4 RID: 35012 RVA: 0x00345F2B File Offset: 0x0034412B
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateWindowTitle()
	{
		this.lblWindowName.Text = ((this.selected != null) ? Localization.Get("TwitchInfo_" + this.Selected.ID, false, null) : "");
	}

	// Token: 0x060088C5 RID: 35013 RVA: 0x00345F64 File Offset: 0x00344164
	public void OpenSelectedWindow()
	{
		if (this.Selected != null)
		{
			this.updateWindowTitle();
			string id = this.Selected.ID;
			GUIWindowManager windowManager = this.xui.playerUI.windowManager;
			if (id == "Actions")
			{
				this.xui.FindWindowGroupByName("twitchInfo").GetChildByType<XUiC_TwitchEntryListWindow>().SetOpenToActions(XUiC_TwitchWindowSelector.extras);
				XUiC_TwitchWindowSelector.extras = false;
				windowManager.Open("twitchInfo", true);
				return;
			}
			if (id == "Votes")
			{
				this.xui.FindWindowGroupByName("twitchInfo").GetChildByType<XUiC_TwitchEntryListWindow>().SetOpenToVotes();
				windowManager.Open("twitchInfo", true);
				return;
			}
			if (id == "ActionHistory")
			{
				this.xui.FindWindowGroupByName("twitchInfo").GetChildByType<XUiC_TwitchEntryListWindow>().SetOpenToHistory();
				windowManager.Open("twitchInfo", true);
				return;
			}
			if (!(id == "Leaderboard"))
			{
				return;
			}
			this.xui.FindWindowGroupByName("twitchInfo").GetChildByType<XUiC_TwitchEntryListWindow>().SetOpenToLeaderboard();
			windowManager.Open("twitchInfo", true);
		}
	}

	// Token: 0x060088C6 RID: 35014 RVA: 0x00346080 File Offset: 0x00344280
	public void SetSelected(string name)
	{
		XUiController childById = base.GetChildById(name.ToLower());
		if (((childById != null) ? childById.ViewComponent : null) is XUiV_Button)
		{
			this.Selected = (XUiV_Button)childById.ViewComponent;
			this.currentCategoryIndex = this.categories.IndexOf(this.Selected.ID.ToLower());
		}
	}

	// Token: 0x060088C7 RID: 35015 RVA: 0x003460DF File Offset: 0x003442DF
	public override void OnOpen()
	{
		base.OnOpen();
		this.OpenSelectedWindow();
		this.xui.DragAndDropWindow.InMenu = true;
		Manager.PlayInsidePlayerHead("open_inventory", -1, 0f, false, false);
		base.RefreshBindings();
	}

	// Token: 0x060088C8 RID: 35016 RVA: 0x00346118 File Offset: 0x00344318
	public override void OnClose()
	{
		base.OnClose();
		this.xui.DragAndDropWindow.InMenu = false;
		Manager.PlayInsidePlayerHead("close_inventory", -1, 0f, false, false);
		if (this.xui.CurrentSelectedEntry != null)
		{
			this.xui.CurrentSelectedEntry.IsSelected = false;
		}
	}

	// Token: 0x060088C9 RID: 35017 RVA: 0x0034616C File Offset: 0x0034436C
	[PublicizedFrom(EAccessModifier.Private)]
	public void openSelectorAndWindow(string _selectedPage)
	{
		_selectedPage = _selectedPage.ToLower();
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
		if (this.xui.playerUI.windowManager.IsWindowOpen("twitchWindowpaging") && this.SelectedName.EqualsCaseInsensitive(_selectedPage) && !this.OverrideClose)
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
			if (this.xui.playerUI.windowManager.IsWindowOpen("twitchWindowpaging"))
			{
				this.xui.playerUI.windowManager.Close("twitchWindowpaging");
				return;
			}
		}
		else
		{
			this.SetSelected(_selectedPage);
			if (this.xui.playerUI.windowManager.IsWindowOpen("twitchWindowpaging"))
			{
				this.OpenSelectedWindow();
				return;
			}
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
			this.xui.playerUI.windowManager.Open("twitchWindowpaging", false);
		}
	}

	// Token: 0x060088CA RID: 35018 RVA: 0x00346275 File Offset: 0x00344475
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage, bool _extras = false)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		XUiC_TwitchWindowSelector.extras = _extras;
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x060088CB RID: 35019 RVA: 0x003462A8 File Offset: 0x003444A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void toggleCategory(int _dir)
	{
		int num = NGUIMath.RepeatIndex(this.currentCategoryIndex + _dir, this.categories.Count);
		XUiController childById = base.GetChildById(this.categories[num]);
		if (((childById != null) ? childById.ViewComponent : null) is XUiV_Button)
		{
			if (childById.ViewComponent.IsVisible)
			{
				this.SetSelected(this.categories[num]);
				this.OpenSelectedWindow();
				return;
			}
			this.currentCategoryIndex = num;
			this.toggleCategory(_dir);
		}
	}

	// Token: 0x060088CC RID: 35020 RVA: 0x00346328 File Offset: 0x00344528
	public static void ToggleCategory(EntityPlayerLocal _localPlayer, int _dir)
	{
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().toggleCategory(_dir);
	}

	// Token: 0x060088CD RID: 35021 RVA: 0x0034634C File Offset: 0x0034454C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (this.xui.playerUI.windowManager.IsKeyShortcutsAllowed())
		{
			if (this.xui.playerUI.playerInput.GUIActions.WindowPagingLeft.WasReleased && windowManager.IsWindowOpen(XUiC_TwitchWindowSelector.ID))
			{
				XUiC_TwitchWindowSelector.ToggleCategory(this.xui.playerUI.entityPlayer, -1);
			}
			if (this.xui.playerUI.playerInput.GUIActions.WindowPagingRight.WasReleased && windowManager.IsWindowOpen(XUiC_TwitchWindowSelector.ID))
			{
				XUiC_TwitchWindowSelector.ToggleCategory(this.xui.playerUI.entityPlayer, 1);
			}
		}
		this.OverrideClose = false;
	}

	// Token: 0x040065F1 RID: 26097
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;

	// Token: 0x040065F2 RID: 26098
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button selected;

	// Token: 0x040065F3 RID: 26099
	public static string ID = "";

	// Token: 0x040065F4 RID: 26100
	public bool OverrideClose;

	// Token: 0x040065F5 RID: 26101
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> categories = new List<string>();

	// Token: 0x040065F6 RID: 26102
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCategoryIndex;

	// Token: 0x040065F7 RID: 26103
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool extras = false;
}
