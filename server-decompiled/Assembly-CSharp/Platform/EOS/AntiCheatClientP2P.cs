using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatCommon;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001CDE RID: 7390
	public class AntiCheatClientP2P
	{
		// Token: 0x1400013F RID: 319
		// (add) Token: 0x0600DB49 RID: 56137 RVA: 0x004E537C File Offset: 0x004E357C
		// (remove) Token: 0x0600DB4A RID: 56138 RVA: 0x004E53DC File Offset: 0x004E35DC
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

		// Token: 0x0600DB4B RID: 56139 RVA: 0x004E5430 File Offset: 0x004E3630
		public AntiCheatClientP2P(IPlatform _owner, AntiCheatClientInterface _antiCheatInterface)
		{
			this.owner = _owner;
			this.antiCheatInterface = _antiCheatInterface;
			this.connectInterface = ((Api)this.owner.Api).ConnectInterface;
		}

		// Token: 0x0600DB4C RID: 56140 RVA: 0x004E5490 File Offset: 0x004E3690
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

		// Token: 0x0600DB4D RID: 56141 RVA: 0x004E55AC File Offset: 0x004E37AC
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

		// Token: 0x0600DB4E RID: 56142 RVA: 0x004E5648 File Offset: 0x004E3848
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

		// Token: 0x0600DB4F RID: 56143 RVA: 0x004E56E4 File Offset: 0x004E38E4
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

		// Token: 0x0600DB50 RID: 56144 RVA: 0x004E57FC File Offset: 0x004E39FC
		public bool IsServerAntiCheatProtected()
		{
			return this.serverDeviceType.RequiresAntiCheat();
		}

		// Token: 0x0600DB51 RID: 56145 RVA: 0x004E580C File Offset: 0x004E3A0C
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

		// Token: 0x0600DB52 RID: 56146 RVA: 0x004E58C8 File Offset: 0x004E3AC8
		public void DisconnectFromServer()
		{
			this.clientAuthStatus = AntiCheatCommonClientAuthStatus.Invalid;
			this.serverDeviceType = ClientInfo.EDeviceType.Unknown;
			this.onRemoteAuthComplete = null;
			this.EndSession();
			this.Deactivate();
			Log.Out("[EOS-ACC] Disconnected from game server");
		}

		// Token: 0x0600DB53 RID: 56147 RVA: 0x004E58F8 File Offset: 0x004E3AF8
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

		// Token: 0x0600DB54 RID: 56148 RVA: 0x004E5970 File Offset: 0x004E3B70
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleMessageToPeer(ref OnMessageToClientCallbackInfo _data)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEAC>().Setup(_data.MessageData.Count, _data.MessageData.Array), false);
		}

		// Token: 0x0600DB55 RID: 56149 RVA: 0x004E59B0 File Offset: 0x004E3BB0
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

		// Token: 0x0600DB56 RID: 56150 RVA: 0x004E5A90 File Offset: 0x004E3C90
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

		// Token: 0x0600DB57 RID: 56151 RVA: 0x004E5AA6 File Offset: 0x004E3CA6
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueueDisconnectFromServer(GameUtils.KickPlayerData _kickPlayerData)
		{
			ThreadManager.StartCoroutine(this.DisconnectOnNextFrame(_kickPlayerData));
		}

		// Token: 0x0600DB58 RID: 56152 RVA: 0x004E5AB8 File Offset: 0x004E3CB8
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

		// Token: 0x0400A5C2 RID: 42434
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5C3 RID: 42435
		[PublicizedFrom(EAccessModifier.Private)]
		public ConnectInterface connectInterface;

		// Token: 0x0400A5C4 RID: 42436
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatClientInterface antiCheatInterface;

		// Token: 0x0400A5C5 RID: 42437
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handleMessageToPeerID;

		// Token: 0x0400A5C6 RID: 42438
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerAuthStateChangeID;

		// Token: 0x0400A5C7 RID: 42439
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong handlePeerActionRequiredID;

		// Token: 0x0400A5C8 RID: 42440
		[PublicizedFrom(EAccessModifier.Private)]
		public IntPtr serverHandle = new IntPtr(int.MaxValue);

		// Token: 0x0400A5C9 RID: 42441
		[PublicizedFrom(EAccessModifier.Private)]
		public ClientInfo.EDeviceType serverDeviceType = ClientInfo.EDeviceType.Unknown;

		// Token: 0x0400A5CA RID: 42442
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object lockObject = new object();

		// Token: 0x0400A5CB RID: 42443
		[PublicizedFrom(EAccessModifier.Private)]
		public AntiCheatCommonClientAuthStatus clientAuthStatus;

		// Token: 0x0400A5CC RID: 42444
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onRemoteAuthComplete;
	}
}
