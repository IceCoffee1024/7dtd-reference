using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200028F RID: 655
[Preserve]
public class ConsoleCmdSetWaterValue : ConsoleCmdAbstract
{
	// Token: 0x170001FC RID: 508
	// (get) Token: 0x0600130A RID: 4874 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x00075D2C File Offset: 0x00073F2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setwatervalue", "swv" };
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x00075D44 File Offset: 0x00073F44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the water value for all flow-permitting blocks within the current selection area, specified in the range of 0 (empty) to 1 (full).";
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x00075D4B File Offset: 0x00073F4B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "'swv [0.0 - 1.0]' Sets water value between empty (0.0) and full (1.0) for all flow-permitting blocks within the current selection bounds. \nE.g. 'swv 0.5' will set all affected blocks to be half-full. \nBlocks which do not permit flow are unchanged.";
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x00075D54 File Offset: 0x00073F54
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		float num;
		if (_params.Count != 1 || !float.TryParse(_params[0], out num) || num < 0f || num > 1f)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		BlockToolSelection instance = BlockToolSelection.Instance;
		if (!instance.SelectionActive)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No selection active. Running this command requires an active selection box.");
			return;
		}
		BlockTools.CubeWaterRPC(GameManager.Instance, instance.SelectionStart, instance.SelectionEnd, new WaterValue((int)(num * 19500f)));
	}
}
