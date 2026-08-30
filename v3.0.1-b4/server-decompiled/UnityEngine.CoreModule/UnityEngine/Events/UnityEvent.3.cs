using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine.Events
{
	// Token: 0x02000306 RID: 774
	[Serializable]
	public class UnityEvent<T0, T1> : UnityEventBase
	{
		// Token: 0x06001FEB RID: 8171 RVA: 0x00034FEA File Offset: 0x000331EA
		[RequiredByNativeCode]
		public UnityEvent()
		{
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x00034FFB File Offset: 0x000331FB
		public void AddListener(UnityAction<T0, T1> call)
		{
			base.AddCall(UnityEvent<T0, T1>.GetDelegate(call));
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x00034D9A File Offset: 0x00032F9A
		public void RemoveListener(UnityAction<T0, T1> call)
		{
			base.RemoveListener(call.Target, call.Method);
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x0003500C File Offset: 0x0003320C
		protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
		{
			return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[]
			{
				typeof(T0),
				typeof(T1)
			});
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00035048 File Offset: 0x00033248
		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall<T0, T1>(target, theFunction);
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00035064 File Offset: 0x00033264
		private static BaseInvokableCall GetDelegate(UnityAction<T0, T1> action)
		{
			return new InvokableCall<T0, T1>(action);
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0003507C File Offset: 0x0003327C
		public void Invoke(T0 arg0, T1 arg1)
		{
			List<BaseInvokableCall> list = base.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				InvokableCall<T0, T1> invokableCall = list[i] as InvokableCall<T0, T1>;
				bool flag = invokableCall != null;
				if (flag)
				{
					invokableCall.Invoke(arg0, arg1);
				}
				else
				{
					InvokableCall invokableCall2 = list[i] as InvokableCall;
					bool flag2 = invokableCall2 != null;
					if (flag2)
					{
						invokableCall2.Invoke();
					}
					else
					{
						BaseInvokableCall baseInvokableCall = list[i];
						bool flag3 = this.m_InvokeArray == null;
						if (flag3)
						{
							this.m_InvokeArray = new object[2];
						}
						this.m_InvokeArray[0] = arg0;
						this.m_InvokeArray[1] = arg1;
						baseInvokableCall.Invoke(this.m_InvokeArray);
					}
				}
			}
		}

		// Token: 0x04000A85 RID: 2693
		private object[] m_InvokeArray = null;
	}
}
