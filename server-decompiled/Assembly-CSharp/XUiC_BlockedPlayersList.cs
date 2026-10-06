using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DFE RID: 3582
[Preserve]
public class XUiC_BlockedPlayersList : XUiController
{
	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x06006D28 RID: 27944 RVA: 0x002AA53F File Offset: 0x002A873F
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

	// Token: 0x17000C33 RID: 3123
	// (get) Token: 0x06006D29 RID: 27945 RVA: 0x00178B59 File Offset: 0x00176D59
	[XuiXmlBinding("blockedmax")]
	public int MaxBlockedEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 500;
		}
	}

	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x06006D2A RID: 27946 RVA: 0x002AA556 File Offset: 0x002A8756
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

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x06006D2B RID: 27947 RVA: 0x002AA56D File Offset: 0x002A876D
	[XuiXmlBinding("recentmax")]
	public int MaxRecentEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 100;
		}
	}

	// Token: 0x06006D2C RID: 27948 RVA: 0x002AA574 File Offset: 0x002A8774
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

	// Token: 0x06006D2D RID: 27949 RVA: 0x002AA5D5 File Offset: 0x002A87D5
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

	// Token: 0x06006D2E RID: 27950 RVA: 0x002AA608 File Offset: 0x002A8808
	public void DisplayMessage(string _header, string _message)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, _header, _message, "", null, false, false, false);
	}

	// Token: 0x06006D2F RID: 27951 RVA: 0x002AA620 File Offset: 0x002A8820
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

	// Token: 0x06006D30 RID: 27952 RVA: 0x002AA768 File Offset: 0x002A8968
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

	// Token: 0x04005378 RID: 21368
	[XuiBindComponent("blockList", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid blockedPlayerList;

	// Token: 0x04005379 RID: 21369
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PlayersBlockedListEntry[] blockedEntries;

	// Token: 0x0400537A RID: 21370
	[XuiBindComponent("blockedPager", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Paging blockedPager;

	// Token: 0x0400537B RID: 21371
	[XuiBindComponent("recentList", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid recentPlayerList;

	// Token: 0x0400537C RID: 21372
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PlayersRecentListEntry[] recentEntries;

	// Token: 0x0400537D RID: 21373
	[XuiBindComponent("recentPager", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Paging recentPager;
}
