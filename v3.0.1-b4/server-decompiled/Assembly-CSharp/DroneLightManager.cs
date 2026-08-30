using System;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class DroneLightManager : MonoBehaviour
{
	// Token: 0x06001DE6 RID: 7654 RVA: 0x000B5C50 File Offset: 0x000B3E50
	public void InitMaterials(string key)
	{
		DroneLightManager.LightEffect lightEffect = this.getLightEffect(key);
		if (lightEffect == null)
		{
			Debug.LogWarning("Failed to find drone light with name: " + key, this);
			return;
		}
		for (int i = 0; i < lightEffect.linkedObjects.Length; i++)
		{
			lightEffect.linkedObjects[i].SetActive(true);
		}
		for (int j = 0; j < base.transform.childCount; j++)
		{
			SkinnedMeshRenderer component = base.transform.GetChild(j).GetComponent<SkinnedMeshRenderer>();
			if (component)
			{
				Material[] materials = component.materials;
				for (int k = materials.Length - 1; k >= 0; k--)
				{
					if (materials[k].name.Replace(" (Instance)", "") == lightEffect.material.name)
					{
						materials[k].SetColor("_EmissionColor", lightEffect.material.GetColor("_EmissionColor"));
						break;
					}
				}
			}
		}
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000B5D3C File Offset: 0x000B3F3C
	public void DisableMaterials(string key)
	{
		DroneLightManager.LightEffect lightEffect = this.getLightEffect(key);
		if (lightEffect == null)
		{
			Debug.LogWarning("Failed to find drone light with name: " + key, this);
			return;
		}
		for (int i = 0; i < lightEffect.linkedObjects.Length; i++)
		{
			lightEffect.linkedObjects[i].SetActive(false);
		}
		for (int j = 0; j < base.transform.childCount; j++)
		{
			SkinnedMeshRenderer component = base.transform.GetChild(j).GetComponent<SkinnedMeshRenderer>();
			if (component)
			{
				Material[] materials = component.materials;
				for (int k = materials.Length - 1; k >= 0; k--)
				{
					if (materials[k].name.Replace(" (Instance)", "") == lightEffect.material.name)
					{
						materials[k].SetColor("_EmissionColor", Color.black);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x000B5E18 File Offset: 0x000B4018
	[PublicizedFrom(EAccessModifier.Private)]
	public DroneLightManager.LightEffect getLightEffect(string key)
	{
		for (int i = 0; i < this.LightEffects.Length; i++)
		{
			if (this.LightEffects[i].material.name == key)
			{
				return this.LightEffects[i];
			}
		}
		return null;
	}

	// Token: 0x040013F4 RID: 5108
	public DroneLightManager.LightEffect[] LightEffects;

	// Token: 0x020003DB RID: 987
	[Serializable]
	public class LightEffect
	{
		// Token: 0x040013F5 RID: 5109
		public bool startsOn;

		// Token: 0x040013F6 RID: 5110
		public Material material;

		// Token: 0x040013F7 RID: 5111
		public GameObject[] linkedObjects;
	}
}
