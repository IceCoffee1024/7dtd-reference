using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C99 RID: 7321
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600D8C8 RID: 55496 RVA: 0x004D9DE2 File Offset: 0x004D7FE2
		public void Init(IPlatform _owner)
		{
			PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _platform)
			{
				XGameInviteRegistrationToken xgameInviteRegistrationToken;
				XblHelpers.Succeeded(SDK.XGameInviteRegisterForEvent(new XGameInviteEventCallback(this.InviteReceivedCallback), out xgameInviteRegistrationToken), "Register for invite event", true, true);
			};
		}

		// Token: 0x0600D8C9 RID: 55497 RVA: 0x004D9E00 File Offset: 0x004D8000
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

		// Token: 0x0600D8CA RID: 55498 RVA: 0x004D9E60 File Offset: 0x004D8060
		public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
		{
			yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
			yield break;
		}

		// Token: 0x0600D8CB RID: 55499 RVA: 0x004D9E7D File Offset: 0x004D807D
		public string GetListenerIdentifier()
		{
			return "XBL";
		}

		// Token: 0x0600D8CC RID: 55500 RVA: 0x004D9E84 File Offset: 0x004D8084
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

		// Token: 0x0600D8CD RID: 55501 RVA: 0x004D9EF0 File Offset: 0x004D80F0
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

		// Token: 0x0400A4EA RID: 42218
		[PublicizedFrom(EAccessModifier.Private)]
		public object inviteLock = new object();

		// Token: 0x0400A4EB RID: 42219
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A4EC RID: 42220
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A4ED RID: 42221
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex msInviteUriMatcher = new Regex("^ms-xbl-(\\w+):\\/\\/(\\w+)\\/?\\?(.*)$", RegexOptions.Compiled);
	}
}
