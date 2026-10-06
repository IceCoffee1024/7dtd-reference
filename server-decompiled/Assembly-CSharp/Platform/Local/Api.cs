using System;

namespace Platform.Local
{
	// Token: 0x02001CF8 RID: 7416
	public class Api : IPlatformApi
	{
		// Token: 0x17001BF8 RID: 7160
		// (get) Token: 0x0600DC2E RID: 56366 RVA: 0x004E86F0 File Offset: 0x004E68F0
		public EApiStatus ClientApiStatus { get; }

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x0600DC2F RID: 56367 RVA: 0x004E86F8 File Offset: 0x004E68F8
		// (remove) Token: 0x0600DC30 RID: 56368 RVA: 0x000027FC File Offset: 0x000009FC
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

		// Token: 0x0600DC31 RID: 56369 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DC32 RID: 56370 RVA: 0x00020119 File Offset: 0x0001E319
		public bool InitClientApis()
		{
			return true;
		}

		// Token: 0x0600DC33 RID: 56371 RVA: 0x00020119 File Offset: 0x0001E319
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600DC34 RID: 56372 RVA: 0x000027FC File Offset: 0x000009FC
		public void ServerApiLoaded()
		{
		}

		// Token: 0x0600DC35 RID: 56373 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DC36 RID: 56374 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DC37 RID: 56375 RVA: 0x00041064 File Offset: 0x0003F264
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}
	}
}
