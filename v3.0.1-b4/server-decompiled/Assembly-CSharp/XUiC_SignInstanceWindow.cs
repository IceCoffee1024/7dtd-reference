using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101B RID: 4123
[Preserve]
public class XUiC_SignInstanceWindow : XUiController
{
	// Token: 0x06008285 RID: 33413 RVA: 0x0031BF30 File Offset: 0x0031A130
	public override void Init()
	{
		base.Init();
		this.comboBlendMode = base.GetChildByType<XUiC_ComboBoxEnum<SignCanvas.SignBlendMode>>();
		this.toggleShowOnImposter = base.GetChildById("toggleShowOnImposter") as XUiC_ToggleButton;
		this.cbxRotation = base.GetChildById("cbxRotation").GetChildByType<XUiC_ComboBoxInt>();
		this.cbxRotation.OnValueChangedGeneric += this.HandleRotationChanged;
		this.previewTexture = (XUiV_Texture)base.GetChildById("previewMaterial").ViewComponent;
		XUiController childById = base.GetChildById("previewArea");
		childById.OnDrag += this.HandlePreviewDragged;
		childById.OnScroll += this.HandlePreviewScrolled;
		base.GetChildById("btnToggleLight").OnPress += this.HandleToggleLight;
	}

	// Token: 0x06008286 RID: 33414 RVA: 0x002F7098 File Offset: 0x002F5298
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x06008287 RID: 33415 RVA: 0x0031BFF7 File Offset: 0x0031A1F7
	public override void OnClose()
	{
		base.OnClose();
		this.comboBlendMode.OnValueChanged -= this.ComboBlendMode_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged -= this.ToggleShowOnImposter_OnValueChanged;
		this.CleanupPreview();
	}

	// Token: 0x06008288 RID: 33416 RVA: 0x0031C034 File Offset: 0x0031A234
	public override void OnOpen()
	{
		base.OnOpen();
		this.comboBlendMode.OnValueChanged -= this.ComboBlendMode_OnValueChanged;
		this.comboBlendMode.OnValueChanged += this.ComboBlendMode_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged -= this.ToggleShowOnImposter_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged += this.ToggleShowOnImposter_OnValueChanged;
		base.RefreshBindings();
	}

