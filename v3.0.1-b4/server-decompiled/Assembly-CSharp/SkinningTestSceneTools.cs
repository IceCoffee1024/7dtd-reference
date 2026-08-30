using System;
using UnityEngine;

// Token: 0x0200151A RID: 5402
public class SkinningTestSceneTools : MonoBehaviour
{
	// Token: 0x0600A96A RID: 43370 RVA: 0x003F38D7 File Offset: 0x003F1AD7
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		if (this.anim != null)
		{
			this.maxLayers = this.anim.layerCount;
		}
	}

	// Token: 0x0600A96B RID: 43371 RVA: 0x003F3904 File Offset: 0x003F1B04
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

	// Token: 0x04007DD2 RID: 32210
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator anim;

	// Token: 0x04007DD3 RID: 32211
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007DD4 RID: 32212
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007DD5 RID: 32213
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007DD6 RID: 32214
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007DD7 RID: 32215
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007DD8 RID: 32216
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007DD9 RID: 32217
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007DDA RID: 32218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007DDB RID: 32219
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;
}
