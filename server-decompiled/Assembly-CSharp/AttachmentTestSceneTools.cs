using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001553 RID: 5459
public class AttachmentTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB06 RID: 43782 RVA: 0x003FCF6C File Offset: 0x003FB16C
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

	// Token: 0x0600AB07 RID: 43783 RVA: 0x003FD050 File Offset: 0x003FB250
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

	// Token: 0x04007F00 RID: 32512
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;

	// Token: 0x04007F01 RID: 32513
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007F02 RID: 32514
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007F03 RID: 32515
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 600f;

	// Token: 0x04007F04 RID: 32516
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007F05 RID: 32517
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalBodyParts;

	// Token: 0x04007F06 RID: 32518
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int randomMaterial;

	// Token: 0x04007F07 RID: 32519
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetLayerWeight;

	// Token: 0x04007F08 RID: 32520
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float endLayerWeight;

	// Token: 0x04007F09 RID: 32521
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel;

	// Token: 0x04007F0A RID: 32522
	public AnimationClip anim;

	// Token: 0x04007F0B RID: 32523
	public GameObject attachPoint;

	// Token: 0x04007F0C RID: 32524
	public GameObject prefabAttachment;

	// Token: 0x04007F0D RID: 32525
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject attached;

	// Token: 0x04007F0E RID: 32526
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer meshRenderer;
}
