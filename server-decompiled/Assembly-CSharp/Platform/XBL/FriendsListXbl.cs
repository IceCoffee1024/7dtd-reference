using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001BF5 RID: 7157
	public class FriendsListXbl
	{
		// Token: 0x0600D4B7 RID: 54455 RVA: 0x004CB977 File Offset: 0x004C9B77
		public FriendsListXbl(SocialManagerXbl socialManager)
		{
			this.socialManager = socialManager;
			if (!socialManager.TryCreateUserGroup(XblPresenceFilter.All, XblRelationshipFilter.Friends, new SocialManagerXbl.UserGroupMembersChanged(this.OnFriendsListChanged), out this.friendsUserGroup))
			{
				Log.Error("[FriendsListXbl] failed to create friends social manager group");
				this.friendsUserGroup = null;
			}
		}

		// Token: 0x0600D4B8 RID: 54456 RVA: 0x004CB9B4 File Offset: 0x004C9BB4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnFriendsListChanged(ulong[] friends)
		{
			if (this.friendsXuidsTemp == null)
			{
				this.friendsXuidsTemp = new HashSet<ulong>();
			}
			foreach (ulong num in friends)
			{
				this.friendsXuidsTemp.Add(num);
			}
			HashSet<ulong> hashSet = this.friendsXuidsTemp;
			HashSet<ulong> hashSet2 = this.friendXuids;
			this.friendXuids = hashSet;
			this.friendsXuidsTemp = hashSet2;
			HashSet<ulong> hashSet3 = this.friendsXuidsTemp;
			if (hashSet3 != null)
			{
				hashSet3.Clear();
			}
			XblXuidMapper.ResolveUserIdentifiers(this.friendXuids);
		}

		// Token: 0x0600D4B9 RID: 54457 RVA: 0x004CBA2F File Offset: 0x004C9C2F
		public bool IsFriend(ulong xuid)
		{
			if (this.friendsUserGroup == null)
			{
				Log.Error("[FriendsListXbl] could not check IsFriend, friends user group has not been initialized yet.");
				return false;
			}
			if (this.friendXuids == null)
			{
				Log.Error("[FriendsListXbl] could not check IsFriend, friends list has not been retrieved yet");
				return false;
			}
			return this.friendXuids.Contains(xuid);
		}

		// Token: 0x0400A1D1 RID: 41425
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SocialManagerXbl socialManager;

		// Token: 0x0400A1D2 RID: 41426
		[PublicizedFrom(EAccessModifier.Private)]
		public XblSocialManagerUserGroupHandle friendsUserGroup;

		// Token: 0x0400A1D3 RID: 41427
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<ulong> friendXuids;

		// Token: 0x0400A1D4 RID: 41428
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<ulong> friendsXuidsTemp;
	}
}
