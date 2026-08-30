using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000286 RID: 646
[Preserve]
public class ConsoleCmdSetWaterValue : ConsoleCmdAbstract
{
	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060012F7 RID: 4855 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0007524C File Offset: 0x0007344C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "setwatervalue", "swv" };
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x00075264 File Offset: 0x00073464
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sets the water value for all flow-permitting blocks within the current selection area, specified in the range of 0 (empty) to 1 (full).";
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0007526B File Offset: 0x0007346B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "'swv [0.0 - 1.0]' Sets water value between empty (0.0) and full (1.0) for all flow-permitting blocks within the current selection bounds. \nE.g. 'swv 0.5' will set all affected blocks to be half-full. \nBlocks which do not permit flow are unchanged.";
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x00075274 File Offset: 0x00073474
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
