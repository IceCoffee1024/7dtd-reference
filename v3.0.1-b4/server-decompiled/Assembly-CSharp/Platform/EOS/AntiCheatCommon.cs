using System;

namespace Platform.EOS
{
	// Token: 0x02001CBA RID: 7354
	public static class AntiCheatCommon
	{
		// Token: 0x0600D9C6 RID: 55750 RVA: 0x004DF830 File Offset: 0x004DDA30
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

		// Token: 0x0600D9C7 RID: 55751 RVA: 0x004DF880 File Offset: 0x004DDA80
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

		// Token: 0x0600D9C8 RID: 55752 RVA: 0x004DF8C2 File Offset: 0x004DDAC2
		public static IntPtr ClientInfoToIntPtr(ClientInfo _clientInfo)
		{
			return new IntPtr(_clientInfo.ClientNumber);
		}

		// Token: 0x0400A4A2 RID: 42146
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A4A3 RID: 42147
		public static bool NoEacCmdLine;

		// Token: 0x0400A4A4 RID: 42148
		public static bool DebugEacVerbose;

		// Token: 0x0400A4A5 RID: 42149
		public static readonly object LockObject = new object();
	}
}
