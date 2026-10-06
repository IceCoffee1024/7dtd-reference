using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200157D RID: 5501
public static class vp_GlobalEventReturn<T, U, R>
{
	// Token: 0x0600AC24 RID: 44068 RVA: 0x004036E4 File Offset: 0x004018E4
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

	// Token: 0x0600AC25 RID: 44069 RVA: 0x00403740 File Offset: 0x00401940
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

	// Token: 0x0600AC26 RID: 44070 RVA: 0x00403791 File Offset: 0x00401991
	public static R Send(string name, T arg1, U arg2)
	{
		return vp_GlobalEventReturn<T, U, R>.Send(name, arg1, arg2, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AC27 RID: 44071 RVA: 0x0040379C File Offset: 0x0040199C
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

	// Token: 0x04007F99 RID: 32665
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
