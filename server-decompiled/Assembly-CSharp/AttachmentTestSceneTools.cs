using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001516 RID: 5398
public class AttachmentTestSceneTools : MonoBehaviour
{
	// Token: 0x0600A956 RID: 43350 RVA: 0x003F2DD8 File Offset: 0x003F0FD8
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

	// Token: 0x0600A957 RID: 43351 RVA: 0x003F2EBC File Offset: 0x003F10BC
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

	// Token: 0x04007DA4 RID: 32164
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;

	// Token: 0x04007DA5 RID: 32165
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007DA6 RID: 32166
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007DA7 RID: 32167
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007DA8 RID: 32168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007DA9 RID: 32169
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007DAA RID: 32170
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007DAB RID: 32171
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007DAC RID: 32172
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007DAD RID: 32173
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;

	// Token: 0x04007DAE RID: 32174
	public AnimationClip anim;

	// Token: 0x04007DAF RID: 32175
	public GameObject attachPoint;

	// Token: 0x04007DB0 RID: 32176
	public GameObject prefabAttachment;

	// Token: 0x04007DB1 RID: 32177
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject attached;

	// Token: 0x04007DB2 RID: 32178
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer meshRenderer;
}