	// Token: 0x06008289 RID: 33417 RVA: 0x0031C0A9 File Offset: 0x0031A2A9
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboBlendMode_OnValueChanged(XUiController _sender, SignCanvas.SignBlendMode _oldValue, SignCanvas.SignBlendMode _newValue)
	{
		TEFeatureCanvas tefeatureCanvas = this.signTileEntity;
		if (((tefeatureCanvas != null) ? tefeatureCanvas.Canvas : null) != null)
		{
			this.signTileEntity.Canvas.BlendMode = _newValue;
			this.RefreshPreviewRenderers();
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x0600828A RID: 33418 RVA: 0x0031C0E8 File Offset: 0x0031A2E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleShowOnImposter_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		TEFeatureCanvas tefeatureCanvas = this.signTileEntity;
		if (((tefeatureCanvas != null) ? tefeatureCanvas.Canvas : null) != null)
		{
			this.signTileEntity.Canvas.ShowOnImposter = _newValue;
			this.signTileEntity.SetModified();
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x0600828B RID: 33419 RVA: 0x0031C138 File Offset: 0x0031A338
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "blockname")
		{
			value = ((this.signTileEntity != null) ? this.signTileEntity.Parent.block.GetBlockName() : "");
			return true;
		}
		if (bindingName == "showblendmode")
		{
			TEFeatureCanvas tefeatureCanvas = this.signTileEntity;
			value = (((tefeatureCanvas != null) ? tefeatureCanvas.Canvas : null) != null && this.signTileEntity.Canvas.IsDecal).ToString();
			return true;
		}
		if (!(bindingName == "allowedonimposter"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		TEFeatureCanvas tefeatureCanvas2 = this.signTileEntity;
		value = (((tefeatureCanvas2 != null) ? tefeatureCanvas2.Canvas : null) != null && this.signTileEntity.Canvas.AllowedOnImposter).ToString();
		return true;
	}

	// Token: 0x0600828C RID: 33420 RVA: 0x0031C214 File Offset: 0x0031A414
	public void InitialiseTo(TEFeatureCanvas sign)
	{
		this.signTileEntity = sign;
		this.comboBlendMode.OnValueChanged -= this.ComboBlendMode_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged -= this.ToggleShowOnImposter_OnValueChanged;
		TEFeatureCanvas tefeatureCanvas = this.signTileEntity;
		if (((tefeatureCanvas != null) ? tefeatureCanvas.Canvas : null) != null)
		{
			this.comboBlendMode.Value = sign.Canvas.BlendMode;
			this.cbxRotation.Value = (long)(sign.Canvas.CanvasRotation * (CanvasRotationMode)90);
			this.toggleShowOnImposter.Value = sign.Canvas.ShowOnImposter;
		}
		else
		{
			this.comboBlendMode.Value = SignCanvas.SignBlendMode.Cutout;
			this.cbxRotation.Value = 0L;
			this.toggleShowOnImposter.Value = false;
		}
		this.comboBlendMode.OnValueChanged += this.ComboBlendMode_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged += this.ToggleShowOnImposter_OnValueChanged;
		base.RefreshBindings();
		this.SetupPreview(sign);
	}

	// Token: 0x0600828D RID: 33421 RVA: 0x0031C318 File Offset: 0x0031A518
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupPreview(TEFeatureCanvas sign)
	{
		this.CleanupPreview();
		if (sign == null)
		{
			return;
		}
		BlockShapeModelEntity blockShapeModelEntity = sign.Parent.block.shape as BlockShapeModelEntity;
		if (blockShapeModelEntity == null)
		{
			return;
		}
		GameObject gameObject = new GameObject("SignPreviewPivot");
		this.renderTextureSystem = new RenderTextureSystem();
		this.renderTextureSystem.Create("signPreview", gameObject, Vector3.zero, new Vector3(1f, 1.5f, -1f), new Vector2i(this.previewTexture.Size.x, this.previewTexture.Size.y), true, false, 1f);
		Transform transform = blockShapeModelEntity.CloneModel(sign.Parent.blockValue, gameObject.transform);
		Utils.SetLayerRecursively(gameObject, 11);
		Quaternion rotationStatic = BlockShapeNew.GetRotationStatic((int)sign.Parent.blockValue.rotation);
		Vector3 vector = rotationStatic * Vector3.forward;
		if (Mathf.Abs(vector.y) < 0.5f)
		{
			vector.y = 0f;
			float num = Vector3.SignedAngle(vector, Vector3.back, Vector3.up);
			gameObject.transform.rotation = Quaternion.AngleAxis(num, Vector3.up);
		}
		else
		{
			gameObject.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up) * Quaternion.Inverse(rotationStatic);
		}
		transform.GetComponentsInChildren<SignRenderer>(true, this.previewSignRenderers);
		this.renderCamera = this.renderTextureSystem.CameraGO.GetComponent<Camera>();
		this.renderCamera.fieldOfView = 40f;
		this.renderCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		this.renderCamera.renderingPath = RenderingPath.DeferredShading;
		this.renderCamera.farClipPlane = 32f;
		Bounds bounds = XUiC_SignInstanceWindow.ComputeBounds(gameObject);
		Vector3 center = bounds.center;
		float num2 = this.renderCamera.fieldOfView * 0.5f * 0.017453292f;
		float num3 = (float)this.previewTexture.Size.x / (float)this.previewTexture.Size.y;
		float num4 = Mathf.Max(bounds.extents.x, Mathf.Max(bounds.extents.y, bounds.extents.z));
		this.maxDistance = Mathf.Max(num4 / Mathf.Tan(num2) * 1.2f, 1.5f);
		this.minDistance = this.maxDistance * 0.1f;
		float num5 = bounds.extents.y / Mathf.Tan(num2);
		float num6 = bounds.extents.x / (Mathf.Tan(num2) * num3);
		this.currentDistance = Mathf.Clamp(Mathf.Max(num5, num6) * 1.15f, this.minDistance, this.maxDistance);
		gameObject.transform.position -= center;
		this.renderCamera.transform.position = new Vector3(0.1f, 0.15f, -1f).normalized * this.currentDistance;
		this.renderCamera.transform.LookAt(Vector3.zero);
		if (sign.Canvas.IsDecal)
		{
			if (this.decalBackgroundMesh == null)
			{
				this.InitializeDecalBackgroundMesh();
			}
			this.decalBackgroundMesh.SetActive(true);
			this.decalBackgroundMesh.transform.SetParent(transform.transform);
			this.decalBackgroundMesh.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			this.decalBackgroundMesh.transform.localPosition = new Vector3(0f, 2f, -0.5f);
		}
		else
		{
			GameObject gameObject2 = this.decalBackgroundMesh;
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
		}
		Light component = this.renderTextureSystem.LightGO.GetComponent<Light>();
		component.type = LightType.Directional;
		component.intensity = 1f;
		component.color = Color.white;
		this.renderTextureSystem.LightGO.transform.position = this.renderCamera.transform.position;
		this.renderTextureSystem.LightGO.transform.rotation = this.renderCamera.transform.rotation;
		this.previewLight = component;
		this.previewTexture.Texture = this.renderTextureSystem.RenderTex;
		this.RefreshPreviewRenderers();
	}

	// Token: 0x0600828E RID: 33422 RVA: 0x0031C78C File Offset: 0x0031A98C
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitializeDecalBackgroundMesh()
	{
		this.decalBackgroundMesh = GameObject.CreatePrimitive(PrimitiveType.Quad);
		this.decalBackgroundMesh.name = "SignInstancePreviewBackgroundMesh";
		this.decalBackgroundMesh.layer = 11;
		this.decalBackgroundMesh.transform.localScale = Vector3.one * 32f;
		MeshRenderer component = this.decalBackgroundMesh.GetComponent<MeshRenderer>();
		component.material = Resources.Load<Material>("Materials/SignPreviewBackground");
		component.material.SetVector("_MainTex_ST", new Vector4(4f, 4f, 0f, 0f));
		this.decalBackgroundMesh.GetComponent<Collider>().enabled = false;
	}

	// Token: 0x0600828F RID: 33423 RVA: 0x0031C838 File Offset: 0x0031AA38
	public void RefreshPreviewRenderers()
	{
		if (this.signTileEntity == null || this.previewSignRenderers.Count == 0)
		{
			return;
		}
		SignDataManager.Instance.TryApplyRenderingData(this.signTileEntity.Canvas.DisplaySignId, delegate(MaterialPropertyBlock mpb)
		{
			mpb.SetInteger(SignShaderIDs._UseTexture, 0);
			mpb.SetTexture(SignShaderIDs._BakedTexture, Texture2D.whiteTexture);
			mpb.SetFloat(SignShaderIDs._CanvasRotation, this.signTileEntity.Canvas.CanvasRotationRadians);
		}, this.previewSignRenderers, this.signTileEntity.Canvas.BlendMode, this.renderCamera);
	}

	// Token: 0x06008290 RID: 33424 RVA: 0x0031C8A0 File Offset: 0x0031AAA0
	[PublicizedFrom(EAccessModifier.Private)]
	public static Bounds ComputeBounds(GameObject go)
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		if (componentsInChildren.Length == 0)
		{
			return new Bounds(go.transform.position, Vector3.one);
		}
		TemporaryObject componentInChildren = go.GetComponentInChildren<TemporaryObject>();
		Transform transform = ((componentInChildren != null) ? componentInChildren.transform : null);
		Bounds bounds = componentsInChildren[0].bounds;
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			if (!(transform != null) || !componentsInChildren[i].transform.IsChildOf(transform))
			{
				bounds.Encapsulate(componentsInChildren[i].bounds);
			}
		}
		return bounds;
	}

