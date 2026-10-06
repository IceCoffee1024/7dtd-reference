using System;
using UnityEngine;

// Token: 0x02001241 RID: 4673
[RequireComponent(typeof(Animator))]
public class SleeperPreview : MonoBehaviour
{
	// Token: 0x0600950B RID: 38155 RVA: 0x00386773 File Offset: 0x00384973
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x0600950C RID: 38156 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetPose(int pose)
	{
	}

	// Token: 0x0600950D RID: 38157 RVA: 0x00386781 File Offset: 0x00384981
	public void SetRotation(float rot)
	{
		base.transform.rotation = Quaternion.AngleAxis(rot, Vector3.up);
	}

	// Token: 0x04006FA8 RID: 28584
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
