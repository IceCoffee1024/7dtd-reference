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
	// Token: 0x02001D92 RID: 7570
	public class Voice : IPartyVoice
	{
		// Token: 0x17001C8A RID: 7306
		// (get) Token: 0x0600DFDD RID: 57309 RVA: 0x004FAA73 File Offset: 0x004F8C73
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

		// Token: 0x17001C8B RID: 7307
		// (get) Token: 0x0600DFDE RID: 57310 RVA: 0x004FAA98 File Offset: 0x004F8C98
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
		// (add) Token: 0x0600DFDF RID: 57311 RVA: 0x004FAAAC File Offset: 0x004F8CAC
		// (remove) Token: 0x0600DFE0 RID: 57312 RVA: 0x004FAB0C File Offset: 0x004F8D0C
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

		// Token: 0x17001C8C RID: 7308
		// (get) Token: 0x0600DFE1 RID: 57313 RVA: 0x004FAB60 File Offset: 0x004F8D60
		// (set) Token: 0x0600DFE2 RID: 57314 RVA: 0x004FAB68 File Offset: 0x004F8D68
		public EPartyVoiceStatus Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EPartyVoiceStatus.Uninitialized;

		// Token: 0x17001C8D RID: 7309
		// (get) Token: 0x0600DFE3 RID: 57315 RVA: 0x004FAB71 File Offset: 0x004F8D71
		public bool InLobby
		{
			get
			{
				return this.lobbyId != null && this.roomEntered;
			}
		}

		// Token: 0x17001C8E RID: 7310
		// (get) Token: 0x0600DFE4 RID: 57316 RVA: 0x004FAB83 File Offset: 0x004F8D83
		public bool InLobbyOrProgress
		{
			get
			{
				return this.InLobby || this.createInProgress || this.joinInProgress;
			}
		}

		// Token: 0x0600DFE5 RID: 57317 RVA: 0x004FAB9D File Offset: 0x004F8D9D
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.api = (Api)this.owner.Api;
			this.api.ClientApiInitialized += this.OnClientApiInitialized;
		}

		// Token: 0x0600DFE6 RID: 57318 RVA: 0x004FABD4 File Offset: 0x004F8DD4
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

		// Token: 0x0600DFE7 RID: 57319 RVA: 0x004FAC30 File Offset: 0x004F8E30
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

		// Token: 0x0600DFE8 RID: 57320 RVA: 0x004FAD40 File Offset: 0x004F8F40
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPartyVoiceInitialized()
		{
			this.AddNotifications();
		}

		// Token: 0x0600DFE9 RID: 57321 RVA: 0x004FAD48 File Offset: 0x004F8F48
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPartyVoiceUninitialize()
		{
			this.RemoveNotifications();
		}

		// Token: 0x0600DFEA RID: 57322 RVA: 0x004FAD50 File Offset: 0x004F8F50
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

		// Token: 0x0600DFEB RID: 57323 RVA: 0x004FAD6B File Offset: 0x004F8F6B
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

		// Token: 0x0600DFEC RID: 57324 RVA: 0x004FAD88 File Offset: 0x004F8F88
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

		// Token: 0x0600DFED RID: 57325 RVA: 0x004FAE98 File Offset: 0x004F9098
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

		// Token: 0x0600DFEE RID: 57326 RVA: 0x004FAF0C File Offset: 0x004F910C
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

		// Token: 0x0600DFEF RID: 57327 RVA: 0x004FAFC8 File Offset: 0x004F91C8
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

		// Token: 0x0600DFF0 RID: 57328 RVA: 0x004FB0B4 File Offset: 0x004F92B4
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
		// (add) Token: 0x0600DFF1 RID: 57329 RVA: 0x004FB1C0 File Offset: 0x004F93C0
		// (remove) Token: 0x0600DFF2 RID: 57330 RVA: 0x004FB1F8 File Offset: 0x004F93F8
		public event Action<IPartyVoice.EVoiceChannelAction> OnLocalPlayerStateChanged;

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x0600DFF3 RID: 57331 RVA: 0x004FB230 File Offset: 0x004F9430
		// (remove) Token: 0x0600DFF4 RID: 57332 RVA: 0x004FB268 File Offset: 0x004F9468
		public event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> OnRemotePlayerStateChanged;

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x0600DFF5 RID: 57333 RVA: 0x004FB2A0 File Offset: 0x004F94A0
		// (remove) Token: 0x0600DFF6 RID: 57334 RVA: 0x004FB2D8 File Offset: 0x004F94D8
		public event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> OnRemotePlayerVoiceStateChanged;

		// Token: 0x0600DFF7 RID: 57335 RVA: 0x004FB310 File Offset: 0x004F9510
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

		// Token: 0x0600DFF8 RID: 57336 RVA: 0x004FB380 File Offset: 0x004F9580
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

		// Token: 0x0600DFF9 RID: 57337 RVA: 0x004FB3E0 File Offset: 0x004F95E0
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

		// Token: 0x0600DFFA RID: 57338 RVA: 0x004FB498 File Offset: 0x004F9698
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

		// Token: 0x0600DFFB RID: 57339 RVA: 0x004FB5F8 File Offset: 0x004F97F8
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

		// Token: 0x0600DFFC RID: 57340 RVA: 0x004FB758 File Offset: 0x004F9958
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

		// Token: 0x0600DFFD RID: 57341 RVA: 0x004FB798 File Offset: 0x004F9998
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

		// Token: 0x0600DFFE RID: 57342 RVA: 0x004FB850 File Offset: 0x004F9A50
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

		// Token: 0x17001C8F RID: 7311
		// (get) Token: 0x0600DFFF RID: 57343 RVA: 0x004FB900 File Offset: 0x004F9B00
		// (set) Token: 0x0600E000 RID: 57344 RVA: 0x004FB908 File Offset: 0x004F9B08
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

		// Token: 0x17001C90 RID: 7312
		// (get) Token: 0x0600E001 RID: 57345 RVA: 0x004FB9DC File Offset: 0x004F9BDC
		// (set) Token: 0x0600E002 RID: 57346 RVA: 0x004FB9E4 File Offset: 0x004F9BE4
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

		// Token: 0x17001C91 RID: 7313
		// (get) Token: 0x0600E003 RID: 57347 RVA: 0x004FBAC0 File Offset: 0x004F9CC0
		// (set) Token: 0x0600E004 RID: 57348 RVA: 0x004FBAC8 File Offset: 0x004F9CC8
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

		// Token: 0x0600E005 RID: 57349 RVA: 0x004FBB28 File Offset: 0x004F9D28
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

		// Token: 0x0600E006 RID: 57350 RVA: 0x004FBBD8 File Offset: 0x004F9DD8
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

		// Token: 0x0600E007 RID: 57351 RVA: 0x004FBD00 File Offset: 0x004F9F00
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

		// Token: 0x0600E008 RID: 57352 RVA: 0x004FBD18 File Offset: 0x004F9F18
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

		// Token: 0x0600E009 RID: 57353 RVA: 0x004FBEC4 File Offset: 0x004FA0C4
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

		// Token: 0x0600E00A RID: 57354 RVA: 0x004FBF94 File Offset: 0x004FA194
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

		// Token: 0x0600E00B RID: 57355 RVA: 0x004FC05C File Offset: 0x004FA25C
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

		// Token: 0x0600E00F RID: 57359 RVA: 0x004FC20A File Offset: 0x004FA40A
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <AddAudioDevicesNotifications>g__OnAudioDevicesChanged|60_0(ref AudioDevicesChangedCallbackInfo data)
		{
			this.RefreshAudioDevices();
		}

		// Token: 0x0600E010 RID: 57360 RVA: 0x004FC212 File Offset: 0x004FA412
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <SetRoomReceivingVolume>g__OnUpdateReceivingVolume|80_0(ref UpdateReceivingVolumeCallbackInfo data)
		{
			if (data.ResultCode != Result.Success)
			{
				Log.Error(string.Format("[EOS-Voice] Setting voice output volume for room '{0}' failed: {1}", data.RoomName, data.ResultCode.ToStringCached<Result>()));
			}
		}

		// Token: 0x0400A8E7 RID: 43239
		[PublicizedFrom(EAccessModifier.Private)]
		public const int voiceLobbyConnectAttempts = 5;

		// Token: 0x0400A8E8 RID: 43240
		[PublicizedFrom(EAccessModifier.Private)]
		public const float voiceLobbyConnectAttemptInterval = 0.5f;

		// Token: 0x0400A8E9 RID: 43241
		[PublicizedFrom(EAccessModifier.Private)]
		public const float platformVolumeToEosRtcVolume = 50f;

		// Token: 0x0400A8EA RID: 43242
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A8EB RID: 43243
		[PublicizedFrom(EAccessModifier.Private)]
		public Api api;

		// Token: 0x0400A8EC RID: 43244
		[PublicizedFrom(EAccessModifier.Private)]
		public RTCInterface rtcInterface;

		// Token: 0x0400A8ED RID: 43245
		[PublicizedFrom(EAccessModifier.Private)]
		public RTCAudioInterface audioInterface;

		// Token: 0x0400A8EE RID: 43246
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyInterface lobbyInterface;

		// Token: 0x0400A8EF RID: 43247
		[PublicizedFrom(EAccessModifier.Private)]
		public string lobbyId;

		// Token: 0x0400A8F0 RID: 43248
		[PublicizedFrom(EAccessModifier.Private)]
		public bool createInProgress;

		// Token: 0x0400A8F1 RID: 43249
		[PublicizedFrom(EAccessModifier.Private)]
		public bool joinInProgress;

		// Token: 0x0400A8F2 RID: 43250
		[PublicizedFrom(EAccessModifier.Private)]
		public bool leaveInProgress;

		// Token: 0x0400A8F3 RID: 43251
		[PublicizedFrom(EAccessModifier.Private)]
		public string roomName;

		// Token: 0x0400A8F4 RID: 43252
		[PublicizedFrom(EAccessModifier.Private)]
		public bool roomEntered;

		// Token: 0x0400A8F5 RID: 43253
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong participantStatusChangedHandle;

		// Token: 0x0400A8F6 RID: 43254
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong participantUpdatedHandle;

		// Token: 0x0400A8F7 RID: 43255
		[PublicizedFrom(EAccessModifier.Private)]
		public Action initializedDelegates;

		// Token: 0x0400A8F8 RID: 43256
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object initializedDelegateLock = new object();

		// Token: 0x0400A8FD RID: 43261
		[PublicizedFrom(EAccessModifier.Private)]
		public string activeInputDeviceId;

		// Token: 0x0400A8FE RID: 43262
		[PublicizedFrom(EAccessModifier.Private)]
		public string activeOutputDeviceId;

		// Token: 0x0400A8FF RID: 43263
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong audioDevicesChangedNotificationId;

		// Token: 0x0400A900 RID: 43264
		[PublicizedFrom(EAccessModifier.Private)]
		public IList<IPartyVoice.VoiceAudioDevice> inputDevices;

		// Token: 0x0400A901 RID: 43265
		[PublicizedFrom(EAccessModifier.Private)]
		public IList<IPartyVoice.VoiceAudioDevice> outputDevices;

		// Token: 0x0400A902 RID: 43266
		[PublicizedFrom(EAccessModifier.Private)]
		public bool muteSelf;

		// Token: 0x0400A903 RID: 43267
		[PublicizedFrom(EAccessModifier.Private)]
		public bool muteOthers;

		// Token: 0x0400A904 RID: 43268
		[PublicizedFrom(EAccessModifier.Private)]
		public float outputVolume = 1f;

		// Token: 0x0400A905 RID: 43269
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<PlatformUserIdentifierAbs> blockedUsers = new HashSet<PlatformUserIdentifierAbs>();

		// Token: 0x02001D93 RID: 7571
		public class EosAudioDevice : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600E011 RID: 57361 RVA: 0x004FC23C File Offset: 0x004FA43C
			public EosAudioDevice(InputDeviceInformation _device)
				: base(false, _device.DefaultDevice)
			{
				this.Id = _device.DeviceId;
				this.Name = _device.DeviceName;
				this.LogDevice("Input");
			}

			// Token: 0x0600E012 RID: 57362 RVA: 0x004FC27B File Offset: 0x004FA47B
			public EosAudioDevice(OutputDeviceInformation _device)
				: base(true, _device.DefaultDevice)
			{
				this.Id = _device.DeviceId;
				this.Name = _device.DeviceName;
				this.LogDevice("Output");
			}

			// Token: 0x0600E013 RID: 57363 RVA: 0x004FC2BC File Offset: 0x004FA4BC
			[PublicizedFrom(EAccessModifier.Private)]
			public void LogDevice(string _inOutString)
			{
				if (GameUtils.GetLaunchArgument("debugeos") != null)
				{
					Log.Out(string.Format("[EOS-Voice] {0} device: Id={1}, Name={2}, Default={3}", new object[] { _inOutString, this.Id, this.Name, this.IsDefault }));
				}
			}

			// Token: 0x0600E014 RID: 57364 RVA: 0x004FC30E File Offset: 0x004FA50E
			public override string ToString()
			{
				if (!this.IsDefault)
				{
					return this.Name;
				}
				return "(Default) " + this.Name;
			}

			// Token: 0x17001C92 RID: 7314
			// (get) Token: 0x0600E015 RID: 57365 RVA: 0x004FC32F File Offset: 0x004FA52F
			public override string Identifier
			{
				get
				{
					return this.Id;
				}
			}

			// Token: 0x0400A906 RID: 43270
			public readonly string Id;

			// Token: 0x0400A907 RID: 43271
			public readonly string Name;
		}
	}
}
