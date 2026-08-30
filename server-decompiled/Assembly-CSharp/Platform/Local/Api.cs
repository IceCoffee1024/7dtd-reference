using System;

namespace Platform.Local
{
	// Token: 0x02001CC6 RID: 7366
	public class Api : IPlatformApi
	{
		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x0600DAB4 RID: 55988 RVA: 0x004E3BA8 File Offset: 0x004E1DA8
		public EApiStatus ClientApiStatus { get; }

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x0600DAB5 RID: 55989 RVA: 0x004E3BB0 File Offset: 0x004E1DB0
		// (remove) Token: 0x0600DAB6 RID: 55990 RVA: 0x000027FC File Offset: 0x000009FC
		public event Action ClientApiInitialized
		{
			add
			{
				lock (this)
				{
					value();
				}
			}
			remove
			{
			}
		}

		// Token: 0x0600DAB7 RID: 55991 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DAB8 RID: 55992 RVA: 0x00020021 File Offset: 0x0001E221
		public bool InitClientApis()
		{
			return true;
		}

		// Token: 0x0600DAB9 RID: 55993 RVA: 0x00020021 File Offset: 0x0001E221
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600DABA RID: 55994 RVA: 0x000027FC File Offset: 0x000009FC
		public void ServerApiLoaded()
		{
		}

		// Token: 0x0600DABB RID: 55995 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DABC RID: 55996 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DABD RID: 55997 RVA: 0x00040F84 File Offset: 0x0003F184
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}
	}
}
