using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using WorldGenerationEngineFinal;

// Token: 0x02000E3A RID: 3642
[Preserve]
public class XUiC_WorldGenerationPreview : XUiController
{
	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x06007024 RID: 28708 RVA: 0x002B23AF File Offset: 0x002B05AF
	// (set) Token: 0x06007025 RID: 28709 RVA: 0x002B23B7 File Offset: 0x002B05B7
	[XuiXmlBinding("ishovered")]
	public bool IsHovered
	{
		get
		{
			return this.isHovered;
		}
		set
		{
			this.isHovered = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x06007026 RID: 28710 RVA: 0x002B23C7 File Offset: 0x002B05C7
	// (set) Token: 0x06007027 RID: 28711 RVA: 0x002B23E0 File Offset: 0x002B05E0
	[XuiXmlBinding("iscontrolling")]
	public bool Controlling
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui.playerUI.windowManager.IsInputLocked;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.Controlling == value)
			{
				return;
			}
			this.xui.playerUI.windowManager.IsInputLocked = value;
			this.xui.playerUI.CursorController.Locked = value;
			this.xui.playerUI.CursorController.SetCursorHidden(value);
			if (value)
			{
				PlatformManager.NativePlatform.Input.ActionSetManager.Push(this.xui.playerUI.playerInput);
			}
			else
			{
				PlatformManager.NativePlatform.Input.ActionSetManager.Pop(null);
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x06007028 RID: 28712 RVA: 0x002B247E File Offset: 0x002B067E
	public override void Init()
	{
		base.Init();
		this.uiPreviewTexture.Controller.OnPress += this.worldPreview_OnPress;
		this.uiPreviewTexture.Controller.OnHover += this.onHover;
	}

	// Token: 0x06007029 RID: 28713 RVA: 0x002B24BE File Offset: 0x002B06BE
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onHover(XUiController _sender, bool _isOver)
	{
		this.IsHovered = _isOver;
	}

	// Token: 0x0600702A RID: 28714 RVA: 0x002B24C7 File Offset: 0x002B06C7
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldPreview_OnPress(XUiController _sender, int _button)
	{
		this.Controlling = true;
	}

	// Token: 0x0600702B RID: 28715 RVA: 0x002B24D0 File Offset: 0x002B06D0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (this.renderTextureSystem == null || !this.Controlling)
		{
			return;
		}
		if (this.xui.playerUI.playerInput.Primary.WasReleased || this.xui.playerUI.playerInput.Menu.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(delegate
			{
				this.Controlling = false;
			}, 1);
			return;
		}
		Transform transform = this.renderTextureSystem.CameraGO.transform;
		if (this.xui.playerUI.playerInput.Reload.WasReleased || this.xui.playerUI.playerInput.PermanentActions.Reload.WasReleased)
		{
			this.resetCamera();
			return;
		}
		Vector3 vector = this.updateMovementUnified(_dt);
		vector *= 150f * Time.deltaTime;
		Vector3 vector2 = transform.position;
		vector2 += transform.forward * vector.z;
		vector2 += transform.right * vector.x;
		vector2 += transform.up * vector.y;
		vector2.y = Utils.FastMax(40f, vector2.y);
		transform.position = vector2;
	}

	// Token: 0x0600702C RID: 28716 RVA: 0x002B2624 File Offset: 0x002B0824
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 updateMovementUnified(float _dt)
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		Vector3 vector = Vector3.zero;
		vector = new Vector3(playerInput.Move.X, playerInput.JumpCrouch.Value - playerInput.ToggleCrouch.Value, playerInput.Move.Y);
		if (playerInput.Run.IsPressed)
		{
			vector *= 10f;
		}
		Transform transform = this.renderTextureSystem.CameraGO.transform;
		this.cameraRotX += playerInput.Look.Y * _dt * -165f;
		this.cameraRotY += playerInput.Look.X * _dt * 165f;
		transform.rotation = Quaternion.Euler(this.cameraRotX, this.cameraRotY, 0f);
		return vector;
	}

	// Token: 0x0600702D RID: 28717 RVA: 0x002B2701 File Offset: 0x002B0901
	public Vector3 GetCameraPosition()
	{
		return this.renderTextureSystem.CameraGO.transform.position;
	}

	// Token: 0x0600702E RID: 28718 RVA: 0x002B2718 File Offset: 0x002B0918
	public override void OnOpen()
	{
		this.renderTextureInit();
		base.OnOpen();
	}

	// Token: 0x0600702F RID: 28719 RVA: 0x002B2726 File Offset: 0x002B0926
	public override void OnClose()
	{
		this.Controlling = false;
		this.renderTextureCleanup();
		this.previewTextureCleanup();
		this.TerrainCleanup();
		this.poiPreviewsCleanup();
		XUiC_WorldGenerationPreview.WorldBuilder = null;
		base.OnClose();
	}

