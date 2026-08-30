using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000219 RID: 537
	[NativeHeader("Runtime/Utilities/PropertyName.h")]
	internal class PropertyNameUtils
	{
		// Token: 0x060017B1 RID: 6065 RVA: 0x0002769C File Offset: 0x0002589C
		[FreeFunction(IsThreadSafe = true)]
		public static PropertyName PropertyNameFromString([Unmarshalled] string name)
		{
			PropertyName propertyName;
			PropertyNameUtils.PropertyNameFromString_Injected(name, out propertyName);
			return propertyName;
		}

		// Token: 0x060017B3 RID: 6067
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PropertyNameFromString_Injected(string name, out PropertyName ret);
	}
}
