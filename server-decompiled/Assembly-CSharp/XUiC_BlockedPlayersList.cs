using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DEF RID: 3567
[Preserve]
public class XUiC_BlockedPlayersList : XUiController
{
	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x06006D30 RID: 27952 RVA: 0x002A71AF File Offset: 0x002A53AF
	[XuiXmlBinding("blockedcount")]
	public int BlockedEntriesCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (BlockedPlayerList.Instance != null)
			{
				return BlockedPlayerList.Instance.EntryCount(true, false);
			}
			return 0;
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x06006D31 RID: 27953 RVA: 0x001783A1 File Offset: 0x001765A1
	[XuiXmlBinding("blockedmax")]
	public int MaxBlockedEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 500;
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x06006D32 RID: 27954 RVA: 0x002A71C6 File Offset: 0x002A53C6
	[XuiXmlBinding("recentcount")]
	public int RecentEntriesCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (BlockedPlayerList.Instance != null)
			{
				return BlockedPlayerList.Instance.EntryCount(false, false);
			}
			return 0;
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x06006D33 RID: 27955 RVA: 0x002A71DD File Offset: 0x002A53DD
	[XuiXmlBinding("recentmax")]
	public int MaxRecentEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 100;
		}
	}

	// Token: 0x06006D34 RID: 27956 RVA: 0x002A71E4 File Offset: 0x002A53E4
	public override void OnOpen()
	{
		base.OnOpen();
		if (BlockedPlayerList.Instance == null)
		{
			return;
		}
		this.blockedPager.Reset();
		this.recentPager.Reset();
		BlockedPlayerList.Instance.UpdatePlayersSeenInWorld(GameManager.Instance.World);
		ThreadManager.StartCoroutine(BlockedPlayerList.Instance.ResolveUserDetails());
		this.updateBlockedList();
		this.updateRecentList();
	}

	// Token: 0x06006D35 RID: 27957 RVA: 0x002A7245 File Offset: 0x002A5445
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (BlockedPlayerList.Instance == null)
		{
			return;
		}
		if (!this.IsDirty)
		{
			return;
		}
		this.IsDirty = false;
		this.updateBlockedList();
		this.updateRecentList();
		base.RefreshBindings();
	}

	// Token: 0x06006D36 RID: 27958 RVA: 0x002A7278 File Offset: 0x002A5478
	public void DisplayMessage(string _header, string _message)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, _header, _message, "", null, false, false, false);
	}

	// Token: 0x06006D37 RID: 27959 RVA: 0x002A7290 File Offset: 0x002A5490
	[XuiBindEvent("OnPageChanged", "blockedPager")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBlockedList()
	{
		for (int i = 0; i < this.blockedPlayerList.Rows; i++)
		{
			this.blockedEntries[i].Clear();
		}
		if (BlockedPlayerList.Instance.PendingResolve())
		{
			this.blockedEntries[0].PlayerName.SetGenericName(Localization.Get("xuiFetchingData", false, null));
			this.IsDirty = true;
			return;
		}
		this.blockedPager.SetLastPageByElementsAndPageLength(BlockedPlayerList.Instance.EntryCount(true, true), this.blockedPlayerList.Rows);
		int num = this.blockedPlayerList.Rows * this.blockedPager.GetPage();
		int num2 = 0;
		foreach (BlockedPlayerList.ListEntry listEntry in BlockedPlayerList.Instance.GetEntriesOrdered(true, true))
		{
			if (num2 < num)
			{
				num2++;
			}
			else
			{
				int num3 = num2 - num;
				if (num3 >= this.blockedPlayerList.Rows)
				{
					break;
				}
				this.blockedEntries[num3].UpdateEntry(listEntry.PlayerData.PrimaryId);
				num2++;
			}
		}
		for (int j = num2 - num; j < this.blockedPlayerList.Rows; j++)
		{
			this.blockedEntries[j].Clear();
		}
	}

	// Token: 0x06006D38 RID: 27960 RVA: 0x002A73D8 File Offset: 0x002A55D8
	[XuiBindEvent("OnPageChanged", "recentPager")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRecentList()
	{
		for (int i = 0; i < this.recentPlayerList.Rows; i++)
		{
			this.recentEntries[i].Clear();
		}
		if (BlockedPlayerList.Instance.PendingResolve())
		{
			this.recentEntries[0].PlayerName.SetGenericName(Localization.Get("xuiFetchingData", false, null));
			this.IsDirty = true;
			return;
		}
		this.recentPager.SetLastPageByElementsAndPageLength(BlockedPlayerList.Instance.EntryCount(false, true), this.recentPlayerList.Rows);
		int num = this.recentPlayerList.Rows * this.recentPager.GetPage();
		int num2 = 0;
		foreach (BlockedPlayerList.ListEntry listEntry in BlockedPlayerList.Instance.GetEntriesOrdered(false, true))
		{
			if (num2 < num)
			{
				num2++;
			}
			else
			{
				int num3 = num2 - num;
				if (num3 >= this.recentPlayerList.Rows)
				{
					break;
				}
				this.recentEntries[num3].UpdateEntry(listEntry.PlayerData.PrimaryId);
				num2++;
			}
		}
		for (int j = num2 - num; j < this.recentPlayerList.Rows; j++)
		{
			this.recentEntries[j].Clear();
		}
	}

	// Token: 0x04005341 RID: 21313
	[XuiBindComponent("blockList", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid blockedPlayerList;

	// Token: 0x04005342 RID: 21314
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PlayersBlockedListEntry[] blockedEntries;

	// Token: 0x04005343 RID: 21315
	[XuiBindComponent("blockedPager", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Paging blockedPager;

	// Token: 0x04005344 RID: 21316
	[XuiBindComponent("recentList", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid recentPlayerList;

	// Token: 0x04005345 RID: 21317
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PlayersRecentListEntry[] recentEntries;

	// Token: 0x04005346 RID: 21318
	[XuiBindComponent("recentPager", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Paging recentPager;
}
