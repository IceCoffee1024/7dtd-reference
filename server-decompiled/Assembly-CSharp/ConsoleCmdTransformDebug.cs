using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B9 RID: 697
[Preserve]
public class ConsoleCmdTransformDebug : ConsoleCmdAbstract
{
	// Token: 0x060013F4 RID: 5108 RVA: 0x00079DCD File Offset: 0x00077FCD
	public ConsoleCmdTransformDebug()
	{
		this.ToggleDebugging();
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x060013F5 RID: 5109 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00079DDB File Offset: 0x00077FDB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "transformdebug", "tdbg" };
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x00079DF3 File Offset: 0x00077FF3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Transform Debugging";
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x00079DFA File Offset: 0x00077FFA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "tdbg - Toggle Transform Debugging";
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x00079E01 File Offset: 0x00078001
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			this.ToggleDebugging();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.getHelp());
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x00079E24 File Offset: 0x00078024
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

	// Token: 0x04000D95 RID: 3477
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EMPTY_NAME = "TransformDebug";

	// Token: 0x04000D96 RID: 3478
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject m_empty;
}
