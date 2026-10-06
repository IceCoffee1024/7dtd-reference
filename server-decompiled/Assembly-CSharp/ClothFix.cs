using System;
using UnityEngine;

// Token: 0x020013DC RID: 5084
public class ClothFix : MonoBehaviour
{
	// Token: 0x06009FA4 RID: 40868 RVA: 0x003C3B8B File Offset: 0x003C1D8B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.cloth = base.GetComponent<Cloth>();
	}

	// Token: 0x06009FA5 RID: 40869 RVA: 0x003C3B9C File Offset: 0x003C1D9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.cloth.enabled = false;
		this.cloth.enabled = true;
		MeshCollider[] components = base.GetComponents<MeshCollider>();
		for (int i = 0; i < components.Length; i++)
		{
			global::UnityEngine.Object.Destroy(components[i]);
		}
	}

	// Token: 0x06009FA6 RID: 40870 RVA: 0x003C3BDE File Offset: 0x003C1DDE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.cloth.enabled = false;
	}

	// Token: 0x040078F2 RID: 30962
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Cloth cloth;
}
