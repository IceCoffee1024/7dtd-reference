using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D84 RID: 3460
[Preserve]
public class XUiC_DataManagement : XUiController
{
	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x06006870 RID: 26736 RVA: 0x00297428 File Offset: 0x00295628
	[XuiXmlBinding("showbar")]
	public bool DataManagementBarEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.dataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		}
	}

	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x06006871 RID: 26737 RVA: 0x00297439 File Offset: 0x00295639
	// (set) Token: 0x06006872 RID: 26738 RVA: 0x00297441 File Offset: 0x00295641
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

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x06006873 RID: 26739 RVA: 0x00297451 File Offset: 0x00295651
	// (set) Token: 0x06006874 RID: 26740 RVA: 0x00297459 File Offset: 0x00295659
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

	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x06006875 RID: 26741 RVA: 0x00297469 File Offset: 0x00295669
	// (set) Token: 0x06006876 RID: 26742 RVA: 0x00297471 File Offset: 0x00295671
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

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x06006877 RID: 26743 RVA: 0x00297481 File Offset: 0x00295681
	[XuiXmlBinding("worldmoveable")]
	public bool WorldMoveable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.worldMove.IsReady;
		}
	}

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x06006878 RID: 26744 RVA: 0x0029748E File Offset: 0x0029568E
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

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x06006879 RID: 26745 RVA: 0x002974B3 File Offset: 0x002956B3
	// (set) Token: 0x0600687A RID: 26746 RVA: 0x002974BB File Offset: 0x002956BB
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

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x0600687B RID: 26747 RVA: 0x002974CB File Offset: 0x002956CB
	// (set) Token: 0x0600687C RID: 26748 RVA: 0x002974D3 File Offset: 0x002956D3
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

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x0600687D RID: 26749 RVA: 0x002974E3 File Offset: 0x002956E3
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

	// Token: 0x0600687E RID: 26750 RVA: 0x002974F6 File Offset: 0x002956F6
	public override void Init()
	{
		base.Init();
		XUiC_DataManagement.id = base.WindowGroup.Id;
		this.IsDirty = true;
	}

	// Token: 0x0600687F RID: 26751 RVA: 0x00297518 File Offset: 0x00295718
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

	// Token: 0x06006880 RID: 26752 RVA: 0x0029763F File Offset: 0x0029583F
	public override void OnClose()
	{
		base.OnClose();
		this.waitingForCommit = false;
		this.commitDeadline = int.MaxValue;
		this.worldList.ClearList();
		this.savesList.ClearList();
		this.playersList.ClearList(true);
	}

	// Token: 0x06006881 RID: 26753 RVA: 0x0029767C File Offset: 0x0029587C
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

	// Token: 0x06006882 RID: 26754 RVA: 0x002976E0 File Offset: 0x002958E0
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

	// Token: 0x06006883 RID: 26755 RVA: 0x00297818 File Offset: 0x00295A18
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

	// Token: 0x06006884 RID: 26756 RVA: 0x00297870 File Offset: 0x00295A70
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

	// Token: 0x06006885 RID: 26757 RVA: 0x0029793C File Offset: 0x00295B3C
	[XuiBindEvent("SelectionChanged", "playersList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayersList_SelectionChanged(XUiC_List<XUiC_DMPlayersList.ListEntry> _list, XUiC_DMPlayersList.ListEntry _previousEntry, XUiC_DMPlayersList.ListEntry _newEntry)
	{
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x06006886 RID: 26758 RVA: 0x0029794C File Offset: 0x00295B4C
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

	// Token: 0x06006887 RID: 26759 RVA: 0x002979C8 File Offset: 0x00295BC8
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

	// Token: 0x06006888 RID: 26760 RVA: 0x00297A44 File Offset: 0x00295C44
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

	// Token: 0x06006889 RID: 26761 RVA: 0x00297AAA File Offset: 0x00295CAA
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeDataManagementWindow();
	}

	// Token: 0x0600688A RID: 26762 RVA: 0x00297AB4 File Offset: 0x00295CB4
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

	// Token: 0x0600688B RID: 26763 RVA: 0x00297B53 File Offset: 0x00295D53
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

	// Token: 0x0600688C RID: 26764 RVA: 0x00297B78 File Offset: 0x00295D78
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

	// Token: 0x0600688D RID: 26765 RVA: 0x00297C08 File Offset: 0x00295E08
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

	// Token: 0x0600688E RID: 26766 RVA: 0x00297C2C File Offset: 0x00295E2C
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

	// Token: 0x0600688F RID: 26767 RVA: 0x00297C8D File Offset: 0x00295E8D
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

	// Token: 0x06006890 RID: 26768 RVA: 0x00297CB0 File Offset: 0x00295EB0
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

	// Token: 0x06006891 RID: 26769 RVA: 0x00297D1D File Offset: 0x00295F1D
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

	// Token: 0x06006892 RID: 26770 RVA: 0x00297D55 File Offset: 0x00295F55
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

	// Token: 0x06006893 RID: 26771 RVA: 0x00297D6C File Offset: 0x00295F6C
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

	// Token: 0x06006894 RID: 26772 RVA: 0x00297DF0 File Offset: 0x00295FF0
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

	// Token: 0x06006895 RID: 26773 RVA: 0x00298298 File Offset: 0x00296498
	[Conditional("DELETION_LOG_ENABLED")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void logDeletion(string _v)
	{
		global::UnityEngine.Debug.Log(_v);
	}

	// Token: 0x06006896 RID: 26774 RVA: 0x002982A0 File Offset: 0x002964A0
	[XuiBindEvent("OnHover", "btnMoveWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnMoveWorld_OnHover(XUiController _sender, bool _isOver)
	{
		this.setWorldMoveBarPreviewActive(_isOver);
	}

	// Token: 0x06006897 RID: 26775 RVA: 0x002982AC File Offset: 0x002964AC
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

	// Token: 0x06006898 RID: 26776 RVA: 0x00298373 File Offset: 0x00296573
	[XuiBindEvent("OnPress", "btnCopySave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopySave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openSaveManagementWindow(XUiC_SaveManagementPrompt.SaveManagementMode.Copy);
	}

	// Token: 0x06006899 RID: 26777 RVA: 0x0029837C File Offset: 0x0029657C
	[XuiBindEvent("OnPress", "btnManageSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnManageSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openSaveManagementWindow(XUiC_SaveManagementPrompt.SaveManagementMode.Manage);
	}

	// Token: 0x0600689A RID: 26778 RVA: 0x00298388 File Offset: 0x00296588
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

	// Token: 0x0600689B RID: 26779 RVA: 0x002983F4 File Offset: 0x002965F4
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

	// Token: 0x0600689C RID: 26780 RVA: 0x002984B4 File Offset: 0x002966B4
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

	// Token: 0x0600689D RID: 26781 RVA: 0x0029851C File Offset: 0x0029671C
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

	// Token: 0x0600689E RID: 26782 RVA: 0x002985DC File Offset: 0x002967DC
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

	// Token: 0x0600689F RID: 26783 RVA: 0x00298644 File Offset: 0x00296844
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

	// Token: 0x060068A0 RID: 26784 RVA: 0x002986AB File Offset: 0x002968AB
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

	// Token: 0x060068A1 RID: 26785 RVA: 0x002986E8 File Offset: 0x002968E8
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

	// Token: 0x060068A2 RID: 26786 RVA: 0x0029871C File Offset: 0x0029691C
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

	// Token: 0x060068A3 RID: 26787 RVA: 0x00298880 File Offset: 0x00296A80
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

	// Token: 0x060068A4 RID: 26788 RVA: 0x002989EC File Offset: 0x00296BEC
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

	// Token: 0x060068A5 RID: 26789 RVA: 0x00298A24 File Offset: 0x00296C24
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

	// Token: 0x060068A6 RID: 26790 RVA: 0x00298A8A File Offset: 0x00296C8A
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

	// Token: 0x060068A7 RID: 26791 RVA: 0x00298AB8 File Offset: 0x00296CB8
	public static bool IsWindowOpen(XUi _xui)
	{
		return _xui.playerUI.windowManager.IsWindowOpen(XUiC_DataManagement.id);
	}

	// Token: 0x060068B4 RID: 26804 RVA: 0x00298B58 File Offset: 0x00296D58
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

	// Token: 0x04005077 RID: 20599
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04005078 RID: 20600
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.PlayerEntryInfoPlatformDataResolver playerEntryResolver;

	// Token: 0x04005079 RID: 20601
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMWorldList worldList;

	// Token: 0x0400507A RID: 20602
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMSavegamesList savesList;

	// Token: 0x0400507B RID: 20603
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMPlayersList playersList;

	// Token: 0x0400507C RID: 20604
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x0400507D RID: 20605
	[XuiBindComponent("btnDeleteWorld", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteWorld;

	// Token: 0x0400507E RID: 20606
	[XuiBindComponent("btnDeleteSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteSave;

	// Token: 0x0400507F RID: 20607
	[XuiBindComponent("btnCopySave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopySave;

	// Token: 0x04005080 RID: 20608
	[XuiBindComponent("btnManageSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManageSave;

	// Token: 0x04005081 RID: 20609
	[XuiBindComponent("btnDeletePlayer", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeletePlayer;

	// Token: 0x04005082 RID: 20610
	[XuiBindComponent("btnDeleteBlockedPlayers", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteBlockedPlayers;

	// Token: 0x04005083 RID: 20611
	[XuiBindComponent("btnMoveWorld", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnMoveWorld;

	// Token: 0x04005084 RID: 20612
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x04005085 RID: 20613
	[PublicizedFrom(EAccessModifier.Private)]
	public bool worldDeletable;

	// Token: 0x04005086 RID: 20614
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveDeletable;

	// Token: 0x04005087 RID: 20615
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveManageable;

	// Token: 0x04005088 RID: 20616
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveCopyable;

	// Token: 0x04005089 RID: 20617
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveVersionValid;

	// Token: 0x0400508A RID: 20618
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerDeletable;

	// Token: 0x0400508B RID: 20619
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showBlockedPlayersInBar;

	// Token: 0x0400508C RID: 20620
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataManagement.WorldMove worldMove;

	// Token: 0x0400508D RID: 20621
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousLockView;

	// Token: 0x0400508E RID: 20622
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClosedCallback;

	// Token: 0x0400508F RID: 20623
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int commitTimeoutSeconds = 30;

	// Token: 0x04005090 RID: 20624
	[PublicizedFrom(EAccessModifier.Private)]
	public int commitToken;

	// Token: 0x04005091 RID: 20625
	[PublicizedFrom(EAccessModifier.Private)]
	public bool waitingForCommit;

	// Token: 0x04005092 RID: 20626
	[PublicizedFrom(EAccessModifier.Private)]
	public int commitDeadline = int.MaxValue;

	// Token: 0x02000D85 RID: 3461
	[PublicizedFrom(EAccessModifier.Private)]
	public enum DeletionMode
	{
		// Token: 0x04005094 RID: 20628
		None,
		// Token: 0x04005095 RID: 20629
		World,
		// Token: 0x04005096 RID: 20630
		Save,
		// Token: 0x04005097 RID: 20631
		Player,
		// Token: 0x04005098 RID: 20632
		BlockedPlayers
	}
}
