using System;
using UnityEngine;

// Token: 0x0200135C RID: 4956
public class Polarizer : MonoBehaviour
{
	// Token: 0x06009C4C RID: 40012 RVA: 0x003B0030 File Offset: 0x003AE230
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		Shader shader = GlobalAssets.FindShader("Custom/DebugView");
		if (shader != null)
		{
			this.material = new Material(shader);
		}
	}

	// Token: 0x06009C4D RID: 40013 RVA: 0x003B005D File Offset: 0x003AE25D
	public static void SetDebugView(Polarizer.ViewEnums view)
	{
		Polarizer.currDebugView = view;
	}

	// Token: 0x06009C4E RID: 40014 RVA: 0x003B0065 File Offset: 0x003AE265
	public static Polarizer.ViewEnums GetDebugView()
	{
		return Polarizer.currDebugView;
	}

	// Token: 0x06009C4F RID: 40015 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPreRender()
	{
	}

	// Token: 0x06009C50 RID: 40016 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPostRender()
	{
	}

	// Token: 0x04007607 RID: 30215
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x04007608 RID: 30216
	public static Polarizer.ViewEnums currDebugView;

	// Token: 0x0200135D RID: 4957
	public enum ViewEnums
	{
		// Token: 0x0400760A RID: 30218
		None,
		// Token: 0x0400760B RID: 30219
		Normals,
		// Token: 0x0400760C RID: 30220
		Albedo,
		// Token: 0x0400760D RID: 30221
		Specular
	}
}
