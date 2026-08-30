using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B6C RID: 7020
	public interface IPartyVoice
	{
		// Token: 0x1400011E RID: 286
		// (add) Token: 0x0600D217 RID: 53783
		// (remove) Token: 0x0600D218 RID: 53784
		event Action Initialized;

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x0600D219 RID: 53785
		EPartyVoiceStatus Status { get; }

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x0600D21A RID: 53786
		bool InLobby { get; }

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x0600D21B RID: 53787
		bool InLobbyOrProgress { get; }

		// Token: 0x0600D21C RID: 53788
		void Init(IPlatform _owner);

		// Token: 0x0600D21D RID: 53789
		void Destroy();

		// Token: 0x0600D21E RID: 53790
		void CreateLobby(Action<string> _lobbyCreatedCallback);

		// Token: 0x0600D21F RID: 53791
		void JoinLobby(string _lobbyId);

		// Token: 0x0600D220 RID: 53792
		void LeaveLobby();

		// Token: 0x0600D221 RID: 53793
		void PromoteLeader(PlatformUserIdentifierAbs _newLeaderIdentifier);

		// Token: 0x0600D222 RID: 53794
		bool IsLobbyOwner();

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x0600D223 RID: 53795
		// (remove) Token: 0x0600D224 RID: 53796
		event Action<IPartyVoice.EVoiceChannelAction> OnLocalPlayerStateChanged;

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x0600D225 RID: 53797
		// (remove) Token: 0x0600D226 RID: 53798
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> OnRemotePlayerStateChanged;

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x0600D227 RID: 53799
		// (remove) Token: 0x0600D228 RID: 53800
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> OnRemotePlayerVoiceStateChanged;

		// Token: 0x0600D229 RID: 53801
		[return: TupleElementNames(new string[] { "inputDevices", "outputDevices" })]
		ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>> GetDevicesList();

		// Token: 0x0600D22A RID: 53802
		void SetInputDevice(string _device);

		// Token: 0x0600D22B RID: 53803
		void SetOutputDevice(string _device);

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x0600D22C RID: 53804
		// (set) Token: 0x0600D22D RID: 53805
		bool MuteSelf { get; set; }

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x0600D22E RID: 53806
		// (set) Token: 0x0600D22F RID: 53807
		bool MuteOthers { get; set; }

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x0600D230 RID: 53808
		// (set) Token: 0x0600D231 RID: 53809
		float OutputVolume { get; set; }

		// Token: 0x0600D232 RID: 53810
		void BlockUser(PlatformUserIdentifierAbs _userIdentifier, bool _block);

		// Token: 0x02001B6D RID: 7021
		public enum EVoiceMemberState
		{
			// Token: 0x0400A094 RID: 41108
			Disabled,
			// Token: 0x0400A095 RID: 41109
			Normal,
			// Token: 0x0400A096 RID: 41110
			Muted,
			// Token: 0x0400A097 RID: 41111
			VoiceActive
		}

		// Token: 0x02001B6E RID: 7022
		public enum EVoiceChannelAction
		{
			// Token: 0x0400A099 RID: 41113
			Joined,
			// Token: 0x0400A09A RID: 41114
			Left
		}

		// Token: 0x02001B6F RID: 7023
		public abstract class VoiceAudioDevice
		{
			// Token: 0x0600D233 RID: 53811 RVA: 0x004C6789 File Offset: 0x004C4989
			public VoiceAudioDevice(bool _isOutput, bool _isDefault)
			{
				this.IsOutput = _isOutput;
				this.IsDefault = _isDefault;
			}

			// Token: 0x170019E5 RID: 6629
			// (get) Token: 0x0600D234 RID: 53812
			public abstract string Identifier { get; }

			// Token: 0x0400A09B RID: 41115
			public readonly bool IsOutput;

			// Token: 0x0400A09C RID: 41116
			public readonly bool IsDefault;
		}

		// Token: 0x02001B70 RID: 7024
		public class VoiceAudioDeviceNotFound : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D235 RID: 53813 RVA: 0x004C679F File Offset: 0x004C499F
			public VoiceAudioDeviceNotFound()
				: base(false, false)
			{
			}

			// Token: 0x0600D236 RID: 53814 RVA: 0x004C67A9 File Offset: 0x004C49A9
			public override string ToString()
			{
				return Localization.Get("noAudioDeviceFound", false, null);
			}

			// Token: 0x170019E6 RID: 6630
			// (get) Token: 0x0600D237 RID: 53815 RVA: 0x00032147 File Offset: 0x00030347
			public override string Identifier
			{
				get
				{
					return "";
				}
			}
		}

		// Token: 0x02001B71 RID: 7025
		public class VoiceAudioDeviceDefault : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D238 RID: 53816 RVA: 0x004C679F File Offset: 0x004C499F
			public VoiceAudioDeviceDefault()
				: base(false, false)
			{
			}

			// Token: 0x0600D239 RID: 53817 RVA: 0x004C67B7 File Offset: 0x004C49B7
			public override string ToString()
			{
				return Localization.Get("defaultAudioDevice", false, null);
			}

			// Token: 0x170019E7 RID: 6631
			// (get) Token: 0x0600D23A RID: 53818 RVA: 0x00032147 File Offset: 0x00030347
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
