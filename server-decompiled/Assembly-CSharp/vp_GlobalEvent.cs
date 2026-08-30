using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001555 RID: 5461
public static class vp_GlobalEvent
{
	// Token: 0x0600AB53 RID: 43859 RVA: 0x003FE040 File Offset: 0x003FC240
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

	// Token: 0x0600AB54 RID: 43860 RVA: 0x003FE09C File Offset: 0x003FC29C
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

	// Token: 0x0600AB55 RID: 43861 RVA: 0x003FE0ED File Offset: 0x003FC2ED
	public static void Send(string name)
	{
		vp_GlobalEvent.Send(name, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AB56 RID: 43862 RVA: 0x003FE0F8 File Offset: 0x003FC2F8
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

	// Token: 0x04007F10 RID: 32528
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
