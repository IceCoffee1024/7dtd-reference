using System;
using UnityEngine.Scripting;

namespace Platform.PSN
{
	// Token: 0x02001CEC RID: 7404
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierPSN : PlatformUserIdentifierAbs
	{
		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x0600DBAF RID: 56239 RVA: 0x000809CD File Offset: 0x0007EBCD
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.PSN;
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x0600DBB0 RID: 56240 RVA: 0x004E767B File Offset: 0x004E587B
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.PSN);

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x0600DBB1 RID: 56241 RVA: 0x004E7683 File Offset: 0x004E5883
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x0600DBB2 RID: 56242 RVA: 0x004E768B File Offset: 0x004E588B
		public override string CombinedString { get; }

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x0600DBB3 RID: 56243 RVA: 0x004E7693 File Offset: 0x004E5893
		// (set) Token: 0x0600DBB4 RID: 56244 RVA: 0x004E769B File Offset: 0x004E589B
		public ulong AccountId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600DBB5 RID: 56245 RVA: 0x004E76A4 File Offset: 0x004E58A4
		public UserIdentifierPSN(ulong _accountId)
		{
			this.AccountId = _accountId;
			this.ReadablePlatformUserIdentifier = this.AccountId.ToString();
			this.CombinedString = this.PlatformIdentifierString + "_" + this.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600DBB6 RID: 56246 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600DBB7 RID: 56247 RVA: 0x004E76FA File Offset: 0x004E58FA
		public override bool Equals(PlatformUserIdentifierAbs _other)
		{
			return _other != null && (this == _other || (_other is UserIdentifierPSN && this.AccountId == (_other as UserIdentifierPSN).AccountId));
		}

		// Token: 0x0600DBB8 RID: 56248 RVA: 0x004E7724 File Offset: 0x004E5924
		public override int GetHashCode()
		{
			return this.AccountId.GetHashCode();
		}
	}
}
