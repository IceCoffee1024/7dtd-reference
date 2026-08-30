using System;
using UnityEngine.Scripting;

namespace Platform.PSN
{
	// Token: 0x02001CBA RID: 7354
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierPSN : PlatformUserIdentifierAbs
	{
		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x0600DA35 RID: 55861 RVA: 0x00080719 File Offset: 0x0007E919
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.PSN;
			}
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x0600DA36 RID: 55862 RVA: 0x004E2B33 File Offset: 0x004E0D33
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.PSN);

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x0600DA37 RID: 55863 RVA: 0x004E2B3B File Offset: 0x004E0D3B
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x0600DA38 RID: 55864 RVA: 0x004E2B43 File Offset: 0x004E0D43
		public override string CombinedString { get; }

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x0600DA39 RID: 55865 RVA: 0x004E2B4B File Offset: 0x004E0D4B
		// (set) Token: 0x0600DA3A RID: 55866 RVA: 0x004E2B53 File Offset: 0x004E0D53
		public ulong AccountId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600DA3B RID: 55867 RVA: 0x004E2B5C File Offset: 0x004E0D5C
		public UserIdentifierPSN(ulong _accountId)
		{
			this.AccountId = _accountId;
			this.ReadablePlatformUserIdentifier = this.AccountId.ToString();
			this.CombinedString = this.PlatformIdentifierString + "_" + this.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600DA3C RID: 55868 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600DA3D RID: 55869 RVA: 0x004E2BB2 File Offset: 0x004E0DB2
		public override bool Equals(PlatformUserIdentifierAbs _other)
		{
			return _other != null && (this == _other || (_other is UserIdentifierPSN && this.AccountId == (_other as UserIdentifierPSN).AccountId));
		}

		// Token: 0x0600DA3E RID: 55870 RVA: 0x004E2BDC File Offset: 0x004E0DDC
		public override int GetHashCode()
		{
			return this.AccountId.GetHashCode();
		}
	}
}
