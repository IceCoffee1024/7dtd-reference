using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200144E RID: 5198
public class OnPostRenderDispatcher : MonoBehaviour
{
	// Token: 0x0600A2EE RID: 41710 RVA: 0x003CCFFF File Offset: 0x003CB1FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		OnPostRenderDispatcher.Instance = this;
	}

	// Token: 0x0600A2EF RID: 41711 RVA: 0x003CD008 File Offset: 0x003CB208
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPostRender()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnPostRender();
		}
	}

	// Token: 0x0600A2F0 RID: 41712 RVA: 0x003CD03C File Offset: 0x003CB23C
	public void Add(IOnPostRender _p)
	{
		this.listeners.Add(_p);
	}

	// Token: 0x0600A2F1 RID: 41713 RVA: 0x003CD04A File Offset: 0x003CB24A
	public void Remove(IOnPostRender _p)
	{
		this.listeners.Remove(_p);
	}

	// Token: 0x040079D2 RID: 31186
	public static OnPostRenderDispatcher Instance;

	// Token: 0x040079D3 RID: 31187
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IOnPostRender> listeners = new List<IOnPostRender>();
}
