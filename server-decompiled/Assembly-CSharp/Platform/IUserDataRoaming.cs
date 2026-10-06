using System;

namespace Platform
{
	// Token: 0x02001BE7 RID: 7143
	public interface IUserDataRoaming
	{
		// Token: 0x0600D4E7 RID: 54503
		void Init(IPlatform platform);

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x0600D4E8 RID: 54504
		bool IsSupported { get; }

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x0600D4E9 RID: 54505
		SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x0600D4EA RID: 54506
		bool SaveRoamingEnabled { get; }

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600D4EB RID: 54507
		bool IsRoamingOptional { get; }

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x0600D4EC RID: 54508
		UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D4ED RID: 54509
		void ValidateRoamingMode();
	}
}
