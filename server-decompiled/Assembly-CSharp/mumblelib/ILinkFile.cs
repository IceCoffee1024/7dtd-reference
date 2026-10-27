using System;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x020016E8 RID: 5864
	public interface ILinkFile : IDisposable
	{
		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x0600B7B0 RID: 47024
		// (set) Token: 0x0600B7AF RID: 47023
		uint UIVersion { get; set; }

		// Token: 0x0600B7B1 RID: 47025
		void Tick();

		// Token: 0x170016F2 RID: 5874
		// (set) Token: 0x0600B7B2 RID: 47026
		Vector3 AvatarPosition { set; }

		// Token: 0x170016F3 RID: 5875
		// (set) Token: 0x0600B7B3 RID: 47027
		Vector3 AvatarForward { set; }

		// Token: 0x170016F4 RID: 5876
		// (set) Token: 0x0600B7B4 RID: 47028
		Vector3 AvatarTop { set; }

		// Token: 0x170016F5 RID: 5877
		// (set) Token: 0x0600B7B5 RID: 47029
		string Name { set; }

		// Token: 0x170016F6 RID: 5878
		// (set) Token: 0x0600B7B6 RID: 47030
		Vector3 CameraPosition { set; }

		// Token: 0x170016F7 RID: 5879
		// (set) Token: 0x0600B7B7 RID: 47031
		Vector3 CameraForward { set; }

		// Token: 0x170016F8 RID: 5880
		// (set) Token: 0x0600B7B8 RID: 47032
		Vector3 CameraTop { set; }

		// Token: 0x170016F9 RID: 5881
		// (set) Token: 0x0600B7B9 RID: 47033
		string Identity { set; }

		// Token: 0x170016FA RID: 5882
		// (set) Token: 0x0600B7BA RID: 47034
		string Context { set; }

		// Token: 0x170016FB RID: 5883
		// (set) Token: 0x0600B7BB RID: 47035
		string Description { set; }
	}
}
