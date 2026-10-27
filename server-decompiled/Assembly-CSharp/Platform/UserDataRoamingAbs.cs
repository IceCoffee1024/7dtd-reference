using System;

namespace Platform
{
	// Token: 0x02001C19 RID: 7193
	public abstract class UserDataRoamingAbs : IUserDataRoaming
	{
		// Token: 0x0600D602 RID: 54786 RVA: 0x004CFECF File Offset: 0x004CE0CF
		public void Init(IPlatform platform)
		{
			this.platform = platform;
		}

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x0600D603 RID: 54787 RVA: 0x004CFED8 File Offset: 0x004CE0D8
		public bool IsSupported
		{
			get
			{
				return this.platform.SaveGameProvider != null;
			}
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x0600D604 RID: 54788
		public abstract SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x0600D605 RID: 54789 RVA: 0x004CFEE8 File Offset: 0x004CE0E8
		public bool SaveRoamingEnabled
		{
			get
			{
				return this.IsSupported && (this.SaveRoamingMode == SaveRoamingMode.Optional || this.SaveRoamingMode == SaveRoamingMode.Forced);
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x0600D606 RID: 54790 RVA: 0x004CFF08 File Offset: 0x004CE108
		public bool IsRoamingOptional
		{
			get
			{
				return this.SaveRoamingMode == SaveRoamingMode.Optional;
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x0600D607 RID: 54791
		public abstract UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D608 RID: 54792 RVA: 0x004CFF13 File Offset: 0x004CE113
		public void ValidateRoamingMode()
		{
			this.ValidateStoragePref(EnumGamePrefs.GameSaveStorageType);
			this.ValidateStoragePref(EnumGamePrefs.UserWorldStorageType);
		}

		// Token: 0x0600D609 RID: 54793 RVA: 0x004CFF2C File Offset: 0x004CE12C
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

		// Token: 0x0600D60A RID: 54794 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserDataRoamingAbs()
		{
		}

		// Token: 0x0400A2D5 RID: 41685
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform platform;
	}
}
