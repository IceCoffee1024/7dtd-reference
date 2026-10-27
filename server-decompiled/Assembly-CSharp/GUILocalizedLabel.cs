using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001245 RID: 4677
public class GUILocalizedLabel : MonoBehaviour
{
	// Token: 0x060094E1 RID: 38113 RVA: 0x003860FC File Offset: 0x003842FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Text component = base.GetComponent<Text>();
		if (component)
		{
			component.text = Localization.Get(this.localizationKey, false, null);
		}
	}

	// Token: 0x04006F92 RID: 28562
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizationKey;
}
