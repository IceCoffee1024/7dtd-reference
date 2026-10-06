using System;
using UnityEngine;

// Token: 0x02001447 RID: 5191
[CreateAssetMenu(menuName = "Gradients/Lightbar Gradient Data")]
[Serializable]
public class LightbarGradients : ScriptableObject
{
	// Token: 0x04007A50 RID: 31312
	public Gradient timeOfDayGradient;

	// Token: 0x04007A51 RID: 31313
	public Gradient dayGradient;

	// Token: 0x04007A52 RID: 31314
	public Gradient nightGradient;

	// Token: 0x04007A53 RID: 31315
	public Gradient cloudDayGradient;

	// Token: 0x04007A54 RID: 31316
	public Gradient cloudNightGradient;

	// Token: 0x04007A55 RID: 31317
	public Gradient bloodmoonGradient;

	// Token: 0x04007A56 RID: 31318
	public Color mainMenuColor;
}
