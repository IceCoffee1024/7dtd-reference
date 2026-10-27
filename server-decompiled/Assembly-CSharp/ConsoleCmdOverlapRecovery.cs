using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000254 RID: 596
[Preserve]
public class ConsoleCmdOverlapRecovery : ConsoleCmdAbstract
{
	// Token: 0x060011A5 RID: 4517 RVA: 0x0006F65A File Offset: 0x0006D85A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "overlap" };
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x0006F66A File Offset: 0x0006D86A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle LocalPlayer's Character Controller Overlap Recovery";
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x0006F674 File Offset: 0x0006D874
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
