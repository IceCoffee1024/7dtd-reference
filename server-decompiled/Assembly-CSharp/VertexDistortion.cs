using System;

// Token: 0x02000BDB RID: 3035
public class VertexDistortion
{
	// Token: 0x06005B58 RID: 23384 RVA: 0x0023343C File Offset: 0x0023163C
	[PublicizedFrom(EAccessModifier.Private)]
	static VertexDistortion()
	{
		for (int i = 0; i < VertexDistortion.arrayB.Length; i++)
		{
			VertexDistortion.arrayB[i] *= 1.5f;
		}
	}

	// Token: 0x04004724 RID: 18212
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] arrayB = new float[] { 0f, 0.2f, 0.15f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f };
}
