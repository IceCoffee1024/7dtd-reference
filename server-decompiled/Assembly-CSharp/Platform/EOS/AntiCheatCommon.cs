using System;

namespace Platform.EOS
{
	// Token: 0x02001D13 RID: 7443
	public static class AntiCheatCommon
	{
		// Token: 0x0600DCDC RID: 56540 RVA: 0x004EA98C File Offset: 0x004E8B8C
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

		// Token: 0x0600DCDD RID: 56541 RVA: 0x004EA9DC File Offset: 0x004E8BDC
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

		// Token: 0x0600DCDE RID: 56542 RVA: 0x004EAA1E File Offset: 0x004E8C1E
		public static IntPtr ClientInfoToIntPtr(ClientInfo _clientInfo)
		{
			return new IntPtr(_clientInfo.ClientNumber);
		}

		// Token: 0x0400A6AA RID: 42666
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A6AB RID: 42667
		public static bool NoEacCmdLine;

		// Token: 0x0400A6AC RID: 42668
		public static bool DebugEacVerbose;

		// Token: 0x0400A6AD RID: 42669
		public static readonly object LockObject = new object();
	}
}
