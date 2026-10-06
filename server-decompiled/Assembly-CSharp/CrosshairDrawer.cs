using System;
using UnityEngine;

// Token: 0x0200139E RID: 5022
public class CrosshairDrawer : MonoBehaviour
{
	// Token: 0x06009E32 RID: 40498 RVA: 0x003BD6A1 File Offset: 0x003BB8A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		if (!this.draw)
		{
			return;
		}
		EntityPlayerLocal.DrawDynamicCrosshair(this.centerX, this.centerY, this.openAreaX, this.openAreaY, this.length, this.thickness, this.opacity, this.color);
	}

	// Token: 0x0400780E RID: 30734
	public bool draw;

	// Token: 0x0400780F RID: 30735
	public float centerX;

	// Token: 0x04007810 RID: 30736
	public float centerY;

	// Token: 0x04007811 RID: 30737
	public float openAreaX;

	// Token: 0x04007812 RID: 30738
	public float openAreaY;

	// Token: 0x04007813 RID: 30739
	public float length;

	// Token: 0x04007814 RID: 30740
	public float thickness;

	// Token: 0x04007815 RID: 30741
	public float opacity;

	// Token: 0x04007816 RID: 30742
	public Color color = Color.white;
}
