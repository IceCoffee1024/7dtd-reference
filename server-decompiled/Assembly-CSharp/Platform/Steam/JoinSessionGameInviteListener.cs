using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CEB RID: 7403
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600DB19 RID: 56089 RVA: 0x004E5641 File Offset: 0x004E3841
		public void Init(IPlatform _owner)
		{
			if (this.m_friends_serverchange == null)
			{
				this.m_friends_serverchange = Callback<GameServerChangeRequested_t>.Create(new Callback<GameServerChangeRequested_t>.DispatchDelegate(this.Friends_GameServerChangeRequested));
			}
			_owner.Api.ClientApiInitialized += delegate
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				for (int i = 0; i < commandLineArgs.Length - 1; i++)
				{
					ulong num;
					if (commandLineArgs[i] == "+connect_lobby" && ulong.TryParse(commandLineArgs[i + 1], out num))
					{
						this.SetLobby(new CSteamID(num));
					}
				}
			};
		}

		// Token: 0x0600DB1A RID: 56090 RVA: 0x004E567C File Offset: 0x004E387C
		[return: TupleElementNames(new string[] { "invite", "password" })]
		public ValueTuple<string, string> TakePendingInvite()
		{
			string text = this.pendingInvite;
			this.pendingInvite = null;
			string text2 = this.pendingPassword;
			this.pendingPassword = null;
			return new ValueTuple<string, string>(text, text2);
		}

		// Token: 0x0600DB1B RID: 56091 RVA: 0x004E56AA File Offset: 0x004E38AA
		public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
		{
			if (string.IsNullOrEmpty(_invite))
			{
				if (_onFinished != null)
				{
					_onFinished(false);
				}
				yield break;
			}
			if (_invite.StartsWith("Lobby:"))
			{
				ILobbyHost lobbyHost = PlatformManager.NativePlatform.LobbyHost;
				if (lobbyHost != null)
				{
					lobbyHost.JoinLobby(_invite.Substring("Lobby:".Length), null);
				}
				if (_onFinished != null)
				{
					_onFinished(true);
				}
				yield break;
			}
			string[] array = _invite.Split(':', StringSplitOptions.None);
			string text = "";
			int num = 0;
			if (array.Length == 2)
			{
				text = array[0];
				num = Convert.ToInt32(array[1]);
			}
			yield return InviteManager.HandleIpPortInvite(text, num, _password, _onFinished);
			yield break;
		}

		// Token: 0x0600DB1C RID: 56092 RVA: 0x004E56C7 File Offset: 0x004E38C7
		public string GetListenerIdentifier()
		{
			return "STM";
		}

		// Token: 0x0600DB1D RID: 56093 RVA: 0x004E56CE File Offset: 0x004E38CE
		public void SetLobby(CSteamID _lobbyId)
		{
			this.pendingInvite = "Lobby:" + _lobbyId.m_SteamID.ToString();
		}

		// Token: 0x0600DB1E RID: 56094 RVA: 0x004E56EC File Offset: 0x004E38EC
		[PublicizedFrom(EAccessModifier.Private)]
		public void Friends_GameServerChangeRequested(GameServerChangeRequested_t _value)
		{
			Log.Out("[Steamworks.NET] Friends_GameServerChangeRequested");
			this.pendingInvite = _value.m_rgchServer;
			this.pendingPassword = _value.m_rgchPassword;
		}

		// Token: 0x0400A627 RID: 42535
		[PublicizedFrom(EAccessModifier.Private)]
		public const string LobbyMarker = "Lobby:";

		// Token: 0x0400A628 RID: 42536
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A629 RID: 42537
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A62A RID: 42538
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameServerChangeRequested_t> m_friends_serverchange;
	}
}
