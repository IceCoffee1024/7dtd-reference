using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200028D RID: 653
[Preserve]
public class ConsoleCmdSetWaterValue : ConsoleCmdAbstract
{
	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06001306 RID: 4870 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x00075B5C File Offset: 0x00073D5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setwatervalue", "swv" };
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x00075B74 File Offset: 0x00073D74
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the water value for all flow-permitting blocks within the current selection area, specified in the range of 0 (empty) to 1 (full).";
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x00075B7B File Offset: 0x00073D7B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "'swv [0.0 - 1.0]' Sets water value between empty (0.0) and full (1.0) for all flow-permitting blocks within the current selection bounds. \nE.g. 'swv 0.5' will set all affected blocks to be half-full. \nBlocks which do not permit flow are unchanged.";
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00075B84 File Offset: 0x00073D84
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
