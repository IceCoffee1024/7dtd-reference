using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B0 RID: 688
[Preserve]
public class ConsoleCmdTransformDebug : ConsoleCmdAbstract
{
	// Token: 0x060013E1 RID: 5089 RVA: 0x000792ED File Offset: 0x000774ED
	public ConsoleCmdTransformDebug()
	{
		this.ToggleDebugging();
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x060013E3 RID: 5091 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x000792FB File Offset: 0x000774FB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "transformdebug", "tdbg" };
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x00079313 File Offset: 0x00077513
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Transform Debugging";
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x0007931A File Offset: 0x0007751A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "tdbg - Toggle Transform Debugging";
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x00079321 File Offset: 0x00077521
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			this.ToggleDebugging();
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.getHelp());
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x00079344 File Offset: 0x00077544
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

	// Token: 0x04000D8A RID: 3466
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EMPTY_NAME = "TransformDebug";

	// Token: 0x04000D8B RID: 3467
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject m_empty;
}
