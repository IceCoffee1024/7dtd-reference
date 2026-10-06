using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001233 RID: 4659
public class GUILocalizedLabel : MonoBehaviour
{
	// Token: 0x0600948A RID: 38026 RVA: 0x00383D00 File Offset: 0x00381F00
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Text component = base.GetComponent<Text>();
		if (component)
		{
			component.text = Localization.Get(this.localizationKey, false, null);
		}
	}

	// Token: 0x04006F51 RID: 28497
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizationKey;
}
