using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using WorldGenerationEngineFinal;

// Token: 0x02000E5E RID: 3678
[Preserve]
public class XUiC_WorldGenerationPreview : XUiController
{
	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x060070BC RID: 28860 RVA: 0x002B8C83 File Offset: 0x002B6E83
	// (set) Token: 0x060070BD RID: 28861 RVA: 0x002B8C8B File Offset: 0x002B6E8B
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

	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x060070BE RID: 28862 RVA: 0x002B8C9B File Offset: 0x002B6E9B
	// (set) Token: 0x060070BF RID: 28863 RVA: 0x002B8CB4 File Offset: 0x002B6EB4
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

	// Token: 0x060070C0 RID: 28864 RVA: 0x002B8D52 File Offset: 0x002B6F52
	public override void Init()
	{
		base.Init();
		this.uiPreviewTexture.Controller.OnPress += this.worldPreview_OnPress;
		this.uiPreviewTexture.Controller.OnHover += this.onHover;
	}

	// Token: 0x060070C1 RID: 28865 RVA: 0x002B8D92 File Offset: 0x002B6F92
	[XuiBindEvent("OnHover", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onHover(XUiController _sender, bool _isOver)
	{
		this.IsHovered = _isOver;
	}

	// Token: 0x060070C2 RID: 28866 RVA: 0x002B8D9B File Offset: 0x002B6F9B
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldPreview_OnPress(XUiController _sender, int _button)
	{
		this.Controlling = true;
	}

	// Token: 0x060070C3 RID: 28867 RVA: 0x002B8DA4 File Offset: 0x002B6FA4
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

	// Token: 0x060070C4 RID: 28868 RVA: 0x002B8EF8 File Offset: 0x002B70F8
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

	// Token: 0x060070C5 RID: 28869 RVA: 0x002B8FD5 File Offset: 0x002B71D5
	public Vector3 GetCameraPosition()
	{
		return this.renderTextureSystem.CameraGO.transform.position;
	}

	// Token: 0x060070C6 RID: 28870 RVA: 0x002B8FEC File Offset: 0x002B71EC
	public override void OnOpen()
	{
		this.renderTextureInit();
		base.OnOpen();
	}

	// Token: 0x060070C7 RID: 28871 RVA: 0x002B8FFA File Offset: 0x002B71FA
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

	// Token: 0x060070C8 RID: 28872 RVA: 0x002B9027 File Offset: 0x002B7227
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

	// Token: 0x060070C9 RID: 28873 RVA: 0x002B9040 File Offset: 0x002B7240
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

	// Token: 0x060070CA RID: 28874 RVA: 0x002B9098 File Offset: 0x002B7298
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

	// Token: 0x060070CB RID: 28875 RVA: 0x002B90F9 File Offset: 0x002B72F9
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

	// Token: 0x060070CC RID: 28876 RVA: 0x002B9118 File Offset: 0x002B7318
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

	// Token: 0x060070CD RID: 28877 RVA: 0x002B9153 File Offset: 0x002B7353
	public void TerrainUpdate()
	{
		if (this.terrainPreviewRootObj && XUiC_WorldGenerationPreview.WorldBuilder != null)
		{
			WorldPreviewTerrain.GenerateTerrain();
		}
	}

	// Token: 0x060070CE RID: 28878 RVA: 0x002B916E File Offset: 0x002B736E
	public void TerrainCleanup()
	{
		WorldPreviewTerrain.Cleanup();
	}

	// Token: 0x060070CF RID: 28879 RVA: 0x002B9175 File Offset: 0x002B7375
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

	// Token: 0x060070D0 RID: 28880 RVA: 0x002B9188 File Offset: 0x002B7388
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

	// Token: 0x060070D1 RID: 28881 RVA: 0x002B9304 File Offset: 0x002B7504
	[PublicizedFrom(EAccessModifier.Private)]
	public void renderTextureCleanup()
	{
		this.renderTextureSystem.SetEnabled(false);
		global::UnityEngine.Object.Destroy(this.renderTextureSystem.ParentGO);
		this.renderTextureSystem = null;
	}

	// Token: 0x060070D2 RID: 28882 RVA: 0x002B932C File Offset: 0x002B752C
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

	// Token: 0x04005552 RID: 21842
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow uiWinGroup;

	// Token: 0x04005553 RID: 21843
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTextureSystem renderTextureSystem;

	// Token: 0x04005554 RID: 21844
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject terrainPreviewRootObj;

	// Token: 0x04005555 RID: 21845
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture uiPreviewTexture;

	// Token: 0x04005556 RID: 21846
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotX;

	// Token: 0x04005557 RID: 21847
	[PublicizedFrom(EAccessModifier.Private)]
	public float cameraRotY;

	// Token: 0x04005558 RID: 21848
	[PublicizedFrom(EAccessModifier.Private)]
	public const float CurrentFlySpeed = 150f;

	// Token: 0x04005559 RID: 21849
	public static WorldBuilder WorldBuilder;

	// Token: 0x0400555A RID: 21850
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D previewImage;

	// Token: 0x0400555B RID: 21851
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32[] previewColors;

	// Token: 0x0400555C RID: 21852
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x02000E5F RID: 3679
	public enum PreviewStep
	{
		// Token: 0x0400555E RID: 21854
		Start,
		// Token: 0x0400555F RID: 21855
		Biome,
		// Token: 0x04005560 RID: 21856
		Terrain,
		// Token: 0x04005561 RID: 21857
		Done
	}

	// Token: 0x02000E60 RID: 3680
	public class PrefabNameHandler : MonoBehaviour
	{
		// Token: 0x060070D5 RID: 28885 RVA: 0x002B93B9 File Offset: 0x002B75B9
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.textMesh = base.transform.GetComponent<TextMesh>();
		}

		// Token: 0x060070D6 RID: 28886 RVA: 0x002B93CC File Offset: 0x002B75CC
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMouseOver(bool _isOver)
		{
			if (this.textMesh == null)
			{
				return;
			}
			base.gameObject.layer = (_isOver ? 11 : 0);
		}

		// Token: 0x04005562 RID: 21858
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public TextMesh textMesh;
	}
}
