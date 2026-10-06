using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200157B RID: 5499
[Preserve]
public static class vp_GlobalEventReturn<R>
{
	// Token: 0x0600AC1A RID: 44058 RVA: 0x00403420 File Offset: 0x00401620
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

	// Token: 0x0600AC1B RID: 44059 RVA: 0x0040347C File Offset: 0x0040167C
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

	// Token: 0x0600AC1C RID: 44060 RVA: 0x004034CD File Offset: 0x004016CD
	public static R Send(string name)
	{
		return vp_GlobalEventReturn<R>.Send(name, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AC1D RID: 44061 RVA: 0x004034D8 File Offset: 0x004016D8
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

	// Token: 0x04007F97 RID: 32663
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
