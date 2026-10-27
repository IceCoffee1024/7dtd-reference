using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E09 RID: 3593
[Preserve]
public class XUiC_BlockedPlayersList : XUiController
{
	// Token: 0x17000C38 RID: 3128
	// (get) Token: 0x06006D4D RID: 27981 RVA: 0x002ABA67 File Offset: 0x002A9C67
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

	// Token: 0x17000C39 RID: 3129
	// (get) Token: 0x06006D4E RID: 27982 RVA: 0x00179AF1 File Offset: 0x00177CF1
	[XuiXmlBinding("blockedmax")]
	public int MaxBlockedEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 500;
		}
	}

	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x06006D4F RID: 27983 RVA: 0x002ABA7E File Offset: 0x002A9C7E
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

	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06006D50 RID: 27984 RVA: 0x002ABA95 File Offset: 0x002A9C95
	[XuiXmlBinding("recentmax")]
	public int MaxRecentEntries
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return 100;
		}
	}

	// Token: 0x06006D51 RID: 27985 RVA: 0x002ABA9C File Offset: 0x002A9C9C
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

	// Token: 0x06006D52 RID: 27986 RVA: 0x002ABAFD File Offset: 0x002A9CFD
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

	// Token: 0x06006D53 RID: 27987 RVA: 0x002ABB30 File Offset: 0x002A9D30
	public void DisplayMessage(string _header, string _message)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, _header, _message, "", null, false, false, false);
	}

	// Token: 0x06006D54 RID: 27988 RVA: 0x002ABB48 File Offset: 0x002A9D48
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

	// Token: 0x06006D55 RID: 27989 RVA: 0x002ABC90 File Offset: 0x002A9E90
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

	// Token: 0x0400539D RID: 21405
	[XuiBindComponent("blockList", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid blockedPlayerList;

	// Token: 0x0400539E RID: 21406
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PlayersBlockedListEntry[] blockedEntries;

	// Token: 0x0400539F RID: 21407
	[XuiBindComponent("blockedPager", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Paging blockedPager;

	// Token: 0x040053A0 RID: 21408
	[XuiBindComponent("recentList", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid recentPlayerList;

	// Token: 0x040053A1 RID: 21409
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_PlayersRecentListEntry[] recentEntries;

	// Token: 0x040053A2 RID: 21410
	[XuiBindComponent("recentPager", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Paging recentPager;
}
