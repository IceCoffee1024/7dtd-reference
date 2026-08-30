using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AA7 RID: 2727
public class SignRenderer : MonoBehaviour
{
	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x060051C8 RID: 20936 RVA: 0x001F355F File Offset: 0x001F175F
	public Renderer Renderer
	{
		get
		{
			return this.renderer;
		}
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x060051C9 RID: 20937 RVA: 0x001F3567 File Offset: 0x001F1767
	public bool IsDecal
	{
		get
		{
			return this.isDecal;
		}
	}

	// Token: 0x060051CA RID: 20938 RVA: 0x001F356F File Offset: 0x001F176F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend && this.InitializeAlphaBlendDecal())
		{
			this.BuildDecalCommandBuffer();
		}
	}

	// Token: 0x060051CB RID: 20939 RVA: 0x001F3590 File Offset: 0x001F1790
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.TeardownDecalIfNeeded();
		}
	}

	// Token: 0x060051CC RID: 20940 RVA: 0x001F35AC File Offset: 0x001F17AC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool InitializeAlphaBlendDecal()
	{
		Camera camera;
		if (!(this.alphaBlendCamera == null))
		{
			camera = this.alphaBlendCamera;
		}
		else
		{
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			camera = ((primaryPlayer != null) ? primaryPlayer.playerCamera : null);
		}
		this.targetCamera = camera;
		if (this.signMaterial == null)
		{
			this.signMaterial = DataLoader.LoadAsset<Material>("@:Entities/Crafting/Materials/sign_decal.mat", false);
		}
		if (this.targetCamera == null || this.renderer == null || this.signMaterial == null)
		{
			Debug.LogError("[SignRenderer] Alpha blended rendering mode enabled but references are missing.", this);
			return false;
		}
		this.renderer.enabled = false;
		return true;
	}

	// Token: 0x060051CD RID: 20941 RVA: 0x001F3653 File Offset: 0x001F1853
	[PublicizedFrom(EAccessModifier.Private)]
	public void TeardownDecalIfNeeded()
	{
		this.renderer.enabled = true;
		this.RemoveDecalCommandBuffer();
	}

	// Token: 0x060051CE RID: 20942 RVA: 0x001F3668 File Offset: 0x001F1868
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildDecalCommandBuffer()
	{
		this.decalCommandBuffer = new CommandBuffer
		{
			name = "SignAlphaBlendedDecal: " + base.gameObject.name
		};
		RenderTargetIdentifier[] array = new RenderTargetIdentifier[]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.GBuffer1,
			BuiltinRenderTextureType.CameraTarget
		};
		this.decalCommandBuffer.SetRenderTarget(array, BuiltinRenderTextureType.CameraTarget);
		this.decalCommandBuffer.SetGlobalTexture("_DecalNormalBuffer", BuiltinRenderTextureType.GBuffer2);
		this.decalCommandBuffer.DrawRenderer(this.renderer, this.signMaterial, 0, 0);
		this.targetCamera.AddCommandBuffer(CameraEvent.BeforeLighting, this.decalCommandBuffer);
	}

	// Token: 0x060051CF RID: 20943 RVA: 0x001F3720 File Offset: 0x001F1920
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveDecalCommandBuffer()
	{
		if (this.targetCamera != null && this.decalCommandBuffer != null)
		{
			this.targetCamera.RemoveCommandBuffer(CameraEvent.BeforeLighting, this.decalCommandBuffer);
		}
		CommandBuffer commandBuffer = this.decalCommandBuffer;
		if (commandBuffer != null)
		{
			commandBuffer.Dispose();
		}
		this.decalCommandBuffer = null;
	}

	// Token: 0x060051D0 RID: 20944 RVA: 0x001F376D File Offset: 0x001F196D
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRenderingOn()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.renderer.enabled = false;
		}
	}

	// Token: 0x060051D1 RID: 20945 RVA: 0x001F378C File Offset: 0x001F198C
	public void SetRenderParameters(MaterialPropertyBlock mpb, SignCanvas.SignBlendMode blendMode = SignCanvas.SignBlendMode.Cutout, Camera alphaBlendCamera = null)
	{
		this.renderer.SetPropertyBlock(mpb);
		if (this.IsDecal)
		{
			this.signBlendMode = blendMode;
			this.alphaBlendCamera = alphaBlendCamera;
			if (blendMode == SignCanvas.SignBlendMode.Cutout)
			{
				this.TeardownDecalIfNeeded();
				return;
			}
			if (blendMode != SignCanvas.SignBlendMode.AlphaBlend)
			{
				return;
			}
			if (this.InitializeAlphaBlendDecal())
			{
				this.RemoveDecalCommandBuffer();
				this.BuildDecalCommandBuffer();
			}
		}
	}

	// Token: 0x04003F3E RID: 16190
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshRenderer renderer;

	// Token: 0x04003F3F RID: 16191
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera targetCamera;

	// Token: 0x04003F40 RID: 16192
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.SignBlendMode signBlendMode;

	// Token: 0x04003F41 RID: 16193
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer decalCommandBuffer;

	// Token: 0x04003F42 RID: 16194
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material signMaterial;

	// Token: 0x04003F43 RID: 16195
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDecal;

	// Token: 0x04003F44 RID: 16196
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cAlphaBlendedSignMaterialPath = "@:Entities/Crafting/Materials/sign_decal.mat";

	// Token: 0x04003F45 RID: 16197
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const CameraEvent cTargetEvent = CameraEvent.BeforeLighting;

	// Token: 0x04003F46 RID: 16198
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera alphaBlendCamera;
}
