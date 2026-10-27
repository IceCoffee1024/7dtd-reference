using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001C75 RID: 7285
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierXbl : PlatformUserIdentifierAbs
	{
		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x0600D81B RID: 55323 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.XBL;
			}
		}

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x0600D81C RID: 55324 RVA: 0x004D7318 File Offset: 0x004D5518
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.XBL);

		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x0600D81D RID: 55325 RVA: 0x004D7320 File Offset: 0x004D5520
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x0600D81E RID: 55326 RVA: 0x004D7328 File Offset: 0x004D5528
		public override string CombinedString { get; }

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600D81F RID: 55327 RVA: 0x004D7330 File Offset: 0x004D5530
		public ulong Xuid
		{
			get
			{
				return XblXuidMapper.GetXuid(this);
			}
		}

		// Token: 0x0600D820 RID: 55328 RVA: 0x004D7338 File Offset: 0x004D5538
		public UserIdentifierXbl(string _pxuid)
		{
			this.pxuid = _pxuid;
			this.ReadablePlatformUserIdentifier = _pxuid;
			this.CombinedString = this.PlatformIdentifierString + "_" + _pxuid;
			this.hashcode = _pxuid.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600D821 RID: 55329 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600D822 RID: 55330 RVA: 0x004D7398 File Offset: 0x004D5598
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
			UserIdentifierXbl userIdentifierXbl = _other as UserIdentifierXbl;
			return userIdentifierXbl != null && string.Equals(userIdentifierXbl.pxuid, this.pxuid, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600D823 RID: 55331 RVA: 0x004D73CE File Offset: 0x004D55CE
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A41F RID: 42015
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string pxuid;

		// Token: 0x0400A420 RID: 42016
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
