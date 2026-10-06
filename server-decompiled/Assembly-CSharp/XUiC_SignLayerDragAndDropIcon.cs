using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001044 RID: 4164
[Preserve]
public class XUiC_SignLayerDragAndDropIcon : XUiController
{
	// Token: 0x060083B5 RID: 33717 RVA: 0x00324DF8 File Offset: 0x00322FF8
	public override void Init()
	{
		base.Init();
		this.signMaterial = base.GetChildById("signMaterial").ViewComponent as XUiV_Texture;
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
	}

	// Token: 0x060083B6 RID: 33718 RVA: 0x00324E60 File Offset: 0x00323060
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		if (this.viewComponent.IsVisible)
		{
			Vector2 screenPosition = this.xui.playerUI.CursorController.GetScreenPosition();
			Vector3 vector = this.xui.playerUI.camera.ScreenToWorldPoint(screenPosition);
			Transform transform = this.xui.transform;
			vector.z = transform.position.z - 3f * transform.lossyScale.z;
			base.ViewComponent.UiTransform.position = vector;
		}
	}

	// Token: 0x060083B7 RID: 33719 RVA: 0x00324F09 File Offset: 0x00323109
	public void SetLayer(GlobalSignId _signId, SignData.SignLayer _layer)
	{
		this.signId = _signId;
		this.layer = _layer;
		this.IsDirty = true;
	}

	// Token: 0x060083B8 RID: 33720 RVA: 0x00324F20 File Offset: 0x00323120
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		if (this.layer != null)
		{
			SignUIStyle signUIStyle = ((this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorOnly || this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorAndMask) ? SignUIStyle.LayerThumbnail : SignUIStyle.MaskThumbnail);
			SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, this.layer, signUIStyle);
		}
	}

	// Token: 0x060083B9 RID: 33721 RVA: 0x00324F84 File Offset: 0x00323184
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (base.GetBindingValueInternal(ref _value, _bindingName))
		{
			return true;
		}
		if (_bindingName == "showdraganddrop")
		{
			_value = (this.layer != null).ToString();
			return true;
		}
		return false;
	}

	// Token: 0x040061D1 RID: 25041
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040061D2 RID: 25042
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;

	// Token: 0x040061D3 RID: 25043
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer layer;
}
