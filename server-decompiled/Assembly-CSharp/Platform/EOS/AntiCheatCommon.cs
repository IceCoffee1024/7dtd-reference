using System;

namespace Platform.EOS
{
	// Token: 0x02001CE1 RID: 7393
	public static class AntiCheatCommon
	{
		// Token: 0x0600DB61 RID: 56161 RVA: 0x004E5E44 File Offset: 0x004E4044
		public static void Init()
		{
			if (AntiCheatCommon.initialized)
			{
				return;
			}
			string launchArgument = GameUtils.GetLaunchArgument("debugeac");
			AntiCheatCommon.DebugEacVerbose = launchArgument != null && launchArgument == "verbose";
			AntiCheatCommon.NoEacCmdLine = GameUtils.GetLaunchArgument("noeac") != null;
			AntiCheatCommon.initialized = true;
		}

		// Token: 0x0600DB62 RID: 56162 RVA: 0x004E5E94 File Offset: 0x004E4094
		public static ClientInfo IntPtrToClientInfo(IntPtr _ptr, string _messageIfNull = null)
		{
			int num = _ptr.ToInt32();
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForClientNumber(num);
			if (clientInfo == null && _messageIfNull != null)
			{
				Log.Error(_messageIfNull, new object[] { num });
			}
			return clientInfo;
		}

		// Token: 0x0600DB63 RID: 56163 RVA: 0x004E5ED6 File Offset: 0x004E40D6
		public static IntPtr ClientInfoToIntPtr(ClientInfo _clientInfo)
		{
			return new IntPtr(_clientInfo.ClientNumber);
		}

		// Token: 0x0400A5D5 RID: 42453
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A5D6 RID: 42454
		public static bool NoEacCmdLine;

		// Token: 0x0400A5D7 RID: 42455
		public static bool DebugEacVerbose;

		// Token: 0x0400A5D8 RID: 42456
		public static readonly object LockObject = new object();
	}
}
