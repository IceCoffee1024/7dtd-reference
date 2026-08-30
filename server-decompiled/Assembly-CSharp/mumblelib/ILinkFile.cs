using System;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x0200169D RID: 5789
	public interface ILinkFile : IDisposable
	{
		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x0600B585 RID: 46469
		// (set) Token: 0x0600B584 RID: 46468
		uint UIVersion { get; set; }

		// Token: 0x0600B586 RID: 46470
		void Tick();

		// Token: 0x17001609 RID: 5641
		// (set) Token: 0x0600B587 RID: 46471
		Vector3 AvatarPosition { set; }

		// Token: 0x1700160A RID: 5642
		// (set) Token: 0x0600B588 RID: 46472
		Vector3 AvatarForward { set; }

		// Token: 0x1700160B RID: 5643
		// (set) Token: 0x0600B589 RID: 46473
		Vector3 AvatarTop { set; }

		// Token: 0x1700160C RID: 5644
		// (set) Token: 0x0600B58A RID: 46474
		string Name { set; }

		// Token: 0x1700160D RID: 5645
		// (set) Token: 0x0600B58B RID: 46475
		Vector3 CameraPosition { set; }

		// Token: 0x1700160E RID: 5646
		// (set) Token: 0x0600B58C RID: 46476
		Vector3 CameraForward { set; }

		// Token: 0x1700160F RID: 5647
		// (set) Token: 0x0600B58D RID: 46477
		Vector3 CameraTop { set; }

		// Token: 0x17001610 RID: 5648
		// (set) Token: 0x0600B58E RID: 46478
		string Identity { set; }

		// Token: 0x17001611 RID: 5649
		// (set) Token: 0x0600B58F RID: 46479
		string Context { set; }

		// Token: 0x17001612 RID: 5650
		// (set) Token: 0x0600B590 RID: 46480
		string Description { set; }
	}
}
