using System;
using UnityEngine.Scripting;

namespace Platform.XBL
{
	// Token: 0x02001C12 RID: 7186
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierXbl : PlatformUserIdentifierAbs
	{
		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x0600D58F RID: 54671 RVA: 0x00080550 File Offset: 0x0007E750
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.XBL;
			}
		}

		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x0600D590 RID: 54672 RVA: 0x004CED08 File Offset: 0x004CCF08
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.XBL);

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x0600D591 RID: 54673 RVA: 0x004CED10 File Offset: 0x004CCF10
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x0600D592 RID: 54674 RVA: 0x004CED18 File Offset: 0x004CCF18
		public override string CombinedString { get; }

		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x0600D593 RID: 54675 RVA: 0x004CED20 File Offset: 0x004CCF20
		public ulong Xuid
		{
			get
			{
				return XblXuidMapper.GetXuid(this);
			}
		}

		// Token: 0x0600D594 RID: 54676 RVA: 0x004CED28 File Offset: 0x004CCF28
		public UserIdentifierXbl(string _pxuid)
		{
			this.pxuid = _pxuid;
			this.ReadablePlatformUserIdentifier = _pxuid;
			this.CombinedString = this.PlatformIdentifierString + "_" + _pxuid;
			this.hashcode = _pxuid.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600D595 RID: 54677 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600D596 RID: 54678 RVA: 0x004CED88 File Offset: 0x004CCF88
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

		// Token: 0x0600D597 RID: 54679 RVA: 0x004CEDBE File Offset: 0x004CCFBE
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A261 RID: 41569
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string pxuid;

		// Token: 0x0400A262 RID: 41570
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
