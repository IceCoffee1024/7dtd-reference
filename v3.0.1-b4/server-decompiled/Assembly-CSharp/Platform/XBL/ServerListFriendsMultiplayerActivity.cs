using System;
using System.Collections.Generic;
using System.Threading;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001BD9 RID: 7129
	public class ServerListFriendsMultiplayerActivity : IServerListInterface
	{
		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x0600D37A RID: 54138 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x0600D37B RID: 54139 RVA: 0x004C693E File Offset: 0x004C4B3E
		public bool IsRefreshing
		{
			get
			{
				return this.isLoadingFriendsList || this.activitySearchCount > 0 || this.sessionSearchCount > 0;
			}
		}

		// Token: 0x0600D37C RID: 54140 RVA: 0x004C695C File Offset: 0x004C4B5C
		public void Init(IPlatform _owner)
		{
			this.user = (User)_owner.User;
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			this.serverLookup = ((crossplatformPlatform != null) ? crossplatformPlatform.ServerLookupInterface : null);
			if (this.serverLookup == null)
			{
				Log.Error("[XBL] no crossplatform server lookup interface provided, friends session search is not possible");
				return;
			}
		}

		// Token: 0x0600D37D RID: 54141 RVA: 0x004C6999 File Offset: 0x004C4B99
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			if (this.serverLookup == null)
			{
				return;
			}
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x0600D37E RID: 54142 RVA: 0x004C69AC File Offset: 0x004C4BAC
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			if (this.serverLookup == null)
			{
				return;
			}
			XblSocialManagerUserGroupHandle xblSocialManagerUserGroupHandle;
			if (!this.user.SocialManager.TryCreateUserGroup(XblPresenceFilter.TitleOnline, XblRelationshipFilter.Friends, new SocialManagerXbl.UserGroupMembersChanged(this.OnlineFriendsListUpdated), out xblSocialManagerUserGroupHandle))
			{
				Log.Error("[XBL] could not create friends user group, friends session search will fail");
				return;
			}
			Log.Out("[XBL] ServerListFriendsMultiplayerActivity starting search");
			this.userGroupHandle = xblSocialManagerUserGroupHandle;
			this.isLoadingFriendsList = true;
		}

		// Token: 0x0600D37F RID: 54143 RVA: 0x004C6A07 File Offset: 0x004C4C07
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnlineFriendsListUpdated(ulong[] _users)
		{
			this.isLoadingFriendsList = false;
			if (_users == null || _users.Length == 0)
			{
				return;
			}
			Interlocked.Increment(ref this.activitySearchCount);
			this.user.MultiplayerActivityQueryManager.GetActivityAsync(_users, new MultiplayerActivityQueryManager.OnGetActivityComplete(this.OnActivitiesRetrieved));
		}

		// Token: 0x0600D380 RID: 54144 RVA: 0x004C6A44 File Offset: 0x004C4C44
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnActivitiesRetrieved(ulong[] _searchedXuids, List<XblMultiplayerActivityInfo> _results)
		{
			int num = 0;
			using (List<XblMultiplayerActivityInfo>.Enumerator enumerator = _results.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!string.IsNullOrEmpty(enumerator.Current.ConnectionString))
					{
						num++;
					}
				}
			}
			if (num == 0 || this.gameServerFoundCallback == null)
			{
				Interlocked.Decrement(ref this.activitySearchCount);
				return;
			}
			ThreadManager.AddSingleTaskMainThread("SearchXboxActivitySessions", delegate(object param)
			{
				foreach (XblMultiplayerActivityInfo xblMultiplayerActivityInfo in _results)
				{
					if (xblMultiplayerActivityInfo.JoinRestriction != XblMultiplayerActivityJoinRestriction.InviteOnly && !string.IsNullOrEmpty(xblMultiplayerActivityInfo.ConnectionString))
					{
						GameServerInfo gameServerInfo = new GameServerInfo();
						gameServerInfo.SetValue(GameInfoString.UniqueId, xblMultiplayerActivityInfo.ConnectionString);
						Interlocked.Increment(ref this.sessionSearchCount);
						this.serverLookup.GetSingleServerDetails(gameServerInfo, EServerRelationType.Friends, new GameServerFoundCallback(this.OnServerFound));
					}
				}
				Interlocked.Decrement(ref this.activitySearchCount);
			}, null);
		}

		// Token: 0x0600D381 RID: 54145 RVA: 0x004C6AE4 File Offset: 0x004C4CE4
		public void OnServerFound(IPlatform _sourcePlatform, GameServerInfo _info, EServerRelationType _source)
		{
			if (_info == null)
			{
				return;
			}
			_info.IsFriends = true;
			GameServerFoundCallback gameServerFoundCallback = this.gameServerFoundCallback;
			if (gameServerFoundCallback != null)
			{
				gameServerFoundCallback(_sourcePlatform, _info, _source);
			}
			Interlocked.Decrement(ref this.sessionSearchCount);
		}

		// Token: 0x0600D382 RID: 54146 RVA: 0x004C6B11 File Offset: 0x004C4D11
		public void StopSearch()
		{
			if (this.userGroupHandle != null)
			{
				this.user.SocialManager.DestroyUserGroup(this.userGroupHandle);
			}
			this.userGroupHandle = null;
		}

		// Token: 0x0600D383 RID: 54147 RVA: 0x004C6B3E File Offset: 0x004C4D3E
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600D384 RID: 54148 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400A0CE RID: 41166
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A0CF RID: 41167
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;

		// Token: 0x0400A0D0 RID: 41168
		[PublicizedFrom(EAccessModifier.Private)]
		public IServerListInterface serverLookup;

		// Token: 0x0400A0D1 RID: 41169
		[PublicizedFrom(EAccessModifier.Private)]
		public XblSocialManagerUserGroupHandle userGroupHandle;

		// Token: 0x0400A0D2 RID: 41170
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isLoadingFriendsList;

		// Token: 0x0400A0D3 RID: 41171
		[PublicizedFrom(EAccessModifier.Private)]
		public int activitySearchCount;

		// Token: 0x0400A0D4 RID: 41172
		[PublicizedFrom(EAccessModifier.Private)]
		public int sessionSearchCount;
	}
}
