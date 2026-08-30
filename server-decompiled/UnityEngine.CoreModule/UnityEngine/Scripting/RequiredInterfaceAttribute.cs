using System;

namespace UnityEngine.Scripting
{
	// Token: 0x0200031A RID: 794
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true)]
	public class RequiredInterfaceAttribute : Attribute
	{
		// Token: 0x06002042 RID: 8258 RVA: 0x00002059 File Offset: 0x00000259
		public RequiredInterfaceAttribute(Type interfaceType)
		{
		}
	}
}
