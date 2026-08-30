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
	// Token: 0x02001BAD RID: 7085
	public static class PlatformUserManager
	{
		// Token: 0x0600D27D RID: 53885 RVA: 0x004C2DA5 File Offset: 0x004C0FA5
		[Conditional("PLATFORM_USER_MANAGER_DEBUG")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string message)
		{
			Log.Out("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D27E RID: 53886 RVA: 0x004C2DA5 File Offset: 0x004C0FA5
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string message)
		{
			Log.Out("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D27F RID: 53887 RVA: 0x004C2DB7 File Offset: 0x004C0FB7
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string message)
		{
			Log.Warning("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D280 RID: 53888 RVA: 0x004C2DC9 File Offset: 0x004C0FC9
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string message)
		{
			Log.Error("[PlatformUserManager] " + message);
		}

		// Token: 0x14000127 RID: 295
		// (add) Token: 0x0600D281 RID: 53889 RVA: 0x004C2DDC File Offset: 0x004C0FDC
		// (remove) Token: 0x0600D282 RID: 53890 RVA: 0x004C2E10 File Offset: 0x004C1010
		public static event PlatformUserBlockedStateChangedHandler BlockedStateChanged;

		// Token: 0x14000128 RID: 296
		// (add) Token: 0x0600D283 RID: 53891 RVA: 0x004C2E44 File Offset: 0x004C1044
		// (remove) Token: 0x0600D284 RID: 53892 RVA: 0x004C2E78 File Offset: 0x004C1078
		public static event PlatformUserDetailsUpdatedHandler DetailsUpdated;

		// Token: 0x0600D285 RID: 53893 RVA: 0x004C2EAC File Offset: 0x004C10AC
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

		// Token: 0x0600D286 RID: 53894 RVA: 0x004C2F84 File Offset: 0x004C1184
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

		// Token: 0x0600D287 RID: 53895 RVA: 0x004C3058 File Offset: 0x004C1258
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

		// Token: 0x0600D288 RID: 53896 RVA: 0x004C309C File Offset: 0x004C129C
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

		// Token: 0x0600D289 RID: 53897 RVA: 0x004C3130 File Offset: 0x004C1330
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

		// Token: 0x0600D28A RID: 53898 RVA: 0x004C31A4 File Offset: 0x004C13A4
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

		// Token: 0x0600D28B RID: 53899 RVA: 0x004C3210 File Offset: 0x004C1410
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

		// Token: 0x0600D28C RID: 53900 RVA: 0x004C321F File Offset: 0x004C141F
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

		// Token: 0x0600D28D RID: 53901 RVA: 0x004C3230 File Offset: 0x004C1430
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

		// Token: 0x0600D28E RID: 53902 RVA: 0x004C32FC File Offset: 0x004C14FC
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

		// Token: 0x0600D28F RID: 53903 RVA: 0x004C330B File Offset: 0x004C150B
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

		// Token: 0x0600D290 RID: 53904 RVA: 0x004C331C File Offset: 0x004C151C
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

		// Token: 0x0600D291 RID: 53905 RVA: 0x004C3374 File Offset: 0x004C1574
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

		// Token: 0x0600D292 RID: 53906 RVA: 0x004C33DC File Offset: 0x004C15DC
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

		// Token: 0x0600D293 RID: 53907 RVA: 0x004C3418 File Offset: 0x004C1618
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

		// Token: 0x0600D294 RID: 53908 RVA: 0x004C3580 File Offset: 0x004C1780
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

		// Token: 0x0600D295 RID: 53909 RVA: 0x004C3644 File Offset: 0x004C1844
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

		// Token: 0x0600D296 RID: 53910 RVA: 0x004C364C File Offset: 0x004C184C
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

		// Token: 0x0600D297 RID: 53911 RVA: 0x004C377C File Offset: 0x004C197C
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

		// Token: 0x0600D298 RID: 53912 RVA: 0x004C3784 File Offset: 0x004C1984
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

		// Token: 0x0600D299 RID: 53913 RVA: 0x004C3800 File Offset: 0x004C1A00
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

		// Token: 0x0600D29A RID: 53914 RVA: 0x004C3958 File Offset: 0x004C1B58
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool CanCheckUserDetails()
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			return ((crossplatformPlatform != null) ? crossplatformPlatform.UserDetailsService : null) != null || PlatformManager.NativePlatform.UserDetailsService != null;
		}

		// Token: 0x0600D29B RID: 53915 RVA: 0x004C397C File Offset: 0x004C1B7C
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

		// Token: 0x0600D29C RID: 53916 RVA: 0x004C398B File Offset: 0x004C1B8B
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

		// Token: 0x0400A02A RID: 41002
		public const int PrimaryIdsPerNativeIdLimit = 3;

		// Token: 0x0400A02B RID: 41003
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool s_enabled;

		// Token: 0x0400A02C RID: 41004
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<PlatformUserIdentifierAbs, PlatformUserManager.PlatformUserData> s_primaryIdToPlatform;

		// Token: 0x0400A02D RID: 41005
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_primaryIdToPlatformLock;

		// Token: 0x0400A02E RID: 41006
		[PublicizedFrom(EAccessModifier.Private)]
		public static OneToManyDictionary<PlatformUserIdentifierAbs, PlatformUserIdentifierAbs> s_nativeIdToPrimaryIds;

		// Token: 0x0400A02F RID: 41007
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_nativeIdToPrimaryIdsLock;

		// Token: 0x0400A030 RID: 41008
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_nativeUserIdsSeen;

		// Token: 0x0400A031 RID: 41009
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_nativeUserIdsSeenLock;

		// Token: 0x0400A032 RID: 41010
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PermissionFrameFrequency = 60;

		// Token: 0x0400A033 RID: 41011
		[PublicizedFrom(EAccessModifier.Private)]
		public static EUserPerms s_lastPermissions;

		// Token: 0x0400A034 RID: 41012
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PersistentFrameFrequency = 300;

		// Token: 0x0400A035 RID: 41013
		[PublicizedFrom(EAccessModifier.Private)]
		public static PersistentPlayerList s_persistentPlayerListLast;

		// Token: 0x0400A036 RID: 41014
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_persistentIdsTemp;

		// Token: 0x0400A037 RID: 41015
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_persistentIdsLast;

		// Token: 0x0400A038 RID: 41016
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserManager.PlatformUserData> s_blockedUsersToUpdate;

		// Token: 0x0400A039 RID: 41017
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_blockedUsersToUpdateLock;

		// Token: 0x0400A03A RID: 41018
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<PlatformUserManager.PlatformUserBlockedResults> s_blockedDataCurrentlyUpdating;

		// Token: 0x0400A03B RID: 41019
		[PublicizedFrom(EAccessModifier.Private)]
		public static IReadOnlyList<IPlatformUserBlockedResults> s_blockedDataCurrentlyUpdatingReadOnly;

		// Token: 0x0400A03D RID: 41021
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserManager.PlatformUserData> s_userDetailsToUpdate;

		// Token: 0x0400A03E RID: 41022
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<PlatformUserManager.PlatformUserDetailsResult> s_userDetailsCurrentlyUpdating;

		// Token: 0x0400A03F RID: 41023
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_userDetailsToUpdateLock;

		// Token: 0x0400A040 RID: 41024
		[PublicizedFrom(EAccessModifier.Private)]
		public const int UserDetailsMaxAttempts = 5;

		// Token: 0x0400A041 RID: 41025
		[PublicizedFrom(EAccessModifier.Private)]
		public const float UserDetailsRetryBaseDelay = 0.5f;

		// Token: 0x0400A042 RID: 41026
		[PublicizedFrom(EAccessModifier.Private)]
		public const float UserDetailsRetryMaxDelay = 30f;

		// Token: 0x02001BAE RID: 7086
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserData : IPlatformUserData, IPlatformUser
		{
			// Token: 0x0600D29D RID: 53917 RVA: 0x004C39B0 File Offset: 0x004C1BB0
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

			// Token: 0x0600D29E RID: 53918 RVA: 0x004C3A38 File Offset: 0x004C1C38
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

			// Token: 0x170019CE RID: 6606
			// (get) Token: 0x0600D29F RID: 53919 RVA: 0x004C3AAF File Offset: 0x004C1CAF
			public PlatformUserIdentifierAbs PrimaryId { get; }

			// Token: 0x170019CF RID: 6607
			// (get) Token: 0x0600D2A0 RID: 53920 RVA: 0x004C3AB8 File Offset: 0x004C1CB8
			// (set) Token: 0x0600D2A1 RID: 53921 RVA: 0x004C3B0C File Offset: 0x004C1D0C
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

			// Token: 0x170019D0 RID: 6608
			// (get) Token: 0x0600D2A2 RID: 53922 RVA: 0x004C3C7C File Offset: 0x004C1E7C
			// (set) Token: 0x0600D2A3 RID: 53923 RVA: 0x004C3C84 File Offset: 0x004C1E84
			public string Name { get; set; }

			// Token: 0x0600D2A4 RID: 53924 RVA: 0x004C3C90 File Offset: 0x004C1E90
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

			// Token: 0x170019D1 RID: 6609
			// (get) Token: 0x0600D2A5 RID: 53925 RVA: 0x004C3CF0 File Offset: 0x004C1EF0
			public IReadOnlyDictionary<EBlockType, PlatformUserManager.PlatformUserBlockedData> Blocked
			{
				get
				{
					return this.m_userBlockedStates;
				}
			}

			// Token: 0x170019D2 RID: 6610
			// (get) Token: 0x0600D2A6 RID: 53926 RVA: 0x004C3CF8 File Offset: 0x004C1EF8
			public IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> Platform.IPlatformUserData.Blocked
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.m_userBlockedStatesReadOnly;
				}
			}

			// Token: 0x0600D2A7 RID: 53927 RVA: 0x004C3D00 File Offset: 0x004C1F00
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

			// Token: 0x0400A044 RID: 41028
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly EnumDictionary<EBlockType, PlatformUserManager.PlatformUserBlockedData> m_userBlockedStates;

			// Token: 0x0400A045 RID: 41029
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> m_userBlockedStatesReadOnly;

			// Token: 0x0400A048 RID: 41032
			public int DetailAttempts;

			// Token: 0x0400A049 RID: 41033
			public float NextDetailRetryTime;
		}

		// Token: 0x02001BB0 RID: 7088
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserBlockedData : IPlatformUserBlockedData
		{
			// Token: 0x0600D2AB RID: 53931 RVA: 0x004C3DAB File Offset: 0x004C1FAB
			public PlatformUserBlockedData(PlatformUserManager.PlatformUserData userData, EBlockType blockType)
			{
				this.m_userData = userData;
				this.Type = blockType;
				this.m_blockedLocally = false;
				this.State = EUserBlockState.NotBlocked;
			}

			// Token: 0x0600D2AC RID: 53932 RVA: 0x004C3DD0 File Offset: 0x004C1FD0
			public override string ToString()
			{
				return string.Format("{0}[Type={1}, State={2}, Locally={3}]", new object[] { "PlatformUserBlockedData", this.Type, this.State, this.Locally });
			}

			// Token: 0x170019D3 RID: 6611
			// (get) Token: 0x0600D2AD RID: 53933 RVA: 0x004C3E1F File Offset: 0x004C201F
			public EBlockType Type { get; }

			// Token: 0x170019D4 RID: 6612
			// (get) Token: 0x0600D2AE RID: 53934 RVA: 0x004C3E27 File Offset: 0x004C2027
			// (set) Token: 0x0600D2AF RID: 53935 RVA: 0x004C3E2F File Offset: 0x004C202F
			public EUserBlockState State
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x170019D5 RID: 6613
			// (get) Token: 0x0600D2B0 RID: 53936 RVA: 0x004C3E38 File Offset: 0x004C2038
			// (set) Token: 0x0600D2B1 RID: 53937 RVA: 0x004C3E40 File Offset: 0x004C2040
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

			// Token: 0x0600D2B2 RID: 53938 RVA: 0x004C3E58 File Offset: 0x004C2058
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

			// Token: 0x0400A04C RID: 41036
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformUserManager.PlatformUserData m_userData;

			// Token: 0x0400A04D RID: 41037
			[PublicizedFrom(EAccessModifier.Private)]
			public bool m_blockedLocally;
		}

		// Token: 0x02001BB1 RID: 7089
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserBlockedResults : IPlatformUserBlockedResults
		{
			// Token: 0x0600D2B3 RID: 53939 RVA: 0x004C3EA8 File Offset: 0x004C20A8
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

			// Token: 0x0600D2B4 RID: 53940 RVA: 0x004C3F1C File Offset: 0x004C211C
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

			// Token: 0x170019D6 RID: 6614
			// (get) Token: 0x0600D2B5 RID: 53941 RVA: 0x004C3FC3 File Offset: 0x004C21C3
			public PlatformUserManager.PlatformUserData User
			{
				get
				{
					return this.m_userData;
				}
			}

			// Token: 0x170019D7 RID: 6615
			// (get) Token: 0x0600D2B6 RID: 53942 RVA: 0x004C3FCB File Offset: 0x004C21CB
			public EnumDictionary<EBlockType, bool> IsBlocked { get; }

			// Token: 0x170019D8 RID: 6616
			// (get) Token: 0x0600D2B7 RID: 53943 RVA: 0x004C3FD3 File Offset: 0x004C21D3
			// (set) Token: 0x0600D2B8 RID: 53944 RVA: 0x004C3FDB File Offset: 0x004C21DB
			public bool HasErrored
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x170019D9 RID: 6617
			// (get) Token: 0x0600D2B9 RID: 53945 RVA: 0x004C3FC3 File Offset: 0x004C21C3
			public IPlatformUser Platform.IPlatformUserBlockedResults.User
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.m_userData;
				}
			}

			// Token: 0x0600D2BA RID: 53946 RVA: 0x004C3FE4 File Offset: 0x004C21E4
			public void Block(EBlockType blockType)
			{
				this.IsBlocked[blockType] = true;
			}

			// Token: 0x0600D2BB RID: 53947 RVA: 0x004C3FF3 File Offset: 0x004C21F3
			public void Error()
			{
				this.HasErrored = true;
			}

			// Token: 0x0400A050 RID: 41040
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformUserManager.PlatformUserData m_userData;
		}

		// Token: 0x02001BB3 RID: 7091
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserDetailsResult
		{
			// Token: 0x0600D2BF RID: 53951 RVA: 0x004C402C File Offset: 0x004C222C
			public PlatformUserDetailsResult(PlatformUserManager.PlatformUserData userData)
			{
				this.UserData = userData;
			}

			// Token: 0x0400A055 RID: 41045
			public readonly PlatformUserManager.PlatformUserData UserData;

			// Token: 0x0400A056 RID: 41046
			public string Name;
		}
	}
}
