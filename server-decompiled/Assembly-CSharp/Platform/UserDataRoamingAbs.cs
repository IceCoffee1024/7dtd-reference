using System;

namespace Platform
{
	// Token: 0x02001BE8 RID: 7144
	public abstract class UserDataRoamingAbs : IUserDataRoaming
	{
		// Token: 0x0600D4EE RID: 54510 RVA: 0x004CC3A7 File Offset: 0x004CA5A7
		public void Init(IPlatform platform)
		{
			this.platform = platform;
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x0600D4EF RID: 54511 RVA: 0x004CC3B0 File Offset: 0x004CA5B0
		public bool IsSupported
		{
			get
			{
				return this.platform.SaveGameProvider != null;
			}
		}

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x0600D4F0 RID: 54512
		public abstract SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x0600D4F1 RID: 54513 RVA: 0x004CC3C0 File Offset: 0x004CA5C0
		public bool SaveRoamingEnabled
		{
			get
			{
				return this.IsSupported && (this.SaveRoamingMode == SaveRoamingMode.Optional || this.SaveRoamingMode == SaveRoamingMode.Forced);
			}
		}

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x0600D4F2 RID: 54514 RVA: 0x004CC3E0 File Offset: 0x004CA5E0
		public bool IsRoamingOptional
		{
			get
			{
				return this.SaveRoamingMode == SaveRoamingMode.Optional;
			}
		}

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x0600D4F3 RID: 54515
		public abstract UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D4F4 RID: 54516 RVA: 0x004CC3EB File Offset: 0x004CA5EB
		public void ValidateRoamingMode()
		{
			this.ValidateStoragePref(EnumGamePrefs.GameSaveStorageType);
			this.ValidateStoragePref(EnumGamePrefs.UserWorldStorageType);
		}

		// Token: 0x0600D4F5 RID: 54517 RVA: 0x004CC404 File Offset: 0x004CA604
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

		// Token: 0x0600D4F6 RID: 54518 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserDataRoamingAbs()
		{
		}

		// Token: 0x0400A1EC RID: 41452
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform platform;
	}
}
