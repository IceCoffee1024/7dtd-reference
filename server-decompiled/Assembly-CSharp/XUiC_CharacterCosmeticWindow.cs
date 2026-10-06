using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EF2 RID: 3826
[Preserve]
public class XUiC_CharacterCosmeticWindow : XUiController
{
	// Token: 0x06007799 RID: 30617 RVA: 0x002D1F64 File Offset: 0x002D0164
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

	// Token: 0x0600779A RID: 30618 RVA: 0x002D1FF8 File Offset: 0x002D01F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleUIShutdown()
	{
		this.xui.playerUI.OnUIShutdown -= this.HandleUIShutdown;
		this.xui.OnShutdown -= this.HandleUIShutdown;
		XUiM_PlayerEquipment.HandleRefreshEquipment -= this.XUiM_PlayerEquipment_HandleRefreshEquipment;
	}

	// Token: 0x0600779B RID: 30619 RVA: 0x002D2049 File Offset: 0x002D0249
	[PublicizedFrom(EAccessModifier.Private)]
	public void PreviewFrame_OnHover(XUiController _sender, bool _isOver)
	{
		this.isMouseOverPreview = _isOver;
	}

	// Token: 0x0600779C RID: 30620 RVA: 0x002D2054 File Offset: 0x002D0254
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

	// Token: 0x0600779D RID: 30621 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment(XUiM_PlayerEquipment _playerEquipment)
	{
	}

	// Token: 0x0600779E RID: 30622 RVA: 0x002D20C0 File Offset: 0x002D02C0
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

	// Token: 0x0600779F RID: 30623 RVA: 0x002D2304 File Offset: 0x002D0504
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

	// Token: 0x060077A0 RID: 30624 RVA: 0x002D28E4 File Offset: 0x002D0AE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiM_PlayerEquipment_HandleRefreshEquipment1(XUiM_PlayerEquipment playerEquipment)
	{
		if (!base.IsOpen)
		{
			return;
		}
		this.MakePreview();
	}

	// Token: 0x060077A1 RID: 30625 RVA: 0x002D28F8 File Offset: 0x002D0AF8
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

	// Token: 0x060077A2 RID: 30626 RVA: 0x002D299C File Offset: 0x002D0B9C
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

	// Token: 0x060077A3 RID: 30627 RVA: 0x002D2AA4 File Offset: 0x002D0CA4
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

	// Token: 0x060077A4 RID: 30628 RVA: 0x002D2B0D File Offset: 0x002D0D0D
	[PublicizedFrom(EAccessModifier.Private)]
	public void MaintainCursorLock()
	{
		this.xui.playerUI.CursorController.SetScreenPosition(this.lockedCursorPos);
	}

	// Token: 0x060077A5 RID: 30629 RVA: 0x002D2B2A File Offset: 0x002D0D2A
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

	// Token: 0x060077A6 RID: 30630 RVA: 0x002D2B58 File Offset: 0x002D0D58
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

	// Token: 0x0400587C RID: 22652
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x0400587D RID: 22653
	[XuiBindComponent("btnClearAll", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnClearAll;

	// Token: 0x0400587E RID: 22654
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController previewFrame;

	// Token: 0x0400587F RID: 22655
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textPreview;

	// Token: 0x04005880 RID: 22656
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer ep;

	// Token: 0x04005881 RID: 22657
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera cam;

	// Token: 0x04005882 RID: 22658
	public RuntimeAnimatorController animationController;

	// Token: 0x04005883 RID: 22659
	public float atlasResolutionScale;

	// Token: 0x04005884 RID: 22660
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RenderTextureSystem renderTextureSystem = new RenderTextureSystem();

	// Token: 0x04005885 RID: 22661
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPreviewDirty;

	// Token: 0x04005886 RID: 22662
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer player;

	// Token: 0x04005887 RID: 22663
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform characterPivot;

	// Token: 0x04005888 RID: 22664
	[PublicizedFrom(EAccessModifier.Private)]
	public const float DragRotateSpeed = 0.4f;

	// Token: 0x04005889 RID: 22665
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ControllerRotateSpeed = 200f;

	// Token: 0x0400588A RID: 22666
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMouseOverPreview;

	// Token: 0x0400588B RID: 22667
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragging;

	// Token: 0x0400588C RID: 22668
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 lastMousePos;

	// Token: 0x0400588D RID: 22669
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RotationStickDeadzone = 0.15f;

	// Token: 0x0400588E RID: 22670
	[PublicizedFrom(EAccessModifier.Private)]
	public const bool InvertRotationStick = true;

	// Token: 0x0400588F RID: 22671
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cursorLockActive;

	// Token: 0x04005890 RID: 22672
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 lockedCursorPos;

	// Token: 0x04005891 RID: 22673
	[PublicizedFrom(EAccessModifier.Private)]
	public bool prevCursorHidden;

	// Token: 0x04005892 RID: 22674
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject previewSDCSObj;

	// Token: 0x04005893 RID: 22675
	[PublicizedFrom(EAccessModifier.Private)]
	public SDCSUtils.TransformCatalog transformCatalog;

	// Token: 0x04005894 RID: 22676
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera renderCamera;

	// Token: 0x04005895 RID: 22677
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
