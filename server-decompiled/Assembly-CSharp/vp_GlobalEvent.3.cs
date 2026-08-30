using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200153C RID: 5436
public static class vp_GlobalEvent<T, U>
{
	// Token: 0x0600AA60 RID: 43616 RVA: 0x003F8F9C File Offset: 0x003F719C
	[Preserve]
	public static void Register(string name, vp_GlobalCallback<T, U> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallback<T, U>> list = (List<vp_GlobalCallback<T, U>>)vp_GlobalEvent<T, U>.m_Callbacks[name];
		if (list == null)
		{
			list = new List<vp_GlobalCallback<T, U>>();
			vp_GlobalEvent<T, U>.m_Callbacks.Add(name, list);
		}
		list.Add(callback);
	}

	// Token: 0x0600AA61 RID: 43617 RVA: 0x003F8FF8 File Offset: 0x003F71F8
	[Preserve]
	public static void Unregister(string name, vp_GlobalCallback<T, U> callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallback<T, U>> list = (List<vp_GlobalCallback<T, U>>)vp_GlobalEvent<T, U>.m_Callbacks[name];
		if (list != null)
		{
			list.Remove(callback);
			return;
		}
		throw vp_GlobalEventInternal.ShowUnregisterException(name);
	}

	// Token: 0x0600AA62 RID: 43618 RVA: 0x003F9049 File Offset: 0x003F7249
	public static void Send(string name, T arg1, U arg2)
	{
		vp_GlobalEvent<T, U>.Send(name, arg1, arg2, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AA63 RID: 43619 RVA: 0x003F9054 File Offset: 0x003F7254
	public static void Send(string name, T arg1, U arg2, vp_GlobalEventMode mode)
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
		List<vp_GlobalCallback<T, U>> list = (List<vp_GlobalCallback<T, U>>)vp_GlobalEvent<T, U>.m_Callbacks[name];
		if (list != null)
		{
			using (List<vp_GlobalCallback<T, U>>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					vp_GlobalCallback<T, U> vp_GlobalCallback = enumerator.Current;
					vp_GlobalCallback(arg1, arg2);
				}
				return;
			}
		}
		if (mode == vp_GlobalEventMode.REQUIRE_LISTENER)
		{
			throw vp_GlobalEventInternal.ShowSendException(name);
		}
	}

	// Token: 0x04007E39 RID: 32313
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
