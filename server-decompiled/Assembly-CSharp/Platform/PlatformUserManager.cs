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
	// Token: 0x02001C06 RID: 7174
	public static class PlatformUserManager
	{
		// Token: 0x0600D583 RID: 54659 RVA: 0x004CDC81 File Offset: 0x004CBE81
		[Conditional("PLATFORM_USER_MANAGER_DEBUG")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string message)
		{
			Log.Out("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D584 RID: 54660 RVA: 0x004CDC81 File Offset: 0x004CBE81
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string message)
		{
			Log.Out("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D585 RID: 54661 RVA: 0x004CDC93 File Offset: 0x004CBE93
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string message)
		{
			Log.Warning("[PlatformUserManager] " + message);
		}

		// Token: 0x0600D586 RID: 54662 RVA: 0x004CDCA5 File Offset: 0x004CBEA5
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string message)
		{
			Log.Error("[PlatformUserManager] " + message);
		}

		// Token: 0x14000127 RID: 295
		// (add) Token: 0x0600D587 RID: 54663 RVA: 0x004CDCB8 File Offset: 0x004CBEB8
		// (remove) Token: 0x0600D588 RID: 54664 RVA: 0x004CDCEC File Offset: 0x004CBEEC
		public static event PlatformUserBlockedStateChangedHandler BlockedStateChanged;

		// Token: 0x14000128 RID: 296
		// (add) Token: 0x0600D589 RID: 54665 RVA: 0x004CDD20 File Offset: 0x004CBF20
		// (remove) Token: 0x0600D58A RID: 54666 RVA: 0x004CDD54 File Offset: 0x004CBF54
		public static event PlatformUserDetailsUpdatedHandler DetailsUpdated;

		// Token: 0x0600D58B RID: 54667 RVA: 0x004CDD88 File Offset: 0x004CBF88
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

		// Token: 0x0600D58C RID: 54668 RVA: 0x004CDE60 File Offset: 0x004CC060
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

		// Token: 0x0600D58D RID: 54669 RVA: 0x004CDF34 File Offset: 0x004CC134
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

		// Token: 0x0600D58E RID: 54670 RVA: 0x004CDF78 File Offset: 0x004CC178
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

		// Token: 0x0600D58F RID: 54671 RVA: 0x004CE00C File Offset: 0x004CC20C
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

		// Token: 0x0600D590 RID: 54672 RVA: 0x004CE080 File Offset: 0x004CC280
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

		// Token: 0x0600D591 RID: 54673 RVA: 0x004CE0EC File Offset: 0x004CC2EC
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

		// Token: 0x0600D592 RID: 54674 RVA: 0x004CE0FB File Offset: 0x004CC2FB
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

		// Token: 0x0600D593 RID: 54675 RVA: 0x004CE10C File Offset: 0x004CC30C
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

		// Token: 0x0600D594 RID: 54676 RVA: 0x004CE1D8 File Offset: 0x004CC3D8
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

		// Token: 0x0600D595 RID: 54677 RVA: 0x004CE1E7 File Offset: 0x004CC3E7
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

		// Token: 0x0600D596 RID: 54678 RVA: 0x004CE1F8 File Offset: 0x004CC3F8
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

		// Token: 0x0600D597 RID: 54679 RVA: 0x004CE250 File Offset: 0x004CC450
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

		// Token: 0x0600D598 RID: 54680 RVA: 0x004CE2B8 File Offset: 0x004CC4B8
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

		// Token: 0x0600D599 RID: 54681 RVA: 0x004CE2F4 File Offset: 0x004CC4F4
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

		// Token: 0x0600D59A RID: 54682 RVA: 0x004CE45C File Offset: 0x004CC65C
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

		// Token: 0x0600D59B RID: 54683 RVA: 0x004CE520 File Offset: 0x004CC720
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

		// Token: 0x0600D59C RID: 54684 RVA: 0x004CE528 File Offset: 0x004CC728
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

		// Token: 0x0600D59D RID: 54685 RVA: 0x004CE658 File Offset: 0x004CC858
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

		// Token: 0x0600D59E RID: 54686 RVA: 0x004CE660 File Offset: 0x004CC860
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

		// Token: 0x0600D59F RID: 54687 RVA: 0x004CE6DC File Offset: 0x004CC8DC
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

		// Token: 0x0600D5A0 RID: 54688 RVA: 0x004CE834 File Offset: 0x004CCA34
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool CanCheckUserDetails()
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			return ((crossplatformPlatform != null) ? crossplatformPlatform.UserDetailsService : null) != null || PlatformManager.NativePlatform.UserDetailsService != null;
		}

		// Token: 0x0600D5A1 RID: 54689 RVA: 0x004CE858 File Offset: 0x004CCA58
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

		// Token: 0x0600D5A2 RID: 54690 RVA: 0x004CE867 File Offset: 0x004CCA67
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

		// Token: 0x0400A230 RID: 41520
		public const int PrimaryIdsPerNativeIdLimit = 3;

		// Token: 0x0400A231 RID: 41521
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool s_enabled;

		// Token: 0x0400A232 RID: 41522
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<PlatformUserIdentifierAbs, PlatformUserManager.PlatformUserData> s_primaryIdToPlatform;

		// Token: 0x0400A233 RID: 41523
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_primaryIdToPlatformLock;

		// Token: 0x0400A234 RID: 41524
		[PublicizedFrom(EAccessModifier.Private)]
		public static OneToManyDictionary<PlatformUserIdentifierAbs, PlatformUserIdentifierAbs> s_nativeIdToPrimaryIds;

		// Token: 0x0400A235 RID: 41525
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_nativeIdToPrimaryIdsLock;

		// Token: 0x0400A236 RID: 41526
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_nativeUserIdsSeen;

		// Token: 0x0400A237 RID: 41527
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_nativeUserIdsSeenLock;

		// Token: 0x0400A238 RID: 41528
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PermissionFrameFrequency = 60;

		// Token: 0x0400A239 RID: 41529
		[PublicizedFrom(EAccessModifier.Private)]
		public static EUserPerms s_lastPermissions;

		// Token: 0x0400A23A RID: 41530
		[PublicizedFrom(EAccessModifier.Private)]
		public const int PersistentFrameFrequency = 300;

		// Token: 0x0400A23B RID: 41531
		[PublicizedFrom(EAccessModifier.Private)]
		public static PersistentPlayerList s_persistentPlayerListLast;

		// Token: 0x0400A23C RID: 41532
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_persistentIdsTemp;

		// Token: 0x0400A23D RID: 41533
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserIdentifierAbs> s_persistentIdsLast;

		// Token: 0x0400A23E RID: 41534
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserManager.PlatformUserData> s_blockedUsersToUpdate;

		// Token: 0x0400A23F RID: 41535
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_blockedUsersToUpdateLock;

		// Token: 0x0400A240 RID: 41536
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<PlatformUserManager.PlatformUserBlockedResults> s_blockedDataCurrentlyUpdating;

		// Token: 0x0400A241 RID: 41537
		[PublicizedFrom(EAccessModifier.Private)]
		public static IReadOnlyList<IPlatformUserBlockedResults> s_blockedDataCurrentlyUpdatingReadOnly;

		// Token: 0x0400A243 RID: 41539
		[PublicizedFrom(EAccessModifier.Private)]
		public static HashSet<PlatformUserManager.PlatformUserData> s_userDetailsToUpdate;

		// Token: 0x0400A244 RID: 41540
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<PlatformUserManager.PlatformUserDetailsResult> s_userDetailsCurrentlyUpdating;

		// Token: 0x0400A245 RID: 41541
		[PublicizedFrom(EAccessModifier.Private)]
		public static ReaderWriterLockSlim s_userDetailsToUpdateLock;

		// Token: 0x0400A246 RID: 41542
		[PublicizedFrom(EAccessModifier.Private)]
		public const int UserDetailsMaxAttempts = 5;

		// Token: 0x0400A247 RID: 41543
		[PublicizedFrom(EAccessModifier.Private)]
		public const float UserDetailsRetryBaseDelay = 0.5f;

		// Token: 0x0400A248 RID: 41544
		[PublicizedFrom(EAccessModifier.Private)]
		public const float UserDetailsRetryMaxDelay = 30f;

		// Token: 0x02001C07 RID: 7175
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserData : IPlatformUserData, IPlatformUser
		{
			// Token: 0x0600D5A3 RID: 54691 RVA: 0x004CE88C File Offset: 0x004CCA8C
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

			// Token: 0x0600D5A4 RID: 54692 RVA: 0x004CE914 File Offset: 0x004CCB14
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

			// Token: 0x17001AE6 RID: 6886
			// (get) Token: 0x0600D5A5 RID: 54693 RVA: 0x004CE98B File Offset: 0x004CCB8B
			public PlatformUserIdentifierAbs PrimaryId { get; }

			// Token: 0x17001AE7 RID: 6887
			// (get) Token: 0x0600D5A6 RID: 54694 RVA: 0x004CE994 File Offset: 0x004CCB94
			// (set) Token: 0x0600D5A7 RID: 54695 RVA: 0x004CE9E8 File Offset: 0x004CCBE8
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

			// Token: 0x17001AE8 RID: 6888
			// (get) Token: 0x0600D5A8 RID: 54696 RVA: 0x004CEB58 File Offset: 0x004CCD58
			// (set) Token: 0x0600D5A9 RID: 54697 RVA: 0x004CEB60 File Offset: 0x004CCD60
			public string Name { get; set; }

			// Token: 0x0600D5AA RID: 54698 RVA: 0x004CEB6C File Offset: 0x004CCD6C
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

			// Token: 0x17001AE9 RID: 6889
			// (get) Token: 0x0600D5AB RID: 54699 RVA: 0x004CEBCC File Offset: 0x004CCDCC
			public IReadOnlyDictionary<EBlockType, PlatformUserManager.PlatformUserBlockedData> Blocked
			{
				get
				{
					return this.m_userBlockedStates;
				}
			}

			// Token: 0x17001AEA RID: 6890
			// (get) Token: 0x0600D5AC RID: 54700 RVA: 0x004CEBD4 File Offset: 0x004CCDD4
			public IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> Platform.IPlatformUserData.Blocked
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.m_userBlockedStatesReadOnly;
				}
			}

			// Token: 0x0600D5AD RID: 54701 RVA: 0x004CEBDC File Offset: 0x004CCDDC
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

			// Token: 0x0400A24A RID: 41546
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly EnumDictionary<EBlockType, PlatformUserManager.PlatformUserBlockedData> m_userBlockedStates;

			// Token: 0x0400A24B RID: 41547
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly IReadOnlyDictionary<EBlockType, IPlatformUserBlockedData> m_userBlockedStatesReadOnly;

			// Token: 0x0400A24E RID: 41550
			public int DetailAttempts;

			// Token: 0x0400A24F RID: 41551
			public float NextDetailRetryTime;
		}

		// Token: 0x02001C09 RID: 7177
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserBlockedData : IPlatformUserBlockedData
		{
			// Token: 0x0600D5B1 RID: 54705 RVA: 0x004CEC87 File Offset: 0x004CCE87
			public PlatformUserBlockedData(PlatformUserManager.PlatformUserData userData, EBlockType blockType)
			{
				this.m_userData = userData;
				this.Type = blockType;
				this.m_blockedLocally = false;
				this.State = EUserBlockState.NotBlocked;
			}

			// Token: 0x0600D5B2 RID: 54706 RVA: 0x004CECAC File Offset: 0x004CCEAC
			public override string ToString()
			{
				return string.Format("{0}[Type={1}, State={2}, Locally={3}]", new object[] { "PlatformUserBlockedData", this.Type, this.State, this.Locally });
			}

			// Token: 0x17001AEB RID: 6891
			// (get) Token: 0x0600D5B3 RID: 54707 RVA: 0x004CECFB File Offset: 0x004CCEFB
			public EBlockType Type { get; }

			// Token: 0x17001AEC RID: 6892
			// (get) Token: 0x0600D5B4 RID: 54708 RVA: 0x004CED03 File Offset: 0x004CCF03
			// (set) Token: 0x0600D5B5 RID: 54709 RVA: 0x004CED0B File Offset: 0x004CCF0B
			public EUserBlockState State
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17001AED RID: 6893
			// (get) Token: 0x0600D5B6 RID: 54710 RVA: 0x004CED14 File Offset: 0x004CCF14
			// (set) Token: 0x0600D5B7 RID: 54711 RVA: 0x004CED1C File Offset: 0x004CCF1C
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

			// Token: 0x0600D5B8 RID: 54712 RVA: 0x004CED34 File Offset: 0x004CCF34
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

			// Token: 0x0400A252 RID: 41554
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformUserManager.PlatformUserData m_userData;

			// Token: 0x0400A253 RID: 41555
			[PublicizedFrom(EAccessModifier.Private)]
			public bool m_blockedLocally;
		}

		// Token: 0x02001C0A RID: 7178
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserBlockedResults : IPlatformUserBlockedResults
		{
			// Token: 0x0600D5B9 RID: 54713 RVA: 0x004CED84 File Offset: 0x004CCF84
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

			// Token: 0x0600D5BA RID: 54714 RVA: 0x004CEDF8 File Offset: 0x004CCFF8
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

			// Token: 0x17001AEE RID: 6894
			// (get) Token: 0x0600D5BB RID: 54715 RVA: 0x004CEE9F File Offset: 0x004CD09F
			public PlatformUserManager.PlatformUserData User
			{
				get
				{
					return this.m_userData;
				}
			}

			// Token: 0x17001AEF RID: 6895
			// (get) Token: 0x0600D5BC RID: 54716 RVA: 0x004CEEA7 File Offset: 0x004CD0A7
			public EnumDictionary<EBlockType, bool> IsBlocked { get; }

			// Token: 0x17001AF0 RID: 6896
			// (get) Token: 0x0600D5BD RID: 54717 RVA: 0x004CEEAF File Offset: 0x004CD0AF
			// (set) Token: 0x0600D5BE RID: 54718 RVA: 0x004CEEB7 File Offset: 0x004CD0B7
			public bool HasErrored
			{
				get; [PublicizedFrom(EAccessModifier.Private)]
				set;
			}

			// Token: 0x17001AF1 RID: 6897
			// (get) Token: 0x0600D5BF RID: 54719 RVA: 0x004CEE9F File Offset: 0x004CD09F
			public IPlatformUser Platform.IPlatformUserBlockedResults.User
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.m_userData;
				}
			}

			// Token: 0x0600D5C0 RID: 54720 RVA: 0x004CEEC0 File Offset: 0x004CD0C0
			public void Block(EBlockType blockType)
			{
				this.IsBlocked[blockType] = true;
			}

			// Token: 0x0600D5C1 RID: 54721 RVA: 0x004CEECF File Offset: 0x004CD0CF
			public void Error()
			{
				this.HasErrored = true;
			}

			// Token: 0x0400A256 RID: 41558
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly PlatformUserManager.PlatformUserData m_userData;
		}

		// Token: 0x02001C0C RID: 7180
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class PlatformUserDetailsResult
		{
			// Token: 0x0600D5C5 RID: 54725 RVA: 0x004CEF08 File Offset: 0x004CD108
			public PlatformUserDetailsResult(PlatformUserManager.PlatformUserData userData)
			{
				this.UserData = userData;
			}

			// Token: 0x0400A25B RID: 41563
			public readonly PlatformUserManager.PlatformUserData UserData;

			// Token: 0x0400A25C RID: 41564
			public string Name;
		}
	}
}
