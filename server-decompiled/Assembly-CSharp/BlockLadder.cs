using System;
using UnityEngine.Scripting;

// Token: 0x02000125 RID: 293
[Preserve]
public class BlockLadder : Block
{
	// Token: 0x060007BC RID: 1980 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsElevator()
	{
		return true;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00036CF4 File Offset: 0x00034EF4
	public override bool IsElevator(int rotation)
	{
		return BlockLadder.climbableRotations[rotation] > 0;
	}

	// Token: 0x04000915 RID: 2325
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] climbableRotations = new byte[]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 0, 1,
		0, 1, 1, 0, 1, 0, 0, 1, 0, 1,
		1, 0, 1, 0, 0, 0, 0, 0, 0, 0,
		0, 0
	};
}
