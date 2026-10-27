using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010CB RID: 4299
[Preserve]
public class XUiC_TwitchVoteInfoEntryList : XUiController
{
	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x06008894 RID: 34964 RVA: 0x00344351 File Offset: 0x00342551
	// (set) Token: 0x06008895 RID: 34965 RVA: 0x00344359 File Offset: 0x00342559
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			if (this.page != value)
			{
				this.page = value;
				this.IsDirty = true;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x06008896 RID: 34966 RVA: 0x00344388 File Offset: 0x00342588
	// (set) Token: 0x06008897 RID: 34967 RVA: 0x00344390 File Offset: 0x00342590
	public XUiC_TwitchVoteInfoEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = true;
			}
		}
	}

	// Token: 0x06008898 RID: 34968 RVA: 0x003443C4 File Offset: 0x003425C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		if (this.entryList[0].Vote != null)
		{
			this.SelectedEntry = this.entryList[0];
			this.entryList[0].SelectCursorElement(true, false);
		}
		else
		{
			this.SelectedEntry = null;
			base.WindowGroup.Controller.GetChildById("searchControls").SelectCursorElement(true, false);
		}
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).SetEntry(this.selectedEntry);
	}

	// Token: 0x06008899 RID: 34969 RVA: 0x0034444C File Offset: 0x0034264C
	public override void Init()
	{
		base.Init();
		XUiC_TwitchInfoWindowGroup xuiC_TwitchInfoWindowGroup = (XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller;
		XUiController childById = xuiC_TwitchInfoWindowGroup.GetChildByType<XUiC_TwitchEntryDescriptionWindow>().GetChildById("btnEnable");
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_TwitchVoteInfoEntry)
			{
				XUiC_TwitchVoteInfoEntry xuiC_TwitchVoteInfoEntry = (XUiC_TwitchVoteInfoEntry)this.children[i];
				xuiC_TwitchVoteInfoEntry.Owner = this;
				xuiC_TwitchVoteInfoEntry.TwitchInfoUIHandler = xuiC_TwitchInfoWindowGroup;
				xuiC_TwitchVoteInfoEntry.OnScroll += this.OnScrollEntry;
				xuiC_TwitchVoteInfoEntry.ViewComponent.NavRightTarget = childById.ViewComponent;
				this.entryList.Add(xuiC_TwitchVoteInfoEntry);
			}
		}
		this.pager = base.Parent.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				if (this.viewComponent.IsVisible)
				{
					this.Page = this.pager.CurrentPageNumber;
				}
			};
		}
	}

	// Token: 0x0600889A RID: 34970 RVA: 0x0034452C File Offset: 0x0034272C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			Log.Out("Vote list update");
			if (this.entryList != null)
			{
				for (int i = 0; i < this.entryList.Count; i++)
				{
					int num = i + this.entryList.Count * this.page;
					XUiC_TwitchVoteInfoEntry xuiC_TwitchVoteInfoEntry = this.entryList[i];
					if (xuiC_TwitchVoteInfoEntry != null)
					{
						xuiC_TwitchVoteInfoEntry.OnPress -= this.OnPressEntry;
						xuiC_TwitchVoteInfoEntry.Selected = false;
						if (num < this.voteList.Count)
						{
							xuiC_TwitchVoteInfoEntry.Vote = this.voteList[num];
							xuiC_TwitchVoteInfoEntry.OnPress += this.OnPressEntry;
							xuiC_TwitchVoteInfoEntry.ViewComponent.SoundPlayOnClick = true;
							xuiC_TwitchVoteInfoEntry.ViewComponent.IsNavigatable = true;
						}
						else
						{
							xuiC_TwitchVoteInfoEntry.Vote = null;
							xuiC_TwitchVoteInfoEntry.ViewComponent.SoundPlayOnClick = false;
							xuiC_TwitchVoteInfoEntry.ViewComponent.IsNavigatable = false;
						}
					}
				}
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging != null)
				{
					xuiC_Paging.SetLastPageByElementsAndPageLength(this.voteList.Count, this.entryList.Count);
				}
			}
			if (this.setFirstEntry)
			{
				this.SetFirstEntry();
				this.setFirstEntry = false;
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x0600889B RID: 34971 RVA: 0x0034466C File Offset: 0x0034286C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchVoteInfoEntry xuiC_TwitchVoteInfoEntry = _sender as XUiC_TwitchVoteInfoEntry;
		if (xuiC_TwitchVoteInfoEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchVoteInfoEntry;
			this.SelectedEntry.TwitchInfoUIHandler.SetEntry(this.SelectedEntry);
		}
	}

	// Token: 0x0600889C RID: 34972 RVA: 0x003446A0 File Offset: 0x003428A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrollEntry(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging == null)
			{
				return;
			}
			xuiC_Paging.PageDown();
			return;
		}
		else
		{
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 == null)
			{
				return;
			}
			xuiC_Paging2.PageUp();
			return;
		}
	}

	// Token: 0x0600889D RID: 34973 RVA: 0x003446CD File Offset: 0x003428CD
	public void SetTwitchVoteList(List<TwitchVote> newVoteEntryList)
	{
		this.Page = 0;
		this.voteList = newVoteEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x0600889E RID: 34974 RVA: 0x003446EB File Offset: 0x003428EB
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x0600889F RID: 34975 RVA: 0x00344710 File Offset: 0x00342910
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x040065C9 RID: 26057
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040065CA RID: 26058
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchVoteInfoEntry> entryList = new List<XUiC_TwitchVoteInfoEntry>();

	// Token: 0x040065CB RID: 26059
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040065CC RID: 26060
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchVoteInfoEntry selectedEntry;

	// Token: 0x040065CD RID: 26061
	public bool setFirstEntry = true;

	// Token: 0x040065CE RID: 26062
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchVote> voteList = new List<TwitchVote>();

	// Token: 0x040065CF RID: 26063
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x040065D0 RID: 26064
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
