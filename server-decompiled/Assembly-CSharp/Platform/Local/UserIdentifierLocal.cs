using System;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001D2F RID: 7471
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierLocal : PlatformUserIdentifierAbs
	{
		// Token: 0x17001C46 RID: 7238
		// (get) Token: 0x0600DD78 RID: 56696 RVA: 0x00020119 File Offset: 0x0001E319
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.Local;
			}
		}

		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x0600DD79 RID: 56697 RVA: 0x004EC4F6 File Offset: 0x004EA6F6
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.Local);

		// Token: 0x17001C48 RID: 7240
		// (get) Token: 0x0600DD7A RID: 56698 RVA: 0x004EC4FE File Offset: 0x004EA6FE
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001C49 RID: 7241
		// (get) Token: 0x0600DD7B RID: 56699 RVA: 0x004EC506 File Offset: 0x004EA706
		public override string CombinedString { get; }

		// Token: 0x0600DD7C RID: 56700 RVA: 0x004EC510 File Offset: 0x004EA710
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

		// Token: 0x0600DD7D RID: 56701 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600DD7E RID: 56702 RVA: 0x004EC588 File Offset: 0x004EA788
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

		// Token: 0x0600DD7F RID: 56703 RVA: 0x004EC5BD File Offset: 0x004EA7BD
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A73C RID: 42812
		public readonly string PlayerName;

		// Token: 0x0400A73D RID: 42813
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
