using System;

namespace Platform.EOS
{
	// Token: 0x02001D44 RID: 7492
	public static class AntiCheatCommon
	{
		// Token: 0x0600DDF0 RID: 56816 RVA: 0x004EE52C File Offset: 0x004EC72C
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

		// Token: 0x0600DDF1 RID: 56817 RVA: 0x004EE57C File Offset: 0x004EC77C
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

		// Token: 0x0600DDF2 RID: 56818 RVA: 0x004EE5BE File Offset: 0x004EC7BE
		public static IntPtr ClientInfoToIntPtr(ClientInfo _clientInfo)
		{
			return new IntPtr(_clientInfo.ClientNumber);
		}

		// Token: 0x0400A793 RID: 42899
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A794 RID: 42900
		public static bool NoEacCmdLine;

		// Token: 0x0400A795 RID: 42901
		public static bool DebugEacVerbose;

		// Token: 0x0400A796 RID: 42902
		public static readonly object LockObject = new object();
	}
}
