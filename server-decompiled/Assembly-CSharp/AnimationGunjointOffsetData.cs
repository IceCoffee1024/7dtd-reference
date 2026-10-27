using System;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public class AnimationGunjointOffsetData
{
	// Token: 0x0600318F RID: 12687 RVA: 0x00141450 File Offset: 0x0013F650
	public static void InitStatic()
	{
		AnimationGunjointOffsetData.AnimationGunjointOffset = new AnimationGunjointOffsetData.AnimationGunjointOffsets[100];
		for (int i = 0; i < AnimationGunjointOffsetData.AnimationGunjointOffset.Length; i++)
		{
			AnimationGunjointOffsetData.AnimationGunjointOffset[i] = new AnimationGunjointOffsetData.AnimationGunjointOffsets(Vector3.zero, Vector3.zero);
		}
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x00141495 File Offset: 0x0013F695
	public static void Cleanup()
	{
		AnimationGunjointOffsetData.InitStatic();
	}

	// Token: 0x04002652 RID: 9810
	public static AnimationGunjointOffsetData.AnimationGunjointOffsets[] AnimationGunjointOffset;

	// Token: 0x020005F5 RID: 1525
	public struct AnimationGunjointOffsets
	{
		// Token: 0x06003192 RID: 12690 RVA: 0x0014149C File Offset: 0x0013F69C
		public AnimationGunjointOffsets(Vector3 _position, Vector3 _rotation)
		{
			this.position = _position;
			this.rotation = _rotation;
		}

		// Token: 0x04002653 RID: 9811
		public Vector3 position;

		// Token: 0x04002654 RID: 9812
		public Vector3 rotation;
	}
}
