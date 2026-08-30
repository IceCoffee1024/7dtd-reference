using System;
using UnityEngine;

// Token: 0x020013A1 RID: 5025
public class ClothFix : MonoBehaviour
{
	// Token: 0x06009DFC RID: 40444 RVA: 0x003B9CAB File Offset: 0x003B7EAB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.cloth = base.GetComponent<Cloth>();
	}

	// Token: 0x06009DFD RID: 40445 RVA: 0x003B9CBC File Offset: 0x003B7EBC
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

	// Token: 0x06009DFE RID: 40446 RVA: 0x003B9CFE File Offset: 0x003B7EFE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.cloth.enabled = false;
	}

	// Token: 0x0400779E RID: 30622
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Cloth cloth;
}
