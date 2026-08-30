using System;

namespace Platform.Local
{
	// Token: 0x02001C9F RID: 7327
	public class Api : IPlatformApi
	{
		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x0600D91C RID: 55580 RVA: 0x004DD5AC File Offset: 0x004DB7AC
		public EApiStatus ClientApiStatus { get; }

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x0600D91D RID: 55581 RVA: 0x004DD5B4 File Offset: 0x004DB7B4
		// (remove) Token: 0x0600D91E RID: 55582 RVA: 0x000027FC File Offset: 0x000009FC
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

		// Token: 0x0600D91F RID: 55583 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D920 RID: 55584 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool InitClientApis()
		{
			return true;
		}

		// Token: 0x0600D921 RID: 55585 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600D922 RID: 55586 RVA: 0x000027FC File Offset: 0x000009FC
		public void ServerApiLoaded()
		{
		}

		// Token: 0x0600D923 RID: 55587 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600D924 RID: 55588 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D925 RID: 55589 RVA: 0x00040D24 File Offset: 0x0003EF24
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}
	}
}
