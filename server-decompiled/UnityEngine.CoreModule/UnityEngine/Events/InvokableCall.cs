using System;
using System.Diagnostics;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002F6 RID: 758
	internal class InvokableCall : BaseInvokableCall
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001F68 RID: 8040 RVA: 0x0003383C File Offset: 0x00031A3C
		// (remove) Token: 0x06001F69 RID: 8041 RVA: 0x00033874 File Offset: 0x00031A74
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event UnityAction Delegate;

		// Token: 0x06001F6A RID: 8042 RVA: 0x000338A9 File Offset: 0x00031AA9
		public InvokableCall(object target, MethodInfo theFunction)
			: base(target, theFunction)
		{
			this.Delegate += (UnityAction)global::System.Delegate.CreateDelegate(typeof(UnityAction), target, theFunction);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000338D2 File Offset: 0x00031AD2
		public InvokableCall(UnityAction action)
		{
			this.Delegate += action;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000338E4 File Offset: 0x00031AE4
		public override void Invoke(object[] args)
		{
			bool flag = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag)
			{
				this.Delegate();
			}
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x00033910 File Offset: 0x00031B10
		public void Invoke()
		{
			bool flag = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag)
			{
				this.Delegate();
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0003393C File Offset: 0x00031B3C
		public override bool Find(object targetObj, MethodInfo method)
		{
			return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
		}
	}
}
