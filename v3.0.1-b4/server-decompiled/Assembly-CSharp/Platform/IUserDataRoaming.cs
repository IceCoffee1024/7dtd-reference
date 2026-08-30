using System;

namespace Platform
{
	// Token: 0x02001B8E RID: 7054
	public interface IUserDataRoaming
	{
		// Token: 0x0600D1E3 RID: 53731
		void Init(IPlatform platform);

		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x0600D1E4 RID: 53732
		bool IsSupported { get; }

		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x0600D1E5 RID: 53733
		SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x0600D1E6 RID: 53734
		bool SaveRoamingEnabled { get; }

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x0600D1E7 RID: 53735
		bool IsRoamingOptional { get; }

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x0600D1E8 RID: 53736
		UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D1E9 RID: 53737
		void ValidateRoamingMode();
	}
}
