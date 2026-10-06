using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;
using UnityEngine;

namespace Platform.XBL
{
	// Token: 0x02001C34 RID: 7220
	public class SocialManagerXbl
	{
		// Token: 0x0600D68E RID: 54926 RVA: 0x004D1AE8 File Offset: 0x004CFCE8
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

		// Token: 0x0600D68F RID: 54927 RVA: 0x004D1B2C File Offset: 0x004CFD2C
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

		// Token: 0x0600D690 RID: 54928 RVA: 0x004D1BE0 File Offset: 0x004CFDE0
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

		// Token: 0x0600D691 RID: 54929 RVA: 0x004D1C3C File Offset: 0x004CFE3C
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

		// Token: 0x0600D692 RID: 54930 RVA: 0x004D1C4C File Offset: 0x004CFE4C
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

		// Token: 0x0400A2DD RID: 41693
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserHandle localUser;

		// Token: 0x0400A2DE RID: 41694
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine updateCoroutine;

		// Token: 0x0400A2DF RID: 41695
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<XblSocialManagerUserGroupHandle, SocialManagerXbl.UserGroup> userGroups = new Dictionary<XblSocialManagerUserGroupHandle, SocialManagerXbl.UserGroup>();

		// Token: 0x02001C35 RID: 7221
		// (Invoke) Token: 0x0600D694 RID: 54932
		public delegate void UserGroupMembersChanged(ulong[] members);

		// Token: 0x02001C36 RID: 7222
		[PublicizedFrom(EAccessModifier.Private)]
		public class UserGroup
		{
			// Token: 0x0600D697 RID: 54935 RVA: 0x004D1CA3 File Offset: 0x004CFEA3
			public UserGroup(XblSocialManagerUserGroupHandle handle, SocialManagerXbl.UserGroupMembersChanged membersChangedCallback)
			{
				this.handle = handle;
				this.membersChangedCallback = membersChangedCallback;
			}

			// Token: 0x0600D698 RID: 54936 RVA: 0x004D1CBC File Offset: 0x004CFEBC
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

			// Token: 0x0400A2E0 RID: 41696
			public readonly XblSocialManagerUserGroupHandle handle;

			// Token: 0x0400A2E1 RID: 41697
			public readonly SocialManagerXbl.UserGroupMembersChanged membersChangedCallback;

			// Token: 0x0400A2E2 RID: 41698
			public bool isLoaded;

			// Token: 0x0400A2E3 RID: 41699
			[PublicizedFrom(EAccessModifier.Private)]
			public ulong[] membersCache;
		}
	}
}
