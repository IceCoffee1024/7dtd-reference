using System;

namespace Platform
{
	// Token: 0x02001BB5 RID: 7093
	public interface IUserDataRoaming
	{
		// Token: 0x0600D36F RID: 54127
		void Init(IPlatform platform);

		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x0600D370 RID: 54128
		bool IsSupported { get; }

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x0600D371 RID: 54129
		SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x0600D372 RID: 54130
		bool SaveRoamingEnabled { get; }

		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x0600D373 RID: 54131
		bool IsRoamingOptional { get; }

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x0600D374 RID: 54132
		UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D375 RID: 54133
		void ValidateRoamingMode();
	}
}
