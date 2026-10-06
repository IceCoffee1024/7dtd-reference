using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200157C RID: 5500
public static class vp_GlobalEventReturn<T, R>
{
	// Token: 0x0600AC1F RID: 44063 RVA: 0x00403578 File Offset: 0x00401778
	public static void Register(string name, vp_GlobalCallbackReturn<T, R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<T, R>> list = (List<vp_GlobalCallbackReturn<T, R>>)vp_GlobalEventReturn<T, R>.m_Callbacks[name];
		if (list == null)
		{
			list = new List<vp_GlobalCallbackReturn<T, R>>();
			vp_GlobalEventReturn<T, R>.m_Callbacks.Add(name, list);
		}
		list.Add(callback);
	}

	// Token: 0x0600AC20 RID: 44064 RVA: 0x004035D4 File Offset: 0x004017D4
	public static void Unregister(string name, vp_GlobalCallbackReturn<T, R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<T, R>> list = (List<vp_GlobalCallbackReturn<T, R>>)vp_GlobalEventReturn<T, R>.m_Callbacks[name];
		if (list != null)
		{
			list.Remove(callback);
			return;
		}
		throw vp_GlobalEventInternal.ShowUnregisterException(name);
	}

	// Token: 0x0600AC21 RID: 44065 RVA: 0x00403625 File Offset: 0x00401825
	public static R Send(string name, T arg1)
	{
		return vp_GlobalEventReturn<T, R>.Send(name, arg1, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AC22 RID: 44066 RVA: 0x00403630 File Offset: 0x00401830
	public static R Send(string name, T arg1, vp_GlobalEventMode mode)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (arg1 == null)
		{
			throw new ArgumentNullException("arg1");
		}
		List<vp_GlobalCallbackReturn<T, R>> list = (List<vp_GlobalCallbackReturn<T, R>>)vp_GlobalEventReturn<T, R>.m_Callbacks[name];
		if (list != null)
		{
			R r = default(R);
			foreach (vp_GlobalCallbackReturn<T, R> vp_GlobalCallbackReturn in list)
			{
				r = vp_GlobalCallbackReturn(arg1);
			}
			return r;
		}
		if (mode == vp_GlobalEventMode.REQUIRE_LISTENER)
		{
			throw vp_GlobalEventInternal.ShowSendException(name);
		}
		return default(R);
	}

	// Token: 0x04007F98 RID: 32664
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
