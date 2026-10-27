using System;
using UnityEngine;

// Token: 0x0200138E RID: 5006
public class Polarizer : MonoBehaviour
{
	// Token: 0x06009D50 RID: 40272 RVA: 0x003B6FB0 File Offset: 0x003B51B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		Shader shader = GlobalAssets.FindShader("Custom/DebugView");
		if (shader != null)
		{
			this.material = new Material(shader);
		}
	}

	// Token: 0x06009D51 RID: 40273 RVA: 0x003B6FDD File Offset: 0x003B51DD
	public static void SetDebugView(Polarizer.ViewEnums view)
	{
		Polarizer.currDebugView = view;
	}

	// Token: 0x06009D52 RID: 40274 RVA: 0x003B6FE5 File Offset: 0x003B51E5
	public static Polarizer.ViewEnums GetDebugView()
	{
		return Polarizer.currDebugView;
	}

	// Token: 0x06009D53 RID: 40275 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPreRender()
	{
	}

	// Token: 0x06009D54 RID: 40276 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPostRender()
	{
	}

	// Token: 0x040076C3 RID: 30403
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x040076C4 RID: 30404
	public static Polarizer.ViewEnums currDebugView;

	// Token: 0x0200138F RID: 5007
	public enum ViewEnums
	{
		// Token: 0x040076C6 RID: 30406
		None,
		// Token: 0x040076C7 RID: 30407
		Normals,
		// Token: 0x040076C8 RID: 30408
		Albedo,
		// Token: 0x040076C9 RID: 30409
		Specular
	}
}
