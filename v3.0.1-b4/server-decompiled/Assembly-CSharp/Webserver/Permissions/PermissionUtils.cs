using System;

namespace Webserver.Permissions
{
	// Token: 0x02001ADD RID: 6877
	public static class PermissionUtils
	{
		// Token: 0x0600CE62 RID: 52834 RVA: 0x004B5100 File Offset: 0x004B3300
		public static bool CanViewAllPlayers(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallplayers", _permissionLevel);
		}

		// Token: 0x0600CE63 RID: 52835 RVA: 0x004B5112 File Offset: 0x004B3312
		public static bool CanViewAllClaims(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallclaims", _permissionLevel);
		}
	}
}
