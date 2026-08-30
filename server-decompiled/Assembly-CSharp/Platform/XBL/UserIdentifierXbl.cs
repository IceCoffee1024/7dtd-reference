using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001BEB RID: 7147
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierXbl : PlatformUserIdentifierAbs
	{
		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x0600D401 RID: 54273 RVA: 0x0007FE9C File Offset: 0x0007E09C
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.XBL;
			}
		}

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x0600D402 RID: 54274 RVA: 0x004C8914 File Offset: 0x004C6B14
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.XBL);

		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x0600D403 RID: 54275 RVA: 0x004C891C File Offset: 0x004C6B1C
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x0600D404 RID: 54276 RVA: 0x004C8924 File Offset: 0x004C6B24
		public override string CombinedString { get; }

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x0600D405 RID: 54277 RVA: 0x004C892C File Offset: 0x004C6B2C
		public ulong Xuid
		{
			get
			{
				return XblXuidMapper.GetXuid(this);
			}
		}

		// Token: 0x0600D406 RID: 54278 RVA: 0x004C8934 File Offset: 0x004C6B34
		public UserIdentifierXbl(string _pxuid)
		{
			this.pxuid = _pxuid;
			this.ReadablePlatformUserIdentifier = _pxuid;
			this.CombinedString = this.PlatformIdentifierString + "_" + _pxuid;
			this.hashcode = _pxuid.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600D407 RID: 54279 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600D408 RID: 54280 RVA: 0x004C8994 File Offset: 0x004C6B94
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

		// Token: 0x0600D409 RID: 54281 RVA: 0x004C89CA File Offset: 0x004C6BCA
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A130 RID: 41264
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string pxuid;

		// Token: 0x0400A131 RID: 41265
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
