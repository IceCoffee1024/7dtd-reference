using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001C11 RID: 7185
	public static class EUserPermsExtensions
	{
		// Token: 0x0600D5F1 RID: 54769 RVA: 0x004CFE15 File Offset: 0x004CE015
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Multiplayer);
		}

		// Token: 0x0600D5F2 RID: 54770 RVA: 0x004CFE28 File Offset: 0x004CE028
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCommunication(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Communication);
		}

		// Token: 0x0600D5F3 RID: 54771 RVA: 0x004CFE3B File Offset: 0x004CE03B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCrossplay(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.Crossplay);
		}

		// Token: 0x0600D5F4 RID: 54772 RVA: 0x004CFE4E File Offset: 0x004CE04E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasHostMultiplayer(this EUserPerms _perms)
		{
			return _perms.HasFlag(EUserPerms.HostMultiplayer);
		}
	}
}
