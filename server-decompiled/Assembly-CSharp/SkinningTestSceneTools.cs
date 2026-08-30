using System;
using UnityEngine;

// Token: 0x02001535 RID: 5429
public class SkinningTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AA67 RID: 43623 RVA: 0x003F8C17 File Offset: 0x003F6E17
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		if (this.anim != null)
		{
			this.maxLayers = this.anim.layerCount;
		}
	}

	// Token: 0x0600AA68 RID: 43624 RVA: 0x003F8C44 File Offset: 0x003F6E44
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space) && this.anim != null)
		{
			this.layerIndex++;
			if (this.layerIndex == this.maxLayers)
			{
				this.layerIndex = 0;
				for (int i = 1; i < this.maxLayers - 1; i++)
				{
					this.anim.SetLayerWeight(i, 0f);
				}
			}
			this.targetLayerWeight = 0f;
			this.endLayerWeight = 1f;
		}
		if (this.layerIndex == 0)
		{
			this.endLayerWeight = Mathf.Lerp(this.endLayerWeight, 0f, 0.01f);
			this.anim.SetLayerWeight(this.maxLayers - 1, this.endLayerWeight);
		}
		else
		{
			this.targetLayerWeight = Mathf.Lerp(this.targetLayerWeight, 1f, 0.01f);
			this.anim.SetLayerWeight(this.layerIndex, this.targetLayerWeight);
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime * -1f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime, 0f);
		}
	}

	// Token: 0x04007EAB RID: 32427
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator anim;

	// Token: 0x04007EAC RID: 32428
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007EAD RID: 32429
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007EAE RID: 32430
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007EAF RID: 32431
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007EB0 RID: 32432
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007EB1 RID: 32433
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007EB2 RID: 32434
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007EB3 RID: 32435
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007EB4 RID: 32436
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;
}
