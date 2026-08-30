using System;
using UnityEngine;

// Token: 0x020015AB RID: 5547
[RequireComponent(typeof(vp_Shooter))]
public class vp_SimpleAITurret : MonoBehaviour
{
	// Token: 0x0600AD88 RID: 44424 RVA: 0x0040AF97 File Offset: 0x00409197
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Shooter = base.GetComponent<vp_Shooter>();
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AD89 RID: 44425 RVA: 0x0040AFB1 File Offset: 0x004091B1
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

	// Token: 0x0600AD8A RID: 44426 RVA: 0x0040AFF4 File Offset: 0x004091F4
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

	// Token: 0x0600AD8B RID: 44427 RVA: 0x0040B080 File Offset: 0x00409280
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

	// Token: 0x04008108 RID: 33032
	public float ViewRange = 10f;

	// Token: 0x04008109 RID: 33033
	public float AimSpeed = 50f;

	// Token: 0x0400810A RID: 33034
	public float WakeInterval = 2f;

	// Token: 0x0400810B RID: 33035
	public float FireAngle = 10f;

	// Token: 0x0400810C RID: 33036
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Shooter m_Shooter;

	// Token: 0x0400810D RID: 33037
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400810E RID: 33038
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Target;

	// Token: 0x0400810F RID: 33039
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
