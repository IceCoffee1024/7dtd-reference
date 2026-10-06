using System;
using UnityEngine.Scripting;

// Token: 0x02000E24 RID: 3620
[Preserve]
public class XUiC_MainMenuPlayerName : XUiController
{
	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x06006E8C RID: 28300 RVA: 0x002B0A9A File Offset: 0x002AEC9A
	[XuiXmlBinding("name")]
	public string PlayerName
	{
		get
		{
			return GamePrefs.GetString(EnumGamePrefs.PlayerName) ?? "";
		}
	}
}
