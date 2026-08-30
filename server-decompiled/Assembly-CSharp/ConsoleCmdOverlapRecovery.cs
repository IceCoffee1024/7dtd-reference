using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200024B RID: 587
[Preserve]
public class ConsoleCmdOverlapRecovery : ConsoleCmdAbstract
{
	// Token: 0x06001192 RID: 4498 RVA: 0x0006EB82 File Offset: 0x0006CD82
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "overlap" };
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06001193 RID: 4499 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x0006EB92 File Offset: 0x0006CD92
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle LocalPlayer's Character Controller Overlap Recovery";
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0006EB9C File Offset: 0x0006CD9C
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
