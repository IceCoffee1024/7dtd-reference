using System;
using UnityEngine;

// Token: 0x02001373 RID: 4979
[CreateAssetMenu(fileName = "HairColorSwatch", menuName = "Hair Color Management/Hair Color Swatch", order = 1)]
public class HairColorSwatch : ScriptableObject
{
	// Token: 0x06009CCA RID: 40138 RVA: 0x003B39D8 File Offset: 0x003B1BD8
	public void ApplyToMaterial(Material material)
	{
		if (material.GetFloat("_HoldColor") > 0.5f)
		{
			return;
		}
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

	// Token: 0x06009CCB RID: 40139 RVA: 0x003B3AC4 File Offset: 0x003B1CC4
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

	// Token: 0x0400763A RID: 30266
	[ColorUsage(false, false)]
	public Color tint1 = Color.red;

	// Token: 0x0400763B RID: 30267
	[ColorUsage(false, false)]
	public Color tint2 = Color.green;

	// Token: 0x0400763C RID: 30268
	[ColorUsage(false, false)]
	public Color tint3 = Color.blue;

	// Token: 0x0400763D RID: 30269
	[Range(0f, 1f)]
	public float tintSharpness = 0.5f;

	// Token: 0x0400763E RID: 30270
	[Range(0f, 1f)]
	public float idMapStrength;

	// Token: 0x0400763F RID: 30271
	[Range(0f, 1f)]
	public float rootDarkening;

	// Token: 0x04007640 RID: 30272
	[Range(0f, 1f)]
	public float metallic;

	// Token: 0x04007641 RID: 30273
	[ColorUsage(false, true)]
	public Color cuticleSpecularColor = Color.white;

	// Token: 0x04007642 RID: 30274
	[ColorUsage(false, true)]
	public Color cortexSpecularColor = Color.white;

	// Token: 0x04007643 RID: 30275
	[Range(0f, 10f)]
	public float smoothnessMultiplier = 1f;

	// Token: 0x04007644 RID: 30276
	[ColorUsage(false, false)]
	public Color subsurfaceAmbient = Color.white;

	// Token: 0x04007645 RID: 30277
	[ColorUsage(false, false)]
	public Color subsurfaceColor = Color.white;
}
