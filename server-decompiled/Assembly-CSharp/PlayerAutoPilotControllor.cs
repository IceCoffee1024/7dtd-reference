using System;
using UnityEngine;

// Token: 0x02001296 RID: 4758
public class PlayerAutoPilotControllor
{
	// Token: 0x060096BD RID: 38589 RVA: 0x0038D64B File Offset: 0x0038B84B
	public PlayerAutoPilotControllor(GameManager _gm)
	{
	}

	// Token: 0x060096BE RID: 38590 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsEnabled()
	{
		return false;
	}

	// Token: 0x060096BF RID: 38591 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x060096C0 RID: 38592 RVA: 0x0003D066 File Offset: 0x0003B266
	public float GetForwardMovement()
	{
		return 0f;
	}

	// Token: 0x040070EA RID: 28906
	[PublicizedFrom(EAccessModifier.Private)]
	public int frameCnt;

	// Token: 0x040070EB RID: 28907
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 lastPosition = Vector3.zero;
}
