using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001CCC RID: 7372
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierLocal : PlatformUserIdentifierAbs
	{
		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x0600DAE9 RID: 56041 RVA: 0x00020021 File Offset: 0x0001E221
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Local;
			}
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x0600DAEA RID: 56042 RVA: 0x004E3E0E File Offset: 0x004E200E
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Local);

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x0600DAEB RID: 56043 RVA: 0x004E3E16 File Offset: 0x004E2016
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x0600DAEC RID: 56044 RVA: 0x004E3E1E File Offset: 0x004E201E
		public override string CombinedString { get; }

		// Token: 0x0600DAED RID: 56045 RVA: 0x004E3E28 File Offset: 0x004E2028
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

		// Token: 0x0600DAEE RID: 56046 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600DAEF RID: 56047 RVA: 0x004E3EA0 File Offset: 0x004E20A0
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

		// Token: 0x0600DAF0 RID: 56048 RVA: 0x004E3ED5 File Offset: 0x004E20D5
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A57E RID: 42366
		public readonly string PlayerName;

		// Token: 0x0400A57F RID: 42367
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
