using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001C44 RID: 7236
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierXbl : PlatformUserIdentifierAbs
	{
		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x0600D707 RID: 55047 RVA: 0x00080804 File Offset: 0x0007EA04
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.XBL;
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x0600D708 RID: 55048 RVA: 0x004D37F0 File Offset: 0x004D19F0
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.XBL);

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x0600D709 RID: 55049 RVA: 0x004D37F8 File Offset: 0x004D19F8
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x0600D70A RID: 55050 RVA: 0x004D3800 File Offset: 0x004D1A00
		public override string CombinedString { get; }

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x0600D70B RID: 55051 RVA: 0x004D3808 File Offset: 0x004D1A08
		public ulong Xuid
		{
			get
			{
				return XblXuidMapper.GetXuid(this);
			}
		}

		// Token: 0x0600D70C RID: 55052 RVA: 0x004D3810 File Offset: 0x004D1A10
		public UserIdentifierXbl(string _pxuid)
		{
			this.pxuid = _pxuid;
			this.ReadablePlatformUserIdentifier = _pxuid;
			this.CombinedString = this.PlatformIdentifierString + "_" + _pxuid;
			this.hashcode = _pxuid.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600D70D RID: 55053 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600D70E RID: 55054 RVA: 0x004D3870 File Offset: 0x004D1A70
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

		// Token: 0x0600D70F RID: 55055 RVA: 0x004D38A6 File Offset: 0x004D1AA6
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A336 RID: 41782
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string pxuid;

		// Token: 0x0400A337 RID: 41783
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
