using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using WorldGenerationEngineFinal;

// Token: 0x02000E69 RID: 3689
[Preserve]
public class XUiC_WorldGenerationPreview : XUiController
{
	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x060070E1 RID: 28897 RVA: 0x002BA1EF File Offset: 0x002B83EF
	// (set) Token: 0x060070E2 RID: 28898 RVA: 0x002BA1F7 File Offset: 0x002B83F7
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

	// Token: 0x17000CE6 RID: 3302
	// (get) Token: 0x060070E3 RID: 28899 RVA: 0x002BA207 File Offset: 0x002B8407
	// (set) Token: 0x060070E4 RID: 28900 RVA: 0x002BA220 File Offset: 0x002B8420
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

	// Token: 0x060070E5 RID: 28901 RVA: 0x002BA2BE File Offset: 0x002B84BE
	public override void Init()
	{
		base.Init();
		this.uiPreviewTexture.Controller.OnPress += this.worldPreview_OnPress;
		this.uiPreviewTexture.Controller.OnHover += this.onHover;
	}

	// Token: 0x060070E6 RID: 28902 RVA: 0x002BA2FE File Offset: 0x002B84FE
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onHover(XUiController _sender, bool _isOver)
	{
		this.IsHovered = _isOver;
	}

	// Token: 0x060070E7 RID: 28903 RVA: 0x002BA307 File Offset: 0x002B8507
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldPreview_OnPress(XUiController _sender, int _button)
	{
		this.Controlling = true;
	}

	// Token: 0x060070E8 RID: 28904 RVA: 0x002BA310 File Offset: 0x002B8510
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

	// Token: 0x060070E9 RID: 28905 RVA: 0x002BA464 File Offset: 0x002B8664
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

	// Token: 0x060070EA RID: 28906 RVA: 0x002BA541 File Offset: 0x002B8741
	public Vector3 GetCameraPosition()
	{
		return this.renderTextureSystem.CameraGO.transform.position;
	}

	// Token: 0x060070EB RID: 28907 RVA: 0x002BA558 File Offset: 0x002B8758
	public override void OnOpen()
	{
		this.renderTextureInit();
		base.OnOpen();
	}

	// Token: 0x060070EC RID: 28908 RVA: 0x002BA566 File Offset: 0x002B8766
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

	// Token: 0x060070ED RID: 28909 RVA: 0x002BA593 File Offset: 0x002B8793
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

	// Token: 0x060070EE RID: 28910 RVA: 0x002BA5AC File Offset: 0x002B87AC
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

	// Token: 0x060070EF RID: 28911 RVA: 0x002BA604 File Offset: 0x002B8804
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

	// Token: 0x060070F0 RID: 28912 RVA: 0x002BA665 File Offset: 0x002B8865
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

	// Token: 0x060070F1 RID: 28913 RVA: 0x002BA684 File Offset: 0x002B8884
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

	// Token: 0x060070F2 RID: 28914 RVA: 0x002BA6BF File Offset: 0x002B88BF
	public void TerrainUpdate()
	{
		if (this.terrainPreviewRootObj && XUiC_WorldGenerationPreview.WorldBuilder != null)
		{
			WorldPreviewTerrain.GenerateTerrain();
		}
	}

	// Token: 0x060070F3 RID: 28915 RVA: 0x002BA6DA File Offset: 0x002B88DA
	public void TerrainCleanup()
	{
		WorldPreviewTerrain.Cleanup();
	}

	// Token: 0x060070F4 RID: 28916 RVA: 0x002BA6E1 File Offset: 0x002B88E1
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

	// Token: 0x060070F5 RID: 28917 RVA: 0x002BA6F4 File Offset: 0x002B88F4
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

	// Token: 0x060070F6 RID: 28918 RVA: 0x002BA870 File Offset: 0x002B8A70
	[PublicizedFrom(EAccessModifier.Private)]
	public void renderTextureCleanup()
	{
		this.renderTextureSystem.SetEnabled(false);
		global::UnityEngine.Object.Destroy(this.renderTextureSystem.ParentGO);
		this.renderTextureSystem = null;
	}

	// Token: 0x060070F7 RID: 28919 RVA: 0x002BA898 File Offset: 0x002B8A98
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

	// Token: 0x04005577 RID: 21879
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow uiWinGroup;

	// Token: 0x04005578 RID: 21880
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x04005579 RID: 21881
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject terrainPreviewRootObj;

	// Token: 0x0400557A RID: 21882
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture uiPreviewTexture;

	// Token: 0x0400557B RID: 21883
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotX;

	// Token: 0x0400557C RID: 21884
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotY;

	// Token: 0x0400557D RID: 21885
	[PublicizedFrom(EAccessModifier.Private)]
	public const float CurrentFlySpeed = 150f;

	// Token: 0x0400557E RID: 21886
	public static WorldBuilder WorldBuilder;

	// Token: 0x0400557F RID: 21887
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D previewImage;

	// Token: 0x04005580 RID: 21888
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32[] previewColors;

	// Token: 0x04005581 RID: 21889
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x02000E6A RID: 3690
	public enum PreviewStep
	{
		// Token: 0x04005583 RID: 21891
		Start,
		// Token: 0x04005584 RID: 21892
		Biome,
		// Token: 0x04005585 RID: 21893
		Terrain,
		// Token: 0x04005586 RID: 21894
		Done
	}

	// Token: 0x02000E6B RID: 3691
	public class PrefabNameHandler : MonoBehaviour
	{
		// Token: 0x060070FA RID: 28922 RVA: 0x002BA925 File Offset: 0x002B8B25
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.textMesh = base.transform.GetComponent<TextMesh>();
		}

		// Token: 0x060070FB RID: 28923 RVA: 0x002BA938 File Offset: 0x002B8B38
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMouseOver(bool _isOver)
		{
			if (this.textMesh == null)
			{
				return;
			}
			base.gameObject.layer = (_isOver ? 11 : 0);
		}

		// Token: 0x04005587 RID: 21895
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public TextMesh textMesh;
	}
}
