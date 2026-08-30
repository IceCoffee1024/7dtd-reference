using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class OpenURLOnClick : MonoBehaviour
{
	// Token: 0x060001B7 RID: 439 RVA: 0x0001038C File Offset: 0x0000E58C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClick()
	{
		UILabel component = base.GetComponent<UILabel>();
		if (component != null)
		{
			string urlAtPosition = component.GetUrlAtPosition(UICamera.lastWorldPosition);
			if (!string.IsNullOrEmpty(urlAtPosition))
			{
				Application.OpenURL(urlAtPosition);
			}
		}
	}
}
