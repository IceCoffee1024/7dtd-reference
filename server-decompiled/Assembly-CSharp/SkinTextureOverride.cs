using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000A6B RID: 2667
public class SkinTextureOverride : MonoBehaviour
{
	// Token: 0x0600502C RID: 20524 RVA: 0x001E9E84 File Offset: 0x001E8084
	public void ApplyOverrides()
	{
		Texture2D texture2D = this.skinAlbedo.LoadAssetAsync<Texture2D>().WaitForCompletion();
		Texture2D texture2D2 = this.skinNormal.LoadAssetAsync<Texture2D>().WaitForCompletion();
		Texture2D texture2D3 = this.skinRMOE.LoadAssetAsync<Texture2D>().WaitForCompletion();
		SkinnedMeshRenderer component = base.transform.GetComponent<SkinnedMeshRenderer>();
		Material[] materials = component.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			if (materials[i] != null && (materials[i].shader.name == "Game/SDCS/Skin" || materials[i].shader.name == "Game/SDCS/Skin_ClipFPV"))
			{
				string name = materials[i].name;
				if (name.Contains("_Body") || name.Contains("_Head") || name.Contains("_Hand"))
				{
					materials[i] = new Material(materials[i]);
					materials[i].SetTexture("_Albedo", texture2D);
					materials[i].SetTexture("_NormalMap", texture2D2);
					materials[i].SetTexture("_Masks", texture2D3);
				}
			}
		}
		component.materials = materials;
	}

	// Token: 0x04003DD0 RID: 15824
	public AssetReference skinAlbedo;

	// Token: 0x04003DD1 RID: 15825
	public AssetReference skinNormal;

	// Token: 0x04003DD2 RID: 15826
	public AssetReference skinRMOE;
}
