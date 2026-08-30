using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AB8 RID: 2744
public class SignRenderer : MonoBehaviour
{
	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x0600524E RID: 21070 RVA: 0x001F5FF7 File Offset: 0x001F41F7
	public Renderer Renderer
	{
		get
		{
			return this.renderer;
		}
	}

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x0600524F RID: 21071 RVA: 0x001F5FFF File Offset: 0x001F41FF
	public bool IsDecal
	{
		get
		{
			return this.isDecal;
		}
	}

	// Token: 0x06005250 RID: 21072 RVA: 0x001F6007 File Offset: 0x001F4207
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend && this.InitializeAlphaBlendDecal())
		{
			this.BuildDecalCommandBuffer();
		}
	}

	// Token: 0x06005251 RID: 21073 RVA: 0x001F6028 File Offset: 0x001F4228
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.TeardownDecalIfNeeded();
		}
	}

	// Token: 0x06005252 RID: 21074 RVA: 0x001F6044 File Offset: 0x001F4244
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

	// Token: 0x06005253 RID: 21075 RVA: 0x001F60EB File Offset: 0x001F42EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void TeardownDecalIfNeeded()
	{
		this.renderer.enabled = true;
		this.RemoveDecalCommandBuffer();
	}

	// Token: 0x06005254 RID: 21076 RVA: 0x001F6100 File Offset: 0x001F4300
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

	// Token: 0x06005255 RID: 21077 RVA: 0x001F61B8 File Offset: 0x001F43B8
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

	// Token: 0x06005256 RID: 21078 RVA: 0x001F6205 File Offset: 0x001F4405
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRenderingOn()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.renderer.enabled = false;
		}
	}

	// Token: 0x06005257 RID: 21079 RVA: 0x001F6224 File Offset: 0x001F4424
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

	// Token: 0x04003FCF RID: 16335
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshRenderer renderer;

	// Token: 0x04003FD0 RID: 16336
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera targetCamera;

	// Token: 0x04003FD1 RID: 16337
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.SignBlendMode signBlendMode;

	// Token: 0x04003FD2 RID: 16338
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer decalCommandBuffer;

	// Token: 0x04003FD3 RID: 16339
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material signMaterial;

	// Token: 0x04003FD4 RID: 16340
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDecal;

	// Token: 0x04003FD5 RID: 16341
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cAlphaBlendedSignMaterialPath = "@:Entities/Crafting/Materials/sign_decal.mat";

	// Token: 0x04003FD6 RID: 16342
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const CameraEvent cTargetEvent = CameraEvent.BeforeLighting;

	// Token: 0x04003FD7 RID: 16343
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera alphaBlendCamera;
}
