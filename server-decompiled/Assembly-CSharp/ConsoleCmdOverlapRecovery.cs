using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200024F RID: 591
[Preserve]
public class ConsoleCmdOverlapRecovery : ConsoleCmdAbstract
{
	// Token: 0x060011A4 RID: 4516 RVA: 0x0006F1DE File Offset: 0x0006D3DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "overlap" };
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060011A5 RID: 4517 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x0006F1EE File Offset: 0x0006D3EE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle LocalPlayer's Character Controller Overlap Recovery";
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x00032147 File Offset: 0x00030347
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x0006F1F8 File Offset: 0x0006D3F8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		CharacterControllerUnity characterControllerUnity = ((primaryPlayer != null) ? primaryPlayer.m_characterController : null) as CharacterControllerUnity;
		if (characterControllerUnity != null)
		{
			characterControllerUnity.enableOverlapRecovery = !characterControllerUnity.enableOverlapRecovery;
			Log.Out(string.Format("CharacterController.enableOverlapRecovery set to {0}", characterControllerUnity.enableOverlapRecovery));
		}
	}
}
