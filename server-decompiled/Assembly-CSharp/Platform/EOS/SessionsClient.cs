using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Epic.OnlineServices;
using Epic.OnlineServices.Sessions;
using UnityEngine;

namespace Platform.EOS
{
	// Token: 0x02001D71 RID: 7537
	public class SessionsClient : IServerListInterface
	{
		// Token: 0x17001C6F RID: 7279
		// (get) Token: 0x0600DF11 RID: 57105 RVA: 0x00020119 File Offset: 0x0001E319
		public bool IsPrefiltered
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600DF12 RID: 57106 RVA: 0x004F5D94 File Offset: 0x004F3F94
		public SessionsClient()
		{
			this.compatibilityVersionString = Constants.cVersionInformation.SerializableString;
			this.compatibilityVersionString = this.compatibilityVersionString.Substring(0, this.compatibilityVersionString.LastIndexOf('.') + 1);
		}

		// Token: 0x0600DF13 RID: 57107 RVA: 0x004F5DEE File Offset: 0x004F3FEE
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x0600DF14 RID: 57108 RVA: 0x004F5E14 File Offset: 0x004F4014
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sessionsInterface = ((Api)this.owner.Api).PlatformInterface.GetSessionsInterface();
			}
		}

		// Token: 0x0600DF15 RID: 57109 RVA: 0x004F5E70 File Offset: 0x004F4070
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
			this.maxResultsCallback = _maxResultsCallback;
			this.sessionSearchErrorCallback = _sessionSearchErrorCallback;
		}

		// Token: 0x17001C70 RID: 7280
		// (get) Token: 0x0600DF16 RID: 57110 RVA: 0x004F5E87 File Offset: 0x004F4087
		public bool IsRefreshing
		{
			get
			{
				return this.refreshingSearchTypes.Count > 0;
			}
		}

		// Token: 0x0600DF17 RID: 57111 RVA: 0x004F5E98 File Offset: 0x004F4098
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			if (!this.pingCoroutineStarted)
			{
				ThreadManager.StartCoroutine(this.getServerPingsCo());
			}
			EosHelpers.AssertMainThread("SeCl.Start");
			bool flag = PermissionsManager.IsCrossplayAllowed();
			List<IServerListInterface.ServerFilter> list = new List<IServerListInterface.ServerFilter>();
			bool? flag2 = null;
			foreach (IServerListInterface.ServerFilter serverFilter in _activeFilters)
			{
				if (serverFilter.Name.ContainsCaseInsensitive(GameInfoString.PlayGroup.ToStringCached<GameInfoString>()))
				{
					Log.Error("[EOS] Play group should not be filterable by the user. It is for internal use only. Ignoring.");
				}
				else if (serverFilter.Name.ContainsCaseInsensitive(GameInfoBool.AllowCrossplay.ToStringCached<GameInfoBool>()))
				{
					flag2 = new bool?(serverFilter.BoolValue);
				}
				else
				{
					list.Add(serverFilter);
				}
			}
			if (flag)
			{
				if (flag2 != null)
				{
					this.<StartSearch>g__AddCrossplayFiltersAndSearch|15_0(list, flag2.Value);
					return;
				}
				List<IServerListInterface.ServerFilter> list2 = new List<IServerListInterface.ServerFilter>(list);
				this.<StartSearch>g__AddCrossplayFiltersAndSearch|15_0(list, false);
				this.<StartSearch>g__AddCrossplayFiltersAndSearch|15_0(list2, true);
				return;
			}
			else
			{
				bool? flag3 = flag2;
				bool flag4 = true;
				if ((flag3.GetValueOrDefault() == flag4) & (flag3 != null))
				{
					Log.Warning("[EOS] Active filter set for servers that allow crossplay, but client does not have crossplay permissions. No work to do.");
					return;
				}
				this.<StartSearch>g__AddCrossplayFiltersAndSearch|15_0(list, false);
				return;
			}
		}

		// Token: 0x0600DF18 RID: 57112 RVA: 0x004F5FBC File Offset: 0x004F41BC
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartSearchInternal(IList<IServerListInterface.ServerFilter> _activeFilters, SessionsClient.ESessionSearchType searchType)
		{
			CreateSessionSearchOptions createSessionSearchOptions = new CreateSessionSearchOptions
			{
				MaxSearchResults = 200U
			};
			object obj = AntiCheatCommon.LockObject;
			SessionSearch sessionSearch;
			Result result;
			lock (obj)
			{
				result = this.sessionsInterface.CreateSessionSearch(ref createSessionSearchOptions, out sessionSearch);
			}
			if (result != Result.Success)
			{
				ServerSearchErrorCallback serverSearchErrorCallback = this.sessionSearchErrorCallback;
				if (serverSearchErrorCallback != null)
				{
					serverSearchErrorCallback(Localization.Get("xuiServerBrowserSearchErrorEOS", false, null));
				}
				Log.Error("[EOS] Failed creating sessions search: " + result.ToStringCached<Result>());
				return;
			}
			if (!this.setSearchParameters(sessionSearch, _activeFilters))
			{
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					sessionSearch.Release();
				}
				return;
			}
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			Log.Out(string.Format("[EOS] Starting session search with {0} filters", _activeFilters.Count));
			SessionSearchFindOptions sessionSearchFindOptions = new SessionSearchFindOptions
			{
				LocalUserId = ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId
			};
			this.refreshingSearchTypes.Add(searchType);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				sessionSearch.Find(ref sessionSearchFindOptions, new SessionsClient.SessionSearchArgs(sessionSearch, microStopwatch, this.gameServerFoundCallback, EServerRelationType.Internet, false, true, searchType, _activeFilters), new SessionSearchOnFindCallback(this.searchFinishedCallback));
			}
		}

		// Token: 0x0600DF19 RID: 57113 RVA: 0x004F6144 File Offset: 0x004F4344
		public void StopSearch()
		{
			this.refreshingSearchTypes.Clear();
			this.serverPingsToGet.Clear();
		}

		// Token: 0x0600DF1A RID: 57114 RVA: 0x004F615C File Offset: 0x004F435C
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
			this.maxResultsCallback = null;
		}

		// Token: 0x0600DF1B RID: 57115 RVA: 0x004F6174 File Offset: 0x004F4374
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			string value = _serverInfo.GetValue(GameInfoString.UniqueId);
			if (string.IsNullOrEmpty(value))
			{
				Log.Error("[EOS] No session to search for in server info");
				_callback(this.owner, null, _relation);
				return;
			}
			EosHelpers.AssertMainThread("SeCl.Single");
			CreateSessionSearchOptions createSessionSearchOptions = new CreateSessionSearchOptions
			{
				MaxSearchResults = 200U
			};
			object obj = AntiCheatCommon.LockObject;
			SessionSearch sessionSearch;
			Result result;
			lock (obj)
			{
				result = this.sessionsInterface.CreateSessionSearch(ref createSessionSearchOptions, out sessionSearch);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed creating sessions search: " + result.ToStringCached<Result>());
				_callback(this.owner, null, _relation);
				return;
			}
			SessionSearchSetSessionIdOptions sessionSearchSetSessionIdOptions = new SessionSearchSetSessionIdOptions
			{
				SessionId = value
			};
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				result = sessionSearch.SetSessionId(ref sessionSearchSetSessionIdOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed setting search session: " + result.ToStringCached<Result>());
				sessionSearch.Release();
				_callback(this.owner, null, _relation);
				return;
			}
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			SessionSearchFindOptions sessionSearchFindOptions = new SessionSearchFindOptions
			{
				LocalUserId = ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId
			};
			this.refreshingSearchTypes.Add(SessionsClient.ESessionSearchType.Single);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				sessionSearch.Find(ref sessionSearchFindOptions, new SessionsClient.SessionSearchArgs(sessionSearch, microStopwatch, _callback, _relation, true, false, SessionsClient.ESessionSearchType.Single, null), new SessionSearchOnFindCallback(this.searchFinishedCallback));
			}
		}

		// Token: 0x0600DF1C RID: 57116 RVA: 0x004F6344 File Offset: 0x004F4544
		[PublicizedFrom(EAccessModifier.Private)]
		public bool setSearchParameters(SessionSearch _searchHandle, IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.setSingleSearchParameter(_searchHandle, stringBuilder, GameInfoString.ServerVersion.ToStringCached<GameInfoString>(), AttributeType.String, ComparisonOp.Contains, ":", 0, false, this.compatibilityVersionString))
			{
				return false;
			}
			string matchmakingGroupTag = SessionsHost.GetMatchmakingGroupTag(PlatformManager.MultiPlatform.User.GetMatchmakingGroup());
			this.setSingleSearchParameter(_searchHandle, stringBuilder, SessionsInterface.SEARCH_BUCKET_ID, AttributeType.String, ComparisonOp.Equal, "=", 0, false, matchmakingGroupTag);
			if (_activeFilters.Count == 0)
			{
				Log.Warning("[EOS] Session search started without any filters from: " + StackTraceUtility.ExtractStackTrace());
				return this.setSingleSearchParameter(_searchHandle, stringBuilder, GameInfoString.LevelName.ToStringCached<GameInfoString>(), AttributeType.String, ComparisonOp.Contains, ":", 0, false, "");
			}
			foreach (IServerListInterface.ServerFilter serverFilter in _activeFilters)
			{
				bool flag;
				switch (serverFilter.Type)
				{
				case IServerListInterface.ServerFilter.EServerFilterType.Any:
					continue;
				case IServerListInterface.ServerFilter.EServerFilterType.BoolValue:
				{
					string text = string.Concat(new string[]
					{
						",",
						serverFilter.Name,
						"=",
						serverFilter.BoolValue ? "1" : "0",
						","
					});
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, "-BoolValues-", AttributeType.String, ComparisonOp.Contains, ":", 0, false, text);
					break;
				}
				case IServerListInterface.ServerFilter.EServerFilterType.IntValue:
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.Int64, ComparisonOp.Equal, "=", serverFilter.IntMinValue, false, null);
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.IntNotValue:
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.Int64, ComparisonOp.Notequal, "!=", serverFilter.IntMinValue, false, null);
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.IntMin:
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.Int64, ComparisonOp.Greaterthanorequal, ">=", serverFilter.IntMinValue, false, null);
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.IntMax:
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.Int64, ComparisonOp.Lessthanorequal, "<=", serverFilter.IntMaxValue, false, null);
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.IntRange:
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.Int64, ComparisonOp.Greaterthanorequal, ">=", serverFilter.IntMinValue, false, null);
					if (flag)
					{
						flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.Int64, ComparisonOp.Lessthanorequal, "<=", serverFilter.IntMaxValue, false, null);
					}
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.StringValue:
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.String, ComparisonOp.Contains, "=", 0, false, "~$#$~" + serverFilter.StringNeedle.ToLowerInvariant());
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.StringContains:
					flag = this.setSingleSearchParameter(_searchHandle, stringBuilder, serverFilter.Name, AttributeType.String, ComparisonOp.Contains, ":", 0, false, serverFilter.StringNeedle.ToLowerInvariant());
					break;
				default:
					throw new ArgumentOutOfRangeException("Type", serverFilter.Type, null);
				}
				if (!flag)
				{
					return false;
				}
			}
			string text2 = "[EOS] Session search filters: ";
			StringBuilder stringBuilder2 = stringBuilder;
			Log.Out(text2 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null));
			return true;
		}

		// Token: 0x0600DF1D RID: 57117 RVA: 0x004F6638 File Offset: 0x004F4838
		[PublicizedFrom(EAccessModifier.Private)]
		public bool setSingleSearchParameter(SessionSearch _searchHandle, StringBuilder _sb, string _key, AttributeType _type, ComparisonOp _comparison, string _comparisonString, int _intValue = 0, bool _boolValue = false, string _stringValue = null)
		{
			AttributeDataValue attributeDataValue;
			switch (_type)
			{
			case AttributeType.Boolean:
				attributeDataValue = new bool?(_boolValue);
				break;
			case AttributeType.Int64:
				attributeDataValue = new long?((long)_intValue);
				break;
			case AttributeType.Double:
				throw new NotSupportedException("[EOS] Session attribute search type Double not supported!");
			case AttributeType.String:
				attributeDataValue = _stringValue;
				break;
			default:
				throw new ArgumentOutOfRangeException("_type", _type, null);
			}
			AttributeDataValue attributeDataValue2 = attributeDataValue;
			SessionSearchSetParameterOptions sessionSearchSetParameterOptions = new SessionSearchSetParameterOptions
			{
				Parameter = new AttributeData?(new AttributeData
				{
					Key = _key,
					Value = attributeDataValue2
				}),
				ComparisonOp = _comparison
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = _searchHandle.SetParameter(ref sessionSearchSetParameterOptions);
			}
			string text;
			switch (_type)
			{
			case AttributeType.Boolean:
				text = _boolValue.ToString();
				break;
			case AttributeType.Int64:
				text = _intValue.ToString();
				break;
			case AttributeType.Double:
				throw new NotSupportedException("[EOS] Session attribute search type Double not supported!");
			case AttributeType.String:
				text = "\"" + _stringValue + "\"";
				break;
			default:
				throw new ArgumentOutOfRangeException("_type", _type, null);
			}
			string text2 = text;
			if (result != Result.Success)
			{
				Log.Error(string.Concat(new string[]
				{
					"[EOS] Failed setting search param '",
					_key,
					"' to '",
					text2,
					"': ",
					result.ToStringCached<Result>()
				}));
			}
			_sb.Append(_key);
			_sb.Append(_comparisonString);
			_sb.Append(text2);
			_sb.Append(", ");
			return result == Result.Success;
		}

		// Token: 0x0600DF1E RID: 57118 RVA: 0x004F67F0 File Offset: 0x004F49F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void searchFinishedCallback(ref SessionSearchFindCallbackInfo _callbackData)
		{
			SessionsClient.SessionSearchArgs sessionSearchArgs = (SessionsClient.SessionSearchArgs)_callbackData.ClientData;
			sessionSearchArgs.Stopwatch.Stop();
			Log.Out(string.Format("[EOS] Search took: {0} ms", sessionSearchArgs.Stopwatch.ElapsedMilliseconds));
			object obj;
			if (_callbackData.ResultCode != Result.Success)
			{
				if (!sessionSearchArgs.CallbackOnFailure)
				{
					Log.Error("[EOS] Failed searching sessions on backend: " + _callbackData.ResultCode.ToStringCached<Result>());
					ServerSearchErrorCallback serverSearchErrorCallback = this.sessionSearchErrorCallback;
					if (serverSearchErrorCallback != null)
					{
						serverSearchErrorCallback(Localization.Get("xuiServerBrowserSearchErrorEOS", false, null));
					}
				}
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					sessionSearchArgs.SearchHandle.Release();
				}
				if (sessionSearchArgs.UpdateRefreshing)
				{
					this.refreshingSearchTypes.Remove(sessionSearchArgs.SearchType);
				}
				if (sessionSearchArgs.CallbackOnFailure)
				{
					GameServerFoundCallback callback = sessionSearchArgs.Callback;
					if (callback == null)
					{
						return;
					}
					callback(this.owner, null, sessionSearchArgs.RelationType);
				}
				return;
			}
			SessionSearchGetSearchResultCountOptions sessionSearchGetSearchResultCountOptions = default(SessionSearchGetSearchResultCountOptions);
			obj = AntiCheatCommon.LockObject;
			uint searchResultCount;
			lock (obj)
			{
				searchResultCount = sessionSearchArgs.SearchHandle.GetSearchResultCount(ref sessionSearchGetSearchResultCountOptions);
			}
			if (sessionSearchArgs.SearchType != SessionsClient.ESessionSearchType.Single)
			{
				MaxResultsReachedCallback maxResultsReachedCallback = this.maxResultsCallback;
				if (maxResultsReachedCallback != null)
				{
					maxResultsReachedCallback(this.owner, searchResultCount >= 200U, 200);
				}
			}
			Log.Out("[EOS] Sessions received: " + searchResultCount.ToString());
			for (uint num = 0U; num < searchResultCount; num += 1U)
			{
				SessionSearchCopySearchResultByIndexOptions sessionSearchCopySearchResultByIndexOptions = new SessionSearchCopySearchResultByIndexOptions
				{
					SessionIndex = num
				};
				obj = AntiCheatCommon.LockObject;
				SessionDetails sessionDetails;
				Result result;
				lock (obj)
				{
					result = sessionSearchArgs.SearchHandle.CopySearchResultByIndex(ref sessionSearchCopySearchResultByIndexOptions, out sessionDetails);
				}
				if (result != Result.Success)
				{
					Log.Error(string.Format("[EOS] Failed getting session {0} data: {1}", num, result.ToStringCached<Result>()));
				}
				else
				{
					SessionDetailsCopyInfoOptions sessionDetailsCopyInfoOptions = default(SessionDetailsCopyInfoOptions);
					obj = AntiCheatCommon.LockObject;
					SessionDetailsInfo? sessionDetailsInfo;
					lock (obj)
					{
						result = sessionDetails.CopyInfo(ref sessionDetailsCopyInfoOptions, out sessionDetailsInfo);
					}
					if (result != Result.Success)
					{
						Log.Error(string.Format("[EOS] Failed getting session {0} data details: {1}", num, result.ToStringCached<Result>()));
						sessionDetails.Release();
					}
					else
					{
						string launchArgument = GameUtils.GetLaunchArgument("debugsessions");
						if (launchArgument != null && !this.debugLogSessionInfo(num, sessionDetails, sessionDetailsInfo.Value, launchArgument == "verbose"))
						{
							sessionDetails.Release();
						}
						else
						{
							GameServerInfo gameServerInfo = this.parseSession(sessionDetails, sessionDetailsInfo.Value);
							if (!this.matchesFilters(gameServerInfo, sessionSearchArgs.SearchFilters))
							{
								gameServerInfo = null;
							}
							if (gameServerInfo != null)
							{
								this.serverPingsToGet.Enqueue(gameServerInfo);
								GameServerFoundCallback callback2 = sessionSearchArgs.Callback;
								if (callback2 != null)
								{
									callback2(this.owner, gameServerInfo, sessionSearchArgs.RelationType);
								}
							}
							sessionDetails.Release();
						}
					}
				}
			}
			if (searchResultCount == 0U && sessionSearchArgs.CallbackOnFailure)
			{
				GameServerFoundCallback callback3 = sessionSearchArgs.Callback;
				if (callback3 != null)
				{
					callback3(this.owner, null, sessionSearchArgs.RelationType);
				}
			}
			sessionSearchArgs.SearchHandle.Release();
			if (sessionSearchArgs.UpdateRefreshing)
			{
				this.refreshingSearchTypes.Remove(sessionSearchArgs.SearchType);
			}
		}

		// Token: 0x0600DF1F RID: 57119 RVA: 0x004F6B58 File Offset: 0x004F4D58
		[PublicizedFrom(EAccessModifier.Private)]
		public bool matchesFilters(GameServerInfo _gameServerInfo, IList<IServerListInterface.ServerFilter> _searchFilters)
		{
			SessionsClient.<>c__DisplayClass25_0 CS$<>8__locals1;
			CS$<>8__locals1._gameServerInfo = _gameServerInfo;
			if (_searchFilters == null)
			{
				return true;
			}
			foreach (IServerListInterface.ServerFilter serverFilter in _searchFilters)
			{
				switch (serverFilter.Type)
				{
				case IServerListInterface.ServerFilter.EServerFilterType.Any:
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.BoolValue:
					if (!SessionsClient.<matchesFilters>g__TestBoolInfo|25_0(serverFilter, ref CS$<>8__locals1))
					{
						return false;
					}
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.IntValue:
				case IServerListInterface.ServerFilter.EServerFilterType.IntNotValue:
				case IServerListInterface.ServerFilter.EServerFilterType.IntMin:
				case IServerListInterface.ServerFilter.EServerFilterType.IntMax:
				case IServerListInterface.ServerFilter.EServerFilterType.IntRange:
					if (!SessionsClient.<matchesFilters>g__TestIntInfo|25_1(serverFilter, ref CS$<>8__locals1))
					{
						return false;
					}
					break;
				case IServerListInterface.ServerFilter.EServerFilterType.StringValue:
				case IServerListInterface.ServerFilter.EServerFilterType.StringContains:
					if (!SessionsClient.<matchesFilters>g__TestStringInfo|25_2(serverFilter, ref CS$<>8__locals1))
					{
						return false;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException("Type", serverFilter.Type, null);
				}
			}
			return true;
		}

		// Token: 0x0600DF20 RID: 57120 RVA: 0x004F6C24 File Offset: 0x004F4E24
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo parseSession(SessionDetails _sessionDetails, SessionDetailsInfo _sessionDetailsInfo)
		{
			GameServerInfo gameServerInfo = new GameServerInfo();
			GameServerInfo gameServerInfo2 = gameServerInfo;
			OnlineSessionPermissionLevel permissionLevel = _sessionDetailsInfo.Settings.Value.PermissionLevel;
			int i;
			if (permissionLevel != OnlineSessionPermissionLevel.PublicAdvertised)
			{
				if (permissionLevel != OnlineSessionPermissionLevel.JoinViaPresence)
				{
					i = 0;
				}
				else
				{
					i = 1;
				}
			}
			else
			{
				i = 2;
			}
			gameServerInfo2.SetValue(GameInfoInt.ServerVisibility, i);
			gameServerInfo.SetValue(GameInfoInt.MaxPlayers, (int)_sessionDetailsInfo.Settings.Value.NumPublicConnections);
			gameServerInfo.SetValue(GameInfoInt.CurrentPlayers, (int)(_sessionDetailsInfo.Settings.Value.NumPublicConnections - _sessionDetailsInfo.NumOpenPublicConnections));
			gameServerInfo.SetValue(GameInfoString.IP, _sessionDetailsInfo.HostAddress);
			if (_sessionDetailsInfo.OwnerUserId != null)
			{
				gameServerInfo.SetValue(GameInfoString.CombinedPrimaryId, UserIdentifierEos.CreateCombinedString(_sessionDetailsInfo.OwnerUserId));
			}
			SessionDetailsGetSessionAttributeCountOptions sessionDetailsGetSessionAttributeCountOptions = default(SessionDetailsGetSessionAttributeCountOptions);
			object obj = AntiCheatCommon.LockObject;
			uint sessionAttributeCount;
			lock (obj)
			{
				sessionAttributeCount = _sessionDetails.GetSessionAttributeCount(ref sessionDetailsGetSessionAttributeCountOptions);
			}
			for (uint num = 0U; num < sessionAttributeCount; num += 1U)
			{
				SessionDetailsCopySessionAttributeByIndexOptions sessionDetailsCopySessionAttributeByIndexOptions = new SessionDetailsCopySessionAttributeByIndexOptions
				{
					AttrIndex = num
				};
				obj = AntiCheatCommon.LockObject;
				SessionDetailsAttribute? sessionDetailsAttribute;
				Result result;
				lock (obj)
				{
					result = _sessionDetails.CopySessionAttributeByIndex(ref sessionDetailsCopySessionAttributeByIndexOptions, out sessionDetailsAttribute);
				}
				if (result != Result.Success)
				{
					Log.Error(string.Format("[EOS] Failed getting session attribute {0}: {1}", num, result.ToStringCached<Result>()));
					return null;
				}
				AttributeData value = sessionDetailsAttribute.Value.Data.Value;
				string text = value.Key;
				switch (value.Value.ValueType)
				{
				case AttributeType.Boolean:
					if (!gameServerInfo.Parse(text, value.Value.AsBool.GetValueOrDefault()))
					{
						return null;
					}
					break;
				case AttributeType.Int64:
					if (!gameServerInfo.Parse(text, (int)value.Value.AsInt64.GetValueOrDefault()))
					{
						return null;
					}
					break;
				case AttributeType.Double:
					Log.Error(string.Format("Session attribute '{0}' is of unsupported type double ({1})", value.Key, value.Value.AsDouble));
					return null;
				case AttributeType.String:
					if (text.EqualsCaseInsensitive("-BoolValues-"))
					{
						string text2 = value.Value.AsUtf8;
						if (text2 == "##EMPTY##")
						{
							text2 = "";
						}
						foreach (string text3 in text2.Split(',', StringSplitOptions.None))
						{
							if (text3.Length > 0)
							{
								int num2 = text3.IndexOf('=');
								if (num2 <= 0 || num2 >= text3.Length - 1)
								{
									Log.Warning(string.Concat(new string[] { "Session attribute ", text, " has invalid content for bool set: '", text3, "' (total: '", text2, "')" }));
								}
								else
								{
									string text4 = text3.Substring(0, num2);
									bool flag2 = text3[num2 + 1] == '1';
									if (!gameServerInfo.Parse(text4, flag2))
									{
										return null;
									}
								}
							}
						}
					}
					else
					{
						string text5 = value.Value.AsUtf8;
						if (text5 == "##EMPTY##")
						{
							text5 = "";
						}
						int num3 = text5.IndexOf("~$#$~", StringComparison.Ordinal);
						if (num3 >= 0)
						{
							text5 = text5.Substring(0, num3);
						}
						if (!gameServerInfo.Parse(text, text5))
						{
							return null;
						}
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			if (ServerInfoCache.Instance.IsFavorite(gameServerInfo))
			{
				gameServerInfo.IsFavorite = true;
			}
			gameServerInfo.LastPlayedLinux = (int)ServerInfoCache.Instance.IsHistory(gameServerInfo);
			return gameServerInfo;
		}

		// Token: 0x0600DF21 RID: 57121 RVA: 0x004F701C File Offset: 0x004F521C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool debugLogSessionInfo(uint _sessionIndex, SessionDetails _sessionDetails, SessionDetailsInfo _sessionDetailsInfo, bool _logAttributes = false)
		{
			SessionDetailsSettings value = _sessionDetailsInfo.Settings.Value;
			Log.Out(string.Format("Session {0}:", _sessionIndex));
			Log.Out("    Details:");
			Log.Out("        Settings:");
			Log.Out(string.Format("            BucketId: {0}", value.BucketId));
			Log.Out(string.Format("            InvitesAllowed: {0}", value.InvitesAllowed));
			Log.Out(string.Format("            PermissionLevel: {0}", value.PermissionLevel));
			Log.Out(string.Format("            SanctionsEnabled: {0}", value.SanctionsEnabled));
			Log.Out(string.Format("            NumPublicConnections: {0}", value.NumPublicConnections));
			Log.Out(string.Format("            AllowJoinInProgress: {0}", value.AllowJoinInProgress));
			Log.Out(string.Format("        Address: {0}", _sessionDetailsInfo.HostAddress));
			Log.Out(string.Format("        SessionId: {0}", _sessionDetailsInfo.SessionId));
			Log.Out(string.Format("        NumOpenPublicConnections: {0}", _sessionDetailsInfo.NumOpenPublicConnections));
			SessionDetailsGetSessionAttributeCountOptions sessionDetailsGetSessionAttributeCountOptions = default(SessionDetailsGetSessionAttributeCountOptions);
			object obj = AntiCheatCommon.LockObject;
			uint sessionAttributeCount;
			lock (obj)
			{
				sessionAttributeCount = _sessionDetails.GetSessionAttributeCount(ref sessionDetailsGetSessionAttributeCountOptions);
			}
			Log.Out(string.Format("    Attributes: {0}", sessionAttributeCount));
			uint num = 0U;
			while (num < sessionAttributeCount)
			{
				SessionDetailsCopySessionAttributeByIndexOptions sessionDetailsCopySessionAttributeByIndexOptions = new SessionDetailsCopySessionAttributeByIndexOptions
				{
					AttrIndex = num
				};
				obj = AntiCheatCommon.LockObject;
				SessionDetailsAttribute? sessionDetailsAttribute;
				Result result;
				lock (obj)
				{
					result = _sessionDetails.CopySessionAttributeByIndex(ref sessionDetailsCopySessionAttributeByIndexOptions, out sessionDetailsAttribute);
				}
				if (result != Result.Success)
				{
					Log.Error(string.Format("[EOS] Failed getting session {0} attribute {1}: {2}", _sessionIndex, num, result.ToStringCached<Result>()));
					return false;
				}
				AttributeData value2 = sessionDetailsAttribute.Value.Data.Value;
				if (_logAttributes)
				{
					goto IL_0242;
				}
				string text = value2.Key;
				if (text.ContainsCaseInsensitive(GameInfoBool.EACEnabled.ToStringCached<GameInfoBool>()) || text.ContainsCaseInsensitive(GameInfoBool.SanctionsIgnored.ToStringCached<GameInfoBool>()) || text.ContainsCaseInsensitive(GameInfoBool.IsPasswordProtected.ToStringCached<GameInfoBool>()) || text.ContainsCaseInsensitive("-BoolValues-"))
				{
					goto IL_0242;
				}
				IL_0328:
				num += 1U;
				continue;
				IL_0242:
				switch (value2.Value.ValueType)
				{
				case AttributeType.Boolean:
					Log.Out(string.Format("    Attr {0}: {1}", value2.Key, value2.Value.AsBool));
					goto IL_0328;
				case AttributeType.Int64:
					Log.Out(string.Format("    Attr {0}: {1}", value2.Key, value2.Value.AsInt64));
					goto IL_0328;
				case AttributeType.Double:
					Log.Out(string.Format("    Attr {0}: {1}", value2.Key, value2.Value.AsDouble));
					goto IL_0328;
				case AttributeType.String:
					Log.Out(string.Format("    Attr {0}: {1}", value2.Key, value2.Value.AsUtf8));
					goto IL_0328;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			return true;
		}

		// Token: 0x0600DF22 RID: 57122 RVA: 0x004F737C File Offset: 0x004F557C
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator getServerPingsCo()
		{
			this.pingCoroutineStarted = true;
			for (;;)
			{
				if (this.pingRequestsInProgress < 8 && this.serverPingsToGet.Count > 0)
				{
					GameServerInfo gameServerInfo = this.serverPingsToGet.Dequeue();
					this.pingRequestsInProgress++;
					ServerInformationTcpClient.RequestRules(gameServerInfo, true, new ServerInformationTcpClient.RulesRequestDone(this.serverPingCallback));
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600DF23 RID: 57123 RVA: 0x004F738B File Offset: 0x004F558B
		[PublicizedFrom(EAccessModifier.Private)]
		public void serverPingCallback(bool _success, string _message, GameServerInfo _gsi)
		{
			this.pingRequestsInProgress--;
		}

		// Token: 0x0600DF24 RID: 57124 RVA: 0x004F739C File Offset: 0x004F559C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <StartSearch>g__AddCrossplayFiltersAndSearch|15_0(List<IServerListInterface.ServerFilter> _filters, bool _allowCrossplay)
		{
			_filters.Add(new IServerListInterface.ServerFilter(GameInfoBool.AllowCrossplay.ToStringCached<GameInfoBool>(), IServerListInterface.ServerFilter.EServerFilterType.BoolValue, 0, 0, _allowCrossplay, null));
			if (_allowCrossplay)
			{
				Log.Out("[EOS] Searching for servers that allow crossplay.");
				_filters.Add(new IServerListInterface.ServerFilter(GameInfoBool.SanctionsIgnored.ToStringCached<GameInfoBool>(), IServerListInterface.ServerFilter.EServerFilterType.BoolValue, 0, 0, false, null));
				_filters.Add(new IServerListInterface.ServerFilter(GameInfoInt.MaxPlayers.ToStringCached<GameInfoInt>(), IServerListInterface.ServerFilter.EServerFilterType.IntMax, 0, 8, false, null));
				Log.Out("[EOS] Searching for servers that have crossplay compatible settings.");
			}
			else
			{
				string text = EPlayGroupExtensions.Current.ToStringCached<EPlayGroup>();
				_filters.Add(new IServerListInterface.ServerFilter(GameInfoString.PlayGroup.ToStringCached<GameInfoString>(), IServerListInterface.ServerFilter.EServerFilterType.StringValue, 0, 0, false, text));
				Log.Out("[EOS] Searching for servers that do not allow crossplay and are in the play group '" + text + "'.");
			}
			this.StartSearchInternal(_filters, _allowCrossplay ? SessionsClient.ESessionSearchType.OnlyCrossplatform : SessionsClient.ESessionSearchType.NoCrossplatform);
		}

		// Token: 0x0600DF25 RID: 57125 RVA: 0x004F744C File Offset: 0x004F564C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static bool <matchesFilters>g__TestBoolInfo|25_0(IServerListInterface.ServerFilter _filter, ref SessionsClient.<>c__DisplayClass25_0 A_1)
		{
			GameInfoBool gameInfoBool;
			if (!EnumUtils.TryParse<GameInfoBool>(_filter.Name, out gameInfoBool, false))
			{
				SessionsClient.<matchesFilters>g__LogMismatch|25_3("[EOS] Dropped search result, could not parse bool filter name '" + _filter.Name + "' to GameInfoBool");
				return false;
			}
			bool value = A_1._gameServerInfo.GetValue(gameInfoBool);
			if (value != _filter.BoolValue)
			{
				SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Format("[EOS] Dropped search result, bool value {0} does not match filter {1}=={2}", value, _filter.Name, _filter.BoolValue));
				return false;
			}
			return true;
		}

		// Token: 0x0600DF26 RID: 57126 RVA: 0x004F74C4 File Offset: 0x004F56C4
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static bool <matchesFilters>g__TestIntInfo|25_1(IServerListInterface.ServerFilter _filter, ref SessionsClient.<>c__DisplayClass25_0 A_1)
		{
			GameInfoInt gameInfoInt;
			if (!EnumUtils.TryParse<GameInfoInt>(_filter.Name, out gameInfoInt, false))
			{
				SessionsClient.<matchesFilters>g__LogMismatch|25_3("[EOS] Dropped search result, could not parse int filter name '" + _filter.Name + "' to GameInfoInt");
				return false;
			}
			int value = A_1._gameServerInfo.GetValue(gameInfoInt);
			switch (_filter.Type)
			{
			case IServerListInterface.ServerFilter.EServerFilterType.IntValue:
				if (value != _filter.IntMinValue)
				{
					SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Format("[EOS] Dropped search result, int value {0} does not match filter {1}=={2}", value, _filter.Name, _filter.IntMinValue));
					return false;
				}
				break;
			case IServerListInterface.ServerFilter.EServerFilterType.IntNotValue:
				if (value == _filter.IntMinValue)
				{
					SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Format("[EOS] Dropped search result, int value {0} does not match filter {1}!={2}", value, _filter.Name, _filter.IntMinValue));
					return false;
				}
				break;
			case IServerListInterface.ServerFilter.EServerFilterType.IntMin:
				if (value < _filter.IntMinValue)
				{
					SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Format("[EOS] Dropped search result, int value {0} does not match filter {1}>={2}", value, _filter.Name, _filter.IntMinValue));
					return false;
				}
				break;
			case IServerListInterface.ServerFilter.EServerFilterType.IntMax:
				if (value > _filter.IntMaxValue)
				{
					SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Format("[EOS] Dropped search result, int value {0} does not match filter {1}<={2}", value, _filter.Name, _filter.IntMaxValue));
					return false;
				}
				break;
			case IServerListInterface.ServerFilter.EServerFilterType.IntRange:
				if (value < _filter.IntMinValue || value > _filter.IntMaxValue)
				{
					SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Format("[EOS] Dropped search result, int value {0} does not match filter {1}>={2} && {3}<={4}", new object[] { value, _filter.Name, _filter.IntMinValue, _filter.Name, _filter.IntMaxValue }));
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x0600DF27 RID: 57127 RVA: 0x004F7664 File Offset: 0x004F5864
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static bool <matchesFilters>g__TestStringInfo|25_2(IServerListInterface.ServerFilter _filter, ref SessionsClient.<>c__DisplayClass25_0 A_1)
		{
			GameInfoString gameInfoString;
			if (!EnumUtils.TryParse<GameInfoString>(_filter.Name, out gameInfoString, false))
			{
				SessionsClient.<matchesFilters>g__LogMismatch|25_3("[EOS] Dropped search result, could not parse string filter name '" + _filter.Name + "' to GameInfoString");
				return false;
			}
			string value = A_1._gameServerInfo.GetValue(gameInfoString);
			IServerListInterface.ServerFilter.EServerFilterType type = _filter.Type;
			if (type != IServerListInterface.ServerFilter.EServerFilterType.StringValue)
			{
				if (type == IServerListInterface.ServerFilter.EServerFilterType.StringContains)
				{
					if (value.IndexOf(_filter.StringNeedle, StringComparison.OrdinalIgnoreCase) < 0)
					{
						SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Concat(new string[] { "[EOS] Dropped search result, string value '", value, "' not included in match filter ", _filter.Name, ".Contains('", _filter.StringNeedle, "')" }));
						return false;
					}
				}
			}
			else if (!string.Equals(value, _filter.StringNeedle))
			{
				SessionsClient.<matchesFilters>g__LogMismatch|25_3(string.Concat(new string[] { "[EOS] Dropped search result, string value '", value, "' does not match filter ", _filter.Name, "=='", _filter.StringNeedle, "'" }));
				return false;
			}
			return true;
		}

		// Token: 0x0600DF28 RID: 57128 RVA: 0x000027FC File Offset: 0x000009FC
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <matchesFilters>g__LogMismatch|25_3(string _s)
		{
		}

		// Token: 0x0400A86D RID: 43117
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A86E RID: 43118
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionsInterface sessionsInterface;

		// Token: 0x0400A86F RID: 43119
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<SessionsClient.ESessionSearchType> refreshingSearchTypes = new HashSet<SessionsClient.ESessionSearchType>();

		// Token: 0x0400A870 RID: 43120
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A871 RID: 43121
		[PublicizedFrom(EAccessModifier.Private)]
		public MaxResultsReachedCallback maxResultsCallback;

		// Token: 0x0400A872 RID: 43122
		[PublicizedFrom(EAccessModifier.Private)]
		public ServerSearchErrorCallback sessionSearchErrorCallback;

		// Token: 0x0400A873 RID: 43123
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string compatibilityVersionString;

		// Token: 0x0400A874 RID: 43124
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pingCoroutineStarted;

		// Token: 0x0400A875 RID: 43125
		[PublicizedFrom(EAccessModifier.Private)]
		public const int concurrentPingRequests = 8;

		// Token: 0x0400A876 RID: 43126
		[PublicizedFrom(EAccessModifier.Private)]
		public int pingRequestsInProgress;

		// Token: 0x0400A877 RID: 43127
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<GameServerInfo> serverPingsToGet = new Queue<GameServerInfo>();

		// Token: 0x02001D72 RID: 7538
		[PublicizedFrom(EAccessModifier.Private)]
		public enum ESessionSearchType
		{
			// Token: 0x0400A879 RID: 43129
			Single,
			// Token: 0x0400A87A RID: 43130
			NoCrossplatform,
			// Token: 0x0400A87B RID: 43131
			OnlyCrossplatform
		}

		// Token: 0x02001D73 RID: 7539
		[PublicizedFrom(EAccessModifier.Private)]
		public class SessionSearchArgs
		{
			// Token: 0x0600DF29 RID: 57129 RVA: 0x004F7770 File Offset: 0x004F5970
			public SessionSearchArgs(SessionSearch _searchHandle, MicroStopwatch _stopwatch, GameServerFoundCallback _callback, EServerRelationType _relation, bool _callbackOnFailure, bool _updateRefreshing, SessionsClient.ESessionSearchType _searchType, IList<IServerListInterface.ServerFilter> _activeFilters)
			{
				this.SearchHandle = _searchHandle;
				this.Stopwatch = _stopwatch;
				this.Callback = _callback;
				this.RelationType = _relation;
				this.CallbackOnFailure = _callbackOnFailure;
				this.UpdateRefreshing = _updateRefreshing;
				this.SearchType = _searchType;
				this.SearchFilters = _activeFilters;
			}

			// Token: 0x0400A87C RID: 43132
			public readonly SessionSearch SearchHandle;

			// Token: 0x0400A87D RID: 43133
			public readonly MicroStopwatch Stopwatch;

			// Token: 0x0400A87E RID: 43134
			public readonly GameServerFoundCallback Callback;

			// Token: 0x0400A87F RID: 43135
			public readonly EServerRelationType RelationType;

			// Token: 0x0400A880 RID: 43136
			public readonly bool CallbackOnFailure;

			// Token: 0x0400A881 RID: 43137
			public readonly bool UpdateRefreshing;

			// Token: 0x0400A882 RID: 43138
			public readonly SessionsClient.ESessionSearchType SearchType;

			// Token: 0x0400A883 RID: 43139
			public readonly IList<IServerListInterface.ServerFilter> SearchFilters;
		}
	}
}
