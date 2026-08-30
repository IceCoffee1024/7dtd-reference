using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B87 RID: 7047
	public static class EUserPermsExtensions
	{
		// Token: 0x0600D1D9 RID: 53721 RVA: 0x004C1411 File Offset: 0x004BF611
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Multiplayer);
		}

		// Token: 0x0600D1DA RID: 53722 RVA: 0x004C1424 File Offset: 0x004BF624
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCommunication(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Communication);
		}

		// Token: 0x0600D1DB RID: 53723 RVA: 0x004C1437 File Offset: 0x004BF637
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCrossplay(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Crossplay);
		}

		// Token: 0x0600D1DC RID: 53724 RVA: 0x004C144A File Offset: 0x004BF64A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasHostMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.HostMultiplayer);
		}
	}
}
