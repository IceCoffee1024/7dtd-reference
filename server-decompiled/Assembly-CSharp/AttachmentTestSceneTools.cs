using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001531 RID: 5425
public class AttachmentTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AA53 RID: 43603 RVA: 0x003F8118 File Offset: 0x003F6318
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

	// Token: 0x0600AA54 RID: 43604 RVA: 0x003F81FC File Offset: 0x003F63FC
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

	// Token: 0x04007E7D RID: 32381
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;

	// Token: 0x04007E7E RID: 32382
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007E7F RID: 32383
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007E80 RID: 32384
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007E81 RID: 32385
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007E82 RID: 32386
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007E83 RID: 32387
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007E84 RID: 32388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007E85 RID: 32389
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007E86 RID: 32390
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;

	// Token: 0x04007E87 RID: 32391
	public AnimationClip anim;

	// Token: 0x04007E88 RID: 32392
	public GameObject attachPoint;

	// Token: 0x04007E89 RID: 32393
	public GameObject prefabAttachment;

	// Token: 0x04007E8A RID: 32394
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject attached;

	// Token: 0x04007E8B RID: 32395
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer meshRenderer;
}
