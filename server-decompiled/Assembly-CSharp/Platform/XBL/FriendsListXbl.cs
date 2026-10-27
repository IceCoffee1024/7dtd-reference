using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C58 RID: 7256
	public class FriendsListXbl
	{
		// Token: 0x0600D743 RID: 55107 RVA: 0x004D3F87 File Offset: 0x004D2187
		public FriendsListXbl(SocialManagerXbl socialManager)
		{
			this.socialManager = socialManager;
			if (!socialManager.TryCreateUserGroup(XblPresenceFilter.All, XblRelationshipFilter.Friends, new SocialManagerXbl.UserGroupMembersChanged(this.OnFriendsListChanged), out this.friendsUserGroup))
			{
				Log.Error("[FriendsListXbl] failed to create friends social manager group");
				this.friendsUserGroup = null;
			}
		}

		// Token: 0x0600D744 RID: 55108 RVA: 0x004D3FC4 File Offset: 0x004D21C4
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

		// Token: 0x0600D745 RID: 55109 RVA: 0x004D403F File Offset: 0x004D223F
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

		// Token: 0x0400A38F RID: 41871
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SocialManagerXbl socialManager;

		// Token: 0x0400A390 RID: 41872
		[PublicizedFrom(EAccessModifier.Private)]
		public XblSocialManagerUserGroupHandle friendsUserGroup;

		// Token: 0x0400A391 RID: 41873
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<ulong> friendXuids;

		// Token: 0x0400A392 RID: 41874
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<ulong> friendsXuidsTemp;
	}
}
