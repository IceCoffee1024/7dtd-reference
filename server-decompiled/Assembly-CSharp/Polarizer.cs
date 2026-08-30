using System;
using UnityEngine;

// Token: 0x02001342 RID: 4930
public class Polarizer : MonoBehaviour
{
	// Token: 0x06009B51 RID: 39761 RVA: 0x003AAD88 File Offset: 0x003A8F88
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		Shader shader = GlobalAssets.FindShader("Custom/DebugView");
		if (shader != null)
		{
			this.material = new Material(shader);
		}
	}

	// Token: 0x06009B52 RID: 39762 RVA: 0x003AADB5 File Offset: 0x003A8FB5
	public static void SetDebugView(Polarizer.ViewEnums view)
	{
		Polarizer.currDebugView = view;
	}

	// Token: 0x06009B53 RID: 39763 RVA: 0x003AADBD File Offset: 0x003A8FBD
	public static Polarizer.ViewEnums GetDebugView()
	{
		return Polarizer.currDebugView;
	}

	// Token: 0x06009B54 RID: 39764 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPreRender()
	{
	}

	// Token: 0x06009B55 RID: 39765 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnPostRender()
	{
	}

	// Token: 0x0400752F RID: 29999
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x04007530 RID: 30000
	public static Polarizer.ViewEnums currDebugView;

	// Token: 0x02001343 RID: 4931
	public enum ViewEnums
	{
		// Token: 0x04007532 RID: 30002
		None,
		// Token: 0x04007533 RID: 30003
		Normals,
		// Token: 0x04007534 RID: 30004
		Albedo,
		// Token: 0x04007535 RID: 30005
		Specular
	}
}
