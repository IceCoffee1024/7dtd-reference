using System;
using UnityEngine;

// Token: 0x02001364 RID: 4964
public class CrosshairDrawer : MonoBehaviour
{
	// Token: 0x06009C8C RID: 40076 RVA: 0x003B3871 File Offset: 0x003B1A71
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		if (!this.draw)
		{
			return;
		}
		EntityPlayerLocal.DrawDynamicCrosshair(this.centerX, this.centerY, this.openAreaX, this.openAreaY, this.length, this.thickness, this.opacity, this.color);
	}

	// Token: 0x040076BB RID: 30395
	public bool draw;

	// Token: 0x040076BC RID: 30396
	public float centerX;

	// Token: 0x040076BD RID: 30397
	public float centerY;

	// Token: 0x040076BE RID: 30398
	public float openAreaX;

	// Token: 0x040076BF RID: 30399
	public float openAreaY;

	// Token: 0x040076C0 RID: 30400
	public float length;

	// Token: 0x040076C1 RID: 30401
	public float thickness;

	// Token: 0x040076C2 RID: 30402
	public float opacity;

	// Token: 0x040076C3 RID: 30403
	public Color color = Color.white;
}
