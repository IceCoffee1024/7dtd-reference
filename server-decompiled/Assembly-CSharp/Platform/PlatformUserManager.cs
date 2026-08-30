using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BD4 RID: 7124
	public static class PlatformUserManager
	{
		// Token: 0x0600D40B RID: 54283 RVA: 0x004C9199 File Offset: 0x004C7399
		[Conditional("PLATFORM_USER_MANAGER_DEBUG")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string message)
		{
			Log.Out("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D40C RID: 54284 RVA: 0x004C9199 File Offset: 0x004C7399
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string message)
		{
			Log.Out("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D40D RID: 54285 RVA: 0x004C91AB File Offset: 0x004C73AB
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string message)
		{
			Log.Warning("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D40E RID: 54286 RVA: 0x004C91BD File Offset: 0x004C73BD
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string message)
		{
			Log.Error("[PlatformUserManager] " + message);
		}

		// Token: 0x14000128 RID: 296
		// (add) Token: 0x0600D40F RID: 54287 RVA: 0x004C91D0 File Offset: 0x004C73D0
		// (remove) Token: 0x0600D410 RID: 54288 RVA: 0x004C9204 File Offset: 0x004C7404
		public static event PlatformUserBlockedStateChangedHandler BlockedStateChanged;

		// Token: 0x14000129 RID: 297
		// (add) Token: 0x0600D411 RID: 54289 RVA: 0x004C9238 File Offset: 0x004C7438
		// (remove) Token: 0x0600D412 RID: 54290 RVA: 0x004C926C File Offset: 0x004C746C
		public static event PlatformUserDetailsUpdatedHandler DetailsUpdated;

		// Token: 0x0600D413 RID: 54291 RVA: 0x004C92A0 File Offset: 0x004C74A0
		public static void Init()
		{
			PlatformUserManager.s_primaryIdToPlatform = new Dictionary<PlatformUserIdentifierAbs, PlatformUserManager.PlatformUserData>();
			PlatformUserManager.s_primaryIdToPlatformLock = new ReaderWriterLockSlim();
			PlatformUserManager.s_nativeIdToPrimaryIds = new OneToManyDictionary<PlatformUserIdentifierAbs, PlatformUserIdentifierAbs>();
			PlatformUserManager.s_nativeIdToPrimaryIdsLock = new ReaderWriterLockSlim();
			PlatformUserManager.s_nativeUserIdsSeen = new HashSet<PlatformUserIdentifierAbs>();
			PlatformUserManager.s_nativeUserIdsSeenLock = new ReaderWriterLockSlim();
			PlatformUserManager.s_lastPermissions = EUserPerms.All;
			PlatformUserManager.s_persistentPlayerListLast = null;
			PlatformUserManager.s_persistentIdsTemp = new HashSet<PlatformUserIdentifierAbs>();
			PlatformUserManager.s_persistentIdsLast = new HashSet<PlatformUserIdentifierAbs>();
			PlatformUserManager.s_blockedUsersToUpdate = new HashSet<PlatformUserManager.PlatformUserData>();
			PlatformUserManager.s_blockedUsersToUpdateLock = new ReaderWriterLockSlim();
			PlatformUserManager.s_blockedDataCurrentlyUpdating = new List<PlatformUserManager.PlatformUserBlockedResults>();
			PlatformUserManager.s_blockedDataCurrentlyUpdatingReadOnly = new ReadOnlyListWrapper<PlatformUserManager.PlatformUserBlockedResults, IPlatformUserBlockedResults>(PlatformUserManager.s_blockedDataCurrentlyUpdating);
			PlatformManager.MultiPlatform.User.UserBlocksChanged += PlatformUserManager.OnPlatformUserBlocksChanged;
			PlatformUserManager.s_userDetailsToUpdate = new HashSet<PlatformUserManager.PlatformUserData>();
			PlatformUserManager.s_userDetailsCurrentlyUpdating = new List<PlatformUserManager.PlatformUserDetailsResult>();
			PlatformUserManager.s_userDetailsToUpdateLock = new ReaderWriterLockSlim();
			PlatformUserManager.s_enabled = true;
		}

		// Token: 0x0600D414 RID: 54292 RVA: 0x004C9378 File Offset: 0x004C7578
		public static void Destroy()
		{
			PlatformUserManager.s_enabled = false;
			PlatformUserManager.s_userDetailsToUpdateLock = null;
			PlatformUserManager.s_userDetailsCurrentlyUpdating = null;
			PlatformUserManager.s_userDetailsToUpdate = null;
			PlatformManager.MultiPlatform.User.UserBlocksChanged -= PlatformUserManager.OnPlatformUserBlocksChanged;
			PlatformUserManager.s_blockedDataCurrentlyUpdatingReadOnly = null;
			PlatformUserManager.s_blockedDataCurrentlyUpdating = null;
			ReaderWriterLockSlim readerWriterLockSlim = PlatformUserManager.s_blockedUsersToUpdateLock;
			if (readerWriterLockSlim != null)
			{
				readerWriterLockSlim.Dispose();
			}
			PlatformUserManager.s_blockedUsersToUpdateLock = null;
			PlatformUserManager.s_blockedUsersToUpdate = null;
			PlatformUserManager.s_persistentIdsLast = null;
			PlatformUserManager.s_persistentIdsTemp = null;
			PlatformUserManager.s_persistentPlayerListLast = null;
			PlatformUserManager.s_lastPermissions = (EUserPerms)0;
			ReaderWriterLockSlim readerWriterLockSlim2 = PlatformUserManager.s_nativeUserIdsSeenLock;
			if (readerWriterLockSlim2 != null)
			{
				readerWriterLockSlim2.Dispose();
			}
			PlatformUserManager.s_nativeUserIdsSeenLock = null;
			PlatformUserManager.s_nativeUserIdsSeen = null;
			ReaderWriterLockSlim readerWriterLockSlim3 = PlatformUserManager.s_nativeIdToPrimaryIdsLock;
			if (readerWriterLockSlim3 != null)
			{
				readerWriterLockSlim3.Dispose();
			}
			PlatformUserManager.s_nativeIdToPrimaryIdsLock = null;
			PlatformUserManager.s_nativeIdToPrimaryIds = null;
			ReaderWriterLockSlim readerWriterLockSlim4 = PlatformUserManager.s_primaryIdToPlatformLock;
			if (readerWriterLockSlim4 != null)
			{
				readerWriterLockSlim4.Dispose();
			}
			PlatformUserManager.s_primaryIdToPlatformLock = null;
			PlatformUserManager.s_primaryIdToPlatform = null;
		}

		// Token: 0x0600D415 RID: 54293 RVA: 0x004C944C File Offset: 0x004C764C
		public static void Update()
		{
			if (!PlatformUserManager.s_enabled)
			{
				return;
			}
			try
			{
				PlatformUserManager.UpdatePermissions();
				PlatformUserManager.UpdatePersistentIds();
				PlatformUserManager.UpdateUserDetails();
				PlatformUserManager.UpdateBlockedStates();
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		// Token: 0x0600D416 RID: 54294 RVA: 0x004C9490 File Offset: 0x004C7690
		public static IPlatformUserData GetOrCreate(PlatformUserIdentifierAbs primaryId)
		{
			if (primaryId == null)
			{
				return null;
			}
			PlatformUserManager.PlatformUserData platformUserData2;
			using (PlatformUserManager.s_primaryIdToPlatformLock.UpgradableReadLockScope())
			{
				PlatformUserManager.PlatformUserData platformUserData;
				if (PlatformUserManager.s_primaryIdToPlatform.TryGetValue(primaryId, out platformUserData))
				{
					return platformUserData;
				}
				using (PlatformUserManager.s_primaryIdToPlatformLock.WriteLockScope())
				{
					platformUserData2 = new PlatformUserManager.PlatformUserData(primaryId);
					PlatformUserManager.s_primaryIdToPlatform.Add(primaryId, platformUserData2);
				}
			}
			PlatformUserManager.OnUserAdded(primaryId, true);
			return platformUserData2;
		}

		// Token: 0x0600D417 RID: 54295 RVA: 0x004C9524 File Offset: 0x004C7724
		public static bool TryGetNativePlatform(PlatformUserIdentifierAbs primaryId, out EPlatformIdentifier platform)
		{
			if (primaryId == null)
			{
				platform = EPlatformIdentifier.None;
				return false;
			}
			bool flag;
			using (PlatformUserManager.s_primaryIdToPlatformLock.ReadLockScope())
			{
				PlatformUserManager.PlatformUserData platformUserData;
				if (!PlatformUserManager.s_primaryIdToPlatform.TryGetValue(primaryId, out platformUserData))
				{
					platform = EPlatformIdentifier.None;
					flag = false;
				}
				else
				{
					PlatformUserIdentifierAbs nativeId = platformUserData.NativeId;
					if (nativeId == null)
					{
						platform = EPlatformIdentifier.None;
						flag = false;
					}
					else
					{
						platform = nativeId.PlatformIdentifier;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600D418 RID: 54296 RVA: 0x004C9598 File Offset: 0x004C7798
		public static int TryGetByNative(PlatformUserIdentifierAbs nativeId, Span<PlatformUserIdentifierAbs> primaryIds)
		{
			if (nativeId == null)
			{
				return 0;
			}
			int num;
			using (PlatformUserManager.s_nativeIdToPrimaryIdsLock.ReadLockScope())
			{
				num = PlatformUserManager.s_nativeIdToPrimaryIds.TryGetByKey(nativeId, primaryIds);
			}
			if (num >= 3)
			{
				PlatformUserManager.LogWarning(string.Format("Expected number of values returned {0} to be less than the limit of PrimaryIds per NativeId ({1}).", num, 3));
			}
			return num;
		}

		// Token: 0x0600D419 RID: 54297 RVA: 0x004C9604 File Offset: 0x004C7804
		public static IEnumerator ResolveUserBlockedCoroutine(IPlatformUserData data)
		{
			for (;;)
			{
				using (PlatformUserManager.s_blockedUsersToUpdateLock.ReadLockScope())
				{
					if (!PlatformUserManager.s_blockedUsersToUpdate.Contains((PlatformUserManager.PlatformUserData)data))
					{
						yield break;
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600D41A RID: 54298 RVA: 0x004C9613 File Offset: 0x004C7813
		public static IEnumerator ResolveUserDetailsCoroutine(IPlatformUserData data)
		{
			for (;;)
			{
				using (PlatformUserManager.s_userDetailsToUpdateLock.ReadLockScope())
				{
					if (!PlatformUserManager.s_userDetailsToUpdate.Contains((PlatformUserManager.PlatformUserData)data))
					{
						yield break;
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600D41B RID: 54299 RVA: 0x004C9624 File Offset: 0x004C7824
		public static bool AreUsersPendingResolve(IReadOnlyList<IPlatformUserData> users)
		{
			if (users == null || users.Count == 0)
			{
				return false;
			}
			using (PlatformUserManager.s_blockedUsersToUpdateLock.ReadLockScope())
			{
				for (int i = 0; i < users.Count; i++)
				{
					if (PlatformUserManager.s_blockedUsersToUpdate.Contains((PlatformUserManager.PlatformUserData)users[i]))
					{
						return true;
					}
				}
			}
			using (PlatformUserManager.s_userDetailsToUpdateLock.ReadLockScope())
			{
				for (int j = 0; j < users.Count; j++)
				{
					if (PlatformUserManager.s_userDetailsToUpdate.Contains((PlatformUserManager.PlatformUserData)users[j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D41C RID: 54300 RVA: 0x004C96F0 File Offset: 0x004C78F0
		public static IEnumerator ResolveUserBlocksCoroutine(IReadOnlyList<IPlatformUserData> users)
		{
			if (users == null || users.Count == 0)
			{
				yield break;
			}
			foreach (IPlatformUserData platformUserData in users)
			{
				yield return PlatformUserManager.ResolveUserBlockedCoroutine(platformUserData);
			}
			IEnumerator<IPlatformUserData> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600D41D RID: 54301 RVA: 0x004C96FF File Offset: 0x004C78FF
		public static IEnumerator ResolveUsersDetailsCoroutine(IReadOnlyList<IPlatformUserData> users)
		{
			if (users == null || users.Count == 0)
			{
				yield break;
			}
			foreach (IPlatformUserData platformUserData in users)
			{
				yield return PlatformUserManager.ResolveUserDetailsCoroutine(platformUserData);
			}
			IEnumerator<IPlatformUserData> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600D41E RID: 54302 RVA: 0x004C9710 File Offset: 0x004C7910
		[PublicizedFrom(EAccessModifier.Private)]
		public static void OnUserAdded(PlatformUserIdentifierAbs userId, bool isPrimary)
		{
			if (!ThreadManager.IsMainThread())
			{
				ThreadManager.AddSingleTaskMainThread("PlatformUserManager.OnUserAdded", delegate(object _)
				{
					PlatformUserManager.OnUserAdded(userId, isPrimary);
				}, null);
				return;
			}
			PlatformManager.MultiPlatform.UserAdded(userId, isPrimary);
		}

		// Token: 0x0600D41F RID: 54303 RVA: 0x004C9768 File Offset: 0x004C7968
		[PublicizedFrom(EAccessModifier.Private)]
		public static void OnBlockedStateChanged(PlatformUserManager.PlatformUserData userData, EBlockType type, EUserBlockState nextBlockState)
		{
			if (!ThreadManager.IsMainThread())
			{
				ThreadManager.AddSingleTaskMainThread("PlatformUserManager.OnBlockedStateChanged", delegate(object _)
				{
					PlatformUserBlockedStateChangedHandler blockedStateChanged2 = PlatformUserManager.BlockedStateChanged;
					if (blockedStateChanged2 == null)
					{
						return;
					}
					blockedStateChanged2(userData, type, nextBlockState);
				}, null);
				return;
			}
			PlatformUserBlockedStateChangedHandler blockedStateChanged = PlatformUserManager.BlockedStateChanged;
			if (blockedStateChanged == null)
			{
				return;
			}
			blockedStateChanged(userData, type, nextBlockState);
		}

		// Token: 0x0600D420 RID: 54304 RVA: 0x004C97D0 File Offset: 0x004C79D0
		[PublicizedFrom(EAccessModifier.Private)]
		public static void UpdatePermissions()
		{
			if (Time.frameCount % 60 != 0)
			{
				return;
			}
			EUserPerms permissions = PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.All);
			if ((PlatformUserManager.s_lastPermissions ^ permissions).HasCommunication())
			{
				PlatformUserManager.MarkBlockedStateChangedAll();
			}
			PlatformUserManager.s_lastPermissions = permissions;
		}

		// Token: 0x0600D421 RID: 54305 RVA: 0x004C980C File Offset: 0x004C7A0C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void UpdatePersistentIds()
		{
			if (Time.frameCount % 300 != 0)
			{
				return;
			}
			PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
			if (persistentPlayers == null)
			{
				return;
			}
			if (PlatformUserManager.s_persistentPlayerListLast != persistentPlayers)
			{
				PlatformUserManager.s_persistentIdsLast.Clear();
				PlatformUserManager.s_persistentPlayerListLast = persistentPlayers;
			}
			ICollection<PlatformUserIdentifierAbs> players = persistentPlayers.Players.Keys;
			PlatformUserManager.s_persistentIdsLast.RemoveWhere((PlatformUserIdentifierAbs last) => !players.Contains(last));
			PlatformUserManager.s_persistentIdsTemp.Clear();
			foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs in players)
			{
				if (!PlatformUserManager.s_persistentIdsLast.Contains(platformUserIdentifierAbs))
				{
					PlatformUserManager.s_persistentIdsLast.Add(platformUserIdentifierAbs);
					PlatformUserManager.s_persistentIdsTemp.Add(platformUserIdentifierAbs);
				}
			}
			foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs2 in PlatformUserManager.s_persistentIdsTemp)
			{
				IPlatformUserData orCreate = PlatformUserManager.GetOrCreate(platformUserIdentifierAbs2);
				foreach (IPlatformUserBlockedData platformUserBlockedData in orCreate.Blocked.Values)
				{
					platformUserBlockedData.Locally = false;
				}
				orCreate.MarkBlockedStateChanged();
			}
		}

		// Token: 0x0600D422 RID: 54306 RVA: 0x004C9974 File Offset: 0x004C7B74
		[PublicizedFrom(EAccessModifier.Private)]
		public static void UpdateUserDetails()
		{
			if (PlatformUserManager.s_userDetailsCurrentlyUpdating.Count > 0)
			{
				return;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			using (PlatformUserManager.s_userDetailsToUpdateLock.UpgradableReadLockScope())
			{
				if (PlatformUserManager.s_userDetailsToUpdate.Count <= 0)
				{
					return;
				}
				foreach (PlatformUserManager.PlatformUserData platformUserData in PlatformUserManager.s_userDetailsToUpdate)
				{
					if (platformUserData.NextDetailRetryTime <= realtimeSinceStartup)
					{
						PlatformUserManager.s_userDetailsCurrentlyUpdating.Add(new PlatformUserManager.PlatformUserDetailsResult(platformUserData));
					}
				}
			}
			if (PlatformUserManager.s_userDetailsCurrentlyUpdating.Count > 0)
			{
				ThreadManager.StartCoroutine(PlatformUserManager.ResolveUserDetailsCoroutine());
			}
		}

		// Token: 0x0600D423 RID: 54307 RVA: 0x004C9A38 File Offset: 0x004C7C38
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator ResolveUserDetailsCoroutine()
		{
			try
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (((crossplatformPlatform != null) ? crossplatformPlatform.UserDetailsService : null) != null)
				{
					List<UserDetailsRequest> list = null;
					List<int> list2 = null;
					for (int i = 0; i < PlatformUserManager.s_userDetailsCurrentlyUpdating.Count; i++)
					{
						PlatformUserManager.PlatformUserDetailsResult platformUserDetailsResult = PlatformUserManager.s_userDetailsCurrentlyUpdating[i];
						if (platformUserDetailsResult.UserData.NativeId != null)
						{
							if (list == null)
							{
								list = new List<UserDetailsRequest>();
							}
							if (list2 == null)
							{
								list2 = new List<int>();
							}
							list.Add(new UserDetailsRequest(platformUserDetailsResult.UserData.PrimaryId, platformUserDetailsResult.UserData.NativeId.PlatformIdentifier));
							list2.Add(i);
						}
					}
					if (list != null)
					{
						yield return PlatformUserManager.<ResolveUserDetailsCoroutine>g__ResolveUserDetails|50_0(PlatformManager.CrossplatformPlatform.UserDetailsService, list, list2, PlatformUserManager.s_userDetailsCurrentlyUpdating);
					}
				}
				if (PlatformManager.NativePlatform.UserDetailsService != null)
				{
					List<UserDetailsRequest> list3 = null;
					List<int> list4 = null;
					for (int j = 0; j < PlatformUserManager.s_userDetailsCurrentlyUpdating.Count; j++)
					{
						PlatformUserManager.PlatformUserDetailsResult platformUserDetailsResult2 = PlatformUserManager.s_userDetailsCurrentlyUpdating[j];
						if (platformUserDetailsResult2.UserData.NativeId != null)
						{
							if (list3 == null)
							{
								list3 = new List<UserDetailsRequest>();
							}
							if (list4 == null)
							{
								list4 = new List<int>();
							}
							list3.Add(new UserDetailsRequest(platformUserDetailsResult2.UserData.NativeId));
							list4.Add(j);
						}
					}
					if (list3 != null)
					{
						yield return PlatformUserManager.<ResolveUserDetailsCoroutine>g__ResolveUserDetails|50_0(PlatformManager.NativePlatform.UserDetailsService, list3, list4, PlatformUserManager.s_userDetailsCurrentlyUpdating);
					}
				}
				foreach (PlatformUserManager.PlatformUserDetailsResult platformUserDetailsResult3 in PlatformUserManager.s_userDetailsCurrentlyUpdating)
				{
					if (!string.IsNullOrEmpty(platformUserDetailsResult3.Name))
					{
						platformUserDetailsResult3.UserData.Name = platformUserDetailsResult3.Name;
						PlatformUserDetailsUpdatedHandler detailsUpdated = PlatformUserManager.DetailsUpdated;
						if (detailsUpdated != null)
						{
							detailsUpdated(platformUserDetailsResult3.UserData, platformUserDetailsResult3.Name);
						}
					}
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				using (PlatformUserManager.s_userDetailsToUpdateLock.WriteLockScope())
				{
					foreach (PlatformUserManager.PlatformUserDetailsResult platformUserDetailsResult4 in PlatformUserManager.s_userDetailsCurrentlyUpdating)
					{
						PlatformUserManager.PlatformUserData userData = platformUserDetailsResult4.UserData;
						if (!string.IsNullOrEmpty(platformUserDetailsResult4.Name))
						{
							userData.DetailAttempts = 0;
							userData.NextDetailRetryTime = 0f;
							PlatformUserManager.s_userDetailsToUpdate.Remove(userData);
						}
						else
						{
							userData.DetailAttempts++;
							if (userData.DetailAttempts >= 5)
							{
								PlatformUserManager.LogWarning(string.Format("[PLATFORM] User details lookup gave up after {0} attempts.", userData.DetailAttempts));
								userData.DetailAttempts = 0;
								userData.NextDetailRetryTime = 0f;
								PlatformUserManager.s_userDetailsToUpdate.Remove(userData);
							}
							else
							{
								float num = 0.5f * (float)(1 << userData.DetailAttempts - 1);
								if (num > 30f)
								{
									num = 30f;
								}
								userData.NextDetailRetryTime = realtimeSinceStartup + num;
							}
						}
					}
				}
			}
			finally
			{
				PlatformUserManager.s_userDetailsCurrentlyUpdating.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x0600D424 RID: 54308 RVA: 0x004C9A40 File Offset: 0x004C7C40
		[PublicizedFrom(EAccessModifier.Private)]
		public static void UpdateBlockedStates()
		{
			if (PlatformUserManager.s_blockedDataCurrentlyUpdating.Count > 0 || PlatformUserManager.s_blockedUsersToUpdate.Count <= 0)
			{
				return;
			}
			PlatformUserManager.PlatformUserData platformUserData;
			bool flag;
			using (PlatformUserManager.s_primaryIdToPlatformLock.ReadLockScope())
			{
				flag = PlatformUserManager.s_primaryIdToPlatform.TryGetValue(PlatformManager.MultiPlatform.User.PlatformUserId, out platformUserData);
			}
			using (PlatformUserManager.s_blockedUsersToUpdateLock.UpgradableReadLockScope())
			{
				if (flag)
				{
					using (PlatformUserManager.s_blockedUsersToUpdateLock.WriteLockScope())
					{
						PlatformUserManager.s_blockedUsersToUpdate.Remove(platformUserData);
					}
				}
				foreach (PlatformUserManager.PlatformUserData platformUserData2 in PlatformUserManager.s_blockedUsersToUpdate)
				{
					PlatformUserManager.s_blockedDataCurrentlyUpdating.Add(new PlatformUserManager.PlatformUserBlockedResults(platformUserData2));
				}
			}
			if (PlatformUserManager.s_blockedDataCurrentlyUpdating.Count > 0)
			{
				ThreadManager.StartCoroutine(PlatformUserManager.UpdateBlockedStatesCoroutine());
			}
		}

		// Token: 0x0600D425 RID: 54309 RVA: 0x004C9B70 File Offset: 0x004C7D70
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator UpdateBlockedStatesCoroutine()
		{
			try
			{
				yield return PlatformManager.MultiPlatform.User.ResolveUserBlocks(PlatformUserManager.s_blockedDataCurrentlyUpdatingReadOnly);
				if (BlockedPlayerList.Instance != null)
				{
					yield return PlatformUserManager.ResolveUserBlocksFromBlockList(PlatformUserManager.s_blockedDataCurrentlyUpdatingReadOnly);
				}
				foreach (PlatformUserManager.PlatformUserBlockedResults platformUserBlockedResults in PlatformUserManager.s_blockedDataCurrentlyUpdating)
				{
					if (!platformUserBlockedResults.HasErrored)
					{
						foreach (EBlockType eblockType in EnumUtils.Values<EBlockType>())
						{
							platformUserBlockedResults.User.Blocked[eblockType].RefreshBlockedState(platformUserBlockedResults.IsBlocked[eblockType]);
						}
					}
				}
				using (PlatformUserManager.s_blockedUsersToUpdateLock.WriteLockScope())
				{
					foreach (PlatformUserManager.PlatformUserBlockedResults platformUserBlockedResults2 in PlatformUserManager.s_blockedDataCurrentlyUpdating)
					{
						PlatformUserManager.s_blockedUsersToUpdate.Remove(platformUserBlockedResults2.User);
					}
				}
			}
			finally
			{
				PlatformUserManager.s_blockedDataCurrentlyUpdating.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x0600D426 RID: 54310 RVA: 0x004C9B78 File Offset: 0x004C7D78
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MarkBlockedStateChangedAll()
		{
			using (PlatformUserManager.s_primaryIdToPlatformLock.ReadLockScope())
			{
				foreach (PlatformUserManager.PlatformUserData platformUserData in PlatformUserManager.s_primaryIdToPlatform.Values)
				{
					platformUserData.MarkBlockedStateChanged();
				}
			}
		}

		// Token: 0x0600D427 RID: 54311 RVA: 0x004C9BF4 File Offset: 0x004C7DF4
		[PublicizedFrom(EAccessModifier.Private)]
		public static void OnPlatformUserBlocksChanged(IReadOnlyCollection<PlatformUserIdentifierAbs> userIds)
		{
			if (userIds == null)
			{
				PlatformUserManager.MarkBlockedStateChangedAll();
				return;
			}
			using (PlatformUserManager.s_primaryIdToPlatformLock.ReadLockScope())
			{
				foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs in userIds)
				{
					PlatformUserManager.PlatformUserData platformUserData;
					if (PlatformUserManager.s_primaryIdToPlatform.TryGetValue(platformUserIdentifierAbs, out platformUserData))
					{
						platformUserData.MarkBlockedStateChanged();
					}
				}
			}
			using (PlatformUserManager.s_nativeIdToPrimaryIdsLock.ReadLockScope())
			{
				foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs2 in userIds)
				{
					IReadOnlyCollection<PlatformUserIdentifierAbs> readOnlyCollection;
					if (PlatformUserManager.s_nativeIdToPrimaryIds.TryGetByKey(platformUserIdentifierAbs2, out readOnlyCollection))
					{
						using (PlatformUserManager.s_primaryIdToPlatformLock.ReadLockScope())
						{
							foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs3 in readOnlyCollection)
							{
								PlatformUserManager.s_primaryIdToPlatform[platformUserIdentifierAbs3].MarkBlockedStateChanged();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600D428 RID: 54312 RVA: 0x004C9D4C File Offset: 0x004C7F4C
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool CanCheckUserDetails()
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			return ((crossplatformPlatform != null) ? crossplatformPlatform.UserDetailsService : null) != null || PlatformManager.NativePlatform.UserDetailsService != null;
		}

		// Token: 0x0600D429 RID: 54313 RVA: 0x004C9D70 File Offset: 0x004C7F70
		public static IEnumerator ResolveUserBlocksFromBlockList(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			if (BlockedPlayerList.Instance == null)
			{
				yield break;
			}
			while (BlockedPlayerList.Instance.PendingResolve())
			{
				yield return null;
			}
			using (IEnumerator<BlockedPlayerList.ListEntry> enumerator = BlockedPlayerList.Instance.GetEntriesOrdered(true, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BlockedPlayerList.ListEntry listEntry = enumerator.Current;
					PlatformUserIdentifierAbs primaryId = listEntry.PlayerData.PrimaryId;
					foreach (IPlatformUserBlockedResults platformUserBlockedResults in _results)
					{
						if (platformUserBlockedResults.User.PrimaryId.Equals(primaryId))
						{
							platformUserBlockedResults.BlockAll();
							break;
						}
					}
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x0600D42A RID: 54314 RVA: 0x004C9D7F File Offset: 0x004C7F7F
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static IEnumerator <ResolveUserDetailsCoroutine>g__ResolveUserDetails|50_0(IUserDetailsService service, IReadOnlyList<UserDetailsRequest> requests, IReadOnlyList<int> resultsIndices, List<PlatformUserManager.PlatformUserDetailsResult> results)
		{
			PlatformUserManager.<>c__DisplayClass50_0 CS$<>8__locals1 = new PlatformUserManager.<>c__DisplayClass50_0();
			CS$<>8__locals1.resultsIndices = resultsIndices;
			CS$<>8__locals1.results = results;
			CS$<>8__locals1.requests = requests;
			CS$<>8__locals1.inProgress = true;
			service.RequestUserDetailsUpdate(CS$<>8__locals1.requests, new UserDetailsRequestCompleteHandler(CS$<>8__locals1.<ResolveUserDetailsCoroutine>g__OnComplete|1));
			while (CS$<>8__locals1.inProgress)
			{
				yield return true;
			}
			yield break;
		}

		// Token: 0x0400A15B RID: 41307
		public const int PrimaryIdsPerNativeIdLimit = 3;

		// Token: 0x0400A15C RID: 41308
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool s_enabled;

		// Token: 0x0400A15D RID: 41309
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<PlatformUserIdentifierAbs, PlatformUserManager.PlatformUserData> s_primaryIdToPlatform;

		// Token: 0x0400A15E RID: 41310
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_primaryIdToPlatformLock;

		// Token: 0x0400A15F RID: 41311
		[PublicizedFrom(EAccessModifier.Private)]
		public static OneToManyDictionary<PlatformUserIdentifierAbs, PlatformUserIdentifierAbs> s_nativeIdToPrimaryIds;

		// Token: 0x0400A160 RID: 41312
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_nativeIdToPrimaryIdsLock;

		// Token: 0x0400A161 RID: 41313
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_nativeUserIdsSeen;

		// Token: 0x0400A162 RID: 41314
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_nativeUserIdsSeenLock;

		// Token: 0x0400A163 RID: 41315
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PermissionFrameFrequency = 60;

		// Token: 0x0400A164 RID: 41316
		[PublicizedFrom(EAccessModifier.Private)]
		public static EUserPerms s_lastPermissions;

		// Token: 0x0400A165 RID: 41317
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PersistentFrameFrequency = 300;

		// Token: 0x0400A166 RID: 41318
		[PublicizedFrom(EAccessModifier.Private)]
		public static PersistentPlayerList s_persistentPlayerListLast;

		// Token: 0x0400A167 RID: 41319
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_persistentIdsTemp;

		// Token: 0x0400A168 RID: 41320
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_persistentIdsLast;

		// Token: 0x0400A169 RID: 41321
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserManager.PlatformUserData> s_blockedUsersToUpdate;

		// Token: 0x0400A16A RID: 41322
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_blockedUsersToUpdateLock;

		// Token: 0x0400A16B RID: 41323
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<PlatformUserManager.PlatformUserBlockedResults> s_blockedDataCurrentlyUpdating;

		// Token: 0x0400A16C RID: 41324
		[PublicizedFrom(EAccessModifier.Private)]
		public static IReadOnlyList<IPlatformUserBlockedResults> s_blockedDataCurrentlyUpdatingReadOnly;

		// Token: 0x0400A16E RID: 41326
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserManager.PlatformUserData> s_userDetailsToUpdate;

		// Token: 0x0400A16F RID: 41327
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<PlatformUserManager.PlatformUserDetailsResult> s_userDetailsCurrentlyUpdating;

		// Token: 0x0400A170 RID: 41328
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_userDetailsToUpdateLock;

		// Token: 0x0400A171 RID: 41329
		[PublicizedFrom(EAccessModifier.Private)]
		public const int UserDetailsMaxAttempts = 5;

		// Token: 0x0400A172 RID: 41330
		[PublicizedFrom(EAccessModifier.Private)]
		public const float UserDetailsRetryBaseDelay = 0.5f;

		// Token: 0x0400A173 RID: 41331
		[PublicizedFrom(EAccessModifier.Private)]
		public const float UserDetailsRetryMaxDelay = 30f;

		// Token: 0x02001BD5 RID: 7125
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserData : IPlatformUserData, IPlatformUser
		{
			// Token: 0x0600D42B RID: 54315 RVA: 0x004C9DA4 File Offset: 0x004C7FA4
			public PlatformUserData(PlatformUserIdentifierAbs primaryId)
			{
				this.PrimaryId = primaryId;
				this.m_userBlockedStates = new EnumDictionary<EBlockType, PlatformUserManager.PlatformUserBlockedData>();
				this.m_userBlockedStatesReadOnly = new ReadOnlyDictionaryWrapper<EBlockType, PlatformUserManager.PlatformUserBlockedData, IPlatformUserBlockedData>(this.m_userBlockedStates);
				foreach (EBlockType eblockType in EnumUtils.Values<EBlockType>())
				{
					this.m_userBlockedStates[eblockType] = new PlatformUserManager.PlatformUserBlockedData(this, eblockType);
				}
				this.RequestUserDetailsUpdate();
			}

			// Token: 0x0600D42C RID: 54316 RVA: 0x004C9E2C File Offset: 0x004C802C
			public override string ToString()
			{
				string text = "{0}[PrimaryId={1}, NativeId={2}, Name={3}, {4}]";
				object[] array = new object[5];
				array[0] = "PlatformUserData";
				array[1] = this.PrimaryId;
				array[2] = this.NativeId;
				array[3] = this.Name;
				array[4] = string.Join(", ", this.Blocked.Select<KeyValuePair<EBlockType, PlatformUserManager.PlatformUserBlockedData>, string>((KeyValuePair<EBlockType, PlatformUserManager.PlatformUserBlockedData> kv) => string.Format("Blocked[{0}]={1}", kv.Key, kv.Value)));
				return string.Format(text, array);
			}

			// Token: 0x17001A40 RID: 6720
			// (get) Token: 0x0600D42D RID: 54317 RVA: 0x004C9EA3 File Offset: 0x004C80A3
			public PlatformUserIdentifierAbs PrimaryId { get; }

			// Token: 0x17001A41 RID: 6721
			// (get) Token: 0x0600D42E RID: 54318 RVA: 0x004C9EAC File Offset: 0x004C80AC
			// (set) Token: 0x0600D42F RID: 54319 RVA: 0x004C9F00 File Offset: 0x004C8100
			public PlatformUserIdentifierAbs NativeId
			{
				get
				{
					PlatformUserIdentifierAbs platformUserIdentifierAbs;
					using (PlatformUserManager.s_nativeIdToPrimaryIdsLock.ReadLockScope())
					{
						PlatformUserIdentifierAbs platformUserIdentifierAbs2;
						platformUserIdentifierAbs = (PlatformUserManager.s_nativeIdToPrimaryIds.TryGetByValue(this.PrimaryId, out platformUserIdentifierAbs2) ? platformUserIdentifierAbs2 : null);
					}
					return platformUserIdentifierAbs;
				}
				set
				{
					if (value == null)
					{
						return;
					}
					using (PlatformUserManager.s_nativeIdToPrimaryIdsLock.UpgradableReadLockScope())
					{
						PlatformUserIdentifierAbs platformUserIdentifierAbs;
						if (PlatformUserManager.s_nativeIdToPrimaryIds.TryGetByValue(this.PrimaryId, out platformUserIdentifierAbs))
						{
							if (platformUserIdentifierAbs.Equals(value))
							{
								return;
							}
							using (PlatformUserManager.s_nativeIdToPrimaryIdsLock.WriteLockScope())
							{
								PlatformUserManager.s_nativeIdToPrimaryIds.RemoveByValue(this.PrimaryId);
								PlatformUserManager.s_nativeIdToPrimaryIds.Add(value, this.PrimaryId);
								PlatformUserManager.LogError(string.Format("Primary ID '{0}' was be remapped from Native ID '{1}' to Native ID '{2}'.", this.PrimaryId, platformUserIdentifierAbs, value));
								goto IL_00BF;
							}
						}
						using (PlatformUserManager.s_nativeIdToPrimaryIdsLock.WriteLockScope())
						{
							PlatformUserManager.s_nativeIdToPrimaryIds.Add(value, this.PrimaryId);
						}
					}
					IL_00BF:
					bool flag;
					using (PlatformUserManager.s_nativeUserIdsSeenLock.UpgradableReadLockScope())
					{
						if (PlatformUserManager.s_nativeUserIdsSeen.Contains(value))
						{
							flag = false;
						}
						else
						{
							using (PlatformUserManager.s_nativeUserIdsSeenLock.WriteLockScope())
							{
								flag = PlatformUserManager.s_nativeUserIdsSeen.Add(value);
							}
						}
					}
					if (flag)
					{
						PlatformUserManager.OnUserAdded(value, false);
						this.RequestUserDetailsUpdate();
					}
				}
			}

			// Token: 0x17001A42 RID: 6722
			// (get) Token: 0x0600D430 RID: 54320 RVA: 0x004CA070 File Offset: 0x004C8270
			// (set) Token: 0x0600D431 RID: 54321 RVA: 0x004CA078 File Offset: 0x004C8278
			public string Name { get; set; }

			// Token: 0x0600D432 RID: 54322 RVA: 0x004CA084 File Offset: 0x004C8284
			public void RequestUserDetailsUpdate()
			{
				if (!PlatformUserManager.CanCheckUserDetails())
				{
					return;
				}
				using (PlatformUserManager.s_userDetailsToUpdateLock.WriteLockScope())
				{
					this.DetailAttempts = 0;
					this.NextDetailRetryTime = 0f;
					PlatformUserManager.s_userDetailsToUpdate.Add(this);
				}
			}

			// Token: 0x17001A43 RID: 6723
			// (get) Token: 0x0600D433 RID: 54323 RVA: 0x004CA0E4 File Offset: 0x004C82E4
			public IReadOnlyDictionary<EBlockType, PlatformUserManager.PlatformUserBlockedData> Blocked
			{
				get
				{
					return this.m_userBlockedStates;
				}
			}

			// Token: 0x17001A44 RID: 6724
			// (get) Token: 0x0600D434 RID: 54324 RVA: 0x004CA0EC File Offset: 0x004C82EC
			public IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> Platform.IPlatformUserData.Blocked
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.m_userBlockedStatesReadOnly;
				}
			}

			// Token: 0x0600D435 RID: 54325 RVA: 0x004CA0F4 File Offset: 0x004C82F4
			public void MarkBlockedStateChanged()
			{
				if (GameManager.IsDedicatedServer)
				{
					return;
				}
				using (PlatformUserManager.s_blockedUsersToUpdateLock.UpgradableReadLockScope())
				{
					if (!PlatformUserManager.s_blockedUsersToUpdate.Contains(this))
					{
						using (PlatformUserManager.s_blockedUsersToUpdateLock.WriteLockScope())
						{
							PlatformUserManager.s_blockedUsersToUpdate.Add(this);
						}
					}
				}
			}

			// Token: 0x0400A175 RID: 41333
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly EnumDictionary<EBlockType, PlatformUserManager.PlatformUserBlockedData> m_userBlockedStates;

			// Token: 0x0400A176 RID: 41334
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> m_userBlockedStatesReadOnly;

			// Token: 0x0400A179 RID: 41337
			public int DetailAttempts;

			// Token: 0x0400A17A RID: 41338
			public float NextDetailRetryTime;
		}

		// Token: 0x02001BD7 RID: 7127
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserBlockedData : IPlatformUserBlockedData
		{
			// Token: 0x0600D439 RID: 54329 RVA: 0x004CA19F File Offset: 0x004C839F
			public PlatformUserBlockedData(PlatformUserManager.PlatformUserData userData, EBlockType blockType)
			{
				this.m_userData = userData;
				this.Type = blockType;
				this.m_blockedLocally = false;
				this.State = EUserBlockState.NotBlocked;
			}

			// Token: 0x0600D43A RID: 54330 RVA: 0x004CA1C4 File Offset: 0x004C83C4
			public override string ToString()
			{
				return string.Format("{0}[Type={1}, State={2}, Locally={3}]", new object[] { "PlatformUserBlockedData", this.Type, this.State, this.Locally });
			}

			// Token: 0x17001A45 RID: 6725
			// (get) Token: 0x0600D43B RID: 54331 RVA: 0x004CA213 File Offset: 0x004C8413
			public EBlockType Type { get; }

			// Token: 0x17001A46 RID: 6726
			// (get) Token: 0x0600D43C RID: 54332 RVA: 0x004CA21B File Offset: 0x004C841B
			// (set) Token: 0x0600D43D RID: 54333 RVA: 0x004CA223 File Offset: 0x004C8423
			public EUserBlockState State
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17001A47 RID: 6727
			// (get) Token: 0x0600D43E RID: 54334 RVA: 0x004CA22C File Offset: 0x004C842C
			// (set) Token: 0x0600D43F RID: 54335 RVA: 0x004CA234 File Offset: 0x004C8434
			public bool Locally
			{
				get
				{
					return this.m_blockedLocally;
				}
				set
				{
					this.m_blockedLocally = value;
					this.RefreshBlockedState(this.State == EUserBlockState.ByPlatform);
				}
			}

			// Token: 0x0600D440 RID: 54336 RVA: 0x004CA24C File Offset: 0x004C844C
			public void RefreshBlockedState(bool isBlockedByPlatform)
			{
				EUserBlockState state = this.State;
				EUserBlockState euserBlockState;
				if (isBlockedByPlatform)
				{
					euserBlockState = EUserBlockState.ByPlatform;
				}
				else if (this.Locally)
				{
					euserBlockState = EUserBlockState.InGame;
				}
				else
				{
					euserBlockState = EUserBlockState.NotBlocked;
				}
				if (euserBlockState == EUserBlockState.ByPlatform)
				{
					this.m_blockedLocally = false;
				}
				this.State = euserBlockState;
				if (state != euserBlockState)
				{
					PlatformUserManager.OnBlockedStateChanged(this.m_userData, this.Type, euserBlockState);
				}
			}

			// Token: 0x0400A17D RID: 41341
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformUserManager.PlatformUserData m_userData;

			// Token: 0x0400A17E RID: 41342
			[PublicizedFrom(EAccessModifier.Private)]
			public bool m_blockedLocally;
		}

		// Token: 0x02001BD8 RID: 7128
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserBlockedResults : IPlatformUserBlockedResults
		{
			// Token: 0x0600D441 RID: 54337 RVA: 0x004CA29C File Offset: 0x004C849C
			public PlatformUserBlockedResults(PlatformUserManager.PlatformUserData userData)
			{
				this.m_userData = userData;
				this.IsBlocked = new EnumDictionary<EBlockType, bool>();
				foreach (EBlockType eblockType in EnumUtils.Values<EBlockType>())
				{
					this.IsBlocked[eblockType] = false;
				}
				this.HasErrored = false;
			}

			// Token: 0x0600D442 RID: 54338 RVA: 0x004CA310 File Offset: 0x004C8510
			public override string ToString()
			{
				string text = "{0}[{1}, HasErrored={2}, {3}.{4}={5}, {6}.{7}={8}]";
				object[] array = new object[9];
				array[0] = "PlatformUserBlockedResults";
				array[1] = string.Join(", ", this.IsBlocked.Select<KeyValuePair<EBlockType, bool>, string>((KeyValuePair<EBlockType, bool> kv) => string.Format("IsBlocked[{0}]={1}", kv.Key, kv.Value)));
				array[2] = this.HasErrored;
				array[3] = "User";
				array[4] = "PrimaryId";
				array[5] = this.User.PrimaryId;
				array[6] = "User";
				array[7] = "NativeId";
				array[8] = this.User.NativeId;
				return string.Format(text, array);
			}

			// Token: 0x17001A48 RID: 6728
			// (get) Token: 0x0600D443 RID: 54339 RVA: 0x004CA3B7 File Offset: 0x004C85B7
			public PlatformUserManager.PlatformUserData User
			{
				get
				{
					return this.m_userData;
				}
			}

			// Token: 0x17001A49 RID: 6729
			// (get) Token: 0x0600D444 RID: 54340 RVA: 0x004CA3BF File Offset: 0x004C85BF
			public EnumDictionary<EBlockType, bool> IsBlocked { get; }

			// Token: 0x17001A4A RID: 6730
			// (get) Token: 0x0600D445 RID: 54341 RVA: 0x004CA3C7 File Offset: 0x004C85C7
			// (set) Token: 0x0600D446 RID: 54342 RVA: 0x004CA3CF File Offset: 0x004C85CF
			public bool HasErrored
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17001A4B RID: 6731
			// (get) Token: 0x0600D447 RID: 54343 RVA: 0x004CA3B7 File Offset: 0x004C85B7
			public IPlatformUser Platform.IPlatformUserBlockedResults.User
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.m_userData;
				}
			}

			// Token: 0x0600D448 RID: 54344 RVA: 0x004CA3D8 File Offset: 0x004C85D8
			public void Block(EBlockType blockType)
			{
				this.IsBlocked[blockType] = true;
			}

			// Token: 0x0600D449 RID: 54345 RVA: 0x004CA3E7 File Offset: 0x004C85E7
			public void Error()
			{
				this.HasErrored = true;
			}

			// Token: 0x0400A181 RID: 41345
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformUserManager.PlatformUserData m_userData;
		}

		// Token: 0x02001BDA RID: 7130
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserDetailsResult
		{
			// Token: 0x0600D44D RID: 54349 RVA: 0x004CA420 File Offset: 0x004C8620
			public PlatformUserDetailsResult(PlatformUserManager.PlatformUserData userData)
			{
				this.UserData = userData;
			}

			// Token: 0x0400A186 RID: 41350
			public readonly PlatformUserManager.PlatformUserData UserData;

			// Token: 0x0400A187 RID: 41351
			public string Name;
		}
	}
}
