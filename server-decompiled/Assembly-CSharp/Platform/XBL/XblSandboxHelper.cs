using System;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C83 RID: 7299
	public class XblSandboxHelper
	{
		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x0600D866 RID: 55398 RVA: 0x004D7EF4 File Offset: 0x004D60F4
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

		// Token: 0x0600D867 RID: 55399 RVA: 0x004D7F18 File Offset: 0x004D6118
		public void RefreshSandboxId()
		{
			this.sandboxId = null;
			ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(this.<RefreshSandboxId>g__GetSandboxTask|3_0), null, null, true);
		}

		// Token: 0x0600D868 RID: 55400 RVA: 0x004D7F36 File Offset: 0x004D6136
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

		// Token: 0x0600D869 RID: 55401 RVA: 0x004D7F66 File Offset: 0x004D6166
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

		// Token: 0x0600D86B RID: 55403 RVA: 0x004D7F98 File Offset: 0x004D6198
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

		// Token: 0x0400A4A5 RID: 42149
		[PublicizedFrom(EAccessModifier.Private)]
		public string sandboxId;
	}
}
