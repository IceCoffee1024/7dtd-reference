using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008AE RID: 2222
public class Occludee : MonoBehaviour
{
	// Token: 0x06004054 RID: 16468 RVA: 0x00191A98 File Offset: 0x0018FC98
	public static void Add(GameObject obj)
	{
		if (!OcclusionManager.Instance.isEnabled)
		{
			return;
		}
		obj.AddComponent<Occludee>();
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x00191AB0 File Offset: 0x0018FCB0
	public static void Refresh(GameObject obj)
	{
		if (!OcclusionManager.Instance.isEnabled)
		{
			return;
		}
		Occludee component = obj.GetComponent<Occludee>();
		if (component && component.node != null)
		{
			component.node.Value.isAreaFound = false;
		}
	}

	// Token: 0x06004056 RID: 16470 RVA: 0x00191AF4 File Offset: 0x0018FCF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		OcclusionManager instance = OcclusionManager.Instance;
		if (instance != null)
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
			if (componentsInChildren.Length != 0)
			{
				this.node = instance.RegisterOccludee(componentsInChildren, 32f);
				if (this.node == null)
				{
					Log.Warning("Occludee:OnEnable failed to register {0}", new object[] { base.name });
				}
			}
		}
	}

	// Token: 0x06004057 RID: 16471 RVA: 0x00191B50 File Offset: 0x0018FD50
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		OcclusionManager instance = OcclusionManager.Instance;
		if (instance != null && this.node != null)
		{
			instance.UnregisterOccludee(this.node);
			this.node = null;
		}
	}

	// Token: 0x040033EB RID: 13291
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedListNode<OcclusionManager.OcclusionEntry> node;
}
