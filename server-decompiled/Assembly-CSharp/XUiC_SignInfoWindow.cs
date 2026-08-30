using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001033 RID: 4147
[Preserve]
public class XUiC_SignInfoWindow : XUiController
{
	// Token: 0x06008369 RID: 33641 RVA: 0x003201EC File Offset: 0x0031E3EC
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

	// Token: 0x0600836A RID: 33642 RVA: 0x00320344 File Offset: 0x0031E544
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

	// Token: 0x0600836B RID: 33643 RVA: 0x00320394 File Offset: 0x0031E594
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

	// Token: 0x0600836C RID: 33644 RVA: 0x003203E8 File Offset: 0x0031E5E8
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

	// Token: 0x0600836D RID: 33645 RVA: 0x0032046C File Offset: 0x0031E66C
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

	// Token: 0x0600836E RID: 33646 RVA: 0x003204D8 File Offset: 0x0031E6D8
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

	// Token: 0x0600836F RID: 33647 RVA: 0x00320534 File Offset: 0x0031E734
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x06008370 RID: 33648 RVA: 0x0032054F File Offset: 0x0031E74F
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInputs();
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x06008371 RID: 33649 RVA: 0x0032057A File Offset: 0x0031E77A
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

	// Token: 0x06008372 RID: 33650 RVA: 0x003205B8 File Offset: 0x0031E7B8
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

	// Token: 0x06008373 RID: 33651 RVA: 0x003206A0 File Offset: 0x0031E8A0
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

	// Token: 0x04006180 RID: 24960
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId = GlobalSignId.InvalidId;

	// Token: 0x04006181 RID: 24961
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData signData;

	// Token: 0x04006182 RID: 24962
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x04006183 RID: 24963
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnEdit;

	// Token: 0x04006184 RID: 24964
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDuplicate;

	// Token: 0x04006185 RID: 24965
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnDelete;

	// Token: 0x04006186 RID: 24966
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExport;

	// Token: 0x04006187 RID: 24967
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignGalleryWindow galleryWindow;

	// Token: 0x04006188 RID: 24968
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ConfirmationPrompt confirmationPrompt;
}
