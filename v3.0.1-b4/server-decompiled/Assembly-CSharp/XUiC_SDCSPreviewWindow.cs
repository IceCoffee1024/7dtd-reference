using System;
using HorizonBasedAmbientOcclusion;
using PI.NGSS;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E33 RID: 3635
[Preserve]
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiC_SDCSPreviewWindow : XUiController
{
	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x06006FDB RID: 28635 RVA: 0x002B0C59 File Offset: 0x002AEE59
	// (set) Token: 0x06006FDC RID: 28636 RVA: 0x002B0C61 File Offset: 0x002AEE61
	[XuiXmlAttribute("can_zoom", false)]
	public bool CanZoom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x06006FDD RID: 28637 RVA: 0x002B0C6A File Offset: 0x002AEE6A
	// (set) Token: 0x06006FDE RID: 28638 RVA: 0x002B0C72 File Offset: 0x002AEE72
	[XuiXmlAttribute("gamepad_rotation_mode", false)]
	public XUiC_SDCSPreviewWindow.eGamepadRotationMode GamepadRotationMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006FDF RID: 28639 RVA: 0x002B0C7C File Offset: 0x002AEE7C
	public override void Init()
	{
		base.Init();
		this.textPreview = (XUiV_Texture)base.GetChildById("playerPreview").ViewComponent;
		this.renderTextureSystem = new RenderTextureSystem();
		base.OnScroll += this.OnScrolled;
		base.OnDrag += this.OnDragged;
	}

	// Token: 0x06006FE0 RID: 28640 RVA: 0x002B0CD9 File Offset: 0x002AEED9
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnScroll -= this.OnScrolled;
		base.OnDrag -= this.OnDragged;
	}

	// Token: 0x06006FE1 RID: 28641 RVA: 0x002B0D05 File Offset: 0x002AEF05
	[XuiBindEvent("OnPress", "zoomButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ZoomButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.toggleHeadZoom();
	}

	// Token: 0x06006FE2 RID: 28642 RVA: 0x002B0D0D File Offset: 0x002AEF0D
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

	// Token: 0x06006FE3 RID: 28643 RVA: 0x002B0D1F File Offset: 0x002AEF1F
	[PublicizedFrom(EAccessModifier.Private)]
	public void startViz()
	{
		this.originalPixelLightCount = QualitySettings.pixelLightCount;
		QualitySettings.pixelLightCount = 4;
	}

	// Token: 0x06006FE4 RID: 28644 RVA: 0x002B0D32 File Offset: 0x002AEF32
	[PublicizedFrom(EAccessModifier.Private)]
	public void endViz()
	{
		SDCSUtils.UnloadViz(this.previewTransform);
		this.renderTextureSystem.Cleanup();
		this.lastProfile = "";
		this.lastFieldOfView = 54f;
		QualitySettings.pixelLightCount = this.originalPixelLightCount;
	}

	// Token: 0x06006FE5 RID: 28645 RVA: 0x002B0D6C File Offset: 0x002AEF6C
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

	// Token: 0x06006FE6 RID: 28646 RVA: 0x002B0DFC File Offset: 0x002AEFFC
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

	// Token: 0x06006FE7 RID: 28647 RVA: 0x002B0EDC File Offset: 0x002AF0DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDragged(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		float x = _mousePositionDelta.x;
		if (this.xui.playerUI.CursorController.GetMouseButton(UICamera.MouseButton.RightButton))
		{
			this.cameraRotate(x);
		}
	}

	// Token: 0x06006FE8 RID: 28648 RVA: 0x002B0F10 File Offset: 0x002AF110
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

	// Token: 0x06006FE9 RID: 28649 RVA: 0x002B100C File Offset: 0x002AF20C
	[PublicizedFrom(EAccessModifier.Private)]
	public void cameraRotate(float _value)
	{
		this.renderCamera.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, _value);
		this.renderTextureSystem.LightGO.transform.RotateAround(this.previewTransform.transform.position, Vector3.up, _value);
	}

	// Token: 0x06006FEA RID: 28650 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrolled(XUiController _sender, float _delta)
	{
	}

	// Token: 0x06006FEB RID: 28651 RVA: 0x002B106C File Offset: 0x002AF26C
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

	// Token: 0x06006FEC RID: 28652 RVA: 0x002B1148 File Offset: 0x002AF348
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

	// Token: 0x06006FED RID: 28653 RVA: 0x002B11CC File Offset: 0x002AF3CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToFullBodyZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(19f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 54f;
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06006FEE RID: 28654 RVA: 0x002B1250 File Offset: 0x002AF450
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToHeadZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(1.5f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 12f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.015f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06006FEF RID: 28655 RVA: 0x002B12FC File Offset: 0x002AF4FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToChestZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, Quaternion.AngleAxis(5f, new Vector3(1f, 0f, 0f)) * this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 20f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.02f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06006FF0 RID: 28656 RVA: 0x002B13A8 File Offset: 0x002AF5A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToEyeZoom()
	{
		this.renderCamera.transform.SetLocalPositionAndRotation(this.originalCamPosition, this.originalCamRotation);
		this.renderTextureSystem.LightGO.transform.SetLocalPositionAndRotation(this.originalLightPosition, this.originalLightRotation);
		this.renderCamera.fieldOfView = 6f;
		this.renderTextureSystem.TargetGO.transform.localPosition = new Vector3(0.015f, -0.78f, 2.14f);
		this.setCameraInitialRotationOffset();
	}

	// Token: 0x06006FF1 RID: 28657 RVA: 0x002B1434 File Offset: 0x002AF634
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

	// Token: 0x06006FF2 RID: 28658 RVA: 0x002B14B4 File Offset: 0x002AF6B4
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

	// Token: 0x06006FF3 RID: 28659 RVA: 0x002B1514 File Offset: 0x002AF714
	public void ZoomToEye()
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && this.xui.playerUI.playerInput.GUIActions.TriggerAxis.Value != 0f)
		{
			return;
		}
		this.state = XUiC_SDCSPreviewWindow.ZoomStates.Eyes;
		this.setToEyeZoom();
	}

	// Token: 0x06006FF4 RID: 28660 RVA: 0x002B1568 File Offset: 0x002AF768
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

	// Token: 0x04005448 RID: 21576
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x04005449 RID: 21577
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textPreview;

	// Token: 0x0400544A RID: 21578
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture renderTexture;

	// Token: 0x0400544B RID: 21579
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera renderCamera;

	// Token: 0x0400544C RID: 21580
	public Transform TargetTransform;

	// Token: 0x0400544D RID: 21581
	public GameObject RotateTable;

	// Token: 0x0400544E RID: 21582
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewTransform;

	// Token: 0x0400544F RID: 21583
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastProfile;

	// Token: 0x04005450 RID: 21584
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastFieldOfView;

	// Token: 0x04005451 RID: 21585
	public Archetype Archetype;

	// Token: 0x04005452 RID: 21586
	[PublicizedFrom(EAccessModifier.Private)]
	public CharacterGazeController characterGazeController;

	// Token: 0x04005453 RID: 21587
	[XuiBindComponent("zoomButton", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button zoomButton;

	// Token: 0x04005454 RID: 21588
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 originalCamPosition;

	// Token: 0x04005455 RID: 21589
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 originalLightPosition;

	// Token: 0x04005456 RID: 21590
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion originalCamRotation;

	// Token: 0x04005457 RID: 21591
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion originalLightRotation;

	// Token: 0x04005459 RID: 21593
	[PublicizedFrom(EAccessModifier.Private)]
	public int originalPixelLightCount;

	// Token: 0x0400545B RID: 21595
	[PublicizedFrom(EAccessModifier.Private)]
	public SDCSUtils.TransformCatalog uiBoneCatalog;

	// Token: 0x0400545C RID: 21596
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SDCSPreviewWindow.ZoomStates state = XUiC_SDCSPreviewWindow.ZoomStates.FullBody;

	// Token: 0x0400545D RID: 21597
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseOrtho = 1f;

	// Token: 0x02000E34 RID: 3636
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eGamepadRotationMode
	{
		// Token: 0x0400545F RID: 21599
		Bumper,
		// Token: 0x04005460 RID: 21600
		Trigger,
		// Token: 0x04005461 RID: 21601
		RightStick
	}

	// Token: 0x02000E35 RID: 3637
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ZoomStates
	{
		// Token: 0x04005463 RID: 21603
		Eyes,
		// Token: 0x04005464 RID: 21604
		Head,
		// Token: 0x04005465 RID: 21605
		Chest,
		// Token: 0x04005466 RID: 21606
		FullBody
	}
}
