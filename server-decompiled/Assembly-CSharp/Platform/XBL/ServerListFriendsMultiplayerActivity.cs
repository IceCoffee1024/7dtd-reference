using System;
using System.Collections.Generic;
using System.Threading;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C00 RID: 7168
	public class ServerListFriendsMultiplayerActivity : IServerListInterface
	{
		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x0600D508 RID: 54536 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x0600D509 RID: 54537 RVA: 0x004CCD32 File Offset: 0x004CAF32
		public bool IsRefreshing
		{
			get
			{
				return this.isLoadingFriendsList || this.activitySearchCount > 0 || this.sessionSearchCount > 0;
			}
		}

		// Token: 0x0600D50A RID: 54538 RVA: 0x004CCD50 File Offset: 0x004CAF50
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

		// Token: 0x0600D50B RID: 54539 RVA: 0x004CCD8D File Offset: 0x004CAF8D
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			if (this.serverLookup == null)
			{
				return;
			}
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x0600D50C RID: 54540 RVA: 0x004CCDA0 File Offset: 0x004CAFA0
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

		// Token: 0x0600D50D RID: 54541 RVA: 0x004CCDFB File Offset: 0x004CAFFB
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

		// Token: 0x0600D50E RID: 54542 RVA: 0x004CCE38 File Offset: 0x004CB038
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

		// Token: 0x0600D50F RID: 54543 RVA: 0x004CCED8 File Offset: 0x004CB0D8
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

		// Token: 0x0600D510 RID: 54544 RVA: 0x004CCF05 File Offset: 0x004CB105
		public void StopSearch()
		{
			if (this.userGroupHandle != null)
			{
				this.user.SocialManager.DestroyUserGroup(this.userGroupHandle);
			}
			this.userGroupHandle = null;
		}

		// Token: 0x0600D511 RID: 54545 RVA: 0x004CCF32 File Offset: 0x004CB132
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600D512 RID: 54546 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400A1FF RID: 41471
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A200 RID: 41472
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;

		// Token: 0x0400A201 RID: 41473
		[PublicizedFrom(EAccessModifier.Private)]
		public IServerListInterface serverLookup;

		// Token: 0x0400A202 RID: 41474
		[PublicizedFrom(EAccessModifier.Private)]
		public XblSocialManagerUserGroupHandle userGroupHandle;

		// Token: 0x0400A203 RID: 41475
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isLoadingFriendsList;

		// Token: 0x0400A204 RID: 41476
		[PublicizedFrom(EAccessModifier.Private)]
		public int activitySearchCount;

		// Token: 0x0400A205 RID: 41477
		[PublicizedFrom(EAccessModifier.Private)]
		public int sessionSearchCount;
	}
}
