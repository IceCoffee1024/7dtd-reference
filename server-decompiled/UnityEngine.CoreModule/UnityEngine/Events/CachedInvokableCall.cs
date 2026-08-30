using System;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002FB RID: 763
	internal class CachedInvokableCall<T> : InvokableCall<T>
	{
		// Token: 0x06001F8B RID: 8075 RVA: 0x00033F48 File Offset: 0x00032148
		public CachedInvokableCall(Object target, MethodInfo theFunction, T argument)
			: base(target, theFunction)
		{
			this.m_Arg1 = argument;
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x00033F5B File Offset: 0x0003215B
		public override void Invoke(object[] args)
		{
			base.Invoke(this.m_Arg1);
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x00033F5B File Offset: 0x0003215B
		public override void Invoke(T arg0)
		{
			base.Invoke(this.m_Arg1);
		}

		// Token: 0x04000A70 RID: 2672
		private readonly T m_Arg1;
	}
}
