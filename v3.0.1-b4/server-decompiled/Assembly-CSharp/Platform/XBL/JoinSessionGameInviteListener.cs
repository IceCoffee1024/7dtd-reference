using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C0F RID: 7183
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600D4AB RID: 54443 RVA: 0x004CB332 File Offset: 0x004C9532
		public void Init(IPlatform _owner)
		{
			PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _platform)
			{
				XGameInviteRegistrationToken xgameInviteRegistrationToken;
				XblHelpers.Succeeded(SDK.XGameInviteRegisterForEvent(new XGameInviteEventCallback(this.InviteReceivedCallback), out xgameInviteRegistrationToken), "Register for invite event", true, true);
			};
		}

		// Token: 0x0600D4AC RID: 54444 RVA: 0x004CB350 File Offset: 0x004C9550
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

		// Token: 0x0600D4AD RID: 54445 RVA: 0x004CB3B0 File Offset: 0x004C95B0
		public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
		{
			yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
			yield break;
		}

		// Token: 0x0600D4AE RID: 54446 RVA: 0x004CB3CD File Offset: 0x004C95CD
		public string GetListenerIdentifier()
		{
			return "XBL";
		}

		// Token: 0x0600D4AF RID: 54447 RVA: 0x004CB3D4 File Offset: 0x004C95D4
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

		// Token: 0x0600D4B0 RID: 54448 RVA: 0x004CB440 File Offset: 0x004C9640
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

		// Token: 0x0400A1FB RID: 41467
		[PublicizedFrom(EAccessModifier.Private)]
		public object inviteLock = new object();

		// Token: 0x0400A1FC RID: 41468
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A1FD RID: 41469
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A1FE RID: 41470
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex msInviteUriMatcher = new Regex("^ms-xbl-(\\w+):\\/\\/(\\w+)\\/?\\?(.*)$", RegexOptions.Compiled);
	}
}
