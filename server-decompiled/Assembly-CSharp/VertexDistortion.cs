using System;

// Token: 0x02000BF8 RID: 3064
public class VertexDistortion
{
	// Token: 0x06005B96 RID: 23446 RVA: 0x00237C84 File Offset: 0x00235E84
	[PublicizedFrom(EAccessModifier.Private)]
	static VertexDistortion()
	{
		for (int i = 0; i < VertexDistortion.arrayB.Length; i++)
		{
			VertexDistortion.arrayB[i] *= 1.5f;
		}
	}

	// Token: 0x04004788 RID: 18312
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] arrayB = new float[] { 0f, 0.2f, 0.15f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f };
}
