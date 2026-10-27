using System;
using UnityEngine.Scripting;

namespace Platform.LAN
{
	// Token: 0x02001D30 RID: 7472
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.LAN)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DD80 RID: 56704 RVA: 0x004EC5C5 File Offset: 0x004EA7C5
		public override void CreateInstances()
		{
			if (!base.AsServerOnly)
			{
				throw new NotSupportedException("This platform can only be used as a server platform.");
			}
			base.ServerListAnnouncer = new LANMasterServerAnnouncer();
		}
	}
}
