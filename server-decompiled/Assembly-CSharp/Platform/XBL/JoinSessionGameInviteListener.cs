using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C68 RID: 7272
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600D7B4 RID: 55220 RVA: 0x004D6292 File Offset: 0x004D4492
		public void Init(IPlatform _owner)
		{
			PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _platform)
			{
				XGameInviteRegistrationToken xgameInviteRegistrationToken;
				XblHelpers.Succeeded(SDK.XGameInviteRegisterForEvent(new XGameInviteEventCallback(this.InviteReceivedCallback), out xgameInviteRegistrationToken), "Register for invite event", true, true);
			};
		}

		// Token: 0x0600D7B5 RID: 55221 RVA: 0x004D62B0 File Offset: 0x004D44B0
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

		// Token: 0x0600D7B6 RID: 55222 RVA: 0x004D6310 File Offset: 0x004D4510
		public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
		{
			yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
			yield break;
		}

		// Token: 0x0600D7B7 RID: 55223 RVA: 0x004D632D File Offset: 0x004D452D
		public string GetListenerIdentifier()
		{
			return "XBL";
		}

		// Token: 0x0600D7B8 RID: 55224 RVA: 0x004D6334 File Offset: 0x004D4534
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

		// Token: 0x0600D7B9 RID: 55225 RVA: 0x004D63A0 File Offset: 0x004D45A0
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

		// Token: 0x0400A401 RID: 41985
		[PublicizedFrom(EAccessModifier.Private)]
		public object inviteLock = new object();

		// Token: 0x0400A402 RID: 41986
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A403 RID: 41987
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A404 RID: 41988
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex msInviteUriMatcher = new Regex("^ms-xbl-(\\w+):\\/\\/(\\w+)\\/?\\?(.*)$", RegexOptions.Compiled);
	}
}
