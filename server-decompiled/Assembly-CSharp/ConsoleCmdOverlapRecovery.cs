using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000252 RID: 594
[Preserve]
public class ConsoleCmdOverlapRecovery : ConsoleCmdAbstract
{
	// Token: 0x060011A1 RID: 4513 RVA: 0x0006F48A File Offset: 0x0006D68A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "overlap" };
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060011A2 RID: 4514 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x0006F49A File Offset: 0x0006D69A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle LocalPlayer's Character Controller Overlap Recovery";
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x0003223F File Offset: 0x0003043F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x0006F4A4 File Offset: 0x0006D6A4
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
