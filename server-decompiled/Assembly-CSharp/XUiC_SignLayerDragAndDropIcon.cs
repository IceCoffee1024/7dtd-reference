using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001054 RID: 4180
[Preserve]
public class XUiC_SignLayerDragAndDropIcon : XUiController
{
	// Token: 0x06008407 RID: 33799 RVA: 0x00326AFC File Offset: 0x00324CFC
	public override void Init()
	{
		base.Init();
		this.signMaterial = base.GetChildById("signMaterial").ViewComponent as XUiV_Texture;
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
	}

	// Token: 0x06008408 RID: 33800 RVA: 0x00326B64 File Offset: 0x00324D64
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

	// Token: 0x06008409 RID: 33801 RVA: 0x00326C0D File Offset: 0x00324E0D
	public void SetLayer(GlobalSignId _signId, SignData.SignLayer _layer)
	{
		this.signId = _signId;
		this.layer = _layer;
		this.IsDirty = true;
	}

	// Token: 0x0600840A RID: 33802 RVA: 0x00326C24 File Offset: 0x00324E24
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		if (this.layer != null)
		{
			SignUIStyle signUIStyle = ((this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorOnly || this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorAndMask) ? SignUIStyle.LayerThumbnail : SignUIStyle.MaskThumbnail);
			SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, this.layer, signUIStyle);
		}
	}

	// Token: 0x0600840B RID: 33803 RVA: 0x00326C88 File Offset: 0x00324E88
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

	// Token: 0x0400620F RID: 25103
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x04006210 RID: 25104
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;

	// Token: 0x04006211 RID: 25105
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer layer;
}
