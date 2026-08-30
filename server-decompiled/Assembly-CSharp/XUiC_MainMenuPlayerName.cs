using System;
using UnityEngine.Scripting;

// Token: 0x02000E15 RID: 3605
[Preserve]
public class XUiC_MainMenuPlayerName : XUiController
{
	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x06006E94 RID: 28308 RVA: 0x002AD6F2 File Offset: 0x002AB8F2
	[XuiXmlBinding("name")]
	public string PlayerName
	{
		get
		{
			return GamePrefs.GetString(EnumGamePrefs.PlayerName) ?? "";
		}
	}
}
