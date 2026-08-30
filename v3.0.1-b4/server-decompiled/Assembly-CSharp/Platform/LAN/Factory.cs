using System;
using UnityEngine.Scripting;

namespace Platform.LAN
{
	// Token: 0x02001CA6 RID: 7334
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.LAN)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600D956 RID: 55638 RVA: 0x004DD8C9 File Offset: 0x004DBAC9
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
