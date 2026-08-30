using System;
using UnityEngine.Scripting;

namespace Platform.PSN
{
	// Token: 0x02001C93 RID: 7315
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierPSN : PlatformUserIdentifierAbs
	{
		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x0600D89D RID: 55453 RVA: 0x00080065 File Offset: 0x0007E265
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.PSN;
			}
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x0600D89E RID: 55454 RVA: 0x004DC537 File Offset: 0x004DA737
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.PSN);

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x0600D89F RID: 55455 RVA: 0x004DC53F File Offset: 0x004DA73F
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x0600D8A0 RID: 55456 RVA: 0x004DC547 File Offset: 0x004DA747
		public override string CombinedString { get; }

		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x0600D8A1 RID: 55457 RVA: 0x004DC54F File Offset: 0x004DA74F
		// (set) Token: 0x0600D8A2 RID: 55458 RVA: 0x004DC557 File Offset: 0x004DA757
		public ulong AccountId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D8A3 RID: 55459 RVA: 0x004DC560 File Offset: 0x004DA760
		public UserIdentifierPSN(ulong _accountId)
		{
			this.AccountId = _accountId;
			this.ReadablePlatformUserIdentifier = this.AccountId.ToString();
			this.CombinedString = this.PlatformIdentifierString + "_" + this.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600D8A4 RID: 55460 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600D8A5 RID: 55461 RVA: 0x004DC5B6 File Offset: 0x004DA7B6
		public override bool Equals(PlatformUserIdentifierAbs _other)
		{
			return _other != null && (this == _other || (_other is UserIdentifierPSN && this.AccountId == (_other as UserIdentifierPSN).AccountId));
		}

		// Token: 0x0600D8A6 RID: 55462 RVA: 0x004DC5E0 File Offset: 0x004DA7E0
		public override int GetHashCode()
		{
			return this.AccountId.GetHashCode();
		}
	}
}
