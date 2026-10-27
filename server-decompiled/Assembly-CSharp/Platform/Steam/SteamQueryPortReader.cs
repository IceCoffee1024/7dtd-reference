using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CF4 RID: 7412
	public class SteamQueryPortReader
	{
		// Token: 0x14000135 RID: 309
		// (add) Token: 0x0600DB5F RID: 56159 RVA: 0x004E603C File Offset: 0x004E423C
		// (remove) Token: 0x0600DB60 RID: 56160 RVA: 0x004E6074 File Offset: 0x004E4274
		[method: PublicizedFrom(EAccessModifier.Private)]
		public event GameServerDetailsCallback GameServerDetailsEvent;

		// Token: 0x0600DB61 RID: 56161 RVA: 0x004E60AC File Offset: 0x004E42AC
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

		// Token: 0x0600DB62 RID: 56162 RVA: 0x004E60F9 File Offset: 0x004E42F9
		public void Disconnect()
		{
			if (this.rulesRequestHandle != HServerQuery.Invalid)
			{
				SteamMatchmakingServers.CancelServerQuery(this.rulesRequestHandle);
				this.rulesRequestHandle = HServerQuery.Invalid;
			}
			this.GameServerDetailsEvent = null;
		}

		// Token: 0x0600DB63 RID: 56163 RVA: 0x004E612A File Offset: 0x004E432A
		public void RegisterGameServerCallbacks(GameServerDetailsCallback _details)
		{
			this.GameServerDetailsEvent = _details;
		}

		// Token: 0x0600DB64 RID: 56164 RVA: 0x004E6133 File Offset: 0x004E4333
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

		// Token: 0x0600DB65 RID: 56165 RVA: 0x004E6148 File Offset: 0x004E4348
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

		// Token: 0x0600DB66 RID: 56166 RVA: 0x004E6258 File Offset: 0x004E4458
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

		// Token: 0x0600DB67 RID: 56167 RVA: 0x004E62D8 File Offset: 0x004E44D8
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesFailedToRespond()
		{
			this.RunGameServerDetailsEvent(this.currentRulesRequest.GameInfo, false);
			this.StartNextRulesRequest();
		}

		// Token: 0x0600DB68 RID: 56168 RVA: 0x004E62F4 File Offset: 0x004E44F4
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

		// Token: 0x0600DB69 RID: 56169 RVA: 0x004E63B0 File Offset: 0x004E45B0
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

		// Token: 0x0400A645 RID: 42565
		[PublicizedFrom(EAccessModifier.Private)]
		public ISteamMatchmakingRulesResponse matchmakingRulesResponse;

		// Token: 0x0400A646 RID: 42566
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<SteamQueryPortReader.RulesRequest> rulesRequests = new Queue<SteamQueryPortReader.RulesRequest>();

		// Token: 0x0400A647 RID: 42567
		[PublicizedFrom(EAccessModifier.Private)]
		public SteamQueryPortReader.RulesRequest currentRulesRequest;

		// Token: 0x0400A648 RID: 42568
		[PublicizedFrom(EAccessModifier.Private)]
		public HServerQuery rulesRequestHandle = HServerQuery.Invalid;

		// Token: 0x02001CF5 RID: 7413
		[PublicizedFrom(EAccessModifier.Private)]
		public class RulesRequest
		{
			// Token: 0x0400A649 RID: 42569
			public uint Ip;

			// Token: 0x0400A64A RID: 42570
			public ushort Port;

			// Token: 0x0400A64B RID: 42571
			public GameServerInfo GameInfo;

			// Token: 0x0400A64C RID: 42572
			public GameServerInfo GameInfoClone;

			// Token: 0x0400A64D RID: 42573
			public bool DataErrors;
		}
	}
}
