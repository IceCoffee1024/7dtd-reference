using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;
using UnityEngine;

namespace Platform.XBL
{
	// Token: 0x02001BDB RID: 7131
	public class SocialManagerXbl
	{
		// Token: 0x0600D388 RID: 54152 RVA: 0x004C6C0C File Offset: 0x004C4E0C
		public SocialManagerXbl(XUserHandle user)
		{
			int num = SDK.XBL.XblSocialManagerAddLocalUser(user, XblSocialManagerExtraDetailLevel.NoExtraDetail);
			if (!Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num))
			{
				XblHelpers.LogHR(num, "XblSocialManagerAddLocalUser", false);
				return;
			}
			this.localUser = user;
		}

		// Token: 0x0600D389 RID: 54153 RVA: 0x004C6C50 File Offset: 0x004C4E50
		public bool TryCreateUserGroup(XblPresenceFilter presenceFilter, XblRelationshipFilter relationshipFilter, SocialManagerXbl.UserGroupMembersChanged callback, out XblSocialManagerUserGroupHandle handle)
		{
			if (this.localUser == null)
			{
				Log.Error("[XBL] Social users lookup not available as the local user was not registered");
				handle = null;
				return false;
			}
			if (callback == null)
			{
				Log.Error("[XBL] TryCreateUserGroup null callback not permitted");
				handle = null;
				return false;
			}
			int num = SDK.XBL.XblSocialManagerCreateSocialUserGroupFromFilters(this.localUser, presenceFilter, relationshipFilter, out handle);
			if (!Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num))
			{
				Log.Error(string.Format("[XBL] Failed to create user group for {0} {1}", presenceFilter, relationshipFilter));
				XblHelpers.LogHR(num, "XblSocialManagerCreateSocialUserGroupFromFilters", false);
				handle = null;
				return false;
			}
			this.userGroups.Add(handle, new SocialManagerXbl.UserGroup(handle, callback));
			if (this.updateCoroutine == null)
			{
				this.updateCoroutine = ThreadManager.StartCoroutine(this.UpdateSocialManagerCoroutine());
			}
			return true;
		}

		// Token: 0x0600D38A RID: 54154 RVA: 0x004C6D04 File Offset: 0x004C4F04
		public void DestroyUserGroup(XblSocialManagerUserGroupHandle handle)
		{
			this.userGroups.Remove(handle);
			int num = SDK.XBL.XblSocialManagerDestroySocialUserGroup(handle);
			if (!Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num))
			{
				XblHelpers.LogHR(num, "XblSocialManagerCreateSocialUserGroupFromFilters", false);
			}
			if (this.userGroups.Count == 0 && this.updateCoroutine != null)
			{
				ThreadManager.StopCoroutine(this.updateCoroutine);
				this.updateCoroutine = null;
			}
		}

		// Token: 0x0600D38B RID: 54155 RVA: 0x004C6D60 File Offset: 0x004C4F60
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator UpdateSocialManagerCoroutine()
		{
			int num;
			for (;;)
			{
				XblSocialManagerEvent[] array;
				num = SDK.XBL.XblSocialManagerDoWork(out array);
				if (!Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num))
				{
					break;
				}
				if (array != null && array.Length != 0)
				{
					if (SocialManagerXbl.IsUpdateRequired(array))
					{
						foreach (SocialManagerXbl.UserGroup userGroup in this.userGroups.Values)
						{
							userGroup.NotifyChanged();
						}
					}
					foreach (XblSocialManagerEvent xblSocialManagerEvent in array)
					{
						if (xblSocialManagerEvent.EventType == XblSocialManagerEventType.SocialUserGroupLoaded)
						{
							SocialManagerXbl.UserGroup userGroup2;
							if (!this.userGroups.TryGetValue(xblSocialManagerEvent.LoadedGroup, out userGroup2))
							{
								Log.Error("[XBL] LoadedGroup did not match saved handle");
							}
							else
							{
								userGroup2.isLoaded = true;
								userGroup2.NotifyChanged();
							}
						}
					}
				}
				yield return null;
			}
			XblHelpers.LogHR(num, "XblAchievementsManagerDoWork", false);
			yield break;
			yield break;
		}

		// Token: 0x0600D38C RID: 54156 RVA: 0x004C6D70 File Offset: 0x004C4F70
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsUpdateRequired(XblSocialManagerEvent[] socialEvents)
		{
			for (int i = 0; i < socialEvents.Length; i++)
			{
				switch (socialEvents[i].EventType)
				{
				case XblSocialManagerEventType.UsersAddedToSocialGraph:
				case XblSocialManagerEventType.UsersRemovedFromSocialGraph:
				case XblSocialManagerEventType.SocialRelationshipsChanged:
					return true;
				case XblSocialManagerEventType.PresenceChanged:
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400A0D7 RID: 41175
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserHandle localUser;

		// Token: 0x0400A0D8 RID: 41176
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine updateCoroutine;

		// Token: 0x0400A0D9 RID: 41177
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<XblSocialManagerUserGroupHandle, SocialManagerXbl.UserGroup> userGroups = new Dictionary<XblSocialManagerUserGroupHandle, SocialManagerXbl.UserGroup>();

		// Token: 0x02001BDC RID: 7132
		// (Invoke) Token: 0x0600D38E RID: 54158
		public delegate void UserGroupMembersChanged(ulong[] members);

		// Token: 0x02001BDD RID: 7133
		[PublicizedFrom(EAccessModifier.Private)]
		public class UserGroup
		{
			// Token: 0x0600D391 RID: 54161 RVA: 0x004C6DC7 File Offset: 0x004C4FC7
			public UserGroup(XblSocialManagerUserGroupHandle handle, SocialManagerXbl.UserGroupMembersChanged membersChangedCallback)
			{
				this.handle = handle;
				this.membersChangedCallback = membersChangedCallback;
			}

			// Token: 0x0600D392 RID: 54162 RVA: 0x004C6DE0 File Offset: 0x004C4FE0
			public void NotifyChanged()
			{
				if (!this.isLoaded)
				{
					return;
				}
				ulong[] array = this.membersCache;
				int num = SDK.XBL.XblSocialManagerUserGroupGetUsersTrackedByGroup(this.handle, out this.membersCache);
				if (!Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num))
				{
					XblHelpers.LogHR(num, "XblSocialManagerUserGroupGetUsersTrackedByGroup", false);
					this.membersCache = null;
					return;
				}
				Array.Sort<ulong>(this.membersCache);
				if (array != null && array.Length == this.membersCache.Length && this.membersCache.SequenceEqual<ulong>(array))
				{
					Log.Out("[XBL] social manager skipping user group update as member list didn't change");
					return;
				}
				ulong[] array2 = new ulong[this.membersCache.Length];
				Array.Copy(this.membersCache, array2, this.membersCache.Length);
				this.membersChangedCallback(array2);
			}

			// Token: 0x0400A0DA RID: 41178
			public readonly XblSocialManagerUserGroupHandle handle;

			// Token: 0x0400A0DB RID: 41179
			public readonly SocialManagerXbl.UserGroupMembersChanged membersChangedCallback;

			// Token: 0x0400A0DC RID: 41180
			public bool isLoaded;

			// Token: 0x0400A0DD RID: 41181
			[PublicizedFrom(EAccessModifier.Private)]
			public ulong[] membersCache;
		}
	}
}
