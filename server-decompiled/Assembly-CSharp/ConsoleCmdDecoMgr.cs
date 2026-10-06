using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x0200020F RID: 527
[Preserve]
public class ConsoleCmdDecoMgr : ConsoleCmdAbstract
{
	// Token: 0x06000FF9 RID: 4089 RVA: 0x00066833 File Offset: 0x00064A33
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "decomgr" };
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x00066843 File Offset: 0x00064A43
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "\"decomgr\": Saves a debug texture visualising the DecoOccupiedMap.\n\"decomgr state\": Saves a debug texture visualising the location/state of all of the DecoObjects saved in decorations.7dtd.";
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000FFB RID: 4091 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0006684C File Offset: 0x00064A4C
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
