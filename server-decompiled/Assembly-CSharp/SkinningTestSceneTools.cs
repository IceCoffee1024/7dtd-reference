using System;
using UnityEngine;

// Token: 0x02001569 RID: 5481
public class SkinningTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB72 RID: 43890 RVA: 0x003FFE6F File Offset: 0x003FE06F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		if (this.anim != null)
		{
			this.maxLayers = this.anim.layerCount;
		}
	}

	// Token: 0x0600AB73 RID: 43891 RVA: 0x003FFE9C File Offset: 0x003FE09C
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

	// Token: 0x04007F6F RID: 32623
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator anim;

	// Token: 0x04007F70 RID: 32624
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007F71 RID: 32625
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007F72 RID: 32626
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007F73 RID: 32627
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007F74 RID: 32628
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007F75 RID: 32629
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007F76 RID: 32630
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007F77 RID: 32631
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007F78 RID: 32632
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;
}
