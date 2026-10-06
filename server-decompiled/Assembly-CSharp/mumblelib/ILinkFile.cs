using System;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016C9 RID: 5833
	public interface ILinkFile : IDisposable
	{
		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x0600B6DC RID: 46812
		// (set) Token: 0x0600B6DB RID: 46811
		uint UIVersion { get; set; }

		// Token: 0x0600B6DD RID: 46813
		void Tick();

		// Token: 0x170016AB RID: 5803
		// (set) Token: 0x0600B6DE RID: 46814
		Vector3 AvatarPosition { set; }

		// Token: 0x170016AC RID: 5804
		// (set) Token: 0x0600B6DF RID: 46815
		Vector3 AvatarForward { set; }

		// Token: 0x170016AD RID: 5805
		// (set) Token: 0x0600B6E0 RID: 46816
		Vector3 AvatarTop { set; }

		// Token: 0x170016AE RID: 5806
		// (set) Token: 0x0600B6E1 RID: 46817
		string Name { set; }

		// Token: 0x170016AF RID: 5807
		// (set) Token: 0x0600B6E2 RID: 46818
		Vector3 CameraPosition { set; }

		// Token: 0x170016B0 RID: 5808
		// (set) Token: 0x0600B6E3 RID: 46819
		Vector3 CameraForward { set; }

		// Token: 0x170016B1 RID: 5809
		// (set) Token: 0x0600B6E4 RID: 46820
		Vector3 CameraTop { set; }

		// Token: 0x170016B2 RID: 5810
		// (set) Token: 0x0600B6E5 RID: 46821
		string Identity { set; }

		// Token: 0x170016B3 RID: 5811
		// (set) Token: 0x0600B6E6 RID: 46822
		string Context { set; }

		// Token: 0x170016B4 RID: 5812
		// (set) Token: 0x0600B6E7 RID: 46823
		string Description { set; }
	}
}
