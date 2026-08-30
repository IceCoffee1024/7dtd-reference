using System;
using System.Runtime.CompilerServices;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001BF9 RID: 7161
	public class XblSandboxHelper
	{
		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x0600D44C RID: 54348 RVA: 0x004C94F0 File Offset: 0x004C76F0
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

		// Token: 0x0600D44D RID: 54349 RVA: 0x004C9514 File Offset: 0x004C7714
		public void RefreshSandboxId()
		{
			this.sandboxId = null;
			ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(this.<RefreshSandboxId>g__GetSandboxTask|3_0), null, null, true);
		}

		// Token: 0x0600D44E RID: 54350 RVA: 0x004C9532 File Offset: 0x004C7732
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

		// Token: 0x0600D44F RID: 54351 RVA: 0x004C9562 File Offset: 0x004C7762
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

		// Token: 0x0600D451 RID: 54353 RVA: 0x004C9594 File Offset: 0x004C7794
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

		// Token: 0x0400A1B6 RID: 41398
		[PublicizedFrom(EAccessModifier.Private)]
		public string sandboxId;
	}
}
