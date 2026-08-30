using System;
using System.Collections.Generic;
using Audio;
using UnityEngine.Scripting;

// Token: 0x020010AD RID: 4269
[Preserve]
public class XUiC_TwitchWindowSelector : XUiController
{
	// Token: 0x17000FDF RID: 4063
	// (get) Token: 0x0600881C RID: 34844 RVA: 0x0033FF5E File Offset: 0x0033E15E
	// (set) Token: 0x0600881D RID: 34845 RVA: 0x0033FF66 File Offset: 0x0033E166
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

	// Token: 0x17000FE0 RID: 4064
	// (get) Token: 0x0600881E RID: 34846 RVA: 0x0033FF9D File Offset: 0x0033E19D
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

	// Token: 0x0600881F RID: 34847 RVA: 0x0033FFB8 File Offset: 0x0033E1B8
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

	// Token: 0x06008820 RID: 34848 RVA: 0x0034006E File Offset: 0x0033E26E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnPress(XUiController _sender, int _mouseButton)
	{
		this.Selected = (XUiV_Button)_sender.ViewComponent;
		this.OpenSelectedWindow();
	}

	// Token: 0x06008821 RID: 34849 RVA: 0x00340087 File Offset: 0x0033E287
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleSelectedChange()
	{
		this.updateWindowTitle();
	}

	// Token: 0x06008822 RID: 34850 RVA: 0x0034008F File Offset: 0x0033E28F
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateWindowTitle()
	{
		this.lblWindowName.Text = ((this.selected != null) ? Localization.Get("TwitchInfo_" + this.Selected.ID, false, null) : "");
	}

	// Token: 0x06008823 RID: 34851 RVA: 0x003400C8 File Offset: 0x0033E2C8
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

	// Token: 0x06008824 RID: 34852 RVA: 0x003401E4 File Offset: 0x0033E3E4
	public void SetSelected(string name)
	{
		XUiController childById = base.GetChildById(name.ToLower());
		if (((childById != null) ? childById.ViewComponent : null) is XUiV_Button)
		{
			this.Selected = (XUiV_Button)childById.ViewComponent;
			this.currentCategoryIndex = this.categories.IndexOf(this.Selected.ID.ToLower());
		}
	}

	// Token: 0x06008825 RID: 34853 RVA: 0x00340243 File Offset: 0x0033E443
	public override void OnOpen()
	{
		base.OnOpen();
		this.OpenSelectedWindow();
		this.xui.DragAndDropWindow.InMenu = true;
		Manager.PlayInsidePlayerHead("open_inventory", -1, 0f, false, false);
		base.RefreshBindings();
	}

	// Token: 0x06008826 RID: 34854 RVA: 0x0034027C File Offset: 0x0033E47C
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

	// Token: 0x06008827 RID: 34855 RVA: 0x003402D0 File Offset: 0x0033E4D0
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

	// Token: 0x06008828 RID: 34856 RVA: 0x003403D9 File Offset: 0x0033E5D9
	public static void OpenSelectorAndWindow(EntityPlayerLocal _localPlayer, string _selectedPage, bool _extras = false)
	{
		if (_localPlayer.IsDead())
		{
			return;
		}
		XUiC_TwitchWindowSelector.extras = _extras;
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().openSelectorAndWindow(_selectedPage);
	}

	// Token: 0x06008829 RID: 34857 RVA: 0x0034040C File Offset: 0x0033E60C
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

	// Token: 0x0600882A RID: 34858 RVA: 0x0034048C File Offset: 0x0033E68C
	public static void ToggleCategory(EntityPlayerLocal _localPlayer, int _dir)
	{
		LocalPlayerUI.GetUIForPlayer(_localPlayer).xui.FindWindowGroupByName("twitchWindowpaging").GetChildByType<XUiC_TwitchWindowSelector>().toggleCategory(_dir);
	}

	// Token: 0x0600882B RID: 34859 RVA: 0x003404B0 File Offset: 0x0033E6B0
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

	// Token: 0x04006570 RID: 25968
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblWindowName;

	// Token: 0x04006571 RID: 25969
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button selected;

	// Token: 0x04006572 RID: 25970
	public static string ID = "";

	// Token: 0x04006573 RID: 25971
	public bool OverrideClose;

	// Token: 0x04006574 RID: 25972
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> categories = new List<string>();

	// Token: 0x04006575 RID: 25973
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCategoryIndex;

	// Token: 0x04006576 RID: 25974
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool extras = false;
}
