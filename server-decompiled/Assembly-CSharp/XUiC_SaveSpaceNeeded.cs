using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DA0 RID: 3488
[Preserve]
public class XUiC_SaveSpaceNeeded : XUiController
{
	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x06006977 RID: 26999 RVA: 0x0029BC98 File Offset: 0x00299E98
	// (set) Token: 0x06006978 RID: 27000 RVA: 0x0029BCA0 File Offset: 0x00299EA0
	[XuiXmlBinding("canCancel")]
	public bool CanCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x06006979 RID: 27001 RVA: 0x0029BCA9 File Offset: 0x00299EA9
	// (set) Token: 0x0600697A RID: 27002 RVA: 0x0029BCB1 File Offset: 0x00299EB1
	[XuiXmlBinding("canDiscard")]
	public bool CanDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x0600697B RID: 27003 RVA: 0x0029BCBA File Offset: 0x00299EBA
	// (set) Token: 0x0600697C RID: 27004 RVA: 0x0029BCC2 File Offset: 0x00299EC2
	[XuiXmlBinding("langKeyTitle")]
	public string LangKeyTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiSave";

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x0600697D RID: 27005 RVA: 0x0029BCCB File Offset: 0x00299ECB
	// (set) Token: 0x0600697E RID: 27006 RVA: 0x0029BCD3 File Offset: 0x00299ED3
	[XuiXmlBinding("langKeyBody")]
	public string LangKeyBody
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmSavingBody";

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x0600697F RID: 27007 RVA: 0x0029BCDC File Offset: 0x00299EDC
	// (set) Token: 0x06006980 RID: 27008 RVA: 0x0029BCE4 File Offset: 0x00299EE4
	[XuiXmlBinding("langKeyCancel")]
	public string LangKeyCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiCancel";

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x06006981 RID: 27009 RVA: 0x0029BCED File Offset: 0x00299EED
	// (set) Token: 0x06006982 RID: 27010 RVA: 0x0029BCF5 File Offset: 0x00299EF5
	[XuiXmlBinding("langKeyDiscard")]
	public string LangKeyDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDiscard";

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x06006983 RID: 27011 RVA: 0x0029BCFE File Offset: 0x00299EFE
	// (set) Token: 0x06006984 RID: 27012 RVA: 0x0029BD06 File Offset: 0x00299F06
	[XuiXmlBinding("langKeyConfirm")]
	public string LangKeyConfirm
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiConfirm";

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x06006985 RID: 27013 RVA: 0x0029BD0F File Offset: 0x00299F0F
	// (set) Token: 0x06006986 RID: 27014 RVA: 0x0029BD17 File Offset: 0x00299F17
	[XuiXmlBinding("langKeyManage")]
	public string LangKeyManage
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmManageSaves";

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x06006987 RID: 27015 RVA: 0x0029BD20 File Offset: 0x00299F20
	[XuiXmlBinding("shouldShowDataBar")]
	public bool ShouldShowDataBar
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SaveInfoProvider.DataLimitEnabled && this.mStorageType.UsesDataLimit();
		}
	}

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x06006988 RID: 27016 RVA: 0x0029BD36 File Offset: 0x00299F36
	[XuiXmlBinding("hasSufficientSpace")]
	public bool HasSufficientSpace
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !SaveInfoProvider.DataLimitEnabled || !this.mStorageType.UsesDataLimit() || this.mPendingBytes <= this.mTotalAvailableBytes;
		}
	}

	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x06006989 RID: 27017 RVA: 0x0029BD5F File Offset: 0x00299F5F
	// (set) Token: 0x0600698A RID: 27018 RVA: 0x0029BD67 File Offset: 0x00299F67
	public XUiC_SaveSpaceNeeded.ConfirmationResult Result
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600698B RID: 27019 RVA: 0x0029BD70 File Offset: 0x00299F70
	[XuiXmlAttribute("prompt_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.mPromptReadyClip = _o;
		});
	}

	// Token: 0x0600698C RID: 27020 RVA: 0x0029BD8A File Offset: 0x00299F8A
	public override void Init()
	{
		base.Init();
		XUiC_SaveSpaceNeeded.ID = base.WindowGroup.Id;
	}

	// Token: 0x0600698D RID: 27021 RVA: 0x0029BDA2 File Offset: 0x00299FA2
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

	// Token: 0x0600698E RID: 27022 RVA: 0x0029BDDA File Offset: 0x00299FDA
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

	// Token: 0x0600698F RID: 27023 RVA: 0x0029BE14 File Offset: 0x0029A014
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

	// Token: 0x06006990 RID: 27024 RVA: 0x0029BE76 File Offset: 0x0029A076
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

	// Token: 0x06006991 RID: 27025 RVA: 0x0029BEB0 File Offset: 0x0029A0B0
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

	// Token: 0x06006992 RID: 27026 RVA: 0x0029BF08 File Offset: 0x0029A108
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

	// Token: 0x06006993 RID: 27027 RVA: 0x0029C01C File Offset: 0x0029A21C
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

	// Token: 0x06006994 RID: 27028 RVA: 0x0029C0FC File Offset: 0x0029A2FC
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

	// Token: 0x06006995 RID: 27029 RVA: 0x0029C1D8 File Offset: 0x0029A3D8
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

	// Token: 0x06006996 RID: 27030 RVA: 0x0029C278 File Offset: 0x0029A478
	public static XUiC_SaveSpaceNeeded Open(long _pendingBytes, string _protectedPath, UserDataStorageType _storageType, bool _onlyShowOnInsufficientSpace = false, bool _canCancel = true, bool _canDiscard = true, string _title = null, string _body = null, string _cancel = null, string _discard = null, string _confirm = null, string _manage = null)
	{
		return XUiC_SaveSpaceNeeded.Open(_pendingBytes, new string[] { _protectedPath }, _storageType, _onlyShowOnInsufficientSpace, _canCancel, _canDiscard, _title, _body, _cancel, _discard, _confirm, _manage);
	}

	// Token: 0x06006997 RID: 27031 RVA: 0x0029C2A8 File Offset: 0x0029A4A8
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

	// Token: 0x0400512A RID: 20778
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultTitle = "xuiSave";

	// Token: 0x0400512B RID: 20779
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultBody = "xuiDmSavingBody";

	// Token: 0x0400512C RID: 20780
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultCancel = "xuiCancel";

	// Token: 0x0400512D RID: 20781
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultDiscard = "xuiDiscard";

	// Token: 0x0400512E RID: 20782
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultConfirm = "xuiConfirm";

	// Token: 0x0400512F RID: 20783
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultManage = "xuiDmManageSaves";

	// Token: 0x04005130 RID: 20784
	public static string ID = "";

	// Token: 0x04005131 RID: 20785
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool mOpenedProperly;

	// Token: 0x04005132 RID: 20786
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005133 RID: 20787
	[XuiBindComponent("btnDiscard", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscard;

	// Token: 0x04005134 RID: 20788
	[XuiBindComponent("btnManage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManage;

	// Token: 0x04005135 RID: 20789
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005136 RID: 20790
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x04005137 RID: 20791
	[PublicizedFrom(EAccessModifier.Private)]
	public long mPendingBytes;

	// Token: 0x04005138 RID: 20792
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] mProtectedPaths;

	// Token: 0x04005139 RID: 20793
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataStorageType mStorageType;

	// Token: 0x04005142 RID: 20802
	[PublicizedFrom(EAccessModifier.Private)]
	public long mTotalAvailableBytes;

	// Token: 0x04005143 RID: 20803
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorHidden;

	// Token: 0x04005144 RID: 20804
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorLocked;

	// Token: 0x04005145 RID: 20805
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip mPromptReadyClip;

	// Token: 0x02000DA1 RID: 3489
	public enum ConfirmationResult
	{
		// Token: 0x04005148 RID: 20808
		Pending,
		// Token: 0x04005149 RID: 20809
		Cancelled,
		// Token: 0x0400514A RID: 20810
		Discarded,
		// Token: 0x0400514B RID: 20811
		Confirmed
	}
}
