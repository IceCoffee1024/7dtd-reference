using System;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;

namespace Platform.EOS
{
	// Token: 0x02001CD9 RID: 7385
	public class AntiCheatClientCS
	{
		// Token: 0x0600DB2D RID: 56109 RVA: 0x004E4A08 File Offset: 0x004E2C08
		public AntiCheatClientCS(IPlatform _owner, AntiCheatClientInterface _antiCheatInterface)
		{
			this.owner = _owner;
			this.antiCheatInterface = _antiCheatInterface;
		}

		// Token: 0x0600DB2E RID: 56110 RVA: 0x004E4A20 File Offset: 0x004E2C20
		public void Activate()
		{
			AddNotifyMessageToServerOptions addNotifyMessageToServerOptions = default(AddNotifyMessageToServerOptions);
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.handleMessageToServerID = this.antiCheatInterface.AddNotifyMessageToServer(ref addNotifyMessageToServerOptions, null, new OnMessageToServerCallback(this.handleMessageToServer));
			}
		}

		// Token: 0x0600DB2F RID: 56111 RVA: 0x004E4A84 File Offset: 0x004E2C84
		public void Deactivate()
		{
			if (this.handleMessageToServerID != 0UL)
			{
				this.antiCheatInterface.RemoveNotifyMessageToServer(this.handleMessageToServerID);
				this.handleMessageToServerID = 0UL;
			}
		}

		// Token: 0x0600DB30 RID: 56112 RVA: 0x004E4AA8 File Offset: 0x004E2CA8
		public void ConnectToServer(Action _onNoAntiCheatOrConnectionComplete, Action<string> _onConnectionFailed)
		{
			ProductUserId productUserId = ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId;
			BeginSessionOptions beginSessionOptions = new BeginSessionOptions
			{
				LocalUserId = productUserId,
				Mode = AntiCheatClientMode.ClientServer
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.BeginSession(ref beginSessionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACC] Begin session failed: " + result.ToStringCached<Result>());
				if (_onConnectionFailed != null)
				{
					_onConnectionFailed(result.ToStringCached<Result>());
				}
			}
			Log.Out("[EOS-ACC] Connected to game server");
			if (_onNoAntiCheatOrConnectionComplete != null)
			{
				_onNoAntiCheatOrConnectionComplete();
			}
		}

		// Token: 0x0600DB31 RID: 56113 RVA: 0x004E4B64 File Offset: 0x004E2D64
		public void HandleMessageFromServer(byte[] _data)
		{
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACC] PushNetworkMessage (len={0})", _data.Length));
			}
			EosHelpers.AssertMainThread("ACC.HMFS");
			ReceiveMessageFromServerOptions receiveMessageFromServerOptions = new ReceiveMessageFromServerOptions
			{
				Data = new ArraySegment<byte>(_data)
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.ReceiveMessageFromServer(ref receiveMessageFromServerOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACC] Failed handling message: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DB32 RID: 56114 RVA: 0x004E4C04 File Offset: 0x004E2E04
		public void DisconnectFromServer()
		{
			Log.Out("[EOS-ACC] Disconnected from game server");
			EosHelpers.AssertMainThread("ACC.Disc");
			EndSessionOptions endSessionOptions = default(EndSessionOptions);
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = this.antiCheatInterface.EndSession(ref endSessionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACC] Stopping module failed: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DB33 RID: 56115 RVA: 0x004E4C80 File Offset: 0x004E2E80
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleMessageToServer(ref OnMessageToServerCallbackInfo _data)
		{
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACC] Forward message to server (len={0})", _data.MessageData.Count));
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEAC>().Setup(_data.MessageData.Count, _data.MessageData.Array), false);
		}

		// Token: 0x0600DB34 RID: 56116 RVA: 0x004E4CE8 File Offset: 0x004E2EE8
		public bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			int num = (int)_stream.Length;
			_stream.SetLength((long)(num + 40));
			ArraySegment<byte> arraySegment = new ArraySegment<byte>(_stream.GetBuffer(), 0, num);
			ProtectMessageOptions protectMessageOptions = new ProtectMessageOptions
			{
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
				Log.Error("[EOS-ACC] Failed encrypting stream: " + result.ToStringCached<Result>());
				return false;
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACC] Encrypted. Orig stream len={0}, result len={1}", num, num2));
			}
			_stream.SetLength((long)((ulong)num2));
			return true;
		}

		// Token: 0x0600DB35 RID: 56117 RVA: 0x004E4E00 File Offset: 0x004E3000
		public bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream)
		{
			int num = (int)_stream.Length;
			ArraySegment<byte> arraySegment = new ArraySegment<byte>(_stream.GetBuffer(), 0, num);
			UnprotectMessageOptions unprotectMessageOptions = new UnprotectMessageOptions
			{
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
				Log.Error("[EOS-ACC] Failed decrypting stream: " + result.ToStringCached<Result>());
				return false;
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACC] Decrypted. Orig stream len={0}, result len={1}", num, num2));
			}
			_stream.SetLength((long)((ulong)num2));
			return true;
		}

		// Token: 0x0400A5AE RID: 42414
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5AF RID: 42415
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A5B0 RID: 42416
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToServerID;
	}
}
