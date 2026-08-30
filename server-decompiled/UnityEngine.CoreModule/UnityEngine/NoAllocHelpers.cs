using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000252 RID: 594
	[NativeHeader("Runtime/Export/Scripting/NoAllocHelpers.bindings.h")]
	internal sealed class NoAllocHelpers
	{
		// Token: 0x0600195B RID: 6491 RVA: 0x0002A5B0 File Offset: 0x000287B0
		public static void ResizeList<T>(List<T> list, int size)
		{
			bool flag = list == null;
			if (flag)
			{
				throw new ArgumentNullException("list");
			}
			bool flag2 = size < 0 || size > list.Capacity;
			if (flag2)
			{
				throw new ArgumentException("invalid size to resize.", "list");
			}
			bool flag3 = size != list.Count;
			if (flag3)
			{
				NoAllocHelpers.Internal_ResizeList(list, size);
			}
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0002A610 File Offset: 0x00028810
		public static void EnsureListElemCount<T>(List<T> list, int count)
		{
			list.Clear();
			bool flag = list.Capacity < count;
			if (flag)
			{
				list.Capacity = count;
			}
			NoAllocHelpers.ResizeList<T>(list, count);
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0002A644 File Offset: 0x00028844
		public static int SafeLength(Array values)
		{
			return (values != null) ? values.Length : 0;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0002A664 File Offset: 0x00028864
		public static int SafeLength<T>(List<T> values)
		{
			return (values != null) ? values.Count : 0;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0002A684 File Offset: 0x00028884
		public static T[] ExtractArrayFromListT<T>(List<T> list)
		{
			return (T[])NoAllocHelpers.ExtractArrayFromList(list);
		}

		// Token: 0x06001960 RID: 6496
		[FreeFunction("NoAllocHelpers_Bindings::Internal_ResizeList")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_ResizeList(object list, int size);

		// Token: 0x06001961 RID: 6497
		[FreeFunction("NoAllocHelpers_Bindings::ExtractArrayFromList")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Array ExtractArrayFromList(object list);
	}
}
