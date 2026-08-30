using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101A RID: 4122
[Preserve]
public class XUiC_SignInfoWindow : XUiController
{
	// Token: 0x06008277 RID: 33399 RVA: 0x0031B9C4 File Offset: 0x00319BC4
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
		this.confirmationPrompt = base.GetChildById("confirmation_prompt_controller") as XUiC_ConfirmationPrompt;
		this.galleryWindow = this.windowGroup.Controller.GetChildByType<XUiC_SignGalleryWindow>();
	}

	// Token: 0x06008278 RID: 33400 RVA: 0x0031BB1C File Offset: 0x00319D1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExport_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.signId.IsValid || this.signData == null)
		{
			return;
		}
		string text = SignTextureExporter.Instance.ExportSignToTexture(this.signData.name, this.signId);
		Log.Out("Sign png saved to: " + text);
	}

	// Token: 0x06008279 RID: 33401 RVA: 0x0031BB6C File Offset: 0x00319D6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnEdit_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.galleryWindow.targetEntity != null)
		{
			XUiC_SignEditorWindow.Open(this.xui.playerUI, this.galleryWindow.targetEntity);
			return;
		}
		XUiC_SignEditorWindow.Open(this.xui.playerUI, this.signData, this.signId);
	}

	// Token: 0x0600827A RID: 33402 RVA: 0x0031BBC0 File Offset: 0x00319DC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void DuplicateSign()
	{
		if (!PrefabEditModeManager.Instance.IsActive())
		{
			Log.Error("Duplicate function can only be used in the Prefab Editor.");
			return;
		}
		string fileNameNoExtension = PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
		SignData signData = SignData.Duplicate(this.signData, SignDataManager.Instance.GetDuplicateName(fileNameNoExtension, this.signData.name) ?? "");
		GlobalSignId globalSignId = SignDataManager.Instance.AddSignToLibrary(fileNameNoExtension, signData);
		PrefabEditModeManager.Instance.NeedsSaving = true;
		this.galleryWindow.RefreshAndSelect(globalSignId);
	}

	// Token: 0x0600827B RID: 33403 RVA: 0x0031BC44 File Offset: 0x00319E44
	[PublicizedFrom(EAccessModifier.Private)]
	public void DeleteSign()
	{
		if (!PrefabEditModeManager.Instance.IsActive())
		{
			Log.Error("Delete function can only be used in the Prefab Editor.");
			return;
		}
		this.confirmationPrompt.ShowPrompt(Localization.Get("lblSignDeleteTitle", false, null), Localization.Get("lblSignDeleteMessage", false, null), Localization.Get("xuiCancel", false, null), Localization.Get("xuiConfirm", false, null), new Action<XUiC_ConfirmationPrompt.Result>(this.OnDeletionPromptResult));
	}

	// Token: 0x0600827C RID: 33404 RVA: 0x0031BCB0 File Offset: 0x00319EB0
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
			PrefabEditModeManager.Instance.NeedsSaving = true;
			this.galleryWindow.RefreshAndSelect(SignData.defaultSignDataID);
		}
	}

	// Token: 0x0600827D RID: 33405 RVA: 0x0031BD0C File Offset: 0x00319F0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x0600827E RID: 33406 RVA: 0x0031BD27 File Offset: 0x00319F27
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInputs();
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x0600827F RID: 33407 RVA: 0x0031BD52 File Offset: 0x00319F52
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInputs()
	{
		if (this.confirmationPrompt.IsVisible || this.signData == null)
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

	// Token: 0x06008280 RID: 33408 RVA: 0x0031BD90 File Offset: 0x00319F90
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "signname")
		{
			if (!this.signId.IsValid)
			{
				value = Localization.Get("lblSignNone", false, null);
			}
			else if (this.signData == null)
			{
				value = string.Format(Localization.Get("lblSignMissing", false, null), this.signId.signGuid.ToString());
			}
			else
			{
				value = this.signData.name;
			}
			return true;
		}
		if (bindingName == "libraryname")
		{
			value = SignDataManager.GetLibraryNiceName(this.signId);
			return true;
		}
		if (!(bindingName == "lastmodified"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = ((this.signId.IsValid && this.signData != null) ? this.signData.lastModified.ToString("g") : "-");
		return true;
	}

	// Token: 0x06008281 RID: 33409 RVA: 0x0031BE78 File Offset: 0x0031A078
	public void SetSignInfo(GlobalSignId signId)
	{
		SignData signData;
		bool flag = SignDataManager.Instance.TryGetSignData(signId, out signData);
		this.signId = signId;
		this.signData = signData;
		this.signMaterial.IsVisible = signId.IsValid && flag;
		bool flag2 = signId.IsValid && flag && signId.libraryId != "[D]";
		this.btnEdit.Enabled = flag2;
		this.btnDelete.Enabled = flag2;
		this.btnDuplicate.Enabled = flag;
		this.btnExport.Enabled = flag;
		base.RefreshBindings();
	}

	// Token: 0x040060B9 RID: 24761
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId = GlobalSignId.InvalidId;

	// Token: 0x040060BA RID: 24762
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData signData;

	// Token: 0x040060BB RID: 24763
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040060BC RID: 24764
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnEdit;

	// Token: 0x040060BD RID: 24765
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDuplicate;

	// Token: 0x040060BE RID: 24766
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDelete;

	// Token: 0x040060BF RID: 24767
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExport;

	// Token: 0x040060C0 RID: 24768
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignGalleryWindow galleryWindow;

	// Token: 0x040060C1 RID: 24769
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ConfirmationPrompt confirmationPrompt;
}
