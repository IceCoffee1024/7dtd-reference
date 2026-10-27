using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008C9 RID: 2249
public class Occludee : MonoBehaviour
{
	// Token: 0x06004072 RID: 16498 RVA: 0x00194D1C File Offset: 0x00192F1C
	public static void Add(GameObject obj)
	{
		if (!OcclusionManager.Instance.isEnabled)
		{
			return;
		}
		obj.AddComponent<Occludee>();
	}

	// Token: 0x06004073 RID: 16499 RVA: 0x00194D34 File Offset: 0x00192F34
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

	// Token: 0x06004074 RID: 16500 RVA: 0x00194D78 File Offset: 0x00192F78
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

	// Token: 0x06004075 RID: 16501 RVA: 0x00194DD4 File Offset: 0x00192FD4
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

	// Token: 0x04003480 RID: 13440
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedListNode<OcclusionManager.OcclusionEntry> node;
}
