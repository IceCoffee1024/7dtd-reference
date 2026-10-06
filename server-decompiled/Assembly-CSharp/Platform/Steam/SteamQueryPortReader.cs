using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CC3 RID: 7363
	public class SteamQueryPortReader
	{
		// Token: 0x14000135 RID: 309
		// (add) Token: 0x0600DA4B RID: 55883 RVA: 0x004E249C File Offset: 0x004E069C
		// (remove) Token: 0x0600DA4C RID: 55884 RVA: 0x004E24D4 File Offset: 0x004E06D4
		[method: PublicizedFrom(EAccessModifier.Private)]
		public event GameServerDetailsCallback GameServerDetailsEvent;

		// Token: 0x0600DA4D RID: 55885 RVA: 0x004E250C File Offset: 0x004E070C
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			if (this.matchmakingRulesResponse != null)
			{
				return;
			}
			this.matchmakingRulesResponse = new ISteamMatchmakingRulesResponse(new ISteamMatchmakingRulesResponse.RulesResponded(this.RulesResponded), new ISteamMatchmakingRulesResponse.RulesFailedToRespond(this.RulesFailedToRespond), new ISteamMatchmakingRulesResponse.RulesRefreshComplete(this.RulesRefreshComplete));
		}

		// Token: 0x0600DA4E RID: 55886 RVA: 0x004E2559 File Offset: 0x004E0759
		public void Disconnect()
		{
			if (this.rulesRequestHandle != HServerQuery.Invalid)
			{
				SteamMatchmakingServers.CancelServerQuery(this.rulesRequestHandle);
				this.rulesRequestHandle = HServerQuery.Invalid;
			}
			this.GameServerDetailsEvent = null;
		}

		// Token: 0x0600DA4F RID: 55887 RVA: 0x004E258A File Offset: 0x004E078A
		public void RegisterGameServerCallbacks(GameServerDetailsCallback _details)
		{
			this.GameServerDetailsEvent = _details;
		}

		// Token: 0x0600DA50 RID: 55888 RVA: 0x004E2593 File Offset: 0x004E0793
		[PublicizedFrom(EAccessModifier.Private)]
		public void RunGameServerDetailsEvent(GameServerInfo _info, bool _success)
		{
			GameServerDetailsCallback gameServerDetailsEvent = this.GameServerDetailsEvent;
			if (gameServerDetailsEvent == null)
			{
				return;
			}
			gameServerDetailsEvent(_info, _success);
		}

		// Token: 0x0600DA51 RID: 55889 RVA: 0x004E25A8 File Offset: 0x004E07A8
		public void GetGameServerInfo(GameServerInfo _gameInfo)
		{
			if (_gameInfo.IsLobby)
			{
				this.RunGameServerDetailsEvent(_gameInfo, true);
				return;
			}
			if (_gameInfo.IsNoResponse)
			{
				this.RunGameServerDetailsEvent(_gameInfo, true);
			}
			string text = _gameInfo.GetValue(GameInfoString.IP);
			long num;
			if (!long.TryParse(text.Replace(".", ""), out num))
			{
				try
				{
					IPHostEntry hostEntry = Dns.GetHostEntry(text);
					if (hostEntry.AddressList.Length == 0)
					{
						Log.Out("Steamworks.NET] No valid IP for server found");
						this.RunGameServerDetailsEvent(_gameInfo, false);
						return;
					}
					text = hostEntry.AddressList[0].ToString();
				}
				catch (SocketException ex)
				{
					string text2 = "Steamworks.NET] No such hostname: \"";
					string text3 = text;
					string text4 = "\": ";
					SocketException ex2 = ex;
					Log.Out(text2 + text3 + text4 + ((ex2 != null) ? ex2.ToString() : null));
					this.RunGameServerDetailsEvent(_gameInfo, false);
					return;
				}
			}
			SteamQueryPortReader.RulesRequest rulesRequest = new SteamQueryPortReader.RulesRequest
			{
				GameInfo = _gameInfo,
				Ip = NetworkUtils.ToInt(text),
				Port = (ushort)_gameInfo.GetValue(GameInfoInt.Port)
			};
			this.rulesRequests.Enqueue(rulesRequest);
			if (this.rulesRequestHandle == HServerQuery.Invalid)
			{
				this.StartNextRulesRequest();
			}
		}

		// Token: 0x0600DA52 RID: 55890 RVA: 0x004E26B8 File Offset: 0x004E08B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartNextRulesRequest()
		{
			this.currentRulesRequest = null;
			this.rulesRequestHandle = HServerQuery.Invalid;
			if (this.rulesRequests.Count > 0)
			{
				this.currentRulesRequest = this.rulesRequests.Dequeue();
				this.currentRulesRequest.GameInfoClone = new GameServerInfo(this.currentRulesRequest.GameInfo);
				this.rulesRequestHandle = SteamMatchmakingServers.ServerRules(this.currentRulesRequest.Ip, this.currentRulesRequest.Port, this.matchmakingRulesResponse);
			}
		}

		// Token: 0x0600DA53 RID: 55891 RVA: 0x004E2738 File Offset: 0x004E0938
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesFailedToRespond()
		{
			this.RunGameServerDetailsEvent(this.currentRulesRequest.GameInfo, false);
			this.StartNextRulesRequest();
		}

		// Token: 0x0600DA54 RID: 55892 RVA: 0x004E2754 File Offset: 0x004E0954
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesRefreshComplete()
		{
			if (!this.currentRulesRequest.DataErrors && this.currentRulesRequest.GameInfoClone.GetValue(GameInfoString.GameName).Length > 0)
			{
				this.currentRulesRequest.GameInfo.Merge(this.currentRulesRequest.GameInfoClone, this.currentRulesRequest.GameInfo.IsLAN ? EServerRelationType.LAN : EServerRelationType.Internet);
				this.RunGameServerDetailsEvent(this.currentRulesRequest.GameInfo, true);
			}
			else
			{
				if (this.currentRulesRequest.DataErrors)
				{
					this.currentRulesRequest.GameInfo.SetValue(GameInfoString.ServerDescription, Localization.Get("xuiServerBrowserFailedRetrievingData", false, null));
				}
				this.RunGameServerDetailsEvent(this.currentRulesRequest.GameInfo, false);
			}
			this.StartNextRulesRequest();
		}

		// Token: 0x0600DA55 RID: 55893 RVA: 0x004E2810 File Offset: 0x004E0A10
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesResponded(string _rule, string _value)
		{
			SteamQueryPortReader.RulesRequest rulesRequest = this.currentRulesRequest;
			if (rulesRequest.DataErrors)
			{
				return;
			}
			if (_rule.EqualsCaseInsensitive("gameinfo") || _rule.EqualsCaseInsensitive("ping"))
			{
				return;
			}
			if (rulesRequest.GameInfoClone.IsLAN && _rule.EqualsCaseInsensitive("ip"))
			{
				return;
			}
			if (!rulesRequest.GameInfoClone.ParseAny(_rule, _value))
			{
				rulesRequest.DataErrors = true;
			}
		}

		// Token: 0x0400A55C RID: 42332
		[PublicizedFrom(EAccessModifier.Private)]
		public ISteamMatchmakingRulesResponse matchmakingRulesResponse;

		// Token: 0x0400A55D RID: 42333
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<SteamQueryPortReader.RulesRequest> rulesRequests = new Queue<SteamQueryPortReader.RulesRequest>();

		// Token: 0x0400A55E RID: 42334
		[PublicizedFrom(EAccessModifier.Private)]
		public SteamQueryPortReader.RulesRequest currentRulesRequest;

		// Token: 0x0400A55F RID: 42335
		[PublicizedFrom(EAccessModifier.Private)]
		public HServerQuery rulesRequestHandle = HServerQuery.Invalid;

		// Token: 0x02001CC4 RID: 7364
		[PublicizedFrom(EAccessModifier.Private)]
		public class RulesRequest
		{
			// Token: 0x0400A560 RID: 42336
			public uint Ip;

			// Token: 0x0400A561 RID: 42337
			public ushort Port;

			// Token: 0x0400A562 RID: 42338
			public GameServerInfo GameInfo;

			// Token: 0x0400A563 RID: 42339
			public GameServerInfo GameInfoClone;

			// Token: 0x0400A564 RID: 42340
			public bool DataErrors;
		}
	}
}
