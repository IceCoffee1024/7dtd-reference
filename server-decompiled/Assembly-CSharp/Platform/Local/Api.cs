using System;

namespace Platform.Local
{
	// Token: 0x02001D29 RID: 7465
	public class Api : IPlatformApi
	{
		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x0600DD42 RID: 56642 RVA: 0x004EC290 File Offset: 0x004EA490
		public EApiStatus ClientApiStatus { get; }

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x0600DD43 RID: 56643 RVA: 0x004EC298 File Offset: 0x004EA498
		// (remove) Token: 0x0600DD44 RID: 56644 RVA: 0x000027FC File Offset: 0x000009FC
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

		// Token: 0x0600DD45 RID: 56645 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DD46 RID: 56646 RVA: 0x00020119 File Offset: 0x0001E319
		public bool InitClientApis()
		{
			return true;
		}

		// Token: 0x0600DD47 RID: 56647 RVA: 0x00020119 File Offset: 0x0001E319
		public bool InitServerApis()
		{
			return true;
		}

		// Token: 0x0600DD48 RID: 56648 RVA: 0x000027FC File Offset: 0x000009FC
		public void ServerApiLoaded()
		{
		}

		// Token: 0x0600DD49 RID: 56649 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DD4A RID: 56650 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DD4B RID: 56651 RVA: 0x00041024 File Offset: 0x0003F224
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}
	}
}
