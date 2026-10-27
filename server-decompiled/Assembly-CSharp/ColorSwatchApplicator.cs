using System;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class ColorSwatchApplicator : MonoBehaviour
{
	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x06004F74 RID: 20340 RVA: 0x0000A1FE File Offset: 0x000083FE
	public static string baseHairColorLoc
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "@:Entities/Player/Common/HairColorSwatches";
		}
	}

	// Token: 0x06004F75 RID: 20341 RVA: 0x001E476C File Offset: 0x001E296C
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

	// Token: 0x06004F76 RID: 20342 RVA: 0x001E47F4 File Offset: 0x001E29F4
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
