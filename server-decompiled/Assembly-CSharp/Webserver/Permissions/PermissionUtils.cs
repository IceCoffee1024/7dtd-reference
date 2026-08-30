using System;

namespace Webserver.Permissions
{
	// Token: 0x02001B04 RID: 6916
	public static class PermissionUtils
	{
		// Token: 0x0600CFEB RID: 53227 RVA: 0x004BB4F4 File Offset: 0x004B96F4
		public static bool CanViewAllPlayers(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallplayers", _permissionLevel);
		}

		// Token: 0x0600CFEC RID: 53228 RVA: 0x004BB506 File Offset: 0x004B9706
		public static bool CanViewAllClaims(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallclaims", _permissionLevel);
		}
	}
}
