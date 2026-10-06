using System;
using UnityEngine;

// Token: 0x020012C8 RID: 4808
public class PlayerAutoPilotControllor
{
	// Token: 0x06009845 RID: 38981 RVA: 0x0039700F File Offset: 0x0039520F
	public PlayerAutoPilotControllor(GameManager _gm)
	{
	}

	// Token: 0x06009846 RID: 38982 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsEnabled()
	{
		return false;
	}

	// Token: 0x06009847 RID: 38983 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06009848 RID: 38984 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public float GetForwardMovement()
	{
		return 0f;
	}

	// Token: 0x04007229 RID: 29225
	[PublicizedFrom(EAccessModifier.Private)]
	public int frameCnt;

	// Token: 0x0400722A RID: 29226
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 lastPosition = Vector3.zero;
}
