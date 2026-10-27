using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AD3 RID: 2771
public class SignRenderer : MonoBehaviour
{
	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x06005286 RID: 21126 RVA: 0x001F9F0F File Offset: 0x001F810F
	public Renderer Renderer
	{
		get
		{
			return this.renderer;
		}
	}

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x06005287 RID: 21127 RVA: 0x001F9F17 File Offset: 0x001F8117
	public bool IsDecal
	{
		get
		{
			return this.isDecal;
		}
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x001F9F1F File Offset: 0x001F811F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend && this.InitializeAlphaBlendDecal())
		{
			this.BuildDecalCommandBuffer();
		}
	}

	// Token: 0x06005289 RID: 21129 RVA: 0x001F9F40 File Offset: 0x001F8140
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.TeardownDecalIfNeeded();
		}
	}

	// Token: 0x0600528A RID: 21130 RVA: 0x001F9F5C File Offset: 0x001F815C
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

	// Token: 0x0600528B RID: 21131 RVA: 0x001FA003 File Offset: 0x001F8203
	[PublicizedFrom(EAccessModifier.Private)]
	public void TeardownDecalIfNeeded()
	{
		this.renderer.enabled = true;
		this.RemoveDecalCommandBuffer();
	}

	// Token: 0x0600528C RID: 21132 RVA: 0x001FA018 File Offset: 0x001F8218
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

	// Token: 0x0600528D RID: 21133 RVA: 0x001FA0D0 File Offset: 0x001F82D0
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

	// Token: 0x0600528E RID: 21134 RVA: 0x001FA11D File Offset: 0x001F831D
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRenderingOn()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.renderer.enabled = false;
		}
	}

	// Token: 0x0600528F RID: 21135 RVA: 0x001FA13C File Offset: 0x001F833C
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

	// Token: 0x0400402B RID: 16427
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshRenderer renderer;

	// Token: 0x0400402C RID: 16428
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera targetCamera;

	// Token: 0x0400402D RID: 16429
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.SignBlendMode signBlendMode;

	// Token: 0x0400402E RID: 16430
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer decalCommandBuffer;

	// Token: 0x0400402F RID: 16431
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material signMaterial;

	// Token: 0x04004030 RID: 16432
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDecal;

	// Token: 0x04004031 RID: 16433
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cAlphaBlendedSignMaterialPath = "@:Entities/Crafting/Materials/sign_decal.mat";

	// Token: 0x04004032 RID: 16434
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const CameraEvent cTargetEvent = CameraEvent.BeforeLighting;

	// Token: 0x04004033 RID: 16435
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera alphaBlendCamera;
}
