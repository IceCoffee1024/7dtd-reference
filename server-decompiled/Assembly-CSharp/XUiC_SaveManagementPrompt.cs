using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D78 RID: 3448
[Preserve]
public class XUiC_SaveManagementPrompt : XUiController
{
	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x060068AE RID: 26798 RVA: 0x00294B11 File Offset: 0x00292D11
	// (set) Token: 0x060068AF RID: 26799 RVA: 0x00294B19 File Offset: 0x00292D19
	[XuiXmlBinding("showStorageType")]
	public bool ShowStorageType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x060068B0 RID: 26800 RVA: 0x00294B22 File Offset: 0x00292D22
	// (set) Token: 0x060068B1 RID: 26801 RVA: 0x00294B2A File Offset: 0x00292D2A
	[XuiXmlBinding("canChangeStorageType")]
	public bool CanChangeStorageType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x060068B2 RID: 26802 RVA: 0x00294B33 File Offset: 0x00292D33
	// (set) Token: 0x060068B3 RID: 26803 RVA: 0x00294B3B File Offset: 0x00292D3B
	[XuiXmlBinding("applyValid")]
	public bool ActionIsValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x060068B4 RID: 26804 RVA: 0x00294B44 File Offset: 0x00292D44
	[XuiXmlBinding("canCopy")]
	public bool CanCopy
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.isRemoteSave;
		}
	}

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x060068B5 RID: 26805 RVA: 0x00294B44 File Offset: 0x00292D44
	[XuiXmlBinding("showNameInput")]
	public bool ShowNameInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.isRemoteSave;
		}
	}

	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x060068B6 RID: 26806 RVA: 0x00294B4F File Offset: 0x00292D4F
	[XuiXmlBinding("showSaveDataLimit")]
	public bool ShowSaveDataLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SaveInfoProvider.WorldEntryInfo worldEntryInfo = this.worldInfo;
			return worldEntryInfo != null && worldEntryInfo.WorldSize != null;
		}
	}

	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x060068B7 RID: 26807 RVA: 0x00294B67 File Offset: 0x00292D67
	[XuiXmlBinding("showIsArchived")]
	public bool ShowIsArchived
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsSupported;
		}
	}

	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x060068B8 RID: 26808 RVA: 0x00294B78 File Offset: 0x00292D78
	[XuiXmlBinding("canChangeIsArchived")]
	public bool CanChangeIsArchived
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<UserDataStorageType> xuiC_ComboBoxEnum = this.cbxStorageType;
			return xuiC_ComboBoxEnum != null && xuiC_ComboBoxEnum.Value.UsesDataLimit();
		}
	}

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x060068B9 RID: 26809 RVA: 0x00294B90 File Offset: 0x00292D90
	[XuiXmlBinding("confirmTextKey")]
	public string ConfirmTextKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_SaveManagementPrompt.SaveManagementMode saveManagementMode = this.mode;
			string text;
			if (saveManagementMode != XUiC_SaveManagementPrompt.SaveManagementMode.Copy)
			{
				if (saveManagementMode != XUiC_SaveManagementPrompt.SaveManagementMode.Manage)
				{
					throw new Exception("Unhandled SaveManagementMode");
				}
				text = (this.isMove ? "xuiDmMoveSave" : "btnApply");
			}
			else
			{
				text = "xuiDmCopySave";
			}
			return text;
		}
	}

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x060068BA RID: 26810 RVA: 0x00294BD8 File Offset: 0x00292DD8
	[XuiXmlBinding("titleTextKey")]
	public string TitleTextKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_SaveManagementPrompt.SaveManagementMode saveManagementMode = this.mode;
			string text;
			if (saveManagementMode != XUiC_SaveManagementPrompt.SaveManagementMode.Copy)
			{
				if (saveManagementMode != XUiC_SaveManagementPrompt.SaveManagementMode.Manage)
				{
					throw new Exception("Unhandled SaveManagementMode");
				}
				text = "xuiDmManageSave";
			}
			else
			{
				text = "xuiDmCopySave";
			}
			return text;
		}
	}

	// Token: 0x1400008F RID: 143
	// (add) Token: 0x060068BB RID: 26811 RVA: 0x00294C10 File Offset: 0x00292E10
	// (remove) Token: 0x060068BC RID: 26812 RVA: 0x00294C48 File Offset: 0x00292E48
	public event XUiC_SaveManagementPrompt.SaveManagementActionCopy OnCopyConfirmed;

	// Token: 0x14000090 RID: 144
	// (add) Token: 0x060068BD RID: 26813 RVA: 0x00294C80 File Offset: 0x00292E80
	// (remove) Token: 0x060068BE RID: 26814 RVA: 0x00294CB8 File Offset: 0x00292EB8
	public event XUiC_SaveManagementPrompt.SaveManagementActionMove OnMoveConfirmed;

	// Token: 0x14000091 RID: 145
	// (add) Token: 0x060068BF RID: 26815 RVA: 0x00294CF0 File Offset: 0x00292EF0
	// (remove) Token: 0x060068C0 RID: 26816 RVA: 0x00294D28 File Offset: 0x00292F28
	public event XUiC_SaveManagementPrompt.SaveManagementActionApply OnApplyConfirmed;

	// Token: 0x060068C1 RID: 26817 RVA: 0x00294D5D File Offset: 0x00292F5D
	public override void Init()
	{
		base.Init();
		this.txtSaveName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
	}

	// Token: 0x060068C2 RID: 26818 RVA: 0x00294D84 File Offset: 0x00292F84
	public override void OnClose()
	{
		base.OnClose();
		if (this.dataManagementBar == null)
		{
			return;
		}
		this.dataManagementBar.SetDeleteHovered(false);
		this.dataManagementBar.SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion.None);
		this.dataManagementBar.SetPendingBytes(0L);
		this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
	}

	// Token: 0x060068C3 RID: 26819 RVA: 0x00294DD5 File Offset: 0x00292FD5
	[PublicizedFrom(EAccessModifier.Private)]
	public void cleanLinks()
	{
		this.saveInfo = null;
		this.worldInfo = null;
		this.dataManagementBar = null;
		this.OnCopyConfirmed = null;
		this.OnMoveConfirmed = null;
		this.OnApplyConfirmed = null;
	}

	// Token: 0x060068C4 RID: 26820 RVA: 0x00294E04 File Offset: 0x00293004
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x00294E60 File Offset: 0x00293060
	[PublicizedFrom(EAccessModifier.Private)]
	public void configureSaveDataLimitAllowedValues(UserDataStorageType _storageType)
	{
		SaveDataLimitType value = this.cbxSaveDataLimit.Value;
		this.cbxSaveDataLimit.Min = null;
		if (_storageType.UsesDataLimit())
		{
			if (value == SaveDataLimitType.Unlimited)
			{
				this.cbxSaveDataLimit.Value = SaveDataLimitType.VeryLong;
			}
			this.cbxSaveDataLimit.Min = new SaveDataLimitType?(SaveDataLimitType.Short);
		}
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x00294EB8 File Offset: 0x002930B8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateSizeLimit(out long _pendingSaveBytes)
	{
		if (this.worldInfo.WorldSize == null)
		{
			_pendingSaveBytes = Math.Max(this.saveInfo.SizeInfo.BytesOnDisk, this.saveInfo.SizeInfo.BytesReserved);
			return true;
		}
		SaveDataLimitType value = this.cbxSaveDataLimit.Value;
		if (value == SaveDataLimitType.Unlimited)
		{
			_pendingSaveBytes = this.saveInfo.SizeInfo.BytesOnDisk;
			return true;
		}
		long num = value.CalculateTotalSize(this.worldInfo.WorldSize.Value);
		if (this.saveInfo.SizeInfo.BytesOnDisk > num)
		{
			_pendingSaveBytes = this.saveInfo.SizeInfo.BytesOnDisk;
			return false;
		}
		_pendingSaveBytes = num;
		return true;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x00294F68 File Offset: 0x00293168
	[PublicizedFrom(EAccessModifier.Private)]
	public void validate()
	{
		long num;
		bool flag = this.validateSizeLimit(out num);
		long num2 = (this.cbxIsArchived.Value ? this.saveInfo.SizeInfo.BytesOnDisk : num);
		if (this.worldInfo.ShouldBeMovedWithSave(this.cbxStorageType.Value))
		{
			num2 += this.worldInfo.WorldDataSize;
		}
		bool flag2 = !this.cbxStorageType.Value.UsesDataLimit() || num2 <= SaveInfoProvider.Instance.TotalAvailableBytes;
		this.cbxSaveDataLimit.TextColor = (flag ? Color.white : Color.red);
		bool flag3 = this.isRemoteSave || this.validateNameAvailable(this.txtSaveName.Text, this.cbxStorageType.Value);
		bool flag4 = !this.isRemoteSave && !string.Equals(this.txtSaveName.Text, this.saveInfo.Name, StringComparison.Ordinal);
		bool flag5 = this.cbxStorageType.Value != this.saveInfo.StorageType;
		this.isMove = flag4 || flag5;
		bool flag6 = flag3 || (this.mode == XUiC_SaveManagementPrompt.SaveManagementMode.Manage && !this.isMove);
		this.txtSaveName.ActiveTextColor = (flag6 ? Color.white : Color.red);
		this.ActionIsValid = flag && flag2 && flag6;
		this.updateDataBarPreview(num2, flag5);
		base.RefreshBindings();
	}

	// Token: 0x060068C8 RID: 26824 RVA: 0x002950D4 File Offset: 0x002932D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateDataBarPreview(long _pendingBytes, bool _storageChanged)
	{
		if (this.dataManagementBar == null)
		{
			return;
		}
		bool flag = this.cbxStorageType.Value.UsesDataLimit();
		bool flag2 = this.saveInfo.StorageType.UsesDataLimit();
		if (flag && (this.mode == XUiC_SaveManagementPrompt.SaveManagementMode.Copy || !flag2))
		{
			this.dataManagementBar.SetDeleteHovered(false);
			this.dataManagementBar.SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion.None);
			this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Preview);
			this.dataManagementBar.SetPendingBytes(_pendingBytes);
			return;
		}
		if (this.mode == XUiC_SaveManagementPrompt.SaveManagementMode.Manage && flag2 && _storageChanged && !flag)
		{
			this.dataManagementBar.SetPendingBytes(0L);
			this.dataManagementBar.SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion.None);
			this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
			this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Secondary);
			this.dataManagementBar.SetDeleteHovered(true);
			return;
		}
		if (!flag || !flag2 || _storageChanged)
		{
			this.dataManagementBar.SetDeleteHovered(false);
			this.dataManagementBar.SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion.None);
			this.dataManagementBar.SetPendingBytes(0L);
			this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
			return;
		}
		long num = _pendingBytes - this.saveInfo.SizeInfo.ReportedSize;
		this.dataManagementBar.SetDeleteHovered(false);
		if (num < 0L)
		{
			long num2 = this.saveInfo.BarStartOffset + _pendingBytes;
			XUiC_DataManagementBar.BarRegion barRegion = new XUiC_DataManagementBar.BarRegion(num2, -num);
			this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
			this.dataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Secondary);
			this.dataManagementBar.SetArchivePreviewRegion(barRegion);
			return;
		}
		if (num > 0L)
		{
			this.dataManagementBar.SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion.None);
			this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Preview);
			this.dataManagementBar.SetPendingBytes(num);
			return;
		}
		this.dataManagementBar.SetArchivePreviewRegion(XUiC_DataManagementBar.BarRegion.None);
		this.dataManagementBar.SetPendingBytes(0L);
		this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
	}

	// Token: 0x060068C9 RID: 26825 RVA: 0x002952A0 File Offset: 0x002934A0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateNameAvailable(string _name, UserDataStorageType _storageType)
	{
		return !string.IsNullOrEmpty(_name) && !SdDirectory.Exists(GameIO.GetSaveGameDir(this.worldInfo.Name, _name, _storageType));
	}

	// Token: 0x060068CA RID: 26826 RVA: 0x002952C6 File Offset: 0x002934C6
	[XuiBindEvent("OnChangeHandler", "txtSaveName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtSaveName_OnChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.validate();
	}

	// Token: 0x060068CB RID: 26827 RVA: 0x002952CE File Offset: 0x002934CE
	[XuiBindEvent("OnValueChanged", "cbxStorageType")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxStorageType_OnValueChanged(XUiController _sender, UserDataStorageType _oldValue, UserDataStorageType _newValue)
	{
		this.configureSaveDataLimitAllowedValues(_newValue);
		this.cbxIsArchived.Value = _newValue.UsesDataLimit() && this.cbxIsArchived.Value;
		this.validate();
	}

	// Token: 0x060068CC RID: 26828 RVA: 0x002952C6 File Offset: 0x002934C6
	[XuiBindEvent("OnValueChanged", "cbxIsArchived")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxIsArchived_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.validate();
	}

	// Token: 0x060068CD RID: 26829 RVA: 0x002952C6 File Offset: 0x002934C6
	[XuiBindEvent("OnValueChanged", "cbxSaveDataLimit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxSaveDataLimit_OnValueChanged(XUiController _sender, SaveDataLimitType _oldValue, SaveDataLimitType _newValue)
	{
		this.validate();
	}

	// Token: 0x060068CE RID: 26830 RVA: 0x002952FE File Offset: 0x002934FE
	public void Cancel()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.cleanLinks();
	}

	// Token: 0x060068CF RID: 26831 RVA: 0x00295322 File Offset: 0x00293522
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x060068D0 RID: 26832 RVA: 0x0029532C File Offset: 0x0029352C
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirm_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		bool value = this.cbxIsArchived.Value;
		long num = ((this.worldInfo.WorldSize != null) ? this.cbxSaveDataLimit.Value.CalculateTotalSize(this.worldInfo.WorldSize.Value) : this.saveInfo.SizeInfo.BytesReserved);
		XUiC_SaveManagementPrompt.SaveManagementMode saveManagementMode = this.mode;
		if (saveManagementMode != XUiC_SaveManagementPrompt.SaveManagementMode.Copy)
		{
			if (saveManagementMode == XUiC_SaveManagementPrompt.SaveManagementMode.Manage)
			{
				if (this.isMove)
				{
					this.performMove(this.txtSaveName.Text, this.cbxStorageType.Value, value, num);
				}
				else
				{
					this.performApply(value, num);
				}
			}
		}
		else
		{
			this.performCopy(this.txtSaveName.Text, this.cbxStorageType.Value, value, num);
		}
		this.cleanLinks();
	}

	// Token: 0x060068D1 RID: 26833 RVA: 0x0029540C File Offset: 0x0029360C
	[PublicizedFrom(EAccessModifier.Private)]
	public void performCopy(string _targetName, UserDataStorageType _storageType, bool _isArchived, long _reservedBytes)
	{
		UserDataManagement.GameSaveCopy gameSaveCopy;
		try
		{
			gameSaveCopy = new UserDataManagement.GameSaveCopy(this.saveInfo, _targetName, _storageType);
		}
		catch (Exception ex)
		{
			Log.Error("Failed to create save copy operation: " + ex.Message);
			return;
		}
		UserDataManagement.SaveCopyResultInfo saveCopyResultInfo = gameSaveCopy.PerformCopy();
		if (saveCopyResultInfo.Result == UserDataManagement.Result.Success)
		{
			UserDataManagement.SetSaveArchived(saveCopyResultInfo.NewSaveDir, _isArchived);
			if (!this.isRemoteSave)
			{
				UserDataManagement.SetSaveDataLimit(saveCopyResultInfo.NewSaveDir, _reservedBytes);
			}
		}
		UserDataStorageType userDataStorageType = (gameSaveCopy.WorldRequiresMoving ? _storageType : this.worldInfo.Location.StorageType);
		XUiC_SaveManagementPrompt.SaveManagementActionCopy onCopyConfirmed = this.OnCopyConfirmed;
		if (onCopyConfirmed == null)
		{
			return;
		}
		onCopyConfirmed(saveCopyResultInfo.Result, _targetName, this.worldInfo.Name, userDataStorageType);
	}

	// Token: 0x060068D2 RID: 26834 RVA: 0x002954C4 File Offset: 0x002936C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void performMove(string _targetName, UserDataStorageType _storageType, bool _isArchived, long _reservedBytes)
	{
		UserDataManagement.GameSaveMove gameSaveMove;
		try
		{
			gameSaveMove = new UserDataManagement.GameSaveMove(this.saveInfo, _targetName, _storageType);
		}
		catch (Exception ex)
		{
			Log.Error("Failed to create save move operation: " + ex.Message);
			return;
		}
		UserDataManagement.SaveMoveResultInfo saveMoveResultInfo = gameSaveMove.PerformMove();
		if (saveMoveResultInfo.Result == UserDataManagement.Result.Success)
		{
			UserDataManagement.SetSaveArchived(saveMoveResultInfo.NewSaveDir, _isArchived);
			if (!this.isRemoteSave)
			{
				UserDataManagement.SetSaveDataLimit(saveMoveResultInfo.NewSaveDir, _reservedBytes);
			}
		}
		UserDataStorageType userDataStorageType = (gameSaveMove.WorldRequiresMoving ? _storageType : this.worldInfo.Location.StorageType);
		XUiC_SaveManagementPrompt.SaveManagementActionMove onMoveConfirmed = this.OnMoveConfirmed;
		if (onMoveConfirmed == null)
		{
			return;
		}
		onMoveConfirmed(saveMoveResultInfo.Result, _targetName, this.worldInfo.Name, userDataStorageType);
	}

	// Token: 0x060068D3 RID: 26835 RVA: 0x0029557C File Offset: 0x0029377C
	[PublicizedFrom(EAccessModifier.Private)]
	public void performApply(bool _isArchived, long _reservedBytes)
	{
		UserDataManagement.SetSaveArchived(this.saveInfo.SaveDir, _isArchived);
		if (!this.isRemoteSave)
		{
			UserDataManagement.SetSaveDataLimit(this.saveInfo.SaveDir, _reservedBytes);
		}
		SaveInfoProvider.Instance.SetDirty();
		XUiC_SaveManagementPrompt.SaveManagementActionApply onApplyConfirmed = this.OnApplyConfirmed;
		if (onApplyConfirmed == null)
		{
			return;
		}
		onApplyConfirmed();
	}

	// Token: 0x060068D4 RID: 26836 RVA: 0x002955D0 File Offset: 0x002937D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setValues(XUiC_SaveManagementPrompt.SaveManagementMode _mode, SaveInfoProvider.SaveEntryInfo _saveInfo, SaveInfoProvider.WorldEntryInfo _worldInfo)
	{
		this.mode = _mode;
		this.saveInfo = _saveInfo;
		this.worldInfo = _worldInfo;
		this.isRemoteSave = _worldInfo.Type == SaveInfoProvider.RemoteWorldsType;
		this.ShowStorageType = PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
		this.CanChangeStorageType = true;
		this.txtSaveName.Text = ((_mode == XUiC_SaveManagementPrompt.SaveManagementMode.Copy) ? (_saveInfo.Name + " - Copy") : (_saveInfo.Name ?? ""));
		this.cbxIsArchived.Value = _saveInfo.StorageType.UsesDataLimit() && _saveInfo.SizeInfo.IsArchived;
		this.cbxSaveDataLimit.Min = null;
		if (_worldInfo.WorldSize != null)
		{
			SaveDataLimitType saveDataLimitType;
			if (!_saveInfo.StorageType.UsesDataLimit() && _saveInfo.SizeInfo.BytesReserved <= 0L)
			{
				this.cbxSaveDataLimit.Value = SaveDataLimitType.Unlimited;
			}
			else if (SaveDataLimitExtensions.TryCalculateSaveDataLimitType(_saveInfo.SizeInfo.BytesReserved, _worldInfo.WorldSize.Value, out saveDataLimitType))
			{
				this.cbxSaveDataLimit.Value = saveDataLimitType;
			}
			else
			{
				this.cbxSaveDataLimit.Value = SaveDataLimitType.VeryLong;
			}
			this.cbxStorageType.Value = _saveInfo.StorageType;
		}
		else if (_saveInfo.SizeInfo.BytesReserved > 0L)
		{
			this.cbxStorageType.Value = _saveInfo.StorageType;
			this.CanChangeStorageType = true;
		}
		else
		{
			this.cbxStorageType.Value = UserDataStorageType.DeviceLocal;
			this.CanChangeStorageType = false;
		}
		this.configureSaveDataLimitAllowedValues(this.cbxStorageType.Value);
		this.validate();
	}

	// Token: 0x060068D5 RID: 26837 RVA: 0x00295764 File Offset: 0x00293964
	public static void Show(XUi _xui, XUiC_DataManagementBar _dataManagementBar, XUiC_SaveManagementPrompt.SaveManagementMode _mode, SaveInfoProvider.SaveEntryInfo _saveInfo, SaveInfoProvider.WorldEntryInfo _worldInfo, XUiC_SaveManagementPrompt.SaveManagementActionCopy _onCopied, XUiC_SaveManagementPrompt.SaveManagementActionMove _onMoved, XUiC_SaveManagementPrompt.SaveManagementActionApply _onApplied)
	{
		XUiC_SaveManagementPrompt childByType = _xui.GetChildByType<XUiC_SaveManagementPrompt>();
		childByType.dataManagementBar = _dataManagementBar;
		childByType.OnCopyConfirmed = _onCopied;
		childByType.OnMoveConfirmed = _onMoved;
		childByType.OnApplyConfirmed = _onApplied;
		childByType.setValues(_mode, _saveInfo, _worldInfo);
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x0400503C RID: 20540
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400503D RID: 20541
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x0400503E RID: 20542
	[XuiBindComponent("txtSaveName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSaveName;

	// Token: 0x0400503F RID: 20543
	[XuiBindComponent("cbxStorageType", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxStorageType;

	// Token: 0x04005040 RID: 20544
	[XuiBindComponent("cbxIsArchived", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool cbxIsArchived;

	// Token: 0x04005041 RID: 20545
	[XuiBindComponent("cbxSaveDataLimit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x04005042 RID: 20546
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar dataManagementBar;

	// Token: 0x04005043 RID: 20547
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SaveManagementPrompt.SaveManagementMode mode;

	// Token: 0x04005046 RID: 20550
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRemoteSave;

	// Token: 0x04005048 RID: 20552
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMove;

	// Token: 0x04005049 RID: 20553
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.SaveEntryInfo saveInfo;

	// Token: 0x0400504A RID: 20554
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.WorldEntryInfo worldInfo;

	// Token: 0x02000D79 RID: 3449
	public enum SaveManagementMode
	{
		// Token: 0x0400504F RID: 20559
		Copy,
		// Token: 0x04005050 RID: 20560
		Manage
	}

	// Token: 0x02000D7A RID: 3450
	// (Invoke) Token: 0x060068D8 RID: 26840
	public delegate void SaveManagementActionCopy(UserDataManagement.Result _result, string _targetSaveName, string _worldName, UserDataStorageType _expectedWorldStorage);

	// Token: 0x02000D7B RID: 3451
	// (Invoke) Token: 0x060068DC RID: 26844
	public delegate void SaveManagementActionMove(UserDataManagement.Result _result, string _targetSaveName, string _worldName, UserDataStorageType _expectedWorldStorage);

	// Token: 0x02000D7C RID: 3452
	// (Invoke) Token: 0x060068E0 RID: 26848
	public delegate void SaveManagementActionApply();
}
