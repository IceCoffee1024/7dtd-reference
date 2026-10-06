using System;
using UnityEngine;

// Token: 0x020003E1 RID: 993
public class DroneLightManager : MonoBehaviour
{
	// Token: 0x06001DFB RID: 7675 RVA: 0x000B65A0 File Offset: 0x000B47A0
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

	// Token: 0x06001DFC RID: 7676 RVA: 0x000B668C File Offset: 0x000B488C
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

	// Token: 0x06001DFD RID: 7677 RVA: 0x000B6768 File Offset: 0x000B4968
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

	// Token: 0x04001402 RID: 5122
	public DroneLightManager.LightEffect[] LightEffects;

	// Token: 0x020003E2 RID: 994
	[Serializable]
	public class LightEffect
	{
		// Token: 0x04001403 RID: 5123
		public bool startsOn;

		// Token: 0x04001404 RID: 5124
		public Material material;

		// Token: 0x04001405 RID: 5125
		public GameObject[] linkedObjects;
	}
}
