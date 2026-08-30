using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101C RID: 4124
[Preserve]
public class XUiC_SignLayerDragAndDropIcon : XUiController
{
	// Token: 0x0600829A RID: 33434 RVA: 0x0031CBE8 File Offset: 0x0031ADE8
	public override void Init()
	{
		base.Init();
		this.signMaterial = base.GetChildById("signMaterial").ViewComponent as XUiV_Texture;
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
	}

	// Token: 0x0600829B RID: 33435 RVA: 0x0031CC50 File Offset: 0x0031AE50
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

	// Token: 0x0600829C RID: 33436 RVA: 0x0031CCF9 File Offset: 0x0031AEF9
	public void SetLayer(GlobalSignId _signId, SignData.SignLayer _layer)
	{
		this.signId = _signId;
		this.layer = _layer;
		this.IsDirty = true;
	}

	// Token: 0x0600829D RID: 33437 RVA: 0x0031CD10 File Offset: 0x0031AF10
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		if (this.layer != null)
		{
			SignUIStyle signUIStyle = ((this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorOnly || this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorAndMask) ? SignUIStyle.LayerThumbnail : SignUIStyle.MaskThumbnail);
			SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, this.layer, signUIStyle);
		}
	}

	// Token: 0x0600829E RID: 33438 RVA: 0x0031CD74 File Offset: 0x0031AF74
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

	// Token: 0x040060CF RID: 24783
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040060D0 RID: 24784
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;

	// Token: 0x040060D1 RID: 24785
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer layer;
}
