using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine.Events
{
	// Token: 0x0200030A RID: 778
	[Serializable]
	public class UnityEvent<T0, T1, T2, T3> : UnityEventBase
	{
		// Token: 0x06002001 RID: 8193 RVA: 0x000352BF File Offset: 0x000334BF
		[RequiredByNativeCode]
		public UnityEvent()
		{
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x000352D0 File Offset: 0x000334D0
		public void AddListener(UnityAction<T0, T1, T2, T3> call)
		{
			base.AddCall(UnityEvent<T0, T1, T2, T3>.GetDelegate(call));
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x00034D9A File Offset: 0x00032F9A
		public void RemoveListener(UnityAction<T0, T1, T2, T3> call)
		{
			base.RemoveListener(call.Target, call.Method);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x000352E0 File Offset: 0x000334E0
		protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
		{
			return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[]
			{
				typeof(T0),
				typeof(T1),
				typeof(T2),
				typeof(T3)
			});
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x00035334 File Offset: 0x00033534
		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall<T0, T1, T2, T3>(target, theFunction);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00035350 File Offset: 0x00033550
		private static BaseInvokableCall GetDelegate(UnityAction<T0, T1, T2, T3> action)
		{
			return new InvokableCall<T0, T1, T2, T3>(action);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00035368 File Offset: 0x00033568
		public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
		{
			List<BaseInvokableCall> list = base.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				InvokableCall<T0, T1, T2, T3> invokableCall = list[i] as InvokableCall<T0, T1, T2, T3>;
				bool flag = invokableCall != null;
				if (flag)
				{
					invokableCall.Invoke(arg0, arg1, arg2, arg3);
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
							this.m_InvokeArray = new object[4];
						}
						this.m_InvokeArray[0] = arg0;
						this.m_InvokeArray[1] = arg1;
						this.m_InvokeArray[2] = arg2;
						this.m_InvokeArray[3] = arg3;
						baseInvokableCall.Invoke(this.m_InvokeArray);
					}
				}
			}
		}

		// Token: 0x04000A87 RID: 2695
		private object[] m_InvokeArray = null;
	}
}
