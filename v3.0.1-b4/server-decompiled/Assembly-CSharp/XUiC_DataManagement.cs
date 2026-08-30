using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D61 RID: 3425
[Preserve]
public class XUiC_DataManagement : XUiController
{
	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x060067DC RID: 26588 RVA: 0x00290F48 File Offset: 0x0028F148
	[XuiXmlBinding("showbar")]
	public bool DataManagementBarEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.dataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		}
	}

	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x060067DD RID: 26589 RVA: 0x00290F59 File Offset: 0x0028F159
	// (set) Token: 0x060067DE RID: 26590 RVA: 0x00290F61 File Offset: 0x0028F161
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

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x060067DF RID: 26591 RVA: 0x00290F71 File Offset: 0x0028F171
	// (set) Token: 0x060067E0 RID: 26592 RVA: 0x00290F79 File Offset: 0x0028F179
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

	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x060067E1 RID: 26593 RVA: 0x00290F89 File Offset: 0x0028F189
	// (set) Token: 0x060067E2 RID: 26594 RVA: 0x00290F91 File Offset: 0x0028F191
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

	// Token: 0x17000B09 RID: 2825
	// (get) Token: 0x060067E3 RID: 26595 RVA: 0x00290FA1 File Offset: 0x0028F1A1
	[XuiXmlBinding("worldmoveable")]
	public bool WorldMoveable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.worldMove.IsReady;
		}
	}

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x060067E4 RID: 26596 RVA: 0x00290FAE File Offset: 0x0028F1AE
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

	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x060067E5 RID: 26597 RVA: 0x00290FD3 File Offset: 0x0028F1D3
	// (set) Token: 0x060067E6 RID: 26598 RVA: 0x00290FDB File Offset: 0x0028F1DB
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

	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x060067E7 RID: 26599 RVA: 0x00290FEB File Offset: 0x0028F1EB
	// (set) Token: 0x060067E8 RID: 26600 RVA: 0x00290FF3 File Offset: 0x0028F1F3
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

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x060067E9 RID: 26601 RVA: 0x00291003 File Offset: 0x0028F203
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

	// Token: 0x060067EA RID: 26602 RVA: 0x00291016 File Offset: 0x0028F216
	public override void Init()
	{
		base.Init();
		XUiC_DataManagement.id = base.WindowGroup.Id;
		this.IsDirty = true;
	}

	// Token: 0x060067EB RID: 26603 RVA: 0x00291038 File Offset: 0x0028F238
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

	// Token: 0x060067EC RID: 26604 RVA: 0x0029115F File Offset: 0x0028F35F
	public override void OnClose()
	{
		base.OnClose();
		this.waitingForCommit = false;
		this.commitDeadline = int.MaxValue;
		this.worldList.ClearList();
		this.savesList.ClearList();
		this.playersList.ClearList(true);
	}

	// Token: 0x060067ED RID: 26605 RVA: 0x0029119C File Offset: 0x0028F39C
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

	// Token: 0x060067EE RID: 26606 RVA: 0x00291200 File Offset: 0x0028F400
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

	// Token: 0x060067EF RID: 26607 RVA: 0x00291338 File Offset: 0x0028F538
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

	// Token: 0x060067F0 RID: 26608 RVA: 0x00291390 File Offset: 0x0028F590
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

	// Token: 0x060067F1 RID: 26609 RVA: 0x0029145C File Offset: 0x0028F65C
	[XuiBindEvent("SelectionChanged", "playersList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayersList_SelectionChanged(XUiC_List<XUiC_DMPlayersList.ListEntry> _list, XUiC_DMPlayersList.ListEntry _previousEntry, XUiC_DMPlayersList.ListEntry _newEntry)
	{
		this.refreshButtonStates();
		this.updateBarSelectionValues();
	}

	// Token: 0x060067F2 RID: 26610 RVA: 0x0029146C File Offset: 0x0028F66C
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

	// Token: 0x060067F3 RID: 26611 RVA: 0x002914E8 File Offset: 0x0028F6E8
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

	// Token: 0x060067F4 RID: 26612 RVA: 0x00291564 File Offset: 0x0028F764
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

	// Token: 0x060067F5 RID: 26613 RVA: 0x002915CA File Offset: 0x0028F7CA
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeDataManagementWindow();
	}

	// Token: 0x060067F6 RID: 26614 RVA: 0x002915D4 File Offset: 0x0028F7D4
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

	// Token: 0x060067F7 RID: 26615 RVA: 0x00291673 File Offset: 0x0028F873
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

	// Token: 0x060067F8 RID: 26616 RVA: 0x00291698 File Offset: 0x0028F898
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

	// Token: 0x060067F9 RID: 26617 RVA: 0x00291728 File Offset: 0x0028F928
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

	// Token: 0x060067FA RID: 26618 RVA: 0x0029174C File Offset: 0x0028F94C
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

	// Token: 0x060067FB RID: 26619 RVA: 0x002917AD File Offset: 0x0028F9AD
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

	// Token: 0x060067FC RID: 26620 RVA: 0x002917D0 File Offset: 0x0028F9D0
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

	// Token: 0x060067FD RID: 26621 RVA: 0x0029183D File Offset: 0x0028FA3D
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

	// Token: 0x060067FE RID: 26622 RVA: 0x00291875 File Offset: 0x0028FA75
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

	// Token: 0x060067FF RID: 26623 RVA: 0x0029188C File Offset: 0x0028FA8C
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

	// Token: 0x06006800 RID: 26624 RVA: 0x00291910 File Offset: 0x0028FB10
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

	// Token: 0x06006801 RID: 26625 RVA: 0x00291DB8 File Offset: 0x0028FFB8
	[Conditional("DELETION_LOG_ENABLED")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void logDeletion(string _v)
	{
		global::UnityEngine.Debug.Log(_v);
	}

	// Token: 0x06006802 RID: 26626 RVA: 0x00291DC0 File Offset: 0x0028FFC0
	[XuiBindEvent("OnHover", "btnMoveWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnMoveWorld_OnHover(XUiController _sender, bool _isOver)
	{
		this.setWorldMoveBarPreviewActive(_isOver);
	}

	// Token: 0x06006803 RID: 26627 RVA: 0x00291DCC File Offset: 0x0028FFCC
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

	// Token: 0x06006804 RID: 26628 RVA: 0x00291E93 File Offset: 0x00290093
	[XuiBindEvent("OnPress", "btnCopySave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopySave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openSaveManagementWindow(XUiC_SaveManagementPrompt.SaveManagementMode.Copy);
	}

	// Token: 0x06006805 RID: 26629 RVA: 0x00291E9C File Offset: 0x0029009C
	[XuiBindEvent("OnPress", "btnManageSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnManageSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openSaveManagementWindow(XUiC_SaveManagementPrompt.SaveManagementMode.Manage);
	}

	// Token: 0x06006806 RID: 26630 RVA: 0x00291EA8 File Offset: 0x002900A8
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

	// Token: 0x06006807 RID: 26631 RVA: 0x00291F14 File Offset: 0x00290114
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

	// Token: 0x06006808 RID: 26632 RVA: 0x00291FD4 File Offset: 0x002901D4
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

	// Token: 0x06006809 RID: 26633 RVA: 0x0029203C File Offset: 0x0029023C
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

	// Token: 0x0600680A RID: 26634 RVA: 0x002920FC File Offset: 0x002902FC
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

	// Token: 0x0600680B RID: 26635 RVA: 0x00292164 File Offset: 0x00290364
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

	// Token: 0x0600680C RID: 26636 RVA: 0x002921CB File Offset: 0x002903CB
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

	// Token: 0x0600680D RID: 26637 RVA: 0x00292208 File Offset: 0x00290408
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

	// Token: 0x0600680E RID: 26638 RVA: 0x0029223C File Offset: 0x0029043C
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

	// Token: 0x0600680F RID: 26639 RVA: 0x002923A0 File Offset: 0x002905A0
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

	// Token: 0x06006810 RID: 26640 RVA: 0x0029250C File Offset: 0x0029070C
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

	// Token: 0x06006811 RID: 26641 RVA: 0x00292544 File Offset: 0x00290744
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

	// Token: 0x06006812 RID: 26642 RVA: 0x002925AA File Offset: 0x002907AA
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

	// Token: 0x06006813 RID: 26643 RVA: 0x002925D8 File Offset: 0x002907D8
	public static bool IsWindowOpen(XUi _xui)
	{
		return _xui.playerUI.windowManager.IsWindowOpen(XUiC_DataManagement.id);
	}

	// Token: 0x06006820 RID: 26656 RVA: 0x00292678 File Offset: 0x00290878
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

	// Token: 0x04004F9E RID: 20382
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04004F9F RID: 20383
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.PlayerEntryInfoPlatformDataResolver playerEntryResolver;

	// Token: 0x04004FA0 RID: 20384
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMWorldList worldList;

	// Token: 0x04004FA1 RID: 20385
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMSavegamesList savesList;

	// Token: 0x04004FA2 RID: 20386
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DMPlayersList playersList;

	// Token: 0x04004FA3 RID: 20387
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04004FA4 RID: 20388
	[XuiBindComponent("btnDeleteWorld", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteWorld;

	// Token: 0x04004FA5 RID: 20389
	[XuiBindComponent("btnDeleteSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteSave;

	// Token: 0x04004FA6 RID: 20390
	[XuiBindComponent("btnCopySave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopySave;

	// Token: 0x04004FA7 RID: 20391
	[XuiBindComponent("btnManageSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManageSave;

	// Token: 0x04004FA8 RID: 20392
	[XuiBindComponent("btnDeletePlayer", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeletePlayer;

	// Token: 0x04004FA9 RID: 20393
	[XuiBindComponent("btnDeleteBlockedPlayers", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteBlockedPlayers;

	// Token: 0x04004FAA RID: 20394
	[XuiBindComponent("btnMoveWorld", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnMoveWorld;

	// Token: 0x04004FAB RID: 20395
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x04004FAC RID: 20396
	[PublicizedFrom(EAccessModifier.Private)]
	public bool worldDeletable;

	// Token: 0x04004FAD RID: 20397
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveDeletable;

	// Token: 0x04004FAE RID: 20398
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveManageable;

	// Token: 0x04004FAF RID: 20399
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveCopyable;

	// Token: 0x04004FB0 RID: 20400
	[PublicizedFrom(EAccessModifier.Private)]
	public bool saveVersionValid;

	// Token: 0x04004FB1 RID: 20401
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerDeletable;

	// Token: 0x04004FB2 RID: 20402
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showBlockedPlayersInBar;

	// Token: 0x04004FB3 RID: 20403
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataManagement.WorldMove worldMove;

	// Token: 0x04004FB4 RID: 20404
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousLockView;

	// Token: 0x04004FB5 RID: 20405
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClosedCallback;

	// Token: 0x04004FB6 RID: 20406
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int commitTimeoutSeconds = 30;

	// Token: 0x04004FB7 RID: 20407
	[PublicizedFrom(EAccessModifier.Private)]
	public int commitToken;

	// Token: 0x04004FB8 RID: 20408
	[PublicizedFrom(EAccessModifier.Private)]
	public bool waitingForCommit;

	// Token: 0x04004FB9 RID: 20409
	[PublicizedFrom(EAccessModifier.Private)]
	public int commitDeadline = int.MaxValue;

	// Token: 0x02000D62 RID: 3426
	[PublicizedFrom(EAccessModifier.Private)]
	public enum DeletionMode
	{
		// Token: 0x04004FBB RID: 20411
		None,
		// Token: 0x04004FBC RID: 20412
		World,
		// Token: 0x04004FBD RID: 20413
		Save,
		// Token: 0x04004FBE RID: 20414
		Player,
		// Token: 0x04004FBF RID: 20415
		BlockedPlayers
	}
}
