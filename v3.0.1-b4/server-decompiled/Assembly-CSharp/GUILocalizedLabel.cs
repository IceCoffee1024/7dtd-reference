using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020011FF RID: 4607
public class GUILocalizedLabel : MonoBehaviour
{
	// Token: 0x060092F8 RID: 37624 RVA: 0x0037A1DC File Offset: 0x003783DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Text component = base.GetComponent<Text>();
		if (component)
		{
			component.text = Localization.Get(this.localizationKey, false, null);
		}
	}

	// Token: 0x04006E0E RID: 28174
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizationKey;
}
