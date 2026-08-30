using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

// Token: 0x020000E6 RID: 230
public class IKController : MonoBehaviour
{
	// Token: 0x060005CF RID: 1487 RVA: 0x00029E5C File Offset: 0x0002805C
	public unsafe void Start()
	{
		this.animator = base.GetComponent<Animator>();
		Transform transform = base.transform.Find("IKRig");
		if (transform)
		{
			this.rig = transform.GetComponent<Rig>();
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				TwoBoneIKConstraint component = transform.GetChild(i).GetComponent<TwoBoneIKConstraint>();
				if (component)
				{
					TwoBoneIKConstraintData twoBoneIKConstraintData = *component.data;
					Transform target = twoBoneIKConstraintData.target;
					if (target)
					{
						int num = this.NameToIndex(target.name);
						if (num >= 0)
						{
							IKController.Constraint constraint;
							constraint.tbConstraint = component;
							constraint.originalWeight = component.weight;
							constraint.originalTargetT = target;
							this.rigConstraints[num] = constraint;
						}
					}
				}
			}
			this.ModifyRig();
		}
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00029F2C File Offset: 0x0002812C
	[PublicizedFrom(EAccessModifier.Private)]
	public int NameToIndex(string name)
	{
		for (int i = 0; i < IKController.IKNames.Length; i++)
		{
			if (name == IKController.IKNames[i])
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00029F5D File Offset: 0x0002815D
	public void SetTargets(List<IKController.Target> _targets)
	{
		this.targets = _targets;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00029F66 File Offset: 0x00028166
	public void Cleanup()
	{
		this.targets = null;
		if (this.rig)
		{
			this.ModifyRig();
		}
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00029F84 File Offset: 0x00028184
	public void ModifyRig()
	{
		if (this.targets == null)
		{
			for (int i = 0; i < 4; i++)
			{
				IKController.Constraint constraint = this.rigConstraints[i];
				if (constraint.originalTargetT)
				{
					TwoBoneIKConstraint tbConstraint = constraint.tbConstraint;
					tbConstraint.weight = constraint.originalWeight;
					tbConstraint.data.target = constraint.originalTargetT;
					Transform transform = tbConstraint.transform;
					transform.position = Vector3.zero;
					transform.rotation = Quaternion.identity;
				}
			}
		}
		else
		{
			Transform transform2 = base.transform;
			for (int j = 0; j < this.targets.Count; j++)
			{
				IKController.Target target = this.targets[j];
				int avatarGoal = (int)target.avatarGoal;
				TwoBoneIKConstraint tbConstraint2 = this.rigConstraints[avatarGoal].tbConstraint;
				if (tbConstraint2)
				{
					Transform transform3 = target.transform;
					if (!transform3)
					{
						transform3 = tbConstraint2.transform;
						Matrix4x4 localToWorldMatrix = transform2.localToWorldMatrix;
						Vector3 vector = localToWorldMatrix.MultiplyPoint(target.position);
						transform3.position = vector;
						Quaternion quaternion = localToWorldMatrix.rotation * Quaternion.Euler(target.rotation);
						transform3.rotation = quaternion;
					}
					tbConstraint2.weight = 1f;
					tbConstraint2.data.target = transform3;
				}
			}
		}
		base.GetComponent<RigBuilder>().Build();
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0002A0E0 File Offset: 0x000282E0
	public void OnAnimatorIK()
	{
		if (!this.animator)
		{
			return;
		}
		if (this.targets == null)
		{
			for (int i = 0; i < 4; i++)
			{
				this.animator.SetIKPositionWeight((AvatarIKGoal)i, 0f);
				this.animator.SetIKRotationWeight((AvatarIKGoal)i, 0f);
			}
			return;
		}
		Transform transform = base.transform;
		for (int j = 0; j < this.targets.Count; j++)
		{
			IKController.Target target = this.targets[j];
			this.animator.SetIKPositionWeight(target.avatarGoal, 1f);
			this.animator.SetIKRotationWeight(target.avatarGoal, 1f);
			Transform transform2 = target.transform;
			if (!transform2)
			{
				Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
				Vector3 vector = localToWorldMatrix.MultiplyPoint(target.position);
				this.animator.SetIKPosition(target.avatarGoal, vector);
				Quaternion quaternion = localToWorldMatrix.rotation * Quaternion.Euler(target.rotation);
				this.animator.SetIKRotation(target.avatarGoal, quaternion);
			}
			else
			{
				this.animator.SetIKPosition(target.avatarGoal, transform2.position);
				this.animator.SetIKRotation(target.avatarGoal, transform2.rotation);
			}
		}
	}

	// Token: 0x04000683 RID: 1667
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cTargetTypeCount = 4;

	// Token: 0x04000684 RID: 1668
	public static string[] IKNames = new string[] { "IKFootL", "IKFootR", "IKHandL", "IKHandR" };

	// Token: 0x04000685 RID: 1669
	public List<IKController.Target> targets;

	// Token: 0x04000686 RID: 1670
	public IKController.Constraint[] rigConstraints = new IKController.Constraint[4];

	// Token: 0x04000687 RID: 1671
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;

	// Token: 0x04000688 RID: 1672
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rig rig;

	// Token: 0x020000E7 RID: 231
	[Serializable]
	public struct Target
	{
		// Token: 0x04000689 RID: 1673
		public AvatarIKGoal avatarGoal;

		// Token: 0x0400068A RID: 1674
		public Transform transform;

		// Token: 0x0400068B RID: 1675
		public Vector3 position;

		// Token: 0x0400068C RID: 1676
		public Vector3 rotation;
	}

	// Token: 0x020000E8 RID: 232
	[Serializable]
	public struct Constraint
	{
		// Token: 0x0400068D RID: 1677
		public TwoBoneIKConstraint tbConstraint;

		// Token: 0x0400068E RID: 1678
		public float originalWeight;

		// Token: 0x0400068F RID: 1679
		public Transform originalTargetT;
	}
}
