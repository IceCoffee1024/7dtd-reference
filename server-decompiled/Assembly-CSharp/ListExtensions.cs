using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

// Token: 0x02000380 RID: 896
public static class ListExtensions
{
	// Token: 0x06001A78 RID: 6776 RVA: 0x0009C290 File Offset: 0x0009A490
	public static T[] GetInternalArray<T>(this List<T> list)
	{
		return ListExtensions.ArrayAccessor<T>.Getter(list);
	}

	// Token: 0x02000381 RID: 897
	[PublicizedFrom(EAccessModifier.Private)]
	public static class ArrayAccessor<T>
	{
		// Token: 0x06001A79 RID: 6777 RVA: 0x0009C2A0 File Offset: 0x0009A4A0
		[PublicizedFrom(EAccessModifier.Private)]
		static ArrayAccessor()
		{
			DynamicMethod dynamicMethod = new DynamicMethod("get", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, typeof(T[]), new Type[] { typeof(List<T>) }, typeof(ListExtensions.ArrayAccessor<T>), true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, typeof(List<T>).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic));
			ilgenerator.Emit(OpCodes.Ret);
			ListExtensions.ArrayAccessor<T>.Getter = (Func<List<T>, T[]>)dynamicMethod.CreateDelegate(typeof(Func<List<T>, T[]>));
		}

		// Token: 0x040010E5 RID: 4325
		public static Func<List<T>, T[]> Getter;
	}
}
