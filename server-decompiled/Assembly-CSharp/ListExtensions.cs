using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

// Token: 0x0200037E RID: 894
public static class ListExtensions
{
	// Token: 0x06001A74 RID: 6772 RVA: 0x0009C05C File Offset: 0x0009A25C
	public static T[] GetInternalArray<T>(this List<T> list)
	{
		return ListExtensions.ArrayAccessor<T>.Getter(list);
	}

	// Token: 0x0200037F RID: 895
	[PublicizedFrom(EAccessModifier.Private)]
	public static class ArrayAccessor<T>
	{
		// Token: 0x06001A75 RID: 6773 RVA: 0x0009C06C File Offset: 0x0009A26C
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

		// Token: 0x040010D5 RID: 4309
		public static Func<List<T>, T[]> Getter;
	}
}
