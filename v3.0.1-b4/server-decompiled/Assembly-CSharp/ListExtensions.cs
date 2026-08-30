using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

// Token: 0x02000377 RID: 887
public static class ListExtensions
{
	// Token: 0x06001A5C RID: 6748 RVA: 0x0009B69C File Offset: 0x0009989C
	public static T[] GetInternalArray<T>(this List<T> list)
	{
		return ListExtensions.ArrayAccessor<T>.Getter(list);
	}

	// Token: 0x02000378 RID: 888
	[PublicizedFrom(EAccessModifier.Private)]
	public static class ArrayAccessor<T>
	{
		// Token: 0x06001A5D RID: 6749 RVA: 0x0009B6AC File Offset: 0x000998AC
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

		// Token: 0x040010C7 RID: 4295
		public static Func<List<T>, T[]> Getter;
	}
}
