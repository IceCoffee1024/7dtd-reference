using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B45 RID: 6981
	public interface IPartyVoice
	{
		// Token: 0x1400011D RID: 285
		// (add) Token: 0x0600D08C RID: 53388
		// (remove) Token: 0x0600D08D RID: 53389
		event Action Initialized;

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x0600D08E RID: 53390
		EPartyVoiceStatus Status { get; }

		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x0600D08F RID: 53391
		bool InLobby { get; }

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x0600D090 RID: 53392
		bool InLobbyOrProgress { get; }

		// Token: 0x0600D091 RID: 53393
		void Init(IPlatform _owner);

		// Token: 0x0600D092 RID: 53394
		void Destroy();

		// Token: 0x0600D093 RID: 53395
		void CreateLobby(Action<string> _lobbyCreatedCallback);

		// Token: 0x0600D094 RID: 53396
		void JoinLobby(string _lobbyId);

		// Token: 0x0600D095 RID: 53397
		void LeaveLobby();

		// Token: 0x0600D096 RID: 53398
		void PromoteLeader(PlatformUserIdentifierAbs _newLeaderIdentifier);

		// Token: 0x0600D097 RID: 53399
		bool IsLobbyOwner();

		// Token: 0x1400011E RID: 286
		// (add) Token: 0x0600D098 RID: 53400
		// (remove) Token: 0x0600D099 RID: 53401
		event Action<IPartyVoice.EVoiceChannelAction> OnLocalPlayerStateChanged;

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x0600D09A RID: 53402
		// (remove) Token: 0x0600D09B RID: 53403
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> OnRemotePlayerStateChanged;

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x0600D09C RID: 53404
		// (remove) Token: 0x0600D09D RID: 53405
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> OnRemotePlayerVoiceStateChanged;

		// Token: 0x0600D09E RID: 53406
		[return: TupleElementNames(new string[] { "inputDevices", "outputDevices" })]
		ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>> GetDevicesList();

		// Token: 0x0600D09F RID: 53407
		void SetInputDevice(string _device);

		// Token: 0x0600D0A0 RID: 53408
		void SetOutputDevice(string _device);

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x0600D0A1 RID: 53409
		// (set) Token: 0x0600D0A2 RID: 53410
		bool MuteSelf { get; set; }

		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x0600D0A3 RID: 53411
		// (set) Token: 0x0600D0A4 RID: 53412
		bool MuteOthers { get; set; }

		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x0600D0A5 RID: 53413
		// (set) Token: 0x0600D0A6 RID: 53414
		float OutputVolume { get; set; }

		// Token: 0x0600D0A7 RID: 53415
		void BlockUser(PlatformUserIdentifierAbs _userIdentifier, bool _block);

		// Token: 0x02001B46 RID: 6982
		public enum EVoiceMemberState
		{
			// Token: 0x04009F63 RID: 40803
			Disabled,
			// Token: 0x04009F64 RID: 40804
			Normal,
			// Token: 0x04009F65 RID: 40805
			Muted,
			// Token: 0x04009F66 RID: 40806
			VoiceActive
		}

		// Token: 0x02001B47 RID: 6983
		public enum EVoiceChannelAction
		{
			// Token: 0x04009F68 RID: 40808
			Joined,
			// Token: 0x04009F69 RID: 40809
			Left
		}

		// Token: 0x02001B48 RID: 6984
		public abstract class VoiceAudioDevice
		{
			// Token: 0x0600D0A8 RID: 53416 RVA: 0x004C0395 File Offset: 0x004BE595
			public VoiceAudioDevice(bool _isOutput, bool _isDefault)
			{
				this.IsOutput = _isOutput;
				this.IsDefault = _isDefault;
			}

			// Token: 0x17001973 RID: 6515
			// (get) Token: 0x0600D0A9 RID: 53417
			public abstract string Identifier { get; }

			// Token: 0x04009F6A RID: 40810
			public readonly bool IsOutput;

			// Token: 0x04009F6B RID: 40811
			public readonly bool IsDefault;
		}

		// Token: 0x02001B49 RID: 6985
		public class VoiceAudioDeviceNotFound : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D0AA RID: 53418 RVA: 0x004C03AB File Offset: 0x004BE5AB
			public VoiceAudioDeviceNotFound()
				: base(false, false)
			{
			}

			// Token: 0x0600D0AB RID: 53419 RVA: 0x004C03B5 File Offset: 0x004BE5B5
			public override string ToString()
			{
				return Localization.Get("noAudioDeviceFound", false, null);
			}

			// Token: 0x17001974 RID: 6516
			// (get) Token: 0x0600D0AC RID: 53420 RVA: 0x00031E8B File Offset: 0x0003008B
			public override string Identifier
			{
				get
				{
					return "";
				}
			}
		}

		// Token: 0x02001B4A RID: 6986
		public class VoiceAudioDeviceDefault : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D0AD RID: 53421 RVA: 0x004C03AB File Offset: 0x004BE5AB
			public VoiceAudioDeviceDefault()
				: base(false, false)
			{
			}

			// Token: 0x0600D0AE RID: 53422 RVA: 0x004C03C3 File Offset: 0x004BE5C3
			public override string ToString()
			{
				return Localization.Get("defaultAudioDevice", false, null);
			}

			// Token: 0x17001975 RID: 6517
			// (get) Token: 0x0600D0AF RID: 53423 RVA: 0x00031E8B File Offset: 0x0003008B
			public override string Identifier
			{
				get
				{
					return "";
				}
			}
		}
	}
}
