using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000299 RID: 665
[Preserve]
public class ConsoleCmdSignEditorDebug : ConsoleCmdAbstract
{
	// Token: 0x0600134A RID: 4938 RVA: 0x000765BE File Offset: 0x000747BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "signeditordebug", "sed" };
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x000765D6 File Offset: 0x000747D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles visibility of the Sign Editor debug panel. ";
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x000765DD File Offset: 0x000747DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "No params: Toggles visibility of the Sign Editor debug panel. ";
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x0600134E RID: 4942 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x000765E4 File Offset: 0x000747E4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		XUiC_SignEditorWindow.ShowDebugPanel = !XUiC_SignEditorWindow.ShowDebugPanel;
		Log.Out("[SignEditor] Set Sign Editor debug panel visibility to " + XUiC_SignEditorWindow.ShowDebugPanel.ToString() + ". Changes take effect the next time a layer is selected in the editor.");
	}
}
