using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200022A RID: 554
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[RequiredByNativeCode]
	public sealed class RequireComponent : Attribute
	{
		// Token: 0x06001841 RID: 6209 RVA: 0x000283F2 File Offset: 0x000265F2
		public RequireComponent(Type requiredComponent)
		{
			this.m_Type0 = requiredComponent;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00028403 File Offset: 0x00026603
		public RequireComponent(Type requiredComponent, Type requiredComponent2)
		{
			this.m_Type0 = requiredComponent;
			this.m_Type1 = requiredComponent2;
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0002841B File Offset: 0x0002661B
		public RequireComponent(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
		{
			this.m_Type0 = requiredComponent;
			this.m_Type1 = requiredComponent2;
			this.m_Type2 = requiredComponent3;
		}

		// Token: 0x04000894 RID: 2196
		public Type m_Type0;

		// Token: 0x04000895 RID: 2197
		public Type m_Type1;

		// Token: 0x04000896 RID: 2198
		public Type m_Type2;
	}
}
