using System;

namespace Platform
{
	// Token: 0x02001B8F RID: 7055
	public abstract class UserDataRoamingAbs : IUserDataRoaming
	{
		// Token: 0x0600D1EA RID: 53738 RVA: 0x004C14CB File Offset: 0x004BF6CB
		public void Init(IPlatform platform)
		{
			this.platform = platform;
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x0600D1EB RID: 53739 RVA: 0x004C14D4 File Offset: 0x004BF6D4
		public bool IsSupported
		{
			get
			{
				return this.platform.SaveGameProvider != null;
			}
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x0600D1EC RID: 53740
		public abstract SaveRoamingMode SaveRoamingMode { get; }

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x0600D1ED RID: 53741 RVA: 0x004C14E4 File Offset: 0x004BF6E4
		public bool SaveRoamingEnabled
		{
			get
			{
				return this.IsSupported && (this.SaveRoamingMode == SaveRoamingMode.Optional || this.SaveRoamingMode == SaveRoamingMode.Forced);
			}
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x0600D1EE RID: 53742 RVA: 0x004C1504 File Offset: 0x004BF704
		public bool IsRoamingOptional
		{
			get
			{
				return this.SaveRoamingMode == SaveRoamingMode.Optional;
			}
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x0600D1EF RID: 53743
		public abstract UserDataStorageType DefaultSaveStorage { get; }

		// Token: 0x0600D1F0 RID: 53744 RVA: 0x004C150F File Offset: 0x004BF70F
		public void ValidateRoamingMode()
		{
			this.ValidateStoragePref(EnumGamePrefs.GameSaveStorageType);
			this.ValidateStoragePref(EnumGamePrefs.UserWorldStorageType);
		}

		// Token: 0x0600D1F1 RID: 53745 RVA: 0x004C1528 File Offset: 0x004BF728
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

		// Token: 0x0600D1F2 RID: 53746 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserDataRoamingAbs()
		{
		}

		// Token: 0x04009FE6 RID: 40934
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform platform;
	}
}
