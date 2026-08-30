using System;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C20 RID: 7200
	public class XblSandboxHelper
	{
		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x0600D5DA RID: 54746 RVA: 0x004CF8E4 File Offset: 0x004CDAE4
		public string SandboxId
		{
			get
			{
				string text = this.sandboxId;
				if (text == null)
				{
					Log.Error("[XBL] XblSandboxHelper SandboxId has not finished refreshing");
					return null;
				}
				return text;
			}
		}

		// Token: 0x0600D5DB RID: 54747 RVA: 0x004CF908 File Offset: 0x004CDB08
		public void RefreshSandboxId()
		{
			this.sandboxId = null;
			ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(this.<RefreshSandboxId>g__GetSandboxTask|3_0), null, null, true);
		}

		// Token: 0x0600D5DC RID: 54748 RVA: 0x004CF926 File Offset: 0x004CDB26
		public static EMatchmakingGroup SandboxIdToMatchmakingGroup(string sandboxId)
		{
			if (sandboxId == "CERT" || sandboxId == "CERT.DEBUG")
			{
				return EMatchmakingGroup.CertQA;
			}
			if (!(sandboxId == "RETAIL"))
			{
				return EMatchmakingGroup.Dev;
			}
			return EMatchmakingGroup.Retail;
		}

		// Token: 0x0600D5DD RID: 54749 RVA: 0x004CF956 File Offset: 0x004CDB56
		public static DLCEnvironmentFlags SandboxIdToDLCEnvironment(string sandboxId)
		{
			if (sandboxId == "CERT" || sandboxId == "CERT.DEBUG")
			{
				return DLCEnvironmentFlags.Cert;
			}
			if (!(sandboxId == "RETAIL"))
			{
				return DLCEnvironmentFlags.Dev;
			}
			return DLCEnvironmentFlags.Retail;
		}

		// Token: 0x0600D5DF RID: 54751 RVA: 0x004CF988 File Offset: 0x004CDB88
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <RefreshSandboxId>g__GetSandboxTask|3_0(ThreadManager.TaskInfo taskInfo)
		{
			string text;
			int num = SDK.XSystemGetXboxLiveSandboxId(out text);
			XblHelpers.LogHR(num, "XSystemGetXboxLiveSandboxId", false);
			if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num))
			{
				Log.Out("[XBL] retrieved sandbox id: " + text);
				this.sandboxId = text;
			}
		}

		// Token: 0x0400A2E7 RID: 41703
		[PublicizedFrom(EAccessModifier.Private)]
		public string sandboxId;
	}
}
