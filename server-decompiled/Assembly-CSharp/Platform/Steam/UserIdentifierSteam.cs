using System;
using Steamworks;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CD5 RID: 7381
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierSteam : PlatformUserIdentifierAbs
	{
		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x0600DAEE RID: 56046 RVA: 0x000470B6 File Offset: 0x000452B6
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x0600DAEF RID: 56047 RVA: 0x004E5950 File Offset: 0x004E3B50
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Steam);

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600DAF0 RID: 56048 RVA: 0x004E5958 File Offset: 0x004E3B58
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x0600DAF1 RID: 56049 RVA: 0x004E5960 File Offset: 0x004E3B60
		public override string CombinedString { get; }

		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x0600DAF2 RID: 56050 RVA: 0x004E5968 File Offset: 0x004E3B68
		// (set) Token: 0x0600DAF3 RID: 56051 RVA: 0x004E5970 File Offset: 0x004E3B70
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

		// Token: 0x0600DAF4 RID: 56052 RVA: 0x004E597C File Offset: 0x004E3B7C
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

		// Token: 0x0600DAF5 RID: 56053 RVA: 0x004E5A04 File Offset: 0x004E3C04
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

		// Token: 0x0600DAF6 RID: 56054 RVA: 0x004E5AA4 File Offset: 0x004E3CA4
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

		// Token: 0x0600DAF7 RID: 56055 RVA: 0x004E5B28 File Offset: 0x004E3D28
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

		// Token: 0x0600DAF8 RID: 56056 RVA: 0x004E5B84 File Offset: 0x004E3D84
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

		// Token: 0x0600DAF9 RID: 56057 RVA: 0x004E5BB6 File Offset: 0x004E3DB6
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A5CB RID: 42443
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public byte[] ticket;

		// Token: 0x0400A5CC RID: 42444
		public UserIdentifierSteam OwnerId;

		// Token: 0x0400A5CD RID: 42445
		public readonly ulong SteamId;

		// Token: 0x0400A5CE RID: 42446
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
