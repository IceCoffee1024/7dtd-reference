using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020010BE RID: 4286
[Preserve]
public class XUiC_TwitchWindowSelector : XUiController
{
	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x0600886B RID: 34923 RVA: 0x00343D76 File Offset: 0x00341F76
	// (set) Token: 0x0600886C RID: 34924 RVA: 0x00343D7E File Offset: 0x00341F7E
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

	// Token: 0x17001015 RID: 4117
	// (get) Token: 0x0600886D RID: 34925 RVA: 0x00343DB5 File Offset: 0x00341FB5
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

	// Token: 0x0600886E RID: 34926 RVA: 0x00343DD0 File Offset: 0x00341FD0
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

	// Token: 0x0600886F RID: 34927 RVA: 0x00343E86 File Offset: 0x00342086
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnPress(XUiController _sender, int _mouseButton)
	{
		this.Selected = (XUiV_Button)_sender.ViewComponent;
		this.OpenSelectedWindow();
	}

	// Token: 0x06008870 RID: 34928 RVA: 0x00343E9F File Offset: 0x0034209F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleSelectedChange()
	{
		this.updateWindowTitle();
	}

	// Token: 0x06008871 RID: 34929 RVA: 0x00343EA7 File Offset: 0x003420A7
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateWindowTitle()
	{
		this.lblWindowName.Text = ((this.selected != null) ? Localization.Get("TwitchInfo_" + this.Selected.ID, false, null) : "");
	}

	// Token: 0x06008872 RID: 34930 RVA: 0x00343EE0 File Offset: 0x003420E0
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

	// Token: 0x06008873 RID: 34931 RVA: 0x00343FFC File Offset: 0x003421FC
	public void SetSelected(string name)
	{
		XUiController childById = base.GetChildById(name.ToLower());
		if (((childById != null) ? childById.ViewComponent : null) is XUiV_Button)
		{
			this.Selected = (XUiV_Button)childById.ViewComponent;
			this.currentCategoryIndex = this.categories.IndexOf(this.Selected.ID.ToLower());
		}
	}

	// Token: 0x06008874 RID: 34932 RVA: 0x0034405B File Offset: 0x0034225B
	public override void OnOpen()
	{
		base.OnOpen();
		this.OpenSelectedWindow();
		this.xui.DragAndDropWindow.InMenu = true;
		Manager.PlayInsidePlayerHead("open_inventory", -1, 0f, false, false);
		base.RefreshBindings();
	}

	// Token: 0x06008875 RID: 34933 RVA: 0x00344094 File Offset: 0x00342294
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

	// Token: 0x06008876 RID: 34934 RVA: 0x003440E8 File Offset: 0x003422E8
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

	// Token: 0x06008877 RID: 34935 RVA: 0x003441F1 File Offset: 0x003423F1
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage, bool _extras = false)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		XUiC_TwitchWindowSelector.extras = _extras;
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x06008878 RID: 34936 RVA: 0x00344224 File Offset: 0x00342424
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

	// Token: 0x06008879 RID: 34937 RVA: 0x003442A4 File Offset: 0x003424A4
	public static void ToggleCategory(EntityPlayerLocal _localPlayer, int _dir)
	{
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().toggleCategory(_dir);
	}

	// Token: 0x0600887A RID: 34938 RVA: 0x003442C8 File Offset: 0x003424C8
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

	// Token: 0x040065B2 RID: 26034
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;

	// Token: 0x040065B3 RID: 26035
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button selected;

	// Token: 0x040065B4 RID: 26036
	public static string ID = "";

	// Token: 0x040065B5 RID: 26037
	public bool OverrideClose;

	// Token: 0x040065B6 RID: 26038
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> categories = new List<string>();

	// Token: 0x040065B7 RID: 26039
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCategoryIndex;

	// Token: 0x040065B8 RID: 26040
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool extras = false;
}
