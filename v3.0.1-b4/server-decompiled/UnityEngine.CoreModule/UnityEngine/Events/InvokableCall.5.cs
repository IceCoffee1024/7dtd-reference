using System;
using System.Diagnostics;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002FA RID: 762
	internal class InvokableCall<T1, T2, T3, T4> : BaseInvokableCall
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001F84 RID: 8068 RVA: 0x00033DB4 File Offset: 0x00031FB4
		// (remove) Token: 0x06001F85 RID: 8069 RVA: 0x00033DEC File Offset: 0x00031FEC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UnityAction<T1, T2, T3, T4> Delegate;

		// Token: 0x06001F86 RID: 8070 RVA: 0x00033E21 File Offset: 0x00032021
		public InvokableCall(object target, MethodInfo theFunction)
			: base(target, theFunction)
		{
			this.Delegate = (UnityAction<T1, T2, T3, T4>)global::System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4>), target, theFunction);
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x00033E49 File Offset: 0x00032049
		public InvokableCall(UnityAction<T1, T2, T3, T4> action)
		{
			this.Delegate += action;
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x00033E5C File Offset: 0x0003205C
		public override void Invoke(object[] args)
		{
			bool flag = args.Length != 4;
			if (flag)
			{
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
			}
			BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
			BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
			BaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
			BaseInvokableCall.ThrowOnInvalidArg<T4>(args[3]);
			bool flag2 = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag2)
			{
				this.Delegate((T1)((object)args[0]), (T2)((object)args[1]), (T3)((object)args[2]), (T4)((object)args[3]));
			}
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x00033EE4 File Offset: 0x000320E4
		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3)
		{
			bool flag = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag)
			{
				this.Delegate(args0, args1, args2, args3);
			}
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x00033F14 File Offset: 0x00032114
		public override bool Find(object targetObj, MethodInfo method)
		{
			return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
		}
	}
}
