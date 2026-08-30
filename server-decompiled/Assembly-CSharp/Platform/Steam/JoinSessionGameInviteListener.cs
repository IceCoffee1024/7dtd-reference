using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C88 RID: 7304
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600D88B RID: 55435 RVA: 0x004DCF59 File Offset: 0x004DB159
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

		// Token: 0x0600D88C RID: 55436 RVA: 0x004DCF94 File Offset: 0x004DB194
		[return: TupleElementNames(new string[] { "invite", "password" })]
		public ValueTuple<string, string> TakePendingInvite()
		{
			string text = this.pendingInvite;
			this.pendingInvite = null;
			string text2 = this.pendingPassword;
			this.pendingPassword = null;
			return new ValueTuple<string, string>(text, text2);
		}

		// Token: 0x0600D88D RID: 55437 RVA: 0x004DCFC2 File Offset: 0x004DB1C2
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

		// Token: 0x0600D88E RID: 55438 RVA: 0x004DCFDF File Offset: 0x004DB1DF
		public string GetListenerIdentifier()
		{
			return "STM";
		}

		// Token: 0x0600D88F RID: 55439 RVA: 0x004DCFE6 File Offset: 0x004DB1E6
		public void SetLobby(CSteamID _lobbyId)
		{
			this.pendingInvite = "Lobby:" + _lobbyId.m_SteamID.ToString();
		}

		// Token: 0x0600D890 RID: 55440 RVA: 0x004DD004 File Offset: 0x004DB204
		[PublicizedFrom(EAccessModifier.Private)]
		public void Friends_GameServerChangeRequested(GameServerChangeRequested_t _value)
		{
			Log.Out("[Steamworks.NET] Friends_GameServerChangeRequested");
			this.pendingInvite = _value.m_rgchServer;
			this.pendingPassword = _value.m_rgchPassword;
		}

		// Token: 0x0400A469 RID: 42089
		[PublicizedFrom(EAccessModifier.Private)]
		public const string LobbyMarker = "Lobby:";

		// Token: 0x0400A46A RID: 42090
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A46B RID: 42091
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A46C RID: 42092
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameServerChangeRequested_t> m_friends_serverchange;
	}
}
