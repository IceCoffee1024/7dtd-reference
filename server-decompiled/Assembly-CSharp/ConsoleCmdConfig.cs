using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x020001FB RID: 507
[Preserve]
public class ConsoleCmdConfig : ConsoleCmdAbstract
{
	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x00065018 File Offset: 0x00063218
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "config" };
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x00065028 File Offset: 0x00063228
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Import/export config data from/to external file";
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x0006502F File Offset: 0x0006322F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Imports/exports config data from/to external file\nUsage:\n   config import [filename]\n   config export [filename]\n";
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x00065038 File Offset: 0x00063238
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		try
		{
			string text = _params[1].ToLower();
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = _params[0];
				if (!(text2 == "import"))
				{
					if (text2 == "export")
					{
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						File.WriteAllText(text, GameOptionsControls.Export());
					}
				}
				else if (File.Exists(text))
				{
					GameOptionsControls.Import(File.ReadAllText(text));
				}
			}
		}
		catch
		{
		}
	}
}
