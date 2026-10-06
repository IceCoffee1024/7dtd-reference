using System;

// Token: 0x02000BED RID: 3053
public class VertexDistortion
{
	// Token: 0x06005B71 RID: 23409 RVA: 0x00236738 File Offset: 0x00234938
	[PublicizedFrom(EAccessModifier.Private)]
	static VertexDistortion()
	{
		for (int i = 0; i < VertexDistortion.arrayB.Length; i++)
		{
			VertexDistortion.arrayB[i] *= 1.5f;
		}
	}

	// Token: 0x04004763 RID: 18275
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] arrayB = new float[] { 0f, 0.2f, 0.15f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f };
}
