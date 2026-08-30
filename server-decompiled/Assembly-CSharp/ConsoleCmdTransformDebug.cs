using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B4 RID: 692
[Preserve]
public class ConsoleCmdTransformDebug : ConsoleCmdAbstract
{
	// Token: 0x060013F3 RID: 5107 RVA: 0x00079949 File Offset: 0x00077B49
	public ConsoleCmdTransformDebug()
	{
		this.ToggleDebugging();
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x060013F4 RID: 5108 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x060013F5 RID: 5109 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x00079957 File Offset: 0x00077B57
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "transformdebug", "tdbg" };
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x0007996F File Offset: 0x00077B6F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Transform Debugging";
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x00079976 File Offset: 0x00077B76
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "tdbg - Toggle Transform Debugging";
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x0007997D File Offset: 0x00077B7D
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			this.ToggleDebugging();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.getHelp());
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x000799A0 File Offset: 0x00077BA0
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

	// Token: 0x04000D9B RID: 3483
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EMPTY_NAME = "TransformDebug";

	// Token: 0x04000D9C RID: 3484
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject m_empty;
}
