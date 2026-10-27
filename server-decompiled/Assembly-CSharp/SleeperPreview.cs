using System;
using UnityEngine;

// Token: 0x02001253 RID: 4691
[RequireComponent(typeof(Animator))]
public class SleeperPreview : MonoBehaviour
{
	// Token: 0x06009562 RID: 38242 RVA: 0x00388B6F File Offset: 0x00386D6F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x06009563 RID: 38243 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetPose(int pose)
	{
	}

	// Token: 0x06009564 RID: 38244 RVA: 0x00388B7D File Offset: 0x00386D7D
	public void SetRotation(float rot)
	{
		base.transform.rotation = Quaternion.AngleAxis(rot, Vector3.up);
	}

	// Token: 0x04006FE9 RID: 28649
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
