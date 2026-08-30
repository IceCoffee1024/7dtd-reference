using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine.Events
{
	// Token: 0x02000304 RID: 772
	[Serializable]
	public class UnityEvent<T0> : UnityEventBase
	{
		// Token: 0x06001FE0 RID: 8160 RVA: 0x00034EAC File Offset: 0x000330AC
		[RequiredByNativeCode]
		public UnityEvent()
		{
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00034EBD File Offset: 0x000330BD
		public void AddListener(UnityAction<T0> call)
		{
			base.AddCall(UnityEvent<T0>.GetDelegate(call));
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00034D9A File Offset: 0x00032F9A
		public void RemoveListener(UnityAction<T0> call)
		{
			base.RemoveListener(call.Target, call.Method);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x00034ED0 File Offset: 0x000330D0
		protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
		{
			return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[] { typeof(T0) });
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x00034EFC File Offset: 0x000330FC
		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall<T0>(target, theFunction);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00034F18 File Offset: 0x00033118
		private static BaseInvokableCall GetDelegate(UnityAction<T0> action)
		{
			return new InvokableCall<T0>(action);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00034F30 File Offset: 0x00033130
		public void Invoke(T0 arg0)
		{
			List<BaseInvokableCall> list = base.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				InvokableCall<T0> invokableCall = list[i] as InvokableCall<T0>;
				bool flag = invokableCall != null;
				if (flag)
				{
					invokableCall.Invoke(arg0);
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
							this.m_InvokeArray = new object[1];
						}
						this.m_InvokeArray[0] = arg0;
						baseInvokableCall.Invoke(this.m_InvokeArray);
					}
				}
			}
		}

		// Token: 0x04000A84 RID: 2692
		private object[] m_InvokeArray = null;
	}
}
