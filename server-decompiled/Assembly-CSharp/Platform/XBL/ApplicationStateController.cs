using System;

namespace Platform.XBL
{
	// Token: 0x02001C09 RID: 7177
	public class ApplicationStateController : IApplicationStateController
	{
		// Token: 0x14000132 RID: 306
		// (add) Token: 0x0600D48C RID: 54412 RVA: 0x004CAAD0 File Offset: 0x004C8CD0
		// (remove) Token: 0x0600D48D RID: 54413 RVA: 0x004CAB08 File Offset: 0x004C8D08
		public event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x0600D48E RID: 54414 RVA: 0x004CAB40 File Offset: 0x004C8D40
		// (remove) Token: 0x0600D48F RID: 54415 RVA: 0x004CAB78 File Offset: 0x004C8D78
		public event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x0600D490 RID: 54416 RVA: 0x00010E62 File Offset: 0x0000F062
		public ApplicationState CurrentApplicationState
		{
			get
			{
				return ApplicationState.Foreground;
			}
		}

		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x0600D491 RID: 54417 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool NetworkConnectionState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D492 RID: 54418 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600D493 RID: 54419 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D494 RID: 54420 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}
	}
}
