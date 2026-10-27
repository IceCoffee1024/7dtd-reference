using System;
using UnityEngine;

// Token: 0x020013B0 RID: 5040
public class CrosshairDrawer : MonoBehaviour
{
	// Token: 0x06009E8A RID: 40586 RVA: 0x003BFAA5 File Offset: 0x003BDCA5
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		if (!this.draw)
		{
			return;
		}
		EntityPlayerLocal.DrawDynamicCrosshair(this.centerX, this.centerY, this.openAreaX, this.openAreaY, this.length, this.thickness, this.opacity, this.color);
	}

	// Token: 0x0400784F RID: 30799
	public bool draw;

	// Token: 0x04007850 RID: 30800
	public float centerX;

	// Token: 0x04007851 RID: 30801
	public float centerY;

	// Token: 0x04007852 RID: 30802
	public float openAreaX;

	// Token: 0x04007853 RID: 30803
	public float openAreaY;

	// Token: 0x04007854 RID: 30804
	public float length;

	// Token: 0x04007855 RID: 30805
	public float thickness;

	// Token: 0x04007856 RID: 30806
	public float opacity;

	// Token: 0x04007857 RID: 30807
	public Color color = Color.white;
}
