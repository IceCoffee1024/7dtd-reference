using System;
using UnityEngine.Scripting;

namespace Platform.LAN
{
	// Token: 0x02001CCD RID: 7373
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.LAN)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DAF1 RID: 56049 RVA: 0x004E3EDD File Offset: 0x004E20DD
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
