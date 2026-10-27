using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D8F RID: 3471
[Preserve]
public class XUiC_DataManagement : XUiController
{
	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x06006895 RID: 26773 RVA: 0x00298950 File Offset: 0x00296B50
	[XuiXmlBinding("showbar")]
	public bool DataManagementBarEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.dataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		}
	}

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x06006896 RID: 26774 RVA: 0x00298961 File Offset: 0x00296B61
	// (set) Token: 0x06006897 RID: 26775 RVA: 0x00298969 File Offset: 0x00296B69
	[XuiXmlBinding("savedeletable")]
	public bool SaveDeletable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveDeletable;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.saveDeletable = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06006898 RID: 26776 RVA: 0x00298979 File Offset: 0x00296B79
	// (set) Token: 0x06006899 RID: 26777 RVA: 0x00298981 File Offset: 0x00296B81
	[XuiXmlBinding("worlddeletable")]
	public bool WorldDeletable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.worldDeletable;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.worldDeletable = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x0600689A RID: 26778 RVA: 0x00298991 File Offset: 0x00296B91
	// (set) Token: 0x0600689B RID: 26779 RVA: 0x00298999 File Offset: 0x00296B99
	[XuiXmlBinding("playerdeletable")]
	public bool PlayerDeletable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.playerDeletable;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.playerDeletable = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x0600689C RID: 26780 RVA: 0x002989A9 File Offset: 0x00296BA9
	[XuiXmlBinding("worldmoveable")]
	public bool WorldMoveable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.worldMove.IsReady;
		}
	}

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x0600689D RID: 26781 RVA: 0x002989B6 File Offset: 0x00296BB6
	[XuiXmlBinding("worldmovelabel")]
	public string WorldMoveLabel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.worldMove.IsReady)
			{
				return string.Empty;
			}
			return XUiC_DataManagement.storageTargetToBtnLabel(this.worldMove.moveToStorageType);
		}
	}

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x0600689E RID: 26782 RVA: 0x002989DB File Offset: 0x00296BDB
	// (set) Token: 0x0600689F RID: 26783 RVA: 0x002989E3 File Offset: 0x00296BE3
	[XuiXmlBinding("savecopyable")]
	public bool SaveCopyable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveCopyable;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.saveCopyable = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x060068A0 RID: 26784 RVA: 0x002989F3 File Offset: 0x00296BF3
	// (set) Token: 0x060068A1 RID: 26785 RVA: 0x002989FB File Offset: 0x00296BFB
	[XuiXmlBinding("savemanageable")]
	public bool SaveManageable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveManageable;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.saveManageable = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x060068A2 RID: 26786 RVA: 0x00298A0B File Offset: 0x00296C0B
	[XuiXmlBinding("blockedplayerscount")]
	public int BlockedPlayersCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_DMPlayersList xuiC_DMPlayersList = this.playersList;
			if (xuiC_DMPlayersList == null)
			{
				return 0;
			}
			return xuiC_DMPlayersList.BlockedPlayerCount;
		}
	}

	// Token: 0x060068A3 RID: 26787 RVA: 0x00298A1E File Offset: 0x00296C1E
	public override void Init()
	{
		base.Init();
		XUiC_DataManagement.id = base.WindowGroup.Id;
		this.IsDirty = true;
	}

	// Token: 0x060068A4 RID: 26788 RVA: 0x00298A40 File Offset: 0x00296C40
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.DataManagementBarEnabled)
		{
			this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
			this.dataManagementBar.SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion.None);
		}
		this.refresh();
		string @string = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		string string2 = GamePrefs.GetString(EnumGamePrefs.GameName);
		UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
		SaveInfoProvider.SaveEntryInfo saveEntryInfo;
		if (!SaveInfoProvider.Instance.TryGetLocalSaveEntry(@string, string2, @int, out saveEntryInfo) || !this.worldList.SelectByKey(saveEntryInfo.WorldEntry.WorldKey))
		{
			this.worldList.SelectedEntryIndex = -1;
			this.savesList.SelectedEntryIndex = -1;
			this.savesList.SetWorldFilter(string.Empty);
			this.playersList.ClearList(true);
		}
		else if (!this.savesList.SelectByName(GamePrefs.GetString(EnumGamePrefs.GameName)))
		{
			this.savesList.SelectedEntryIndex = -1;
			this.playersList.ClearList(true);
		}
		this.playersList.SelectedEntryIndex = -1;
		if (this.DataManagementBarEnabled)
		{
			this.dataManagementBar.SetSelectionDepth((this.savesList.SelectedEntryIndex == -1) ? XUiC_DataManagementBar.SelectionDepth.Primary : XUiC_DataManagementBar.SelectionDepth.Secondary);
		}
		this.refreshButtonStates();
		this.updateBarSelectionValues();
		this.IsDirty = true;
	}

	// Token: 0x060068A5 RID: 26789 RVA: 0x00298B67 File Offset: 0x00296D67
	public override void OnClose()
	{
		base.OnClose();
		this.waitingForCommit = false;
		this.commitDeadline = int.MaxValue;
		this.worldList.ClearList();
		this.savesList.ClearList();
		this.playersList.ClearList(true);
	}

	// Token: 0x060068A6 RID: 26790 RVA: 0x00298BA4 File Offset: 0x00296DA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void refresh()
	{
		SaveInfoProvider instance = SaveInfoProvider.Instance;
		this.worldList.RebuildList(instance.WorldEntryInfos, false);
		this.savesList.RebuildList(instance.SaveEntryInfos, false);
		if (this.DataManagementBarEnabled)
		{
			this.dataManagementBar.SetUsedBytes(instance.TotalUsedBytes);
			this.dataManagementBar.SetAllowanceBytes(instance.TotalAllowanceBytes);
		}
	}

	// Token: 0x060068A7 RID: 26791 RVA: 0x00298C08 File Offset: 0x00296E08
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.waitingForCommit)
		{
			if (!SaveDataUtils.SaveDataManager.IsCommitPending(this.commitToken))
			{
				this.waitingForCommit = false;
				XUiC_MessageBoxWindowGroup.Close(this.xui);
			}
			else if ((int)Time.unscaledTimeAsDouble >= this.commitDeadline)
			{
				this.commitDeadline = int.MaxValue;
				XUiC_MessageBoxWindowGroup.Close(this.xui);
				XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmCommitTitle", false, null), Localization.Get("xuiPleaseWait", false, null), "", delegate
				{
					this.waitingForCommit = false;
				}, false, false, false);
			}
		}
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased && (!this.waitingForCommit || this.commitDeadline >= 2147483647))
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.closeDataManagementWindow), 1);
			return;
		}
		if (this.playerEntryResolver != null && this.playerEntryResolver.IsComplete)
		{
			this.playersList.RebuildList(this.playerEntryResolver.pendingPlayerEntries, false);
			this.playerEntryResolver = null;
			this.IsDirty = true;
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060068A8 RID: 26792 RVA: 0x00298D40 File Offset: 0x00296F40
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_DMWorldList.ListEntry> _list, XUiC_DMWorldList.ListEntry _previousEntry, XUiC_DMWorldList.ListEntry _newEntry)
	{
		string text = "";
		if (_newEntry != null)
		{
			text = _newEntry.Key;
		}
		this.savesList.SetWorldFilter(text);
		this.savesList.ClearSelection();
		this.playersList.ClearSelection();
		this.playersList.ClearList(true);
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x060068A9 RID: 26793 RVA: 0x00298D98 File Offset: 0x00296F98
	[XuiBindEvent("SelectionChanged", "savesList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SavesList_SelectionChanged(XUiC_List<XUiC_DMSavegamesList.ListEntry> _list, XUiC_DMSavegamesList.ListEntry _previousEntry, XUiC_DMSavegamesList.ListEntry _newEntry)
	{
		if (_newEntry != null)
		{
			IEnumerable<SaveInfoProvider.PlayerEntryInfo> enumerable = SaveInfoProvider.Instance.SaveEntryInfos.Where<SaveInfoProvider.SaveEntryInfo>((SaveInfoProvider.SaveEntryInfo _entry) => _entry.WorldEntry.WorldKey.Equals(_newEntry.WorldKey) && _entry.Name.Equals(_newEntry.SaveName)).SelectMany<SaveInfoProvider.SaveEntryInfo, SaveInfoProvider.PlayerEntryInfo>((SaveInfoProvider.SaveEntryInfo _entry) => _entry.PlayerEntryInfos);
			this.playerEntryResolver = SaveInfoProvider.PlayerEntryInfoPlatformDataResolver.StartNew(enumerable);
			if (this.playerEntryResolver.IsComplete)
			{
				this.playersList.RebuildList(this.playerEntryResolver.pendingPlayerEntries, false);
				this.playerEntryResolver = null;
			}
			else
			{
				this.playersList.ShowLoading();
				this.playersList.ClearList(false);
			}
		}
		this.playersList.ClearSelection();
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x060068AA RID: 26794 RVA: 0x00298E64 File Offset: 0x00297064
	[XuiBindEvent("SelectionChanged", "playersList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayersList_SelectionChanged(XUiC_List<XUiC_DMPlayersList.ListEntry> _list, XUiC_DMPlayersList.ListEntry _previousEntry, XUiC_DMPlayersList.ListEntry _newEntry)
	{
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x060068AB RID: 26795 RVA: 0x00298E74 File Offset: 0x00297074
	[XuiBindEvent("OnEntryHovered", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_OnChildElementHovered(XUiC_DMWorldList.ListEntry _sender, bool _isOver)
	{
		if (!this.DataManagementBarEnabled)
		{
			return;
		}
		if (_isOver)
		{
			SaveInfoProvider.WorldEntryInfo worldEntryInfo = ((_sender != null) ? _sender.WorldEntryInfo : null);
			if (worldEntryInfo != null)
			{
				long num = worldEntryInfo.SaveDataSizeForLimit;
				if (worldEntryInfo.UsesDataLimit)
				{
					num += worldEntryInfo.WorldDataSize;
				}
				XUiC_DataManagementBar.BarRegion barRegion = new XUiC_DataManagementBar.BarRegion(worldEntryInfo.BarStartOffset, num);
				this.dataManagementBar.SetHoveredByteRegion(barRegion);
				goto IL_0060;
			}
		}
		this.dataManagementBar.SetHoveredByteRegion(XUiC_DataManagementBar.BarRegion.None);
		IL_0060:
		this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Primary);
	}

	// Token: 0x060068AC RID: 26796 RVA: 0x00298EF0 File Offset: 0x002970F0
	[XuiBindEvent("OnEntryHovered", "savesList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SavesList_OnChildElementHovered(XUiC_DMSavegamesList.ListEntry _sender, bool _isOver)
	{
		if (!this.DataManagementBarEnabled)
		{
			return;
		}
		if (_isOver)
		{
			SaveInfoProvider.SaveEntryInfo saveEntryInfo = ((_sender != null) ? _sender.SaveEntryInfo : null);
			if (saveEntryInfo != null)
			{
				SaveInfoProvider.SaveSizeInfo sizeInfo = saveEntryInfo.SizeInfo;
				if (sizeInfo.UsesDataLimit)
				{
					XUiC_DataManagementBar.BarRegion barRegion = new XUiC_DataManagementBar.BarRegion(saveEntryInfo.BarStartOffset, saveEntryInfo.SizeInfo.ReportedSize);
					this.dataManagementBar.SetHoveredByteRegion(barRegion);
					goto IL_0062;
				}
			}
		}
		this.dataManagementBar.SetHoveredByteRegion(XUiC_DataManagementBar.BarRegion.None);
		IL_0062:
		this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Secondary);
	}

	// Token: 0x060068AD RID: 26797 RVA: 0x00298F6C File Offset: 0x0029716C
	[XuiBindEvent("OnEntryHovered", "playersList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayersList_OnChildElementHovered(XUiC_DMPlayersList.ListEntry _sender, bool _isOver)
	{
		if (!this.DataManagementBarEnabled)
		{
			return;
		}
		if (_isOver)
		{
			SaveInfoProvider.PlayerEntryInfo playerEntryInfo = ((_sender != null) ? _sender.PlayerEntryInfo : null);
			if (playerEntryInfo != null)
			{
				XUiC_DataManagementBar.BarRegion barRegion = new XUiC_DataManagementBar.BarRegion(playerEntryInfo.BarStartOffset, playerEntryInfo.Size);
				this.dataManagementBar.SetHoveredByteRegion(barRegion);
				goto IL_004D;
			}
		}
		this.dataManagementBar.SetHoveredByteRegion(XUiC_DataManagementBar.BarRegion.None);
		IL_004D:
		this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Tertiary);
	}

	// Token: 0x060068AE RID: 26798 RVA: 0x00298FD2 File Offset: 0x002971D2
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeDataManagementWindow();
	}

	// Token: 0x060068AF RID: 26799 RVA: 0x00298FDC File Offset: 0x002971DC
	[XuiBindEvent("OnPress", "btnDeleteWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteWorld_OnPressed(XUiController _sender, int _mouseButton)
	{
		int num = this.savesList.GetSavesInWorld(this.worldList.SelectedEntryData.Key).Count<XUiC_DMSavegamesList.ListEntry>();
		XUiC_MessageBoxWindowGroup.ShowConfirmCancel(this.xui, Localization.Get("xuiWorldDelete", false, null), (num > 0) ? string.Format(Localization.Get("xuiWorldDeleteConfirmation", false, null), num) : Localization.Get("xuiWorldDeleteConfirmationNoSaves", false, null), "", delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.World, XUiC_ConfirmationPrompt.Result.Confirmed);
		}, delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.World, XUiC_ConfirmationPrompt.Result.Cancelled);
		}, 1.5f, false, false, false);
		this.dataManagementBar.SetDeleteWindowDisplayed(true);
	}

	// Token: 0x060068B0 RID: 26800 RVA: 0x0029907B File Offset: 0x0029727B
	[XuiBindEvent("OnHover", "btnDeleteWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteWorld_OnHover(XUiController _sender, bool _isOver)
	{
		if (this.DataManagementBarEnabled)
		{
			this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Primary);
			this.dataManagementBar.SetDeleteHovered(_isOver);
		}
	}

	// Token: 0x060068B1 RID: 26801 RVA: 0x002990A0 File Offset: 0x002972A0
	[XuiBindEvent("OnPress", "btnDeleteSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		bool flag = this.worldList.SelectedEntryData.Type == SaveInfoProvider.RemoteWorldsType;
		XUiC_MessageBoxWindowGroup.ShowConfirmCancel(this.xui, Localization.Get(flag ? "xuiDmDeleteRemoteSave" : "xuiDeleteSaveGame", false, null), Localization.Get(flag ? "xuiDmDeleteRemoteSaveConfirmation" : "xuiDeleteSaveGame", false, null), "", delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.Save, XUiC_ConfirmationPrompt.Result.Confirmed);
		}, delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.Save, XUiC_ConfirmationPrompt.Result.Cancelled);
		}, 1.5f, false, false, false);
		this.dataManagementBar.SetDeleteWindowDisplayed(true);
	}

	// Token: 0x060068B2 RID: 26802 RVA: 0x00299130 File Offset: 0x00297330
	[XuiBindEvent("OnHover", "btnDeleteSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteSave_OnHover(XUiController _sender, bool _isOver)
	{
		if (this.DataManagementBarEnabled)
		{
			this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Secondary);
			this.dataManagementBar.SetDeleteHovered(_isOver);
		}
	}

	// Token: 0x060068B3 RID: 26803 RVA: 0x00299154 File Offset: 0x00297354
	[XuiBindEvent("OnPress", "btnDeletePlayer")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeletePlayer_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_MessageBoxWindowGroup.ShowConfirmCancel(this.xui, Localization.Get("xuiDmDeletePlayer", false, null), Localization.Get("xuiDmDeletePlayerConfirmation", false, null), "", delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.Player, XUiC_ConfirmationPrompt.Result.Confirmed);
		}, delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.Player, XUiC_ConfirmationPrompt.Result.Cancelled);
		}, 1.5f, false, false, false);
		this.dataManagementBar.SetDeleteWindowDisplayed(true);
	}

	// Token: 0x060068B4 RID: 26804 RVA: 0x002991B5 File Offset: 0x002973B5
	[XuiBindEvent("OnHover", "btnDeletePlayer")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeletePlayer_OnHover(XUiController _sender, bool _isOver)
	{
		if (this.DataManagementBarEnabled)
		{
			this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Tertiary);
			this.dataManagementBar.SetDeleteHovered(_isOver);
		}
	}

	// Token: 0x060068B5 RID: 26805 RVA: 0x002991D8 File Offset: 0x002973D8
	[XuiBindEvent("OnPress", "btnDeleteBlockedPlayers")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteBlockedPlayers_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.playersList.ClearSelection();
		this.showBlockedPlayersInBar = true;
		this.updateBarSelectionValues();
		XUiC_MessageBoxWindowGroup.ShowConfirmCancel(this.xui, Localization.Get("xuiDmDeleteBlockedPlayers", false, null), Localization.Get("xuiDmDeletePlayerConfirmation", false, null), "", delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.BlockedPlayers, XUiC_ConfirmationPrompt.Result.Confirmed);
		}, delegate
		{
			this.processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode.BlockedPlayers, XUiC_ConfirmationPrompt.Result.Cancelled);
		}, 1.5f, false, false, false);
	}

	// Token: 0x060068B6 RID: 26806 RVA: 0x00299245 File Offset: 0x00297445
	[XuiBindEvent("OnHover", "btnDeleteBlockedPlayers")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteBlockedPlayers_OnHover(XUiController _sender, bool _isOver)
	{
		if (_isOver != this.showBlockedPlayersInBar)
		{
			this.showBlockedPlayersInBar = _isOver;
			this.updateBarSelectionValues();
		}
		if (this.DataManagementBarEnabled)
		{
			this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Tertiary);
			this.dataManagementBar.SetDeleteHovered(_isOver);
		}
	}

	// Token: 0x060068B7 RID: 26807 RVA: 0x0029927D File Offset: 0x0029747D
	[PublicizedFrom(EAccessModifier.Private)]
	public void processDeletionConfirmationResult(XUiC_DataManagement.DeletionMode _deletionMode, XUiC_ConfirmationPrompt.Result _result)
	{
		if (_result == XUiC_ConfirmationPrompt.Result.Confirmed)
		{
			this.processDeletion(_deletionMode);
			return;
		}
		this.cancelDeletion(_deletionMode);
	}

	// Token: 0x060068B8 RID: 26808 RVA: 0x00299294 File Offset: 0x00297494
	[PublicizedFrom(EAccessModifier.Private)]
	public void cancelDeletion(XUiC_DataManagement.DeletionMode _deletionMode)
	{
		XUiC_Button xuiC_Button;
		switch (_deletionMode)
		{
		case XUiC_DataManagement.DeletionMode.World:
			xuiC_Button = this.btnDeleteWorld;
			break;
		case XUiC_DataManagement.DeletionMode.Save:
			xuiC_Button = this.btnDeleteSave;
			break;
		case XUiC_DataManagement.DeletionMode.Player:
			xuiC_Button = this.btnDeletePlayer;
			break;
		default:
			xuiC_Button = null;
			break;
		}
		XUiController xuiController = xuiC_Button;
		this.dataManagementBar.SetDeleteWindowDisplayed(false);
		this.xui.playerUI.CursorController.SetNavigationLockView(this.viewComponent, (xuiController != null) ? xuiController.ViewComponent : null);
		this.showBlockedPlayersInBar = false;
		this.updateBarSelectionValues();
	}

	// Token: 0x060068B9 RID: 26809 RVA: 0x00299318 File Offset: 0x00297518
	[PublicizedFrom(EAccessModifier.Private)]
	public void processDeletion(XUiC_DataManagement.DeletionMode _deletionMode)
	{
		int num = this.worldList.SelectedEntryIndex;
		int num2 = this.savesList.SelectedEntryIndex;
		int num3 = this.playersList.SelectedEntryIndex;
		XUiController xuiController = null;
		switch (_deletionMode)
		{
		case XUiC_DataManagement.DeletionMode.World:
			try
			{
				XUiC_DMWorldList.ListEntry selectedEntryData = this.worldList.SelectedEntryData;
				if (selectedEntryData == null)
				{
					throw new NotSupportedException("Failed to retrieve selected world.");
				}
				if (!selectedEntryData.Deletable)
				{
					throw new NotSupportedException("Tried to delete non-generated world.");
				}
				if (selectedEntryData.Location == PathAbstractions.AbstractedLocation.None)
				{
					throw new NotSupportedException("Tried to delete world entry with location == none.");
				}
				GameUtils.DeleteWorld(selectedEntryData.Location);
				if (GamePrefs.GetString(EnumGamePrefs.GameWorld).EqualsCaseInsensitive(selectedEntryData.Location.Name))
				{
					GamePrefs.Set(EnumGamePrefs.GameWorld, "Navezgane");
				}
				this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
				num = Mathf.Clamp(num, -1, this.worldList.EntryCount - 1);
				num2 = -1;
				num3 = -1;
			}
			catch (Exception ex)
			{
				Log.Error("Error occurred while deleting world: \"" + ex.Message + "\"");
			}
			xuiController = this.btnDeleteWorld;
			goto IL_03E2;
		case XUiC_DataManagement.DeletionMode.Save:
			try
			{
				XUiC_DMSavegamesList.ListEntry selectedEntryData2 = this.savesList.SelectedEntryData;
				if (selectedEntryData2 == null)
				{
					throw new NotSupportedException("Failed to retrieve selected save.");
				}
				SdDirectory.Delete(selectedEntryData2.SaveDirectory, true);
				this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
				num2 = Mathf.Clamp(num2, -1, this.savesList.EntryCount - 1);
				num3 = -1;
			}
			catch (Exception ex2)
			{
				Log.Error("Error occurred while deleting save: \"" + ex2.Message + "\"");
			}
			xuiController = this.btnDeleteSave;
			goto IL_03E2;
		case XUiC_DataManagement.DeletionMode.Player:
			try
			{
				XUiC_DMPlayersList.ListEntry selectedEntryData3 = this.playersList.SelectedEntryData;
				if (selectedEntryData3 == null)
				{
					throw new Exception("Failed to retrieve selected player entry.");
				}
				XUiC_DMSavegamesList.ListEntry selectedEntryData4 = this.savesList.SelectedEntryData;
				if (selectedEntryData4 == null)
				{
					throw new Exception("Failed to retrieve selected save entry.");
				}
				string text = selectedEntryData4.SaveDirectory + "/Player";
				if (!SdDirectory.Exists(text))
				{
					throw new Exception("Player save data directory not found at expected path: " + text + ".");
				}
				foreach (SdFileSystemInfo sdFileSystemInfo in new SdDirectoryInfo(text).GetFileSystemInfos(selectedEntryData3.ID + "*"))
				{
					if (string.IsNullOrEmpty(sdFileSystemInfo.Extension) && SdDirectory.Exists(sdFileSystemInfo.FullName))
					{
						SdDirectory.Delete(sdFileSystemInfo.FullName, true);
					}
					else
					{
						SdFile.Delete(sdFileSystemInfo.FullName);
					}
				}
				this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
				num3 = Mathf.Clamp(num3, -1, this.playersList.EntryCount - 1);
			}
			catch (Exception ex3)
			{
				Log.Error("Error occurred while deleting player: \"" + ex3.Message + "\"");
			}
			xuiController = this.btnDeletePlayer;
			goto IL_03E2;
		case XUiC_DataManagement.DeletionMode.BlockedPlayers:
			try
			{
				XUiC_DMSavegamesList.ListEntry selectedEntryData5 = this.savesList.SelectedEntryData;
				if (selectedEntryData5 == null)
				{
					throw new Exception("Failed to retrieve selected save entry.");
				}
				string text2 = selectedEntryData5.SaveDirectory + "/Player";
				if (!SdDirectory.Exists(text2))
				{
					throw new Exception("Player save data directory not found at expected path: " + text2 + ".");
				}
				SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(text2);
				foreach (SaveInfoProvider.PlayerEntryInfo playerEntryInfo in this.playersList.BlockedPlayers)
				{
					try
					{
						SdFileSystemInfo[] array = sdDirectoryInfo.GetFileSystemInfos(playerEntryInfo.Id + "*");
						for (int i = 0; i < array.Length; i++)
						{
							SdFile.Delete(array[i].FullName);
						}
					}
					catch (Exception ex4)
					{
						Log.Error("Error occurred while deleting blocked player save files: " + ex4.Message);
					}
				}
				this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
			}
			catch (Exception ex5)
			{
				Log.Error("Error occurred while deleting blocked players: \"" + ex5.Message + "\"");
			}
			this.showBlockedPlayersInBar = false;
			goto IL_03E2;
		}
		global::UnityEngine.Debug.LogError("Error in internal logic: invalid deletion mode. No data will be deleted.");
		IL_03E2:
		this.waitForCommitIfPending();
		SaveInfoProvider.Instance.SetDirty();
		this.refresh();
		this.worldList.SelectedEntryIndex = num;
		this.savesList.SelectedEntryIndex = num2;
		this.playersList.SelectedEntryIndex = num3;
		this.dataManagementBar.SetDeleteWindowDisplayed(false);
		this.xui.playerUI.CursorController.SetNavigationLockView(this.viewComponent, (xuiController != null) ? xuiController.ViewComponent : null);
	}

	// Token: 0x060068BA RID: 26810 RVA: 0x002997C0 File Offset: 0x002979C0
	[Conditional("DELETION_LOG_ENABLED")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void logDeletion(string _v)
	{
		global::UnityEngine.Debug.Log(_v);
	}

	// Token: 0x060068BB RID: 26811 RVA: 0x002997C8 File Offset: 0x002979C8
	[XuiBindEvent("OnHover", "btnMoveWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnMoveWorld_OnHover(XUiController _sender, bool _isOver)
	{
		this.setWorldMoveBarPreviewActive(_isOver);
	}

	// Token: 0x060068BC RID: 26812 RVA: 0x002997D4 File Offset: 0x002979D4
	[XuiBindEvent("OnPress", "btnMoveWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnMoveWorld_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.worldMove.IsReady)
		{
			throw new Exception("Can not move world. No world selected for moving");
		}
		this.setWorldMoveBarPreviewActive(true);
		int countOfAssociatedSavesInSameStorage = this.worldMove.CountOfAssociatedSavesInSameStorage;
		XUiC_MessageBoxWindowGroup.ShowConfirmCancel(this.xui, Localization.Get(string.Format("xuiDmMoveWorldTitleTo{0}", this.worldMove.moveToStorageType), false, null), (countOfAssociatedSavesInSameStorage > 0) ? string.Format(Localization.Get(string.Format("xuiDmMoveWorldTo{0}", this.worldMove.moveToStorageType), false, null), countOfAssociatedSavesInSameStorage) : Localization.Get("xuiDmMoveWorldNoSaves", false, null), "", new Action(this.<BtnMoveWorld_OnPressed>g__OnConfirmationResult|77_1), delegate
		{
			this.setWorldMoveBarPreviewActive(false);
		}, 1.5f, false, false, false);
	}

	// Token: 0x060068BD RID: 26813 RVA: 0x0029989B File Offset: 0x00297A9B
	[XuiBindEvent("OnPress", "btnCopySave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopySave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openSaveManagementWindow(XUiC_SaveManagementPrompt.SaveManagementMode.Copy);
	}

	// Token: 0x060068BE RID: 26814 RVA: 0x002998A4 File Offset: 0x00297AA4
	[XuiBindEvent("OnPress", "btnManageSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnManageSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openSaveManagementWindow(XUiC_SaveManagementPrompt.SaveManagementMode.Manage);
	}

	// Token: 0x060068BF RID: 26815 RVA: 0x002998B0 File Offset: 0x00297AB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void openSaveManagementWindow(XUiC_SaveManagementPrompt.SaveManagementMode _mode)
	{
		XUiC_DMSavegamesList xuiC_DMSavegamesList = this.savesList;
		XUiC_DMSavegamesList.ListEntry listEntry = ((xuiC_DMSavegamesList != null) ? xuiC_DMSavegamesList.SelectedEntryData : null);
		if (listEntry == null)
		{
			return;
		}
		SaveInfoProvider.SaveEntryInfo saveEntryInfo = listEntry.SaveEntryInfo;
		SaveInfoProvider.WorldEntryInfo worldEntry = saveEntryInfo.WorldEntry;
		XUiC_SaveManagementPrompt.Show(this.xui, this.dataManagementBar, _mode, saveEntryInfo, worldEntry, new XUiC_SaveManagementPrompt.SaveManagementActionCopy(this.OnSaveCopyCompleted), new XUiC_SaveManagementPrompt.SaveManagementActionMove(this.OnSaveMoveCompleted), new XUiC_SaveManagementPrompt.SaveManagementActionApply(this.OnSaveApplyCompleted));
	}

	// Token: 0x060068C0 RID: 26816 RVA: 0x0029991C File Offset: 0x00297B1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSaveMoveCompleted(UserDataManagement.Result _result, string _targetSaveName, string _worldName, UserDataStorageType _expectedWorldStorage)
	{
		this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
		if (_result != UserDataManagement.Result.TargetAlreadyExists)
		{
			if (_result == UserDataManagement.Result.Exception)
			{
				XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmSaveMoveErrorTitle", false, null), Localization.Get("xuiDmSaveManageErrorFailed", false, null), "", null, false, false, false);
			}
		}
		else
		{
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmSaveMoveErrorTitle", false, null), Localization.Get("xuiDmSaveManageErrorExists", false, null), "", null, false, false, false);
		}
		this.waitForCommitIfPending();
		this.refresh();
		string worldEntryKey = SaveInfoProvider.GetWorldEntryKey(_worldName, _expectedWorldStorage);
		if (this.worldList.SelectByKey(worldEntryKey))
		{
			this.savesList.SelectByName(_targetSaveName);
		}
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x060068C1 RID: 26817 RVA: 0x002999DC File Offset: 0x00297BDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSaveApplyCompleted()
	{
		this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
		this.waitForCommitIfPending();
		int selectedEntryIndex = this.worldList.SelectedEntryIndex;
		int selectedEntryIndex2 = this.savesList.SelectedEntryIndex;
		this.refresh();
		this.worldList.SelectedEntryIndex = selectedEntryIndex;
		this.savesList.SelectedEntryIndex = selectedEntryIndex2;
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x060068C2 RID: 26818 RVA: 0x00299A44 File Offset: 0x00297C44
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSaveCopyCompleted(UserDataManagement.Result _result, string _targetSaveName, string _worldName, UserDataStorageType _expectedWorldStorage)
	{
		this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
		if (_result != UserDataManagement.Result.TargetAlreadyExists)
		{
			if (_result == UserDataManagement.Result.Exception)
			{
				XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmSaveCopyErrorTitle", false, null), Localization.Get("xuiDmSaveManageErrorFailed", false, null), "", null, false, false, false);
			}
		}
		else
		{
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmSaveCopyErrorTitle", false, null), Localization.Get("xuiDmSaveManageErrorExists", false, null), "", null, false, false, false);
		}
		this.waitForCommitIfPending();
		this.refresh();
		string worldEntryKey = SaveInfoProvider.GetWorldEntryKey(_worldName, _expectedWorldStorage);
		if (this.worldList.SelectByKey(worldEntryKey))
		{
			this.savesList.SelectByName(_targetSaveName);
		}
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x060068C3 RID: 26819 RVA: 0x00299B04 File Offset: 0x00297D04
	[PublicizedFrom(EAccessModifier.Private)]
	public void waitForCommitIfPending()
	{
		if (!SaveDataUtils.SaveDataManager.IsCommitPending(this.commitToken))
		{
			return;
		}
		this.waitingForCommit = true;
		this.commitDeadline = (int)Time.unscaledTimeAsDouble + XUiC_DataManagement.commitTimeoutSeconds;
		XUiC_MessageBoxWindowGroup.ShowCustom(this.xui, Localization.Get("xuiDmCommitTitle", false, null), Localization.Get("xuiPleaseWait", false, null), "", null, false, false, -1, -1);
	}

	// Token: 0x060068C4 RID: 26820 RVA: 0x00299B6C File Offset: 0x00297D6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setWorldMoveBarPreviewActive(bool _isActive)
	{
		if (!this.DataManagementBarEnabled || !this.worldMove.IsReady)
		{
			return;
		}
		if (this.worldMove.moveToStorageType.UsesDataLimit())
		{
			long worldDataSize = this.worldMove.worldInfo.WorldDataSize;
			this.setBarMovePreviewModeActive(_isActive, worldDataSize);
			return;
		}
		this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Primary);
		this.dataManagementBar.SetDeleteHovered(_isActive);
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x00299BD3 File Offset: 0x00297DD3
	[PublicizedFrom(EAccessModifier.Private)]
	public void setBarMovePreviewModeActive(bool _isActive, long _pendingBytes)
	{
		if (_isActive && _pendingBytes > 0L)
		{
			this.dataManagementBar.SetPendingBytes(_pendingBytes);
			this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Preview);
			return;
		}
		this.dataManagementBar.SetPendingBytes(0L);
		this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x00299C10 File Offset: 0x00297E10
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataStorageType getStorageTypeToMoveTo(UserDataStorageType _moveFromStorageType)
	{
		UserDataStorageType userDataStorageType;
		if (_moveFromStorageType != UserDataStorageType.DeviceLocal)
		{
			if (_moveFromStorageType != UserDataStorageType.Roaming)
			{
				throw new Exception(string.Format("Unhandled move from storage type {0}", _moveFromStorageType));
			}
			userDataStorageType = UserDataStorageType.DeviceLocal;
		}
		else
		{
			userDataStorageType = UserDataStorageType.Roaming;
		}
		return userDataStorageType;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x00299C44 File Offset: 0x00297E44
	[PublicizedFrom(EAccessModifier.Private)]
	public void refreshButtonStates()
	{
		XUiC_DMWorldList xuiC_DMWorldList = this.worldList;
		XUiC_DMWorldList.ListEntry listEntry = ((xuiC_DMWorldList != null) ? xuiC_DMWorldList.SelectedEntryData : null);
		if (listEntry != null)
		{
			this.WorldDeletable = listEntry.Deletable && !SaveInfoProvider.Instance.IsDirectoryProtected(listEntry.Location.FullPath);
			if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && listEntry.Moveable)
			{
				this.worldMove = new UserDataManagement.WorldMove(listEntry.WorldEntryInfo, this.getStorageTypeToMoveTo(listEntry.WorldEntryInfo.Location.StorageType));
			}
			else
			{
				this.worldMove = default(UserDataManagement.WorldMove);
			}
		}
		else
		{
			this.WorldDeletable = false;
			this.worldMove = default(UserDataManagement.WorldMove);
		}
		XUiC_DMSavegamesList xuiC_DMSavegamesList = this.savesList;
		XUiC_DMSavegamesList.ListEntry listEntry2 = ((xuiC_DMSavegamesList != null) ? xuiC_DMSavegamesList.SelectedEntryData : null);
		if (listEntry2 != null)
		{
			this.SaveDeletable = !SaveInfoProvider.Instance.IsDirectoryProtected(listEntry2.SaveDirectory);
			XUiC_DMWorldList.ListEntry selectedEntryData = this.worldList.SelectedEntryData;
			bool flag = ((selectedEntryData != null) ? selectedEntryData.Type : null) != SaveInfoProvider.RemoteWorldsType;
			this.SaveManageable = flag || PlatformManager.MultiPlatform.UserDataRoaming.IsSupported;
			this.SaveCopyable = flag;
		}
		else
		{
			this.SaveDeletable = false;
			this.SaveManageable = false;
			this.SaveCopyable = false;
		}
		this.PlayerDeletable = this.SaveDeletable && this.playersList.SelectedEntryIndex >= 0;
		this.IsDirty = true;
	}

	// Token: 0x060068C8 RID: 26824 RVA: 0x00299DA8 File Offset: 0x00297FA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBarSelectionValues()
	{
		if (!this.DataManagementBarEnabled)
		{
			return;
		}
		XUiC_DataManagementBar.BarRegion none = XUiC_DataManagementBar.BarRegion.None;
		XUiC_DataManagementBar.BarRegion none2 = XUiC_DataManagementBar.BarRegion.None;
		XUiC_DataManagementBar.BarRegion none3 = XUiC_DataManagementBar.BarRegion.None;
		XUiC_DMWorldList.ListEntry selectedEntryData = this.worldList.SelectedEntryData;
		if (selectedEntryData != null)
		{
			long num = (selectedEntryData.UsesDataLimit ? (selectedEntryData.WorldDataSize + selectedEntryData.SaveDataSizeForLimit) : selectedEntryData.SaveDataSizeForLimit);
			none = new XUiC_DataManagementBar.BarRegion(selectedEntryData.WorldEntryInfo.BarStartOffset, num);
			XUiC_DMSavegamesList.ListEntry selectedEntryData2 = this.savesList.SelectedEntryData;
			if (selectedEntryData2 != null && selectedEntryData2.SaveEntryInfo.SizeInfo.UsesDataLimit)
			{
				none2 = new XUiC_DataManagementBar.BarRegion(selectedEntryData2.SaveEntryInfo.BarStartOffset, selectedEntryData2.SaveEntryInfo.SizeInfo.ReportedSize);
				if (this.showBlockedPlayersInBar)
				{
					long num2 = 0L;
					foreach (SaveInfoProvider.PlayerEntryInfo playerEntryInfo in this.playersList.BlockedPlayers)
					{
						num2 += playerEntryInfo.Size;
					}
					none3 = new XUiC_DataManagementBar.BarRegion(none2.End - num2, num2);
				}
				else
				{
					XUiC_DMPlayersList.ListEntry selectedEntryData3 = this.playersList.SelectedEntryData;
					if (selectedEntryData3 != null)
					{
						none3 = new XUiC_DataManagementBar.BarRegion(selectedEntryData3.PlayerEntryInfo.BarStartOffset, selectedEntryData3.PlayerEntryInfo.Size);
					}
				}
			}
		}
		this.dataManagementBar.SetSelectedByteRegion(none, none2, none3);
	}

	// Token: 0x060068C9 RID: 26825 RVA: 0x00299F14 File Offset: 0x00298114
	[PublicizedFrom(EAccessModifier.Private)]
	public static string storageTargetToBtnLabel(UserDataStorageType _storageTarget)
	{
		if (_storageTarget == UserDataStorageType.DeviceLocal)
		{
			return Localization.Get("btnMoveToDeviceLocal", false, null);
		}
		if (_storageTarget == UserDataStorageType.Roaming)
		{
			return Localization.Get("btnMoveToRoaming", false, null);
		}
		throw new Exception(string.Format("Unhandled storage target {0}", _storageTarget));
	}

	// Token: 0x060068CA RID: 26826 RVA: 0x00299F4C File Offset: 0x0029814C
	public static void OpenDataManagementWindow(XUiController _parentController, Action _onClosedCallback = null)
	{
		GUIWindowManager windowManager = _parentController.xui.playerUI.windowManager;
		windowManager.Open(XUiC_DataManagement.id, false);
		XUiWindowGroup xuiWindowGroup = (XUiWindowGroup)windowManager.GetWindow(XUiC_DataManagement.id);
		XUiC_DataManagement xuiC_DataManagement;
		if (xuiWindowGroup == null)
		{
			xuiC_DataManagement = null;
		}
		else
		{
			XUiController controller = xuiWindowGroup.Controller;
			xuiC_DataManagement = ((controller != null) ? controller.GetChildByType<XUiC_DataManagement>() : null);
		}
		XUiC_DataManagement xuiC_DataManagement2 = xuiC_DataManagement;
		if (xuiC_DataManagement2 == null)
		{
			global::UnityEngine.Debug.LogError("Failed to retrieve reference to XUiC_DataManagement instance.");
			return;
		}
		xuiC_DataManagement2.onClosedCallback = _onClosedCallback;
	}

	// Token: 0x060068CB RID: 26827 RVA: 0x00299FB2 File Offset: 0x002981B2
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeDataManagementWindow()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		Action action = this.onClosedCallback;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x060068CC RID: 26828 RVA: 0x00299FE0 File Offset: 0x002981E0
	public static bool IsWindowOpen(XUi _xui)
	{
		return _xui.playerUI.windowManager.IsWindowOpen(XUiC_DataManagement.id);
	}

	// Token: 0x060068D9 RID: 26841 RVA: 0x0029A080 File Offset: 0x00298280
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <BtnMoveWorld_OnPressed>g__OnConfirmationResult|77_1()
	{
		this.setWorldMoveBarPreviewActive(false);
		UserDataManagement.Result result = this.worldMove.PerformMove();
		switch (result)
		{
		case UserDataManagement.Result.Success:
			this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
			break;
		case UserDataManagement.Result.TargetAlreadyExists:
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmWorldMoveErrorTitle", false, null), Localization.Get("xuiDmWorldMoveErrorExists", false, null), "", null, false, false, false);
			break;
		case UserDataManagement.Result.Exception:
			this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmWorldMoveErrorTitle", false, null), Localization.Get("xuiDmWorldMoveErrorFailed", false, null), "", null, false, false, false);
			break;
		case UserDataManagement.Result.FailedToMoveSaves:
			this.commitToken = SaveDataUtils.SaveDataManager.CommitAsync();
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiDmSaveMoveErrorTitle", false, null), Localization.Get("xuiDmSaveMoveErrorMultiple", false, null), "", null, false, false, false);
			break;
		default:
			Log.Error(string.Format("Unhandled world move result type {0}", result));
			break;
		}
		string name = this.worldMove.worldInfo.Name;
		UserDataStorageType userDataStorageType = ((result == UserDataManagement.Result.Success) ? this.worldMove.moveToStorageType : this.worldMove.worldInfo.Location.StorageType);
		this.waitForCommitIfPending();
		this.refresh();
		string worldEntryKey = SaveInfoProvider.GetWorldEntryKey(name, userDataStorageType);
		this.worldList.SelectByKey(worldEntryKey);
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x0400509C RID: 20636
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x0400509D RID: 20637
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.PlayerEntryInfoPlatformDataResolver playerEntryResolver;

	// Token: 0x0400509E RID: 20638
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMWorldList worldList;

	// Token: 0x0400509F RID: 20639
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMSavegamesList savesList;

	// Token: 0x040050A0 RID: 20640
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMPlayersList playersList;

	// Token: 0x040050A1 RID: 20641
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040050A2 RID: 20642
	[XuiBindComponent("btnDeleteWorld", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteWorld;

	// Token: 0x040050A3 RID: 20643
	[XuiBindComponent("btnDeleteSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteSave;

	// Token: 0x040050A4 RID: 20644
	[XuiBindComponent("btnCopySave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopySave;

	// Token: 0x040050A5 RID: 20645
	[XuiBindComponent("btnManageSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManageSave;

	// Token: 0x040050A6 RID: 20646
	[XuiBindComponent("btnDeletePlayer", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeletePlayer;

	// Token: 0x040050A7 RID: 20647
	[XuiBindComponent("btnDeleteBlockedPlayers", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteBlockedPlayers;

	// Token: 0x040050A8 RID: 20648
	[XuiBindComponent("btnMoveWorld", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnMoveWorld;

	// Token: 0x040050A9 RID: 20649
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x040050AA RID: 20650
	[PublicizedFrom(EAccessModifier.Private)]
	public bool worldDeletable;

	// Token: 0x040050AB RID: 20651
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveDeletable;

	// Token: 0x040050AC RID: 20652
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveManageable;

	// Token: 0x040050AD RID: 20653
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveCopyable;

	// Token: 0x040050AE RID: 20654
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveVersionValid;

	// Token: 0x040050AF RID: 20655
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerDeletable;

	// Token: 0x040050B0 RID: 20656
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showBlockedPlayersInBar;

	// Token: 0x040050B1 RID: 20657
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataManagement.WorldMove worldMove;

	// Token: 0x040050B2 RID: 20658
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousLockView;

	// Token: 0x040050B3 RID: 20659
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClosedCallback;

	// Token: 0x040050B4 RID: 20660
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int commitTimeoutSeconds = 30;

	// Token: 0x040050B5 RID: 20661
	[PublicizedFrom(EAccessModifier.Private)]
	public int commitToken;

	// Token: 0x040050B6 RID: 20662
	[PublicizedFrom(EAccessModifier.Private)]
	public bool waitingForCommit;

	// Token: 0x040050B7 RID: 20663
	[PublicizedFrom(EAccessModifier.Private)]
	public int commitDeadline = int.MaxValue;

	// Token: 0x02000D90 RID: 3472
	[PublicizedFrom(EAccessModifier.Private)]
	public enum DeletionMode
	{
		// Token: 0x040050B9 RID: 20665
		None,
		// Token: 0x040050BA RID: 20666
		World,
		// Token: 0x040050BB RID: 20667
		Save,
		// Token: 0x040050BC RID: 20668
		Player,
		// Token: 0x040050BD RID: 20669
		BlockedPlayers
	}
}
