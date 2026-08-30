using System;

namespace HarmonyLib
{
	// Token: 0x0200000C RID: 12
	// (Invoke) Token: 0x0600001F RID: 31
	[Obsolete("Use AccessTools.FieldRefAccess<T, S> for fields and AccessTools.MethodDelegate<Func<T, S>> for property getters")]
	public delegate S GetterHandler<in T, out S>(T source);
}
