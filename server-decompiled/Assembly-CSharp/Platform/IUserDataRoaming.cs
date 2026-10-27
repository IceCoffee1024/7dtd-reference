using System;

namespace Platform
{
	// Token: 0x02001C18 RID: 7192
	public interface IUserDataRoaming
	{
		// Token: 0x0600D5FB RID: 54779
		void Init(IPlatform platform);

		// Token: 0x17001B12 RID: 6930
		// (get) Token: 0x0600D5FC RID: 54780
		bool IsSupported { get; }

		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x0600D5FD RID: 54781
		SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x0600D5FE RID: 54782
		bool SaveRoamingEnabled { get; }

		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x0600D5FF RID: 54783
		bool IsRoamingOptional { get; }

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x0600D600 RID: 54784
		UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D601 RID: 54785
		void ValidateRoamingMode();
	}
}
