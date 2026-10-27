using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001565 RID: 5477
public class AttachmentTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB5E RID: 43870 RVA: 0x003FF370 File Offset: 0x003FD570
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.animator = base.GetComponent<Animator>();
		if (this.animator != null)
		{
			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(this.animator.runtimeAnimatorController);
			List<KeyValuePair<AnimationClip, AnimationClip>> list = new List<KeyValuePair<AnimationClip, AnimationClip>>();
			foreach (AnimationClip animationClip in animatorOverrideController.animationClips)
			{
				list.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, this.anim));
			}
			animatorOverrideController.ApplyOverrides(list);
			this.animator.runtimeAnimatorController = animatorOverrideController;
		}
		if (this.attached != null)
		{
			this.attached = global::UnityEngine.Object.Instantiate<GameObject>(this.prefabAttachment);
			this.attached.transform.parent = this.attachPoint.transform;
			this.attached.transform.localPosition = Vector3.zero;
			this.attached.transform.localEulerAngles = Vector3.zero;
		}
	}

	// Token: 0x0600AB5F RID: 43871 RVA: 0x003FF454 File Offset: 0x003FD654
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime * -1f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime, 0f);
		}
	}

	// Token: 0x04007F41 RID: 32577
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;

	// Token: 0x04007F42 RID: 32578
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007F43 RID: 32579
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007F44 RID: 32580
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007F45 RID: 32581
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007F46 RID: 32582
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007F47 RID: 32583
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007F48 RID: 32584
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007F49 RID: 32585
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007F4A RID: 32586
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;

	// Token: 0x04007F4B RID: 32587
	public AnimationClip anim;

	// Token: 0x04007F4C RID: 32588
	public GameObject attachPoint;

	// Token: 0x04007F4D RID: 32589
	public GameObject prefabAttachment;

	// Token: 0x04007F4E RID: 32590
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject attached;

	// Token: 0x04007F4F RID: 32591
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer meshRenderer;
}
