using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class ArchetypePreviewIKController : MonoBehaviour
{
	// Token: 0x060003AF RID: 943 RVA: 0x0001AFE0 File Offset: 0x000191E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.animator = base.GetComponent<Animator>();
		Transform transform = base.transform.FindInChilds("LeftFoot", false);
		Transform transform2 = base.transform.FindInChilds("RightFoot", false);
		this.leftFootPos = transform.position;
		this.leftFootRot = transform.rotation;
		this.rightFootPos = transform2.position;
		this.rightFootRot = transform2.rotation;
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0001B050 File Offset: 0x00019250
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAnimatorIK()
	{
		if (this.animator)
		{
			this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
			this.animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
			this.animator.SetIKPosition(AvatarIKGoal.LeftFoot, this.leftFootPos);
			this.animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.Euler(this.leftFootRot.eulerAngles.x + this.FootRotationModifier.x, this.leftFootRot.eulerAngles.y - this.FootRotationModifier.y, this.leftFootRot.eulerAngles.z + this.FootRotationModifier.z));
			this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
			this.animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
			this.animator.SetIKPosition(AvatarIKGoal.RightFoot, this.rightFootPos);
			this.animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.Euler(this.rightFootRot.eulerAngles.x + this.FootRotationModifier.x, this.rightFootRot.eulerAngles.y + this.FootRotationModifier.y, this.rightFootRot.eulerAngles.z + this.FootRotationModifier.z));
		}
	}

	// Token: 0x04000445 RID: 1093
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator animator;

	// Token: 0x04000446 RID: 1094
	public bool ikActive;

	// Token: 0x04000447 RID: 1095
	public Transform rightHandObj;

	// Token: 0x04000448 RID: 1096
	public Transform lookObj;

	// Token: 0x04000449 RID: 1097
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 leftFootPos;

	// Token: 0x0400044A RID: 1098
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 rightFootPos;

	// Token: 0x0400044B RID: 1099
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion leftFootRot;

	// Token: 0x0400044C RID: 1100
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Quaternion rightFootRot;

	// Token: 0x0400044D RID: 1101
	public Vector3 FootRotationModifier = new Vector3(-62f, -198f, -93.5f);
}
