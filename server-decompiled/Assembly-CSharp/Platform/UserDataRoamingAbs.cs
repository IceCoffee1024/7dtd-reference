using System;

namespace Platform
{
	// Token: 0x02001BB6 RID: 7094
	public abstract class UserDataRoamingAbs : IUserDataRoaming
	{
		// Token: 0x0600D376 RID: 54134 RVA: 0x004C78BF File Offset: 0x004C5ABF
		public void Init(IPlatform platform)
		{
			this.platform = platform;
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x0600D377 RID: 54135 RVA: 0x004C78C8 File Offset: 0x004C5AC8
		public bool IsSupported
		{
			get
			{
				return this.platform.SaveGameProvider != null;
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x0600D378 RID: 54136
		public abstract SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x0600D379 RID: 54137 RVA: 0x004C78D8 File Offset: 0x004C5AD8
		public bool SaveRoamingEnabled
		{
			get
			{
				return this.IsSupported && (this.SaveRoamingMode == SaveRoamingMode.Optional || this.SaveRoamingMode == SaveRoamingMode.Forced);
			}
		}

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x0600D37A RID: 54138 RVA: 0x004C78F8 File Offset: 0x004C5AF8
		public bool IsRoamingOptional
		{
			get
			{
				return this.SaveRoamingMode == SaveRoamingMode.Optional;
			}
		}

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x0600D37B RID: 54139
		public abstract UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D37C RID: 54140 RVA: 0x004C7903 File Offset: 0x004C5B03
		public void ValidateRoamingMode()
		{
			this.ValidateStoragePref(EnumGamePrefs.GameSaveStorageType);
			this.ValidateStoragePref(EnumGamePrefs.UserWorldStorageType);
		}

		// Token: 0x0600D37D RID: 54141 RVA: 0x004C791C File Offset: 0x004C5B1C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ValidateStoragePref(EnumGamePrefs pref)
		{
			UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(pref);
			UserDataStorageType userDataStorageType = @int;
			SaveRoamingMode saveRoamingMode = this.SaveRoamingMode;
			if (saveRoamingMode != SaveRoamingMode.None)
			{
				if (saveRoamingMode == SaveRoamingMode.Forced)
				{
					userDataStorageType = UserDataStorageType.Roaming;
				}
			}
			else
			{
				userDataStorageType = UserDataStorageType.DeviceLocal;
			}
			if (@int != userDataStorageType)
			{
				Log.Out(string.Format("UserDataRoaming invalid storage pref {0} configured for {1}. Platform roaming mode is {2}. Changing to {3}", new object[]
				{
					@int,
					pref.ToStringCached<EnumGamePrefs>(),
					this.SaveRoamingMode,
					userDataStorageType
				}));
				GamePrefs.Set(pref, (int)userDataStorageType);
			}
		}

		// Token: 0x0600D37E RID: 54142 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserDataRoamingAbs()
		{
		}

		// Token: 0x0400A117 RID: 41239
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform platform;
	}
}
