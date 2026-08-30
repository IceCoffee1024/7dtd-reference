using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D7D RID: 3453
[Preserve]
public class XUiC_SaveSpaceNeeded : XUiController
{
	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x060068E3 RID: 26851 RVA: 0x002957B8 File Offset: 0x002939B8
	// (set) Token: 0x060068E4 RID: 26852 RVA: 0x002957C0 File Offset: 0x002939C0
	[XuiXmlBinding("canCancel")]
	public bool CanCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x060068E5 RID: 26853 RVA: 0x002957C9 File Offset: 0x002939C9
	// (set) Token: 0x060068E6 RID: 26854 RVA: 0x002957D1 File Offset: 0x002939D1
	[XuiXmlBinding("canDiscard")]
	public bool CanDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x060068E7 RID: 26855 RVA: 0x002957DA File Offset: 0x002939DA
	// (set) Token: 0x060068E8 RID: 26856 RVA: 0x002957E2 File Offset: 0x002939E2
	[XuiXmlBinding("langKeyTitle")]
	public string LangKeyTitle
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiSave";

	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x060068E9 RID: 26857 RVA: 0x002957EB File Offset: 0x002939EB
	// (set) Token: 0x060068EA RID: 26858 RVA: 0x002957F3 File Offset: 0x002939F3
	[XuiXmlBinding("langKeyBody")]
	public string LangKeyBody
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmSavingBody";

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x060068EB RID: 26859 RVA: 0x002957FC File Offset: 0x002939FC
	// (set) Token: 0x060068EC RID: 26860 RVA: 0x00295804 File Offset: 0x00293A04
	[XuiXmlBinding("langKeyCancel")]
	public string LangKeyCancel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiCancel";

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x060068ED RID: 26861 RVA: 0x0029580D File Offset: 0x00293A0D
	// (set) Token: 0x060068EE RID: 26862 RVA: 0x00295815 File Offset: 0x00293A15
	[XuiXmlBinding("langKeyDiscard")]
	public string LangKeyDiscard
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDiscard";

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x060068EF RID: 26863 RVA: 0x0029581E File Offset: 0x00293A1E
	// (set) Token: 0x060068F0 RID: 26864 RVA: 0x00295826 File Offset: 0x00293A26
	[XuiXmlBinding("langKeyConfirm")]
	public string LangKeyConfirm
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiConfirm";

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x060068F1 RID: 26865 RVA: 0x0029582F File Offset: 0x00293A2F
	// (set) Token: 0x060068F2 RID: 26866 RVA: 0x00295837 File Offset: 0x00293A37
	[XuiXmlBinding("langKeyManage")]
	public string LangKeyManage
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "xuiDmManageSaves";

	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x060068F3 RID: 26867 RVA: 0x00295840 File Offset: 0x00293A40
	[XuiXmlBinding("shouldShowDataBar")]
	public bool ShouldShowDataBar
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SaveInfoProvider.DataLimitEnabled && this.mStorageType.UsesDataLimit();
		}
	}

	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x060068F4 RID: 26868 RVA: 0x00295856 File Offset: 0x00293A56
	[XuiXmlBinding("hasSufficientSpace")]
	public bool HasSufficientSpace
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !SaveInfoProvider.DataLimitEnabled || !this.mStorageType.UsesDataLimit() || this.mPendingBytes <= this.mTotalAvailableBytes;
		}
	}

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x060068F5 RID: 26869 RVA: 0x0029587F File Offset: 0x00293A7F
	// (set) Token: 0x060068F6 RID: 26870 RVA: 0x00295887 File Offset: 0x00293A87
	public XUiC_SaveSpaceNeeded.ConfirmationResult Result
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060068F7 RID: 26871 RVA: 0x00295890 File Offset: 0x00293A90
	[XuiXmlAttribute("prompt_sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attribBrowseSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.mPromptReadyClip = _o;
		});
	}

	// Token: 0x060068F8 RID: 26872 RVA: 0x002958AA File Offset: 0x00293AAA
	public override void Init()
	{
		base.Init();
		XUiC_SaveSpaceNeeded.ID = base.WindowGroup.Id;
	}

	// Token: 0x060068F9 RID: 26873 RVA: 0x002958C2 File Offset: 0x00293AC2
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

	// Token: 0x060068FA RID: 26874 RVA: 0x002958FA File Offset: 0x00293AFA
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

	// Token: 0x060068FB RID: 26875 RVA: 0x00295934 File Offset: 0x00293B34
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

	// Token: 0x060068FC RID: 26876 RVA: 0x00295996 File Offset: 0x00293B96
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

	// Token: 0x060068FD RID: 26877 RVA: 0x002959D0 File Offset: 0x00293BD0
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

	// Token: 0x060068FE RID: 26878 RVA: 0x00295A28 File Offset: 0x00293C28
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

	// Token: 0x060068FF RID: 26879 RVA: 0x00295B3C File Offset: 0x00293D3C
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

	// Token: 0x06006900 RID: 26880 RVA: 0x00295C1C File Offset: 0x00293E1C
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

	// Token: 0x06006901 RID: 26881 RVA: 0x00295CF8 File Offset: 0x00293EF8
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

	// Token: 0x06006902 RID: 26882 RVA: 0x00295D98 File Offset: 0x00293F98
	public static XUiC_SaveSpaceNeeded Open(long _pendingBytes, string _protectedPath, UserDataStorageType _storageType, bool _onlyShowOnInsufficientSpace = false, bool _canCancel = true, bool _canDiscard = true, string _title = null, string _body = null, string _cancel = null, string _discard = null, string _confirm = null, string _manage = null)
	{
		return XUiC_SaveSpaceNeeded.Open(_pendingBytes, new string[] { _protectedPath }, _storageType, _onlyShowOnInsufficientSpace, _canCancel, _canDiscard, _title, _body, _cancel, _discard, _confirm, _manage);
	}

	// Token: 0x06006903 RID: 26883 RVA: 0x00295DC8 File Offset: 0x00293FC8
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

	// Token: 0x04005051 RID: 20561
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultTitle = "xuiSave";

	// Token: 0x04005052 RID: 20562
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultBody = "xuiDmSavingBody";

	// Token: 0x04005053 RID: 20563
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultCancel = "xuiCancel";

	// Token: 0x04005054 RID: 20564
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultDiscard = "xuiDiscard";

	// Token: 0x04005055 RID: 20565
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultConfirm = "xuiConfirm";

	// Token: 0x04005056 RID: 20566
	[PublicizedFrom(EAccessModifier.Private)]
	public const string LangKeyDefaultManage = "xuiDmManageSaves";

	// Token: 0x04005057 RID: 20567
	public static string ID = "";

	// Token: 0x04005058 RID: 20568
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool mOpenedProperly;

	// Token: 0x04005059 RID: 20569
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400505A RID: 20570
	[XuiBindComponent("btnDiscard", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscard;

	// Token: 0x0400505B RID: 20571
	[XuiBindComponent("btnManage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManage;

	// Token: 0x0400505C RID: 20572
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x0400505D RID: 20573
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x0400505E RID: 20574
	[PublicizedFrom(EAccessModifier.Private)]
	public long mPendingBytes;

	// Token: 0x0400505F RID: 20575
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] mProtectedPaths;

	// Token: 0x04005060 RID: 20576
	[PublicizedFrom(EAccessModifier.Private)]
	public UserDataStorageType mStorageType;

	// Token: 0x04005069 RID: 20585
	[PublicizedFrom(EAccessModifier.Private)]
	public long mTotalAvailableBytes;

	// Token: 0x0400506A RID: 20586
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorHidden;

	// Token: 0x0400506B RID: 20587
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mWasCursorLocked;

	// Token: 0x0400506C RID: 20588
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip mPromptReadyClip;

	// Token: 0x02000D7E RID: 3454
	public enum ConfirmationResult
	{
		// Token: 0x0400506F RID: 20591
		Pending,
		// Token: 0x04005070 RID: 20592
		Cancelled,
		// Token: 0x04005071 RID: 20593
		Discarded,
		// Token: 0x04005072 RID: 20594
		Confirmed
	}
}
