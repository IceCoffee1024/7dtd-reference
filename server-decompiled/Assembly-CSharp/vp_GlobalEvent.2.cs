using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200153B RID: 5435
public static class vp_GlobalEvent<T>
{
	// Token: 0x0600AA5B RID: 43611 RVA: 0x003F8E44 File Offset: 0x003F7044
	[Preserve]
	public static void Register(string name, vp_GlobalCallback<T> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallback<T>> list = (List<vp_GlobalCallback<T>>)vp_GlobalEvent<T>.m_Callbacks[name];
		if (list == null)
		{
			list = new List<vp_GlobalCallback<T>>();
			vp_GlobalEvent<T>.m_Callbacks.Add(name, list);
		}
		list.Add(callback);
	}

	// Token: 0x0600AA5C RID: 43612 RVA: 0x003F8EA0 File Offset: 0x003F70A0
	[Preserve]
	public static void Unregister(string name, vp_GlobalCallback<T> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallback<T>> list = (List<vp_GlobalCallback<T>>)vp_GlobalEvent<T>.m_Callbacks[name];
		if (list != null)
		{
			list.Remove(callback);
			return;
		}
		throw vp_GlobalEventInternal.ShowUnregisterException(name);
	}

	// Token: 0x0600AA5D RID: 43613 RVA: 0x003F8EF1 File Offset: 0x003F70F1
	public static void Send(string name, T arg1)
	{
		vp_GlobalEvent<T>.Send(name, arg1, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AA5E RID: 43614 RVA: 0x003F8EFC File Offset: 0x003F70FC
	public static void Send(string name, T arg1, vp_GlobalEventMode mode)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (arg1 == null)
		{
			throw new ArgumentNullException("arg1");
		}
		List<vp_GlobalCallback<T>> list = (List<vp_GlobalCallback<T>>)vp_GlobalEvent<T>.m_Callbacks[name];
		if (list != null)
		{
			using (List<vp_GlobalCallback<T>>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					vp_GlobalCallback<T> vp_GlobalCallback = enumerator.Current;
					vp_GlobalCallback(arg1);
				}
				return;
			}
		}
		if (mode == vp_GlobalEventMode.REQUIRE_LISTENER)
		{
			throw vp_GlobalEventInternal.ShowSendException(name);
		}
	}

	// Token: 0x04007E38 RID: 32312
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
