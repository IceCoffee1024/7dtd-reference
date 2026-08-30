using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200028B RID: 651
[Preserve]
public class ConsoleCmdShowClouds : ConsoleCmdAbstract
{
	// Token: 0x06001317 RID: 4887 RVA: 0x00075937 File Offset: 0x00073B37
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showClouds" };
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x00075947 File Offset: 0x00073B47
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Artist command to show one layer of clouds.";
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x0007594E File Offset: 0x00073B4E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "type \"showClouds myCloudTexture\" where \"myCloudTexture\" is the name of the texture you want to see.\ntype \"showClouds\" to turn off this view.\nNote: cloud textures MUST be locasted at ./resources/textures/environment/spectrums/default\n";
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00075955 File Offset: 0x00073B55
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 0)
		{
			Resources.Load("Textures/Environment/Spectrums/default/" + _params[0], typeof(Texture));
		}
	}
}
