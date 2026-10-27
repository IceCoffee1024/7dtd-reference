using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200029B RID: 667
[Preserve]
public class ConsoleCmdSignEditorDebug : ConsoleCmdAbstract
{
	// Token: 0x0600134E RID: 4942 RVA: 0x0007678E File Offset: 0x0007498E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "signeditordebug", "sed" };
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x000767A6 File Offset: 0x000749A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles visibility of the Sign Editor debug panel. ";
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x000767AD File Offset: 0x000749AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "No params: Toggles visibility of the Sign Editor debug panel. ";
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06001351 RID: 4945 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06001352 RID: 4946 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x000767B4 File Offset: 0x000749B4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		XUiC_SignEditorWindow.ShowDebugPanel = !XUiC_SignEditorWindow.ShowDebugPanel;
		Log.Out("[SignEditor] Set Sign Editor debug panel visibility to " + XUiC_SignEditorWindow.ShowDebugPanel.ToString() + ". Changes take effect the next time a layer is selected in the editor.");
	}
}
