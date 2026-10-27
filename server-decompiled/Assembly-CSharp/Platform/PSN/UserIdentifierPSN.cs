using System;
using UnityEngine.Scripting;

namespace Platform.PSN
{
	// Token: 0x02001D1D RID: 7453
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierPSN : PlatformUserIdentifierAbs
	{
		// Token: 0x17001C0C RID: 7180
		// (get) Token: 0x0600DCC3 RID: 56515 RVA: 0x00080BF2 File Offset: 0x0007EDF2
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.PSN;
			}
		}

		// Token: 0x17001C0D RID: 7181
		// (get) Token: 0x0600DCC4 RID: 56516 RVA: 0x004EB21B File Offset: 0x004E941B
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.PSN);

		// Token: 0x17001C0E RID: 7182
		// (get) Token: 0x0600DCC5 RID: 56517 RVA: 0x004EB223 File Offset: 0x004E9423
		public override string ReadablePlatformUserIdentifier { get; }

		// Token: 0x17001C0F RID: 7183
		// (get) Token: 0x0600DCC6 RID: 56518 RVA: 0x004EB22B File Offset: 0x004E942B
		public override string CombinedString { get; }

		// Token: 0x17001C10 RID: 7184
		// (get) Token: 0x0600DCC7 RID: 56519 RVA: 0x004EB233 File Offset: 0x004E9433
		// (set) Token: 0x0600DCC8 RID: 56520 RVA: 0x004EB23B File Offset: 0x004E943B
		public ulong AccountId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600DCC9 RID: 56521 RVA: 0x004EB244 File Offset: 0x004E9444
		public UserIdentifierPSN(ulong _accountId)
		{
			this.AccountId = _accountId;
			this.ReadablePlatformUserIdentifier = this.AccountId.ToString();
			this.CombinedString = this.PlatformIdentifierString + "_" + this.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600DCCA RID: 56522 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool DecodeTicket(string _ticket)
		{
			return true;
		}

		// Token: 0x0600DCCB RID: 56523 RVA: 0x004EB29A File Offset: 0x004E949A
		public override bool Equals(PlatformUserIdentifierAbs _other)
		{
			return _other != null && (this == _other || (_other is UserIdentifierPSN && this.AccountId == (_other as UserIdentifierPSN).AccountId));
		}

		// Token: 0x0600DCCC RID: 56524 RVA: 0x004EB2C4 File Offset: 0x004E94C4
		public override int GetHashCode()
		{
			return this.AccountId.GetHashCode();
		}
	}
}
