using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200020A RID: 522
	[NativeHeader("Editor/Src/Properties/DrivenPropertyManager.h")]
	internal class DrivenPropertyManager
	{
		// Token: 0x06001793 RID: 6035 RVA: 0x000273ED File Offset: 0x000255ED
		[Conditional("UNITY_EDITOR")]
		public static void RegisterProperty(Object driver, Object target, string propertyPath)
		{
			DrivenPropertyManager.RegisterPropertyPartial(driver, target, propertyPath);
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000273F9 File Offset: 0x000255F9
		[Conditional("UNITY_EDITOR")]
		public static void TryRegisterProperty(Object driver, Object target, string propertyPath)
		{
			DrivenPropertyManager.TryRegisterPropertyPartial(driver, target, propertyPath);
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x00027405 File Offset: 0x00025605
		[Conditional("UNITY_EDITOR")]
		public static void UnregisterProperty(Object driver, Object target, string propertyPath)
		{
			DrivenPropertyManager.UnregisterPropertyPartial(driver, target, propertyPath);
		}

		// Token: 0x06001796 RID: 6038
		[Conditional("UNITY_EDITOR")]
		[NativeConditional("UNITY_EDITOR")]
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnregisterProperties([NotNull("ArgumentNullException")] Object driver);

		// Token: 0x06001797 RID: 6039
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RegisterPropertyPartial([NotNull("ArgumentNullException")] Object driver, [NotNull("ArgumentNullException")] Object target, [NotNull("ArgumentNullException")] string propertyPath);

		// Token: 0x06001798 RID: 6040
		[NativeConditional("UNITY_EDITOR")]
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void TryRegisterPropertyPartial([NotNull("ArgumentNullException")] Object driver, [NotNull("ArgumentNullException")] Object target, [NotNull("ArgumentNullException")] string propertyPath);

		// Token: 0x06001799 RID: 6041
		[StaticAccessor("GetDrivenPropertyManager()", StaticAccessorType.Dot)]
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnregisterPropertyPartial([NotNull("ArgumentNullException")] Object driver, [NotNull("ArgumentNullException")] Object target, [NotNull("ArgumentNullException")] string propertyPath);
	}
}
