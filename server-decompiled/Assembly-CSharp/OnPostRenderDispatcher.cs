using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001489 RID: 5257
public class OnPostRenderDispatcher : MonoBehaviour
{
	// Token: 0x0600A497 RID: 42135 RVA: 0x003D6F53 File Offset: 0x003D5153
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		OnPostRenderDispatcher.Instance = this;
	}

	// Token: 0x0600A498 RID: 42136 RVA: 0x003D6F5C File Offset: 0x003D515C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnPostRender()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnPostRender();
		}
	}

	// Token: 0x0600A499 RID: 42137 RVA: 0x003D6F90 File Offset: 0x003D5190
	public void Add(IOnPostRender _p)
	{
		this.listeners.Add(_p);
	}

	// Token: 0x0600A49A RID: 42138 RVA: 0x003D6F9E File Offset: 0x003D519E
	public void Remove(IOnPostRender _p)
	{
		this.listeners.Remove(_p);
	}

	// Token: 0x04007B26 RID: 31526
	public static OnPostRenderDispatcher Instance;

	// Token: 0x04007B27 RID: 31527
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<IOnPostRender> listeners = new List<IOnPostRender>();
}
