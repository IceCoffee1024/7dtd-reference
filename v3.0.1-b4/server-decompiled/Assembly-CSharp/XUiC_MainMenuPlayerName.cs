using System;
using UnityEngine.Scripting;

// Token: 0x02000E00 RID: 3584
[Preserve]
public class XUiC_MainMenuPlayerName : XUiController
{
	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x06006DF4 RID: 28148 RVA: 0x002AA1DE File Offset: 0x002A83DE
	[XuiXmlBinding("name")]
	public string PlayerName
	{
		get
		{
			return GamePrefs.GetString(EnumGamePrefs.PlayerName) ?? "";
		}
	}
}
