using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using WorldGenerationEngineFinal;

// Token: 0x02000E4F RID: 3663
[Preserve]
public class XUiC_WorldGenerationPreview : XUiController
{
	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x060070C4 RID: 28868 RVA: 0x002B58D3 File Offset: 0x002B3AD3
	// (set) Token: 0x060070C5 RID: 28869 RVA: 0x002B58DB File Offset: 0x002B3ADB
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

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x060070C6 RID: 28870 RVA: 0x002B58EB File Offset: 0x002B3AEB
	// (set) Token: 0x060070C7 RID: 28871 RVA: 0x002B5904 File Offset: 0x002B3B04
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

	// Token: 0x060070C8 RID: 28872 RVA: 0x002B59A2 File Offset: 0x002B3BA2
	public override void Init()
	{
		base.Init();
		this.uiPreviewTexture.Controller.OnPress += this.worldPreview_OnPress;
		this.uiPreviewTexture.Controller.OnHover += this.onHover;
	}

	// Token: 0x060070C9 RID: 28873 RVA: 0x002B59E2 File Offset: 0x002B3BE2
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onHover(XUiController _sender, bool _isOver)
	{
		this.IsHovered = _isOver;
	}

	// Token: 0x060070CA RID: 28874 RVA: 0x002B59EB File Offset: 0x002B3BEB
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldPreview_OnPress(XUiController _sender, int _button)
	{
		this.Controlling = true;
	}

	// Token: 0x060070CB RID: 28875 RVA: 0x002B59F4 File Offset: 0x002B3BF4
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

	// Token: 0x060070CC RID: 28876 RVA: 0x002B5B48 File Offset: 0x002B3D48
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

	// Token: 0x060070CD RID: 28877 RVA: 0x002B5C25 File Offset: 0x002B3E25
	public Vector3 GetCameraPosition()
	{
		return this.renderTextureSystem.CameraGO.transform.position;
	}

	// Token: 0x060070CE RID: 28878 RVA: 0x002B5C3C File Offset: 0x002B3E3C
	public override void OnOpen()
	{
		this.renderTextureInit();
		base.OnOpen();
	}

	// Token: 0x060070CF RID: 28879 RVA: 0x002B5C4A File Offset: 0x002B3E4A
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

	// Token: 0x060070D0 RID: 28880 RVA: 0x002B5C77 File Offset: 0x002B3E77
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

	// Token: 0x060070D1 RID: 28881 RVA: 0x002B5C90 File Offset: 0x002B3E90
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

	// Token: 0x060070D2 RID: 28882 RVA: 0x002B5CE8 File Offset: 0x002B3EE8
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

	// Token: 0x060070D3 RID: 28883 RVA: 0x002B5D49 File Offset: 0x002B3F49
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

	// Token: 0x060070D4 RID: 28884 RVA: 0x002B5D68 File Offset: 0x002B3F68
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

	// Token: 0x060070D5 RID: 28885 RVA: 0x002B5DA3 File Offset: 0x002B3FA3
	public void TerrainUpdate()
	{
		if (this.terrainPreviewRootObj && XUiC_WorldGenerationPreview.WorldBuilder != null)
		{
			WorldPreviewTerrain.GenerateTerrain();
		}
	}

	// Token: 0x060070D6 RID: 28886 RVA: 0x002B5DBE File Offset: 0x002B3FBE
	public void TerrainCleanup()
	{
		WorldPreviewTerrain.Cleanup();
	}

	// Token: 0x060070D7 RID: 28887 RVA: 0x002B5DC5 File Offset: 0x002B3FC5
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

	// Token: 0x060070D8 RID: 28888 RVA: 0x002B5DD8 File Offset: 0x002B3FD8
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

	// Token: 0x060070D9 RID: 28889 RVA: 0x002B5F54 File Offset: 0x002B4154
	[PublicizedFrom(EAccessModifier.Private)]
	public void renderTextureCleanup()
	{
		this.renderTextureSystem.SetEnabled(false);
		global::UnityEngine.Object.Destroy(this.renderTextureSystem.ParentGO);
		this.renderTextureSystem = null;
	}

	// Token: 0x060070DA RID: 28890 RVA: 0x002B5F7C File Offset: 0x002B417C
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

	// Token: 0x0400551B RID: 21787
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow uiWinGroup;

	// Token: 0x0400551C RID: 21788
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x0400551D RID: 21789
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject terrainPreviewRootObj;

	// Token: 0x0400551E RID: 21790
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture uiPreviewTexture;

	// Token: 0x0400551F RID: 21791
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotX;

	// Token: 0x04005520 RID: 21792
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotY;

	// Token: 0x04005521 RID: 21793
	[PublicizedFrom(EAccessModifier.Private)]
	public const float CurrentFlySpeed = 150f;

	// Token: 0x04005522 RID: 21794
	public static WorldBuilder WorldBuilder;

	// Token: 0x04005523 RID: 21795
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D previewImage;

	// Token: 0x04005524 RID: 21796
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32[] previewColors;

	// Token: 0x04005525 RID: 21797
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x02000E50 RID: 3664
	public enum PreviewStep
	{
		// Token: 0x04005527 RID: 21799
		Start,
		// Token: 0x04005528 RID: 21800
		Biome,
		// Token: 0x04005529 RID: 21801
		Terrain,
		// Token: 0x0400552A RID: 21802
		Done
	}

	// Token: 0x02000E51 RID: 3665
	public class PrefabNameHandler : MonoBehaviour
	{
		// Token: 0x060070DD RID: 28893 RVA: 0x002B6009 File Offset: 0x002B4209
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.textMesh = base.transform.GetComponent<TextMesh>();
		}

		// Token: 0x060070DE RID: 28894 RVA: 0x002B601C File Offset: 0x002B421C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMouseOver(bool _isOver)
		{
			if (this.textMesh == null)
			{
				return;
			}
			base.gameObject.layer = (_isOver ? 11 : 0);
		}

		// Token: 0x0400552B RID: 21803
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public TextMesh textMesh;
	}
}
