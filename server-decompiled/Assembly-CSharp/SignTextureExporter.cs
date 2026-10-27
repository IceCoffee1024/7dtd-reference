using System;
using System.IO;
using UnityEngine;

// Token: 0x02000AD5 RID: 2773
public sealed class SignTextureExporter
{
	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x06005296 RID: 21142 RVA: 0x001FA57C File Offset: 0x001F877C
	public static SignTextureExporter Instance
	{
		get
		{
			return SignTextureExporter.m_instance;
		}
	}

	// Token: 0x06005297 RID: 21143 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureExporter()
	{
	}

	// Token: 0x06005298 RID: 21144 RVA: 0x001FA584 File Offset: 0x001F8784
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

	// Token: 0x06005299 RID: 21145 RVA: 0x001FA654 File Offset: 0x001F8854
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

	// Token: 0x0600529A RID: 21146 RVA: 0x001FA6E8 File Offset: 0x001F88E8
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

	// Token: 0x04004036 RID: 16438
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureExporter m_instance = new SignTextureExporter();

	// Token: 0x04004037 RID: 16439
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kResolutionMultiplier = 4;

	// Token: 0x04004038 RID: 16440
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kWidth = 4096;

	// Token: 0x04004039 RID: 16441
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kHeight = 4096;

	// Token: 0x0400403A RID: 16442
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x0400403B RID: 16443
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader shader;

	// Token: 0x0400403C RID: 16444
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x0400403D RID: 16445
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture rt;

	// Token: 0x0400403E RID: 16446
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D readback;

	// Token: 0x0400403F RID: 16447
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;
}
