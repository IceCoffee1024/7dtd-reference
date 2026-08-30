using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000056 RID: 86
[AddComponentMenu("NGUI/Examples/Play Idle Animations")]
public class PlayIdleAnimations : MonoBehaviour
{
	// Token: 0x060001BE RID: 446 RVA: 0x000105A4 File Offset: 0x0000E7A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mAnim = base.GetComponentInChildren<Animation>();
		if (this.mAnim == null)
		{
			Debug.LogWarning(NGUITools.GetHierarchy(base.gameObject) + " has no Animation component");
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		foreach (object obj in this.mAnim)
		{
			AnimationState animationState = (AnimationState)obj;
			if (animationState.clip.name == "idle")
			{
				animationState.layer = 0;
				this.mIdle = animationState.clip;
				this.mAnim.Play(this.mIdle.name);
			}
			else if (animationState.clip.name.StartsWith("idle"))
			{
				animationState.layer = 1;
				this.mBreaks.Add(animationState.clip);
			}
		}
		if (this.mBreaks.Count == 0)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x000106BC File Offset: 0x0000E8BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.mNextBreak < Time.time)
		{
			if (this.mBreaks.Count == 1)
			{
				AnimationClip animationClip = this.mBreaks[0];
				this.mNextBreak = Time.time + animationClip.length + global::UnityEngine.Random.Range(5f, 15f);
				this.mAnim.CrossFade(animationClip.name);
				return;
			}
			int num = global::UnityEngine.Random.Range(0, this.mBreaks.Count - 1);
			if (this.mLastIndex == num)
			{
				num++;
				if (num >= this.mBreaks.Count)
				{
					num = 0;
				}
			}
			this.mLastIndex = num;
			AnimationClip animationClip2 = this.mBreaks[num];
			this.mNextBreak = Time.time + animationClip2.length + global::UnityEngine.Random.Range(2f, 8f);
			this.mAnim.CrossFade(animationClip2.name);
		}
	}

	// Token: 0x04000269 RID: 617
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animation mAnim;

	// Token: 0x0400026A RID: 618
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AnimationClip mIdle;

	// Token: 0x0400026B RID: 619
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AnimationClip> mBreaks = new List<AnimationClip>();

	// Token: 0x0400026C RID: 620
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mNextBreak;

	// Token: 0x0400026D RID: 621
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int mLastIndex;
}
