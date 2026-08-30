using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000A50 RID: 2640
public class SkinTextureOverride : MonoBehaviour
{
	// Token: 0x06004FD2 RID: 20434 RVA: 0x001E5EA0 File Offset: 0x001E40A0
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

	// Token: 0x04003D31 RID: 15665
	public AssetReference skinAlbedo;

	// Token: 0x04003D32 RID: 15666
	public AssetReference skinNormal;

	// Token: 0x04003D33 RID: 15667
	public AssetReference skinRMOE;
}
