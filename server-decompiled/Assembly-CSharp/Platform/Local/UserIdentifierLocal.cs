using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001CFE RID: 7422
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierLocal : PlatformUserIdentifierAbs
	{
		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x0600DC64 RID: 56420 RVA: 0x00020119 File Offset: 0x0001E319
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Local;
			}
		}

		// Token: 0x17001BFF RID: 7167
		// (get) Token: 0x0600DC65 RID: 56421 RVA: 0x004E8956 File Offset: 0x004E6B56
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Local);

		// Token: 0x17001C00 RID: 7168
		// (get) Token: 0x0600DC66 RID: 56422 RVA: 0x004E895E File Offset: 0x004E6B5E
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001C01 RID: 7169
		// (get) Token: 0x0600DC67 RID: 56423 RVA: 0x004E8966 File Offset: 0x004E6B66
		public override string CombinedString { get; }

		// Token: 0x0600DC68 RID: 56424 RVA: 0x004E8970 File Offset: 0x004E6B70
		public UserIdentifierLocal(string _playername)
		{
			if (string.IsNullOrEmpty(_playername))
			{
				throw new ArgumentException("Playername must not be empty", "_playername");
			}
			this.PlayerName = _playername;
			this.ReadablePlatformUserIdentifier = _playername;
			this.CombinedString = this.PlatformIdentifierString + "_" + _playername;
			this.hashcode = _playername.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600DC69 RID: 56425 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600DC6A RID: 56426 RVA: 0x004E89E8 File Offset: 0x004E6BE8
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
			UserIdentifierLocal userIdentifierLocal = _other as UserIdentifierLocal;
			return userIdentifierLocal != null && userIdentifierLocal.PlayerName == this.PlayerName;
		}

		// Token: 0x0600DC6B RID: 56427 RVA: 0x004E8A1D File Offset: 0x004E6C1D
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A653 RID: 42579
		public readonly string PlayerName;

		// Token: 0x0400A654 RID: 42580
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
