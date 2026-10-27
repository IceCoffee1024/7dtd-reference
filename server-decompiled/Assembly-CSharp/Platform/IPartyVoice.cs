using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001BCF RID: 7119
	public interface IPartyVoice
	{
		// Token: 0x1400011D RID: 285
		// (add) Token: 0x0600D4A3 RID: 54435
		// (remove) Token: 0x0600D4A4 RID: 54436
		event Action Initialized;

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600D4A5 RID: 54437
		EPartyVoiceStatus Status { get; }

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x0600D4A6 RID: 54438
		bool InLobby { get; }

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x0600D4A7 RID: 54439
		bool InLobbyOrProgress { get; }

		// Token: 0x0600D4A8 RID: 54440
		void Init(IPlatform _owner);

		// Token: 0x0600D4A9 RID: 54441
		void Destroy();

		// Token: 0x0600D4AA RID: 54442
		void CreateLobby(Action<string> _lobbyCreatedCallback);

		// Token: 0x0600D4AB RID: 54443
		void JoinLobby(string _lobbyId);

		// Token: 0x0600D4AC RID: 54444
		void LeaveLobby();

		// Token: 0x0600D4AD RID: 54445
		void PromoteLeader(PlatformUserIdentifierAbs _newLeaderIdentifier);

		// Token: 0x0600D4AE RID: 54446
		bool IsLobbyOwner();

		// Token: 0x1400011E RID: 286
		// (add) Token: 0x0600D4AF RID: 54447
		// (remove) Token: 0x0600D4B0 RID: 54448
		event Action<IPartyVoice.EVoiceChannelAction> OnLocalPlayerStateChanged;

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x0600D4B1 RID: 54449
		// (remove) Token: 0x0600D4B2 RID: 54450
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> OnRemotePlayerStateChanged;

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x0600D4B3 RID: 54451
		// (remove) Token: 0x0600D4B4 RID: 54452
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> OnRemotePlayerVoiceStateChanged;

		// Token: 0x0600D4B5 RID: 54453
		[return: TupleElementNames(new string[] { "inputDevices", "outputDevices" })]
		ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>> GetDevicesList();

		// Token: 0x0600D4B6 RID: 54454
		void SetInputDevice(string _device);

		// Token: 0x0600D4B7 RID: 54455
		void SetOutputDevice(string _device);

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x0600D4B8 RID: 54456
		// (set) Token: 0x0600D4B9 RID: 54457
		bool MuteSelf { get; set; }

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x0600D4BA RID: 54458
		// (set) Token: 0x0600D4BB RID: 54459
		bool MuteOthers { get; set; }

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x0600D4BC RID: 54460
		// (set) Token: 0x0600D4BD RID: 54461
		float OutputVolume { get; set; }

		// Token: 0x0600D4BE RID: 54462
		void BlockUser(PlatformUserIdentifierAbs _userIdentifier, bool _block);

		// Token: 0x02001BD0 RID: 7120
		public enum EVoiceMemberState
		{
			// Token: 0x0400A252 RID: 41554
			Disabled,
			// Token: 0x0400A253 RID: 41555
			Normal,
			// Token: 0x0400A254 RID: 41556
			Muted,
			// Token: 0x0400A255 RID: 41557
			VoiceActive
		}

		// Token: 0x02001BD1 RID: 7121
		public enum EVoiceChannelAction
		{
			// Token: 0x0400A257 RID: 41559
			Joined,
			// Token: 0x0400A258 RID: 41560
			Left
		}

		// Token: 0x02001BD2 RID: 7122
		public abstract class VoiceAudioDevice
		{
			// Token: 0x0600D4BF RID: 54463 RVA: 0x004CED99 File Offset: 0x004CCF99
			public VoiceAudioDevice(bool _isOutput, bool _isDefault)
			{
				this.IsOutput = _isOutput;
				this.IsDefault = _isDefault;
			}

			// Token: 0x17001AD3 RID: 6867
			// (get) Token: 0x0600D4C0 RID: 54464
			public abstract string Identifier { get; }

			// Token: 0x0400A259 RID: 41561
			public readonly bool IsOutput;

			// Token: 0x0400A25A RID: 41562
			public readonly bool IsDefault;
		}

		// Token: 0x02001BD3 RID: 7123
		public class VoiceAudioDeviceNotFound : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D4C1 RID: 54465 RVA: 0x004CEDAF File Offset: 0x004CCFAF
			public VoiceAudioDeviceNotFound()
				: base(false, false)
			{
			}

			// Token: 0x0600D4C2 RID: 54466 RVA: 0x004CEDB9 File Offset: 0x004CCFB9
			public override string ToString()
			{
				return Localization.Get("noAudioDeviceFound", false, null);
			}

			// Token: 0x17001AD4 RID: 6868
			// (get) Token: 0x0600D4C3 RID: 54467 RVA: 0x000321FF File Offset: 0x000303FF
			public override string Identifier
			{
				get
				{
					return "";
				}
			}
		}

		// Token: 0x02001BD4 RID: 7124
		public class VoiceAudioDeviceDefault : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D4C4 RID: 54468 RVA: 0x004CEDAF File Offset: 0x004CCFAF
			public VoiceAudioDeviceDefault()
				: base(false, false)
			{
			}

			// Token: 0x0600D4C5 RID: 54469 RVA: 0x004CEDC7 File Offset: 0x004CCFC7
			public override string ToString()
			{
				return Localization.Get("defaultAudioDevice", false, null);
			}

			// Token: 0x17001AD5 RID: 6869
			// (get) Token: 0x0600D4C6 RID: 54470 RVA: 0x000321FF File Offset: 0x000303FF
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
