using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001043 RID: 4163
[Preserve]
public class XUiC_SignInstanceWindow : XUiController, ISignRenderingDataUpdateListener
{
	// Token: 0x0600839D RID: 33693 RVA: 0x003240B0 File Offset: 0x003222B0
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

	// Token: 0x0600839E RID: 33694 RVA: 0x00324177 File Offset: 0x00322377
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.previewRefreshPending)
		{
			this.previewRefreshPending = false;
			this.RefreshPreviewRenderers();
		}
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x0600839F RID: 33695 RVA: 0x003241B1 File Offset: 0x003223B1
	public override void OnClose()
	{
		base.OnClose();
		this.comboBlendMode.OnValueChanged -= this.ComboBlendMode_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged -= this.ToggleShowOnImposter_OnValueChanged;
		this.CleanupPreview();
	}

	// Token: 0x060083A0 RID: 33696 RVA: 0x003241F0 File Offset: 0x003223F0
	public override void OnOpen()
	{
		base.OnOpen();
		this.comboBlendMode.OnValueChanged -= this.ComboBlendMode_OnValueChanged;
		this.comboBlendMode.OnValueChanged += this.ComboBlendMode_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged -= this.ToggleShowOnImposter_OnValueChanged;
		this.toggleShowOnImposter.OnValueChanged += this.ToggleShowOnImposter_OnValueChanged;
		base.RefreshBindings();
	}

	// Token: 0x060083A1 RID: 33697 RVA: 0x00324268 File Offset: 0x00322468
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComboBlendMode_OnValueChanged(XUiController _sender, SignCanvas.SignBlendMode _oldValue, SignCanvas.SignBlendMode _newValue)
	{
		TEFeatureCanvas tefeatureCanvas = this.signTileEntity;
		if (((tefeatureCanvas != null) ? tefeatureCanvas.Canvas : null) != null)
		{
			this.signTileEntity.Canvas.BlendMode = _newValue;
			this.RefreshPreviewRenderers();
			Action onSettingsChanged = this.OnSettingsChanged;
			if (onSettingsChanged == null)
			{
				return;
			}
			onSettingsChanged();
		}
	}

	// Token: 0x060083A2 RID: 33698 RVA: 0x003242B6 File Offset: 0x003224B6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleShowOnImposter_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		TEFeatureCanvas tefeatureCanvas = this.signTileEntity;
		if (((tefeatureCanvas != null) ? tefeatureCanvas.Canvas : null) != null)
		{
			this.signTileEntity.Canvas.ShowOnImposter = _newValue;
			Action onSettingsChanged = this.OnSettingsChanged;
			if (onSettingsChanged == null)
			{
				return;
			}
			onSettingsChanged();
		}
	}

	// Token: 0x060083A3 RID: 33699 RVA: 0x003242F4 File Offset: 0x003224F4
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

	// Token: 0x060083A4 RID: 33700 RVA: 0x003243D0 File Offset: 0x003225D0
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

	// Token: 0x060083A5 RID: 33701 RVA: 0x003244D4 File Offset: 0x003226D4
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

	// Token: 0x060083A6 RID: 33702 RVA: 0x00324948 File Offset: 0x00322B48
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

	// Token: 0x060083A7 RID: 33703 RVA: 0x003249F1 File Offset: 0x00322BF1
	public void RefreshPreviewRenderers()
	{
		if (this.signTileEntity == null || this.previewSignRenderers.Count == 0)
		{
			return;
		}
		SignDataManager.Instance.RegisterListener(this.signTileEntity.Canvas.DisplaySignId, this, true);
		this.ApplyRenderingDataToPreview();
	}

	// Token: 0x060083A8 RID: 33704 RVA: 0x00324A2B File Offset: 0x00322C2B
	public void HandleRenderingDataUpdate()
	{
		this.ApplyRenderingDataToPreview();
	}

	// Token: 0x060083A9 RID: 33705 RVA: 0x00324A33 File Offset: 0x00322C33
	public void HandleSignDataRemoved()
	{
		this.previewRefreshPending = true;
	}

	// Token: 0x060083AA RID: 33706 RVA: 0x00324A3C File Offset: 0x00322C3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyRenderingDataToPreview()
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

	// Token: 0x060083AB RID: 33707 RVA: 0x00324AA4 File Offset: 0x00322CA4
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

	// Token: 0x060083AC RID: 33708 RVA: 0x00324B28 File Offset: 0x00322D28
	[PublicizedFrom(EAccessModifier.Private)]
	public void CleanupPreview()
	{
		SignDataManager.Instance.DeregisterListener(this);
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

	// Token: 0x060083AD RID: 33709 RVA: 0x00324BBC File Offset: 0x00322DBC
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

	// Token: 0x060083AE RID: 33710 RVA: 0x00324C0B File Offset: 0x00322E0B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePreviewScrolled(XUiController _sender, float _delta)
	{
		this.ZoomCamera(_delta);
	}

	// Token: 0x060083AF RID: 33711 RVA: 0x00324C14 File Offset: 0x00322E14
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

	// Token: 0x060083B0 RID: 33712 RVA: 0x00324CA0 File Offset: 0x00322EA0
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

	// Token: 0x060083B1 RID: 33713 RVA: 0x00324D15 File Offset: 0x00322F15
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleToggleLight(XUiController _sender, int _mouseButton)
	{
		if (this.previewLight != null)
		{
			this.previewLight.enabled = !this.previewLight.enabled;
		}
	}

	// Token: 0x060083B2 RID: 33714 RVA: 0x00324D40 File Offset: 0x00322F40
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleRotationChanged(XUiController _sender)
	{
		if (this.signTileEntity != null)
		{
			this.signTileEntity.Canvas.CanvasRotation = (CanvasRotationMode)(this.cbxRotation.Value / 90L);
			this.cbxRotation.Value = (long)(this.signTileEntity.Canvas.CanvasRotation * (CanvasRotationMode)90);
			this.RefreshPreviewRenderers();
			Action onSettingsChanged = this.OnSettingsChanged;
			if (onSettingsChanged == null)
			{
				return;
			}
			onSettingsChanged();
		}
	}

	// Token: 0x040061C2 RID: 25026
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureCanvas signTileEntity;

	// Token: 0x040061C3 RID: 25027
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SignCanvas.SignBlendMode> comboBlendMode;

	// Token: 0x040061C4 RID: 25028
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxRotation;

	// Token: 0x040061C5 RID: 25029
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleShowOnImposter;

	// Token: 0x040061C6 RID: 25030
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x040061C7 RID: 25031
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera renderCamera;

	// Token: 0x040061C8 RID: 25032
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture previewTexture;

	// Token: 0x040061C9 RID: 25033
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SignRenderer> previewSignRenderers = new List<SignRenderer>();

	// Token: 0x040061CA RID: 25034
	[PublicizedFrom(EAccessModifier.Private)]
	public Light previewLight;

	// Token: 0x040061CB RID: 25035
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance;

	// Token: 0x040061CC RID: 25036
	[PublicizedFrom(EAccessModifier.Private)]
	public float minDistance;

	// Token: 0x040061CD RID: 25037
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentDistance;

	// Token: 0x040061CE RID: 25038
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject decalBackgroundMesh;

	// Token: 0x040061CF RID: 25039
	public Action OnSettingsChanged;

	// Token: 0x040061D0 RID: 25040
	[PublicizedFrom(EAccessModifier.Private)]
	public bool previewRefreshPending;
}
