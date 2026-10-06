using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001577 RID: 5495
public static class vp_GlobalEvent
{
	// Token: 0x0600AC06 RID: 44038 RVA: 0x00402E94 File Offset: 0x00401094
	public static void Register(string name, vp_GlobalCallback callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallback> list = (List<vp_GlobalCallback>)vp_GlobalEvent.m_Callbacks[name];
		if (list == null)
		{
			list = new List<vp_GlobalCallback>();
			vp_GlobalEvent.m_Callbacks.Add(name, list);
		}
		list.Add(callback);
	}

	// Token: 0x0600AC07 RID: 44039 RVA: 0x00402EF0 File Offset: 0x004010F0
	public static void Unregister(string name, vp_GlobalCallback callback)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		List<vp_GlobalCallback> list = (List<vp_GlobalCallback>)vp_GlobalEvent.m_Callbacks[name];
		if (list != null)
		{
			list.Remove(callback);
			return;
		}
		throw vp_GlobalEventInternal.ShowUnregisterException(name);
	}

	// Token: 0x0600AC08 RID: 44040 RVA: 0x00402F41 File Offset: 0x00401141
	public static void Send(string name)
	{
		vp_GlobalEvent.Send(name, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AC09 RID: 44041 RVA: 0x00402F4C File Offset: 0x0040114C
	public static void Send(string name, vp_GlobalEventMode mode)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		List<vp_GlobalCallback> list = (List<vp_GlobalCallback>)vp_GlobalEvent.m_Callbacks[name];
		if (list != null)
		{
			using (List<vp_GlobalCallback>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					vp_GlobalCallback vp_GlobalCallback = enumerator.Current;
					vp_GlobalCallback();
				}
				return;
			}
		}
		if (mode == vp_GlobalEventMode.REQUIRE_LISTENER)
		{
			throw vp_GlobalEventInternal.ShowSendException(name);
		}
	}

	// Token: 0x04007F93 RID: 32659
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
