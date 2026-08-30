using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D91 RID: 3473
[Preserve]
public class XUiC_SaveSpaceNeeded : XUiController
{
	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x0600697F RID: 27007 RVA: 0x0029890C File Offset: 0x00296B0C
	// (set) Token: 0x06006980 RID: 27008 RVA: 0x00298914 File Offset: 0x00296B14
	[XuiXmlBinding("canCancel")]
	public bool CanCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x06006981 RID: 27009 RVA: 0x0029891D File Offset: 0x00296B1D
	// (set) Token: 0x06006982 RID: 27010 RVA: 0x00298925 File Offset: 0x00296B25
	[XuiXmlBinding("canDiscard")]
	public bool CanDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x06006983 RID: 27011 RVA: 0x0029892E File Offset: 0x00296B2E
	// (set) Token: 0x06006984 RID: 27012 RVA: 0x00298936 File Offset: 0x00296B36
	[XuiXmlBinding("langKeyTitle")]
	public string LangKeyTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiSave";

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06006985 RID: 27013 RVA: 0x0029893F File Offset: 0x00296B3F
	// (set) Token: 0x06006986 RID: 27014 RVA: 0x00298947 File Offset: 0x00296B47
	[XuiXmlBinding("langKeyBody")]
	public string LangKeyBody
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmSavingBody";

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x06006987 RID: 27015 RVA: 0x00298950 File Offset: 0x00296B50
	// (set) Token: 0x06006988 RID: 27016 RVA: 0x00298958 File Offset: 0x00296B58
	[XuiXmlBinding("langKeyCancel")]
	public string LangKeyCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiCancel";

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x06006989 RID: 27017 RVA: 0x00298961 File Offset: 0x00296B61
	// (set) Token: 0x0600698A RID: 27018 RVA: 0x00298969 File Offset: 0x00296B69
	[XuiXmlBinding("langKeyDiscard")]
	public string LangKeyDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDiscard";

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x0600698B RID: 27019 RVA: 0x00298972 File Offset: 0x00296B72
	// (set) Token: 0x0600698C RID: 27020 RVA: 0x0029897A File Offset: 0x00296B7A
	[XuiXmlBinding("langKeyConfirm")]
	public string LangKeyConfirm
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiConfirm";

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x0600698D RID: 27021 RVA: 0x00298983 File Offset: 0x00296B83
	// (set) Token: 0x0600698E RID: 27022 RVA: 0x0029898B File Offset: 0x00296B8B
	[XuiXmlBinding("langKeyManage")]
	public string LangKeyManage
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmManageSaves";

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x0600698F RID: 27023 RVA: 0x00298994 File Offset: 0x00296B94
	[XuiXmlBinding("shouldShowDataBar")]
	public bool ShouldShowDataBar
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SaveInfoProvider.DataLimitEnabled && this.mStorageType.UsesDataLimit();
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06006990 RID: 27024 RVA: 0x002989AA File Offset: 0x00296BAA
	[XuiXmlBinding("hasSufficientSpace")]
	public bool HasSufficientSpace
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !SaveInfoProvider.DataLimitEnabled || !this.mStorageType.UsesDataLimit() || this.mPendingBytes <= this.mTotalAvailableBytes;
		}
	}

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x06006991 RID: 27025 RVA: 0x002989D3 File Offset: 0x00296BD3
	// (set) Token: 0x06006992 RID: 27026 RVA: 0x002989DB File Offset: 0x00296BDB
	public XUiC_SaveSpaceNeeded.ConfirmationResult Result
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006993 RID: 27027 RVA: 0x002989E4 File Offset: 0x00296BE4
	[XuiXmlAttribute("prompt_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.mPromptReadyClip = _o;
		});
	}

	// Token: 0x06006994 RID: 27028 RVA: 0x002989FE File Offset: 0x00296BFE
	public override void Init()
	{
		base.Init();
		XUiC_SaveSpaceNeeded.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006995 RID: 27029 RVA: 0x00298A16 File Offset: 0x00296C16
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.CanCancel)
		{
			Log.Error("[SaveSpaceNeeded] Cancel button was pressed even though cancel is hidden?");
			return;
		}
		this.Result = XUiC_SaveSpaceNeeded.ConfirmationResult.Cancelled;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006996 RID: 27030 RVA: 0x00298A4E File Offset: 0x00296C4E
	[XuiBindEvent("OnPress", "btnDiscard")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDiscard_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.CanDiscard)
		{
			Log.Error("[SaveSpaceNeeded] Discard button was pressed even though discard is hidden?");
			return;
		}
		this.Result = XUiC_SaveSpaceNeeded.ConfirmationResult.Discarded;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006997 RID: 27031 RVA: 0x00298A88 File Offset: 0x00296C88
	[XuiBindEvent("OnPress", "btnManage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnManage_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiWindowGroup xuiWindowGroup = (XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow(XUiC_LoadingScreen.ID);
		object obj;
		if (xuiWindowGroup == null)
		{
			obj = null;
		}
		else
		{
			XUiController controller = xuiWindowGroup.Controller;
			obj = ((controller != null) ? controller.GetChildByType<XUiC_LoadingScreen>() : null);
		}
		object obj2 = obj;
		if (obj2 != null)
		{
			obj2.SetTipsVisible(false);
		}
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x06006998 RID: 27032 RVA: 0x00298AEA File Offset: 0x00296CEA
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirm_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.HasSufficientSpace)
		{
			Log.Error("[SaveSpaceNeeded] Confirm button was pressed even though there isn't enough free space?");
			return;
		}
		this.Result = XUiC_SaveSpaceNeeded.ConfirmationResult.Confirmed;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006999 RID: 27033 RVA: 0x00298B24 File Offset: 0x00296D24
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDataManagementWindowClosed()
	{
		XUiWindowGroup xuiWindowGroup = (XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow(XUiC_LoadingScreen.ID);
		object obj;
		if (xuiWindowGroup == null)
		{
			obj = null;
		}
		else
		{
			XUiController controller = xuiWindowGroup.Controller;
			obj = ((controller != null) ? controller.GetChildByType<XUiC_LoadingScreen>() : null);
		}
		object obj2 = obj;
		if (obj2 != null)
		{
			obj2.SetTipsVisible(true);
		}
		this.updateBarValues();
	}

	// Token: 0x0600699A RID: 27034 RVA: 0x00298B7C File Offset: 0x00296D7C
	public override void OnOpen()
	{
		base.OnOpen();
		if (!XUiC_SaveSpaceNeeded.mOpenedProperly)
		{
			Log.Error("[SaveSpaceNeeded] XUiC_SaveSpaceNeeded should be opened with the static Open method so that initInternal is executed.");
		}
		XUiC_SaveSpaceNeeded.mOpenedProperly = false;
		if (this.mProtectedPaths != null)
		{
			foreach (string text in this.mProtectedPaths)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					SaveInfoProvider.Instance.SetDirectoryProtected(text, true);
				}
			}
		}
		if (this.ShouldShowDataBar)
		{
			this.updateBarValues();
			if (this.mPendingBytes != 0L)
			{
				Log.Out("[SaveSpaceNeeded] Pending Bytes: " + this.mPendingBytes.FormatSize(true) + ", Total Available Bytes: " + this.mTotalAvailableBytes.FormatSize(true));
			}
		}
		Manager.PlayXUiSound(this.mPromptReadyClip, 1f);
		this.mWasCursorHidden = this.xui.playerUI.CursorController.GetCursorHidden();
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		this.mWasCursorLocked = this.xui.playerUI.CursorController.Locked;
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x0600699B RID: 27035 RVA: 0x00298C90 File Offset: 0x00296E90
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.CursorController.SetCursorHidden(this.mWasCursorHidden);
		this.xui.playerUI.CursorController.Locked = this.mWasCursorLocked;
		if (this.mProtectedPaths != null)
		{
			foreach (string text in this.mProtectedPaths)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					SaveInfoProvider.Instance.SetDirectoryProtected(text, false);
				}
			}
		}
		this.mPendingBytes = 0L;
		this.mProtectedPaths = null;
		this.CanCancel = true;
		this.CanDiscard = true;
		this.LangKeyTitle = "xuiSave";
		this.LangKeyBody = "xuiDmSavingBody";
		this.LangKeyCancel = "xuiCancel";
		this.LangKeyDiscard = "xuiDiscard";
		this.LangKeyConfirm = "xuiConfirm";
		this.LangKeyManage = "xuiDmManageSaves";
	}

	// Token: 0x0600699C RID: 27036 RVA: 0x00298D70 File Offset: 0x00296F70
	[PublicizedFrom(EAccessModifier.Private)]
	public void initInternal(long _pendingBytes, string[] _protectedPaths, UserDataStorageType _storageType, bool _canCancel, bool _canDiscard, string _langKeyTitle, string _langKeyBody, string _langKeyCancel, string _langKeyDiscard, string _langKeyConfirm, string _langKeyManage)
	{
		this.mPendingBytes = _pendingBytes;
		this.mProtectedPaths = _protectedPaths;
		this.mStorageType = _storageType;
		this.CanCancel = _canCancel;
		this.CanDiscard = _canDiscard;
		this.LangKeyTitle = (string.IsNullOrWhiteSpace(_langKeyTitle) ? "xuiSave" : _langKeyTitle);
		this.LangKeyBody = (string.IsNullOrWhiteSpace(_langKeyBody) ? "xuiDmSavingBody" : _langKeyBody);
		this.LangKeyCancel = (string.IsNullOrWhiteSpace(_langKeyCancel) ? "xuiCancel" : _langKeyCancel);
		this.LangKeyDiscard = (string.IsNullOrWhiteSpace(_langKeyDiscard) ? "xuiDiscard" : _langKeyDiscard);
		this.LangKeyConfirm = (string.IsNullOrWhiteSpace(_langKeyConfirm) ? "xuiConfirm" : _langKeyConfirm);
		this.LangKeyManage = (string.IsNullOrWhiteSpace(_langKeyManage) ? "xuiDmManageSaves" : _langKeyManage);
		this.mTotalAvailableBytes = SaveInfoProvider.Instance.TotalAvailableBytes;
		this.Result = XUiC_SaveSpaceNeeded.ConfirmationResult.Pending;
	}

	// Token: 0x0600699D RID: 27037 RVA: 0x00298E4C File Offset: 0x0029704C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBarValues()
	{
		SaveInfoProvider instance = SaveInfoProvider.Instance;
		this.dataManagementBar.ViewComponent.IsVisible = SaveInfoProvider.DataLimitEnabled;
		this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Preview);
		this.dataManagementBar.SetUsedBytes(instance.TotalUsedBytes);
		this.dataManagementBar.SetAllowanceBytes(instance.TotalAllowanceBytes);
		this.dataManagementBar.SetPendingBytes(this.mPendingBytes);
		this.mTotalAvailableBytes = instance.TotalAvailableBytes;
		base.RefreshBindings();
		if (this.HasSufficientSpace)
		{
			this.btnConfirm.SelectCursorElement(true, false);
			return;
		}
		this.btnManage.SelectCursorElement(true, false);
	}

	// Token: 0x0600699E RID: 27038 RVA: 0x00298EEC File Offset: 0x002970EC
	public static XUiC_SaveSpaceNeeded Open(long _pendingBytes, string _protectedPath, UserDataStorageType _storageType, bool _onlyShowOnInsufficientSpace = false, bool _canCancel = true, bool _canDiscard = true, string _title = null, string _body = null, string _cancel = null, string _discard = null, string _confirm = null, string _manage = null)
	{
		return XUiC_SaveSpaceNeeded.Open(_pendingBytes, new string[] { _protectedPath }, _storageType, _onlyShowOnInsufficientSpace, _canCancel, _canDiscard, _title, _body, _cancel, _discard, _confirm, _manage);
	}

	// Token: 0x0600699F RID: 27039 RVA: 0x00298F1C File Offset: 0x0029711C
	public static XUiC_SaveSpaceNeeded Open(long _pendingBytes, string[] _protectedPaths, UserDataStorageType _storageType, bool _onlyShowOnInsufficientSpace = false, bool _canCancel = true, bool _canDiscard = true, string _title = null, string _body = null, string _cancel = null, string _discard = null, string _confirm = null, string _manage = null)
	{
		if (_onlyShowOnInsufficientSpace)
		{
			if (!SaveInfoProvider.DataLimitEnabled || !_storageType.UsesDataLimit())
			{
				return null;
			}
			if (_pendingBytes <= SaveInfoProvider.Instance.TotalAvailableBytes)
			{
				return null;
			}
		}
		GUIWindowManager windowManager = LocalPlayerUI.primaryUI.xui.playerUI.windowManager;
		XUiWindowGroup xuiWindowGroup = (XUiWindowGroup)windowManager.GetWindow(XUiC_SaveSpaceNeeded.ID);
		XUiC_SaveSpaceNeeded xuiC_SaveSpaceNeeded;
		if (xuiWindowGroup == null)
		{
			xuiC_SaveSpaceNeeded = null;
		}
		else
		{
			XUiController controller = xuiWindowGroup.Controller;
			xuiC_SaveSpaceNeeded = ((controller != null) ? controller.GetChildByType<XUiC_SaveSpaceNeeded>() : null);
		}
		XUiC_SaveSpaceNeeded xuiC_SaveSpaceNeeded2 = xuiC_SaveSpaceNeeded;
		if (xuiC_SaveSpaceNeeded2 == null)
		{
			Log.Error("[SaveSpaceNeeded] Failed to retrieve reference to XUiC_SaveSpaceNeeded instance.");
			return null;
		}
		xuiC_SaveSpaceNeeded2.initInternal(_pendingBytes, _protectedPaths, _storageType, _canCancel, _canDiscard, _title, _body, _cancel, _discard, _confirm, _manage);
		XUiC_SaveSpaceNeeded.mOpenedProperly = true;
		windowManager.Open(XUiC_SaveSpaceNeeded.ID, false);
		XUiC_SaveSpaceNeeded.mOpenedProperly = false;
		return xuiC_SaveSpaceNeeded2;
	}

	// Token: 0x040050F3 RID: 20723
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultTitle = "xuiSave";

	// Token: 0x040050F4 RID: 20724
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultBody = "xuiDmSavingBody";

	// Token: 0x040050F5 RID: 20725
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultCancel = "xuiCancel";

	// Token: 0x040050F6 RID: 20726
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultDiscard = "xuiDiscard";

	// Token: 0x040050F7 RID: 20727
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultConfirm = "xuiConfirm";

	// Token: 0x040050F8 RID: 20728
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultManage = "xuiDmManageSaves";

	// Token: 0x040050F9 RID: 20729
	public static string ID = "";

	// Token: 0x040050FA RID: 20730
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool mOpenedProperly;

	// Token: 0x040050FB RID: 20731
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040050FC RID: 20732
	[XuiBindComponent("btnDiscard", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscard;

	// Token: 0x040050FD RID: 20733
	[XuiBindComponent("btnManage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManage;

	// Token: 0x040050FE RID: 20734
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x040050FF RID: 20735
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x04005100 RID: 20736
	[PublicizedFrom(EAccessModifier.Private)]
	public long mPendingBytes;

	// Token: 0x04005101 RID: 20737
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] mProtectedPaths;

	// Token: 0x04005102 RID: 20738
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataStorageType mStorageType;

	// Token: 0x0400510B RID: 20747
	[PublicizedFrom(EAccessModifier.Private)]
	public long mTotalAvailableBytes;

	// Token: 0x0400510C RID: 20748
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorHidden;

	// Token: 0x0400510D RID: 20749
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorLocked;

	// Token: 0x0400510E RID: 20750
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip mPromptReadyClip;

	// Token: 0x02000D92 RID: 3474
	public enum ConfirmationResult
	{
		// Token: 0x04005111 RID: 20753
		Pending,
		// Token: 0x04005112 RID: 20754
		Cancelled,
		// Token: 0x04005113 RID: 20755
		Discarded,
		// Token: 0x04005114 RID: 20756
		Confirmed
	}
}
