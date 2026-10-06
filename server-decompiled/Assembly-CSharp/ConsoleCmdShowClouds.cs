using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000292 RID: 658
[Preserve]
public class ConsoleCmdShowClouds : ConsoleCmdAbstract
{
	// Token: 0x06001326 RID: 4902 RVA: 0x00076247 File Offset: 0x00074447
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showClouds" };
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x00076257 File Offset: 0x00074457
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Artist command to show one layer of clouds.";
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x0007625E File Offset: 0x0007445E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "type \"showClouds myCloudTexture\" where \"myCloudTexture\" is the name of the texture you want to see.\ntype \"showClouds\" to turn off this view.\nNote: cloud textures MUST be locasted at ./resources/textures/environment/spectrums/default\n";
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x00076265 File Offset: 0x00074465
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 0)
		{
			Resources.Load("Textures/Environment/Spectrums/default/" + _params[0], typeof(Texture));
		}
	}
}
