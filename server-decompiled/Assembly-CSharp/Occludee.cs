using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BA RID: 2234
public class Occludee : MonoBehaviour
{
	// Token: 0x060040B9 RID: 16569 RVA: 0x00193B70 File Offset: 0x00191D70
	public static void Add(GameObject obj)
	{
		if (!OcclusionManager.Instance.isEnabled)
		{
			return;
		}
		obj.AddComponent<Occludee>();
	}

	// Token: 0x060040BA RID: 16570 RVA: 0x00193B88 File Offset: 0x00191D88
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

	// Token: 0x060040BB RID: 16571 RVA: 0x00193BCC File Offset: 0x00191DCC
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

	// Token: 0x060040BC RID: 16572 RVA: 0x00193C28 File Offset: 0x00191E28
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

	// Token: 0x0400346C RID: 13420
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedListNode<OcclusionManager.OcclusionEntry> node;
}
