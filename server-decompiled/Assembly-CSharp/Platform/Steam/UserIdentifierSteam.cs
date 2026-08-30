using System;
using Steamworks;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CA3 RID: 7331
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierSteam : PlatformUserIdentifierAbs
	{
		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x0600D974 RID: 55668 RVA: 0x00046EDA File Offset: 0x000450DA
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x0600D975 RID: 55669 RVA: 0x004E0E08 File Offset: 0x004DF008
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Steam);

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x0600D976 RID: 55670 RVA: 0x004E0E10 File Offset: 0x004DF010
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x0600D977 RID: 55671 RVA: 0x004E0E18 File Offset: 0x004DF018
		public override string CombinedString { get; }

		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x0600D978 RID: 55672 RVA: 0x004E0E20 File Offset: 0x004DF020
		// (set) Token: 0x0600D979 RID: 55673 RVA: 0x004E0E28 File Offset: 0x004DF028
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

		// Token: 0x0600D97A RID: 55674 RVA: 0x004E0E34 File Offset: 0x004DF034
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

		// Token: 0x0600D97B RID: 55675 RVA: 0x004E0EBC File Offset: 0x004DF0BC
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

		// Token: 0x0600D97C RID: 55676 RVA: 0x004E0F5C File Offset: 0x004DF15C
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

		// Token: 0x0600D97D RID: 55677 RVA: 0x004E0FE0 File Offset: 0x004DF1E0
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

		// Token: 0x0600D97E RID: 55678 RVA: 0x004E103C File Offset: 0x004DF23C
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

		// Token: 0x0600D97F RID: 55679 RVA: 0x004E106E File Offset: 0x004DF26E
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A4F6 RID: 42230
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public byte[] ticket;

		// Token: 0x0400A4F7 RID: 42231
		public UserIdentifierSteam OwnerId;

		// Token: 0x0400A4F8 RID: 42232
		public readonly ulong SteamId;

		// Token: 0x0400A4F9 RID: 42233
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
