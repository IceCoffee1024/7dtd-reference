using System;
using UnityEngine;

// Token: 0x02001229 RID: 4649
[RequireComponent(typeof(Animator))]
public class SleeperPreview : MonoBehaviour
{
	// Token: 0x0600947D RID: 38013 RVA: 0x00381F2B File Offset: 0x0038012B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x0600947E RID: 38014 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetPose(int pose)
	{
	}

	// Token: 0x0600947F RID: 38015 RVA: 0x00381F39 File Offset: 0x00380139
	public void SetRotation(float rot)
	{
		base.transform.rotation = Quaternion.AngleAxis(rot, Vector3.up);
	}

	// Token: 0x04006F3E RID: 28478
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
