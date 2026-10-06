using System;
using UnityEngine.Scripting;

namespace Platform.LAN
{
	// Token: 0x02001CFF RID: 7423
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.LAN)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600DC6C RID: 56428 RVA: 0x004E8A25 File Offset: 0x004E6C25
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
