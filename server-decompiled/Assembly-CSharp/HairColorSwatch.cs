using System;
using UnityEngine;

// Token: 0x02001361 RID: 4961
[CreateAssetMenu(fileName = "HairColorSwatch", menuName = "Hair Color Management/Hair Color Swatch", order = 1)]
public class HairColorSwatch : ScriptableObject
{
	// Token: 0x06009C72 RID: 40050 RVA: 0x003B15D4 File Offset: 0x003AF7D4
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

	// Token: 0x06009C73 RID: 40051 RVA: 0x003B16C0 File Offset: 0x003AF8C0
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

	// Token: 0x040075F9 RID: 30201
	[ColorUsage(false, false)]
	public Color tint1 = Color.red;

	// Token: 0x040075FA RID: 30202
	[ColorUsage(false, false)]
	public Color tint2 = Color.green;

	// Token: 0x040075FB RID: 30203
	[ColorUsage(false, false)]
	public Color tint3 = Color.blue;

	// Token: 0x040075FC RID: 30204
	[Range(0f, 1f)]
	public float tintSharpness = 0.5f;

	// Token: 0x040075FD RID: 30205
	[Range(0f, 1f)]
	public float idMapStrength;

	// Token: 0x040075FE RID: 30206
	[Range(0f, 1f)]
	public float rootDarkening;

	// Token: 0x040075FF RID: 30207
	[Range(0f, 1f)]
	public float metallic;

	// Token: 0x04007600 RID: 30208
	[ColorUsage(false, true)]
	public Color cuticleSpecularColor = Color.white;

	// Token: 0x04007601 RID: 30209
	[ColorUsage(false, true)]
	public Color cortexSpecularColor = Color.white;

	// Token: 0x04007602 RID: 30210
	[Range(0f, 10f)]
	public float smoothnessMultiplier = 1f;

	// Token: 0x04007603 RID: 30211
	[ColorUsage(false, false)]
	public Color subsurfaceAmbient = Color.white;

	// Token: 0x04007604 RID: 30212
	[ColorUsage(false, false)]
	public Color subsurfaceColor = Color.white;
}
