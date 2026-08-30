using System;

namespace HarmonyLib
{
	// Token: 0x0200000D RID: 13
	// (Invoke) Token: 0x06000023 RID: 35
	[Obsolete("Use AccessTools.FieldRefAccess<T, S> for fields and AccessTools.MethodDelegate<Action<T, S>> for property setters")]
	public delegate void SetterHandler<in T, in S>(T source, S value);
}
