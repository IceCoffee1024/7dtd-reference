using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200153F RID: 5439
public static class vp_GlobalEventReturn<T, R>
{
	// Token: 0x0600AA6F RID: 43631 RVA: 0x003F93E4 File Offset: 0x003F75E4
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

	// Token: 0x0600AA70 RID: 43632 RVA: 0x003F9440 File Offset: 0x003F7640
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

	// Token: 0x0600AA71 RID: 43633 RVA: 0x003F9491 File Offset: 0x003F7691
	public static R Send(string name, T arg1)
	{
		return vp_GlobalEventReturn<T, R>.Send(name, arg1, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AA72 RID: 43634 RVA: 0x003F949C File Offset: 0x003F769C
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

	// Token: 0x04007E3C RID: 32316
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
