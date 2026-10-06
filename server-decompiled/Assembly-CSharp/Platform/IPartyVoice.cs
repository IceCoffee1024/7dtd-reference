using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B9E RID: 7070
	public interface IPartyVoice
	{
		// Token: 0x1400011D RID: 285
		// (add) Token: 0x0600D38F RID: 54159
		// (remove) Token: 0x0600D390 RID: 54160
		event Action Initialized;

		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x0600D391 RID: 54161
		EPartyVoiceStatus Status { get; }

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x0600D392 RID: 54162
		bool InLobby { get; }

		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x0600D393 RID: 54163
		bool InLobbyOrProgress { get; }

		// Token: 0x0600D394 RID: 54164
		void Init(IPlatform _owner);

		// Token: 0x0600D395 RID: 54165
		void Destroy();

		// Token: 0x0600D396 RID: 54166
		void CreateLobby(Action<string> _lobbyCreatedCallback);

		// Token: 0x0600D397 RID: 54167
		void JoinLobby(string _lobbyId);

		// Token: 0x0600D398 RID: 54168
		void LeaveLobby();

		// Token: 0x0600D399 RID: 54169
		void PromoteLeader(PlatformUserIdentifierAbs _newLeaderIdentifier);

		// Token: 0x0600D39A RID: 54170
		bool IsLobbyOwner();

		// Token: 0x1400011E RID: 286
		// (add) Token: 0x0600D39B RID: 54171
		// (remove) Token: 0x0600D39C RID: 54172
		event Action<IPartyVoice.EVoiceChannelAction> OnLocalPlayerStateChanged;

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x0600D39D RID: 54173
		// (remove) Token: 0x0600D39E RID: 54174
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceChannelAction> OnRemotePlayerStateChanged;

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x0600D39F RID: 54175
		// (remove) Token: 0x0600D3A0 RID: 54176
		event Action<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> OnRemotePlayerVoiceStateChanged;

		// Token: 0x0600D3A1 RID: 54177
		[return: TupleElementNames(new string[] { "inputDevices", "outputDevices" })]
		ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>> GetDevicesList();

		// Token: 0x0600D3A2 RID: 54178
		void SetInputDevice(string _device);

		// Token: 0x0600D3A3 RID: 54179
		void SetOutputDevice(string _device);

		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x0600D3A4 RID: 54180
		// (set) Token: 0x0600D3A5 RID: 54181
		bool MuteSelf { get; set; }

		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x0600D3A6 RID: 54182
		// (set) Token: 0x0600D3A7 RID: 54183
		bool MuteOthers { get; set; }

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x0600D3A8 RID: 54184
		// (set) Token: 0x0600D3A9 RID: 54185
		float OutputVolume { get; set; }

		// Token: 0x0600D3AA RID: 54186
		void BlockUser(PlatformUserIdentifierAbs _userIdentifier, bool _block);

		// Token: 0x02001B9F RID: 7071
		public enum EVoiceMemberState
		{
			// Token: 0x0400A169 RID: 41321
			Disabled,
			// Token: 0x0400A16A RID: 41322
			Normal,
			// Token: 0x0400A16B RID: 41323
			Muted,
			// Token: 0x0400A16C RID: 41324
			VoiceActive
		}

		// Token: 0x02001BA0 RID: 7072
		public enum EVoiceChannelAction
		{
			// Token: 0x0400A16E RID: 41326
			Joined,
			// Token: 0x0400A16F RID: 41327
			Left
		}

		// Token: 0x02001BA1 RID: 7073
		public abstract class VoiceAudioDevice
		{
			// Token: 0x0600D3AB RID: 54187 RVA: 0x004CB271 File Offset: 0x004C9471
			public VoiceAudioDevice(bool _isOutput, bool _isDefault)
			{
				this.IsOutput = _isOutput;
				this.IsDefault = _isDefault;
			}

			// Token: 0x17001A8B RID: 6795
			// (get) Token: 0x0600D3AC RID: 54188
			public abstract string Identifier { get; }

			// Token: 0x0400A170 RID: 41328
			public readonly bool IsOutput;

			// Token: 0x0400A171 RID: 41329
			public readonly bool IsDefault;
		}

		// Token: 0x02001BA2 RID: 7074
		public class VoiceAudioDeviceNotFound : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D3AD RID: 54189 RVA: 0x004CB287 File Offset: 0x004C9487
			public VoiceAudioDeviceNotFound()
				: base(false, false)
			{
			}

			// Token: 0x0600D3AE RID: 54190 RVA: 0x004CB291 File Offset: 0x004C9491
			public override string ToString()
			{
				return Localization.Get("noAudioDeviceFound", false, null);
			}

			// Token: 0x17001A8C RID: 6796
			// (get) Token: 0x0600D3AF RID: 54191 RVA: 0x0003223F File Offset: 0x0003043F
			public override string Identifier
			{
				get
				{
					return "";
				}
			}
		}

		// Token: 0x02001BA3 RID: 7075
		public class VoiceAudioDeviceDefault : IPartyVoice.VoiceAudioDevice
		{
			// Token: 0x0600D3B0 RID: 54192 RVA: 0x004CB287 File Offset: 0x004C9487
			public VoiceAudioDeviceDefault()
				: base(false, false)
			{
			}

			// Token: 0x0600D3B1 RID: 54193 RVA: 0x004CB29F File Offset: 0x004C949F
			public override string ToString()
			{
				return Localization.Get("defaultAudioDevice", false, null);
			}

			// Token: 0x17001A8D RID: 6797
			// (get) Token: 0x0600D3B2 RID: 54194 RVA: 0x0003223F File Offset: 0x0003043F
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
