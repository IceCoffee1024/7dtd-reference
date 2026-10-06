using System;

namespace Platform.XBL
{
	// Token: 0x02001C62 RID: 7266
	public class ApplicationStateController : IApplicationStateController
	{
		// Token: 0x14000132 RID: 306
		// (add) Token: 0x0600D792 RID: 55186 RVA: 0x004D59AC File Offset: 0x004D3BAC
		// (remove) Token: 0x0600D793 RID: 55187 RVA: 0x004D59E4 File Offset: 0x004D3BE4
		public event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x0600D794 RID: 55188 RVA: 0x004D5A1C File Offset: 0x004D3C1C
		// (remove) Token: 0x0600D795 RID: 55189 RVA: 0x004D5A54 File Offset: 0x004D3C54
		public event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x0600D796 RID: 55190 RVA: 0x00010E62 File Offset: 0x0000F062
		public ApplicationState CurrentApplicationState
		{
			get
			{
				return ApplicationState.Foreground;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x0600D797 RID: 55191 RVA: 0x00020119 File Offset: 0x0001E319
		public bool NetworkConnectionState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D798 RID: 55192 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600D799 RID: 55193 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D79A RID: 55194 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}
	}
}
