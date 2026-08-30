using System;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002F5 RID: 757
	internal abstract class BaseInvokableCall
	{
		// Token: 0x06001F62 RID: 8034 RVA: 0x00009E2F File Offset: 0x0000802F
		protected BaseInvokableCall()
		{
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x00033744 File Offset: 0x00031944
		protected BaseInvokableCall(object target, MethodInfo function)
		{
			bool flag = function == null;
			if (flag)
			{
				throw new ArgumentNullException("function");
			}
			bool isStatic = function.IsStatic;
			if (isStatic)
			{
				bool flag2 = target != null;
				if (flag2)
				{
					throw new ArgumentException("target must be null");
				}
			}
			else
			{
				bool flag3 = target == null;
				if (flag3)
				{
					throw new ArgumentNullException("target");
				}
			}
		}

		// Token: 0x06001F64 RID: 8036
		public abstract void Invoke(object[] args);

		// Token: 0x06001F65 RID: 8037 RVA: 0x000337A8 File Offset: 0x000319A8
		protected static void ThrowOnInvalidArg<T>(object arg)
		{
			bool flag = arg != null && !(arg is T);
			if (flag)
			{
				throw new ArgumentException(UnityString.Format("Passed argument 'args[0]' is of the wrong type. Type:{0} Expected:{1}", new object[]
				{
					arg.GetType(),
					typeof(T)
				}));
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000337F8 File Offset: 0x000319F8
		protected static bool AllowInvoke(Delegate @delegate)
		{
			object target = @delegate.Target;
			bool flag = target == null;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				Object @object = target as Object;
				bool flag3 = @object != null;
				flag2 = !flag3 || @object != null;
			}
			return flag2;
		}

		// Token: 0x06001F67 RID: 8039
		public abstract bool Find(object targetObj, MethodInfo method);
	}
}
