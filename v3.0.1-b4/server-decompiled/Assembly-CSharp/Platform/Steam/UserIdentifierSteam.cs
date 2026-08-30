using System;
using Steamworks;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C7C RID: 7292
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierSteam : PlatformUserIdentifierAbs
	{
		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x0600D7E2 RID: 55266 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x0600D7E3 RID: 55267 RVA: 0x004DA8EC File Offset: 0x004D8AEC
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Steam);

		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x0600D7E4 RID: 55268 RVA: 0x004DA8F4 File Offset: 0x004D8AF4
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x0600D7E5 RID: 55269 RVA: 0x004DA8FC File Offset: 0x004D8AFC
		public override string CombinedString { get; }

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x0600D7E6 RID: 55270 RVA: 0x004DA904 File Offset: 0x004D8B04
		// (set) Token: 0x0600D7E7 RID: 55271 RVA: 0x004DA90C File Offset: 0x004D8B0C
		public byte[] Ticket
		{
			get
			{
				return this.ticket;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this.ticket = value;
			}
		}

		// Token: 0x0600D7E8 RID: 55272 RVA: 0x004DA918 File Offset: 0x004D8B18
		public UserIdentifierSteam(string _steamId)
		{
			ulong num;
			if (_steamId.Length != 17 || !ulong.TryParse(_steamId, out num))
			{
				throw new ArgumentException("Not a valid SteamID: " + _steamId, "_steamId");
			}
			this.SteamId = num;
			this.ReadablePlatformUserIdentifier = _steamId;
			this.CombinedString = this.PlatformIdentifierString + "_" + _steamId;
			this.hashcode = num.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600D7E9 RID: 55273 RVA: 0x004DA9A0 File Offset: 0x004D8BA0
		public UserIdentifierSteam(ulong _steamId)
		{
			if (_steamId < 10000000000000000UL || _steamId > 99999999999999999UL)
			{
				throw new ArgumentException("Not a valid SteamID: " + _steamId.ToString(), "_steamId");
			}
			this.SteamId = _steamId;
			this.ReadablePlatformUserIdentifier = _steamId.ToString();
			this.CombinedString = this.PlatformIdentifierString + "_" + _steamId.ToString();
			this.hashcode = _steamId.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600D7EA RID: 55274 RVA: 0x004DAA40 File Offset: 0x004D8C40
		public UserIdentifierSteam(CSteamID _steamId)
		{
			this.SteamId = _steamId.m_SteamID;
			this.ReadablePlatformUserIdentifier = _steamId.ToString();
			string platformIdentifierString = this.PlatformIdentifierString;
			string text = "_";
			CSteamID csteamID = _steamId;
			this.CombinedString = platformIdentifierString + text + csteamID.ToString();
			this.hashcode = _steamId.m_SteamID.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600D7EB RID: 55275 RVA: 0x004DAAC4 File Offset: 0x004D8CC4
		public override bool DecodeTicket(string _ticket)
		{
			if (string.IsNullOrEmpty(_ticket))
			{
				return false;
			}
			try
			{
				this.Ticket = Convert.FromBase64String(_ticket);
			}
			catch (FormatException ex)
			{
				Log.Error("Convert.FromBase64String: " + ex.Message);
				Log.Exception(ex);
				return false;
			}
			return true;
		}

		// Token: 0x0600D7EC RID: 55276 RVA: 0x004DAB20 File Offset: 0x004D8D20
		public override bool Equals(PlatformUserIdentifierAbs _other)
		{
			if (_other == null)
			{
				return false;
			}
			if (this == _other)
			{
				return true;
			}
			UserIdentifierSteam userIdentifierSteam = _other as UserIdentifierSteam;
			return userIdentifierSteam != null && userIdentifierSteam.SteamId == this.SteamId;
		}

		// Token: 0x0600D7ED RID: 55277 RVA: 0x004DAB52 File Offset: 0x004D8D52
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A3C4 RID: 41924
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public byte[] ticket;

		// Token: 0x0400A3C5 RID: 41925
		public UserIdentifierSteam OwnerId;

		// Token: 0x0400A3C6 RID: 41926
		public readonly ulong SteamId;

		// Token: 0x0400A3C7 RID: 41927
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
