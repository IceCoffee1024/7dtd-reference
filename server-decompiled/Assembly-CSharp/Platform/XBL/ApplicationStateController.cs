using System;

namespace Platform.XBL
{
	// Token: 0x02001C93 RID: 7315
	public class ApplicationStateController : IApplicationStateController
	{
		// Token: 0x14000132 RID: 306
		// (add) Token: 0x0600D8A6 RID: 55462 RVA: 0x004D94D4 File Offset: 0x004D76D4
		// (remove) Token: 0x0600D8A7 RID: 55463 RVA: 0x004D950C File Offset: 0x004D770C
		public event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x0600D8A8 RID: 55464 RVA: 0x004D9544 File Offset: 0x004D7744
		// (remove) Token: 0x0600D8A9 RID: 55465 RVA: 0x004D957C File Offset: 0x004D777C
		public event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x0600D8AA RID: 55466 RVA: 0x00010E62 File Offset: 0x0000F062
		public ApplicationState CurrentApplicationState
		{
			get
			{
				return ApplicationState.Foreground;
			}
		}

		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x0600D8AB RID: 55467 RVA: 0x00020119 File Offset: 0x0001E319
		public bool NetworkConnectionState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D8AC RID: 55468 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600D8AD RID: 55469 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D8AE RID: 55470 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}
	}
}
