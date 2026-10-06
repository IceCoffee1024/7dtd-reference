using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAudio;
using UnityEngine;

namespace Platform.EOS
{
	// Token: 0x02001D61 RID: 7521
	public class Voice : IPartyVoice
	{
		// Token: 0x17001C42 RID: 7234
		// (get) Token: 0x0600DEC9 RID: 57033 RVA: 0x004F6ED3 File Offset: 0x004F50D3
		public UserIdentifierEos localUserIdentifier
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				IPlatform platform = this.owner;
				object obj;
				if (platform == null)
				{
					obj = null;
				}
				else
				{
					IUserClient user = platform.User;
					obj = ((user != null) ? user.PlatformUserId : null);
				}
				return (UserIdentifierEos)obj;
			}
		}

		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x0600DECA RID: 57034 RVA: 0x004F6EF8 File Offset: 0x004F50F8
		public ProductUserId localProductUserId
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				UserIdentifierEos localUserIdentifier = this.localUserIdentifier;
				if (localUserIdentifier == null)
				{
					return null;
				}
				return localUserIdentifier.ProductUserId;
			}
		}

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x0600DECB RID: 57035 RVA: 0x004F6F0C File Offset: 0x004F510C
		// (remove) Token: 0x0600DECC RID: 57036 RVA: 0x004F6F6C File Offset: 0x004F516C
		public event Action Initialized
		{
			add
			{
				object obj = this.initializedDelegateLock;
				lock (obj)
				{
					this.initializedDelegates = (Action)Delegate.Combine(this.initializedDelegates, value);
					if (this.Status == EPartyVoiceStatus.Ok)
					{
						value();
					}
				}
			}
			remove
			{
				object obj = this.initializedDelegateLock;
				lock (obj)
				{
					this.initializedDelegates = (Action)Delegate.Remove(this.initializedDelegates, value);
				}
			}
		}

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x0600DECD RID: 57037 RVA: 0x004F6FC0 File Offset: 0x004F51C0
		// (set) Token: 0x0600DECE RID: 57038 RVA: 0x004F6FC8 File Offset: 0x004F51C8
		public EPartyVoiceStatus Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EPartyVoiceStatus.Uninitialized;

		// Token: 0x17001C45 RID: 7237
		// (get) Token: 0x0600DECF RID: 57039 RVA: 0x004F6FD1 File Offset: 0x004F51D1
		public bool InLobby
		{
			get
			{
				return this.lobbyId != null && this.roomEntered;
			}
		}

		// Token: 0x17001C46 RID: 7238
		// (get) Token: 0x0600DED0 RID: 57040 RVA: 0x004F6FE3 File Offset: 0x004F51E3
		public bool InLobbyOrProgress
		{
			get
			{
				return this.InLobby || this.createInProgress || this.joinInProgress;
			}
		}

		// Token: 0x0600DED1 RID: 57041 RVA: 0x004F6FFD File Offset: 0x004F51FD
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.api = (Api)this.owner.Api;
			this.api.ClientApiInitialized += this.OnClientApiInitialized;
		}

		// Token: 0x0600DED2 RID: 57042 RVA: 0x004F7034 File Offset: 0x004F5234
		public void Destroy()
		{
			if (this.Status == EPartyVoiceStatus.Ok)
			{
				this.OnPartyVoiceUninitialize();
			}
			this.Status = EPartyVoiceStatus.Uninitialized;
			this.lobbyInterface = null;
			this.audioInterface = null;
			this.rtcInterface = null;
			this.api.ClientApiInitialized -= this.OnClientApiInitialized;
			this.api = null;
			this.owner = null;
		}

		// Token: 0x0600DED3 RID: 57043 RVA: 0x004F7090 File Offset: 0x004F5290
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientApiInitialized()
		{
			PlatformInterface platformInterface = this.api.PlatformInterface;
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.rtcInterface = ((platformInterface != null) ? platformInterface.GetRTCInterface() : null);
				RTCInterface rtcinterface = this.rtcInterface;
				this.audioInterface = ((rtcinterface != null) ? rtcinterface.GetAudioInterface() : null);
				this.lobbyInterface = ((platformInterface != null) ? platformInterface.GetLobbyInterface() : null);
			}
			lockObject = this.initializedDelegateLock;
			lock (lockObject)
			{
				if (this.rtcInterface != null && this.audioInterface != null && this.lobbyInterface != null)
				{
					this.Status = EPartyVoiceStatus.Ok;
					this.OnPartyVoiceInitialized();
					Log.Out("[EOS-Voice] Successfully initialized.");
					Action action = this.initializedDelegates;
					if (action != null)
					{
						action();
					}
				}
				else
				{
					this.Status = EPartyVoiceStatus.PermanentError;
					Log.Warning("[EOS-Voice] Failed to initialize.");
				}
			}
		}

		// Token: 0x0600DED4 RID: 57044 RVA: 0x004F71A0 File Offset: 0x004F53A0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPartyVoiceInitialized()
		{
			this.AddNotifications();
		}

		// Token: 0x0600DED5 RID: 57045 RVA: 0x004F71A8 File Offset: 0x004F53A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPartyVoiceUninitialize()
		{
			this.RemoveNotifications();
		}

		// Token: 0x0600DED6 RID: 57046 RVA: 0x004F71B0 File Offset: 0x004F53B0
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddNotifications()
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not add notifications because voice is currently not ready.");
				return;
			}
			this.AddAudioDevicesNotifications();
		}

		// Token: 0x0600DED7 RID: 57047 RVA: 0x004F71CB File Offset: 0x004F53CB
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveNotifications()
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not remove notifications because voice is currently not ready.");
				return;
			}
			this.RemoveAudioDevicesNotifications();
		}

		// Token: 0x0600DED8 RID: 57048 RVA: 0x004F71E8 File Offset: 0x004F53E8
		public void CreateLobby(Action<string> _lobbyCreatedCallback)
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not create lobby because voice is currently not valid.");
				return;
			}
			if (this.lobbyId != null)
			{
				Log.Error("[EOS-Voice] Can not create lobby while already in another");
				Log.Error(StackTraceUtility.ExtractStackTrace());
				return;
			}
			this.createInProgress = true;
			Log.Out("[EOS-Voice] Creating lobby");
			EosHelpers.AssertMainThread("Voice.Create");
			CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
			{
				LocalUserId = this.localProductUserId,
				MaxLobbyMembers = 8U,
				PermissionLevel = LobbyPermissionLevel.Joinviapresence,
				PresenceEnabled = false,
				AllowInvites = false,
				EnableRTCRoom = true,
				BucketId = "PartyVoice",
				LocalRTCOptions = new LocalRTCOptions?(new LocalRTCOptions
				{
					LocalAudioDeviceInputStartsMuted = true
				})
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.lobbyInterface.CreateLobby(ref createLobbyOptions, _lobbyCreatedCallback, delegate(ref CreateLobbyCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS-Voice] Create lobby failed: " + _callbackData.ResultCode.ToStringCached<Result>());
						this.createInProgress = false;
						Action<string> action = (Action<string>)_callbackData.ClientData;
						if (action == null)
						{
							return;
						}
						action(null);
						return;
					}
					else
					{
						this.lobbyEntered(_callbackData.LobbyId);
						Action<string> action2 = (Action<string>)_callbackData.ClientData;
						if (action2 == null)
						{
							return;
						}
						action2(this.lobbyId);
						return;
					}
				});
			}
		}

		// Token: 0x0600DED9 RID: 57049 RVA: 0x004F72F8 File Offset: 0x004F54F8
		public void JoinLobby(string _lobbyId)
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not join lobby because voice is currently not ready.");
				return;
			}
			if (string.IsNullOrEmpty(_lobbyId))
			{
				Log.Error("[EOS-Voice] Can not join lobby, missing id");
				return;
			}
			if (this.lobbyId != null)
			{
				if (this.lobbyId != _lobbyId)
				{
					Log.Error("[EOS-Voice] Can not join lobby while already in another");
				}
				return;
			}
			Log.Out("[EOS-Voice] Joining lobby");
			this.joinInProgress = true;
			ThreadManager.StartCoroutine(this.tryJoinLobbyCo(_lobbyId));
		}

		// Token: 0x0600DEDA RID: 57050 RVA: 0x004F736C File Offset: 0x004F556C
		public void LeaveLobby()
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not leave lobby because voice is currently not ready.");
				return;
			}
			if (this.lobbyId == null)
			{
				return;
			}
			if (this.leaveInProgress)
			{
				return;
			}
			this.leaveInProgress = true;
			Log.Out("[EOS-Voice] Leaving lobby");
			EosHelpers.AssertMainThread("Voice.Leave");
			LeaveLobbyOptions leaveLobbyOptions = new LeaveLobbyOptions
			{
				LocalUserId = this.localProductUserId,
				LobbyId = this.lobbyId
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.lobbyInterface.LeaveLobby(ref leaveLobbyOptions, null, delegate(ref LeaveLobbyCallbackInfo _callbackData)
				{
					this.lobbyLeft();
					this.leaveInProgress = false;
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS-Voice] Leave lobby failed: " + _callbackData.ResultCode.ToStringCached<Result>());
					}
				});
			}
		}

		// Token: 0x0600DEDB RID: 57051 RVA: 0x004F7428 File Offset: 0x004F5628
		public void PromoteLeader(PlatformUserIdentifierAbs _newLeaderIdentifier)
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not promote leader because voice is currently not ready.");
				return;
			}
			if (!this.IsLobbyOwner())
			{
				return;
			}
			UserIdentifierEos userIdentifierEos = _newLeaderIdentifier as UserIdentifierEos;
			if (userIdentifierEos == null)
			{
				Log.Error(string.Format("[EOS-Voice] New leader user identifier is not an EOS identifier: {0}", _newLeaderIdentifier));
				return;
			}
			Log.Out("[EOS-Voice] Promoting lobby owner");
			EosHelpers.AssertMainThread("Voice.Prom");
			PromoteMemberOptions promoteMemberOptions = new PromoteMemberOptions
			{
				LocalUserId = this.localProductUserId,
				LobbyId = this.lobbyId,
				TargetUserId = userIdentifierEos.ProductUserId
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.lobbyInterface.PromoteMember(ref promoteMemberOptions, null, delegate(ref PromoteMemberCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS-Voice] Promoting leader failed: " + _callbackData.ResultCode.ToStringCached<Result>());
					}
				});
			}
		}

		// Token: 0x0600DEDC RID: 57052 RVA: 0x004F7514 File Offset: 0x004F5714
		public bool IsLobbyOwner()
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not check if lobby owner because voice is currently not ready.");
				return false;
			}
			if (this.lobbyId == null)
			{
				return false;
			}
			EosHelpers.AssertMainThread("Voice.Own");
			CopyLobbyDetailsHandleOptions copyLobbyDetailsHandleOptions = new CopyLobbyDetailsHandleOptions
			{
				LocalUserId = this.localProductUserId,
				LobbyId = this.lobbyId
			};
			object obj = AntiCheatCommon.LockObject;
			LobbyDetails lobbyDetails;
			Result result;
			lock (obj)
			{
				result = this.lobbyInterface.CopyLobbyDetailsHandle(ref copyLobbyDetailsHandleOptions, out lobbyDetails);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS-Voice] Getting local lobby details failed: " + result.ToStringCached<Result>());
				return false;
			}
			LobbyDetailsGetLobbyOwnerOptions lobbyDetailsGetLobbyOwnerOptions = default(LobbyDetailsGetLobbyOwnerOptions);
			obj = AntiCheatCommon.LockObject;
			ProductUserId lobbyOwner;
			lock (obj)
			{
				lobbyOwner = lobbyDetails.GetLobbyOwner(ref lobbyDetailsGetLobbyOwnerOptions);
			}
			bool flag2 = lobbyOwner == this.localProductUserId;
			lobbyDetails.Release();
			return flag2;
		}

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x0600DEDD RID: 57053 RVA: 0x004F7620 File Offset: 0x004F5820
		// (remove) Token: 0x0600DEDE RID: 57054 RVA: 0x004F7658 File Offset: 0x004F5858
		public event Action<IPartyVoice.EVoiceChannelAction> OnLocalPlayerStateChanged;

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x0600DEDF RID: 57055 RVA: 0x004F7690 File Offset: 0x004F5890
		// (remove) Token: 0x0600DEE0 RID: 57056 RVA: 0x004F76C8 File Offset: 0x004F58C8
		public event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> OnRemotePlayerStateChanged;

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x0600DEE1 RID: 57057 RVA: 0x004F7700 File Offset: 0x004F5900
		// (remove) Token: 0x0600DEE2 RID: 57058 RVA: 0x004F7738 File Offset: 0x004F5938
		public event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> OnRemotePlayerVoiceStateChanged;

		// Token: 0x0600DEE3 RID: 57059 RVA: 0x004F7770 File Offset: 0x004F5970
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddAudioDevicesNotifications()
		{
			if (this.audioDevicesChangedNotificationId == 0UL)
			{
				this.RefreshAudioDevices();
				AddNotifyAudioDevicesChangedOptions addNotifyAudioDevicesChangedOptions = default(AddNotifyAudioDevicesChangedOptions);
				object lockObject = AntiCheatCommon.LockObject;
				lock (lockObject)
				{
					this.audioDevicesChangedNotificationId = this.audioInterface.AddNotifyAudioDevicesChanged(ref addNotifyAudioDevicesChangedOptions, null, new OnAudioDevicesChangedCallback(this.<AddAudioDevicesNotifications>g__OnAudioDevicesChanged|60_0));
				}
			}
		}

		// Token: 0x0600DEE4 RID: 57060 RVA: 0x004F77E0 File Offset: 0x004F59E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveAudioDevicesNotifications()
		{
			if (this.audioDevicesChangedNotificationId != 0UL)
			{
				object lockObject = AntiCheatCommon.LockObject;
				lock (lockObject)
				{
					RTCAudioInterface rtcaudioInterface = this.audioInterface;
					if (rtcaudioInterface != null)
					{
						rtcaudioInterface.RemoveNotifyAudioDevicesChanged(this.audioDevicesChangedNotificationId);
					}
				}
				this.audioDevicesChangedNotificationId = 0UL;
			}
		}

		// Token: 0x0600DEE5 RID: 57061 RVA: 0x004F7840 File Offset: 0x004F5A40
		[PublicizedFrom(EAccessModifier.Private)]
		public void RefreshAudioDevices()
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not refresh audio devices because voice is currently not ready.");
				return;
			}
			QueryInputDevicesInformationOptions queryInputDevicesInformationOptions = default(QueryInputDevicesInformationOptions);
			object obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.audioInterface.QueryInputDevicesInformation(ref queryInputDevicesInformationOptions, null, new OnQueryInputDevicesInformationCallback(this.OnQueryInputDevicesInformation));
			}
			QueryOutputDevicesInformationOptions queryOutputDevicesInformationOptions = default(QueryOutputDevicesInformationOptions);
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.audioInterface.QueryOutputDevicesInformation(ref queryOutputDevicesInformationOptions, null, new OnQueryOutputDevicesInformationCallback(this.OnQueryOutputDevicesInformation));
			}
		}

		// Token: 0x0600DEE6 RID: 57062 RVA: 0x004F78F8 File Offset: 0x004F5AF8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnQueryInputDevicesInformation(ref OnQueryInputDevicesInformationCallbackInfo data)
		{
			if (data.ResultCode != Result.Success)
			{
				Log.Error("[EOS-Voice] Query Input Devices Information Failed: " + data.ResultCode.ToStringCached<Result>());
				return;
			}
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Warning(string.Format("[EOS-Voice] can not query input devices information. Voice status: {0}", this.Status));
				return;
			}
			GetInputDevicesCountOptions getInputDevicesCountOptions = default(GetInputDevicesCountOptions);
			object obj = AntiCheatCommon.LockObject;
			uint inputDevicesCount;
			lock (obj)
			{
				inputDevicesCount = this.audioInterface.GetInputDevicesCount(ref getInputDevicesCountOptions);
			}
			IList<IPartyVoice.VoiceAudioDevice> list = new List<IPartyVoice.VoiceAudioDevice>();
			for (uint num = 0U; num < inputDevicesCount; num += 1U)
			{
				CopyInputDeviceInformationByIndexOptions copyInputDeviceInformationByIndexOptions = new CopyInputDeviceInformationByIndexOptions
				{
					DeviceIndex = num
				};
				obj = AntiCheatCommon.LockObject;
				InputDeviceInformation? inputDeviceInformation;
				Result result;
				lock (obj)
				{
					result = this.audioInterface.CopyInputDeviceInformationByIndex(ref copyInputDeviceInformationByIndexOptions, out inputDeviceInformation);
				}
				try
				{
					if (result != Result.Success || inputDeviceInformation == null)
					{
						Log.Warning(string.Format("[EOS-Voice] Could not query input device {0}: {1}", num, result.ToStringCached<Result>()));
					}
					else
					{
						list.Add(new Voice.EosAudioDevice(inputDeviceInformation.Value));
					}
				}
				finally
				{
					bool flag2 = inputDeviceInformation != null;
				}
			}
			this.inputDevices = list;
		}

		// Token: 0x0600DEE7 RID: 57063 RVA: 0x004F7A58 File Offset: 0x004F5C58
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnQueryOutputDevicesInformation(ref OnQueryOutputDevicesInformationCallbackInfo data)
		{
			if (data.ResultCode != Result.Success)
			{
				Log.Error("[EOS-Voice] Query Output Devices Information Failed: " + data.ResultCode.ToStringCached<Result>());
				return;
			}
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Warning(string.Format("[EOS-Voice] can not query output devices information. Voice status: {0}", this.Status));
				return;
			}
			IList<IPartyVoice.VoiceAudioDevice> list = new List<IPartyVoice.VoiceAudioDevice>();
			GetOutputDevicesCountOptions getOutputDevicesCountOptions = default(GetOutputDevicesCountOptions);
			object obj = AntiCheatCommon.LockObject;
			uint outputDevicesCount;
			lock (obj)
			{
				outputDevicesCount = this.audioInterface.GetOutputDevicesCount(ref getOutputDevicesCountOptions);
			}
			for (uint num = 0U; num < outputDevicesCount; num += 1U)
			{
				CopyOutputDeviceInformationByIndexOptions copyOutputDeviceInformationByIndexOptions = new CopyOutputDeviceInformationByIndexOptions
				{
					DeviceIndex = num
				};
				obj = AntiCheatCommon.LockObject;
				OutputDeviceInformation? outputDeviceInformation;
				Result result;
				lock (obj)
				{
					result = this.audioInterface.CopyOutputDeviceInformationByIndex(ref copyOutputDeviceInformationByIndexOptions, out outputDeviceInformation);
				}
				try
				{
					if (result != Result.Success || outputDeviceInformation == null)
					{
						Log.Warning(string.Format("[EOS-Voice] Could not query output device {0}: {1}", num, result.ToStringCached<Result>()));
					}
					else
					{
						list.Add(new Voice.EosAudioDevice(outputDeviceInformation.Value));
					}
				}
				finally
				{
					bool flag2 = outputDeviceInformation != null;
				}
			}
			this.outputDevices = list;
		}

		// Token: 0x0600DEE8 RID: 57064 RVA: 0x004F7BB8 File Offset: 0x004F5DB8
		[return: TupleElementNames(new string[] { "inputDevices", "outputDevices" })]
		public ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>> GetDevicesList()
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not get voice devices because voice is currently not ready.");
				return new ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>>(Array.Empty<IPartyVoice.VoiceAudioDevice>(), Array.Empty<IPartyVoice.VoiceAudioDevice>());
			}
			EosHelpers.AssertMainThread("Voice.GetDev");
			return new ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>>(this.inputDevices, this.outputDevices);
		}

		// Token: 0x0600DEE9 RID: 57065 RVA: 0x004F7BF8 File Offset: 0x004F5DF8
		public void SetInputDevice(string _device)
		{
			Voice.<>c__DisplayClass66_0 CS$<>8__locals1 = new Voice.<>c__DisplayClass66_0();
			CS$<>8__locals1._device = _device;
			CS$<>8__locals1.<>4__this = this;
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not set input device because voice is currently not ready.");
				return;
			}
			EosHelpers.AssertMainThread("Voice.SetIn");
			SetInputDeviceSettingsOptions setInputDeviceSettingsOptions = new SetInputDeviceSettingsOptions
			{
				LocalUserId = this.localProductUserId,
				RealDeviceId = CS$<>8__locals1._device,
				PlatformAEC = true
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.audioInterface.SetInputDeviceSettings(ref setInputDeviceSettingsOptions, null, new OnSetInputDeviceSettingsCallback(CS$<>8__locals1.<SetInputDevice>g__OnSetInputDeviceSettings|0));
			}
		}

		// Token: 0x0600DEEA RID: 57066 RVA: 0x004F7CB0 File Offset: 0x004F5EB0
		public void SetOutputDevice(string _device)
		{
			Voice.<>c__DisplayClass67_0 CS$<>8__locals1 = new Voice.<>c__DisplayClass67_0();
			CS$<>8__locals1._device = _device;
			CS$<>8__locals1.<>4__this = this;
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not set output device because voice is currently not ready.");
				return;
			}
			EosHelpers.AssertMainThread("Voice.SetOut");
			SetOutputDeviceSettingsOptions setOutputDeviceSettingsOptions = new SetOutputDeviceSettingsOptions
			{
				LocalUserId = this.localProductUserId,
				RealDeviceId = CS$<>8__locals1._device
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.audioInterface.SetOutputDeviceSettings(ref setOutputDeviceSettingsOptions, null, new OnSetOutputDeviceSettingsCallback(CS$<>8__locals1.<SetOutputDevice>g__OnSetOutputDeviceSettings|0));
			}
		}

		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x0600DEEB RID: 57067 RVA: 0x004F7D60 File Offset: 0x004F5F60
		// (set) Token: 0x0600DEEC RID: 57068 RVA: 0x004F7D68 File Offset: 0x004F5F68
		public bool MuteSelf
		{
			get
			{
				return this.muteSelf;
			}
			set
			{
				if (this.Status != EPartyVoiceStatus.Ok)
				{
					Log.Error("[EOS-Voice] Can not mute self because voice is currently not ready.");
					return;
				}
				if (value == this.muteSelf)
				{
					return;
				}
				this.muteSelf = value;
				if (this.roomName == null)
				{
					return;
				}
				EosHelpers.AssertMainThread("Voice.Mute");
				UpdateSendingOptions updateSendingOptions = new UpdateSendingOptions
				{
					LocalUserId = this.localProductUserId,
					RoomName = this.roomName,
					AudioStatus = (value ? RTCAudioStatus.Disabled : RTCAudioStatus.Enabled)
				};
				object lockObject = AntiCheatCommon.LockObject;
				lock (lockObject)
				{
					this.audioInterface.UpdateSending(ref updateSendingOptions, null, delegate(ref UpdateSendingCallbackInfo _callbackData)
					{
						if (_callbackData.ResultCode != Result.Success)
						{
							Log.Error("[EOS-Voice] Toggling voice sending state failed: " + _callbackData.ResultCode.ToStringCached<Result>());
						}
					});
				}
			}
		}

		// Token: 0x17001C48 RID: 7240
		// (get) Token: 0x0600DEED RID: 57069 RVA: 0x004F7E3C File Offset: 0x004F603C
		// (set) Token: 0x0600DEEE RID: 57070 RVA: 0x004F7E44 File Offset: 0x004F6044
		public bool MuteOthers
		{
			get
			{
				return this.muteOthers;
			}
			set
			{
				if (this.Status != EPartyVoiceStatus.Ok)
				{
					Log.Error("[EOS-Voice] Can not mute others because voice is currently not ready.");
					return;
				}
				if (value == this.muteOthers)
				{
					return;
				}
				this.muteOthers = value;
				if (this.roomName == null)
				{
					return;
				}
				EosHelpers.AssertMainThread("Voice.MuteOth");
				UpdateReceivingOptions updateReceivingOptions = new UpdateReceivingOptions
				{
					LocalUserId = this.localProductUserId,
					RoomName = this.roomName,
					AudioEnabled = !value
				};
				object lockObject = AntiCheatCommon.LockObject;
				lock (lockObject)
				{
					this.audioInterface.UpdateReceiving(ref updateReceivingOptions, null, delegate(ref UpdateReceivingCallbackInfo _callbackData)
					{
						if (_callbackData.ResultCode != Result.Success)
						{
							Log.Error("[EOS-Voice] Toggling voice receiving state failed: " + _callbackData.ResultCode.ToStringCached<Result>());
							return;
						}
						Log.Out(string.Format("[EOS-Voice] Mute all state changed to: {0}", value));
					});
				}
			}
		}

		// Token: 0x17001C49 RID: 7241
		// (get) Token: 0x0600DEEF RID: 57071 RVA: 0x004F7F20 File Offset: 0x004F6120
		// (set) Token: 0x0600DEF0 RID: 57072 RVA: 0x004F7F28 File Offset: 0x004F6128
		public float OutputVolume
		{
			get
			{
				return this.outputVolume;
			}
			set
			{
				if (this.Status != EPartyVoiceStatus.Ok)
				{
					Log.Error("[EOS-Voice] Can not set output volume because voice is currently not ready.");
					return;
				}
				if ((double)value > (double)this.outputVolume - 0.01 && (double)value < (double)this.outputVolume + 0.01)
				{
					return;
				}
				this.outputVolume = value;
				this.SetRoomReceivingVolume(this.outputVolume);
			}
		}

		// Token: 0x0600DEF1 RID: 57073 RVA: 0x004F7F88 File Offset: 0x004F6188
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetRoomReceivingVolume(float platformVolume)
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not set room receiving volume because voice is currently not ready.");
				return;
			}
			if (this.roomName == null)
			{
				return;
			}
			EosHelpers.AssertMainThread("Voice.Vol");
			UpdateReceivingVolumeOptions updateReceivingVolumeOptions = new UpdateReceivingVolumeOptions
			{
				LocalUserId = this.localProductUserId,
				RoomName = this.roomName,
				Volume = platformVolume * 50f
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.audioInterface.UpdateReceivingVolume(ref updateReceivingVolumeOptions, null, new OnUpdateReceivingVolumeCallback(Voice.<SetRoomReceivingVolume>g__OnUpdateReceivingVolume|80_0));
			}
		}

		// Token: 0x0600DEF2 RID: 57074 RVA: 0x004F8038 File Offset: 0x004F6238
		public void BlockUser(PlatformUserIdentifierAbs _userIdentifier, bool _block)
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				Log.Error("[EOS-Voice] Can not block user because voice is currently not ready.");
				return;
			}
			if (this.roomName == null)
			{
				return;
			}
			if (this.muteOthers)
			{
				return;
			}
			UserIdentifierEos userIdentifierEos = _userIdentifier as UserIdentifierEos;
			if (userIdentifierEos == null)
			{
				Log.Error(string.Format("[EOS-Voice] Block user identifier is not an EOS identifier: {0}", _userIdentifier));
				return;
			}
			Log.Out(string.Format("[EOS-Voice] Blocking user: {0} = {1}", userIdentifierEos, _block));
			EosHelpers.AssertMainThread("Voice.Block");
			BlockParticipantOptions blockParticipantOptions = new BlockParticipantOptions
			{
				LocalUserId = this.localProductUserId,
				RoomName = this.roomName,
				ParticipantId = userIdentifierEos.ProductUserId,
				Blocked = _block
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.rtcInterface.BlockParticipant(ref blockParticipantOptions, null, delegate(ref BlockParticipantCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS-Voice] Blocking user failed: " + _callbackData.ResultCode.ToStringCached<Result>());
						return;
					}
					if (_block)
					{
						this.blockedUsers.Add(_userIdentifier);
						Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> onRemotePlayerVoiceStateChanged = this.OnRemotePlayerVoiceStateChanged;
						if (onRemotePlayerVoiceStateChanged == null)
						{
							return;
						}
						onRemotePlayerVoiceStateChanged(_userIdentifier, IPartyVoice.EVoiceMemberState.Muted);
						return;
					}
					else
					{
						this.blockedUsers.Remove(_userIdentifier);
						Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> onRemotePlayerVoiceStateChanged2 = this.OnRemotePlayerVoiceStateChanged;
						if (onRemotePlayerVoiceStateChanged2 == null)
						{
							return;
						}
						onRemotePlayerVoiceStateChanged2(_userIdentifier, IPartyVoice.EVoiceMemberState.Normal);
						return;
					}
				});
			}
		}

		// Token: 0x0600DEF3 RID: 57075 RVA: 0x004F8160 File Offset: 0x004F6360
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator tryJoinLobbyCo(string _lobbyId)
		{
			Voice.<>c__DisplayClass83_0 CS$<>8__locals1 = new Voice.<>c__DisplayClass83_0();
			CS$<>8__locals1.<>4__this = this;
			EosHelpers.AssertMainThread("Voice.Join");
			int attempts = 0;
			CS$<>8__locals1.lobbyIdFound = null;
			CS$<>8__locals1.lobbyDetails = null;
			object obj;
			while (attempts < 5)
			{
				Voice.<>c__DisplayClass83_1 CS$<>8__locals2 = new Voice.<>c__DisplayClass83_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				int num = attempts;
				attempts = num + 1;
				Log.Out(string.Format("[EOS-Voice] Trying to find lobby for id {0}, attempt {1}", _lobbyId, attempts));
				CreateLobbySearchOptions createLobbySearchOptions = new CreateLobbySearchOptions
				{
					MaxResults = 10U
				};
				obj = AntiCheatCommon.LockObject;
				Result result;
				lock (obj)
				{
					result = this.lobbyInterface.CreateLobbySearch(ref createLobbySearchOptions, out CS$<>8__locals2.lobbySearch);
				}
				if (result != Result.Success)
				{
					Log.Error("[EOS-Voice] Create lobby search failed: " + result.ToStringCached<Result>());
					this.joinInProgress = false;
					yield break;
				}
				LobbySearchSetLobbyIdOptions lobbySearchSetLobbyIdOptions = new LobbySearchSetLobbyIdOptions
				{
					LobbyId = _lobbyId
				};
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					result = CS$<>8__locals2.lobbySearch.SetLobbyId(ref lobbySearchSetLobbyIdOptions);
				}
				if (result != Result.Success)
				{
					Log.Error("[EOS-Voice] Set lobby search lobbyid failed: " + result.ToStringCached<Result>());
					obj = AntiCheatCommon.LockObject;
					lock (obj)
					{
						CS$<>8__locals2.lobbySearch.Release();
					}
					this.joinInProgress = false;
					yield break;
				}
				CS$<>8__locals2.findDone = false;
				CS$<>8__locals2.findError = false;
				CS$<>8__locals2.CS$<>8__locals1.lobbyDetails = null;
				LobbySearchFindOptions lobbySearchFindOptions = new LobbySearchFindOptions
				{
					LocalUserId = this.localProductUserId
				};
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					CS$<>8__locals2.lobbySearch.Find(ref lobbySearchFindOptions, null, delegate(ref LobbySearchFindCallbackInfo _callbackData)
					{
						object obj2;
						if (_callbackData.ResultCode == Result.NotFound)
						{
							obj2 = AntiCheatCommon.LockObject;
							lock (obj2)
							{
								CS$<>8__locals2.lobbySearch.Release();
							}
							CS$<>8__locals2.findDone = true;
							return;
						}
						if (_callbackData.ResultCode != Result.Success)
						{
							Log.Error("[EOS-Voice] Find lobby failed: " + _callbackData.ResultCode.ToStringCached<Result>());
							obj2 = AntiCheatCommon.LockObject;
							lock (obj2)
							{
								CS$<>8__locals2.lobbySearch.Release();
							}
							CS$<>8__locals2.findDone = true;
							CS$<>8__locals2.findError = true;
							return;
						}
						LobbySearchGetSearchResultCountOptions lobbySearchGetSearchResultCountOptions = default(LobbySearchGetSearchResultCountOptions);
						obj2 = AntiCheatCommon.LockObject;
						uint searchResultCount;
						lock (obj2)
						{
							searchResultCount = CS$<>8__locals2.lobbySearch.GetSearchResultCount(ref lobbySearchGetSearchResultCountOptions);
						}
						if (searchResultCount != 1U)
						{
							Log.Error(string.Format("[EOS-Voice] Find lobby returned unexpected number of results ({0})", searchResultCount));
							obj2 = AntiCheatCommon.LockObject;
							lock (obj2)
							{
								CS$<>8__locals2.lobbySearch.Release();
							}
							CS$<>8__locals2.findDone = true;
							return;
						}
						LobbySearchCopySearchResultByIndexOptions lobbySearchCopySearchResultByIndexOptions = new LobbySearchCopySearchResultByIndexOptions
						{
							LobbyIndex = 0U
						};
						obj2 = AntiCheatCommon.LockObject;
						Result result2;
						lock (obj2)
						{
							result2 = CS$<>8__locals2.lobbySearch.CopySearchResultByIndex(ref lobbySearchCopySearchResultByIndexOptions, out CS$<>8__locals2.CS$<>8__locals1.lobbyDetails);
						}
						if (result2 != Result.Success)
						{
							Log.Error("[EOS-Voice] Get lobby details failed: " + result2.ToStringCached<Result>());
							obj2 = AntiCheatCommon.LockObject;
							lock (obj2)
							{
								CS$<>8__locals2.lobbySearch.Release();
							}
							CS$<>8__locals2.CS$<>8__locals1.lobbyDetails = null;
							CS$<>8__locals2.findDone = true;
							CS$<>8__locals2.findError = true;
							return;
						}
						LobbyDetailsCopyInfoOptions lobbyDetailsCopyInfoOptions = default(LobbyDetailsCopyInfoOptions);
						obj2 = AntiCheatCommon.LockObject;
						LobbyDetailsInfo? lobbyDetailsInfo;
						lock (obj2)
						{
							result2 = CS$<>8__locals2.CS$<>8__locals1.lobbyDetails.CopyInfo(ref lobbyDetailsCopyInfoOptions, out lobbyDetailsInfo);
						}
						if (result2 != Result.Success)
						{
							Log.Error("[EOS-Voice] Get lobby details info failed: " + result2.ToStringCached<Result>());
							obj2 = AntiCheatCommon.LockObject;
							lock (obj2)
							{
								CS$<>8__locals2.CS$<>8__locals1.lobbyDetails.Release();
							}
							CS$<>8__locals2.CS$<>8__locals1.lobbyDetails = null;
							obj2 = AntiCheatCommon.LockObject;
							lock (obj2)
							{
								CS$<>8__locals2.lobbySearch.Release();
							}
							CS$<>8__locals2.findDone = true;
							CS$<>8__locals2.findError = true;
							return;
						}
						CS$<>8__locals2.CS$<>8__locals1.lobbyIdFound = lobbyDetailsInfo.Value.LobbyId;
						Log.Out("[EOS-Voice] Found lobby: " + CS$<>8__locals2.CS$<>8__locals1.lobbyIdFound);
						CS$<>8__locals2.findDone = true;
						obj2 = AntiCheatCommon.LockObject;
						lock (obj2)
						{
							CS$<>8__locals2.lobbySearch.Release();
						}
					});
					goto IL_0278;
				}
				goto IL_0261;
				IL_0278:
				if (CS$<>8__locals2.findDone)
				{
					if (CS$<>8__locals2.findError)
					{
						CS$<>8__locals2.CS$<>8__locals1.lobbyDetails = null;
						Log.Error("[EOS-Voice] Failed joining voice lobby");
						this.joinInProgress = false;
						yield break;
					}
					if (CS$<>8__locals2.CS$<>8__locals1.lobbyIdFound == null)
					{
						yield return new WaitForSeconds(0.5f);
						CS$<>8__locals2 = null;
						continue;
					}
					break;
				}
				IL_0261:
				yield return null;
				goto IL_0278;
			}
			if (CS$<>8__locals1.lobbyDetails == null)
			{
				Log.Error("[EOS-Voice] Did not find lobby");
				this.joinInProgress = false;
				yield break;
			}
			Log.Out(string.Format("[EOS-Voice] Found lobby on {0} attempt", attempts));
			JoinLobbyOptions joinLobbyOptions = new JoinLobbyOptions
			{
				LocalUserId = this.localProductUserId,
				PresenceEnabled = false,
				LobbyDetailsHandle = CS$<>8__locals1.lobbyDetails,
				LocalRTCOptions = new LocalRTCOptions?(new LocalRTCOptions
				{
					LocalAudioDeviceInputStartsMuted = true
				})
			};
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.lobbyInterface.JoinLobby(ref joinLobbyOptions, null, delegate(ref JoinLobbyCallbackInfo _callbackData)
				{
					object obj3;
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS-Voice] Join lobby failed: " + _callbackData.ResultCode.ToStringCached<Result>());
						obj3 = AntiCheatCommon.LockObject;
						lock (obj3)
						{
							CS$<>8__locals1.lobbyDetails.Release();
						}
						CS$<>8__locals1.<>4__this.joinInProgress = false;
						return;
					}
					CS$<>8__locals1.<>4__this.lobbyEntered(CS$<>8__locals1.lobbyIdFound);
					obj3 = AntiCheatCommon.LockObject;
					lock (obj3)
					{
						CS$<>8__locals1.lobbyDetails.Release();
					}
				});
				yield break;
			}
			yield break;
		}

		// Token: 0x0600DEF4 RID: 57076 RVA: 0x004F8178 File Offset: 0x004F6378
		[PublicizedFrom(EAccessModifier.Private)]
		public void lobbyEntered(string _lobbyId)
		{
			this.blockedUsers.Clear();
			this.lobbyId = _lobbyId;
			GetRTCRoomNameOptions getRTCRoomNameOptions = new GetRTCRoomNameOptions
			{
				LocalUserId = this.localProductUserId,
				LobbyId = this.lobbyId
			};
			object obj = AntiCheatCommon.LockObject;
			Utf8String utf8String;
			Result rtcroomName;
			lock (obj)
			{
				rtcroomName = this.lobbyInterface.GetRTCRoomName(ref getRTCRoomNameOptions, out utf8String);
			}
			if (rtcroomName != Result.Success)
			{
				Log.Error("[EOS-Voice] Getting local lobby room name failed: " + rtcroomName.ToStringCached<Result>());
			}
			this.roomName = utf8String;
			AddNotifyParticipantStatusChangedOptions addNotifyParticipantStatusChangedOptions = new AddNotifyParticipantStatusChangedOptions
			{
				LocalUserId = this.localProductUserId,
				RoomName = this.roomName
			};
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.participantStatusChangedHandle = this.rtcInterface.AddNotifyParticipantStatusChanged(ref addNotifyParticipantStatusChangedOptions, null, new OnParticipantStatusChangedCallback(this.participantStatusChanged));
			}
			AddNotifyParticipantUpdatedOptions addNotifyParticipantUpdatedOptions = new AddNotifyParticipantUpdatedOptions
			{
				LocalUserId = this.localProductUserId,
				RoomName = this.roomName
			};
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.participantUpdatedHandle = this.audioInterface.AddNotifyParticipantUpdated(ref addNotifyParticipantUpdatedOptions, null, new OnParticipantUpdatedCallback(this.participantVoiceChanged));
			}
			this.SetRoomReceivingVolume(this.outputVolume);
		}

		// Token: 0x0600DEF5 RID: 57077 RVA: 0x004F8324 File Offset: 0x004F6524
		[PublicizedFrom(EAccessModifier.Private)]
		public void participantVoiceChanged(ref ParticipantUpdatedCallbackInfo _data)
		{
			if (Api.DebugLevel == Api.EDebugLevel.Verbose)
			{
				Log.Out(string.Format("[EOS-Voice] Participant update: {0}, speaking={1}, audio={2}", _data.ParticipantId, _data.Speaking, _data.AudioStatus));
			}
			UserIdentifierEos userIdentifierEos = new UserIdentifierEos(_data.ParticipantId);
			if (userIdentifierEos.Equals(this.localUserIdentifier))
			{
				return;
			}
			if (!this.blockedUsers.Contains(userIdentifierEos))
			{
				Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> onRemotePlayerVoiceStateChanged = this.OnRemotePlayerVoiceStateChanged;
				if (onRemotePlayerVoiceStateChanged != null)
				{
					PlatformUserIdentifierAbs platformUserIdentifierAbs = userIdentifierEos;
					IPartyVoice.EVoiceMemberState evoiceMemberState;
					switch (_data.AudioStatus)
					{
					case RTCAudioStatus.Unsupported:
						evoiceMemberState = IPartyVoice.EVoiceMemberState.Disabled;
						break;
					case RTCAudioStatus.Enabled:
						evoiceMemberState = IPartyVoice.EVoiceMemberState.VoiceActive;
						break;
					case RTCAudioStatus.Disabled:
						evoiceMemberState = IPartyVoice.EVoiceMemberState.Normal;
						break;
					case RTCAudioStatus.AdminDisabled:
						evoiceMemberState = IPartyVoice.EVoiceMemberState.Muted;
						break;
					case RTCAudioStatus.NotListeningDisabled:
						evoiceMemberState = IPartyVoice.EVoiceMemberState.Muted;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					onRemotePlayerVoiceStateChanged(platformUserIdentifierAbs, evoiceMemberState);
				}
				return;
			}
			Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> onRemotePlayerVoiceStateChanged2 = this.OnRemotePlayerVoiceStateChanged;
			if (onRemotePlayerVoiceStateChanged2 == null)
			{
				return;
			}
			onRemotePlayerVoiceStateChanged2(userIdentifierEos, IPartyVoice.EVoiceMemberState.Muted);
		}

		// Token: 0x0600DEF6 RID: 57078 RVA: 0x004F83F4 File Offset: 0x004F65F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void participantStatusChanged(ref ParticipantStatusChangedCallbackInfo _data)
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				return;
			}
			if (Api.DebugLevel == Api.EDebugLevel.Verbose)
			{
				Log.Out(string.Format("[EOS-Voice] Participant state changed: {0}, {1}", _data.ParticipantId, _data.ParticipantStatus));
			}
			UserIdentifierEos userIdentifierEos = new UserIdentifierEos(_data.ParticipantId);
			if (userIdentifierEos.Equals(this.localUserIdentifier))
			{
				this.roomEntered = _data.ParticipantStatus == RTCParticipantStatus.Joined;
				if (this.roomEntered)
				{
					this.createInProgress = false;
					this.joinInProgress = false;
					this.muteSelf = true;
					this.muteOthers = false;
				}
				Action<IPartyVoice.EVoiceChannelAction> onLocalPlayerStateChanged = this.OnLocalPlayerStateChanged;
				if (onLocalPlayerStateChanged == null)
				{
					return;
				}
				onLocalPlayerStateChanged((_data.ParticipantStatus == RTCParticipantStatus.Joined) ? IPartyVoice.EVoiceChannelAction.Joined : IPartyVoice.EVoiceChannelAction.Left);
				return;
			}
			else
			{
				Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> onRemotePlayerStateChanged = this.OnRemotePlayerStateChanged;
				if (onRemotePlayerStateChanged == null)
				{
					return;
				}
				onRemotePlayerStateChanged(userIdentifierEos, (_data.ParticipantStatus == RTCParticipantStatus.Joined) ? IPartyVoice.EVoiceChannelAction.Joined : IPartyVoice.EVoiceChannelAction.Left);
				return;
			}
		}

		// Token: 0x0600DEF7 RID: 57079 RVA: 0x004F84BC File Offset: 0x004F66BC
		[PublicizedFrom(EAccessModifier.Private)]
		public void lobbyLeft()
		{
			if (this.Status != EPartyVoiceStatus.Ok)
			{
				return;
			}
			this.lobbyId = null;
			this.roomName = null;
			this.muteSelf = true;
			this.muteOthers = false;
			this.blockedUsers.Clear();
			if (this.participantStatusChangedHandle != 0UL)
			{
				object obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					this.rtcInterface.RemoveNotifyParticipantStatusChanged(this.participantStatusChangedHandle);
				}
			}
			if (this.participantUpdatedHandle != 0UL)
			{
				object obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					this.audioInterface.RemoveNotifyParticipantUpdated(this.participantUpdatedHandle);
				}
			}
			this.participantStatusChangedHandle = 0UL;
			this.participantUpdatedHandle = 0UL;
		}

		// Token: 0x0600DEFB RID: 57083 RVA: 0x004F866A File Offset: 0x004F686A
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <AddAudioDevicesNotifications>g__OnAudioDevicesChanged|60_0(ref AudioDevicesChangedCallbackInfo data)
		{
			this.RefreshAudioDevices();
		}

		// Token: 0x0600DEFC RID: 57084 RVA: 0x004F8672 File Offset: 0x004F6872
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <SetRoomReceivingVolume>g__OnUpdateReceivingVolume|80_0(ref UpdateReceivingVolumeCallbackInfo data)
		{
			if (data.ResultCode != Result.Success)
			{
				Log.Error(string.Format("[EOS-Voice] Setting voice output volume for room '{0}' failed: {1}", data.RoomName, data.ResultCode.ToStringCached<Result>()));
			}
		}

		// Token: 0x0400A7FE RID: 43006
		[PublicizedFrom(EAccessModifier.Private)]
		public const int voiceLobbyConnectAttempts = 5;

		// Token: 0x0400A7FF RID: 43007
		[PublicizedFrom(EAccessModifier.Private)]
		public const float voiceLobbyConnectAttemptInterval = 0.5f;

		// Token: 0x0400A800 RID: 43008
		[PublicizedFrom(EAccessModifier.Private)]
		public const float platformVolumeToEosRtcVolume = 50f;

		// Token: 0x0400A801 RID: 43009
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A802 RID: 43010
		[PublicizedFrom(EAccessModifier.Private)]
		public Api api;

		// Token: 0x0400A803 RID: 43011
		[PublicizedFrom(EAccessModifier.Private)]
		public RTCInterface rtcInterface;

		// Token: 0x0400A804 RID: 43012
		[PublicizedFrom(EAccessModifier.Private)]
		public RTCAudioInterface audioInterface;

		// Token: 0x0400A805 RID: 43013
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyInterface lobbyInterface;

		// Token: 0x0400A806 RID: 43014
		[PublicizedFrom(EAccessModifier.Private)]
		public string lobbyId;

		// Token: 0x0400A807 RID: 43015
		[PublicizedFrom(EAccessModifier.Private)]
		public bool createInProgress;

		// Token: 0x0400A808 RID: 43016
		[PublicizedFrom(EAccessModifier.Private)]
		public bool joinInProgress;

		// Token: 0x0400A809 RID: 43017
		[PublicizedFrom(EAccessModifier.Private)]
		public bool leaveInProgress;

		// Token: 0x0400A80A RID: 43018
		[PublicizedFrom(EAccessModifier.Private)]
		public string roomName;

		// Token: 0x0400A80B RID: 43019
		[PublicizedFrom(EAccessModifier.Private)]
		public bool roomEntered;

		// Token: 0x0400A80C RID: 43020
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong participantStatusChangedHandle;

		// Token: 0x0400A80D RID: 43021
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong participantUpdatedHandle;

		// Token: 0x0400A80E RID: 43022
		[PublicizedFrom(EAccessModifier.Private)]
		public Action initializedDelegates;

		// Token: 0x0400A80F RID: 43023
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object initializedDelegateLock = new object();

		// Token: 0x0400A814 RID: 43028
		[PublicizedFrom(EAccessModifier.Private)]
		public string activeInputDeviceId;

		// Token: 0x0400A815 RID: 43029
		[PublicizedFrom(EAccessModifier.Private)]
		public string activeOutputDeviceId;

		// Token: 0x0400A816 RID: 43030
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong audioDevicesChangedNotificationId;

		// Token: 0x0400A817 RID: 43031
		[PublicizedFrom(EAccessModifier.Private)]
		public IList<IPartyVoice.VoiceAudioDevice> inputDevices;

		// Token: 0x0400A818 RID: 43032
		[PublicizedFrom(EAccessModifier.Private)]
		public IList<IPartyVoice.VoiceAudioDevice> outputDevices;

		// Token: 0x0400A819 RID: 43033
		[PublicizedFrom(EAccessModifier.Private)]
		public bool muteSelf;

		// Token: 0x0400A81A RID: 43034
		[PublicizedFrom(EAccessModifier.Private)]
		public bool muteOthers;

		// Token: 0x0400A81B RID: 43035
		[PublicizedFrom(EAccessModifier.Private)]
		public float outputVolume = 1f;

		// Token: 0x0400A81C RID: 43036
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<PlatformUserIdentifierAbs> blockedUsers = new HashSet<PlatformUserIdentifierAbs>();

		// Token: 0x02001D62 RID: 7522
		public class EosAudioDevice : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600DEFD RID: 57085 RVA: 0x004F869C File Offset: 0x004F689C
			public EosAudioDevice(InputDeviceInformation _device)
				: base(false, _device.DefaultDevice)
			{
				this.Id = _device.DeviceId;
				this.Name = _device.DeviceName;
				this.LogDevice("Input");
			}

			// Token: 0x0600DEFE RID: 57086 RVA: 0x004F86DB File Offset: 0x004F68DB
			public EosAudioDevice(OutputDeviceInformation _device)
				: base(true, _device.DefaultDevice)
			{
				this.Id = _device.DeviceId;
				this.Name = _device.DeviceName;
				this.LogDevice("Output");
			}

			// Token: 0x0600DEFF RID: 57087 RVA: 0x004F871C File Offset: 0x004F691C
			[PublicizedFrom(EAccessModifier.Private)]
			public void LogDevice(string _inOutString)
			{
				if (GameUtils.GetLaunchArgument("debugeos") != null)
				{
					Log.Out(string.Format("[EOS-Voice] {0} device: Id={1}, Name={2}, Default={3}", new object[] { _inOutString, this.Id, this.Name, this.IsDefault }));
				}
			}

			// Token: 0x0600DF00 RID: 57088 RVA: 0x004F876E File Offset: 0x004F696E
			public override string ToString()
			{
				if (!this.IsDefault)
				{
					return this.Name;
				}
				return "(Default) " + this.Name;
			}

			// Token: 0x17001C4A RID: 7242
			// (get) Token: 0x0600DF01 RID: 57089 RVA: 0x004F878F File Offset: 0x004F698F
			public override string Identifier
			{
				get
				{
					return this.Id;
				}
			}

			// Token: 0x0400A81D RID: 43037
			public readonly string Id;

			// Token: 0x0400A81E RID: 43038
			public readonly string Name;
		}
	}
}
