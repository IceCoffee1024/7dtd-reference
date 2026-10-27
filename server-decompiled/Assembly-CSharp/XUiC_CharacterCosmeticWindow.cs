using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EFD RID: 3837
[Preserve]
public class XUiC_CharacterCosmeticWindow : XUiController
{
	// Token: 0x060077BF RID: 30655 RVA: 0x002D34BC File Offset: 0x002D16BC
	public override void Init()
	{
		base.Init();
		this.lblName = (XUiV_Label)base.GetChildById("characterName").ViewComponent;
		this.textPreview = (XUiV_Texture)base.GetChildById("playerPreviewSDCS").ViewComponent;
		this.IsDirty = true;
		XUiM_PlayerEquipment.HandleRefreshEquipment += this.XUiM_PlayerEquipment_HandleRefreshEquipment;
		this.xui.playerUI.OnUIShutdown += this.HandleUIShutdown;
		this.xui.OnShutdown += this.HandleUIShutdown;
	}

	// Token: 0x060077C0 RID: 30656 RVA: 0x002D3550 File Offset: 0x002D1750
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleUIShutdown()
	{
		this.xui.playerUI.OnUIShutdown -= this.HandleUIShutdown;
		this.xui.OnShutdown -= this.HandleUIShutdown;
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
	}

	// Token: 0x060077C1 RID: 30657 RVA: 0x002D35A1 File Offset: 0x002D17A1
	[PublicizedFrom(EAccessModifier.Private)]
	public void PreviewFrame_OnHover(XUiController _sender, bool _isOver)
	{
		this.isMouseOverPreview = _isOver;
	}

	// Token: 0x060077C2 RID: 30658 RVA: 0x002D35AC File Offset: 0x002D17AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void PreviewFrame_OnPress(XUiController _sender, int _mouseButton)
	{
		if (!this.xui.DragAndDropWindow.IsEmpty())
		{
			ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
			ItemStack itemStack = this.xui.PlayerEquipment.EquipItem(currentStack);
			if (currentStack != itemStack)
			{
				this.xui.DragAndDropWindow.SetCurrentStack(itemStack, true);
				this.xui.DragAndDropWindow.PickUpType = XUiC_ItemStack.StackLocationTypes.Equipment;
			}
		}
	}

	// Token: 0x060077C3 RID: 30659 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment _playerEquipment)
	{
	}

