using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000238 RID: 568
[Preserve]
public class ConsoleCmdListItems : ConsoleCmdAbstract
{
	// Token: 0x06001102 RID: 4354 RVA: 0x0006C6D9 File Offset: 0x0006A8D9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "lists all items that contain the given substring";
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0006C6E0 File Offset: 0x0006A8E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listitems", "li" };
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0006C6F8 File Offset: 0x0006A8F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "List all available item names\nUsage:\n   1. listitems <searchString>\n   2. listitems *\n1. List only names that contain the given string.\n2. List all names.";
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0006C700 File Offset: 0x0006A900
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
