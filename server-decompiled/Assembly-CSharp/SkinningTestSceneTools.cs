using System;
using UnityEngine;

// Token: 0x02001557 RID: 5463
public class SkinningTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB1A RID: 43802 RVA: 0x003FDA6B File Offset: 0x003FBC6B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		if (this.anim != null)
		{
			this.maxLayers = this.anim.layerCount;
		}
	}

	// Token: 0x0600AB1B RID: 43803 RVA: 0x003FDA98 File Offset: 0x003FBC98
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

	// Token: 0x04007F2E RID: 32558
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator anim;

	// Token: 0x04007F2F RID: 32559
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007F30 RID: 32560
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007F31 RID: 32561
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007F32 RID: 32562
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007F33 RID: 32563
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007F34 RID: 32564
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007F35 RID: 32565
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007F36 RID: 32566
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007F37 RID: 32567
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;
}
