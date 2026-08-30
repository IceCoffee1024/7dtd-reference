using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200153E RID: 5438
[Preserve]
public static class vp_GlobalEventReturn<R>
{
	// Token: 0x0600AA6A RID: 43626 RVA: 0x003F928C File Offset: 0x003F748C
	public static void Register(string name, vp_GlobalCallbackReturn<R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<R>> list = (List<vp_GlobalCallbackReturn<R>>)vp_GlobalEventReturn<R>.m_Callbacks[name];
		if (list == null)
		{
			list = new List<vp_GlobalCallbackReturn<R>>();
			vp_GlobalEventReturn<R>.m_Callbacks.Add(name, list);
		}
		list.Add(callback);
	}

	// Token: 0x0600AA6B RID: 43627 RVA: 0x003F92E8 File Offset: 0x003F74E8
	public static void Unregister(string name, vp_GlobalCallbackReturn<R> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallbackReturn<R>> list = (List<vp_GlobalCallbackReturn<R>>)vp_GlobalEventReturn<R>.m_Callbacks[name];
		if (list != null)
		{
			list.Remove(callback);
			return;
		}
		throw vp_GlobalEventInternal.ShowUnregisterException(name);
	}

	// Token: 0x0600AA6C RID: 43628 RVA: 0x003F9339 File Offset: 0x003F7539
	public static R Send(string name)
	{
		return vp_GlobalEventReturn<R>.Send(name, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AA6D RID: 43629 RVA: 0x003F9344 File Offset: 0x003F7544
	public static R Send(string name, vp_GlobalEventMode mode)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		List<vp_GlobalCallbackReturn<R>> list = (List<vp_GlobalCallbackReturn<R>>)vp_GlobalEventReturn<R>.m_Callbacks[name];
		if (list != null)
		{
			R r = default(R);
			foreach (vp_GlobalCallbackReturn<R> vp_GlobalCallbackReturn in list)
			{
				r = vp_GlobalCallbackReturn();
			}
			return r;
		}
		if (mode == vp_GlobalEventMode.REQUIRE_LISTENER)
		{
			throw vp_GlobalEventInternal.ShowSendException(name);
		}
		return default(R);
	}

	// Token: 0x04007E3B RID: 32315
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
