using System;
using System.Collections;
using System.Reflection;
using System.Security;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200023E RID: 574
	[RequiredByNativeCode]
	internal class SetupCoroutine
	{
		// Token: 0x060018A6 RID: 6310 RVA: 0x00028E64 File Offset: 0x00027064
		[SecuritySafeCritical]
		[RequiredByNativeCode]
		public unsafe static void InvokeMoveNext(IEnumerator enumerator, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			*(byte*)(void*)returnValueAddress = (enumerator.MoveNext() ? 1 : 0);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x00028EA0 File Offset: 0x000270A0
		[RequiredByNativeCode]
		public static object InvokeMember(object behaviour, string name, object variable)
		{
			object[] array = null;
			bool flag = variable != null;
			if (flag)
			{
				array = new object[] { variable };
			}
			return behaviour.GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, behaviour, array, null, null, null);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00028EE0 File Offset: 0x000270E0
		public static object InvokeStatic(Type klass, string name, object variable)
		{
			object[] array = null;
			bool flag = variable != null;
			if (flag)
			{
				array = new object[] { variable };
			}
			return klass.InvokeMember(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, array, null, null, null);
		}
	}
}
