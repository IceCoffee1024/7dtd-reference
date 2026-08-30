using System;
using UnityEngine;

// Token: 0x020013BC RID: 5052
public class ClothFix : MonoBehaviour
{
	// Token: 0x06009EF9 RID: 40697 RVA: 0x003BEFEF File Offset: 0x003BD1EF
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.cloth = base.GetComponent<Cloth>();
	}

	// Token: 0x06009EFA RID: 40698 RVA: 0x003BF000 File Offset: 0x003BD200
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

	// Token: 0x06009EFB RID: 40699 RVA: 0x003BF042 File Offset: 0x003BD242
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.cloth.enabled = false;
	}

	// Token: 0x04007877 RID: 30839
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Cloth cloth;
}
