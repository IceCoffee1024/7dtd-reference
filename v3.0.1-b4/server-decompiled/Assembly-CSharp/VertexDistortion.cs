using System;

// Token: 0x02000BC9 RID: 3017
public class VertexDistortion
{
	// Token: 0x06005ACF RID: 23247 RVA: 0x00230720 File Offset: 0x0022E920
	[PublicizedFrom(EAccessModifier.Private)]
	static VertexDistortion()
	{
		for (int i = 0; i < VertexDistortion.arrayB.Length; i++)
		{
			VertexDistortion.arrayB[i] *= 1.5f;
		}
	}

	// Token: 0x04004688 RID: 18056
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] arrayB = new float[] { 0f, 0.2f, 0.15f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f };
}
