using System;
using UnityEngine;

// Token: 0x02000A34 RID: 2612
public class ColorSwatchApplicator : MonoBehaviour
{
	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x06004EFB RID: 20219 RVA: 0x0000A1FE File Offset: 0x000083FE
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x001DF93C File Offset: 0x001DDB3C
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

	// Token: 0x06004EFD RID: 20221 RVA: 0x001DF9C4 File Offset: 0x001DDBC4
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
