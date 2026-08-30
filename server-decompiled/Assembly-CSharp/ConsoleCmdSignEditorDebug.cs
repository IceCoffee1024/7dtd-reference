using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000292 RID: 658
[Preserve]
public class ConsoleCmdSignEditorDebug : ConsoleCmdAbstract
{
	// Token: 0x0600133B RID: 4923 RVA: 0x00075CAE File Offset: 0x00073EAE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "signeditordebug", "sed" };
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x00075CC6 File Offset: 0x00073EC6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles visibility of the Sign Editor debug panel. ";
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00075CCD File Offset: 0x00073ECD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "No params: Toggles visibility of the Sign Editor debug panel. ";
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x0600133E RID: 4926 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x0600133F RID: 4927 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x00075CD4 File Offset: 0x00073ED4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		XUiC_SignEditorWindow.ShowDebugPanel = !XUiC_SignEditorWindow.ShowDebugPanel;
		Log.Out("[SignEditor] Set Sign Editor debug panel visibility to " + XUiC_SignEditorWindow.ShowDebugPanel.ToString() + ". Changes take effect the next time a layer is selected in the editor.");
	}
}
