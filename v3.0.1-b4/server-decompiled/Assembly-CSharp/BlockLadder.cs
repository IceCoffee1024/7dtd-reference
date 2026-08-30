using System;
using UnityEngine.Scripting;

// Token: 0x02000124 RID: 292
[Preserve]
public class BlockLadder : Block
{
	// Token: 0x060007B2 RID: 1970 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsElevator()
	{
		return true;
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x00036A04 File Offset: 0x00034C04
	public override bool IsElevator(int rotation)
	{
		return BlockLadder.climbableRotations[rotation] > 0;
	}

	// Token: 0x04000913 RID: 2323
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] climbableRotations = new byte[]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 0, 1,
		0, 1, 1, 0, 1, 0, 0, 1, 0, 1,
		1, 0, 1, 0, 0, 0, 0, 0, 0, 0,
		0, 0
	};
}
