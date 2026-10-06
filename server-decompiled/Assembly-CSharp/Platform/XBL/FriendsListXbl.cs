using System;
using System.Collections.Generic;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C27 RID: 7207
	public class FriendsListXbl
	{
		// Token: 0x0600D62F RID: 54831 RVA: 0x004D045F File Offset: 0x004CE65F
		public FriendsListXbl(SocialManagerXbl socialManager)
		{
			this.socialManager = socialManager;
			if (!socialManager.TryCreateUserGroup(XblPresenceFilter.All, XblRelationshipFilter.Friends, new SocialManagerXbl.UserGroupMembersChanged(this.OnFriendsListChanged), out this.friendsUserGroup))
			{
				Log.Error("[FriendsListXbl] failed to create friends social manager group");
				this.friendsUserGroup = null;
			}
		}

		// Token: 0x0600D630 RID: 54832 RVA: 0x004D049C File Offset: 0x004CE69C
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

		// Token: 0x0600D631 RID: 54833 RVA: 0x004D0517 File Offset: 0x004CE717
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

		// Token: 0x0400A2A6 RID: 41638
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SocialManagerXbl socialManager;

		// Token: 0x0400A2A7 RID: 41639
		[PublicizedFrom(EAccessModifier.Private)]
		public XblSocialManagerUserGroupHandle friendsUserGroup;

		// Token: 0x0400A2A8 RID: 41640
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<ulong> friendXuids;

		// Token: 0x0400A2A9 RID: 41641
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<ulong> friendsXuidsTemp;
	}
}
