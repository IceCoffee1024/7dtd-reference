using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DAB RID: 3499
[Preserve]
public class XUiC_SaveSpaceNeeded : XUiController
{
	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x0600699C RID: 27036 RVA: 0x0029D1C0 File Offset: 0x0029B3C0
	// (set) Token: 0x0600699D RID: 27037 RVA: 0x0029D1C8 File Offset: 0x0029B3C8
	[XuiXmlBinding("canCancel")]
	public bool CanCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x0600699E RID: 27038 RVA: 0x0029D1D1 File Offset: 0x0029B3D1
	// (set) Token: 0x0600699F RID: 27039 RVA: 0x0029D1D9 File Offset: 0x0029B3D9
	[XuiXmlBinding("canDiscard")]
	public bool CanDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x060069A0 RID: 27040 RVA: 0x0029D1E2 File Offset: 0x0029B3E2
	// (set) Token: 0x060069A1 RID: 27041 RVA: 0x0029D1EA File Offset: 0x0029B3EA
	[XuiXmlBinding("langKeyTitle")]
	public string LangKeyTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiSave";

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x060069A2 RID: 27042 RVA: 0x0029D1F3 File Offset: 0x0029B3F3
	// (set) Token: 0x060069A3 RID: 27043 RVA: 0x0029D1FB File Offset: 0x0029B3FB
	[XuiXmlBinding("langKeyBody")]
	public string LangKeyBody
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmSavingBody";

	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x060069A4 RID: 27044 RVA: 0x0029D204 File Offset: 0x0029B404
	// (set) Token: 0x060069A5 RID: 27045 RVA: 0x0029D20C File Offset: 0x0029B40C
	[XuiXmlBinding("langKeyCancel")]
	public string LangKeyCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiCancel";

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x060069A6 RID: 27046 RVA: 0x0029D215 File Offset: 0x0029B415
	// (set) Token: 0x060069A7 RID: 27047 RVA: 0x0029D21D File Offset: 0x0029B41D
	[XuiXmlBinding("langKeyDiscard")]
	public string LangKeyDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDiscard";

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x060069A8 RID: 27048 RVA: 0x0029D226 File Offset: 0x0029B426
	// (set) Token: 0x060069A9 RID: 27049 RVA: 0x0029D22E File Offset: 0x0029B42E
	[XuiXmlBinding("langKeyConfirm")]
	public string LangKeyConfirm
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiConfirm";

	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x060069AA RID: 27050 RVA: 0x0029D237 File Offset: 0x0029B437
	// (set) Token: 0x060069AB RID: 27051 RVA: 0x0029D23F File Offset: 0x0029B43F
	[XuiXmlBinding("langKeyManage")]
	public string LangKeyManage
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmManageSaves";

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x060069AC RID: 27052 RVA: 0x0029D248 File Offset: 0x0029B448
	[XuiXmlBinding("shouldShowDataBar")]
	public bool ShouldShowDataBar
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SaveInfoProvider.DataLimitEnabled && this.mStorageType.UsesDataLimit();
		}
	}

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x060069AD RID: 27053 RVA: 0x0029D25E File Offset: 0x0029B45E
	[XuiXmlBinding("hasSufficientSpace")]
	public bool HasSufficientSpace
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !SaveInfoProvider.DataLimitEnabled || !this.mStorageType.UsesDataLimit() || this.mPendingBytes <= this.mTotalAvailableBytes;
		}
	}

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x060069AE RID: 27054 RVA: 0x0029D287 File Offset: 0x0029B487
	// (set) Token: 0x060069AF RID: 27055 RVA: 0x0029D28F File Offset: 0x0029B48F
	public XUiC_SaveSpaceNeeded.ConfirmationResult Result
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060069B0 RID: 27056 RVA: 0x0029D298 File Offset: 0x0029B498
	[XuiXmlAttribute("prompt_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.mPromptReadyClip = _o;
		});
	}

	// Token: 0x060069B1 RID: 27057 RVA: 0x0029D2B2 File Offset: 0x0029B4B2
	public override void Init()
	{
		base.Init();
		XUiC_SaveSpaceNeeded.ID = base.WindowGroup.Id;
	}

	// Token: 0x060069B2 RID: 27058 RVA: 0x0029D2CA File Offset: 0x0029B4CA
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

	// Token: 0x060069B3 RID: 27059 RVA: 0x0029D302 File Offset: 0x0029B502
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

	// Token: 0x060069B4 RID: 27060 RVA: 0x0029D33C File Offset: 0x0029B53C
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

	// Token: 0x060069B5 RID: 27061 RVA: 0x0029D39E File Offset: 0x0029B59E
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

	// Token: 0x060069B6 RID: 27062 RVA: 0x0029D3D8 File Offset: 0x0029B5D8
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

	// Token: 0x060069B7 RID: 27063 RVA: 0x0029D430 File Offset: 0x0029B630
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

	// Token: 0x060069B8 RID: 27064 RVA: 0x0029D544 File Offset: 0x0029B744
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

	// Token: 0x060069B9 RID: 27065 RVA: 0x0029D624 File Offset: 0x0029B824
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

	// Token: 0x060069BA RID: 27066 RVA: 0x0029D700 File Offset: 0x0029B900
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

	// Token: 0x060069BB RID: 27067 RVA: 0x0029D7A0 File Offset: 0x0029B9A0
	public static XUiC_SaveSpaceNeeded Open(long _pendingBytes, string _protectedPath, UserDataStorageType _storageType, bool _onlyShowOnInsufficientSpace = false, bool _canCancel = true, bool _canDiscard = true, string _title = null, string _body = null, string _cancel = null, string _discard = null, string _confirm = null, string _manage = null)
	{
		return XUiC_SaveSpaceNeeded.Open(_pendingBytes, new string[] { _protectedPath }, _storageType, _onlyShowOnInsufficientSpace, _canCancel, _canDiscard, _title, _body, _cancel, _discard, _confirm, _manage);
	}

	// Token: 0x060069BC RID: 27068 RVA: 0x0029D7D0 File Offset: 0x0029B9D0
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

	// Token: 0x0400514F RID: 20815
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultTitle = "xuiSave";

	// Token: 0x04005150 RID: 20816
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultBody = "xuiDmSavingBody";

	// Token: 0x04005151 RID: 20817
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultCancel = "xuiCancel";

	// Token: 0x04005152 RID: 20818
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultDiscard = "xuiDiscard";

	// Token: 0x04005153 RID: 20819
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultConfirm = "xuiConfirm";

	// Token: 0x04005154 RID: 20820
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultManage = "xuiDmManageSaves";

	// Token: 0x04005155 RID: 20821
	public static string ID = "";

	// Token: 0x04005156 RID: 20822
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool mOpenedProperly;

	// Token: 0x04005157 RID: 20823
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005158 RID: 20824
	[XuiBindComponent("btnDiscard", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscard;

	// Token: 0x04005159 RID: 20825
	[XuiBindComponent("btnManage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManage;

	// Token: 0x0400515A RID: 20826
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x0400515B RID: 20827
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x0400515C RID: 20828
	[PublicizedFrom(EAccessModifier.Private)]
	public long mPendingBytes;

	// Token: 0x0400515D RID: 20829
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] mProtectedPaths;

	// Token: 0x0400515E RID: 20830
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataStorageType mStorageType;

	// Token: 0x04005167 RID: 20839
	[PublicizedFrom(EAccessModifier.Private)]
	public long mTotalAvailableBytes;

	// Token: 0x04005168 RID: 20840
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorHidden;

	// Token: 0x04005169 RID: 20841
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorLocked;

	// Token: 0x0400516A RID: 20842
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip mPromptReadyClip;

	// Token: 0x02000DAC RID: 3500
	public enum ConfirmationResult
	{
		// Token: 0x0400516D RID: 20845
		Pending,
		// Token: 0x0400516E RID: 20846
		Cancelled,
		// Token: 0x0400516F RID: 20847
		Discarded,
		// Token: 0x04005170 RID: 20848
		Confirmed
	}
}
