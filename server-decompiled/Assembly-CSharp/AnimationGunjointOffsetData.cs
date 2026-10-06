using System;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public class AnimationGunjointOffsetData
{
	// Token: 0x06003181 RID: 12673 RVA: 0x0014085C File Offset: 0x0013EA5C
	public static void InitStatic()
	{
		AnimationGunjointOffsetData.AnimationGunjointOffset = new AnimationGunjointOffsetData.AnimationGunjointOffsets[100];
		for (int i = 0; i < AnimationGunjointOffsetData.AnimationGunjointOffset.Length; i++)
		{
			AnimationGunjointOffsetData.AnimationGunjointOffset[i] = new AnimationGunjointOffsetData.AnimationGunjointOffsets(Vector3.zero, Vector3.zero);
		}
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x001408A1 File Offset: 0x0013EAA1
	public static void Cleanup()
	{
		AnimationGunjointOffsetData.InitStatic();
	}

	// Token: 0x04002637 RID: 9783
	public static AnimationGunjointOffsetData.AnimationGunjointOffsets[] AnimationGunjointOffset;

	// Token: 0x020005EE RID: 1518
	public struct AnimationGunjointOffsets
	{
		// Token: 0x06003184 RID: 12676 RVA: 0x001408A8 File Offset: 0x0013EAA8
		public AnimationGunjointOffsets(Vector3 _position, Vector3 _rotation)
		{
			this.position = _position;
			this.rotation = _rotation;
		}

		// Token: 0x04002638 RID: 9784
		public Vector3 position;

		// Token: 0x04002639 RID: 9785
		public Vector3 rotation;
	}
}
