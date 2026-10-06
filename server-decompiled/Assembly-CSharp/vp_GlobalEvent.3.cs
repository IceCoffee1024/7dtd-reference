using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001579 RID: 5497
public static class vp_GlobalEvent<T, U>
{
	// Token: 0x0600AC10 RID: 44048 RVA: 0x00403130 File Offset: 0x00401330
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

	// Token: 0x0600AC11 RID: 44049 RVA: 0x0040318C File Offset: 0x0040138C
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

	// Token: 0x0600AC12 RID: 44050 RVA: 0x004031DD File Offset: 0x004013DD
	public static void Send(string name, T arg1, U arg2)
	{
		vp_GlobalEvent<T, U>.Send(name, arg1, arg2, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AC13 RID: 44051 RVA: 0x004031E8 File Offset: 0x004013E8
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

	// Token: 0x04007F95 RID: 32661
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