	// Token: 0x06007030 RID: 28720 RVA: 0x002B2753 File Offset: 0x002B0953
	public IEnumerator ShowPreview(XUiC_WorldGenerationPreview.PreviewStep _step)
	{
		if (this.uiWinGroup.PreviewQualityLevel == XUiC_WorldGenerationWindow.PreviewQuality.NoPreview)
		{
			yield break;
		}
		switch (_step)
		{
		case XUiC_WorldGenerationPreview.PreviewStep.Biome:
			this.PreviewInit();
			yield return XUiC_WorldGenerationPreview.WorldBuilder.SetMessage(Localization.Get("xuiRwgCreatingPreview", false, null), true, false);
			XUiC_WorldGenerationPreview.WorldBuilder.PreviewTextureUpdateBiomes(this.previewColors);
			XUiC_WorldGenerationPreview.previewImage.SetPixels32(this.previewColors);
			XUiC_WorldGenerationPreview.previewImage.Apply(true);
			WorldPreviewTerrain.SetTexture(XUiC_WorldGenerationPreview.previewImage);
			break;
		case XUiC_WorldGenerationPreview.PreviewStep.Terrain:
			this.TerrainUpdate();
			break;
		case XUiC_WorldGenerationPreview.PreviewStep.Done:
		{
			MicroStopwatch ms = new MicroStopwatch(true);
			yield return XUiC_WorldGenerationPreview.WorldBuilder.PreviewTextureUpdateFinal(this.previewColors);
			Log.Out("CreatePreviewTexture in {0}", new object[] { (float)ms.ElapsedMilliseconds * 0.001f });
			XUiC_WorldGenerationPreview.previewImage.SetPixels32(this.previewColors);
			XUiC_WorldGenerationPreview.previewImage.Apply(true);
			WorldPreviewTerrain.SetTexture(XUiC_WorldGenerationPreview.previewImage);
			this.TerrainUpdate();
			break;
		}
		}
		yield break;
	}

	// Token: 0x06007031 RID: 28721 RVA: 0x002B276C File Offset: 0x002B096C
	public void PreviewInit()
	{
		this.previewTextureInit();
		this.TerrainCleanup();
		if (!this.terrainPreviewRootObj || XUiC_WorldGenerationPreview.WorldBuilder == null)
		{
			return;
		}
		WorldPreviewTerrain.Init(this.uiWinGroup.WorldBuilder, this.terrainPreviewRootObj.transform);
		this.TerrainUpdate();
		this.resetCamera();
	}

