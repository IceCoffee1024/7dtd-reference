using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200011D RID: 285
	[NativeHeader("Runtime/Utilities/PropertyName.h")]
	[NativeHeader("Runtime/Director/Core/ExposedPropertyTable.bindings.h")]
	public struct ExposedPropertyResolver
	{
		// Token: 0x06000723 RID: 1827 RVA: 0x00009E88 File Offset: 0x00008088
		internal static Object ResolveReferenceInternal(IntPtr ptr, PropertyName name, out bool isValid)
		{
			bool flag = ptr == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentNullException("Argument \"ptr\" can't be null.");
			}
			return ExposedPropertyResolver.ResolveReferenceBindingsInternal(ptr, name, out isValid);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00009EBC File Offset: 0x000080BC
		[FreeFunction("ExposedPropertyTableBindings::ResolveReferenceInternal")]
		private static Object ResolveReferenceBindingsInternal(IntPtr ptr, PropertyName name, out bool isValid)
		{
			return ExposedPropertyResolver.ResolveReferenceBindingsInternal_Injected(ptr, ref name, out isValid);
		}

		// Token: 0x06000725 RID: 1829
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object ResolveReferenceBindingsInternal_Injected(IntPtr ptr, ref PropertyName name, out bool isValid);

		// Token: 0x040003A8 RID: 936
		internal IntPtr table;
	}
}
