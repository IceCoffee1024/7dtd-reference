using System;
using UnityEngine;

// Token: 0x020013EE RID: 5102
public class ClothFix : MonoBehaviour
{
	// Token: 0x06009FFC RID: 40956 RVA: 0x003C5F8F File Offset: 0x003C418F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.cloth = base.GetComponent<Cloth>();
	}

	// Token: 0x06009FFD RID: 40957 RVA: 0x003C5FA0 File Offset: 0x003C41A0
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

	// Token: 0x06009FFE RID: 40958 RVA: 0x003C5FE2 File Offset: 0x003C41E2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.cloth.enabled = false;
	}

	// Token: 0x04007933 RID: 31027
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Cloth cloth;
}
