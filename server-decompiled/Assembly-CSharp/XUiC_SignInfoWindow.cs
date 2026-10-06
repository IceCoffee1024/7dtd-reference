using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001042 RID: 4162
[Preserve]
public class XUiC_SignInfoWindow : XUiController
{
	// Token: 0x06008386 RID: 33670 RVA: 0x00323938 File Offset: 0x00321B38
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

	// Token: 0x06008387 RID: 33671 RVA: 0x00323AC8 File Offset: 0x00321CC8
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

	// Token: 0x06008388 RID: 33672 RVA: 0x00323B24 File Offset: 0x00321D24
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

	// Token: 0x06008389 RID: 33673 RVA: 0x00323B5C File Offset: 0x00321D5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSharing_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.signData == null || !XUiC_SignInfoWindow.CheckEditsAllowed(this.signId) || !SignDataManager.IsPlayerLibraryId(this.signId.libraryId))
		{
			return;
		}
		this.sharingPrompt.ShowPrompt(SignDataManager.Instance.GetEffectiveSharing(this.signId), new Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing>(this.OnSharingPromptResult));
	}

	// Token: 0x0600838A RID: 33674 RVA: 0x00323BB8 File Offset: 0x00321DB8
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

	// Token: 0x0600838B RID: 33675 RVA: 0x00323C0C File Offset: 0x00321E0C
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

	// Token: 0x0600838C RID: 33676 RVA: 0x00323C84 File Offset: 0x00321E84
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

	// Token: 0x0600838D RID: 33677 RVA: 0x00323D00 File Offset: 0x00321F00
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

	// Token: 0x0600838E RID: 33678 RVA: 0x00323D62 File Offset: 0x00321F62
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x0600838F RID: 33679 RVA: 0x00323D7D File Offset: 0x00321F7D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInputs();
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x06008390 RID: 33680 RVA: 0x00323DA8 File Offset: 0x00321FA8
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

	// Token: 0x17000F76 RID: 3958
	// (get) Token: 0x06008391 RID: 33681 RVA: 0x00323E00 File Offset: 0x00322000
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

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x06008392 RID: 33682 RVA: 0x00323E65 File Offset: 0x00322065
	[XuiXmlBinding("libraryname")]
	public string LibraryName
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SignDataManager.GetLibraryNiceName(this.signId);
		}
	}

	// Token: 0x17000F78 RID: 3960
	// (get) Token: 0x06008393 RID: 33683 RVA: 0x00323E72 File Offset: 0x00322072
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

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x06008394 RID: 33684 RVA: 0x00323EA4 File Offset: 0x003220A4
	[XuiXmlBinding("showsharing")]
	public bool ShowSharing
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.signId.IsValid && this.signData != null && SignDataManager.IsPlayerLibraryId(this.signId.libraryId);
		}
	}

	// Token: 0x17000F7A RID: 3962
	// (get) Token: 0x06008395 RID: 33685 RVA: 0x00323ECD File Offset: 0x003220CD
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

	// Token: 0x06008396 RID: 33686 RVA: 0x00323F04 File Offset: 0x00322104
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

	// Token: 0x06008397 RID: 33687 RVA: 0x00323FB4 File Offset: 0x003221B4
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

	// Token: 0x06008398 RID: 33688 RVA: 0x00324014 File Offset: 0x00322214
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool CheckCopyAllowed(GlobalSignId signId)
	{
		return signId.IsValid && !SignDataManager.localLibraryIds.Contains(signId.libraryId) && (!SignDataManager.IsPlayerLibraryId(signId.libraryId) || XUiC_SignInfoWindow.IsLocallyOwned(signId) || (SignDataManager.Instance.GetEffectiveSharing(signId) == SignData.SignSharing.Copyable && SignDataManager.IsLocalPlayerAlliedWith(signId.libraryId)));
	}

	// Token: 0x06008399 RID: 33689 RVA: 0x00324074 File Offset: 0x00322274
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsLocallyOwned(GlobalSignId signId)
	{
		return signId.libraryId == PlatformManager.InternalLocalUserIdentifier.CombinedString;
	}

	// Token: 0x040061B6 RID: 25014
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId = GlobalSignId.InvalidId;

	// Token: 0x040061B7 RID: 25015
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData signData;

	// Token: 0x040061B8 RID: 25016
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040061B9 RID: 25017
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnEdit;

	// Token: 0x040061BA RID: 25018
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDuplicate;

	// Token: 0x040061BB RID: 25019
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDelete;

	// Token: 0x040061BC RID: 25020
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExport;

	// Token: 0x040061BD RID: 25021
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSharing;

	// Token: 0x040061BE RID: 25022
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignGalleryWindow galleryWindow;

	// Token: 0x040061BF RID: 25023
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ConfirmationPrompt confirmationPrompt;

	// Token: 0x040061C0 RID: 25024
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignSharingPrompt sharingPrompt;

	// Token: 0x040061C1 RID: 25025
	public Action OnSignAddOrRemove;
}
