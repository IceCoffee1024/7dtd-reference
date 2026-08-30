using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001540 RID: 5440
public static class vp_GlobalEventReturn<T, U, R>
{
	// Token: 0x0600AA74 RID: 43636 RVA: 0x003F9550 File Offset: 0x003F7750
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

	// Token: 0x0600AA75 RID: 43637 RVA: 0x003F95AC File Offset: 0x003F77AC
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

	// Token: 0x0600AA76 RID: 43638 RVA: 0x003F95FD File Offset: 0x003F77FD
	public static R Send(string name, T arg1, U arg2)
	{
		return vp_GlobalEventReturn<T, U, R>.Send(name, arg1, arg2, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AA77 RID: 43639 RVA: 0x003F9608 File Offset: 0x003F7808
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

	// Token: 0x04007E3D RID: 32317
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
