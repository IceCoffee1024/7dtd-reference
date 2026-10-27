using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023A RID: 570
[Preserve]
public class ConsoleCmdListItems : ConsoleCmdAbstract
{
	// Token: 0x06001106 RID: 4358 RVA: 0x0006C8A9 File Offset: 0x0006AAA9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "lists all items that contain the given substring";
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x0006C8B0 File Offset: 0x0006AAB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listitems", "li" };
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x0006C8C8 File Offset: 0x0006AAC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "List all available item names\nUsage:\n   1. listitems <searchString>\n   2. listitems *\n1. List only names that contain the given string.\n2. List all names.";
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x0006C8D0 File Offset: 0x0006AAD0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 1 || _params[0].Length == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Usage: listitems <searchString>");
			return;
		}
		int count = ItemClass.ItemNames.Count;
		bool flag = _params[0].Trim().Equals("*");
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			string text = ItemClass.ItemNames[i];
			if (flag || text.IndexOf(_params[0], StringComparison.OrdinalIgnoreCase) >= 0)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("    " + text);
				num++;
			}
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Listed {0} matching items.", num));
	}
}
