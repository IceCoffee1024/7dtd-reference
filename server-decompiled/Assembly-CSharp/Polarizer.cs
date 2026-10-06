using System;
using UnityEngine;

// Token: 0x0200137C RID: 4988
public class Polarizer : MonoBehaviour
{
	// Token: 0x06009CF8 RID: 40184 RVA: 0x003B4BAC File Offset: 0x003B2DAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		Shader shader = GlobalAssets.FindShader("Custom/DebugView");
		if (shader != null)
		{
			this.material = new Material(shader);
		}
	}

	// Token: 0x06009CF9 RID: 40185 RVA: 0x003B4BD9 File Offset: 0x003B2DD9
	public static void SetDebugView(Polarizer.ViewEnums view)
	{
		Polarizer.currDebugView = view;
	}

	// Token: 0x06009CFA RID: 40186 RVA: 0x003B4BE1 File Offset: 0x003B2DE1
	public static Polarizer.ViewEnums GetDebugView()
	{
		return Polarizer.currDebugView;
	}

	// Token: 0x06009CFB RID: 40187 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPreRender()
	{
	}

	// Token: 0x06009CFC RID: 40188 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPostRender()
	{
	}

	// Token: 0x04007682 RID: 30338
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x04007683 RID: 30339
	public static Polarizer.ViewEnums currDebugView;

	// Token: 0x0200137D RID: 4989
	public enum ViewEnums
	{
		// Token: 0x04007685 RID: 30341
		None,
		// Token: 0x04007686 RID: 30342
		Normals,
		// Token: 0x04007687 RID: 30343
		Albedo,
		// Token: 0x04007688 RID: 30344
		Specular
	}
}
