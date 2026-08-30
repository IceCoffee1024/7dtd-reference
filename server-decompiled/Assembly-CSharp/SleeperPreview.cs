using System;
using UnityEngine;

// Token: 0x0200120D RID: 4621
[RequireComponent(typeof(Animator))]
public class SleeperPreview : MonoBehaviour
{
	// Token: 0x06009379 RID: 37753 RVA: 0x0037CC4F File Offset: 0x0037AE4F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x0600937A RID: 37754 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetPose(int pose)
	{
	}

	// Token: 0x0600937B RID: 37755 RVA: 0x0037CC5D File Offset: 0x0037AE5D
	public void SetRotation(float rot)
	{
		base.transform.rotation = Quaternion.AngleAxis(rot, Vector3.up);
	}

	// Token: 0x04006E65 RID: 28261
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
