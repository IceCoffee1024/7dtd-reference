using System;
using UnityEngine;

// Token: 0x02001427 RID: 5159
[CreateAssetMenu(menuName = "Gradients/Lightbar Gradient Data")]
[Serializable]
public class LightbarGradients : ScriptableObject
{
	// Token: 0x040079D5 RID: 31189
	public Gradient timeOfDayGradient;

	// Token: 0x040079D6 RID: 31190
	public Gradient dayGradient;

	// Token: 0x040079D7 RID: 31191
	public Gradient nightGradient;

	// Token: 0x040079D8 RID: 31192
	public Gradient cloudDayGradient;

	// Token: 0x040079D9 RID: 31193
	public Gradient cloudNightGradient;

	// Token: 0x040079DA RID: 31194
	public Gradient bloodmoonGradient;

	// Token: 0x040079DB RID: 31195
	public Color mainMenuColor;
}
