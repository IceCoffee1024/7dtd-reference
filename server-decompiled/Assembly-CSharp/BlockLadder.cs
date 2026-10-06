using System;
using UnityEngine.Scripting;

// Token: 0x02000127 RID: 295
[Preserve]
public class BlockLadder : Block
{
	// Token: 0x060007C2 RID: 1986 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsElevator()
	{
		return true;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00036DE8 File Offset: 0x00034FE8
	public override bool IsElevator(int rotation)
	{
		return BlockLadder.climbableRotations[rotation] > 0;
	}

	// Token: 0x04000918 RID: 2328
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] climbableRotations = new byte[]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 0, 1,
		0, 1, 1, 0, 1, 0, 0, 1, 0, 1,
		1, 0, 1, 0, 0, 0, 0, 0, 0, 0,
		0, 0
	};
}
