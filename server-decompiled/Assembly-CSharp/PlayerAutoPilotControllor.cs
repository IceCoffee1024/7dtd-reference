using System;
using UnityEngine;

// Token: 0x020012B0 RID: 4784
public class PlayerAutoPilotControllor
{
	// Token: 0x060097B8 RID: 38840 RVA: 0x003927A7 File Offset: 0x003909A7
	public PlayerAutoPilotControllor(GameManager _gm)
	{
	}

	// Token: 0x060097B9 RID: 38841 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsEnabled()
	{
		return false;
	}

	// Token: 0x060097BA RID: 38842 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x060097BB RID: 38843 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public float GetForwardMovement()
	{
		return 0f;
	}

	// Token: 0x040071BF RID: 29119
	[PublicizedFrom(EAccessModifier.Private)]
	public int frameCnt;

	// Token: 0x040071C0 RID: 29120
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 lastPosition = Vector3.zero;
}
