using System;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;

namespace Platform.EOS
{
	// Token: 0x02001CB2 RID: 7346
	public class AntiCheatClientCS
	{
		// Token: 0x0600D992 RID: 55698 RVA: 0x004DE3F4 File Offset: 0x004DC5F4
		public AntiCheatClientCS(IPlatform _owner, AntiCheatClientInterface _antiCheatInterface)
		{
			this.owner = _owner;
			this.antiCheatInterface = _antiCheatInterface;
		}

		// Token: 0x0600D993 RID: 55699 RVA: 0x004DE40C File Offset: 0x004DC60C
		public void Activate()
		{
			AddNotifyMessageToServerOptions addNotifyMessageToServerOptions = default(AddNotifyMessageToServerOptions);
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.handleMessageToServerID = this.antiCheatInterface.AddNotifyMessageToServer(ref addNotifyMessageToServerOptions, null, new OnMessageToServerCallback(this.handleMessageToServer));
			}
		}

		// Token: 0x0600D994 RID: 55700 RVA: 0x004DE470 File Offset: 0x004DC670
		public void Deactivate()
		{
			if (this.handleMessageToServerID != 0UL)
			{
				this.antiCheatInterface.RemoveNotifyMessageToServer(this.handleMessageToServerID);
				this.handleMessageToServerID = 0UL;
			}
		}

		// Token: 0x0600D995 RID: 55701 RVA: 0x004DE494 File Offset: 0x004DC694
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

		// Token: 0x0600D996 RID: 55702 RVA: 0x004DE550 File Offset: 0x004DC750
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

		// Token: 0x0600D997 RID: 55703 RVA: 0x004DE5F0 File Offset: 0x004DC7F0
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

		// Token: 0x0600D998 RID: 55704 RVA: 0x004DE66C File Offset: 0x004DC86C
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleMessageToServer(ref OnMessageToServerCallbackInfo _data)
		{
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACC] Forward message to server (len={0})", _data.MessageData.Count));
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEAC>().Setup(_data.MessageData.Count, _data.MessageData.Array), false);
		}

		// Token: 0x0600D999 RID: 55705 RVA: 0x004DE6D4 File Offset: 0x004DC8D4
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

		// Token: 0x0600D99A RID: 55706 RVA: 0x004DE7EC File Offset: 0x004DC9EC
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

		// Token: 0x0400A47B RID: 42107
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A47C RID: 42108
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A47D RID: 42109
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToServerID;
	}
}
