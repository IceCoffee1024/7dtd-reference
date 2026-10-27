using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatCommon;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D41 RID: 7489
	public class AntiCheatClientP2P
	{
		// Token: 0x1400013E RID: 318
		// (add) Token: 0x0600DDD8 RID: 56792 RVA: 0x004EDA64 File Offset: 0x004EBC64
		// (remove) Token: 0x0600DDD9 RID: 56793 RVA: 0x004EDAC4 File Offset: 0x004EBCC4
		public event Action OnRemoteAuthComplete
		{
			add
			{
				object obj = this.lockObject;
				lock (obj)
				{
					this.onRemoteAuthComplete = (Action)Delegate.Combine(this.onRemoteAuthComplete, value);
					if (this.clientAuthStatus == AntiCheatCommonClientAuthStatus.RemoteAuthComplete)
					{
						value();
					}
				}
			}
			remove
			{
				object obj = this.lockObject;
				lock (obj)
				{
					this.onRemoteAuthComplete = (Action)Delegate.Remove(this.onRemoteAuthComplete, value);
				}
			}
		}

		// Token: 0x0600DDDA RID: 56794 RVA: 0x004EDB18 File Offset: 0x004EBD18
		public AntiCheatClientP2P(IPlatform _owner, AntiCheatClientInterface _antiCheatInterface)
		{
			this.owner = _owner;
			this.antiCheatInterface = _antiCheatInterface;
			this.connectInterface = ((Api)this.owner.Api).ConnectInterface;
		}

		// Token: 0x0600DDDB RID: 56795 RVA: 0x004EDB78 File Offset: 0x004EBD78
		public void Activate()
		{
			if (this.handleMessageToPeerID == 0UL)
			{
				AddNotifyMessageToPeerOptions addNotifyMessageToPeerOptions = default(AddNotifyMessageToPeerOptions);
				object obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					this.handleMessageToPeerID = this.antiCheatInterface.AddNotifyMessageToPeer(ref addNotifyMessageToPeerOptions, null, new OnMessageToPeerCallback(this.handleMessageToPeer));
				}
			}
			if (this.handlePeerAuthStateChangeID == 0UL)
			{
				AddNotifyPeerAuthStatusChangedOptions addNotifyPeerAuthStatusChangedOptions = default(AddNotifyPeerAuthStatusChangedOptions);
				object obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					this.handlePeerAuthStateChangeID = this.antiCheatInterface.AddNotifyPeerAuthStatusChanged(ref addNotifyPeerAuthStatusChangedOptions, null, new OnPeerAuthStatusChangedCallback(this.handlePeerAuthStateChange));
				}
			}
			if (this.handlePeerActionRequiredID == 0UL)
			{
				AddNotifyPeerActionRequiredOptions addNotifyPeerActionRequiredOptions = default(AddNotifyPeerActionRequiredOptions);
				object obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					this.handlePeerActionRequiredID = this.antiCheatInterface.AddNotifyPeerActionRequired(ref addNotifyPeerActionRequiredOptions, null, new OnPeerActionRequiredCallback(this.handlePeerActionRequired));
				}
			}
		}

		// Token: 0x0600DDDC RID: 56796 RVA: 0x004EDC94 File Offset: 0x004EBE94
		public void Deactivate()
		{
			object obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				if (this.handleMessageToPeerID != 0UL)
				{
					this.antiCheatInterface.RemoveNotifyMessageToPeer(this.handleMessageToPeerID);
					this.handleMessageToPeerID = 0UL;
				}
				if (this.handlePeerAuthStateChangeID != 0UL)
				{
					this.antiCheatInterface.RemoveNotifyPeerAuthStatusChanged(this.handlePeerAuthStateChangeID);
					this.handlePeerAuthStateChangeID = 0UL;
				}
				if (this.handlePeerActionRequiredID != 0UL)
				{
					this.antiCheatInterface.RemoveNotifyPeerActionRequired(this.handlePeerActionRequiredID);
					this.handlePeerActionRequiredID = 0UL;
				}
			}
		}

		// Token: 0x0600DDDD RID: 56797 RVA: 0x004EDD30 File Offset: 0x004EBF30
		[PublicizedFrom(EAccessModifier.Private)]
		public bool BeginSession()
		{
			ProductUserId productUserId = ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId;
			BeginSessionOptions beginSessionOptions = new BeginSessionOptions
			{
				LocalUserId = productUserId,
				Mode = AntiCheatClientMode.PeerToPeer
			};
			object obj = AntiCheatCommon.LockObject;
			Result result;
			lock (obj)
			{
				result = this.antiCheatInterface.BeginSession(ref beginSessionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACCP2P] Starting module failed: " + result.ToStringCached<Result>());
				return false;
			}
			return true;
		}

		// Token: 0x0600DDDE RID: 56798 RVA: 0x004EDDCC File Offset: 0x004EBFCC
		public void ConnectToServer([TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _hostIdentifierAndToken, Action _onConnectionComplete, Action<string> _onConnectionFailed)
		{
			AntiCheatClientP2P.<>c__DisplayClass18_0 CS$<>8__locals1 = new AntiCheatClientP2P.<>c__DisplayClass18_0();
			CS$<>8__locals1._onConnectionFailed = _onConnectionFailed;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1._onConnectionComplete = _onConnectionComplete;
			PlatformUserIdentifierAbs item = _hostIdentifierAndToken.Item1;
			CS$<>8__locals1.identifierEos = item as UserIdentifierEos;
			if (CS$<>8__locals1.identifierEos != null)
			{
				CS$<>8__locals1.identifierEos.DecodeTicket(_hostIdentifierAndToken.Item2);
				IdToken idToken = new IdToken
				{
					JsonWebToken = CS$<>8__locals1.identifierEos.Ticket,
					ProductUserId = CS$<>8__locals1.identifierEos.ProductUserId
				};
				VerifyIdTokenOptions verifyIdTokenOptions = new VerifyIdTokenOptions
				{
					IdToken = new IdToken?(idToken)
				};
				object obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					this.connectInterface.VerifyIdToken(ref verifyIdTokenOptions, null, new OnVerifyIdTokenCallback(CS$<>8__locals1.<ConnectToServer>g__VerifyIdTokenCallback|0));
				}
				return;
			}
			Log.Warning(string.Format("[EOS] [ACl.Auth] Expected EOS Crossplatform ID? But got: {0}", _hostIdentifierAndToken.Item1));
			Action<string> onConnectionFailed = CS$<>8__locals1._onConnectionFailed;
			if (onConnectionFailed == null)
			{
				return;
			}
			onConnectionFailed("Invalid EOS Crossplatform ID");
		}

		// Token: 0x0600DDDF RID: 56799 RVA: 0x004EDEE4 File Offset: 0x004EC0E4
		public bool IsServerAntiCheatProtected()
		{
			return this.serverDeviceType.RequiresAntiCheat();
		}

		// Token: 0x0600DDE0 RID: 56800 RVA: 0x004EDEF4 File Offset: 0x004EC0F4
		public void HandleMessageFromPeer(byte[] _data)
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				return;
			}
			if (AntiCheatCommon.DebugEacVerbose)
			{
				Log.Out(string.Format("[EOS-ACC] PushNetworkMessage (len={0})", _data.Length));
			}
			EosHelpers.AssertMainThread("ACC.HMFP");
			ReceiveMessageFromPeerOptions receiveMessageFromPeerOptions = new ReceiveMessageFromPeerOptions
			{
				Data = new ArraySegment<byte>(_data),
				PeerHandle = this.serverHandle
			};
			object obj = AntiCheatCommon.LockObject;
			Result result;
			lock (obj)
			{
				result = this.antiCheatInterface.ReceiveMessageFromPeer(ref receiveMessageFromPeerOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACC] Failed handling message: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DDE1 RID: 56801 RVA: 0x004EDFB0 File Offset: 0x004EC1B0
		public void DisconnectFromServer()
		{
			this.clientAuthStatus = AntiCheatCommonClientAuthStatus.Invalid;
			this.serverDeviceType = ClientInfo.EDeviceType.Unknown;
			this.onRemoteAuthComplete = null;
			this.EndSession();
			this.Deactivate();
			Log.Out("[EOS-ACC] Disconnected from game server");
		}

		// Token: 0x0600DDE2 RID: 56802 RVA: 0x004EDFE0 File Offset: 0x004EC1E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void EndSession()
		{
			EosHelpers.AssertMainThread("ACC.Disc");
			EndSessionOptions endSessionOptions = default(EndSessionOptions);
			object obj = AntiCheatCommon.LockObject;
			Result result;
			lock (obj)
			{
				result = this.antiCheatInterface.EndSession(ref endSessionOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-ACC] Stopping module failed: " + result.ToStringCached<Result>());
			}
		}

		// Token: 0x0600DDE3 RID: 56803 RVA: 0x004EE058 File Offset: 0x004EC258
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleMessageToPeer(ref OnMessageToClientCallbackInfo _data)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEAC>().Setup(_data.MessageData.Count, _data.MessageData.Array), false);
		}

		// Token: 0x0600DDE4 RID: 56804 RVA: 0x004EE098 File Offset: 0x004EC298
		[PublicizedFrom(EAccessModifier.Private)]
		public void handlePeerActionRequired(ref OnClientActionRequiredCallbackInfo _data)
		{
			if (_data.ClientHandle != this.serverHandle)
			{
				Log.Error("[EOS-ACCP2P] Received Peer action for non-server peer as a client.");
				return;
			}
			AntiCheatCommonClientAction clientAction = _data.ClientAction;
			AntiCheatCommonClientActionReason actionReasonCode = _data.ActionReasonCode;
			string text = _data.ActionReasonDetailsString;
			if (clientAction == AntiCheatCommonClientAction.RemovePlayer)
			{
				Log.Out(string.Concat(new string[]
				{
					"[EOS-ACCP2P] Disconnecting from server. Reason=",
					actionReasonCode.ToStringCached<AntiCheatCommonClientActionReason>(),
					", details='",
					text,
					"'"
				}));
				GameUtils.EKickReason ekickReason = GameUtils.EKickReason.EosEacViolation;
				int num = (int)actionReasonCode;
				string text2 = text;
				this.QueueDisconnectFromServer(new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text2));
				return;
			}
			Log.Warning(string.Concat(new string[]
			{
				"[EOS-ACCP2P] Got invalid action (",
				clientAction.ToStringCached<AntiCheatCommonClientAction>(),
				"), reason='",
				actionReasonCode.ToStringCached<AntiCheatCommonClientActionReason>(),
				"', details='",
				text,
				"'"
			}));
		}

		// Token: 0x0600DDE5 RID: 56805 RVA: 0x004EE178 File Offset: 0x004EC378
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator DisconnectOnNextFrame(GameUtils.KickPlayerData _kickPlayerData)
		{
			yield return null;
			this.EndSession();
			this.Deactivate();
			SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
			GameManager.Instance.ShowMessagePlayerDenied(_kickPlayerData);
			yield break;
		}

		// Token: 0x0600DDE6 RID: 56806 RVA: 0x004EE18E File Offset: 0x004EC38E
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueueDisconnectFromServer(GameUtils.KickPlayerData _kickPlayerData)
		{
			ThreadManager.StartCoroutine(this.DisconnectOnNextFrame(_kickPlayerData));
		}

		// Token: 0x0600DDE7 RID: 56807 RVA: 0x004EE1A0 File Offset: 0x004EC3A0
		[PublicizedFrom(EAccessModifier.Private)]
		public void handlePeerAuthStateChange(ref OnClientAuthStatusChangedCallbackInfo _data)
		{
			if (_data.ClientHandle != this.serverHandle)
			{
				Log.Error("[EOS-ACCP2P] Received Peer auth state change for non-server peer as a client.");
				return;
			}
			this.clientAuthStatus = _data.ClientAuthStatus;
			if (this.clientAuthStatus == AntiCheatCommonClientAuthStatus.RemoteAuthComplete)
			{
				Log.Out("[EOS-ACCP2P] Auth State Change for Server : " + this.clientAuthStatus.ToStringCached<AntiCheatCommonClientAuthStatus>());
				Action action = this.onRemoteAuthComplete;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x0400A780 RID: 42880
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A781 RID: 42881
		[PublicizedFrom(EAccessModifier.Private)]
		public ConnectInterface connectInterface;

		// Token: 0x0400A782 RID: 42882
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A783 RID: 42883
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToPeerID;

		// Token: 0x0400A784 RID: 42884
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerAuthStateChangeID;

		// Token: 0x0400A785 RID: 42885
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerActionRequiredID;

		// Token: 0x0400A786 RID: 42886
		[PublicizedFrom(EAccessModifier.Private)]
		public IntPtr serverHandle = new IntPtr(int.MaxValue);

		// Token: 0x0400A787 RID: 42887
		[PublicizedFrom(EAccessModifier.Private)]
		public ClientInfo.EDeviceType serverDeviceType = ClientInfo.EDeviceType.Unknown;

		// Token: 0x0400A788 RID: 42888
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObject = new object();

		// Token: 0x0400A789 RID: 42889
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatCommonClientAuthStatus clientAuthStatus;

		// Token: 0x0400A78A RID: 42890
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onRemoteAuthComplete;
	}
}
