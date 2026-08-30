using System;
using System.Collections.Generic;
using Platform.LAN;
using Platform.Shared;
using UnityEngine.Scripting;

namespace Platform.Local
{
	// Token: 0x02001CA1 RID: 7329
	[Preserve]
	[PlatformFactory(EPlatformIdentifier.Local)]
	public class Factory : AbsPlatform
	{
		// Token: 0x0600D92D RID: 55597 RVA: 0x004DD5F0 File Offset: 0x004DB7F0
		public override void CreateInstances()
		{
			base.Api = new Api();
			if (!base.AsServerOnly)
			{
				base.User = new User();
				LocalServerDetect localServerDetect = new LocalServerDetect();
				base.ServerListInterfaces = new List<IServerListInterface>
				{
					localServerDetect,
					new FavoriteServers(),
					new LANServerList()
				};
				base.ServerLookupInterface = localServerDetect;
				base.Utils = new Platform.Shared.Utils();
				base.Input = new PlayerInputManager();
				base.RemoteFileStorage = new RemoteFileStorage();
				base.EntitlementValidators = new List<IEntitlementValidator>
				{
					new DownloadableContentValidator()
				};
			}
		}
	}
}
