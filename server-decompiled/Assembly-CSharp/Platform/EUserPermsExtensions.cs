using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001BAE RID: 7086
	public static class EUserPermsExtensions
	{
		// Token: 0x0600D365 RID: 54117 RVA: 0x004C7805 File Offset: 0x004C5A05
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Multiplayer);
		}

		// Token: 0x0600D366 RID: 54118 RVA: 0x004C7818 File Offset: 0x004C5A18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCommunication(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Communication);
		}

		// Token: 0x0600D367 RID: 54119 RVA: 0x004C782B File Offset: 0x004C5A2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCrossplay(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Crossplay);
		}

		// Token: 0x0600D368 RID: 54120 RVA: 0x004C783E File Offset: 0x004C5A3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasHostMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.HostMultiplayer);
		}
	}
}
