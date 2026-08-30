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
	// Token: 0x02001D07 RID: 7431
	public class Voice : IPartyVoice
	{
		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x0600DBAD RID: 56237 RVA: 0x004EB817 File Offset: 0x004E9A17
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

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x0600DBAE RID: 56238 RVA: 0x004EB83C File Offset: 0x004E9A3C
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
		// (add) Token: 0x0600DBAF RID: 56239 RVA: 0x004EB850 File Offset: 0x004E9A50
		// (remove) Token: 0x0600DBB0 RID: 56240 RVA: 0x004EB8B0 File Offset: 0x004E9AB0
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

		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x0600DBB1 RID: 56241 RVA: 0x004EB904 File Offset: 0x004E9B04
		// (set) Token: 0x0600DBB2 RID: 56242 RVA: 0x004EB90C File Offset: 0x004E9B0C
		public EPartyVoiceStatus Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EPartyVoiceStatus.Uninitialized;

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x0600DBB3 RID: 56243 RVA: 0x004EB915 File Offset: 0x004E9B15
		public bool InLobby
		{
			get
			{
				return this.lobbyId != null && this.roomEntered;
			}
		}

		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x0600DBB4 RID: 56244 RVA: 0x004EB927 File Offset: 0x004E9B27
		public bool InLobbyOrProgress
		{
			get
			{
				return this.InLobby || this.createInProgress || this.joinInProgress;
			}
		}

		// Token: 0x0600DBB5 RID: 56245 RVA: 0x004EB941 File Offset: 0x004E9B41
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.api = (Api)this.owner.Api;
			this.api.ClientApiInitialized += this.OnClientApiInitialized;
		}

		// Token: 0x0600DBB6 RID: 56246 RVA: 0x004EB978 File Offset: 0x004E9B78
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

		// Token: 0x0600DBB7 RID: 56247 RVA: 0x004EB9D4 File Offset: 0x004E9BD4
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

		// Token: 0x0600DBB8 RID: 56248 RVA: 0x004EBAE4 File Offset: 0x004E9CE4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPartyVoiceInitialized()
		{
			this.AddNotifications();
		}

		// Token: 0x0600DBB9 RID: 56249 RVA: 0x004EBAEC File Offset: 0x004E9CEC
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPartyVoiceUninitialize()
		{
			this.RemoveNotifications();
		}

		// Token: 0x0600DBBA RID: 56250 RVA: 0x004EBAF4 File Offset: 0x004E9CF4
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

		// Token: 0x0600DBBB RID: 56251 RVA: 0x004EBB0F File Offset: 0x004E9D0F
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

		// Token: 0x0600DBBC RID: 56252 RVA: 0x004EBB2C File Offset: 0x004E9D2C
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

		// Token: 0x0600DBBD RID: 56253 RVA: 0x004EBC3C File Offset: 0x004E9E3C
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

		// Token: 0x0600DBBE RID: 56254 RVA: 0x004EBCB0 File Offset: 0x004E9EB0
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

		// Token: 0x0600DBBF RID: 56255 RVA: 0x004EBD6C File Offset: 0x004E9F6C
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

		// Token: 0x0600DBC0 RID: 56256 RVA: 0x004EBE58 File Offset: 0x004EA058
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
		// (add) Token: 0x0600DBC1 RID: 56257 RVA: 0x004EBF64 File Offset: 0x004EA164
		// (remove) Token: 0x0600DBC2 RID: 56258 RVA: 0x004EBF9C File Offset: 0x004EA19C
		public event Action<IPartyVoice.EVoiceChannelAction> OnLocalPlayerStateChanged;

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x0600DBC3 RID: 56259 RVA: 0x004EBFD4 File Offset: 0x004EA1D4
		// (remove) Token: 0x0600DBC4 RID: 56260 RVA: 0x004EC00C File Offset: 0x004EA20C
		public event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> OnRemotePlayerStateChanged;

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x0600DBC5 RID: 56261 RVA: 0x004EC044 File Offset: 0x004EA244
		// (remove) Token: 0x0600DBC6 RID: 56262 RVA: 0x004EC07C File Offset: 0x004EA27C
		public event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> OnRemotePlayerVoiceStateChanged;

		// Token: 0x0600DBC7 RID: 56263 RVA: 0x004EC0B4 File Offset: 0x004EA2B4
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

		// Token: 0x0600DBC8 RID: 56264 RVA: 0x004EC124 File Offset: 0x004EA324
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

		// Token: 0x0600DBC9 RID: 56265 RVA: 0x004EC184 File Offset: 0x004EA384
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

		// Token: 0x0600DBCA RID: 56266 RVA: 0x004EC23C File Offset: 0x004EA43C
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

		// Token: 0x0600DBCB RID: 56267 RVA: 0x004EC39C File Offset: 0x004EA59C
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

		// Token: 0x0600DBCC RID: 56268 RVA: 0x004EC4FC File Offset: 0x004EA6FC
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

		// Token: 0x0600DBCD RID: 56269 RVA: 0x004EC53C File Offset: 0x004EA73C
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

		// Token: 0x0600DBCE RID: 56270 RVA: 0x004EC5F4 File Offset: 0x004EA7F4
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

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x0600DBCF RID: 56271 RVA: 0x004EC6A4 File Offset: 0x004EA8A4
		// (set) Token: 0x0600DBD0 RID: 56272 RVA: 0x004EC6AC File Offset: 0x004EA8AC
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

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x0600DBD1 RID: 56273 RVA: 0x004EC780 File Offset: 0x004EA980
		// (set) Token: 0x0600DBD2 RID: 56274 RVA: 0x004EC788 File Offset: 0x004EA988
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

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x0600DBD3 RID: 56275 RVA: 0x004EC864 File Offset: 0x004EAA64
		// (set) Token: 0x0600DBD4 RID: 56276 RVA: 0x004EC86C File Offset: 0x004EAA6C
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

		// Token: 0x0600DBD5 RID: 56277 RVA: 0x004EC8CC File Offset: 0x004EAACC
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

		// Token: 0x0600DBD6 RID: 56278 RVA: 0x004EC97C File Offset: 0x004EAB7C
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

		// Token: 0x0600DBD7 RID: 56279 RVA: 0x004ECAA4 File Offset: 0x004EACA4
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

		// Token: 0x0600DBD8 RID: 56280 RVA: 0x004ECABC File Offset: 0x004EACBC
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

		// Token: 0x0600DBD9 RID: 56281 RVA: 0x004ECC68 File Offset: 0x004EAE68
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

		// Token: 0x0600DBDA RID: 56282 RVA: 0x004ECD38 File Offset: 0x004EAF38
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

		// Token: 0x0600DBDB RID: 56283 RVA: 0x004ECE00 File Offset: 0x004EB000
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

		// Token: 0x0600DBDF RID: 56287 RVA: 0x004ECFAE File Offset: 0x004EB1AE
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <AddAudioDevicesNotifications>g__OnAudioDevicesChanged|60_0(ref AudioDevicesChangedCallbackInfo data)
		{
			this.RefreshAudioDevices();
		}

		// Token: 0x0600DBE0 RID: 56288 RVA: 0x004ECFB6 File Offset: 0x004EB1B6
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <SetRoomReceivingVolume>g__OnUpdateReceivingVolume|80_0(ref UpdateReceivingVolumeCallbackInfo data)
		{
			if (data.ResultCode != Result.Success)
			{
				Log.Error(string.Format("[EOS-Voice] Setting voice output volume for room '{0}' failed: {1}", data.RoomName, data.ResultCode.ToStringCached<Result>()));
			}
		}

		// Token: 0x0400A5F2 RID: 42482
		[PublicizedFrom(EAccessModifier.Private)]
		public const int voiceLobbyConnectAttempts = 5;

		// Token: 0x0400A5F3 RID: 42483
		[PublicizedFrom(EAccessModifier.Private)]
		public const float voiceLobbyConnectAttemptInterval = 0.5f;

		// Token: 0x0400A5F4 RID: 42484
		[PublicizedFrom(EAccessModifier.Private)]
		public const float platformVolumeToEosRtcVolume = 50f;

		// Token: 0x0400A5F5 RID: 42485
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5F6 RID: 42486
		[PublicizedFrom(EAccessModifier.Private)]
		public Api api;

		// Token: 0x0400A5F7 RID: 42487
		[PublicizedFrom(EAccessModifier.Private)]
		public RTCInterface rtcInterface;

		// Token: 0x0400A5F8 RID: 42488
		[PublicizedFrom(EAccessModifier.Private)]
		public RTCAudioInterface audioInterface;

		// Token: 0x0400A5F9 RID: 42489
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyInterface lobbyInterface;

		// Token: 0x0400A5FA RID: 42490
		[PublicizedFrom(EAccessModifier.Private)]
		public string lobbyId;

		// Token: 0x0400A5FB RID: 42491
		[PublicizedFrom(EAccessModifier.Private)]
		public bool createInProgress;

		// Token: 0x0400A5FC RID: 42492
		[PublicizedFrom(EAccessModifier.Private)]
		public bool joinInProgress;

		// Token: 0x0400A5FD RID: 42493
		[PublicizedFrom(EAccessModifier.Private)]
		public bool leaveInProgress;

		// Token: 0x0400A5FE RID: 42494
		[PublicizedFrom(EAccessModifier.Private)]
		public string roomName;

		// Token: 0x0400A5FF RID: 42495
		[PublicizedFrom(EAccessModifier.Private)]
		public bool roomEntered;

		// Token: 0x0400A600 RID: 42496
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong participantStatusChangedHandle;

		// Token: 0x0400A601 RID: 42497
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong participantUpdatedHandle;

		// Token: 0x0400A602 RID: 42498
		[PublicizedFrom(EAccessModifier.Private)]
		public Action initializedDelegates;

		// Token: 0x0400A603 RID: 42499
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object initializedDelegateLock = new object();

		// Token: 0x0400A608 RID: 42504
		[PublicizedFrom(EAccessModifier.Private)]
		public string activeInputDeviceId;

		// Token: 0x0400A609 RID: 42505
		[PublicizedFrom(EAccessModifier.Private)]
		public string activeOutputDeviceId;

		// Token: 0x0400A60A RID: 42506
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong audioDevicesChangedNotificationId;

		// Token: 0x0400A60B RID: 42507
		[PublicizedFrom(EAccessModifier.Private)]
		public IList<IPartyVoice.VoiceAudioDevice> inputDevices;

		// Token: 0x0400A60C RID: 42508
		[PublicizedFrom(EAccessModifier.Private)]
		public IList<IPartyVoice.VoiceAudioDevice> outputDevices;

		// Token: 0x0400A60D RID: 42509
		[PublicizedFrom(EAccessModifier.Private)]
		public bool muteSelf;

		// Token: 0x0400A60E RID: 42510
		[PublicizedFrom(EAccessModifier.Private)]
		public bool muteOthers;

		// Token: 0x0400A60F RID: 42511
		[PublicizedFrom(EAccessModifier.Private)]
		public float outputVolume = 1f;

		// Token: 0x0400A610 RID: 42512
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<PlatformUserIdentifierAbs> blockedUsers = new HashSet<PlatformUserIdentifierAbs>();

		// Token: 0x02001D08 RID: 7432
		public class EosAudioDevice : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600DBE1 RID: 56289 RVA: 0x004ECFE0 File Offset: 0x004EB1E0
			public EosAudioDevice(InputDeviceInformation _device)
				: base(false, _device.DefaultDevice)
			{
				this.Id = _device.DeviceId;
				this.Name = _device.DeviceName;
				this.LogDevice("Input");
			}

			// Token: 0x0600DBE2 RID: 56290 RVA: 0x004ED01F File Offset: 0x004EB21F
			public EosAudioDevice(OutputDeviceInformation _device)
				: base(true, _device.DefaultDevice)
			{
				this.Id = _device.DeviceId;
				this.Name = _device.DeviceName;
				this.LogDevice("Output");
			}

			// Token: 0x0600DBE3 RID: 56291 RVA: 0x004ED060 File Offset: 0x004EB260
			[PublicizedFrom(EAccessModifier.Private)]
			public void LogDevice(string _inOutString)
			{
				if (GameUtils.GetLaunchArgument("debugeos") != null)
				{
					Log.Out(string.Format("[EOS-Voice] {0} device: Id={1}, Name={2}, Default={3}", new object[] { _inOutString, this.Id, this.Name, this.IsDefault }));
				}
			}

			// Token: 0x0600DBE4 RID: 56292 RVA: 0x004ED0B2 File Offset: 0x004EB2B2
			public override string ToString()
			{
				if (!this.IsDefault)
				{
					return this.Name;
				}
				return "(Default) " + this.Name;
			}

			// Token: 0x17001B32 RID: 6962
			// (get) Token: 0x0600DBE5 RID: 56293 RVA: 0x004ED0D3 File Offset: 0x004EB2D3
			public override string Identifier
			{
				get
				{
					return this.Id;
				}
			}

			// Token: 0x0400A611 RID: 42513
			public readonly string Id;

			// Token: 0x0400A612 RID: 42514
			public readonly string Name;
		}
	}
}