	// Token: 0x06008291 RID: 33425 RVA: 0x0031C924 File Offset: 0x0031AB24
	[PublicizedFrom(EAccessModifier.Private)]
	public void CleanupPreview()
	{
		if (this.decalBackgroundMesh != null)
		{
			this.decalBackgroundMesh.transform.SetParent(base.ViewComponent.UiTransform);
			this.decalBackgroundMesh.SetActive(false);
		}
		this.previewSignRenderers.Clear();
		if (this.renderTextureSystem != null)
		{
			this.renderTextureSystem.Cleanup();
			this.renderTextureSystem = null;
		}
		this.renderCamera = null;
		this.previewLight = null;
		if (this.previewTexture != null)
		{
			this.previewTexture.Texture = null;
		}
	}

	// Token: 0x06008292 RID: 33426 RVA: 0x0031C9B0 File Offset: 0x0031ABB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePreviewDragged(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		if (this.renderCamera == null)
		{
			return;
		}
		KeyCode currentKey = UICamera.currentKey;
		if (currentKey == KeyCode.Mouse0)
		{
			this.OrbitCamera(_mousePositionDelta);
			return;
		}
		if (currentKey - KeyCode.Mouse1 > 1)
		{
			return;
		}
		this.ZoomCamera(_mousePositionDelta.y * 0.005f);
	}

	// Token: 0x06008293 RID: 33427 RVA: 0x0031C9FF File Offset: 0x0031ABFF
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePreviewScrolled(XUiController _sender, float _delta)
	{
		this.ZoomCamera(_delta);
	}

	// Token: 0x06008294 RID: 33428 RVA: 0x0031CA08 File Offset: 0x0031AC08
	[PublicizedFrom(EAccessModifier.Private)]
	public void OrbitCamera(Vector2 _mousePositionDelta)
	{
		if (Mathf.Abs(_mousePositionDelta.x) > 0.01f)
		{
			this.renderCamera.transform.RotateAround(Vector3.zero, Vector3.up, _mousePositionDelta.x * 0.5f);
		}
		if (Mathf.Abs(_mousePositionDelta.y) > 0.01f)
		{
			this.renderCamera.transform.RotateAround(Vector3.zero, this.renderCamera.transform.right, -_mousePositionDelta.y * 0.5f);
		}
	}

	// Token: 0x06008295 RID: 33429 RVA: 0x0031CA94 File Offset: 0x0031AC94
	[PublicizedFrom(EAccessModifier.Private)]
	public void ZoomCamera(float _zoomDelta)
	{
		if (this.renderCamera == null)
		{
			return;
		}
		this.currentDistance = Mathf.Clamp(this.currentDistance - _zoomDelta * this.maxDistance, this.minDistance, this.maxDistance);
		this.renderCamera.transform.position = this.renderCamera.transform.position.normalized * this.currentDistance;
	}

	// Token: 0x06008296 RID: 33430 RVA: 0x0031CB09 File Offset: 0x0031AD09
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleToggleLight(XUiController _sender, int _mouseButton)
	{
		if (this.previewLight != null)
		{
			this.previewLight.enabled = !this.previewLight.enabled;
		}
	}

	// Token: 0x06008297 RID: 33431 RVA: 0x0031CB34 File Offset: 0x0031AD34
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleRotationChanged(XUiController _sender)
	{
		if (this.signTileEntity != null)
		{
			this.signTileEntity.Canvas.CanvasRotation = (CanvasRotationMode)(this.cbxRotation.Value / 90L);
			this.cbxRotation.Value = (long)(this.signTileEntity.Canvas.CanvasRotation * (CanvasRotationMode)90);
			this.RefreshPreviewRenderers();
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x040060C2 RID: 24770
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCanvas signTileEntity;

	// Token: 0x040060C3 RID: 24771
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SignCanvas.SignBlendMode> comboBlendMode;

	// Token: 0x040060C4 RID: 24772
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxRotation;

	// Token: 0x040060C5 RID: 24773
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleShowOnImposter;

	// Token: 0x040060C6 RID: 24774
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x040060C7 RID: 24775
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera renderCamera;

	// Token: 0x040060C8 RID: 24776
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture previewTexture;

	// Token: 0x040060C9 RID: 24777
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SignRenderer> previewSignRenderers = new List<SignRenderer>();

	// Token: 0x040060CA RID: 24778
	[PublicizedFrom(EAccessModifier.Private)]
	public Light previewLight;

	// Token: 0x040060CB RID: 24779
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance;

	// Token: 0x040060CC RID: 24780
	[PublicizedFrom(EAccessModifier.Private)]
	public float minDistance;

	// Token: 0x040060CD RID: 24781
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentDistance;

	// Token: 0x040060CE RID: 24782
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject decalBackgroundMesh;
}
