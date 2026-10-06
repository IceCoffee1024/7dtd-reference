using System;
using UnityEngine;

// Token: 0x02000A4F RID: 2639
public class ColorSwatchApplicator : MonoBehaviour
{
	// Token: 0x170008A0 RID: 2208
	// (get) Token: 0x06004F50 RID: 20304 RVA: 0x0000A1FE File Offset: 0x000083FE
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x06004F51 RID: 20305 RVA: 0x001E3684 File Offset: 0x001E1884
	public void ApplyColorSwatch(string color)
	{
		if (!string.IsNullOrEmpty(color))
		{
			string text = ColorSwatchApplicator.baseHairColorLoc + "/" + color + ".asset";
			ScriptableObject scriptableObject = DataLoader.LoadAsset<ScriptableObject>(text, false);
			if (scriptableObject == null)
			{
				Log.Warning(string.Concat(new string[] { "SDCSUtils::", text, " not found for hair color ", color, "!" }));
				return;
			}
			HairColorSwatch hairColorSwatch = scriptableObject as HairColorSwatch;
			if (hairColorSwatch != null)
			{
				this.ApplySwatchToGameObject(hairColorSwatch);
			}
		}
	}

	// Token: 0x06004F52 RID: 20306 RVA: 0x001E370C File Offset: 0x001E190C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplySwatchToGameObject(HairColorSwatch hairSwatch)
	{
		foreach (Renderer renderer in base.transform.gameObject.GetComponentsInChildren<Renderer>(true))
		{
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				if (materials[j].shader.name == "Game/SDCS/Hair" && !materials[j].name.Contains("lashes"))
				{
					materials[j] = new Material(materials[j]);
					hairSwatch.ApplyToMaterial(materials[j]);
				}
			}
			renderer.materials = materials;
		}
	}
}
