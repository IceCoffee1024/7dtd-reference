using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200157E RID: 5502
[Preserve]
public static class vp_GlobalEventReturn<T, U, V, R>
{
	// Token: 0x0600AC29 RID: 44073 RVA: 0x00403864 File Offset: 0x00401A64
	[Preserve]
	public static void Register(string name, vp_GlobalCallbackReturn<T, U, V, R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<T, U, V, R>> list = (List<vp_GlobalCallbackReturn<T, U, V, R>>)vp_GlobalEventReturn<T, U, V, R>.m_Callbacks[name];
		if (list == null)
		{
			list = new List<vp_GlobalCallbackReturn<T, U, V, R>>();
			vp_GlobalEventReturn<T, U, V, R>.m_Callbacks.Add(name, list);
		}
		list.Add(callback);
	}

	// Token: 0x0600AC2A RID: 44074 RVA: 0x004038C0 File Offset: 0x00401AC0
	[Preserve]
	public static void Unregister(string name, vp_GlobalCallbackReturn<T, U, V, R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<T, U, V, R>> list = (List<vp_GlobalCallbackReturn<T, U, V, R>>)vp_GlobalEventReturn<T, U, V, R>.m_Callbacks[name];
		if (list != null)
		{
			list.Remove(callback);
			return;
		}
		throw vp_GlobalEventInternal.ShowUnregisterException(name);
	}

	// Token: 0x0600AC2B RID: 44075 RVA: 0x00403911 File Offset: 0x00401B11
	public static R Send(string name, T arg1, U arg2, V arg3)
	{
		return vp_GlobalEventReturn<T, U, V, R>.Send(name, arg1, arg2, arg3, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AC2C RID: 44076 RVA: 0x00403920 File Offset: 0x00401B20
	public static R Send(string name, T arg1, U arg2, V arg3, vp_GlobalEventMode mode)
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
		if (arg3 == null)
		{
			throw new ArgumentNullException("arg3");
		}
		List<vp_GlobalCallbackReturn<T, U, V, R>> list = (List<vp_GlobalCallbackReturn<T, U, V, R>>)vp_GlobalEventReturn<T, U, V, R>.m_Callbacks[name];
		if (list != null)
		{
			R r = default(R);
			foreach (vp_GlobalCallbackReturn<T, U, V, R> vp_GlobalCallbackReturn in list)
			{
				r = vp_GlobalCallbackReturn(arg1, arg2, arg3);
			}
			return r;
		}
		if (mode == vp_GlobalEventMode.REQUIRE_LISTENER)
		{
			throw vp_GlobalEventInternal.ShowSendException(name);
		}
		return default(R);
	}

	// Token: 0x04007F9A RID: 32666
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
