using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x02001328 RID: 4904
public static class TintableMaterialBaker
{
	// Token: 0x17001245 RID: 4677
	// (get) Token: 0x06009ACE RID: 39630 RVA: 0x003A79A6 File Offset: 0x003A5BA6
	public static Shader BakerShader
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (TintableMaterialBaker._bakerShader == null)
			{
				TintableMaterialBaker._bakerShader = Shader.Find("Hidden/AlbedoBaker");
			}
			return TintableMaterialBaker._bakerShader;
		}
	}

	// Token: 0x17001246 RID: 4678
	// (get) Token: 0x06009ACF RID: 39631 RVA: 0x003A79C9 File Offset: 0x003A5BC9
	public static Material BakerMaterial
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (TintableMaterialBaker._bakerMaterial == null && TintableMaterialBaker.BakerShader != null)
			{
				TintableMaterialBaker._bakerMaterial = new Material(TintableMaterialBaker.BakerShader);
			}
			return TintableMaterialBaker._bakerMaterial;
		}
	}

	// Token: 0x06009AD0 RID: 39632 RVA: 0x003A79FC File Offset: 0x003A5BFC
	public static int ProcessGameObject(GameObject gameObject)
	{
		if (gameObject == null)
		{
			Debug.LogWarning("GameObject is null, nothing to process");
			return 0;
		}
		if (TintableMaterialBaker.BakerMaterial == null)
		{
			Debug.LogError("Baker shader 'Hidden/AlbedoBaker' not found! Make sure the AlbedoBaker.shader is in your project.");
			return 0;
		}
		Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
		int num = 0;
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer == null))
			{
				Material[] materials = renderer.materials;
				bool flag = false;
				for (int j = 0; j < materials.Length; j++)
				{
					Material material = materials[j];
					if (TintableMaterialBaker.IsTintableMaterial(material))
					{
						Material material2 = new Material(material);
						if (TintableMaterialBaker.BakeMaterial(material2))
						{
							materials[j] = material2;
							flag = true;
							num++;
						}
					}
				}
				if (flag)
				{
					renderer.materials = materials;
				}
			}
		}
		return num;
	}

	// Token: 0x06009AD1 RID: 39633 RVA: 0x003A7AB8 File Offset: 0x003A5CB8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool IsTintableMaterial(Material material)
	{
		return !(material == null) && !(material.shader == null) && (material.shader.name.Contains("_Tintable") && material.HasProperty("_Albedo") && material.HasProperty("_IndexTex") && material.HasProperty("_TintMask")) && material.HasProperty("_PaletteTex");
	}

	// Token: 0x06009AD2 RID: 39634 RVA: 0x003A7B2C File Offset: 0x003A5D2C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool BakeMaterial(Material material)
	{
		Shader nonTintableShader = TintableMaterialBaker.GetNonTintableShader(material);
		if (nonTintableShader == null)
		{
			return false;
		}
		Texture texture = material.GetTexture("_Albedo");
		Texture texture2 = material.GetTexture("_IndexTex");
		Texture texture3 = material.GetTexture("_TintMask");
		Texture texture4 = material.GetTexture("_PaletteTex");
		if (texture == null)
		{
			Debug.LogWarning("Material " + material.name + " is missing Albedo texture, skipping");
			return false;
		}
		if (texture2 == null)
		{
			Debug.LogWarning("Material " + material.name + " is missing Index texture, skipping");
			return false;
		}
		if (texture4 == null)
		{
			Debug.LogWarning("Material " + material.name + " is missing Palette texture, skipping");
			return false;
		}
		Texture2D texture2D = TintableMaterialBaker.BakeAlbedoWithTintsGPU(material, texture, texture2, texture3, texture4);
		if (texture2D == null)
		{
			Debug.LogError("Failed to bake texture for material " + material.name);
			return false;
		}
		material.shader = nonTintableShader;
		material.SetTexture("_Albedo", texture2D);
		return true;
	}

	// Token: 0x06009AD3 RID: 39635 RVA: 0x003A7C30 File Offset: 0x003A5E30
	[PublicizedFrom(EAccessModifier.Private)]
	public static Shader GetNonTintableShader(Material material)
	{
		if (material == null || material.shader == null)
		{
			return null;
		}
		string name = material.shader.name;
		string text = Regex.Replace(name, "_Tintable", "", RegexOptions.IgnoreCase);
		if (text == name)
		{
			Debug.LogWarning(string.Concat(new string[] { "Material ", material.name, " shader '", name, "' doesn't contain '_Tintable'. Skipping." }));
			return null;
		}
		Shader shader = Shader.Find(text);
		if (shader == null)
		{
			Debug.LogWarning(string.Concat(new string[] { "Non-tintable shader '", text, "' not found for material ", material.name, ". Skipping." }));
			return null;
		}
		return shader;
	}

	// Token: 0x06009AD4 RID: 39636 RVA: 0x003A7CFC File Offset: 0x003A5EFC
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D BakeAlbedoWithTintsGPU(Material sourceMaterial, Texture albedo, Texture indexTex, Texture tintMask, Texture paletteTex)
	{
		TintableMaterialBaker.BakerMaterial.SetTexture("_Albedo", albedo);
		TintableMaterialBaker.BakerMaterial.SetTexture("_IndexTex", indexTex);
		TintableMaterialBaker.BakerMaterial.SetTexture("_TintMask", tintMask);
		TintableMaterialBaker.BakerMaterial.SetTexture("_PaletteTex", paletteTex);
		TintableMaterialBaker.BakerMaterial.SetFloat("_PaletteTexelSize", 1f / (float)paletteTex.width);
		int width = albedo.width;
		int height = albedo.height;
		bool flag = GraphicsFormatUtility.HasAlphaChannel(albedo.graphicsFormat);
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
		Graphics.Blit(null, temporary, TintableMaterialBaker.BakerMaterial);
		Texture2D texture2D = TintableMaterialBaker.RenderTextureToTexture2D(temporary, width, height, flag);
		texture2D.name = albedo.name + "_Baked";
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}

	// Token: 0x06009AD5 RID: 39637 RVA: 0x003A7DC0 File Offset: 0x003A5FC0
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D RenderTextureToTexture2D(RenderTexture renderTexture, int width, int height, bool retainAlphaChannel)
	{
		Texture2D texture2D;
		if (retainAlphaChannel)
		{
			texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
		}
		else
		{
			texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
		texture2D.Compress(true);
		texture2D.Apply();
		RenderTexture.active = active;
		return texture2D;
	}

	// Token: 0x06009AD6 RID: 39638 RVA: 0x003A7E21 File Offset: 0x003A6021
	public static void Cleanup()
	{
		if (TintableMaterialBaker._bakerMaterial != null)
		{
			global::UnityEngine.Object.DestroyImmediate(TintableMaterialBaker._bakerMaterial);
			TintableMaterialBaker._bakerMaterial = null;
		}
		TintableMaterialBaker._bakerShader = null;
	}

	// Token: 0x040074B2 RID: 29874
	[PublicizedFrom(EAccessModifier.Private)]
	public static Shader _bakerShader;

	// Token: 0x040074B3 RID: 29875
	[PublicizedFrom(EAccessModifier.Private)]
	public static Material _bakerMaterial;
}
