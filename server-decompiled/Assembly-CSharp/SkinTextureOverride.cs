using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000A75 RID: 2677
public class SkinTextureOverride : MonoBehaviour
{
	// Token: 0x06005050 RID: 20560 RVA: 0x001EAF60 File Offset: 0x001E9160
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

	// Token: 0x04003DF1 RID: 15857
	public AssetReference skinAlbedo;

	// Token: 0x04003DF2 RID: 15858
	public AssetReference skinNormal;

	// Token: 0x04003DF3 RID: 15859
	public AssetReference skinRMOE;
}
