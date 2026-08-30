using System;

namespace Platform.XBL
{
	// Token: 0x02001C30 RID: 7216
	public class ApplicationStateController : IApplicationStateController
	{
		// Token: 0x14000133 RID: 307
		// (add) Token: 0x0600D61A RID: 54810 RVA: 0x004D0EC4 File Offset: 0x004CF0C4
		// (remove) Token: 0x0600D61B RID: 54811 RVA: 0x004D0EFC File Offset: 0x004CF0FC
		public event IApplicationStateController.ApplicationStateChanged OnApplicationStateChanged;

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x0600D61C RID: 54812 RVA: 0x004D0F34 File Offset: 0x004CF134
		// (remove) Token: 0x0600D61D RID: 54813 RVA: 0x004D0F6C File Offset: 0x004CF16C
		public event IApplicationStateController.NetworkStateChanged OnNetworkStateChanged;

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x0600D61E RID: 54814 RVA: 0x00010E62 File Offset: 0x0000F062
		public ApplicationState CurrentApplicationState
		{
			get
			{
				return ApplicationState.Foreground;
			}
		}

		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x0600D61F RID: 54815 RVA: 0x00020021 File Offset: 0x0001E221
		public bool NetworkConnectionState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D620 RID: 54816 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600D621 RID: 54817 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D622 RID: 54818 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}
	}
}
