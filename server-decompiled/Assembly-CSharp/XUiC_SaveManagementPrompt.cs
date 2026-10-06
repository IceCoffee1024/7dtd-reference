using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D9B RID: 3483
[Preserve]
public class XUiC_SaveManagementPrompt : XUiController
{
	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x06006942 RID: 26946 RVA: 0x0029AFF1 File Offset: 0x002991F1
	// (set) Token: 0x06006943 RID: 26947 RVA: 0x0029AFF9 File Offset: 0x002991F9
	[XuiXmlBinding("showStorageType")]
	public bool ShowStorageType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x06006944 RID: 26948 RVA: 0x0029B002 File Offset: 0x00299202
	// (set) Token: 0x06006945 RID: 26949 RVA: 0x0029B00A File Offset: 0x0029920A
	[XuiXmlBinding("canChangeStorageType")]
	public bool CanChangeStorageType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x06006946 RID: 26950 RVA: 0x0029B013 File Offset: 0x00299213
	// (set) Token: 0x06006947 RID: 26951 RVA: 0x0029B01B File Offset: 0x0029921B
	[XuiXmlBinding("applyValid")]
	public bool ActionIsValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x06006948 RID: 26952 RVA: 0x0029B024 File Offset: 0x00299224
	[XuiXmlBinding("canCopy")]
	public bool CanCopy
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.isRemoteSave;
		}
	}

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x06006949 RID: 26953 RVA: 0x0029B024 File Offset: 0x00299224
	[XuiXmlBinding("showNameInput")]
	public bool ShowNameInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.isRemoteSave;
		}
	}

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x0600694A RID: 26954 RVA: 0x0029B02F File Offset: 0x0029922F
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

	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x0600694B RID: 26955 RVA: 0x0029B047 File Offset: 0x00299247
	[XuiXmlBinding("showIsArchived")]
	public bool ShowIsArchived
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.UserDataRoaming.IsSupported;
		}
	}

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x0600694C RID: 26956 RVA: 0x0029B058 File Offset: 0x00299258
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

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x0600694D RID: 26957 RVA: 0x0029B070 File Offset: 0x00299270
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

	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x0600694E RID: 26958 RVA: 0x0029B0B8 File Offset: 0x002992B8
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
	// (add) Token: 0x0600694F RID: 26959 RVA: 0x0029B0F0 File Offset: 0x002992F0
	// (remove) Token: 0x06006950 RID: 26960 RVA: 0x0029B128 File Offset: 0x00299328
	public event XUiC_SaveManagementPrompt.SaveManagementActionCopy OnCopyConfirmed;

	// Token: 0x14000090 RID: 144
	// (add) Token: 0x06006951 RID: 26961 RVA: 0x0029B160 File Offset: 0x00299360
	// (remove) Token: 0x06006952 RID: 26962 RVA: 0x0029B198 File Offset: 0x00299398
	public event XUiC_SaveManagementPrompt.SaveManagementActionMove OnMoveConfirmed;

	// Token: 0x14000091 RID: 145
	// (add) Token: 0x06006953 RID: 26963 RVA: 0x0029B1D0 File Offset: 0x002993D0
	// (remove) Token: 0x06006954 RID: 26964 RVA: 0x0029B208 File Offset: 0x00299408
	public event XUiC_SaveManagementPrompt.SaveManagementActionApply OnApplyConfirmed;

	// Token: 0x06006955 RID: 26965 RVA: 0x0029B23D File Offset: 0x0029943D
	public override void Init()
	{
		base.Init();
		this.txtSaveName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
	}

	// Token: 0x06006956 RID: 26966 RVA: 0x0029B264 File Offset: 0x00299464
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

	// Token: 0x06006957 RID: 26967 RVA: 0x0029B2B5 File Offset: 0x002994B5
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

	// Token: 0x06006958 RID: 26968 RVA: 0x0029B2E4 File Offset: 0x002994E4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006959 RID: 26969 RVA: 0x0029B340 File Offset: 0x00299540
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

	// Token: 0x0600695A RID: 26970 RVA: 0x0029B398 File Offset: 0x00299598
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

	// Token: 0x0600695B RID: 26971 RVA: 0x0029B448 File Offset: 0x00299648
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

	// Token: 0x0600695C RID: 26972 RVA: 0x0029B5B4 File Offset: 0x002997B4
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

	// Token: 0x0600695D RID: 26973 RVA: 0x0029B780 File Offset: 0x00299980
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateNameAvailable(string _name, UserDataStorageType _storageType)
	{
		return !string.IsNullOrEmpty(_name) && !SdDirectory.Exists(GameIO.GetSaveGameDir(this.worldInfo.Name, _name, _storageType));
	}

	// Token: 0x0600695E RID: 26974 RVA: 0x0029B7A6 File Offset: 0x002999A6
	[XuiBindEvent("OnChangeHandler", "txtSaveName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtSaveName_OnChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.validate();
	}

	// Token: 0x0600695F RID: 26975 RVA: 0x0029B7AE File Offset: 0x002999AE
	[XuiBindEvent("OnValueChanged", "cbxStorageType")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxStorageType_OnValueChanged(XUiController _sender, UserDataStorageType _oldValue, UserDataStorageType _newValue)
	{
		this.configureSaveDataLimitAllowedValues(_newValue);
		this.cbxIsArchived.Value = _newValue.UsesDataLimit() && this.cbxIsArchived.Value;
		this.validate();
	}

	// Token: 0x06006960 RID: 26976 RVA: 0x0029B7A6 File Offset: 0x002999A6
	[XuiBindEvent("OnValueChanged", "cbxIsArchived")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxIsArchived_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.validate();
	}

	// Token: 0x06006961 RID: 26977 RVA: 0x0029B7A6 File Offset: 0x002999A6
	[XuiBindEvent("OnValueChanged", "cbxSaveDataLimit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxSaveDataLimit_OnValueChanged(XUiController _sender, SaveDataLimitType _oldValue, SaveDataLimitType _newValue)
	{
		this.validate();
	}

	// Token: 0x06006962 RID: 26978 RVA: 0x0029B7DE File Offset: 0x002999DE
	public void Cancel()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.cleanLinks();
	}

	// Token: 0x06006963 RID: 26979 RVA: 0x0029B802 File Offset: 0x00299A02
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x06006964 RID: 26980 RVA: 0x0029B80C File Offset: 0x00299A0C
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

	// Token: 0x06006965 RID: 26981 RVA: 0x0029B8EC File Offset: 0x00299AEC
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

	// Token: 0x06006966 RID: 26982 RVA: 0x0029B9A4 File Offset: 0x00299BA4
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

	// Token: 0x06006967 RID: 26983 RVA: 0x0029BA5C File Offset: 0x00299C5C
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

	// Token: 0x06006968 RID: 26984 RVA: 0x0029BAB0 File Offset: 0x00299CB0
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

	// Token: 0x06006969 RID: 26985 RVA: 0x0029BC44 File Offset: 0x00299E44
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

	// Token: 0x04005115 RID: 20757
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005116 RID: 20758
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005117 RID: 20759
	[XuiBindComponent("txtSaveName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSaveName;

	// Token: 0x04005118 RID: 20760
	[XuiBindComponent("cbxStorageType", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxStorageType;

	// Token: 0x04005119 RID: 20761
	[XuiBindComponent("cbxIsArchived", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxBool cbxIsArchived;

	// Token: 0x0400511A RID: 20762
	[XuiBindComponent("cbxSaveDataLimit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x0400511B RID: 20763
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DataManagementBar dataManagementBar;

	// Token: 0x0400511C RID: 20764
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SaveManagementPrompt.SaveManagementMode mode;

	// Token: 0x0400511F RID: 20767
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRemoteSave;

	// Token: 0x04005121 RID: 20769
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMove;

	// Token: 0x04005122 RID: 20770
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.SaveEntryInfo saveInfo;

	// Token: 0x04005123 RID: 20771
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveInfoProvider.WorldEntryInfo worldInfo;

	// Token: 0x02000D9C RID: 3484
	public enum SaveManagementMode
	{
		// Token: 0x04005128 RID: 20776
		Copy,
		// Token: 0x04005129 RID: 20777
		Manage
	}

	// Token: 0x02000D9D RID: 3485
	// (Invoke) Token: 0x0600696C RID: 26988
	public delegate void SaveManagementActionCopy(UserDataManagement.Result _result, string _targetSaveName, string _worldName, UserDataStorageType _expectedWorldStorage);

	// Token: 0x02000D9E RID: 3486
	// (Invoke) Token: 0x06006970 RID: 26992
	public delegate void SaveManagementActionMove(UserDataManagement.Result _result, string _targetSaveName, string _worldName, UserDataStorageType _expectedWorldStorage);

	// Token: 0x02000D9F RID: 3487
	// (Invoke) Token: 0x06006974 RID: 26996
	public delegate void SaveManagementActionApply();
}
