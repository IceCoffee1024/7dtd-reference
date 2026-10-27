using System;
using Steamworks;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001D06 RID: 7430
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierSteam : PlatformUserIdentifierAbs
	{
		// Token: 0x17001BEA RID: 7146
		// (get) Token: 0x0600DC02 RID: 56322 RVA: 0x00047076 File Offset: 0x00045276
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001BEB RID: 7147
		// (get) Token: 0x0600DC03 RID: 56323 RVA: 0x004E94F0 File Offset: 0x004E76F0
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Steam);

		// Token: 0x17001BEC RID: 7148
		// (get) Token: 0x0600DC04 RID: 56324 RVA: 0x004E94F8 File Offset: 0x004E76F8
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001BED RID: 7149
		// (get) Token: 0x0600DC05 RID: 56325 RVA: 0x004E9500 File Offset: 0x004E7700
		public override string CombinedString { get; }

		// Token: 0x17001BEE RID: 7150
		// (get) Token: 0x0600DC06 RID: 56326 RVA: 0x004E9508 File Offset: 0x004E7708
		// (set) Token: 0x0600DC07 RID: 56327 RVA: 0x004E9510 File Offset: 0x004E7710
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

		// Token: 0x0600DC08 RID: 56328 RVA: 0x004E951C File Offset: 0x004E771C
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

		// Token: 0x0600DC09 RID: 56329 RVA: 0x004E95A4 File Offset: 0x004E77A4
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

		// Token: 0x0600DC0A RID: 56330 RVA: 0x004E9644 File Offset: 0x004E7844
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

		// Token: 0x0600DC0B RID: 56331 RVA: 0x004E96C8 File Offset: 0x004E78C8
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

		// Token: 0x0600DC0C RID: 56332 RVA: 0x004E9724 File Offset: 0x004E7924
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

		// Token: 0x0600DC0D RID: 56333 RVA: 0x004E9756 File Offset: 0x004E7956
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A6B4 RID: 42676
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public byte[] ticket;

		// Token: 0x0400A6B5 RID: 42677
		public UserIdentifierSteam OwnerId;

		// Token: 0x0400A6B6 RID: 42678
		public readonly ulong SteamId;

		// Token: 0x0400A6B7 RID: 42679
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
