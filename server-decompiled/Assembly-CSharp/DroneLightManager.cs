using System;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public class DroneLightManager : MonoBehaviour
{
	// Token: 0x06001DFF RID: 7679 RVA: 0x000B67B0 File Offset: 0x000B49B0
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

	// Token: 0x06001E00 RID: 7680 RVA: 0x000B689C File Offset: 0x000B4A9C
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

	// Token: 0x06001E01 RID: 7681 RVA: 0x000B6978 File Offset: 0x000B4B78
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

	// Token: 0x04001412 RID: 5138
	public DroneLightManager.LightEffect[] LightEffects;

	// Token: 0x020003E4 RID: 996
	[Serializable]
	public class LightEffect
	{
		// Token: 0x04001413 RID: 5139
		public bool startsOn;

		// Token: 0x04001414 RID: 5140
		public Material material;

		// Token: 0x04001415 RID: 5141
		public GameObject[] linkedObjects;
	}
}
