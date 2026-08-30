using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Platform.XBL
{
	// Token: 0x02001C21 RID: 7201
	public static class XblXuidMapper
	{
		// Token: 0x14000131 RID: 305
		// (add) Token: 0x0600D5E0 RID: 54752 RVA: 0x004CF9C8 File Offset: 0x004CDBC8
		// (remove) Token: 0x0600D5E1 RID: 54753 RVA: 0x004CF9FC File Offset: 0x004CDBFC
		public static event XblXuidMapper.XuidMappedHandler XuidMapped;

		// Token: 0x14000132 RID: 306
		// (add) Token: 0x0600D5E2 RID: 54754 RVA: 0x004CFA30 File Offset: 0x004CDC30
		// (remove) Token: 0x0600D5E3 RID: 54755 RVA: 0x004CFA64 File Offset: 0x004CDC64
		public static event XblXuidMapper.UserIdentifierMappedHandler UserIdentifierMapped;

		// Token: 0x0600D5E5 RID: 54757 RVA: 0x004CFB09 File Offset: 0x004CDD09
		public static void Enable()
		{
			if (XblXuidMapper.s_enabled)
			{
				return;
			}
			XblXuidMapper.s_enabled = true;
			Log.Out("[XBL-XuidMapper] Enabled.");
		}

		// Token: 0x0600D5E6 RID: 54758 RVA: 0x004CFB24 File Offset: 0x004CDD24
		public static ulong GetXuid(PlatformUserIdentifierAbs userId)
		{
			if (!XblXuidMapper.s_enabled)
			{
				return 0UL;
			}
			XblXuidMapper.XuidState xuidState = XblXuidMapper.GetXuidState(userId);
			using (XblXuidMapper.s_xuidStateDictionaryLock.ReadLockScope())
			{
				ulong num;
				if (XblXuidMapper.s_xuidStateToXuid.TryGetByKey(xuidState, out num))
				{
					return num;
				}
			}
			if (XblXuidMapper.s_mappingService == null)
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				XblXuidMapper.s_mappingService = ((crossplatformPlatform != null) ? crossplatformPlatform.IdMappingService : null);
			}
			if (XblXuidMapper.s_mappingService == null)
			{
				Log.Error("[XBL-XuidMapper] ID mapping service required to identify Xbl users");
				return 0UL;
			}
			XblXuidMapper.ResolveXuid(xuidState);
			return 0UL;
		}

		// Token: 0x0600D5E7 RID: 54759 RVA: 0x004CFBBC File Offset: 0x004CDDBC
		public static void SetXuid(PlatformUserIdentifierAbs userId, ulong xuid)
		{
			if (xuid == 0UL)
			{
				return;
			}
			XblXuidMapper.XuidState xuidState = XblXuidMapper.GetXuidState(userId);
			ulong num;
			using (XblXuidMapper.s_xuidStateDictionaryLock.UpgradableReadLockScope())
			{
				if (!XblXuidMapper.s_xuidStateToXuid.TryGetByKey(xuidState, out num))
				{
					num = 0UL;
				}
				if (num == xuid)
				{
					return;
				}
				using (XblXuidMapper.s_xuidStateDictionaryLock.WriteLockScope())
				{
					XblXuidMapper.s_xuidStateToXuid.RemoveByKey(xuidState);
					XblXuidMapper.s_xuidStateToXuid.RemoveByValue(xuid);
					XblXuidMapper.s_xuidStateToXuid.Add(xuidState, xuid);
				}
				XblXuidMapper.XuidState xuidState2 = xuidState;
				lock (xuidState2)
				{
					xuidState.InProgress = false;
					xuidState.AttemptsCompleted++;
				}
			}
			if (num != 0UL)
			{
				using (XblXuidMapper.s_xuidStateDictionaryLock.ReadLockScope())
				{
					IReadOnlyCollection<PlatformUserIdentifierAbs> readOnlyCollection;
					XblXuidMapper.s_xuidStateToUserId.TryGetByKey(xuidState, out readOnlyCollection);
					Log.Warning(string.Format("[XBL-XuidMapper] Unexpected mapping change Xuid changed from '{0}' to '{1}' for UserIds: {2}", num, xuid, string.Join<PlatformUserIdentifierAbs>(", ", readOnlyCollection)));
				}
			}
			object obj = XblXuidMapper.s_xuidMappedResultTempLock;
			lock (obj)
			{
				XblXuidMapper.s_xuidMappedResultTemp.Clear();
				using (XblXuidMapper.s_xuidStateDictionaryLock.ReadLockScope())
				{
					XblXuidMapper.s_xuidStateToUserId.TryGetByKey(xuidState, XblXuidMapper.s_xuidMappedResultTemp);
				}
				XblXuidMapper.XuidMappedHandler xuidMapped = XblXuidMapper.XuidMapped;
				if (xuidMapped != null)
				{
					xuidMapped(XblXuidMapper.s_xuidMappedResultTemp, xuid);
				}
				XblXuidMapper.s_xuidMappedResultTemp.Clear();
			}
		}

		// Token: 0x0600D5E8 RID: 54760 RVA: 0x004CFD94 File Offset: 0x004CDF94
		[PublicizedFrom(EAccessModifier.Private)]
		public static void ResolveXuid(XblXuidMapper.XuidState xuidState)
		{
			XblXuidMapper.<>c__DisplayClass24_0 CS$<>8__locals1 = new XblXuidMapper.<>c__DisplayClass24_0();
			CS$<>8__locals1.xuidState = xuidState;
			CS$<>8__locals1.userIds = null;
			XblXuidMapper.XuidState xuidState2 = CS$<>8__locals1.xuidState;
			lock (xuidState2)
			{
				if (CS$<>8__locals1.xuidState.InProgress)
				{
					return;
				}
				using (XblXuidMapper.s_xuidStateDictionaryLock.ReadLockScope())
				{
					IReadOnlyCollection<PlatformUserIdentifierAbs> readOnlyCollection;
					if (XblXuidMapper.s_xuidStateToUserId.TryGetByKey(CS$<>8__locals1.xuidState, out readOnlyCollection))
					{
						foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs in readOnlyCollection)
						{
							if (XblXuidMapper.s_mappingService.CanQuery(platformUserIdentifierAbs))
							{
								using (XblXuidMapper.s_userIdsWithNoXblMappingLock.ReadLockScope())
								{
									if (XblXuidMapper.s_userIdsWithNoXblMapping.Contains(platformUserIdentifierAbs))
									{
										continue;
									}
								}
								if (CS$<>8__locals1.userIds == null)
								{
									CS$<>8__locals1.userIds = new List<PlatformUserIdentifierAbs>();
								}
								CS$<>8__locals1.userIds.Add(platformUserIdentifierAbs);
							}
						}
					}
				}
				if (CS$<>8__locals1.userIds == null || CS$<>8__locals1.userIds.Count <= 0)
				{
					return;
				}
				CS$<>8__locals1.attempt = CS$<>8__locals1.xuidState.AttemptsCompleted + 1;
				CS$<>8__locals1.xuidState.InProgress = true;
			}
			CS$<>8__locals1.userIdsIndex = -1;
			CS$<>8__locals1.<ResolveXuid>g__ProcessNextId|1();
		}

		// Token: 0x0600D5E9 RID: 54761 RVA: 0x004CFF44 File Offset: 0x004CE144
		public static void ResolveUserIdentifiers(IReadOnlyCollection<ulong> xuids)
		{
			XblXuidMapper.<>c__DisplayClass25_0 CS$<>8__locals1 = new XblXuidMapper.<>c__DisplayClass25_0();
			if (XblXuidMapper.s_mappingService == null)
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				XblXuidMapper.s_mappingService = ((crossplatformPlatform != null) ? crossplatformPlatform.IdMappingService : null);
			}
			if (XblXuidMapper.s_mappingService == null)
			{
				return;
			}
			using (XblXuidMapper.s_xuidsCheckedForUserIdentifiersLock.ReadLockScope())
			{
				CS$<>8__locals1.xuidsToRequest = (from xuid in xuids
					where !XblXuidMapper.s_xuidsCheckedForUserIdentifiers.Contains(xuid)
					where XblXuidMapper.s_mappingService.CanReverseQuery(EPlatformIdentifier.XBL, xuid.ToString())
					select xuid).ToArray<ulong>();
			}
			if (CS$<>8__locals1.xuidsToRequest.Length == 0)
			{
				return;
			}
			CS$<>8__locals1.requests = CS$<>8__locals1.xuidsToRequest.Select<ulong, MappedAccountReverseRequest>((ulong xuid) => new MappedAccountReverseRequest(EPlatformIdentifier.XBL, xuid.ToString())).ToArray<MappedAccountReverseRequest>();
			Log.Out("[XBL-XuidMapper] Reverse mapping XUID(s) '" + string.Join<ulong>("','", xuids) + "'...");
			XblXuidMapper.s_mappingService.ReverseQueryMappedAccountsDetails(CS$<>8__locals1.requests, new MappedAccountsReverseQueryCallback(CS$<>8__locals1.<ResolveUserIdentifiers>g__ResolveUserIdentifiersCallback|1));
		}

		// Token: 0x0600D5EA RID: 54762 RVA: 0x004D0078 File Offset: 0x004CE278
		[PublicizedFrom(EAccessModifier.Private)]
		public static XblXuidMapper.XuidState GetXuidState(PlatformUserIdentifierAbs userId)
		{
			XblXuidMapper.XuidState xuidState2;
			using (XblXuidMapper.s_xuidStateDictionaryLock.UpgradableReadLockScope())
			{
				XblXuidMapper.XuidState xuidState;
				if (XblXuidMapper.s_xuidStateToUserId.TryGetByValue(userId, out xuidState))
				{
					xuidState2 = xuidState;
				}
				else
				{
					using (XblXuidMapper.s_xuidStateDictionaryLock.WriteLockScope())
					{
						PlatformUserIdentifierAbs[] array = new PlatformUserIdentifierAbs[4];
						int num = PlatformUserManager.TryGetByNative(userId, array.AsSpan<PlatformUserIdentifierAbs>(0, array.Length - 1));
						array[num] = userId;
						int num2 = num + 1;
						int num3 = 0;
						while (num3 < num2 && !XblXuidMapper.s_xuidStateToUserId.TryGetByValue(array[num3], out xuidState))
						{
							num3++;
						}
						if (xuidState == null)
						{
							xuidState = new XblXuidMapper.XuidState();
						}
						for (int i = 0; i < num2; i++)
						{
							PlatformUserIdentifierAbs platformUserIdentifierAbs = array[i];
							XblXuidMapper.XuidState xuidState3;
							if (!XblXuidMapper.s_xuidStateToUserId.TryGetByValue(platformUserIdentifierAbs, out xuidState3))
							{
								XblXuidMapper.s_xuidStateToUserId.Add(xuidState, platformUserIdentifierAbs);
							}
							else if (xuidState3 != xuidState)
							{
								XblXuidMapper.s_xuidStateToUserId.RemoveByValue(platformUserIdentifierAbs);
								XblXuidMapper.s_xuidStateToUserId.Add(xuidState, platformUserIdentifierAbs);
								Log.Error(string.Format("[XBL-XuidMapper] Unexpected state merge. UserId '{0}' already had state but has been merged with UserIds: '{1}'.", array[i], string.Join<PlatformUserIdentifierAbs>("', '", array.Take<PlatformUserIdentifierAbs>(num2))));
							}
						}
					}
					xuidState2 = xuidState;
				}
			}
			return xuidState2;
		}

		// Token: 0x0600D5EB RID: 54763 RVA: 0x004D01DC File Offset: 0x004CE3DC
		[PublicizedFrom(EAccessModifier.Private)]
		public static XblXuidMapper.XuidState GetXuidState(ulong xuid)
		{
			if (xuid == 0UL)
			{
				return XblXuidMapper.EmptyXuidState;
			}
			XblXuidMapper.XuidState xuidState2;
			using (XblXuidMapper.s_xuidStateDictionaryLock.UpgradableReadLockScope())
			{
				XblXuidMapper.XuidState xuidState;
				if (XblXuidMapper.s_xuidStateToXuid.TryGetByValue(xuid, out xuidState))
				{
					xuidState2 = xuidState;
				}
				else
				{
					using (XblXuidMapper.s_xuidStateDictionaryLock.WriteLockScope())
					{
						xuidState = new XblXuidMapper.XuidState();
						XblXuidMapper.s_xuidStateToXuid.Add(xuidState, xuid);
					}
					xuidState2 = xuidState;
				}
			}
			return xuidState2;
		}

		// Token: 0x0600D5EC RID: 54764 RVA: 0x004D026C File Offset: 0x004CE46C
		public static void ResolveXuids(IReadOnlyList<XuidResolveRequest> _requests, Action<IReadOnlyList<XuidResolveRequest>> _onComplete)
		{
			XblXuidMapper.<>c__DisplayClass29_0 CS$<>8__locals1 = new XblXuidMapper.<>c__DisplayClass29_0();
			CS$<>8__locals1._requests = _requests;
			CS$<>8__locals1._onComplete = _onComplete;
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			IUserIdentifierMappingService userIdentifierMappingService = ((crossplatformPlatform != null) ? crossplatformPlatform.IdMappingService : null);
			if (userIdentifierMappingService == null)
			{
				Log.Error("[XBL-XuidMapper] Cannot resolve xuids, no mapping service available");
				CS$<>8__locals1._onComplete(CS$<>8__locals1._requests);
				return;
			}
			List<MappedAccountRequest> list = null;
			CS$<>8__locals1.requestIndices = null;
			int i = 0;
			while (i < CS$<>8__locals1._requests.Count)
			{
				XuidResolveRequest xuidResolveRequest = CS$<>8__locals1._requests[i];
				XblXuidMapper.XuidState xuidState = XblXuidMapper.GetXuidState(xuidResolveRequest.Id);
				IReadOnlyCollection<PlatformUserIdentifierAbs> readOnlyCollection;
				using (XblXuidMapper.s_xuidStateDictionaryLock.ReadLockScope())
				{
					ulong num;
					if (!XblXuidMapper.s_xuidStateToXuid.TryGetByKey(xuidState, out num))
					{
						num = 0UL;
					}
					if (num != 0UL)
					{
						xuidResolveRequest.Xuid = num;
						xuidResolveRequest.IsSuccess = true;
						goto IL_0169;
					}
					if (!XblXuidMapper.s_xuidStateToUserId.TryGetByKey(xuidState, out readOnlyCollection))
					{
						goto IL_0169;
					}
				}
				goto IL_00D0;
				IL_0169:
				i++;
				continue;
				IL_00D0:
				foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs in readOnlyCollection)
				{
					if (userIdentifierMappingService.CanQuery(platformUserIdentifierAbs))
					{
						using (XblXuidMapper.s_userIdsWithNoXblMappingLock.ReadLockScope())
						{
							if (XblXuidMapper.s_userIdsWithNoXblMapping.Contains(platformUserIdentifierAbs))
							{
								continue;
							}
						}
						if (list == null)
						{
							list = new List<MappedAccountRequest>();
						}
						if (CS$<>8__locals1.requestIndices == null)
						{
							CS$<>8__locals1.requestIndices = new List<int>();
						}
						list.Add(new MappedAccountRequest(platformUserIdentifierAbs, EPlatformIdentifier.XBL));
						CS$<>8__locals1.requestIndices.Add(i);
						break;
					}
				}
				goto IL_0169;
			}
			if (list == null)
			{
				CS$<>8__locals1._onComplete(CS$<>8__locals1._requests);
				return;
			}
			userIdentifierMappingService.QueryMappedAccountsDetails(list, new MappedAccountsQueryCallback(CS$<>8__locals1.<ResolveXuids>g__Callback|0));
		}

		// Token: 0x0400A2E8 RID: 41704
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool s_enabled;

		// Token: 0x0400A2E9 RID: 41705
		[PublicizedFrom(EAccessModifier.Private)]
		public static IUserIdentifierMappingService s_mappingService;

		// Token: 0x0400A2EA RID: 41706
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly XblXuidMapper.XuidState EmptyXuidState = new XblXuidMapper.XuidState();

		// Token: 0x0400A2ED RID: 41709
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly OneToManyDictionary<XblXuidMapper.XuidState, PlatformUserIdentifierAbs> s_xuidStateToUserId = new OneToManyDictionary<XblXuidMapper.XuidState, PlatformUserIdentifierAbs>();

		// Token: 0x0400A2EE RID: 41710
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly BiDictionary<XblXuidMapper.XuidState, ulong> s_xuidStateToXuid = new BiDictionary<XblXuidMapper.XuidState, ulong>();

		// Token: 0x0400A2EF RID: 41711
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ReaderWriterLockSlim s_xuidStateDictionaryLock = new ReaderWriterLockSlim();

		// Token: 0x0400A2F0 RID: 41712
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly HashSet<PlatformUserIdentifierAbs> s_userIdsWithNoXblMapping = new HashSet<PlatformUserIdentifierAbs>();

		// Token: 0x0400A2F1 RID: 41713
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ReaderWriterLockSlim s_userIdsWithNoXblMappingLock = new ReaderWriterLockSlim();

		// Token: 0x0400A2F2 RID: 41714
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly HashSet<ulong> s_xuidsCheckedForUserIdentifiers = new HashSet<ulong>();

		// Token: 0x0400A2F3 RID: 41715
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ReaderWriterLockSlim s_xuidsCheckedForUserIdentifiersLock = new ReaderWriterLockSlim();

		// Token: 0x0400A2F4 RID: 41716
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<PlatformUserIdentifierAbs> s_xuidMappedResultTemp = new List<PlatformUserIdentifierAbs>();

		// Token: 0x0400A2F5 RID: 41717
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object s_xuidMappedResultTempLock = new object();

		// Token: 0x02001C22 RID: 7202
		// (Invoke) Token: 0x0600D5EE RID: 54766
		public delegate void XuidMappedHandler(IReadOnlyCollection<PlatformUserIdentifierAbs> userIds, ulong xuid);

		// Token: 0x02001C23 RID: 7203
		// (Invoke) Token: 0x0600D5F2 RID: 54770
		public delegate void UserIdentifierMappedHandler(ulong xuid, PlatformUserIdentifierAbs userId);

		// Token: 0x02001C24 RID: 7204
		[PublicizedFrom(EAccessModifier.Private)]
		public sealed class XuidState
		{
			// Token: 0x0400A2F6 RID: 41718
			public int AttemptsCompleted;

			// Token: 0x0400A2F7 RID: 41719
			public bool InProgress;
		}
	}
}
