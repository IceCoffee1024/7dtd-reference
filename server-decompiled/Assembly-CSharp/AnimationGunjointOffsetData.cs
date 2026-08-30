using System;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
public class AnimationGunjointOffsetData
{
	// Token: 0x0600310C RID: 12556 RVA: 0x0013ED50 File Offset: 0x0013CF50
	public static void InitStatic()
	{
		AnimationGunjointOffsetData.AnimationGunjointOffset = new AnimationGunjointOffsetData.AnimationGunjointOffsets[100];
		for (int i = 0; i < AnimationGunjointOffsetData.AnimationGunjointOffset.Length; i++)
		{
			AnimationGunjointOffsetData.AnimationGunjointOffset[i] = new AnimationGunjointOffsetData.AnimationGunjointOffsets(Vector3.zero, Vector3.zero);
		}
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x0013ED95 File Offset: 0x0013CF95
	public static void Cleanup()
	{
		AnimationGunjointOffsetData.InitStatic();
	}

	// Token: 0x04002609 RID: 9737
	public static AnimationGunjointOffsetData.AnimationGunjointOffsets[] AnimationGunjointOffset;

	// Token: 0x020005E8 RID: 1512
	public struct AnimationGunjointOffsets
	{
		// Token: 0x0600310F RID: 12559 RVA: 0x0013ED9C File Offset: 0x0013CF9C
		public AnimationGunjointOffsets(Vector3 _position, Vector3 _rotation)
		{
			this.position = _position;
			this.rotation = _rotation;
		}

		// Token: 0x0400260A RID: 9738
		public Vector3 position;

		// Token: 0x0400260B RID: 9739
		public Vector3 rotation;
	}
}
