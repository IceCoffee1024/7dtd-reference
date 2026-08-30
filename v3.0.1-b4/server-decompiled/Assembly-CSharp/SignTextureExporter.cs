using System;
using System.IO;
using UnityEngine;

// Token: 0x02000AA8 RID: 2728
public sealed class SignTextureExporter
{
	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x060051D3 RID: 20947 RVA: 0x001F37DE File Offset: 0x001F19DE
	public static SignTextureExporter Instance
	{
		get
		{
			return SignTextureExporter.m_instance;
		}
	}

	// Token: 0x060051D4 RID: 20948 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SignTextureExporter()
	{
	}

	// Token: 0x060051D5 RID: 20949 RVA: 0x001F37E8 File Offset: 0x001F19E8
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

	// Token: 0x060051D6 RID: 20950 RVA: 0x001F38B8 File Offset: 0x001F1AB8
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

	// Token: 0x060051D7 RID: 20951 RVA: 0x001F394C File Offset: 0x001F1B4C
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

	// Token: 0x04003F47 RID: 16199
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SignTextureExporter m_instance = new SignTextureExporter();

	// Token: 0x04003F48 RID: 16200
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kResolutionMultiplier = 4;

	// Token: 0x04003F49 RID: 16201
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kWidth = 4096;

	// Token: 0x04003F4A RID: 16202
	[PublicizedFrom(EAccessModifier.Private)]
	public const int kHeight = 4096;

	// Token: 0x04003F4B RID: 16203
	[PublicizedFrom(EAccessModifier.Private)]
	public const string kShaderName = "Game/SignTech/UI";

	// Token: 0x04003F4C RID: 16204
	[PublicizedFrom(EAccessModifier.Private)]
	public Shader shader;

	// Token: 0x04003F4D RID: 16205
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x04003F4E RID: 16206
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture rt;

	// Token: 0x04003F4F RID: 16207
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D readback;

	// Token: 0x04003F50 RID: 16208
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;
}
