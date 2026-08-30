using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02001094 RID: 4244
[Preserve]
public class XUiC_TwitchWindowSelector : XUiController
{
	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x0600872A RID: 34602 RVA: 0x0033B736 File Offset: 0x00339936
	// (set) Token: 0x0600872B RID: 34603 RVA: 0x0033B73E File Offset: 0x0033993E
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

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x0600872C RID: 34604 RVA: 0x0033B775 File Offset: 0x00339975
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

	// Token: 0x0600872D RID: 34605 RVA: 0x0033B790 File Offset: 0x00339990
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

	// Token: 0x0600872E RID: 34606 RVA: 0x0033B846 File Offset: 0x00339A46
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnPress(XUiController _sender, int _mouseButton)
	{
		this.Selected = (XUiV_Button)_sender.ViewComponent;
		this.OpenSelectedWindow();
	}

	// Token: 0x0600872F RID: 34607 RVA: 0x0033B85F File Offset: 0x00339A5F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleSelectedChange()
	{
		this.updateWindowTitle();
	}

	// Token: 0x06008730 RID: 34608 RVA: 0x0033B867 File Offset: 0x00339A67
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateWindowTitle()
	{
		this.lblWindowName.Text = ((this.selected != null) ? Localization.Get("TwitchInfo_" + this.Selected.ID, false, null) : "");
	}

	// Token: 0x06008731 RID: 34609 RVA: 0x0033B8A0 File Offset: 0x00339AA0
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

	// Token: 0x06008732 RID: 34610 RVA: 0x0033B9BC File Offset: 0x00339BBC
	public void SetSelected(string name)
	{
		XUiController childById = base.GetChildById(name.ToLower());
		if (((childById != null) ? childById.ViewComponent : null) is XUiV_Button)
		{
			this.Selected = (XUiV_Button)childById.ViewComponent;
			this.currentCategoryIndex = this.categories.IndexOf(this.Selected.ID.ToLower());
		}
	}

	// Token: 0x06008733 RID: 34611 RVA: 0x0033BA1B File Offset: 0x00339C1B
	public override void OnOpen()
	{
		base.OnOpen();
		this.OpenSelectedWindow();
		this.xui.DragAndDropWindow.InMenu = true;
		Manager.PlayInsidePlayerHead("open_inventory", -1, 0f, false, false);
		base.RefreshBindings();
	}

	// Token: 0x06008734 RID: 34612 RVA: 0x0033BA54 File Offset: 0x00339C54
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

	// Token: 0x06008735 RID: 34613 RVA: 0x0033BAA8 File Offset: 0x00339CA8
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

	// Token: 0x06008736 RID: 34614 RVA: 0x0033BBB1 File Offset: 0x00339DB1
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage, bool _extras = false)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		XUiC_TwitchWindowSelector.extras = _extras;
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x06008737 RID: 34615 RVA: 0x0033BBE4 File Offset: 0x00339DE4
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

	// Token: 0x06008738 RID: 34616 RVA: 0x0033BC64 File Offset: 0x00339E64
	public static void ToggleCategory(EntityPlayerLocal _localPlayer, int _dir)
	{
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().toggleCategory(_dir);
	}

	// Token: 0x06008739 RID: 34617 RVA: 0x0033BC88 File Offset: 0x00339E88
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

	// Token: 0x040064A9 RID: 25769
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;

	// Token: 0x040064AA RID: 25770
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button selected;

	// Token: 0x040064AB RID: 25771
	public static string ID = "";

	// Token: 0x040064AC RID: 25772
	public bool OverrideClose;

	// Token: 0x040064AD RID: 25773
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> categories = new List<string>();

	// Token: 0x040064AE RID: 25774
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCategoryIndex;

	// Token: 0x040064AF RID: 25775
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool extras = false;
}
