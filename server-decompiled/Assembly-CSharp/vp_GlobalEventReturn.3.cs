using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200155B RID: 5467
public static class vp_GlobalEventReturn<T, U, R>
{
	// Token: 0x0600AB71 RID: 43889 RVA: 0x003FE890 File Offset: 0x003FCA90
	public static void Register(string name, vp_GlobalCallbackReturn<T, U, R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<T, U, R>> list = (List<vp_GlobalCallbackReturn<T, U, R>>)vp_GlobalEventReturn<T, U, R>.m_Callbacks[name];
		if (list == null)
		{
			list = new List<vp_GlobalCallbackReturn<T, U, R>>();
			vp_GlobalEventReturn<T, U, R>.m_Callbacks.Add(name, list);
		}
		list.Add(callback);
	}

	// Token: 0x0600AB72 RID: 43890 RVA: 0x003FE8EC File Offset: 0x003FCAEC
	public static void Unregister(string name, vp_GlobalCallbackReturn<T, U, R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<T, U, R>> list = (List<vp_GlobalCallbackReturn<T, U, R>>)vp_GlobalEventReturn<T, U, R>.m_Callbacks[name];
		if (list != null)
		{
			list.Remove(callback);
			return;
		}
		throw vp_GlobalEventInternal.ShowUnregisterException(name);
	}

	// Token: 0x0600AB73 RID: 43891 RVA: 0x003FE93D File Offset: 0x003FCB3D
	public static R Send(string name, T arg1, U arg2)
	{
		return vp_GlobalEventReturn<T, U, R>.Send(name, arg1, arg2, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AB74 RID: 43892 RVA: 0x003FE948 File Offset: 0x003FCB48
	public static R Send(string name, T arg1, U arg2, vp_GlobalEventMode mode)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (arg1 == null)
		{
			throw new ArgumentNullException("arg1");
		}
		if (arg2 == null)
		{
			throw new ArgumentNullException("arg2");
		}
		List<vp_GlobalCallbackReturn<T, U, R>> list = (List<vp_GlobalCallbackReturn<T, U, R>>)vp_GlobalEventReturn<T, U, R>.m_Callbacks[name];
		if (list != null)
		{
			R r = default(R);
			foreach (vp_GlobalCallbackReturn<T, U, R> vp_GlobalCallbackReturn in list)
			{
				r = vp_GlobalCallbackReturn(arg1, arg2);
			}
			return r;
		}
		if (mode == vp_GlobalEventMode.REQUIRE_LISTENER)
		{
			throw vp_GlobalEventInternal.ShowSendException(name);
		}
		return default(R);
	}

	// Token: 0x04007F16 RID: 32534
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
