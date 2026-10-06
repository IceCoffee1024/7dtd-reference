using System;
using System.IO;
using UnityEngine;

// Token: 0x02000ACB RID: 2763
public sealed class SignTextureExporter
{
	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06005272 RID: 21106 RVA: 0x001F94A0 File Offset: 0x001F76A0
	public static SignTextureExporter Instance
	{
		get
		{
			return SignTextureExporter.m_instance;
		}
	}

	// Token: 0x06005273 RID: 21107 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureExporter()
	{
	}

	// Token: 0x06005274 RID: 21108 RVA: 0x001F94A8 File Offset: 0x001F76A8
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

	// Token: 0x06005275 RID: 21109 RVA: 0x001F9578 File Offset: 0x001F7778
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

	// Token: 0x06005276 RID: 21110 RVA: 0x001F960C File Offset: 0x001F780C
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

	// Token: 0x04004015 RID: 16405
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureExporter m_instance = new SignTextureExporter();

	// Token: 0x04004016 RID: 16406
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kResolutionMultiplier = 4;

	// Token: 0x04004017 RID: 16407
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kWidth = 4096;

	// Token: 0x04004018 RID: 16408
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kHeight = 4096;

	// Token: 0x04004019 RID: 16409
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x0400401A RID: 16410
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader shader;

	// Token: 0x0400401B RID: 16411
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x0400401C RID: 16412
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture rt;

	// Token: 0x0400401D RID: 16413
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D readback;

	// Token: 0x0400401E RID: 16414
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;
}
