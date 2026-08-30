using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001469 RID: 5225
public class OnPostRenderDispatcher : MonoBehaviour
{
	// Token: 0x0600A3EB RID: 41963 RVA: 0x003D2343 File Offset: 0x003D0543
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		OnPostRenderDispatcher.Instance = this;
	}

	// Token: 0x0600A3EC RID: 41964 RVA: 0x003D234C File Offset: 0x003D054C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPostRender()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnPostRender();
		}
	}

	// Token: 0x0600A3ED RID: 41965 RVA: 0x003D2380 File Offset: 0x003D0580
	public void Add(IOnPostRender _p)
	{
		this.listeners.Add(_p);
	}

	// Token: 0x0600A3EE RID: 41966 RVA: 0x003D238E File Offset: 0x003D058E
	public void Remove(IOnPostRender _p)
	{
		this.listeners.Remove(_p);
	}

	// Token: 0x04007AAB RID: 31403
	public static OnPostRenderDispatcher Instance;

	// Token: 0x04007AAC RID: 31404
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IOnPostRender> listeners = new List<IOnPostRender>();
}
