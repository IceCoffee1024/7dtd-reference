using System;
using UnityEngine;

// Token: 0x02001327 RID: 4903
[CreateAssetMenu(fileName = "HairColorSwatch", menuName = "Hair Color Management/Hair Color Swatch", order = 1)]
public class HairColorSwatch : ScriptableObject
{
	// Token: 0x06009ACB RID: 39627 RVA: 0x003A77C0 File Offset: 0x003A59C0
	public void ApplyToMaterial(Material material)
	{
		material.SetColor("_Tint1", this.tint1);
		material.SetColor("_Tint2", this.tint2);
		material.SetColor("_Tint3", this.tint3);
		material.SetFloat("_TintSharpness", this.tintSharpness);
		material.SetFloat("_IDMapStrength", this.idMapStrength);
		material.SetFloat("_RootDarkening", this.rootDarkening);
		material.SetFloat("_Metallic", this.metallic);
		material.SetColor("_CuticleSpecularColor", this.cuticleSpecularColor);
		material.SetColor("_CortexSpecularColor", this.cortexSpecularColor);
		material.SetFloat("_SmoothnessMultiplier", this.smoothnessMultiplier);
		material.SetColor("_SubsurfaceAmbient", this.subsurfaceAmbient);
		material.SetColor("_SubsurfaceColor", this.subsurfaceColor);
	}

	// Token: 0x06009ACC RID: 39628 RVA: 0x003A789C File Offset: 0x003A5A9C
	public void ApplySwatchToGameObject(GameObject targetGameObject)
	{
		Shader shader = Shader.Find("Game/SDCS/Hair");
		if (targetGameObject != null)
		{
			foreach (Renderer renderer in targetGameObject.GetComponentsInChildren<Renderer>(true))
			{
				Material[] array;
				if (Application.isPlaying)
				{
					array = renderer.materials;
				}
				else
				{
					array = renderer.sharedMaterials;
				}
				foreach (Material material in array)
				{
					if (material.shader == shader)
					{
						this.ApplyToMaterial(material);
					}
				}
			}
			return;
		}
		Debug.LogWarning("No target GameObject selected.");
	}

	// Token: 0x040074A6 RID: 29862
	[ColorUsage(false, false)]
	public Color tint1 = Color.red;

	// Token: 0x040074A7 RID: 29863
	[ColorUsage(false, false)]
	public Color tint2 = Color.green;

	// Token: 0x040074A8 RID: 29864
	[ColorUsage(false, false)]
	public Color tint3 = Color.blue;

	// Token: 0x040074A9 RID: 29865
	[Range(0f, 1f)]
	public float tintSharpness = 0.5f;

	// Token: 0x040074AA RID: 29866
	[Range(0f, 1f)]
	public float idMapStrength;

	// Token: 0x040074AB RID: 29867
	[Range(0f, 1f)]
	public float rootDarkening;

	// Token: 0x040074AC RID: 29868
	[Range(0f, 1f)]
	public float metallic;

	// Token: 0x040074AD RID: 29869
	[ColorUsage(false, true)]
	public Color cuticleSpecularColor = Color.white;

	// Token: 0x040074AE RID: 29870
	[ColorUsage(false, true)]
	public Color cortexSpecularColor = Color.white;

	// Token: 0x040074AF RID: 29871
	[Range(0f, 10f)]
	public float smoothnessMultiplier = 1f;

	// Token: 0x040074B0 RID: 29872
	[ColorUsage(false, false)]
	public Color subsurfaceAmbient = Color.white;

	// Token: 0x040074B1 RID: 29873
	[ColorUsage(false, false)]
	public Color subsurfaceColor = Color.white;
}
