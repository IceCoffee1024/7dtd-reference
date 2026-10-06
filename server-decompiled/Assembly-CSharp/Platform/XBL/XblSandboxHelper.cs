using System;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C52 RID: 7250
	public class XblSandboxHelper
	{
		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x0600D752 RID: 55122 RVA: 0x004D43CC File Offset: 0x004D25CC
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

		// Token: 0x0600D753 RID: 55123 RVA: 0x004D43F0 File Offset: 0x004D25F0
		public void RefreshSandboxId()
		{
			this.sandboxId = null;
			ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(this.<RefreshSandboxId>g__GetSandboxTask|3_0), null, null, true);
		}

		// Token: 0x0600D754 RID: 55124 RVA: 0x004D440E File Offset: 0x004D260E
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

		// Token: 0x0600D755 RID: 55125 RVA: 0x004D443E File Offset: 0x004D263E
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

		// Token: 0x0600D757 RID: 55127 RVA: 0x004D4470 File Offset: 0x004D2670
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

		// Token: 0x0400A3BC RID: 41916
		[PublicizedFrom(EAccessModifier.Private)]
		public string sandboxId;
	}
}
