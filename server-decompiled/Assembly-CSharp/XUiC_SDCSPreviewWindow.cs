using System;
using HorizonBasedAmbientOcclusion;
using PI.NGSS;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E62 RID: 3682
[Preserve]
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiC_SDCSPreviewWindow : XUiController
{
	// Token: 0x17000CD5 RID: 3285
	// (get) Token: 0x06007098 RID: 28824 RVA: 0x002B8A99 File Offset: 0x002B6C99
	// (set) Token: 0x06007099 RID: 28825 RVA: 0x002B8AA1 File Offset: 0x002B6CA1
	[XuiXmlAttribute("can_zoom", false)]
	public bool CanZoom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x0600709A RID: 28826 RVA: 0x002B8AAA File Offset: 0x002B6CAA
	// (set) Token: 0x0600709B RID: 28827 RVA: 0x002B8AB2 File Offset: 0x002B6CB2
	[XuiXmlAttribute("gamepad_rotation_mode", false)]
	public XUiC_SDCSPreviewWindow.eGamepadRotationMode GamepadRotationMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600709C RID: 28828 RVA: 0x002B8ABC File Offset: 0x002B6CBC
	public override void Init()
	{
		base.Init();
		this.textPreview = (XUiV_Texture)base.GetChildById("playerPreview").ViewComponent;
		this.renderTextureSystem = new RenderTextureSystem();
		base.OnScroll += this.OnScrolled;
		base.OnDrag += this.OnDragged;
	}

	// Token: 0x0600709D RID: 28829 RVA: 0x002B8B19 File Offset: 0x002B6D19
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnScroll -= this.OnScrolled;
		base.OnDrag -= this.OnDragged;
	}

	// Token: 0x0600709E RID: 28830 RVA: 0x002B8B45 File Offset: 0x002B6D45
	[XuiBindEvent("OnPress", "zoomButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ZoomButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.toggleHeadZoom();
	}

	// Token: 0x0600709F RID: 28831 RVA: 0x002B8B4D File Offset: 0x002B6D4D
	[XuiBindEvent("OnVisiblity", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void visibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.startViz();
			return;
		}
		this.endViz();
	}

	// Token: 0x060070A0 RID: 28832 RVA: 0x002B8B5F File Offset: 0x002B6D5F
	[PublicizedFrom(EAccessModifier.Private)]
	public void startViz()
	{
		this.originalPixelLightCount = QualitySettings.pixelLightCount;
		QualitySettings.pixelLightCount = 4;
	}

	// Token: 0x060070A1 RID: 28833 RVA: 0x002B8B72 File Offset: 0x002B6D72
	[PublicizedFrom(EAccessModifier.Private)]
	public void endViz()
	{
		SDCSUtils.UnloadViz(this.previewTransform);
		this.renderTextureSystem.Cleanup();
		this.lastProfile = "";
		this.lastFieldOfView = 54f;
		QualitySettings.pixelLightCount = this.originalPixelLightCount;
	}

	// Token: 0x060070A2 RID: 28834 RVA: 0x002B8BAC File Offset: 0x002B6DAC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		string text = ProfileSDF.CurrentProfileName();
		if (this.viewComponent.IsActiveInHierarchy && text != this.lastProfile)
		{
			this.Archetype = Archetype.GetArchetype(text);
			if (this.Archetype == null)
			{
				this.Archetype = ProfileSDF.CreateTempArchetype(text);
			}
			this.MakePreview();
			this.lastProfile = text;
			if (this.CanZoom)
			{
				this.state = XUiC_SDCSPreviewWindow.ZoomStates.Head;
				this.setToHeadZoom();
			}
		}
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			this.updateController();
		}
	}

	// Token: 0x060070A3 RID: 28835 RVA: 0x002B8C3C File Offset: 0x002B6E3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateController()
	{
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			float num = 0f;
			switch (this.GamepadRotationMode)
			{
			case XUiC_SDCSPreviewWindow.eGamepadRotationMode.Bumper:
				num = this.xui.playerUI.playerInput.GUIActions.BumperAxis.Value;
				break;
			case XUiC_SDCSPreviewWindow.eGamepadRotationMode.Trigger:
				num = this.xui.playerUI.playerInput.GUIActions.TriggerAxis.Value;
				break;
			case XUiC_SDCSPreviewWindow.eGamepadRotationMode.RightStick:
				num = -this.xui.playerUI.playerInput.GUIActions.Camera.Value.x;
				break;
			}
			if (num != 0f)
			{
				this.cameraRotate(-num);
			}
			if (this.xui.playerUI.playerInput.GUIActions.HalfStack.WasPressed)
			{
				this.toggleHeadZoom();
			}
		}
	}

	// Token: 0x060070A4 RID: 28836 RVA: 0x002B8D1C File Offset: 0x002B6F1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDragged(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		float x = _mousePositionDelta.x;
		if (this.xui.playerUI.CursorController.GetMouseButton(UICamera.MouseButton.RightButton))
		{
			this.cameraRotate(x);
		}
	}

	// Token: 0x060070A5 RID: 28837 RVA: 0x002B8D50 File Offset: 0x002B6F50
	[PublicizedFrom(EAccessModifier.Private)]
	public void cameraVerticalPan(float _value)
	{
		this.renderCamera.transform.localPosition -= new Vector3(0f, _value, 0f);
		if (this.renderCamera.transform.localPosition.y < -1.5f)
		{
			this.renderCamera.transform.localPosition = new Vector3(this.renderCamera.transform.localPosition.x, -1.5f, this.renderCamera.transform.localPosition.z);
			return;
		}
		if (this.renderCamera.transform.localPosition.y > 0f)
		{
			this.renderCamera.transform.localPosition = new Vector3(this.renderCamera.transform.localPosition.x, 0f, this.renderCamera.transform.localPosition.z);
		}
	}

	// Token: 0x060070A6 RID: 28838 RVA: 0x002B8E4C File Offset: 0x002B704C
	[PublicizedFrom(EAccessModifier.Private)]
	public void cameraRotate(float _value)
	{
		this.renderCamera.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, _value);
		this.renderTextureSystem.LightGO.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, _value);
	}

	// Token: 0x060070A7 RID: 28839 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrolled(XUiController _sender, float _delta)
	{
	}

	// Token: 0x060070A8 RID: 28840 RVA: 0x002B8EAC File Offset: 0x002B70AC
	public void MakePreview()
	{
		SDCSUtils.CreateVizUI(this.Archetype, ref this.previewTransform, ref this.uiBoneCatalog);
		this.previewTransform.GetComponentInChildren<Animator>().Update(0f);
		this.init();
		this.previewTransform.transform.parent = this.renderTextureSystem.TargetGO.transform;
		this.previewTransform.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		this.previewTransform.transform.localPosition = new Vector3(0f, -0.9f, 0f);
		this.characterGazeController = this.previewTransform.GetComponentInChildren<CharacterGazeController>();
		Utils.SetLayerRecursively(this.renderTextureSystem.TargetGO, 11);
		this.textPreview.Texture = this.renderTexture;
	}

	// Token: 0x060070A9 RID: 28841 RVA: 0x002B8F88 File Offset: 0x002B7188
	[PublicizedFrom(EAccessModifier.Private)]
	public void toggleHeadZoom()
	{
		XUiC_SDCSPreviewWindow.ZoomStates zoomStates;
		switch (this.state)
		{
		case XUiC_SDCSPreviewWindow.ZoomStates.Eyes:
			zoomStates = XUiC_SDCSPreviewWindow.ZoomStates.Head;
			break;
		case XUiC_SDCSPreviewWindow.ZoomStates.Head:
			zoomStates = XUiC_SDCSPreviewWindow.ZoomStates.Chest;
			break;
		case XUiC_SDCSPreviewWindow.ZoomStates.Chest:
			zoomStates = XUiC_SDCSPreviewWindow.ZoomStates.FullBody;
			break;
		case XUiC_SDCSPreviewWindow.ZoomStates.FullBody:
			zoomStates = XUiC_SDCSPreviewWindow.ZoomStates.Eyes;
			break;
		default:
			zoomStates = this.state;
			break;
		}
		this.state = zoomStates;
		switch (this.state)
		{
		case XUiC_SDCSPreviewWindow.ZoomStates.Eyes:
			this.setToEyeZoom();
			return;
		case XUiC_SDCSPreviewWindow.ZoomStates.Head:
			this.setToHeadZoom();
			return;
		case XUiC_SDCSPreviewWindow.ZoomStates.Chest:
			this.setToChestZoom();
			return;
		case XUiC_SDCSPreviewWindow.ZoomStates.FullBody:
			this.setToFullBodyZoom();
			return;
		default:
			return;
		}
	}

	// Token: 0x060070AA RID: 28842 RVA: 0x002B900C File Offset: 0x002B720C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToFullBodyZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(19f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 54f;
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x060070AB RID: 28843 RVA: 0x002B9090 File Offset: 0x002B7290
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToHeadZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(1.5f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 12f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.015f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x060070AC RID: 28844 RVA: 0x002B913C File Offset: 0x002B733C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToChestZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(5f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 20f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.02f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x060070AD RID: 28845 RVA: 0x002B91E8 File Offset: 0x002B73E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToEyeZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 6f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.015f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x060070AE RID: 28846 RVA: 0x002B9274 File Offset: 0x002B7474
	[PublicizedFrom(EAccessModifier.Private)]
	public void setCameraInitialRotationOffset()
	{
		this.renderCamera.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, -30f);
		this.renderTextureSystem.LightGO.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, -30f);
		if (this.characterGazeController != null)
		{
			this.characterGazeController.SnapNextUpdate();
		}
	}

	// Token: 0x060070AF RID: 28847 RVA: 0x002B92F4 File Offset: 0x002B74F4
	public void ZoomToHead()
	{
		if (this.state == XUiC_SDCSPreviewWindow.ZoomStates.Head)
		{
			return;
		}
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && this.xui.playerUI.playerInput.GUIActions.TriggerAxis.Value != 0f)
		{
			return;
		}
		this.state = XUiC_SDCSPreviewWindow.ZoomStates.Head;
		this.setToHeadZoom();
	}

	// Token: 0x060070B0 RID: 28848 RVA: 0x002B9354 File Offset: 0x002B7554
	public void ZoomToEye()
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && this.xui.playerUI.playerInput.GUIActions.TriggerAxis.Value != 0f)
		{
			return;
		}
		this.state = XUiC_SDCSPreviewWindow.ZoomStates.Eyes;
		this.setToEyeZoom();
	}

	// Token: 0x060070B1 RID: 28849 RVA: 0x002B93A8 File Offset: 0x002B75A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void init()
	{
		if (this.renderTextureSystem.ParentGO != null)
		{
			return;
		}
		this.renderTextureSystem.Create("characterpreview", new GameObject(), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), this.textPreview.Size, true, true, 1f);
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0f, -0.75f, 2.15f);
		this.renderTexture = this.renderTextureSystem.RenderTex;
		this.renderCamera = this.renderTextureSystem.CameraGO.GetComponent<Camera>();
		this.renderCamera.orthographic = false;
		this.renderCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		this.renderCamera.fieldOfView = 54f;
		this.renderCamera.renderingPath = RenderingPath.DeferredShading;
		this.renderCamera.tag = "MainCamera";
		this.renderCamera.gameObject.AddComponent<StreamingController>();
		this.renderTextureSystem.CameraGO.AddComponent<NGSS_Local>().NGSS_PCSS_SOFTNESS_NEAR = 0.05f;
		HBAO hbao = this.renderTextureSystem.CameraGO.AddComponent<HBAO>();
		hbao.SetAoPerPixelNormals(HBAO.PerPixelNormals.Reconstruct);
		hbao.SetAoIntensity(0.5f);
		this.renderTextureSystem.LightGO.GetComponent<Light>().enabled = false;
		GameObject gameObject = new GameObject("Key Light", new Type[] { typeof(Light) });
		gameObject.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject.transform.SetPositionAndRotation(new Vector3(0.25f, 0.475f, 0.62f), Quaternion.Euler(33f, -8f, 0f));
		gameObject.AddComponent<NGSS_Directional>().NGSS_PCSS_ENABLED = true;
		Light component = gameObject.GetComponent<Light>();
		component.color = new Color(0.9f, 0.8f, 0.7f, 1f);
		component.type = LightType.Spot;
		component.range = 20f;
		component.spotAngle = 60f;
		component.intensity = 1.5f;
		component.shadows = LightShadows.Hard;
		component.shadowStrength = 0.2f;
		component.shadowBias = 0.005f;
		NGSS_FrustumShadows ngss_FrustumShadows = this.renderTextureSystem.CameraGO.AddComponent<NGSS_FrustumShadows>();
		ngss_FrustumShadows.mainShadowsLight = component;
		ngss_FrustumShadows.m_fastBlur = false;
		ngss_FrustumShadows.m_shadowsBlur = 1f;
		ngss_FrustumShadows.m_shadowsBlurIterations = 4;
		ngss_FrustumShadows.m_rayThickness = 0.025f;
		GameObject gameObject2 = new GameObject("Fill Light", new Type[] { typeof(Light) });
		gameObject2.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject2.transform.SetPositionAndRotation(new Vector3(-1.15f, 1.4f, 1f), Quaternion.Euler(50f, 45f, 0f));
		Light component2 = gameObject2.GetComponent<Light>();
		component2.color = new Color(1f, 1f, 1f, 1f);
		component2.type = LightType.Spot;
		component2.range = 20f;
		component2.spotAngle = 60f;
		component2.intensity = 0.5f;
		component2.shadows = LightShadows.Hard;
		component2.shadowStrength = 0.2f;
		component2.shadowBias = 0.005f;
		GameObject gameObject3 = new GameObject("Fill 2 Light", new Type[] { typeof(Light) });
		gameObject3.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject3.transform.SetPositionAndRotation(new Vector3(0f, -1.5f, -0.5f), Quaternion.Euler(-15f, 0f, 0f));
		Light component3 = gameObject3.GetComponent<Light>();
		component3.color = new Color(1f, 1f, 1f, 1f);
		component3.type = LightType.Spot;
		component3.range = 20f;
		component3.spotAngle = 60f;
		component3.intensity = 0.5f;
		component3.shadows = LightShadows.Hard;
		component3.shadowStrength = 0.2f;
		component3.shadowBias = 0.005f;
		GameObject gameObject4 = new GameObject("Back Light", new Type[] { typeof(Light) });
		gameObject4.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject4.transform.SetPositionAndRotation(new Vector3(-0.6f, 0.75f, 2.6f), Quaternion.Euler(55f, 133f, 0f));
		Light component4 = gameObject4.GetComponent<Light>();
		component4.color = new Color(0.4f, 0.75f, 1f, 1f);
		component4.type = LightType.Spot;
		component4.spotAngle = 60f;
		component4.range = 20f;
		component4.intensity = 1.5f;
		component4.shadows = LightShadows.Hard;
		component4.shadowStrength = 0.2f;
		component4.shadowBias = 0.005f;
		this.originalCamPosition = this.renderCamera.transform.localPosition;
		this.originalCamRotation = this.renderCamera.transform.localRotation;
		this.originalLightPosition = this.renderTextureSystem.LightGO.transform.localPosition;
		this.originalLightRotation = this.renderTextureSystem.LightGO.transform.localRotation;
		this.renderCamera.transform.localPosition = new Vector3(0f, -0.75f, 0f);
	}

	// Token: 0x04005548 RID: 21832
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x04005549 RID: 21833
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textPreview;

	// Token: 0x0400554A RID: 21834
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture renderTexture;

	// Token: 0x0400554B RID: 21835
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera renderCamera;

	// Token: 0x0400554C RID: 21836
	public Transform TargetTransform;

	// Token: 0x0400554D RID: 21837
	public GameObject RotateTable;

	// Token: 0x0400554E RID: 21838
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewTransform;

	// Token: 0x0400554F RID: 21839
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastProfile;

	// Token: 0x04005550 RID: 21840
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastFieldOfView;

	// Token: 0x04005551 RID: 21841
	public Archetype Archetype;

	// Token: 0x04005552 RID: 21842
	[PublicizedFrom(EAccessModifier.Private)]
	public CharacterGazeController characterGazeController;

	// Token: 0x04005553 RID: 21843
	[XuiBindComponent("zoomButton", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button zoomButton;

	// Token: 0x04005554 RID: 21844
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 originalCamPosition;

	// Token: 0x04005555 RID: 21845
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 originalLightPosition;

	// Token: 0x04005556 RID: 21846
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion originalCamRotation;

	// Token: 0x04005557 RID: 21847
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion originalLightRotation;

	// Token: 0x04005559 RID: 21849
	[PublicizedFrom(EAccessModifier.Private)]
	public int originalPixelLightCount;

	// Token: 0x0400555B RID: 21851
	[PublicizedFrom(EAccessModifier.Private)]
	public SDCSUtils.TransformCatalog uiBoneCatalog;

	// Token: 0x0400555C RID: 21852
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SDCSPreviewWindow.ZoomStates state = XUiC_SDCSPreviewWindow.ZoomStates.FullBody;

	// Token: 0x0400555D RID: 21853
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseOrtho = 1f;

	// Token: 0x02000E63 RID: 3683
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eGamepadRotationMode
	{
		// Token: 0x0400555F RID: 21855
		Bumper,
		// Token: 0x04005560 RID: 21856
		Trigger,
		// Token: 0x04005561 RID: 21857
		RightStick
	}

	// Token: 0x02000E64 RID: 3684
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ZoomStates
	{
		// Token: 0x04005563 RID: 21859
		Eyes,
		// Token: 0x04005564 RID: 21860
		Head,
		// Token: 0x04005565 RID: 21861
		Chest,
		// Token: 0x04005566 RID: 21862
		FullBody
	}
}
