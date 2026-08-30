using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatCommon;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001CB7 RID: 7351
	public class AntiCheatClientP2P
	{
		// Token: 0x1400013E RID: 318
		// (add) Token: 0x0600D9AE RID: 55726 RVA: 0x004DED68 File Offset: 0x004DCF68
		// (remove) Token: 0x0600D9AF RID: 55727 RVA: 0x004DEDC8 File Offset: 0x004DCFC8
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

		// Token: 0x0600D9B0 RID: 55728 RVA: 0x004DEE1C File Offset: 0x004DD01C
		public AntiCheatClientP2P(IPlatform _owner, AntiCheatClientInterface _antiCheatInterface)
		{
			this.owner = _owner;
			this.antiCheatInterface = _antiCheatInterface;
			this.connectInterface = ((Api)this.owner.Api).ConnectInterface;
		}

		// Token: 0x0600D9B1 RID: 55729 RVA: 0x004DEE7C File Offset: 0x004DD07C
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

		// Token: 0x0600D9B2 RID: 55730 RVA: 0x004DEF98 File Offset: 0x004DD198
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

		// Token: 0x0600D9B3 RID: 55731 RVA: 0x004DF034 File Offset: 0x004DD234
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

		// Token: 0x0600D9B4 RID: 55732 RVA: 0x004DF0D0 File Offset: 0x004DD2D0
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

		// Token: 0x0600D9B5 RID: 55733 RVA: 0x004DF1E8 File Offset: 0x004DD3E8
		public bool IsServerAntiCheatProtected()
		{
			return this.serverDeviceType.RequiresAntiCheat();
		}

		// Token: 0x0600D9B6 RID: 55734 RVA: 0x004DF1F8 File Offset: 0x004DD3F8
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

		// Token: 0x0600D9B7 RID: 55735 RVA: 0x004DF2B4 File Offset: 0x004DD4B4
		public void DisconnectFromServer()
		{
			this.clientAuthStatus = AntiCheatCommonClientAuthStatus.Invalid;
			this.serverDeviceType = ClientInfo.EDeviceType.Unknown;
			this.onRemoteAuthComplete = null;
			this.EndSession();
			this.Deactivate();
			Log.Out("[EOS-ACC] Disconnected from game server");
		}

		// Token: 0x0600D9B8 RID: 55736 RVA: 0x004DF2E4 File Offset: 0x004DD4E4
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

		// Token: 0x0600D9B9 RID: 55737 RVA: 0x004DF35C File Offset: 0x004DD55C
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleMessageToPeer(ref OnMessageToClientCallbackInfo _data)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEAC>().Setup(_data.MessageData.Count, _data.MessageData.Array), false);
		}

		// Token: 0x0600D9BA RID: 55738 RVA: 0x004DF39C File Offset: 0x004DD59C
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

		// Token: 0x0600D9BB RID: 55739 RVA: 0x004DF47C File Offset: 0x004DD67C
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

		// Token: 0x0600D9BC RID: 55740 RVA: 0x004DF492 File Offset: 0x004DD692
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueueDisconnectFromServer(GameUtils.KickPlayerData _kickPlayerData)
		{
			ThreadManager.StartCoroutine(this.DisconnectOnNextFrame(_kickPlayerData));
		}

		// Token: 0x0600D9BD RID: 55741 RVA: 0x004DF4A4 File Offset: 0x004DD6A4
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

		// Token: 0x0400A48F RID: 42127
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A490 RID: 42128
		[PublicizedFrom(EAccessModifier.Private)]
		public ConnectInterface connectInterface;

		// Token: 0x0400A491 RID: 42129
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A492 RID: 42130
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToPeerID;

		// Token: 0x0400A493 RID: 42131
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerAuthStateChangeID;

		// Token: 0x0400A494 RID: 42132
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerActionRequiredID;

		// Token: 0x0400A495 RID: 42133
		[PublicizedFrom(EAccessModifier.Private)]
		public IntPtr serverHandle = new IntPtr(int.MaxValue);

		// Token: 0x0400A496 RID: 42134
		[PublicizedFrom(EAccessModifier.Private)]
		public ClientInfo.EDeviceType serverDeviceType = ClientInfo.EDeviceType.Unknown;

		// Token: 0x0400A497 RID: 42135
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObject = new object();

		// Token: 0x0400A498 RID: 42136
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatCommonClientAuthStatus clientAuthStatus;

		// Token: 0x0400A499 RID: 42137
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onRemoteAuthComplete;
	}
}
