using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001CA5 RID: 7333
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierLocal : PlatformUserIdentifierAbs
	{
		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x0600D94E RID: 55630 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Local;
			}
		}

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x0600D94F RID: 55631 RVA: 0x004DD7FA File Offset: 0x004DB9FA
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Local);

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x0600D950 RID: 55632 RVA: 0x004DD802 File Offset: 0x004DBA02
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x0600D951 RID: 55633 RVA: 0x004DD80A File Offset: 0x004DBA0A
		public override string CombinedString { get; }

		// Token: 0x0600D952 RID: 55634 RVA: 0x004DD814 File Offset: 0x004DBA14
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

		// Token: 0x0600D953 RID: 55635 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600D954 RID: 55636 RVA: 0x004DD88C File Offset: 0x004DBA8C
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

		// Token: 0x0600D955 RID: 55637 RVA: 0x004DD8C1 File Offset: 0x004DBAC1
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A44B RID: 42059
		public readonly string PlayerName;

		// Token: 0x0400A44C RID: 42060
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
