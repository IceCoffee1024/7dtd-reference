using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000200 RID: 512
[Preserve]
public class ConsoleCmdConfig : ConsoleCmdAbstract
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x00065720 File Offset: 0x00063920
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "config" };
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x00065730 File Offset: 0x00063930
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Import/export config data from/to external file";
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00065737 File Offset: 0x00063937
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Imports/exports config data from/to external file\nUsage:\n   config import [filename]\n   config export [filename]\n";
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x00065740 File Offset: 0x00063940
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
