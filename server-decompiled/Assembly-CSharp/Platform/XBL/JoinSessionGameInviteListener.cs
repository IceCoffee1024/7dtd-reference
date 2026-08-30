using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C36 RID: 7222
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600D63B RID: 54843 RVA: 0x004D1756 File Offset: 0x004CF956
		public void Init(IPlatform _owner)
		{
			PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _platform)
			{
				XGameInviteRegistrationToken xgameInviteRegistrationToken;
				XblHelpers.Succeeded(SDK.XGameInviteRegisterForEvent(new XGameInviteEventCallback(this.InviteReceivedCallback), out xgameInviteRegistrationToken), "Register for invite event", true, true);
			};
		}

		// Token: 0x0600D63C RID: 54844 RVA: 0x004D1774 File Offset: 0x004CF974
		[return: TupleElementNames(new string[] { "invite", "password" })]
		public ValueTuple<string, string> TakePendingInvite()
		{
			object obj = this.inviteLock;
			ValueTuple<string, string> valueTuple;
			lock (obj)
			{
				string text = this.pendingInvite;
				this.pendingInvite = null;
				string text2 = this.pendingPassword;
				this.pendingPassword = null;
				valueTuple = new ValueTuple<string, string>(text, text2);
			}
			return valueTuple;
		}

		// Token: 0x0600D63D RID: 54845 RVA: 0x004D17D4 File Offset: 0x004CF9D4
		public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
		{
			yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
			yield break;
		}

		// Token: 0x0600D63E RID: 54846 RVA: 0x004D17F1 File Offset: 0x004CF9F1
		public string GetListenerIdentifier()
		{
			return "XBL";
		}

		// Token: 0x0600D63F RID: 54847 RVA: 0x004D17F8 File Offset: 0x004CF9F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void InviteReceivedCallback(IntPtr _, string _inviteuri)
		{
			Log.Out("[XBL] Invite received: '" + _inviteuri + "'");
			string text = this.ParseInviteUri(_inviteuri);
			if (text == null)
			{
				Log.Error("[XBL] Received invite but could not extract connect information");
				return;
			}
			object obj = this.inviteLock;
			lock (obj)
			{
				this.pendingInvite = text;
			}
		}

		// Token: 0x0600D640 RID: 54848 RVA: 0x004D1864 File Offset: 0x004CFA64
		[PublicizedFrom(EAccessModifier.Private)]
		public string ParseInviteUri(string _inviteUri)
		{
			Match match = JoinSessionGameInviteListener.msInviteUriMatcher.Match(_inviteUri);
			if (!match.Success)
			{
				return null;
			}
			string[] array = match.Groups[3].Value.Split('&', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('=', StringSplitOptions.None);
				if (array2[0].EqualsCaseInsensitive("connectionString"))
				{
					return array2[1];
				}
			}
			return null;
		}

		// Token: 0x0400A32C RID: 41772
		[PublicizedFrom(EAccessModifier.Private)]
		public object inviteLock = new object();

		// Token: 0x0400A32D RID: 41773
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A32E RID: 41774
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A32F RID: 41775
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex msInviteUriMatcher = new Regex("^ms-xbl-(\\w+):\\/\\/(\\w+)\\/?\\?(.*)$", RegexOptions.Compiled);
	}
}
