using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200158B RID: 5515
public static class vp_GlobalEvent<T, U>
{
	// Token: 0x0600AC68 RID: 44136 RVA: 0x00405534 File Offset: 0x00403734
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

	// Token: 0x0600AC69 RID: 44137 RVA: 0x00405590 File Offset: 0x00403790
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

	// Token: 0x0600AC6A RID: 44138 RVA: 0x004055E1 File Offset: 0x004037E1
	public static void Send(string name, T arg1, U arg2)
	{
		vp_GlobalEvent<T, U>.Send(name, arg1, arg2, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AC6B RID: 44139 RVA: 0x004055EC File Offset: 0x004037EC
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

	// Token: 0x04007FD6 RID: 32726
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
