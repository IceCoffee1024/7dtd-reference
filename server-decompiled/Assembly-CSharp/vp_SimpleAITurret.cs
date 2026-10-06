using System;
using UnityEngine;

// Token: 0x020015CD RID: 5581
[RequireComponent(typeof(vp_Shooter))]
public class vp_SimpleAITurret : MonoBehaviour
{
	// Token: 0x0600AE3B RID: 44603 RVA: 0x0040FDEB File Offset: 0x0040DFEB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Shooter = base.GetComponent<vp_Shooter>();
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AE3C RID: 44604 RVA: 0x0040FE05 File Offset: 0x0040E005
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (!this.m_Timer.Active)
		{
			vp_Timer.In(this.WakeInterval, delegate
			{
				if (this.m_Target == null)
				{
					this.m_Target = this.ScanForLocalPlayer();
					return;
				}
				this.m_Target = null;
			}, this.m_Timer);
		}
		if (this.m_Target != null)
		{
			this.AttackTarget();
		}
	}

	// Token: 0x0600AE3D RID: 44605 RVA: 0x0040FE48 File Offset: 0x0040E048
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Transform ScanForLocalPlayer()
	{
		foreach (Collider collider in Physics.OverlapSphere(this.m_Transform.position, this.ViewRange, 1073741824))
		{
			RaycastHit raycastHit;
			Physics.Linecast(this.m_Transform.position, collider.transform.position + Vector3.up, out raycastHit);
			if (!(raycastHit.collider != null) || !(raycastHit.collider != collider))
			{
				return collider.transform;
			}
		}
		return null;
	}

	// Token: 0x0600AE3E RID: 44606 RVA: 0x0040FED4 File Offset: 0x0040E0D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AttackTarget()
	{
		Quaternion quaternion = Quaternion.LookRotation(this.m_Target.GetComponent<Collider>().bounds.center - this.m_Transform.position);
		this.m_Transform.rotation = Quaternion.RotateTowards(this.m_Transform.rotation, quaternion, Time.deltaTime * this.AimSpeed);
		if (Mathf.Abs(vp_3DUtility.LookAtAngleHorizontal(this.m_Transform.position, this.m_Transform.forward, this.m_Target.position)) < this.FireAngle)
		{
			this.m_Shooter.TryFire();
		}
	}

	// Token: 0x0400818B RID: 33163
	public float ViewRange = 10f;

	// Token: 0x0400818C RID: 33164
	public float AimSpeed = 50f;

	// Token: 0x0400818D RID: 33165
	public float WakeInterval = 2f;

	// Token: 0x0400818E RID: 33166
	public float FireAngle = 10f;

	// Token: 0x0400818F RID: 33167
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Shooter m_Shooter;

	// Token: 0x04008190 RID: 33168
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008191 RID: 33169
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Target;

	// Token: 0x04008192 RID: 33170
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
