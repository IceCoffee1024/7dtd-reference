using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001559 RID: 5465
[Preserve]
public static class vp_GlobalEventReturn<R>
{
	// Token: 0x0600AB67 RID: 43879 RVA: 0x003FE5CC File Offset: 0x003FC7CC
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

	// Token: 0x0600AB68 RID: 43880 RVA: 0x003FE628 File Offset: 0x003FC828
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

	// Token: 0x0600AB69 RID: 43881 RVA: 0x003FE679 File Offset: 0x003FC879
	public static R Send(string name)
	{
		return vp_GlobalEventReturn<R>.Send(name, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AB6A RID: 43882 RVA: 0x003FE684 File Offset: 0x003FC884
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

	// Token: 0x04007F14 RID: 32532
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
