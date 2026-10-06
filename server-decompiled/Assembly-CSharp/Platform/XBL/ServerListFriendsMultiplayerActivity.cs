using System;
using System.Collections.Generic;
using System.Threading;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C32 RID: 7218
	public class ServerListFriendsMultiplayerActivity : IServerListInterface
	{
		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x0600D680 RID: 54912 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x0600D681 RID: 54913 RVA: 0x004D181A File Offset: 0x004CFA1A
		public bool IsRefreshing
		{
			get
			{
				return this.isLoadingFriendsList || this.activitySearchCount > 0 || this.sessionSearchCount > 0;
			}
		}

		// Token: 0x0600D682 RID: 54914 RVA: 0x004D1838 File Offset: 0x004CFA38
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

		// Token: 0x0600D683 RID: 54915 RVA: 0x004D1875 File Offset: 0x004CFA75
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			if (this.serverLookup == null)
			{
				return;
			}
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x0600D684 RID: 54916 RVA: 0x004D1888 File Offset: 0x004CFA88
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

		// Token: 0x0600D685 RID: 54917 RVA: 0x004D18E3 File Offset: 0x004CFAE3
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

		// Token: 0x0600D686 RID: 54918 RVA: 0x004D1920 File Offset: 0x004CFB20
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

		// Token: 0x0600D687 RID: 54919 RVA: 0x004D19C0 File Offset: 0x004CFBC0
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

		// Token: 0x0600D688 RID: 54920 RVA: 0x004D19ED File Offset: 0x004CFBED
		public void StopSearch()
		{
			if (this.userGroupHandle != null)
			{
				this.user.SocialManager.DestroyUserGroup(this.userGroupHandle);
			}
			this.userGroupHandle = null;
		}

		// Token: 0x0600D689 RID: 54921 RVA: 0x004D1A1A File Offset: 0x004CFC1A
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600D68A RID: 54922 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400A2D4 RID: 41684
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A2D5 RID: 41685
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;

		// Token: 0x0400A2D6 RID: 41686
		[PublicizedFrom(EAccessModifier.Private)]
		public IServerListInterface serverLookup;

		// Token: 0x0400A2D7 RID: 41687
		[PublicizedFrom(EAccessModifier.Private)]
		public XblSocialManagerUserGroupHandle userGroupHandle;

		// Token: 0x0400A2D8 RID: 41688
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isLoadingFriendsList;

		// Token: 0x0400A2D9 RID: 41689
		[PublicizedFrom(EAccessModifier.Private)]
		public int activitySearchCount;

		// Token: 0x0400A2DA RID: 41690
		[PublicizedFrom(EAccessModifier.Private)]
		public int sessionSearchCount;
	}
}
