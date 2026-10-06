using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001BE0 RID: 7136
	public static class EUserPermsExtensions
	{
		// Token: 0x0600D4DD RID: 54493 RVA: 0x004CC2ED File Offset: 0x004CA4ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Multiplayer);
		}

		// Token: 0x0600D4DE RID: 54494 RVA: 0x004CC300 File Offset: 0x004CA500
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCommunication(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Communication);
		}

		// Token: 0x0600D4DF RID: 54495 RVA: 0x004CC313 File Offset: 0x004CA513
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCrossplay(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Crossplay);
		}

		// Token: 0x0600D4E0 RID: 54496 RVA: 0x004CC326 File Offset: 0x004CA526
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasHostMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.HostMultiplayer);
		}
	}
}
