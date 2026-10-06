using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BF RID: 2239
public class Occludee : MonoBehaviour
{
	// Token: 0x0600404F RID: 16463 RVA: 0x00193C74 File Offset: 0x00191E74
	public static void Add(GameObject obj)
	{
		if (!OcclusionManager.Instance.isEnabled)
		{
			return;
		}
		obj.AddComponent<Occludee>();
	}

	// Token: 0x06004050 RID: 16464 RVA: 0x00193C8C File Offset: 0x00191E8C
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

	// Token: 0x06004051 RID: 16465 RVA: 0x00193CD0 File Offset: 0x00191ED0
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

	// Token: 0x06004052 RID: 16466 RVA: 0x00193D2C File Offset: 0x00191F2C
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

	// Token: 0x04003460 RID: 13408
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedListNode<OcclusionManager.OcclusionEntry> node;
}
