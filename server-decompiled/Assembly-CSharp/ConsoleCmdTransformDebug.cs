using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B7 RID: 695
[Preserve]
public class ConsoleCmdTransformDebug : ConsoleCmdAbstract
{
	// Token: 0x060013F0 RID: 5104 RVA: 0x00079BFD File Offset: 0x00077DFD
	public ConsoleCmdTransformDebug()
	{
		this.ToggleDebugging();
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x060013F1 RID: 5105 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060013F2 RID: 5106 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x00079C0B File Offset: 0x00077E0B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "transformdebug", "tdbg" };
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x00079C23 File Offset: 0x00077E23
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Transform Debugging";
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x00079C2A File Offset: 0x00077E2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "tdbg - Toggle Transform Debugging";
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x00079C31 File Offset: 0x00077E31
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			this.ToggleDebugging();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.getHelp());
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00079C54 File Offset: 0x00077E54
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleDebugging()
	{
		if (!this.m_empty)
		{
			Log.Out("Creating TransformDebug GameObject");
			this.m_empty = new GameObject("TransformDebug");
			this.m_empty.AddComponent<TransformDebug>();
			global::UnityEngine.Object.DontDestroyOnLoad(this.m_empty);
			return;
		}
		Log.Out("Destroying TransformDebug GameObject");
		global::UnityEngine.Object.Destroy(this.m_empty);
	}

	// Token: 0x04000D94 RID: 3476
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EMPTY_NAME = "TransformDebug";

	// Token: 0x04000D95 RID: 3477
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject m_empty;
}
