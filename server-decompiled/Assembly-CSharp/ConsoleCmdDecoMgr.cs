using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000211 RID: 529
[Preserve]
public class ConsoleCmdDecoMgr : ConsoleCmdAbstract
{
	// Token: 0x06000FFD RID: 4093 RVA: 0x00066A03 File Offset: 0x00064C03
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "decomgr" };
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x00066A13 File Offset: 0x00064C13
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "\"decomgr\": Saves a debug texture visualising the DecoOccupiedMap.\n\"decomgr state\": Saves a debug texture visualising the location/state of all of the DecoObjects saved in decorations.7dtd.";
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000FFF RID: 4095 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06001000 RID: 4096 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x00066A1C File Offset: 0x00064C1C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count > 0 && _params[0] == "state")
		{
			DecoManager.Instance.SaveStateDebugTexture(Path.Join(GameIO.GetApplicationTempPath(), "decostate.png"));
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Saved decostate.png to temp directory.");
			return;
		}
		DecoManager.Instance.SaveDebugTexture(Path.Join(GameIO.GetApplicationTempPath(), "deco.png"), false);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Saved deco.png to temp directory.");
	}
}
