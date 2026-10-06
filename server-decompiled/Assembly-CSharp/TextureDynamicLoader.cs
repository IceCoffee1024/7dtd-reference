using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C6E RID: 3182
public class TextureDynamicLoader : MonoBehaviour
{
	// Token: 0x06006052 RID: 24658 RVA: 0x00264BF0 File Offset: 0x00262DF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (!this.DistanceChecks)
		{
			return;
		}
		if (Time.time - this.lastTimeChecked < (float)(this.bLastTimeFarAwayCamera ? 5 : 1))
		{
			return;
		}
		this.lastTimeChecked = Time.time;
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		if (this.mainCamera == null)
		{
			return;
		}
		float num = Vector3.Distance(this.mainCamera.transform.position, base.transform.position);
		bool flag = num > (float)this.LoResDistance;
		if (this.bHiResLoaded && flag)
		{
			this.bHiResLoaded = false;
			this.SetLoResTexture(false);
		}
		else if (!this.bHiResLoaded && !flag)
		{
			this.bHiResLoaded = true;
			this.SetHiResTexture();
		}
		this.bLastTimeFarAwayCamera = num > 50f;
	}

	// Token: 0x06006053 RID: 24659 RVA: 0x00264CC1 File Offset: 0x00262EC1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (!this.DistanceChecks && !this.bHiResLoaded)
		{
			this.bHiResLoaded = true;
			this.SetHiResTexture();
		}
	}

	// Token: 0x06006054 RID: 24660 RVA: 0x00264CE0 File Offset: 0x00262EE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.mainCamera = null;
		if (this.bHiResLoaded)
		{
			this.SetLoResTexture(false);
			this.bHiResLoaded = false;
		}
	}

	// Token: 0x06006055 RID: 24661 RVA: 0x00264D00 File Offset: 0x00262F00
	public bool IsHiResTextureLoaded(out bool _bHires)
	{
		_bHires = false;
		this.checkMaterials();
		if (this.materials.Count == 0 || this.materials[0].Length == 0 || !this.materials[0][0])
		{
			return false;
		}
		Texture texture = this.materials[0][0].GetTexture(TextureDynamicLoader.textureTypes[0]);
		if (texture == null)
		{
			return false;
		}
		string name = texture.name;
		_bHires = !name.EndsWith("_LOW");
		return true;
	}

	// Token: 0x06006056 RID: 24662 RVA: 0x00264D88 File Offset: 0x00262F88
	public void SetHiResTexture()
	{
		this.checkMaterials();
		for (int i = 0; i < this.materials.Count; i++)
		{
			for (int j = 0; j < this.materials[i].Length; j++)
			{
				for (int k = 0; k < TextureDynamicLoader.textureTypes.Length; k++)
				{
					Material material = this.materials[i][j];
					if (material.HasProperty(TextureDynamicLoader.textureTypes[k]))
					{
						this.setHiResTexture(material, TextureDynamicLoader.textureTypes[k]);
					}
				}
			}
		}
	}

	// Token: 0x06006057 RID: 24663 RVA: 0x00264E08 File Offset: 0x00263008
	public void SetLoResTexture(bool _bFindFolderAndCreateLoResTex = false)
	{
		this.checkMaterials();
		if (!Application.isPlaying && _bFindFolderAndCreateLoResTex)
		{
			string text = this.determineAssetsFolder();
			if (this.CreateLowResTexture)
			{
				for (int i = 0; i < this.materials.Count; i++)
				{
					for (int j = 0; j < this.materials[i].Length; j++)
					{
						for (int k = 0; k < TextureDynamicLoader.textureTypes.Length; k++)
						{
							Material material = this.materials[i][j];
							if (!(material == null) && material.HasProperty(TextureDynamicLoader.textureTypes[k]))
							{
								Texture texture = material.GetTexture(TextureDynamicLoader.textureTypes[k]);
								if (!(texture == null) && texture is Texture2D)
								{
									this.createLoResTexture(texture as Texture2D, text);
								}
							}
						}
					}
				}
			}
		}
		for (int l = 0; l < this.materials.Count; l++)
		{
			for (int m = 0; m < this.materials[l].Length; m++)
			{
				for (int n = 0; n < TextureDynamicLoader.textureTypes.Length; n++)
				{
					Material material2 = this.materials[l][m];
					if (!(material2 == null) && material2.HasProperty(TextureDynamicLoader.textureTypes[n]))
					{
						this.setLoResTexture(material2, TextureDynamicLoader.textureTypes[n]);
					}
				}
			}
		}
	}

	// Token: 0x06006058 RID: 24664 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void createLoResTexture(Texture2D _tex, string _path)
	{
	}

	// Token: 0x06006059 RID: 24665 RVA: 0x00264F70 File Offset: 0x00263170
	public static void SaveTexture(Texture2D _texture, string _fileName)
	{
		byte[] array = _texture.EncodeToPNG();
		SdFile.WriteAllBytes(_fileName, array);
	}

	// Token: 0x0600605A RID: 24666 RVA: 0x000200DA File Offset: 0x0001E2DA
	[PublicizedFrom(EAccessModifier.Private)]
	public string determineAssetsFolder()
	{
		return null;
	}

	// Token: 0x0600605B RID: 24667 RVA: 0x00264F8C File Offset: 0x0026318C
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkMaterials()
	{
		if (Application.isPlaying && this.bGotMaterials)
		{
			return;
		}
		this.bGotMaterials = true;
		base.GetComponentsInChildren<Renderer>(true, this.renderes);
		this.materials.Clear();
		this.materials.Capacity = this.renderes.Count;
		for (int i = 0; i < this.renderes.Count; i++)
		{
			Renderer renderer = this.renderes[i];
			bool flag = true;
			int num = 0;
			while (this.ExcludedRenderers != null && num < this.ExcludedRenderers.Length)
			{
				if (renderer == this.ExcludedRenderers[num])
				{
					flag = false;
					break;
				}
				num++;
			}
			if (flag)
			{
				if (Application.isPlaying && this.UseInstancedMaterial)
				{
					this.materials.Add(renderer.materials);
				}
				else
				{
					this.materials.Add(renderer.sharedMaterials);
				}
			}
		}
	}

	// Token: 0x0600605C RID: 24668 RVA: 0x0026506C File Offset: 0x0026326C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setHiResTexture(Material _m, string _propName)
	{
		if (!_m.HasProperty(_propName))
		{
			return;
		}
		Texture texture = _m.GetTexture(_propName);
		if (texture == null)
		{
			return;
		}
		string text = texture.name;
		if (!text.EndsWith("_LOW"))
		{
			if (Application.isPlaying)
			{
				TextureLoadingManager.Instance.LoadTexture(_m, _propName, this.AssetPath, text, texture);
			}
			return;
		}
		text = text.Substring(0, text.Length - "_LOW".Length);
		if (Application.isPlaying)
		{
			TextureLoadingManager.Instance.LoadTexture(_m, _propName, this.AssetPath, text, texture);
			return;
		}
		Texture texture2 = Resources.Load<Texture2D>(this.AssetPath + text);
		_m.SetTexture(_propName, texture2);
	}

	// Token: 0x0600605D RID: 24669 RVA: 0x00265114 File Offset: 0x00263314
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLoResTexture(Material _m, string _propName)
	{
		Texture texture = _m.GetTexture(_propName);
		if (texture == null)
		{
			return;
		}
		string text = texture.name;
		if (text.EndsWith("_LOW"))
		{
			if (Application.isPlaying)
			{
				TextureLoadingManager.Instance.UnloadTexture(this.AssetPath, text.Substring(0, text.Length - "_LOW".Length));
			}
			return;
		}
		text += "_LOW";
		bool flag = true;
		if (Application.isPlaying)
		{
			flag = TextureLoadingManager.Instance.UnloadTexture(this.AssetPath, texture.name);
		}
		if (flag || this.UseInstancedMaterial)
		{
			Texture texture2 = Resources.Load<Texture2D>(this.AssetPath + text);
			_m.SetTexture(_propName, texture2);
		}
	}

	// Token: 0x04004BC0 RID: 19392
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cWidthLoRes = 64;

	// Token: 0x04004BC1 RID: 19393
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayNearCamera = 1;

	// Token: 0x04004BC2 RID: 19394
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDelayFarAwayCamera = 5;

	// Token: 0x04004BC3 RID: 19395
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDefaultDistanceLoRes = 20;

	// Token: 0x04004BC4 RID: 19396
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDistFarAwayCamera = 50;

	// Token: 0x04004BC5 RID: 19397
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPrefixPath = "Assets/Resources";

	// Token: 0x04004BC6 RID: 19398
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cPostFixLoResTex = "_LOW";

	// Token: 0x04004BC7 RID: 19399
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string[] textureTypes = new string[] { "_MainTex", "_BumpMap", "_MetallicGlossMap", "_SpecGlossMap", "_OcclusionMap", "_EmissionMap" };

	// Token: 0x04004BC8 RID: 19400
	public int LoResDistance = 20;

	// Token: 0x04004BC9 RID: 19401
	public string AssetPath;

	// Token: 0x04004BCA RID: 19402
	public bool DistanceChecks = true;

	// Token: 0x04004BCB RID: 19403
	public bool UseInstancedMaterial = true;

	// Token: 0x04004BCC RID: 19404
	public Renderer[] ExcludedRenderers;

	// Token: 0x04004BCD RID: 19405
	public bool CreateLowResTexture;

	// Token: 0x04004BCE RID: 19406
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Material[]> materials = new List<Material[]>();

	// Token: 0x04004BCF RID: 19407
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Renderer> renderes = new List<Renderer>();

	// Token: 0x04004BD0 RID: 19408
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bGotMaterials;

	// Token: 0x04004BD1 RID: 19409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeChecked;

	// Token: 0x04004BD2 RID: 19410
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mainCamera;

	// Token: 0x04004BD3 RID: 19411
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bLastTimeFarAwayCamera;

	// Token: 0x04004BD4 RID: 19412
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bHiResLoaded;
}
