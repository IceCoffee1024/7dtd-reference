using System;
using System.Diagnostics;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002F7 RID: 759
	internal class InvokableCall<T1> : BaseInvokableCall
	{
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001F6F RID: 8047 RVA: 0x00033970 File Offset: 0x00031B70
		// (remove) Token: 0x06001F70 RID: 8048 RVA: 0x000339A8 File Offset: 0x00031BA8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UnityAction<T1> Delegate;

		// Token: 0x06001F71 RID: 8049 RVA: 0x000339DD File Offset: 0x00031BDD
		public InvokableCall(object target, MethodInfo theFunction)
			: base(target, theFunction)
		{
			this.Delegate += (UnityAction<T1>)global::System.Delegate.CreateDelegate(typeof(UnityAction<T1>), target, theFunction);
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x00033A06 File Offset: 0x00031C06
		public InvokableCall(UnityAction<T1> action)
		{
			this.Delegate += action;
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x00033A18 File Offset: 0x00031C18
		public override void Invoke(object[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
			}
			BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
			bool flag2 = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag2)
			{
				this.Delegate((T1)((object)args[0]));
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x00033A6C File Offset: 0x00031C6C
		public virtual void Invoke(T1 args0)
		{
			bool flag = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag)
			{
				this.Delegate(args0);
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00033A98 File Offset: 0x00031C98
		public override bool Find(object targetObj, MethodInfo method)
		{
			return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
		}
	}
}
