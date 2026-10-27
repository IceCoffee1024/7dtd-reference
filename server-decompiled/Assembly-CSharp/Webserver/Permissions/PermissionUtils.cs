using System;

namespace Webserver.Permissions
{
	// Token: 0x02001B61 RID: 7009
	public static class PermissionUtils
	{
		// Token: 0x0600D257 RID: 53847 RVA: 0x004C34E8 File Offset: 0x004C16E8
		public static bool CanViewAllPlayers(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallplayers", _permissionLevel);
		}

		// Token: 0x0600D258 RID: 53848 RVA: 0x004C34FA File Offset: 0x004C16FA
		public static bool CanViewAllClaims(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallclaims", _permissionLevel);
		}
	}
}
