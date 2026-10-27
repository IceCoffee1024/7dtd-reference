using System;
using UnityEngine;

// Token: 0x020012DA RID: 4826
public class PlayerAutoPilotControllor
{
	// Token: 0x0600989D RID: 39069 RVA: 0x00399413 File Offset: 0x00397613
	public PlayerAutoPilotControllor(GameManager _gm)
	{
	}

	// Token: 0x0600989E RID: 39070 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsEnabled()
	{
		return false;
	}

	// Token: 0x0600989F RID: 39071 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x060098A0 RID: 39072 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetForwardMovement()
	{
		return 0f;
	}

	// Token: 0x0400726A RID: 29290
	[PublicizedFrom(EAccessModifier.Private)]
	public int frameCnt;

	// Token: 0x0400726B RID: 29291
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 lastPosition = Vector3.zero;
}
