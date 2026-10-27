using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200149B RID: 5275
public class OnPostRenderDispatcher : MonoBehaviour
{
	// Token: 0x0600A4EF RID: 42223 RVA: 0x003D9357 File Offset: 0x003D7557
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		OnPostRenderDispatcher.Instance = this;
	}

	// Token: 0x0600A4F0 RID: 42224 RVA: 0x003D9360 File Offset: 0x003D7560
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPostRender()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnPostRender();
		}
	}

	// Token: 0x0600A4F1 RID: 42225 RVA: 0x003D9394 File Offset: 0x003D7594
	public void Add(IOnPostRender _p)
	{
		this.listeners.Add(_p);
	}

	// Token: 0x0600A4F2 RID: 42226 RVA: 0x003D93A2 File Offset: 0x003D75A2
	public void Remove(IOnPostRender _p)
	{
		this.listeners.Remove(_p);
	}

	// Token: 0x04007B67 RID: 31591
	public static OnPostRenderDispatcher Instance;

	// Token: 0x04007B68 RID: 31592
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IOnPostRender> listeners = new List<IOnPostRender>();
}
