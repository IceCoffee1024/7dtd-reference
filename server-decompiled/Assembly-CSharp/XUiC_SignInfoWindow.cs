using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001052 RID: 4178
[Preserve]
public class XUiC_SignInfoWindow : XUiController
{
	// Token: 0x060083D8 RID: 33752 RVA: 0x0032563C File Offset: 0x0032383C
	public override void Init()
	{
		base.Init();
		this.signMaterial = base.GetChildById("signMaterial").ViewComponent as XUiV_Texture;
		this.signMaterial.CreateMaterial("Game/SignTech/UI");
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
		this.btnEdit = (XUiC_SimpleButton)base.GetChildById("btnEdit");
		this.btnDuplicate = (XUiC_SimpleButton)base.GetChildById("btnDuplicate");
		this.btnDelete = (XUiC_SimpleButton)base.GetChildById("btnDelete");
		this.btnExport = (XUiC_SimpleButton)base.GetChildById("btnExport");
		this.btnEdit.OnPressed += this.BtnEdit_OnPressed;
		this.btnDuplicate.OnPressed += delegate(XUiController _, int _)
		{
			this.DuplicateSign();
		};
		this.btnDelete.OnPressed += delegate(XUiController _, int _)
		{
			this.DeleteSign();
		};
		this.btnExport.OnPressed += this.BtnExport_OnPressed;
		this.btnSharing = (XUiC_SimpleButton)base.GetChildById("btnSharing");
		this.btnSharing.OnPressed += this.BtnSharing_OnPressed;
		this.confirmationPrompt = base.GetChildById("confirmation_prompt_controller") as XUiC_ConfirmationPrompt;
		this.sharingPrompt = base.GetChildByType<XUiC_SignSharingPrompt>();
		this.galleryWindow = this.windowGroup.Controller.GetChildByType<XUiC_SignGalleryWindow>();
	}

