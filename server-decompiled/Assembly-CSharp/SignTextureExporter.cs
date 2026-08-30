using System;
using System.IO;
using UnityEngine;

// Token: 0x02000AB9 RID: 2745
public sealed class SignTextureExporter
{
	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x06005259 RID: 21081 RVA: 0x001F6276 File Offset: 0x001F4476
	public static SignTextureExporter Instance
	{
		get
		{
			return SignTextureExporter.m_instance;
		}
	}

	// Token: 0x0600525A RID: 21082 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureExporter()
	{
	}

	// Token: 0x0600525B RID: 21083 RVA: 0x001F6280 File Offset: 0x001F4480
	[PublicizedFrom(EAccessModifier.Private)]
	public void Initialize()
	{
		this.shader = GlobalAssets.FindShader("Game/SignTech/UI");
		if (this.shader == null)
		{
			throw new InvalidOperationException("Shader not found: Game/SignTech/UI");
		}
		this.material = new Material(this.shader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		this.rt = new RenderTexture(4096, 4096, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
		{
			name = "SignTextureExporter_RT",
			useMipMap = false,
			autoGenerateMips = false,
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Bilinear,
			antiAliasing = 1
		};
		this.rt.Create();
		this.readback = new Texture2D(4096, 4096, TextureFormat.RGBA32, false, false);
		this.readback.name = "SignTextureExporter_Readback";
		this.initialized = true;
	}

	// Token: 0x0600525C RID: 21084 RVA: 0x001F6350 File Offset: 0x001F4550
	public void Cleanup()
	{
		this.initialized = false;
		if (this.material != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.material);
		}
		if (this.rt != null)
		{
			if (this.rt.IsCreated())
			{
				this.rt.Release();
			}
			global::UnityEngine.Object.DestroyImmediate(this.rt);
		}
		if (this.readback != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.readback);
		}
		this.shader = null;
		this.material = null;
		this.rt = null;
		this.readback = null;
	}

	// Token: 0x0600525D RID: 21085 RVA: 0x001F63E4 File Offset: 0x001F45E4
	public string ExportSignToTexture(string signName, GlobalSignId signId)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		SignDataManager.Instance.TryApplyRenderingData(signId, 1f, this.material, null, SignUIStyle.Standard);
		RenderTexture active = RenderTexture.active;
		string text3;
		try
		{
			RenderTexture.active = this.rt;
			GL.Clear(true, true, Color.clear);
			Graphics.Blit(null, this.rt, this.material);
			this.readback.ReadPixels(new Rect(0f, 0f, 4096f, 4096f), 0, 0, false);
			this.readback.Apply(false, false);
			byte[] array = this.readback.EncodeToPNG();
			if (string.IsNullOrEmpty(signName))
			{
				signName = "Unnamed Sign";
			}
			string text = string.Format("{0}_{1:yyyyMMdd_HHmmss}.png", signName, DateTime.Now);
			string text2 = Path.Combine(Application.dataPath + "/../", text);
			File.WriteAllBytes(text2, array);
			text3 = text2;
		}
		finally
		{
			RenderTexture.active = active;
		}
		return text3;
	}

	// Token: 0x04003FD8 RID: 16344
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureExporter m_instance = new SignTextureExporter();

	// Token: 0x04003FD9 RID: 16345
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kResolutionMultiplier = 4;

	// Token: 0x04003FDA RID: 16346
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kWidth = 4096;

	// Token: 0x04003FDB RID: 16347
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kHeight = 4096;

	// Token: 0x04003FDC RID: 16348
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x04003FDD RID: 16349
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader shader;

	// Token: 0x04003FDE RID: 16350
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x04003FDF RID: 16351
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture rt;

	// Token: 0x04003FE0 RID: 16352
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D readback;

	// Token: 0x04003FE1 RID: 16353
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;
}
