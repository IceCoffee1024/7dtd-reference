using System;
using UnityEngine;

// Token: 0x020015DF RID: 5599
[RequireComponent(typeof(vp_Shooter))]
public class vp_SimpleAITurret : MonoBehaviour
{
	// Token: 0x0600AE93 RID: 44691 RVA: 0x004121EF File Offset: 0x004103EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Shooter = base.GetComponent<vp_Shooter>();
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AE94 RID: 44692 RVA: 0x00412209 File Offset: 0x00410409
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

	// Token: 0x0600AE95 RID: 44693 RVA: 0x0041224C File Offset: 0x0041044C
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

	// Token: 0x0600AE96 RID: 44694 RVA: 0x004122D8 File Offset: 0x004104D8
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

	// Token: 0x040081CC RID: 33228
	public float ViewRange = 10f;

	// Token: 0x040081CD RID: 33229
	public float AimSpeed = 50f;

	// Token: 0x040081CE RID: 33230
	public float WakeInterval = 2f;

	// Token: 0x040081CF RID: 33231
	public float FireAngle = 10f;

	// Token: 0x040081D0 RID: 33232
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Shooter m_Shooter;

	// Token: 0x040081D1 RID: 33233
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081D2 RID: 33234
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Target;

	// Token: 0x040081D3 RID: 33235
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
