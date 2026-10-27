using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EE8 RID: 3816
[Preserve]
public class XUiC_CameraWindow : XUiController
{
	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x060076D3 RID: 30419 RVA: 0x002CD88E File Offset: 0x002CBA8E
	// (set) Token: 0x060076D4 RID: 30420 RVA: 0x002CD896 File Offset: 0x002CBA96
	public XUiController Owner { get; set; }

	// Token: 0x060076D5 RID: 30421 RVA: 0x002CD8A0 File Offset: 0x002CBAA0
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("camera");
		if (childById != null)
		{
			this.cameraView = (XUiV_Texture)childById.ViewComponent;
		}
		XUiController childById2 = base.GetChildById("cameraDrag");
		if (childById2 != null)
		{
			this.cameraDrag = childById2.ViewComponent;
		}
		XUiController childById3 = base.GetChildById("cameraClick");
		if (childById3 != null)
		{
			this.cameraClick = childById3.ViewComponent;
		}
	}

	// Token: 0x060076D6 RID: 30422 RVA: 0x002CD90C File Offset: 0x002CBB0C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.TileEntity == null)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = this.xui.playerUI.localPlayer.entityPlayerLocal;
		this.maximizedWindow = XUiC_CameraWindow.hackyIsOpeningMaximizedWindow;
		XUiC_CameraWindow.hackyIsOpeningMaximizedWindow = false;
		if (!this.maximizedWindow && XUiC_CameraWindow.firstPass)
		{
			XUiC_CameraWindow.firstPass = false;
		}
		if (this.cameraClick != null)
		{
			this.cameraClick.Controller.OnPress += this.OnPreviewClicked;
		}
		if (!(this.TileEntity.BlockTransform != null))
		{
			this.OnClose();
			return;
		}
		this.cameraController = this.TileEntity.BlockTransform.GetComponent<IPowerSystemCamera>();
		if (this.cameraController == null)
		{
			this.OnClose();
			return;
		}
		GameManager.Instance.StartCoroutine(this.xui.playerUI.localPlayer.entityPlayerLocal.CancelInventoryActions(delegate
		{
			Color white = Color.white;
			white.a = 0f;
			this.cameraController.SetConeColor(white);
			this.cameraController.SetConeActive(true);
			this.cameraController.SetLaserActive(true);
			if (this.cameraDrag != null)
			{
				this.TileEntity.SetUserAccessing(true);
				this.TileEntity.SetModified();
				this.xui.playerUI.CursorController.SetCursorHidden(true);
			}
			EntityPlayerLocal entityPlayerLocal2 = this.xui.playerUI.localPlayer.entityPlayerLocal;
			entityPlayerLocal2.SetFirstPersonView(false, true);
			this.cameraParentTransform = this.TileEntity.BlockTransform.FindInChilds("camera", false);
			this.myRenderTexture = new RenderTexture(this.cameraView.Size.x, this.cameraView.Size.y, 24);
			this.cameraController.SetUserAccessing(entityPlayerLocal2.entityId);
			OcclusionManager.Instance.SetMultipleCameras(true);
		}, false));
	}

	// Token: 0x060076D7 RID: 30423 RVA: 0x002CD9FC File Offset: 0x002CBBFC
	public override void OnClose()
	{
		base.OnClose();
		OcclusionManager.Instance.SetMultipleCameras(false);
		if (!XUiC_CameraWindow.hackyIsOpeningMaximizedWindow && !this.maximizedWindow)
		{
			XUiC_CameraWindow.firstPass = false;
			if (this.xui != null && this.xui.playerUI != null && this.xui.playerUI.entityPlayer != null && this.xui.playerUI.entityPlayer.emodel != null)
			{
				this.xui.playerUI.entityPlayer.SwitchToPreferredCameraMode(true);
			}
		}
		if (this.cameraController != null)
		{
			this.cameraController.SetConeActive(false);
			this.cameraController.SetLaserActive(false);
			this.cameraController.SetConeColor(this.cameraController.GetOriginalConeColor());
			this.cameraController.SetUserAccessing(-1);
			this.cameraController = null;
		}
		this.DestroyCamera();
		RenderTexture renderTexture = this.myRenderTexture;
		if (renderTexture != null)
		{
			renderTexture.Release();
		}
		this.myRenderTexture = null;
		this.cameraView.Texture = null;
		if (this.cameraClick != null)
		{
			this.cameraClick.Controller.OnPress -= this.OnPreviewClicked;
		}
		if (this.TileEntity == null)
		{
			return;
		}
		if (this.cameraDrag != null)
		{
			this.TileEntity.SetUserAccessing(false);
			this.TileEntity.SetModified();
			if (this.xui != null && this.xui.playerUI != null && this.xui.playerUI.uiCamera != null && this.xui.playerUI.CursorController != null)
			{
				this.xui.playerUI.CursorController.SetCursorHidden(false);
			}
		}
		if (WireManager.Instance != null)
		{
			WireManager.Instance.RefreshPulseObjects();
		}
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x060076D8 RID: 30424 RVA: 0x002CDBE8 File Offset: 0x002CBDE8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.cameraParentTransform == null && this.TileEntity != null)
		{
			if (this.TileEntity.BlockTransform == null)
			{
				this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
				return;
			}
			this.cameraParentTransform = this.TileEntity.BlockTransform.FindInChilds("camera", false);
		}
		if (this.sensorCamera == null && this.cameraParentTransform != null)
		{
			this.CreateCamera();
		}
		if (this.sensorCamera != null)
		{
			this.sensorCamera.backgroundColor = GameManager.Instance.World.m_WorldEnvironment.GetAmbientColor();
		}
		Vector3i vector3i = this.TileEntity.ToWorldPos();
		vector3i.y++;
		this.isBuried = GameManager.Instance.World.GetBlock(vector3i).Block.shape.IsTerrain();
		if (this.sensorCamera != null)
		{
			bool flag = this.TileEntity.IsPowered && !this.isBuried;
			this.sensorCamera.gameObject.SetActive(flag);
			this.cameraView.IsVisible = flag;
		}
		if (this.cameraDrag != null)
		{
			Vector2 vector = this.xui.playerUI.entityPlayer.MoveController.GetCameraInputSensitivity() * 2f;
			float num;
			float num2;
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				num = this.xui.playerUI.playerInput.Look.X;
				num2 = this.xui.playerUI.playerInput.Look.Y;
			}
			else
			{
				num = this.xui.playerUI.playerInput.GUIActions.Look.X;
				num2 = this.xui.playerUI.playerInput.GUIActions.Look.Y;
			}
			num *= vector.x;
			num2 *= -1f * vector.y;
			if (this.cameraController is MotionSensorController || this.cameraController is SpotlightController)
			{
				this.TileEntity.CenteredYaw = Mathf.Clamp(this.TileEntity.CenteredYaw + num, -90f, 90f);
				this.TileEntity.CenteredPitch = Mathf.Clamp(this.TileEntity.CenteredPitch + num2, -80f, 80f);
			}
			else
			{
				this.TileEntity.CenteredYaw = this.TileEntity.CenteredYaw + num;
				this.TileEntity.CenteredPitch = Mathf.Clamp(this.TileEntity.CenteredPitch + num2, -80f, 80f);
			}
			PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
			PlayerActionsGUI guiactions = playerInput.GUIActions;
			AutoTurretController autoTurretController = this.cameraController as AutoTurretController;
			if (autoTurretController != null)
			{
				autoTurretController.FireController.PlayerFire(guiactions.Submit.IsPressed || playerInput.Primary.IsPressed);
			}
		}
		if (Time.realtimeSinceStartup > this.nextModifiedTime)
		{
			this.TileEntity.SetModified();
			this.nextModifiedTime = Time.realtimeSinceStartup + 1f;
		}
	}

	// Token: 0x060076D9 RID: 30425 RVA: 0x002CDF34 File Offset: 0x002CC134
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateCamera()
	{
		GameObject gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/ElectricityCamera"), this.cameraParentTransform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		this.sensorCamera = gameObject.GetComponent<Camera>();
		this.sensorCamera.nearClipPlane = 0.01f;
		this.sensorCamera.depth = -10f;
		this.sensorCamera.farClipPlane = 1000f;
		this.sensorCamera.fieldOfView = 80f;
		this.sensorCamera.cullingMask &= -513;
		this.sensorCamera.renderingPath = RenderingPath.DeferredShading;
		this.sensorCamera.clearFlags = CameraClearFlags.Color;
		this.sensorCamera.targetTexture = this.myRenderTexture;
		if (SystemInfo.graphicsDeviceVersion.Contains("OpenGL"))
		{
			this.cameraView.Flip = UIBasicSprite.Flip.Vertically;
		}
		this.cameraView.Texture = this.sensorCamera.targetTexture;
	}

	// Token: 0x060076DA RID: 30426 RVA: 0x002CE03B File Offset: 0x002CC23B
	[PublicizedFrom(EAccessModifier.Private)]
	public void DestroyCamera()
	{
		if (this.sensorCamera != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.sensorCamera.gameObject);
		}
	}

	// Token: 0x060076DB RID: 30427 RVA: 0x002CE05C File Offset: 0x002CC25C
	public void OnPreviewClicked(XUiController _sender, int _mouseButton)
	{
		if (this.TileEntity.IsPowered && !this.isBuried)
		{
			XUiC_CameraWindow.hackyIsOpeningMaximizedWindow = true;
			XUiC_CameraWindow.lastWindowGroup = this.windowGroup.Id;
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			XUiC_PowerCameraWindowGroup xuiC_PowerCameraWindowGroup = (XUiC_PowerCameraWindowGroup)((XUiWindowGroup)this.xui.playerUI.windowManager.GetWindow("powercamera")).Controller;
			xuiC_PowerCameraWindowGroup.TileEntity = this.TileEntity;
			xuiC_PowerCameraWindowGroup.UseEdgeDetection = this.UseEdgeDetection;
			this.xui.playerUI.windowManager.Open("powercamera", true);
			this.xui.playerUI.entityPlayer.PlayOneShot("motion_sensor_trigger", false, false, false, null, 1f);
			return;
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		string text = (this.TileEntity.IsPowered ? "ttTurretIsBuried" : "ttRequiresPowerForCamera");
		GameManager.ShowTooltip(entityPlayer, text, string.Empty, "ui_denied", null, false, false, 0f);
	}

	// Token: 0x04005819 RID: 22553
	public TileEntityPowered TileEntity;

	// Token: 0x0400581A RID: 22554
	public bool UseEdgeDetection = true;

	// Token: 0x0400581B RID: 22555
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture cameraView;

	// Token: 0x0400581C RID: 22556
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView cameraDrag;

	// Token: 0x0400581D RID: 22557
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView cameraClick;

	// Token: 0x0400581E RID: 22558
	[PublicizedFrom(EAccessModifier.Private)]
	public IPowerSystemCamera cameraController;

	// Token: 0x0400581F RID: 22559
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform cameraParentTransform;

	// Token: 0x04005820 RID: 22560
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera sensorCamera;

	// Token: 0x04005821 RID: 22561
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture myRenderTexture;

	// Token: 0x04005822 RID: 22562
	public static bool hackyIsOpeningMaximizedWindow;

	// Token: 0x04005823 RID: 22563
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool firstPass = true;

	// Token: 0x04005824 RID: 22564
	[PublicizedFrom(EAccessModifier.Private)]
	public float nextModifiedTime;

	// Token: 0x04005825 RID: 22565
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBuried;

	// Token: 0x04005826 RID: 22566
	[PublicizedFrom(EAccessModifier.Private)]
	public bool maximizedWindow;

	// Token: 0x04005827 RID: 22567
	public static string lastWindowGroup;

	// Token: 0x04005828 RID: 22568
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDraggingCamera;
}