	// Token: 0x06007032 RID: 28722 RVA: 0x002B27C4 File Offset: 0x002B09C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void previewTextureInit()
	{
		this.previewTextureCleanup();
		if (this.uiWinGroup.PreviewQualityLevel == XUiC_WorldGenerationWindow.PreviewQuality.NoPreview)
		{
			return;
		}
		int worldSize = this.uiWinGroup.WorldSize;
		int num = worldSize * worldSize;
		if (this.previewColors == null || this.previewColors.Length != num)
		{
			this.previewColors = new Color32[num];
		}
		XUiC_WorldGenerationPreview.previewImage = new Texture2D(worldSize, worldSize)
		{
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x06007033 RID: 28723 RVA: 0x002B2825 File Offset: 0x002B0A25
	[PublicizedFrom(EAccessModifier.Private)]
	public void previewTextureCleanup()
	{
		if (!XUiC_WorldGenerationPreview.previewImage)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(XUiC_WorldGenerationPreview.previewImage);
		XUiC_WorldGenerationPreview.previewImage = null;
	}

	// Token: 0x06007034 RID: 28724 RVA: 0x002B2844 File Offset: 0x002B0A44
	public void PreviewTextureDraw()
	{
		if (!XUiC_WorldGenerationPreview.previewImage || XUiC_WorldGenerationPreview.WorldBuilder == null)
		{
			return;
		}
		XUiC_WorldGenerationPreview.previewImage.SetPixels32(this.previewColors);
		XUiC_WorldGenerationPreview.previewImage.Apply(true);
		WorldPreviewTerrain.SetTexture(XUiC_WorldGenerationPreview.previewImage);
	}

	// Token: 0x06007035 RID: 28725 RVA: 0x002B287F File Offset: 0x002B0A7F
	public void TerrainUpdate()
	{
		if (this.terrainPreviewRootObj && XUiC_WorldGenerationPreview.WorldBuilder != null)
		{
			WorldPreviewTerrain.GenerateTerrain();
		}
	}

	// Token: 0x06007036 RID: 28726 RVA: 0x002B289A File Offset: 0x002B0A9A
	public void TerrainCleanup()
	{
		WorldPreviewTerrain.Cleanup();
	}

	// Token: 0x06007037 RID: 28727 RVA: 0x002B28A1 File Offset: 0x002B0AA1
	[PublicizedFrom(EAccessModifier.Private)]
	public void poiPreviewsCleanup()
	{
		XUiC_WorldGenerationWindow xuiC_WorldGenerationWindow = this.uiWinGroup;
		if (xuiC_WorldGenerationWindow == null)
		{
			return;
		}
		xuiC_WorldGenerationWindow.CleanupPreviewManager();
	}

	// Token: 0x06007038 RID: 28728 RVA: 0x002B28B4 File Offset: 0x002B0AB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void renderTextureInit()
	{
		if (this.renderTextureSystem == null)
		{
			this.terrainPreviewRootObj = new GameObject("TerrainMesh");
			this.renderTextureSystem = new RenderTextureSystem();
			this.renderTextureSystem.Create("worldpreview", this.terrainPreviewRootObj, new Vector3(0f, 0f, 0f), new Vector3(0f, 4000f, 0f), this.uiPreviewTexture.Size, false, false, 1f);
			Camera component = this.renderTextureSystem.CameraGO.transform.GetComponent<Camera>();
			component.nearClipPlane = 0.1f;
			component.farClipPlane = 20000f;
			this.resetCamera();
			Transform transform = this.renderTextureSystem.LightGO.transform;
			transform.localPosition = new Vector3(0f, 2000f, 0f);
			transform.localRotation = Quaternion.Euler(30f, 0f, 0f);
			transform.GetComponent<Light>().type = LightType.Directional;
			transform.GetComponent<Light>().intensity = 1.2f;
			this.terrainPreviewRootObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.terrainPreviewRootObj.transform.position = new Vector3(-10240f, 0f, -10240f);
			this.uiPreviewTexture.Texture = this.renderTextureSystem.RenderTex;
		}
		this.renderTextureSystem.SetEnabled(true);
	}

	// Token: 0x06007039 RID: 28729 RVA: 0x002B2A30 File Offset: 0x002B0C30
	[PublicizedFrom(EAccessModifier.Private)]
	public void renderTextureCleanup()
	{
		this.renderTextureSystem.SetEnabled(false);
		global::UnityEngine.Object.Destroy(this.renderTextureSystem.ParentGO);
		this.renderTextureSystem = null;
	}

	// Token: 0x0600703A RID: 28730 RVA: 0x002B2A58 File Offset: 0x002B0C58
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetCamera()
	{
		if (this.renderTextureSystem == null || XUiC_WorldGenerationPreview.WorldBuilder == null)
		{
			return;
		}
		this.cameraRotX = 90f;
		this.cameraRotY = 0f;
		Transform transform = this.renderTextureSystem.CameraGO.transform;
		transform.localPosition = new Vector3(0f, (float)XUiC_WorldGenerationPreview.WorldBuilder.WorldSize * 0.8745f, 0f);
		transform.localRotation = Quaternion.Euler(this.cameraRotX, this.cameraRotY, 0f);
	}

	// Token: 0x04005477 RID: 21623
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow uiWinGroup;

	// Token: 0x04005478 RID: 21624
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x04005479 RID: 21625
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject terrainPreviewRootObj;

	// Token: 0x0400547A RID: 21626
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture uiPreviewTexture;

	// Token: 0x0400547B RID: 21627
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotX;

	// Token: 0x0400547C RID: 21628
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotY;

	// Token: 0x0400547D RID: 21629
	[PublicizedFrom(EAccessModifier.Private)]
	public const float CurrentFlySpeed = 150f;

	// Token: 0x0400547E RID: 21630
	public static WorldBuilder WorldBuilder;

	// Token: 0x0400547F RID: 21631
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D previewImage;

	// Token: 0x04005480 RID: 21632
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32[] previewColors;

	// Token: 0x04005481 RID: 21633
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x02000E3B RID: 3643
	public enum PreviewStep
	{
		// Token: 0x04005483 RID: 21635
		Start,
		// Token: 0x04005484 RID: 21636
		Biome,
		// Token: 0x04005485 RID: 21637
		Terrain,
		// Token: 0x04005486 RID: 21638
		Done
	}

	// Token: 0x02000E3C RID: 3644
	public class PrefabNameHandler : MonoBehaviour
	{
		// Token: 0x0600703D RID: 28733 RVA: 0x002B2AE5 File Offset: 0x002B0CE5
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.textMesh = base.transform.GetComponent<TextMesh>();
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x002B2AF8 File Offset: 0x002B0CF8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMouseOver(bool _isOver)
		{
			if (this.textMesh == null)
			{
				return;
			}
			base.gameObject.layer = (_isOver ? 11 : 0);
		}

		// Token: 0x04005487 RID: 21639
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public TextMesh textMesh;
	}
}
