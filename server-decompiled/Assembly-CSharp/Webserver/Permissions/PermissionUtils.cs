using System;

namespace Webserver.Permissions
{
	// Token: 0x02001B30 RID: 6960
	public static class PermissionUtils
	{
		// Token: 0x0600D143 RID: 53571 RVA: 0x004BF9C0 File Offset: 0x004BDBC0
		public static bool CanViewAllPlayers(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallplayers", _permissionLevel);
		}

		// Token: 0x0600D144 RID: 53572 RVA: 0x004BF9D2 File Offset: 0x004BDBD2
		public static bool CanViewAllClaims(int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webapi.viewallclaims", _permissionLevel);
		}
	}
}
