using System;
using HorizonBasedAmbientOcclusion;
using PI.NGSS;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E57 RID: 3671
[Preserve]
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiC_SDCSPreviewWindow : XUiController
{
	// Token: 0x17000CCF RID: 3279
	// (get) Token: 0x06007073 RID: 28787 RVA: 0x002B752D File Offset: 0x002B572D
	// (set) Token: 0x06007074 RID: 28788 RVA: 0x002B7535 File Offset: 0x002B5735
	[XuiXmlAttribute("can_zoom", false)]
	public bool CanZoom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000CD0 RID: 3280
	// (get) Token: 0x06007075 RID: 28789 RVA: 0x002B753E File Offset: 0x002B573E
	// (set) Token: 0x06007076 RID: 28790 RVA: 0x002B7546 File Offset: 0x002B5746
	[XuiXmlAttribute("gamepad_rotation_mode", false)]
	public XUiC_SDCSPreviewWindow.eGamepadRotationMode GamepadRotationMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007077 RID: 28791 RVA: 0x002B7550 File Offset: 0x002B5750
	public override void Init()
	{
		base.Init();
		this.textPreview = (XUiV_Texture)base.GetChildById("playerPreview").ViewComponent;
		this.renderTextureSystem = new RenderTextureSystem();
		base.OnScroll += this.OnScrolled;
		base.OnDrag += this.OnDragged;
	}

	// Token: 0x06007078 RID: 28792 RVA: 0x002B75AD File Offset: 0x002B57AD
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnScroll -= this.OnScrolled;
		base.OnDrag -= this.OnDragged;
	}

	// Token: 0x06007079 RID: 28793 RVA: 0x002B75D9 File Offset: 0x002B57D9
	[XuiBindEvent("OnPress", "zoomButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ZoomButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.toggleHeadZoom();
	}

	// Token: 0x0600707A RID: 28794 RVA: 0x002B75E1 File Offset: 0x002B57E1
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

	// Token: 0x0600707B RID: 28795 RVA: 0x002B75F3 File Offset: 0x002B57F3
	[PublicizedFrom(EAccessModifier.Private)]
	public void startViz()
	{
		this.originalPixelLightCount = QualitySettings.pixelLightCount;
		QualitySettings.pixelLightCount = 4;
	}

	// Token: 0x0600707C RID: 28796 RVA: 0x002B7606 File Offset: 0x002B5806
	[PublicizedFrom(EAccessModifier.Private)]
	public void endViz()
	{
		SDCSUtils.UnloadViz(this.previewTransform);
		this.renderTextureSystem.Cleanup();
		this.lastProfile = "";
		this.lastFieldOfView = 54f;
		QualitySettings.pixelLightCount = this.originalPixelLightCount;
	}

	// Token: 0x0600707D RID: 28797 RVA: 0x002B7640 File Offset: 0x002B5840
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

	// Token: 0x0600707E RID: 28798 RVA: 0x002B76D0 File Offset: 0x002B58D0
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

	// Token: 0x0600707F RID: 28799 RVA: 0x002B77B0 File Offset: 0x002B59B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDragged(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		float x = _mousePositionDelta.x;
		if (this.xui.playerUI.CursorController.GetMouseButton(UICamera.MouseButton.RightButton))
		{
			this.cameraRotate(x);
		}
	}

	// Token: 0x06007080 RID: 28800 RVA: 0x002B77E4 File Offset: 0x002B59E4
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

	// Token: 0x06007081 RID: 28801 RVA: 0x002B78E0 File Offset: 0x002B5AE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void cameraRotate(float _value)
	{
		this.renderCamera.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, _value);
		this.renderTextureSystem.LightGO.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, _value);
	}

	// Token: 0x06007082 RID: 28802 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrolled(XUiController _sender, float _delta)
	{
	}

	// Token: 0x06007083 RID: 28803 RVA: 0x002B7940 File Offset: 0x002B5B40
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

	// Token: 0x06007084 RID: 28804 RVA: 0x002B7A1C File Offset: 0x002B5C1C
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

	// Token: 0x06007085 RID: 28805 RVA: 0x002B7AA0 File Offset: 0x002B5CA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToFullBodyZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(19f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 54f;
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06007086 RID: 28806 RVA: 0x002B7B24 File Offset: 0x002B5D24
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToHeadZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(1.5f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 12f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.015f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06007087 RID: 28807 RVA: 0x002B7BD0 File Offset: 0x002B5DD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToChestZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(5f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 20f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.02f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06007088 RID: 28808 RVA: 0x002B7C7C File Offset: 0x002B5E7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToEyeZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 6f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.015f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06007089 RID: 28809 RVA: 0x002B7D08 File Offset: 0x002B5F08
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

	// Token: 0x0600708A RID: 28810 RVA: 0x002B7D88 File Offset: 0x002B5F88
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

	// Token: 0x0600708B RID: 28811 RVA: 0x002B7DE8 File Offset: 0x002B5FE8
	public void ZoomToEye()
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && this.xui.playerUI.playerInput.GUIActions.TriggerAxis.Value != 0f)
		{
			return;
		}
		this.state = XUiC_SDCSPreviewWindow.ZoomStates.Eyes;
		this.setToEyeZoom();
	}

	// Token: 0x0600708C RID: 28812 RVA: 0x002B7E3C File Offset: 0x002B603C
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

	// Token: 0x04005523 RID: 21795
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x04005524 RID: 21796
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textPreview;

	// Token: 0x04005525 RID: 21797
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture renderTexture;

	// Token: 0x04005526 RID: 21798
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera renderCamera;

	// Token: 0x04005527 RID: 21799
	public Transform TargetTransform;

	// Token: 0x04005528 RID: 21800
	public GameObject RotateTable;

	// Token: 0x04005529 RID: 21801
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewTransform;

	// Token: 0x0400552A RID: 21802
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastProfile;

	// Token: 0x0400552B RID: 21803
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastFieldOfView;

	// Token: 0x0400552C RID: 21804
	public Archetype Archetype;

	// Token: 0x0400552D RID: 21805
	[PublicizedFrom(EAccessModifier.Private)]
	public CharacterGazeController characterGazeController;

	// Token: 0x0400552E RID: 21806
	[XuiBindComponent("zoomButton", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button zoomButton;

	// Token: 0x0400552F RID: 21807
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 originalCamPosition;

	// Token: 0x04005530 RID: 21808
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 originalLightPosition;

	// Token: 0x04005531 RID: 21809
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion originalCamRotation;

	// Token: 0x04005532 RID: 21810
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion originalLightRotation;

	// Token: 0x04005534 RID: 21812
	[PublicizedFrom(EAccessModifier.Private)]
	public int originalPixelLightCount;

	// Token: 0x04005536 RID: 21814
	[PublicizedFrom(EAccessModifier.Private)]
	public SDCSUtils.TransformCatalog uiBoneCatalog;

	// Token: 0x04005537 RID: 21815
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SDCSPreviewWindow.ZoomStates state = XUiC_SDCSPreviewWindow.ZoomStates.FullBody;

	// Token: 0x04005538 RID: 21816
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseOrtho = 1f;

	// Token: 0x02000E58 RID: 3672
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eGamepadRotationMode
	{
		// Token: 0x0400553A RID: 21818
		Bumper,
		// Token: 0x0400553B RID: 21819
		Trigger,
		// Token: 0x0400553C RID: 21820
		RightStick
	}

	// Token: 0x02000E59 RID: 3673
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ZoomStates
	{
		// Token: 0x0400553E RID: 21822
		Eyes,
		// Token: 0x0400553F RID: 21823
		Head,
		// Token: 0x04005540 RID: 21824
		Chest,
		// Token: 0x04005541 RID: 21825
		FullBody
	}
}
