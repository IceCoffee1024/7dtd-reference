using System;
using UnityEngine;

// Token: 0x02001459 RID: 5209
[CreateAssetMenu(menuName = "Gradients/Lightbar Gradient Data")]
[Serializable]
public class LightbarGradients : ScriptableObject
{
	// Token: 0x04007A91 RID: 31377
	public Gradient timeOfDayGradient;

	// Token: 0x04007A92 RID: 31378
	public Gradient dayGradient;

	// Token: 0x04007A93 RID: 31379
	public Gradient nightGradient;

	// Token: 0x04007A94 RID: 31380
	public Gradient cloudDayGradient;

	// Token: 0x04007A95 RID: 31381
	public Gradient cloudNightGradient;

	// Token: 0x04007A96 RID: 31382
	public Gradient bloodmoonGradient;

	// Token: 0x04007A97 RID: 31383
	public Color mainMenuColor;
}
