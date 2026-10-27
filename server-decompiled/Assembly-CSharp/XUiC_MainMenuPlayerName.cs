using System;
using UnityEngine.Scripting;

// Token: 0x02000E2F RID: 3631
[Preserve]
public class XUiC_MainMenuPlayerName : XUiController
{
	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x06006EB1 RID: 28337 RVA: 0x002B2006 File Offset: 0x002B0206
	[XuiXmlBinding("name")]
	public string PlayerName
	{
		get
		{
			return GamePrefs.GetString(EnumGamePrefs.PlayerName) ?? "";
		}
	}
}
