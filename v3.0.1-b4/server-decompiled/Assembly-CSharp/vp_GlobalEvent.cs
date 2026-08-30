using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200153A RID: 5434
public static class vp_GlobalEvent
{
	// Token: 0x0600AA56 RID: 43606 RVA: 0x003F8D00 File Offset: 0x003F6F00
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

	// Token: 0x0600AA57 RID: 43607 RVA: 0x003F8D5C File Offset: 0x003F6F5C
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

	// Token: 0x0600AA58 RID: 43608 RVA: 0x003F8DAD File Offset: 0x003F6FAD
	public static void Send(string name)
	{
		vp_GlobalEvent.Send(name, vp_GlobalEventMode.DONT_REQUIRE_LISTENER);
	}

	// Token: 0x0600AA59 RID: 43609 RVA: 0x003F8DB8 File Offset: 0x003F6FB8
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

	// Token: 0x04007E37 RID: 32311
	[PublicizedFrom(EAccessModifier.Private)]
	public static Hashtable m_Callbacks = vp_GlobalEventInternal.Callbacks;
}
