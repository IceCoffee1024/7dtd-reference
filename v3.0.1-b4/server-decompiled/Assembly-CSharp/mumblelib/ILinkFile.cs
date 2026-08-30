using System;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x02001677 RID: 5751
	public interface ILinkFile : IDisposable
	{
		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x0600B402 RID: 46082
		// (set) Token: 0x0600B401 RID: 46081
		uint UIVersion { get; set; }

		// Token: 0x0600B403 RID: 46083
		void Tick();

		// Token: 0x17001597 RID: 5527
		// (set) Token: 0x0600B404 RID: 46084
		Vector3 AvatarPosition { set; }

		// Token: 0x17001598 RID: 5528
		// (set) Token: 0x0600B405 RID: 46085
		Vector3 AvatarForward { set; }

		// Token: 0x17001599 RID: 5529
		// (set) Token: 0x0600B406 RID: 46086
		Vector3 AvatarTop { set; }

		// Token: 0x1700159A RID: 5530
		// (set) Token: 0x0600B407 RID: 46087
		string Name { set; }

		// Token: 0x1700159B RID: 5531
		// (set) Token: 0x0600B408 RID: 46088
		Vector3 CameraPosition { set; }

		// Token: 0x1700159C RID: 5532
		// (set) Token: 0x0600B409 RID: 46089
		Vector3 CameraForward { set; }

		// Token: 0x1700159D RID: 5533
		// (set) Token: 0x0600B40A RID: 46090
		Vector3 CameraTop { set; }

		// Token: 0x1700159E RID: 5534
		// (set) Token: 0x0600B40B RID: 46091
		string Identity { set; }

		// Token: 0x1700159F RID: 5535
		// (set) Token: 0x0600B40C RID: 46092
		string Context { set; }

		// Token: 0x170015A0 RID: 5536
		// (set) Token: 0x0600B40D RID: 46093
		string Description { set; }
	}
}
