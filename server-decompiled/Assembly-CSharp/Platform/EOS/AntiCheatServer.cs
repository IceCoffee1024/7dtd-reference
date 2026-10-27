using System;
using System.IO;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatCommon;
using Epic.OnlineServices.AntiCheatServer;

namespace Platform.EOS
{
	// Token: 0x02001D45 RID: 7493
	public class AntiCheatServer : IAntiCheatServer, IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600DDF4 RID: 56820 RVA: 0x004EE5D7 File Offset: 0x004EC7D7
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x0600DDF5 RID: 56821 RVA: 0x004EE5FC File Offset: 0x004EC7FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			EosHelpers.AssertMainThread("ACS.Init");
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.antiCheatInterface = ((Api)this.owner.Api).PlatformInterface.GetAntiCheatServerInterface();
			}
			if (this.antiCheatInterface == null)
			{
				Log.Out("[EAC] AntiCheatServer initialized with null interface");
				return;
			}
		}

		// Token: 0x0600DDF6 RID: 56822 RVA: 0x004EE678 File Offset: 0x004EC878
		[PublicizedFrom(EAccessModifier.Private)]
		public void addCallbacks()
		{
			AddNotifyMessageToClientOptions addNotifyMessageToClientOptions = default(AddNotifyMessageToClientOptions);
			object obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.handleMessageToClientID = this.antiCheatInterface.AddNotifyMessageToClient(ref addNotifyMessageToClientOptions, null, new OnMessageToClientCallback(this.handleMessageToClient));
			}
			AddNotifyClientActionRequiredOptions addNotifyClientActionRequiredOptions = default(AddNotifyClientActionRequiredOptions);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.handleClientActionRequiredID = this.antiCheatInterface.AddNotifyClientActionRequired(ref addNotifyClientActionRequiredOptions, null, new OnClientActionRequiredCallback(this.handleClientAction));
			}
			AddNotifyClientAuthStatusChangedOptions addNotifyClientAuthStatusChangedOptions = default(AddNotifyClientAuthStatusChangedOptions);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.handleClientAuthStateChangeID = this.antiCheatInterface.AddNotifyClientAuthStatusChanged(ref addNotifyClientAuthStatusChangedOptions, null, new OnClientAuthStatusChangedCallback(this.handleClientAuthStateChange));
			}
		}

		// Token: 0x0600DDF7 RID: 56823 RVA: 0x004EE780 File Offset: 0x004EC980
		[PublicizedFrom(EAccessModifier.Private)]
		public void removeCallbacks()
		{
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				if (this.handleMessageToClientID > 0UL)
				{
					this.antiCheatInterface.RemoveNotifyMessageToClient(this.handleMessageToClientID);
					this.handleMessageToClientID = 0UL;
				}
				if (this.handleClientActionRequiredID > 0UL)
				{
					this.antiCheatInterface.RemoveNotifyClientActionRequired(this.handleClientActionRequiredID);
					this.handleClientActionRequiredID = 0UL;
				}
				if (this.handleClientAuthStateChangeID > 0UL)
				{
					this.antiCheatInterface.RemoveNotifyClientAuthStatusChanged(this.handleClientAuthStateChangeID);
					this.handleClientAuthStateChangeID = 0UL;
				}
			}
		}

		// Token: 0x0600DDF8 RID: 56824 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DDF9 RID: 56825 RVA: 0x004EE824 File Offset: 0x004ECA24
		public bool GetHostUserIdAndToken([TupleElementNames(new string[] { "userId", "token" })] out ValueTuple<PlatformUserIdentifierAbs, string> _hostUserIdAndToken)
		{
			_hostUserIdAndToken = default(ValueTuple<PlatformUserIdentifierAbs, string>);
			return false;
		}

		// Token: 0x0600DDFA RID: 56826 RVA: 0x004EE830 File Offset: 0x004ECA30
		public bool StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate)
		{
			if (this.ServerEacEnabled())
			{
				this.addCallbacks();
				Log.Out("[EAC] Starting EAC server");
				this.authSuccessfulDelegate = _authSuccessfulDelegate;
				this.kickPlayerDelegate = _kickPlayerDelegate;
				IUserClient user = this.owner.User;
				PlatformUserIdentifierAbs platformUserIdentifierAbs;
				if ((platformUserIdentifierAbs = ((user != null) ? user.PlatformUserId : null)) == null)
				{
					IUserClient userServer = this.owner.UserServer;
					platformUserIdentifierAbs = ((userServer != null) ? userServer.PlatformUserId : null);
				}
				UserIdentifierEos userIdentifierEos = (UserIdentifierEos)platformUserIdentifierAbs;
				ProductUserId productUserId = ((userIdentifierEos != null) ? userIdentifierEos.ProductUserId : null);
				string value = SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoString.GameHost);
				BeginSessionOptions beginSessionOptions = new BeginSessionOptions
				{
					EnableGameplayData = false,
					LocalUserId = productUserId,
					RegisterTimeoutSeconds = 60U,
					ServerName = value
				};
				object lockObject = AntiCheatCommon.LockObject;
				Result result;
				lock (lockObject)
				{
					result = this.antiCheatInterface.BeginSession(ref beginSessionOptions);
				}
				if (result != Result.Success)
				{
					Log.Error("[EOS-ACS] Starting module failed: " + result.ToStringCached<Result>());
				}
				else
				{
					this.serverRunning = true;
				}
				return result == Result.Success;
			}
			return true;
		}

		// Token: 0x0600DDFB RID: 56827 RVA: 0x004EE954 File Offset: 0x004ECB54
		public bool RegisterUser(ClientInfo _client)
		{
			if (!this.serverRunning)
			{
				return false;
			}
			Log.Out(string.Format("[EOS-ACS] Registering user: {0}", _client));
			EosHelpers.AssertMainThread("ACS.Reg");
			RegisterClientOptions registerClientOptions = new RegisterClientOptions
			{
				UserId = ((UserIdentifierEos)_client.CrossplatformId).ProductUserId,
				ClientHandle = AntiCheatCommon.ClientInfoToIntPtr(_client),
				ClientPlatform = EosHelpers.DeviceTypeToAntiCheatPlatformMappings[_client.device],
				ClientType = (_client.requiresAntiCheat ? AntiCheatCommonClientType.ProtectedClient : AntiCheatCommonClientType.UnprotectedClient),
				IpAddress = _client.ip
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.RegisterClient(ref registerClientOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACS] Failed registerung user: " + result.ToStringCached<Result>());
				return false;
			}
			return true;
		}

		// Token: 0x0600DDFC RID: 56828 RVA: 0x004EEA48 File Offset: 0x004ECC48
		public void FreeUser(ClientInfo _client)
		{
			if (!this.serverRunning)
			{
				return;
			}
			EosHelpers.AssertMainThread("ACS.Free");
			Log.Out(string.Format("[EOS-ACS] FreeUser: {0}", _client));
			UnregisterClientOptions unregisterClientOptions = new UnregisterClientOptions
			{
				ClientHandle = AntiCheatCommon.ClientInfoToIntPtr(_client)
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.UnregisterClient(ref unregisterClientOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACS] Failed unregistering user: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DDFD RID: 56829 RVA: 0x004EEAE4 File Offset: 0x004ECCE4
		public void HandleMessageFromClient(ClientInfo _cInfo, byte[] _data)
		{
			if (!this.serverRunning)
			{
				Log.Warning("[EOS-ACS] Server: Received EAC package but EAC was not initialized");
				return;
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACS] PushNetworkMessage (len={0}, from={1})", _data.Length, _cInfo.InternalId));
			}
			ReceiveMessageFromClientOptions receiveMessageFromClientOptions = new ReceiveMessageFromClientOptions
			{
				Data = new ArraySegment<byte>(_data),
				ClientHandle = AntiCheatCommon.ClientInfoToIntPtr(_cInfo)
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.ReceiveMessageFromClient(ref receiveMessageFromClientOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACS] Failed handling message: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DDFE RID: 56830 RVA: 0x004EEBA0 File Offset: 0x004ECDA0
		public void StopServer()
		{
			if (!this.serverRunning)
			{
				return;
			}
			this.removeCallbacks();
			EndSessionOptions endSessionOptions = default(EndSessionOptions);
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.EndSession(ref endSessionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACS] Stopping module failed: " + result.ToStringCached<Result>());
			}
			this.serverRunning = false;
			this.authSuccessfulDelegate = null;
			this.kickPlayerDelegate = null;
		}

		// Token: 0x0600DDFF RID: 56831 RVA: 0x004EEC2C File Offset: 0x004ECE2C
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleMessageToClient(ref OnMessageToClientCallbackInfo _data)
		{
			ClientInfo clientInfo = AntiCheatCommon.IntPtrToClientInfo(_data.ClientHandle, "[EOS-ACS] Got message for unknown client number: {0}");
			if (clientInfo == null)
			{
				Log.Out(string.Format("[EOS-ACS] FreeUser: {0}", _data.ClientHandle));
				UnregisterClientOptions unregisterClientOptions = new UnregisterClientOptions
				{
					ClientHandle = _data.ClientHandle
				};
				object lockObject = AntiCheatCommon.LockObject;
				Result result;
				lock (lockObject)
				{
					result = this.antiCheatInterface.UnregisterClient(ref unregisterClientOptions);
				}
				if (result != Result.Success)
				{
					Log.Error("[EOS-ACS] Failed unregistering user: " + result.ToStringCached<Result>());
				}
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACS] Forward message to client (len={0}, to={1})", _data.MessageData.Count, clientInfo.InternalId));
			}
			if (clientInfo != null)
			{
				clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageEAC>().Setup(_data.MessageData.Count, _data.MessageData.Array));
			}
		}

		// Token: 0x0600DE00 RID: 56832 RVA: 0x004EED38 File Offset: 0x004ECF38
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleClientAction(ref OnClientActionRequiredCallbackInfo _data)
		{
			ClientInfo clientInfo = AntiCheatCommon.IntPtrToClientInfo(_data.ClientHandle, "[EOS-ACS] Got action for unknown client number: {0}");
			if (clientInfo == null)
			{
				return;
			}
			AntiCheatCommonClientAction clientAction = _data.ClientAction;
			AntiCheatCommonClientActionReason actionReasonCode = _data.ActionReasonCode;
			string text = _data.ActionReasonDetailsString;
			if (clientAction != AntiCheatCommonClientAction.RemovePlayer)
			{
				Log.Warning(string.Format("[EOS-ACS] Got invalid action ({0}), reason='{1}', details={2}, client={3}", new object[]
				{
					clientAction.ToStringCached<AntiCheatCommonClientAction>(),
					actionReasonCode.ToStringCached<AntiCheatCommonClientActionReason>(),
					text,
					clientInfo
				}));
				return;
			}
			Log.Out(string.Format("[EOS-ACS] Kicking player. Reason={0}, details='{1}', client={2}", actionReasonCode.ToStringCached<AntiCheatCommonClientActionReason>(), text, clientInfo));
			KickPlayerDelegate kickPlayerDelegate = this.kickPlayerDelegate;
			if (kickPlayerDelegate == null)
			{
				return;
			}
			ClientInfo clientInfo2 = clientInfo;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.EosEacViolation;
			int num = (int)actionReasonCode;
			string text2 = text;
			kickPlayerDelegate(clientInfo2, new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text2));
		}

		// Token: 0x0600DE01 RID: 56833 RVA: 0x004EEDE8 File Offset: 0x004ECFE8
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleClientAuthStateChange(ref OnClientAuthStatusChangedCallbackInfo _data)
		{
			ClientInfo clientInfo = AntiCheatCommon.IntPtrToClientInfo(_data.ClientHandle, "[EOS-ACS] Got auth state change for unknown client number: {0}");
			if (_data.ClientAuthStatus == AntiCheatCommonClientAuthStatus.RemoteAuthComplete)
			{
				AuthenticationSuccessfulCallbackDelegate authenticationSuccessfulCallbackDelegate = this.authSuccessfulDelegate;
				if (authenticationSuccessfulCallbackDelegate == null)
				{
					return;
				}
				authenticationSuccessfulCallbackDelegate(clientInfo);
			}
		}

		// Token: 0x0600DE02 RID: 56834 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DE03 RID: 56835 RVA: 0x004EEE20 File Offset: 0x004ED020
		public bool ServerEacEnabled()
		{
			return this.antiCheatInterface != null && GamePrefs.GetBool(EnumGamePrefs.EACEnabled);
		}

		// Token: 0x0600DE04 RID: 56836 RVA: 0x004EEE39 File Offset: 0x004ED039
		public bool ServerEacAvailable()
		{
			return this.antiCheatInterface != null;
		}

		// Token: 0x0600DE05 RID: 56837 RVA: 0x004EEE47 File Offset: 0x004ED047
		public bool EncryptionAvailable()
		{
			return this.ServerEacEnabled();
		}

		// Token: 0x0600DE06 RID: 56838 RVA: 0x004EEE50 File Offset: 0x004ED050
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			int num = (int)_stream.Length;
			_stream.SetLength((long)(num + 40));
			ArraySegment<byte> arraySegment = new ArraySegment<byte>(_stream.GetBuffer(), 0, num);
			ProtectMessageOptions protectMessageOptions = new ProtectMessageOptions
			{
				ClientHandle = AntiCheatCommon.ClientInfoToIntPtr(_cInfo),
				Data = arraySegment,
				OutBufferSizeBytes = (uint)(num + 40)
			};
			byte[] array = MemoryPools.poolByte.Alloc(num + 40);
			ArraySegment<byte> arraySegment2 = new ArraySegment<byte>(array);
			object lockObject = AntiCheatCommon.LockObject;
			uint num2;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.ProtectMessage(ref protectMessageOptions, arraySegment2, out num2);
			}
			_stream.SetLength(0L);
			_stream.Write(array, 0, (int)num2);
			_stream.Position = 0L;
			MemoryPools.poolByte.Free(array);
			if (result != Result.Success)
			{
				Log.Error(string.Format("[EOS-ACS] Failed encrypting stream for {0}: {1}", _cInfo.InternalId, result.ToStringCached<Result>()));
				return false;
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACS] Encrypted. Orig stream len={0}, result len={1}", num, num2));
			}
			_stream.SetLength((long)((ulong)num2));
			return true;
		}

		// Token: 0x0600DE07 RID: 56839 RVA: 0x004EEF7C File Offset: 0x004ED17C
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			int num = (int)_stream.Length;
			ArraySegment<byte> arraySegment = new ArraySegment<byte>(_stream.GetBuffer(), 0, num);
			UnprotectMessageOptions unprotectMessageOptions = new UnprotectMessageOptions
			{
				ClientHandle = AntiCheatCommon.ClientInfoToIntPtr(_cInfo),
				Data = arraySegment,
				OutBufferSizeBytes = (uint)num
			};
			byte[] array = MemoryPools.poolByte.Alloc(num);
			ArraySegment<byte> arraySegment2 = new ArraySegment<byte>(array);
			object lockObject = AntiCheatCommon.LockObject;
			uint num2;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.UnprotectMessage(ref unprotectMessageOptions, arraySegment2, out num2);
			}
			_stream.SetLength(0L);
			try
			{
				_stream.Write(array, 0, (int)num2);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
			_stream.Position = 0L;
			MemoryPools.poolByte.Free(array);
			if (result != Result.Success)
			{
				Log.Error(string.Format("[EOS-ACS] Failed decrypting stream from {0}: {1}", _cInfo.InternalId, result.ToStringCached<Result>()));
				return false;
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACS] Decrypted. Orig stream len={0}, result len={1}", num, num2));
			}
			_stream.SetLength((long)((ulong)num2));
			return true;
		}

		// Token: 0x0400A797 RID: 42903
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A798 RID: 42904
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatServerInterface antiCheatInterface;

		// Token: 0x0400A799 RID: 42905
		[PublicizedFrom(EAccessModifier.Private)]
		public bool serverRunning;

		// Token: 0x0400A79A RID: 42906
		[PublicizedFrom(EAccessModifier.Private)]
		public AuthenticationSuccessfulCallbackDelegate authSuccessfulDelegate;

		// Token: 0x0400A79B RID: 42907
		[PublicizedFrom(EAccessModifier.Private)]
		public KickPlayerDelegate kickPlayerDelegate;

		// Token: 0x0400A79C RID: 42908
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToClientID;

		// Token: 0x0400A79D RID: 42909
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleClientAuthStateChangeID;

		// Token: 0x0400A79E RID: 42910
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleClientActionRequiredID;
	}
}