	// Token: 0x060083D9 RID: 33753 RVA: 0x003257CC File Offset: 0x003239CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExport_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.signId.IsValid || this.signData == null || !XUiC_SignInfoWindow.CheckCopyAllowed(this.signId))
		{
			return;
		}
		string text = SignTextureExporter.Instance.ExportSignToTexture(this.signData.name, this.signId);
		Log.Out("Sign png saved to: " + text);
	}

	// Token: 0x060083DA RID: 33754 RVA: 0x00325828 File Offset: 0x00323A28
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnEdit_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!XUiC_SignInfoWindow.CheckEditsAllowed(this.signId))
		{
			Log.Error(string.Format("Failed to edit sign with ID: {0}. Local user does not have permission over this sign.", this.signId));
			return;
		}
		this.galleryWindow.LaunchEditor();
	}

	// Token: 0x060083DB RID: 33755 RVA: 0x00325860 File Offset: 0x00323A60
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSharing_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.signData == null || !XUiC_SignInfoWindow.CheckEditsAllowed(this.signId) || !SignDataManager.IsPlayerLibraryId(this.signId.libraryId))
		{
			return;
		}
		this.sharingPrompt.ShowPrompt(SignDataManager.Instance.GetEffectiveSharing(this.signId), new Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing>(this.OnSharingPromptResult));
	}

	// Token: 0x060083DC RID: 33756 RVA: 0x003258BC File Offset: 0x00323ABC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSharingPromptResult(XUiC_SignSharingPrompt.Result _result, SignData.SignSharing _sharing)
	{
		if (_result != XUiC_SignSharingPrompt.Result.Saved || this.signData.sharing == _sharing)
		{
			return;
		}
		this.signData.sharing = _sharing;
		this.signData.lastModified = DateTime.Now;
		SignDataManager.Instance.MarkSignDirty(this.signId);
		base.RefreshBindings();
	}

	// Token: 0x060083DD RID: 33757 RVA: 0x00325910 File Offset: 0x00323B10
	[PublicizedFrom(EAccessModifier.Private)]
	public void DuplicateSign()
	{
		if (!XUiC_SignInfoWindow.CheckCopyAllowed(this.signId))
		{
			Log.Error(string.Format("Failed to duplicate sign with ID: {0}. Local user does not have permission to copy this sign.", this.signId));
			return;
		}
		string currentSaveTargetLibraryName = XUiC_SignGalleryWindow.GetCurrentSaveTargetLibraryName(this.galleryWindow.openedInEditMode);
		GlobalSignId globalSignId;
		SignDataManager.Instance.CopySignToLibrary(this.signData, currentSaveTargetLibraryName, out globalSignId);
		Action onSignAddOrRemove = this.OnSignAddOrRemove;
		if (onSignAddOrRemove != null)
		{
			onSignAddOrRemove();
		}
		this.galleryWindow.RefreshAndSelect(globalSignId);
	}

	// Token: 0x060083DE RID: 33758 RVA: 0x00325988 File Offset: 0x00323B88
	[PublicizedFrom(EAccessModifier.Private)]
	public void DeleteSign()
	{
		if (!XUiC_SignInfoWindow.CheckEditsAllowed(this.signId))
		{
			Log.Error(string.Format("Failed to delete sign with ID: {0}. Local user does not have permission over this sign.", this.signId));
			return;
		}
		this.confirmationPrompt.ShowPrompt(Localization.Get("lblSignDeleteTitle", false, null), Localization.Get("lblSignDeleteMessage", false, null), Localization.Get("xuiCancel", false, null), Localization.Get("xuiConfirm", false, null), new Action<XUiC_ConfirmationPrompt.Result>(this.OnDeletionPromptResult));
	}

	// Token: 0x060083DF RID: 33759 RVA: 0x00325A04 File Offset: 0x00323C04
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDeletionPromptResult(XUiC_ConfirmationPrompt.Result result)
	{
		if (result != XUiC_ConfirmationPrompt.Result.Cancelled && result == XUiC_ConfirmationPrompt.Result.Confirmed)
		{
			if (!SignDataManager.Instance.TryDeleteSign(this.signId))
			{
				Log.Error(string.Format("Failed to delete sign with ID: {0}", this.signId));
				return;
			}
			Action onSignAddOrRemove = this.OnSignAddOrRemove;
			if (onSignAddOrRemove != null)
			{
				onSignAddOrRemove();
			}
			this.galleryWindow.RefreshAndSelect(SignData.defaultSignDataID);
		}
	}

	// Token: 0x060083E0 RID: 33760 RVA: 0x00325A66 File Offset: 0x00323C66
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x060083E1 RID: 33761 RVA: 0x00325A81 File Offset: 0x00323C81
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInputs();
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060083E2 RID: 33762 RVA: 0x00325AAC File Offset: 0x00323CAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInputs()
	{
		if (this.confirmationPrompt.IsVisible || this.sharingPrompt.IsVisible || this.signData == null)
		{
			return;
		}
		if (InputUtils.ControlKeyPressed && Input.GetKeyDown(KeyCode.D))
		{
			this.DuplicateSign();
		}
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			this.DeleteSign();
		}
	}

	// Token: 0x17000F81 RID: 3969
	// (get) Token: 0x060083E3 RID: 33763 RVA: 0x00325B04 File Offset: 0x00323D04
	[XuiXmlBinding("signname")]
	public string SignName
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.signId.IsValid)
			{
				return Localization.Get("lblSignNone", false, null);
			}
			if (this.signData == null)
			{
				return string.Format(Localization.Get("lblSignMissing", false, null), this.signId.signGuid.ToString());
			}
			return this.signData.name;
		}
	}

	// Token: 0x17000F82 RID: 3970
	// (get) Token: 0x060083E4 RID: 33764 RVA: 0x00325B69 File Offset: 0x00323D69
	[XuiXmlBinding("libraryname")]
	public string LibraryName
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SignDataManager.GetLibraryNiceName(this.signId);
		}
	}

	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x060083E5 RID: 33765 RVA: 0x00325B76 File Offset: 0x00323D76
	[XuiXmlBinding("lastmodified")]
	public string LastModified
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.signId.IsValid || this.signData == null)
			{
				return "-";
			}
			return this.signData.lastModified.ToString("g");
		}
	}

	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x060083E6 RID: 33766 RVA: 0x00325BA8 File Offset: 0x00323DA8
	[XuiXmlBinding("showsharing")]
	public bool ShowSharing
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.signId.IsValid && this.signData != null && SignDataManager.IsPlayerLibraryId(this.signId.libraryId);
		}
	}

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x060083E7 RID: 33767 RVA: 0x00325BD1 File Offset: 0x00323DD1
	[XuiXmlBinding("sharinglevel")]
	public string SharingLevel
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.ShowSharing)
			{
				return "-";
			}
			return Localization.Get("lblSignSharing" + SignDataManager.Instance.GetEffectiveSharing(this.signId).ToStringCached<SignData.SignSharing>(), false, null);
		}
	}

	// Token: 0x060083E8 RID: 33768 RVA: 0x00325C08 File Offset: 0x00323E08
	public void SetSignInfo(GlobalSignId signId)
	{
		SignData signData;
		bool flag = SignDataManager.Instance.TryGetSignData(signId, out signData);
		this.signId = signId;
		this.signData = signData;
		this.signMaterial.IsVisible = signId.IsValid && flag;
		bool flag2 = flag && XUiC_SignInfoWindow.CheckEditsAllowed(signId);
		this.btnEdit.Enabled = flag2;
		this.btnDelete.Enabled = flag2;
		bool flag3 = flag && XUiC_SignInfoWindow.CheckCopyAllowed(signId);
		this.btnDuplicate.Enabled = flag3;
		this.btnExport.Enabled = flag3;
		bool flag4 = flag && SignDataManager.IsPlayerLibraryId(signId.libraryId);
		this.btnSharing.Enabled = flag2 && flag4;
		base.RefreshBindings();
	}

	// Token: 0x060083E9 RID: 33769 RVA: 0x00325CB8 File Offset: 0x00323EB8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool CheckEditsAllowed(GlobalSignId signId)
	{
		if (!signId.IsValid)
		{
			return false;
		}
		if (signId.libraryId == "[D]")
		{
			return false;
		}
		if (SignDataManager.localLibraryIds.Contains(signId.libraryId))
		{
			return false;
		}
		if (SignDataManager.IsPlayerLibraryId(signId.libraryId))
		{
			return XUiC_SignInfoWindow.IsLocallyOwned(signId);
		}
		return PrefabEditModeManager.Instance.IsActive();
	}

	// Token: 0x060083EA RID: 33770 RVA: 0x00325D18 File Offset: 0x00323F18
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool CheckCopyAllowed(GlobalSignId signId)
	{
		return signId.IsValid && !SignDataManager.localLibraryIds.Contains(signId.libraryId) && (!SignDataManager.IsPlayerLibraryId(signId.libraryId) || XUiC_SignInfoWindow.IsLocallyOwned(signId) || (SignDataManager.Instance.GetEffectiveSharing(signId) == SignData.SignSharing.Copyable && SignDataManager.IsLocalPlayerAlliedWith(signId.libraryId)));
	}

	// Token: 0x060083EB RID: 33771 RVA: 0x00325D78 File Offset: 0x00323F78
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsLocallyOwned(GlobalSignId signId)
	{
		return signId.libraryId == PlatformManager.InternalLocalUserIdentifier.CombinedString;
	}

	// Token: 0x040061F4 RID: 25076
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId = GlobalSignId.InvalidId;

	// Token: 0x040061F5 RID: 25077
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData signData;

	// Token: 0x040061F6 RID: 25078
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040061F7 RID: 25079
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnEdit;

	// Token: 0x040061F8 RID: 25080
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDuplicate;

	// Token: 0x040061F9 RID: 25081
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDelete;

	// Token: 0x040061FA RID: 25082
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExport;

	// Token: 0x040061FB RID: 25083
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSharing;

	// Token: 0x040061FC RID: 25084
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignGalleryWindow galleryWindow;

	// Token: 0x040061FD RID: 25085
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ConfirmationPrompt confirmationPrompt;

	// Token: 0x040061FE RID: 25086
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignSharingPrompt sharingPrompt;

	// Token: 0x040061FF RID: 25087
	public Action OnSignAddOrRemove;
}
