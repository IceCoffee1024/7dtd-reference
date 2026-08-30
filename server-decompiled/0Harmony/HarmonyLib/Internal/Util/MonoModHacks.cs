using System;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace HarmonyLib.Internal.Util
{
	// Token: 0x02000064 RID: 100
	internal static class MonoModHacks
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x00015548 File Offset: 0x00013748
		internal unsafe static MethodBase GetEndOfChain(this ILHook self)
		{
			return *MonoModHacks.ManagedDetourState_EndOfChain_Ref(*MonoModHacks.ILHook_State_Ref(self));
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00015561 File Offset: 0x00013761
		internal unsafe static MethodBase GetEndOfChain(this MethodDetourInfo self)
		{
			return *MonoModHacks.ManagedDetourState_EndOfChain_Ref(*MonoModHacks.MethodDetourInfo_State_Ref(self));
		}

		// Token: 0x04000119 RID: 281
		private static readonly AccessTools.FieldRef<object, MethodBase> ManagedDetourState_EndOfChain_Ref = AccessTools.FieldRefAccess<object, MethodBase>(AccessTools.DeclaredField(typeof(DetourManager).GetNestedType("ManagedDetourState", AccessTools.all), "EndOfChain"));

		// Token: 0x0400011A RID: 282
		private static readonly AccessTools.FieldRef<MethodDetourInfo, object> MethodDetourInfo_State_Ref = AccessTools.FieldRefAccess<MethodDetourInfo, object>("state");

		// Token: 0x0400011B RID: 283
		private static readonly AccessTools.FieldRef<ILHook, object> ILHook_State_Ref = AccessTools.FieldRefAccess<ILHook, object>("state");
	}
}
