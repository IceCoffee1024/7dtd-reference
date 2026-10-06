using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AC9 RID: 2761
public class SignRenderer : MonoBehaviour
{
	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x06005262 RID: 21090 RVA: 0x001F8E33 File Offset: 0x001F7033
	public Renderer Renderer
	{
		get
		{
			return this.renderer;
		}
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x06005263 RID: 21091 RVA: 0x001F8E3B File Offset: 0x001F703B
	public bool IsDecal
	{
		get
		{
			return this.isDecal;
		}
	}

	// Token: 0x06005264 RID: 21092 RVA: 0x001F8E43 File Offset: 0x001F7043
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend && this.InitializeAlphaBlendDecal())
		{
			this.BuildDecalCommandBuffer();
		}
	}

	// Token: 0x06005265 RID: 21093 RVA: 0x001F8E64 File Offset: 0x001F7064
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.TeardownDecalIfNeeded();
		}
	}

	// Token: 0x06005266 RID: 21094 RVA: 0x001F8E80 File Offset: 0x001F7080
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

	// Token: 0x06005267 RID: 21095 RVA: 0x001F8F27 File Offset: 0x001F7127
	[PublicizedFrom(EAccessModifier.Private)]
	public void TeardownDecalIfNeeded()
	{
		this.renderer.enabled = true;
		this.RemoveDecalCommandBuffer();
	}

	// Token: 0x06005268 RID: 21096 RVA: 0x001F8F3C File Offset: 0x001F713C
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

	// Token: 0x06005269 RID: 21097 RVA: 0x001F8FF4 File Offset: 0x001F71F4
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

	// Token: 0x0600526A RID: 21098 RVA: 0x001F9041 File Offset: 0x001F7241
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRenderingOn()
	{
		if (this.isDecal && this.signBlendMode == SignCanvas.SignBlendMode.AlphaBlend)
		{
			this.renderer.enabled = false;
		}
	}

	// Token: 0x0600526B RID: 21099 RVA: 0x001F9060 File Offset: 0x001F7260
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

	// Token: 0x0400400A RID: 16394
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshRenderer renderer;

	// Token: 0x0400400B RID: 16395
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera targetCamera;

	// Token: 0x0400400C RID: 16396
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SignCanvas.SignBlendMode signBlendMode;

	// Token: 0x0400400D RID: 16397
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer decalCommandBuffer;

	// Token: 0x0400400E RID: 16398
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material signMaterial;

	// Token: 0x0400400F RID: 16399
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDecal;

	// Token: 0x04004010 RID: 16400
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cAlphaBlendedSignMaterialPath = "@:Entities/Crafting/Materials/sign_decal.mat";

	// Token: 0x04004011 RID: 16401
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const CameraEvent cTargetEvent = CameraEvent.BeforeLighting;

	// Token: 0x04004012 RID: 16402
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera alphaBlendCamera;
}
