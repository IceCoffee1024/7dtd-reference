using System;
using UnityEngine;

// Token: 0x0200140C RID: 5132
[CreateAssetMenu(menuName = "Gradients/Lightbar Gradient Data")]
[Serializable]
public class LightbarGradients : ScriptableObject
{
	// Token: 0x040078FC RID: 30972
	public Gradient timeOfDayGradient;

	// Token: 0x040078FD RID: 30973
	public Gradient dayGradient;

	// Token: 0x040078FE RID: 30974
	public Gradient nightGradient;

	// Token: 0x040078FF RID: 30975
	public Gradient cloudDayGradient;

	// Token: 0x04007900 RID: 30976
	public Gradient cloudNightGradient;

	// Token: 0x04007901 RID: 30977
	public Gradient bloodmoonGradient;

	// Token: 0x04007902 RID: 30978
	public Color mainMenuColor;
}
