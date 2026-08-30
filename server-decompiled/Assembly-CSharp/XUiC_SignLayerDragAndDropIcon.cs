using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001035 RID: 4149
[Preserve]
public class XUiC_SignLayerDragAndDropIcon : XUiController
{
	// Token: 0x0600838C RID: 33676 RVA: 0x00321410 File Offset: 0x0031F610
	public override void Init()
	{
		base.Init();
		this.signMaterial = base.GetChildById("signMaterial").ViewComponent as XUiV_Texture;
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
	}

	// Token: 0x0600838D RID: 33677 RVA: 0x00321478 File Offset: 0x0031F678
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

	// Token: 0x0600838E RID: 33678 RVA: 0x00321521 File Offset: 0x0031F721
	public void SetLayer(GlobalSignId _signId, SignData.SignLayer _layer)
	{
		this.signId = _signId;
		this.layer = _layer;
		this.IsDirty = true;
	}

	// Token: 0x0600838F RID: 33679 RVA: 0x00321538 File Offset: 0x0031F738
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		if (this.layer != null)
		{
			SignUIStyle signUIStyle = ((this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorOnly || this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorAndMask) ? SignUIStyle.LayerThumbnail : SignUIStyle.MaskThumbnail);
			SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, this.layer, signUIStyle);
		}
	}

	// Token: 0x06008390 RID: 33680 RVA: 0x0032159C File Offset: 0x0031F79C
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

	// Token: 0x04006196 RID: 24982
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x04006197 RID: 24983
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;

	// Token: 0x04006198 RID: 24984
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer layer;
}