	// Token: 0x060077C4 RID: 30660 RVA: 0x002D3618 File Offset: 0x002D1818
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null || GameManager.Instance.World == null)
		{
			return;
		}
		if (this.ep == null)
		{
			this.ep = this.xui.playerUI.entityPlayer;
		}
		if (this.IsDirty)
		{
			if (this.player == null)
			{
				return;
			}
			this.lblName.Text = this.player.PlayerDisplayName;
			this.IsDirty = false;
			base.RefreshBindings();
		}
		if (this.isPreviewDirty)
		{
			this.MakePreview();
		}
		this.textPreview.Texture = this.renderTextureSystem.RenderTex;
		if (this.characterPivot != null)
		{
			XUi xui = this.xui;
			PlayerActionsGUI playerActionsGUI;
			if (xui == null)
			{
				playerActionsGUI = null;
			}
			else
			{
				LocalPlayerUI playerUI = xui.playerUI;
				if (playerUI == null)
				{
					playerActionsGUI = null;
				}
				else
				{
					PlayerActionsLocal playerInput = playerUI.playerInput;
					playerActionsGUI = ((playerInput != null) ? playerInput.GUIActions : null);
				}
			}
			PlayerActionsGUI playerActionsGUI2 = playerActionsGUI;
			if (playerActionsGUI2 != null)
			{
				bool flag = PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard;
				this.xui.playerUI.CursorController.GetMouseButton(UICamera.MouseButton.LeftButton);
				bool flag2 = false;
				if (flag)
				{
					flag2 = true;
					float x = playerActionsGUI2.Camera.Value.x;
					if (Mathf.Abs(x) > 0.15f)
					{
						float num = -1f;
						this.characterPivot.Rotate(Vector3.up, num * x * 200f * _dt, Space.World);
					}
				}
				if (this.cursorLockActive)
				{
					if (flag2)
					{
						this.MaintainCursorLock();
					}
					else
					{
						this.EndCursorLock();
					}
				}
			}
			bool mouseButton = this.xui.playerUI.CursorController.GetMouseButton(UICamera.MouseButton.LeftButton);
			bool mouseButton2 = this.xui.playerUI.CursorController.GetMouseButton(UICamera.MouseButton.RightButton);
			if (mouseButton || mouseButton2)
			{
				if (!this.isDragging && this.isMouseOverPreview)
				{
					this.isDragging = true;
					this.lastMousePos = Input.mousePosition;
				}
				if (this.isDragging)
				{
					Vector2 vector = Input.mousePosition;
					Vector2 vector2 = vector - this.lastMousePos;
					this.lastMousePos = vector;
					if (Mathf.Abs(vector2.x) > 0.01f)
					{
						float num2 = vector2.x * 0.4f;
						this.characterPivot.Rotate(Vector3.up, -num2, Space.World);
					}
				}
			}
			else
			{
				this.isDragging = false;
			}
		}
		base.Update(_dt);
	}

	// Token: 0x060077C5 RID: 30661 RVA: 0x002D385C File Offset: 0x002D1A5C
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		this.isPreviewDirty = true;
		this.player = this.xui.playerUI.entityPlayer;
		this.previewFrame = base.GetChildById("previewFrameSDCS");
		if (this.previewFrame != null)
		{
			this.previewFrame.OnPress += this.PreviewFrame_OnPress;
			this.previewFrame.OnHover += this.PreviewFrame_OnHover;
		}
		this.textPreview = (XUiV_Texture)base.GetChildById("playerPreviewSDCS").ViewComponent;
		if (this.renderTextureSystem.ParentGO == null)
		{
			this.renderTextureSystem.Create("playerpreview", new GameObject(), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), this.textPreview.Size, true, false, 1f);
		}
		this.renderCamera = this.renderTextureSystem.CameraGO.GetComponent<Camera>();
		this.renderCamera.transform.localPosition = new Vector3(0f, 1.5f, 0f);
		this.renderCamera.transform.localRotation = Quaternion.Euler(12f, 0f, 0f);
		this.renderCamera.orthographic = false;
		this.renderCamera.fieldOfView = 54f;
		this.renderCamera.renderingPath = RenderingPath.DeferredShading;
		if (this.characterPivot == null)
		{
			GameObject gameObject = new GameObject("CharacterPivot");
			this.characterPivot = gameObject.transform;
			this.characterPivot.SetParent(this.renderTextureSystem.ParentGO.transform, false);
			this.characterPivot.localPosition = new Vector3(0f, 0f, 2.15f);
			this.characterPivot.localRotation = Quaternion.AngleAxis(-30f, Vector3.up);
		}
		this.renderTextureSystem.LightGO.GetComponent<Light>().enabled = false;
		GameObject gameObject2 = new GameObject("Key Light", new Type[] { typeof(Light) });
		gameObject2.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject2.transform.SetPositionAndRotation(new Vector3(1.5f, 2.5f, -1.5f), Quaternion.Euler(20f, -20f, 0f));
		Light component = gameObject2.GetComponent<Light>();
		component.color = new Color(0.9f, 0.8f, 0.7f, 1f);
		component.type = LightType.Spot;
		component.range = 20f;
		component.spotAngle = 60f;
		component.intensity = 1.5f;
		component.shadows = LightShadows.Hard;
		component.shadowStrength = 0.2f;
		component.shadowBias = 0.005f;
		component.cullingMask = 2048;
		GameObject gameObject3 = new GameObject("Fill Light", new Type[] { typeof(Light) });
		gameObject3.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject3.transform.SetPositionAndRotation(new Vector3(-2f, 3f, 0f), Quaternion.Euler(35f, 45f, 0f));
		Light component2 = gameObject3.GetComponent<Light>();
		component2.color = new Color(1f, 1f, 1f, 1f);
		component2.type = LightType.Spot;
		component2.range = 20f;
		component2.spotAngle = 60f;
		component2.intensity = 0.5f;
		component2.shadows = LightShadows.Hard;
		component2.shadowStrength = 0.2f;
		component2.shadowBias = 0.005f;
		component2.cullingMask = 2048;
		GameObject gameObject4 = new GameObject("Fill 2 Light", new Type[] { typeof(Light) });
		gameObject4.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject4.transform.SetPositionAndRotation(new Vector3(0f, -0.5f, -1.5f), Quaternion.Euler(-20f, 0f, 0f));
		Light component3 = gameObject4.GetComponent<Light>();
		component3.color = new Color(1f, 1f, 1f, 1f);
		component3.type = LightType.Spot;
		component3.range = 20f;
		component3.spotAngle = 60f;
		component3.intensity = 0.5f;
		component3.shadows = LightShadows.Hard;
		component3.shadowStrength = 0.2f;
		component3.shadowBias = 0.005f;
		component3.cullingMask = 2048;
		GameObject gameObject5 = new GameObject("Back Light", new Type[] { typeof(Light) });
		gameObject5.transform.SetParent(this.renderTextureSystem.LightGO.transform, false);
		gameObject5.transform.SetPositionAndRotation(new Vector3(-2f, 5f, 2f), Quaternion.Euler(60f, 105f, 0f));
		Light component4 = gameObject5.GetComponent<Light>();
		component4.color = new Color(0.4f, 0.75f, 1f, 1f);
		component4.type = LightType.Spot;
		component4.spotAngle = 60f;
		component4.range = 20f;
		component4.intensity = 1.5f;
		component4.shadows = LightShadows.Hard;
		component4.shadowStrength = 0.2f;
		component4.shadowBias = 0.005f;
		component4.cullingMask = 2048;
		if (this.player as EntityPlayerLocal != null && this.player.emodel as EModelSDCS != null)
		{
			XUiM_PlayerEquipment.HandleRefreshEquipment += this.XUiM_PlayerEquipment_HandleRefreshEquipment1;
		}
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.CharacterPreview, 0f);
	}

	// Token: 0x060077C6 RID: 30662 RVA: 0x002D3E3C File Offset: 0x002D203C
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment1(XUiM_PlayerEquipment playerEquipment)
	{
		if (!base.IsOpen)
		{
			return;
		}
		this.MakePreview();
	}

	// Token: 0x060077C7 RID: 30663 RVA: 0x002D3E50 File Offset: 0x002D2050
	public override void OnClose()
	{
		base.OnClose();
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment1;
		SDCSUtils.UnloadViz(this.previewSDCSObj);
		this.renderTextureSystem.Cleanup();
		this.EndCursorLock();
		this.characterPivot = null;
		this.isMouseOverPreview = false;
		this.isDragging = false;
		if (this.previewFrame != null)
		{
			this.previewFrame.OnPress -= this.PreviewFrame_OnPress;
			this.previewFrame.OnHover -= this.PreviewFrame_OnHover;
			this.previewFrame = null;
		}
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.CharacterPreview);
	}

	// Token: 0x060077C8 RID: 30664 RVA: 0x002D3EF4 File Offset: 0x002D20F4
	public void MakePreview()
	{
		if (this.ep == null)
		{
			return;
		}
		if (this.ep.emodel == null)
		{
			return;
		}
		EModelSDCS emodelSDCS = this.ep.emodel as EModelSDCS;
		if (emodelSDCS != null)
		{
			this.isPreviewDirty = false;
			SDCSUtils.CreateVizUI(emodelSDCS.Archetype, ref this.previewSDCSObj, ref this.transformCatalog, this.ep, true);
			Utils.SetLayerRecursively(this.previewSDCSObj, 11);
			Transform transform = this.previewSDCSObj.transform;
			transform.SetParent((this.characterPivot != null) ? this.characterPivot : this.renderTextureSystem.ParentGO.transform, false);
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			CharacterGazeController componentInChildren = this.previewSDCSObj.GetComponentInChildren<CharacterGazeController>();
			if (componentInChildren && this.renderCamera != null)
			{
				componentInChildren.LookAtTransformOverride = this.renderCamera.transform;
			}
		}
	}

	// Token: 0x060077C9 RID: 30665 RVA: 0x002D3FFC File Offset: 0x002D21FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BeginCursorLock()
	{
		if (this.cursorLockActive)
		{
			return;
		}
		this.cursorLockActive = true;
		this.lockedCursorPos = this.xui.playerUI.CursorController.GetScreenPosition();
		this.prevCursorHidden = this.xui.playerUI.CursorController.VirtualCursorHidden;
		this.xui.playerUI.CursorController.VirtualCursorHidden = true;
	}

	// Token: 0x060077CA RID: 30666 RVA: 0x002D4065 File Offset: 0x002D2265
	[PublicizedFrom(EAccessModifier.Private)]
	public void MaintainCursorLock()
	{
		this.xui.playerUI.CursorController.SetScreenPosition(this.lockedCursorPos);
	}

	// Token: 0x060077CB RID: 30667 RVA: 0x002D4082 File Offset: 0x002D2282
	[PublicizedFrom(EAccessModifier.Private)]
	public void EndCursorLock()
	{
		if (!this.cursorLockActive)
		{
			return;
		}
		this.cursorLockActive = false;
		this.xui.playerUI.CursorController.VirtualCursorHidden = this.prevCursorHidden;
	}

	// Token: 0x060077CC RID: 30668 RVA: 0x002D40B0 File Offset: 0x002D22B0
	[XuiBindEvent("OnPressed", "btnClearAll")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnClearAll_OnPress(XUiController _sender, int _mouseButton)
	{
		Equipment equipment = this.xui.PlayerEquipment.Equipment;
		equipment.ClearTempCosmeticSlot();
		equipment.ClearCosmeticSlots();
		XUiC_CharacterCosmeticsListWindow xuiC_CharacterCosmeticsListWindow;
		if (base.WindowGroup.Controller.TryGetChildController<XUiC_CharacterCosmeticsListWindow>("", out xuiC_CharacterCosmeticsListWindow))
		{
			xuiC_CharacterCosmeticsListWindow.SetEmptyCosmeticForCurrentCategory();
		}
		((XUiC_CharacterCosmeticWindowGroup)base.WindowGroup.Controller).ResetPreview();
		EModelSDCS emodelSDCS = this.xui.playerUI.entityPlayer.emodel as EModelSDCS;
		if (emodelSDCS != null)
		{
			emodelSDCS.GenerateMeshes();
		}
	}

	// Token: 0x040058A1 RID: 22689
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x040058A2 RID: 22690
	[XuiBindComponent("btnClearAll", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnClearAll;

	// Token: 0x040058A3 RID: 22691
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController previewFrame;

	// Token: 0x040058A4 RID: 22692
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textPreview;

	// Token: 0x040058A5 RID: 22693
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer ep;

	// Token: 0x040058A6 RID: 22694
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera cam;

	// Token: 0x040058A7 RID: 22695
	public RuntimeAnimatorController animationController;

	// Token: 0x040058A8 RID: 22696
	public float atlasResolutionScale;

	// Token: 0x040058A9 RID: 22697
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RenderTextureSystem renderTextureSystem = new RenderTextureSystem();

	// Token: 0x040058AA RID: 22698
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPreviewDirty;

	// Token: 0x040058AB RID: 22699
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer player;

	// Token: 0x040058AC RID: 22700
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform characterPivot;

	// Token: 0x040058AD RID: 22701
	[PublicizedFrom(EAccessModifier.Private)]
	public const float DragRotateSpeed = 0.4f;

	// Token: 0x040058AE RID: 22702
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ControllerRotateSpeed = 200f;

	// Token: 0x040058AF RID: 22703
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMouseOverPreview;

	// Token: 0x040058B0 RID: 22704
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragging;

	// Token: 0x040058B1 RID: 22705
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 lastMousePos;

	// Token: 0x040058B2 RID: 22706
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RotationStickDeadzone = 0.15f;

	// Token: 0x040058B3 RID: 22707
	[PublicizedFrom(EAccessModifier.Private)]
	public const bool InvertRotationStick = true;

	// Token: 0x040058B4 RID: 22708
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cursorLockActive;

	// Token: 0x040058B5 RID: 22709
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 lockedCursorPos;

	// Token: 0x040058B6 RID: 22710
	[PublicizedFrom(EAccessModifier.Private)]
	public bool prevCursorHidden;

	// Token: 0x040058B7 RID: 22711
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewSDCSObj;

	// Token: 0x040058B8 RID: 22712
	[PublicizedFrom(EAccessModifier.Private)]
	public SDCSUtils.TransformCatalog transformCatalog;

	// Token: 0x040058B9 RID: 22713
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera renderCamera;

	// Token: 0x040058BA RID: 22714
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
