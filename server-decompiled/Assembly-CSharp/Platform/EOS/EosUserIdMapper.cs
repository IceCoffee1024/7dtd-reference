using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D20 RID: 7456
	public class EosUserIdMapper : IUserIdentifierMappingService
	{
		// Token: 0x0600DD3C RID: 56636 RVA: 0x004ED495 File Offset: 0x004EB695
		public EosUserIdMapper(Api _eosApi, User _eosUser)
		{
			this.api = _eosApi;
			this.user = _eosUser;
		}

		// Token: 0x0600DD3D RID: 56637 RVA: 0x004ED4AB File Offset: 0x004EB6AB
		public bool CanQuery(PlatformUserIdentifierAbs _id)
		{
			return _id is UserIdentifierEos;
		}

		// Token: 0x0600DD3E RID: 56638 RVA: 0x004ED4B8 File Offset: 0x004EB6B8
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryValidateUser(out ProductUserId loggedInUser, out MappedAccountQueryResult failureResult)
		{
			loggedInUser = null;
			failureResult = MappedAccountQueryResult.QueryFailed;
			PlatformUserIdentifierAbs platformUserId = this.user.PlatformUserId;
			if (platformUserId == null)
			{
				Log.Out("[EOS] Cannot query mapped account details yet - EOS does not have info yet");
				failureResult = MappedAccountQueryResult.NotReady;
				return false;
			}
			UserIdentifierEos userIdentifierEos = platformUserId as UserIdentifierEos;
			if (userIdentifierEos == null)
			{
				Log.Error(string.Format("[EOS] Cannot query mapped account details. EosUserIdMapper got non-EOS id type: {0}", platformUserId));
				return false;
			}
			loggedInUser = userIdentifierEos.ProductUserId;
			if (loggedInUser == null)
			{
				Log.Out(string.Format("[EOS] Cannot query mapped account details - EOS does not have info yet for {0}", userIdentifierEos));
				failureResult = MappedAccountQueryResult.NotReady;
				return false;
			}
			return true;
		}

		// Token: 0x0600DD3F RID: 56639 RVA: 0x004ED530 File Offset: 0x004EB730
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool TryValidateRequest(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform, out ProductUserId _puid)
		{
			UserIdentifierEos userIdentifierEos = _id as UserIdentifierEos;
			if (userIdentifierEos == null)
			{
				Log.Error(string.Format("[EOS] Cannot retrieve mapped account details, {0} is not an eos product user id", _id));
				_puid = null;
				return false;
			}
			_puid = userIdentifierEos.ProductUserId;
			if (!EosHelpers.PlatformIdentifierMappings.ContainsKey(_platform))
			{
				Log.Error(string.Format("[EOS] Cannot retrieve mapped acount details, target platform {0} does not map to a known external account type", _platform));
				return false;
			}
			return true;
		}

		// Token: 0x0600DD40 RID: 56640 RVA: 0x004ED58C File Offset: 0x004EB78C
		public void QueryMappedAccountDetails(PlatformUserIdentifierAbs _id, EPlatformIdentifier _platform, MappedAccountQueryCallback _callback)
		{
			ProductUserId productUserId;
			MappedAccountQueryResult mappedAccountQueryResult;
			if (!this.TryValidateUser(out productUserId, out mappedAccountQueryResult))
			{
				_callback(mappedAccountQueryResult, _platform, null, null);
				return;
			}
			ProductUserId productUserId2;
			if (!EosUserIdMapper.TryValidateRequest(_id, _platform, out productUserId2))
			{
				_callback(MappedAccountQueryResult.QueryFailed, _platform, null, null);
				return;
			}
			this.QueryMappedExternalAccount(productUserId, productUserId2, _platform, _callback);
		}

		// Token: 0x0600DD41 RID: 56641 RVA: 0x004ED5D0 File Offset: 0x004EB7D0
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueryMappedExternalAccount(ProductUserId _loggedInUser, ProductUserId _puid, EPlatformIdentifier _platform, MappedAccountQueryCallback _callback)
		{
			if (!ThreadManager.IsMainThread())
			{
				ThreadManager.AddSingleTaskMainThread("QueryEosMappedAccount", delegate(object _)
				{
					this.QueryMappedExternalAccount(_loggedInUser, _puid, _platform, _callback);
				}, null);
				return;
			}
			if (this.api.ConnectInterface == null)
			{
				Log.Out("[EOS] QueryProductUserIdMappings failed, connect interface null");
				_callback(MappedAccountQueryResult.QueryFailed, _platform, null, null);
				return;
			}
			ExternalAccountType externalAccountType;
			if (!EosHelpers.PlatformIdentifierMappings.TryGetValue(_platform, out externalAccountType))
			{
				Log.Out(string.Format("[EOS] Unknown external account type for {0}", _platform));
				_callback(MappedAccountQueryResult.QueryFailed, _platform, null, null);
				return;
			}
			QueryProductUserIdMappingsOptions queryProductUserIdMappingsOptions = default(QueryProductUserIdMappingsOptions);
			queryProductUserIdMappingsOptions.LocalUserId = _loggedInUser;
			queryProductUserIdMappingsOptions.ProductUserIds = new ProductUserId[] { _puid };
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.api.ConnectInterface.QueryProductUserIdMappings(ref queryProductUserIdMappingsOptions, queryProductUserIdMappingsOptions.ProductUserIds, delegate(ref QueryProductUserIdMappingsCallbackInfo _response)
				{
					if (_response.ResultCode != Result.Success)
					{
						Log.Out(string.Format("[EOS] QueryProductUserIdMappings failed {0}", _response.ResultCode));
						_callback(MappedAccountQueryResult.QueryFailed, _platform, null, null);
						return;
					}
					if (this.api.ConnectInterface == null)
					{
						Log.Out("[EOS] QueryProductUserIdMappings failed, connect interface null");
						_callback(MappedAccountQueryResult.QueryFailed, _platform, null, null);
						return;
					}
					ProductUserId productUserId = ((ProductUserId[])_response.ClientData)[0];
					CopyProductUserExternalAccountByAccountTypeOptions copyProductUserExternalAccountByAccountTypeOptions = default(CopyProductUserExternalAccountByAccountTypeOptions);
					copyProductUserExternalAccountByAccountTypeOptions.TargetUserId = productUserId;
					copyProductUserExternalAccountByAccountTypeOptions.AccountIdType = externalAccountType;
					ExternalAccountInfo externalAccountInfo;
					if (!this.TryCopyResult(copyProductUserExternalAccountByAccountTypeOptions, out externalAccountInfo))
					{
						_callback(MappedAccountQueryResult.MappingNotFound, _platform, null, null);
						return;
					}
					if (Api.DebugLevel == Api.EDebugLevel.Verbose)
					{
						Log.Out(string.Format("[EOS] found external account for {0}: Type: {1}, Id: {2}", _puid, externalAccountType, externalAccountInfo.AccountId));
					}
					_callback(MappedAccountQueryResult.Success, _platform, externalAccountInfo.AccountId, externalAccountInfo.DisplayName);
				});
			}
		}

		// Token: 0x0600DD42 RID: 56642 RVA: 0x004ED71C File Offset: 0x004EB91C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryCopyResult(CopyProductUserExternalAccountByAccountTypeOptions _copyOptions, out ExternalAccountInfo _externalAccountInfo)
		{
			object lockObject = AntiCheatCommon.LockObject;
			ExternalAccountInfo? externalAccountInfo;
			Result result;
			lock (lockObject)
			{
				result = this.api.ConnectInterface.CopyProductUserExternalAccountByAccountType(ref _copyOptions, out externalAccountInfo);
			}
			if (result != Result.Success)
			{
				Log.Out(string.Format("[EOS] {0} copy failed. Result: {1}", _copyOptions.TargetUserId, result));
				_externalAccountInfo = default(ExternalAccountInfo);
				return false;
			}
			if (externalAccountInfo == null)
			{
				Log.Out(string.Format("[EOS] {0} copy failed, null info", _copyOptions.TargetUserId));
				_externalAccountInfo = default(ExternalAccountInfo);
				return false;
			}
			_externalAccountInfo = externalAccountInfo.Value;
			return true;
		}

		// Token: 0x0600DD43 RID: 56643 RVA: 0x004ED7C8 File Offset: 0x004EB9C8
		public void QueryMappedAccountsDetails(IReadOnlyList<MappedAccountRequest> _requests, MappedAccountsQueryCallback _callback)
		{
			if (_requests.Count == 0)
			{
				_callback(_requests);
				return;
			}
			ProductUserId productUserId;
			MappedAccountQueryResult mappedAccountQueryResult;
			if (!this.TryValidateUser(out productUserId, out mappedAccountQueryResult))
			{
				foreach (MappedAccountRequest mappedAccountRequest in _requests)
				{
					mappedAccountRequest.Result = mappedAccountQueryResult;
				}
				_callback(_requests);
				return;
			}
			this.QueryMappedExternalAccounts(productUserId, _requests, _callback);
		}

		// Token: 0x0600DD44 RID: 56644 RVA: 0x004ED83C File Offset: 0x004EBA3C
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueryMappedExternalAccounts(ProductUserId _loggedInUser, IReadOnlyList<MappedAccountRequest> _requests, MappedAccountsQueryCallback _callback)
		{
			if (!ThreadManager.IsMainThread())
			{
				ThreadManager.AddSingleTaskMainThread("QueryEosMappedAccounts", delegate(object _)
				{
					this.QueryMappedExternalAccounts(_loggedInUser, _requests, _callback);
				}, null);
				return;
			}
			if (this.api.ConnectInterface == null)
			{
				Log.Out("[EOS] QueryProductUserIdMappings failed, connect interface null");
				foreach (MappedAccountRequest mappedAccountRequest in _requests)
				{
					mappedAccountRequest.Result = MappedAccountQueryResult.QueryFailed;
				}
				_callback(_requests);
				return;
			}
			List<ProductUserId> puids = null;
			List<int> requestIndices = null;
			for (int i = 0; i < _requests.Count; i++)
			{
				MappedAccountRequest mappedAccountRequest2 = _requests[i];
				ProductUserId productUserId;
				if (!EosUserIdMapper.TryValidateRequest(mappedAccountRequest2.Id, mappedAccountRequest2.Platform, out productUserId))
				{
					mappedAccountRequest2.Result = MappedAccountQueryResult.QueryFailed;
				}
				else
				{
					if (puids == null)
					{
						puids = new List<ProductUserId>();
					}
					if (requestIndices == null)
					{
						requestIndices = new List<int>();
					}
					puids.Add(productUserId);
					requestIndices.Add(i);
				}
			}
			if (puids == null)
			{
				_callback(_requests);
				return;
			}
			QueryProductUserIdMappingsOptions queryProductUserIdMappingsOptions = default(QueryProductUserIdMappingsOptions);
			queryProductUserIdMappingsOptions.LocalUserId = _loggedInUser;
			queryProductUserIdMappingsOptions.ProductUserIds = puids.ToArray();
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.api.ConnectInterface.QueryProductUserIdMappings(ref queryProductUserIdMappingsOptions, null, delegate(ref QueryProductUserIdMappingsCallbackInfo _response)
				{
					if (_response.ResultCode != Result.Success)
					{
						Log.Out(string.Format("[EOS] QueryProductUserIdMappings failed {0}", _response.ResultCode));
						foreach (MappedAccountRequest mappedAccountRequest3 in _requests)
						{
							mappedAccountRequest3.Result = MappedAccountQueryResult.QueryFailed;
						}
						_callback(_requests);
						return;
					}
					if (this.api.ConnectInterface == null)
					{
						Log.Out("[EOS] QueryProductUserIdMappings failed, connect interface null");
						foreach (MappedAccountRequest mappedAccountRequest4 in _requests)
						{
							mappedAccountRequest4.Result = MappedAccountQueryResult.QueryFailed;
						}
						_callback(_requests);
						return;
					}
					CopyProductUserExternalAccountByAccountTypeOptions copyProductUserExternalAccountByAccountTypeOptions = default(CopyProductUserExternalAccountByAccountTypeOptions);
					for (int j = 0; j < puids.Count; j++)
					{
						int num = requestIndices[j];
						MappedAccountRequest mappedAccountRequest5 = _requests[num];
						ProductUserId productUserId2 = puids[j];
						copyProductUserExternalAccountByAccountTypeOptions.TargetUserId = productUserId2;
						copyProductUserExternalAccountByAccountTypeOptions.AccountIdType = EosHelpers.PlatformIdentifierMappings[mappedAccountRequest5.Platform];
						ExternalAccountInfo externalAccountInfo;
						if (!this.TryCopyResult(copyProductUserExternalAccountByAccountTypeOptions, out externalAccountInfo))
						{
							mappedAccountRequest5.Result = MappedAccountQueryResult.MappingNotFound;
						}
						else
						{
							mappedAccountRequest5.MappedAccountId = externalAccountInfo.AccountId;
							mappedAccountRequest5.DisplayName = externalAccountInfo.DisplayName;
							mappedAccountRequest5.Result = MappedAccountQueryResult.Success;
							if (Api.DebugLevel == Api.EDebugLevel.Verbose)
							{
								Log.Out(string.Format("[EOS] found external account for {0}: Type: {1}, Id: {2}", productUserId2, externalAccountInfo.AccountIdType, externalAccountInfo.AccountId));
							}
						}
					}
					_callback(_requests);
				});
			}
		}

		// Token: 0x0600DD45 RID: 56645 RVA: 0x004EDA18 File Offset: 0x004EBC18
		public bool CanReverseQuery(EPlatformIdentifier _platform, string _platformId)
		{
			return EosHelpers.PlatformIdentifierMappings.ContainsKey(_platform);
		}

		// Token: 0x0600DD46 RID: 56646 RVA: 0x004EDA25 File Offset: 0x004EBC25
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool TryValidateReverseRequest(EPlatformIdentifier _platform, out ExternalAccountType _externalAccountType)
		{
			if (!EosHelpers.PlatformIdentifierMappings.TryGetValue(_platform, out _externalAccountType))
			{
				Log.Error(string.Format("[EOS] Cannot retrieve reverse mapped account details, target platform {0} does not map to a known external account type", _platform));
				return false;
			}
			return true;
		}

		// Token: 0x0600DD47 RID: 56647 RVA: 0x004EDA50 File Offset: 0x004EBC50
		public void ReverseQueryMappedAccountDetails(EPlatformIdentifier _platform, string _platformId, MappedAccountReverseQueryCallback _callback)
		{
			EosUserIdMapper.<>c__DisplayClass13_0 CS$<>8__locals1 = new EosUserIdMapper.<>c__DisplayClass13_0();
			CS$<>8__locals1._callback = _callback;
			this.ReverseQueryMappedAccountsDetails(new MappedAccountReverseRequest[]
			{
				new MappedAccountReverseRequest(_platform, _platformId)
			}, new MappedAccountsReverseQueryCallback(CS$<>8__locals1.<ReverseQueryMappedAccountDetails>g__Callback|0));
		}

		// Token: 0x0600DD48 RID: 56648 RVA: 0x004EDA8C File Offset: 0x004EBC8C
		public void ReverseQueryMappedAccountsDetails(IReadOnlyList<MappedAccountReverseRequest> _requests, MappedAccountsReverseQueryCallback _callback)
		{
			if (_requests.Count == 0)
			{
				_callback(_requests);
				return;
			}
			ProductUserId productUserId;
			MappedAccountQueryResult mappedAccountQueryResult;
			if (!this.TryValidateUser(out productUserId, out mappedAccountQueryResult))
			{
				foreach (MappedAccountReverseRequest mappedAccountReverseRequest in _requests)
				{
					mappedAccountReverseRequest.Result = mappedAccountQueryResult;
				}
				_callback(_requests);
				return;
			}
			this.QueryExternalAccountMappings(productUserId, _requests, _callback);
		}

		// Token: 0x0600DD49 RID: 56649 RVA: 0x004EDB00 File Offset: 0x004EBD00
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueryExternalAccountMappings(ProductUserId _loggedInUser, IReadOnlyList<MappedAccountReverseRequest> _requests, MappedAccountsReverseQueryCallback _callback)
		{
			EosUserIdMapper.<>c__DisplayClass15_0 CS$<>8__locals1 = new EosUserIdMapper.<>c__DisplayClass15_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1._loggedInUser = _loggedInUser;
			CS$<>8__locals1._requests = _requests;
			CS$<>8__locals1._callback = _callback;
			if (!ThreadManager.IsMainThread())
			{
				ThreadManager.AddSingleTaskMainThread("QueryEosExternalAccountMappings", delegate(object _)
				{
					CS$<>8__locals1.<>4__this.QueryExternalAccountMappings(CS$<>8__locals1._loggedInUser, CS$<>8__locals1._requests, CS$<>8__locals1._callback);
				}, null);
				return;
			}
			if (this.api.ConnectInterface == null)
			{
				Log.Out("[EOS] QueryExternalAccountMappings failed, connect interface null");
				foreach (MappedAccountReverseRequest mappedAccountReverseRequest in CS$<>8__locals1._requests)
				{
					mappedAccountReverseRequest.Result = MappedAccountQueryResult.QueryFailed;
				}
				CS$<>8__locals1._callback(CS$<>8__locals1._requests);
				return;
			}
			CS$<>8__locals1.requestsByType = null;
			foreach (MappedAccountReverseRequest mappedAccountReverseRequest2 in CS$<>8__locals1._requests)
			{
				ExternalAccountType externalAccountType;
				if (!EosUserIdMapper.TryValidateReverseRequest(mappedAccountReverseRequest2.Platform, out externalAccountType))
				{
					mappedAccountReverseRequest2.Result = MappedAccountQueryResult.QueryFailed;
				}
				else
				{
					if (CS$<>8__locals1.requestsByType == null)
					{
						CS$<>8__locals1.requestsByType = new Dictionary<ExternalAccountType, List<MappedAccountReverseRequest>>();
					}
					List<MappedAccountReverseRequest> list;
					if (!CS$<>8__locals1.requestsByType.TryGetValue(externalAccountType, out list))
					{
						list = new List<MappedAccountReverseRequest>();
						CS$<>8__locals1.requestsByType[externalAccountType] = list;
					}
					list.Add(mappedAccountReverseRequest2);
				}
			}
			if (CS$<>8__locals1.requestsByType == null)
			{
				CS$<>8__locals1._callback(CS$<>8__locals1._requests);
				return;
			}
			CS$<>8__locals1.requestsFinished = 0;
			foreach (KeyValuePair<ExternalAccountType, List<MappedAccountReverseRequest>> keyValuePair in CS$<>8__locals1.requestsByType)
			{
				ExternalAccountType externalAccountType2;
				List<MappedAccountReverseRequest> list2;
				keyValuePair.Deconstruct(out externalAccountType2, out list2);
				ExternalAccountType externalAccountType3 = externalAccountType2;
				List<MappedAccountReverseRequest> list3 = list2;
				this.QueryExternalAccountMappings(CS$<>8__locals1._loggedInUser, externalAccountType3, list3, new Action(CS$<>8__locals1.<QueryExternalAccountMappings>g__Callback|1));
			}
		}

		// Token: 0x0600DD4A RID: 56650 RVA: 0x004EDCDC File Offset: 0x004EBEDC
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueryExternalAccountMappings(ProductUserId _loggedInUser, ExternalAccountType _externalAccountType, IReadOnlyList<MappedAccountReverseRequest> _requests, Action _callback)
		{
			Utf8String[] externalAccountIds = new Utf8String[_requests.Count];
			for (int i = 0; i < _requests.Count; i++)
			{
				externalAccountIds[i] = _requests[i].Id;
			}
			QueryExternalAccountMappingsOptions queryExternalAccountMappingsOptions = new QueryExternalAccountMappingsOptions
			{
				LocalUserId = _loggedInUser,
				AccountIdType = _externalAccountType,
				ExternalAccountIds = externalAccountIds
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.api.ConnectInterface.QueryExternalAccountMappings(ref queryExternalAccountMappingsOptions, null, delegate(ref QueryExternalAccountMappingsCallbackInfo _response)
				{
					if (_response.ResultCode != Result.Success)
					{
						Log.Out(string.Format("[EOS] QueryProductUserIdMappings failed {0}", _response.ResultCode));
						foreach (MappedAccountReverseRequest mappedAccountReverseRequest in _requests)
						{
							mappedAccountReverseRequest.Result = MappedAccountQueryResult.QueryFailed;
						}
						_callback();
						return;
					}
					if (this.api.ConnectInterface == null)
					{
						Log.Out("[EOS] QueryProductUserIdMappings failed, connect interface null");
						foreach (MappedAccountReverseRequest mappedAccountReverseRequest2 in _requests)
						{
							mappedAccountReverseRequest2.Result = MappedAccountQueryResult.QueryFailed;
						}
						_callback();
						return;
					}
					GetExternalAccountMappingsOptions getExternalAccountMappingsOptions = new GetExternalAccountMappingsOptions
					{
						LocalUserId = _loggedInUser,
						AccountIdType = _externalAccountType
					};
					for (int j = 0; j < externalAccountIds.Length; j++)
					{
						getExternalAccountMappingsOptions.TargetExternalUserId = externalAccountIds[j];
						MappedAccountReverseRequest mappedAccountReverseRequest3 = _requests[j];
						object lockObject2 = AntiCheatCommon.LockObject;
						ProductUserId externalAccountMapping;
						lock (lockObject2)
						{
							externalAccountMapping = this.api.ConnectInterface.GetExternalAccountMapping(ref getExternalAccountMappingsOptions);
						}
						if (externalAccountMapping == null || !externalAccountMapping.IsValid())
						{
							mappedAccountReverseRequest3.Result = MappedAccountQueryResult.MappingNotFound;
						}
						else
						{
							mappedAccountReverseRequest3.Result = MappedAccountQueryResult.Success;
							mappedAccountReverseRequest3.PlatformId = new UserIdentifierEos(externalAccountMapping);
							if (Api.DebugLevel == Api.EDebugLevel.Verbose)
							{
								Log.Out(string.Format("[EOS] found EOS account {0} from external account: Type: {1}, Id: {2}", externalAccountMapping, _externalAccountType, mappedAccountReverseRequest3.Id));
							}
						}
					}
					_callback();
				});
			}
		}

		// Token: 0x0400A6F1 RID: 42737
		[PublicizedFrom(EAccessModifier.Private)]
		public Api api;

		// Token: 0x0400A6F2 RID: 42738
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;
	}
}
