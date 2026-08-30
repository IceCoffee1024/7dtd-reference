using System;
using UnityEngine;

// Token: 0x0200137E RID: 4990
public class CrosshairDrawer : MonoBehaviour
{
	// Token: 0x06009D87 RID: 40327 RVA: 0x003B8B25 File Offset: 0x003B6D25
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		if (!this.draw)
		{
			return;
		}
		EntityPlayerLocal.DrawDynamicCrosshair(this.centerX, this.centerY, this.openAreaX, this.openAreaY, this.length, this.thickness, this.opacity, this.color);
	}

	// Token: 0x04007793 RID: 30611
	public bool draw;

	// Token: 0x04007794 RID: 30612
	public float centerX;

	// Token: 0x04007795 RID: 30613
	public float centerY;

	// Token: 0x04007796 RID: 30614
	public float openAreaX;

	// Token: 0x04007797 RID: 30615
	public float openAreaY;

	// Token: 0x04007798 RID: 30616
	public float length;

	// Token: 0x04007799 RID: 30617
	public float thickness;

	// Token: 0x0400779A RID: 30618
	public float opacity;

	// Token: 0x0400779B RID: 30619
	public Color color = Color.white;
}
