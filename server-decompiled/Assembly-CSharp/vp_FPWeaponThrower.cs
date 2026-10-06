using System;
using UnityEngine;

// Token: 0x02001624 RID: 5668
public class vp_FPWeaponThrower : vp_WeaponThrower
{
	// Token: 0x170014D4 RID: 5332
	// (get) Token: 0x0600B160 RID: 45408 RVA: 0x00423B99 File Offset: 0x00421D99
	public vp_FPWeapon FPWeapon
	{
		get
		{
			if (this.m_FPWeapon == null)
			{
				this.m_FPWeapon = (vp_FPWeapon)base.Transform.GetComponent(typeof(vp_FPWeapon));
			}
			return this.m_FPWeapon;
		}
	}

	// Token: 0x170014D5 RID: 5333
	// (get) Token: 0x0600B161 RID: 45409 RVA: 0x00423BCF File Offset: 0x00421DCF
	public vp_FPWeaponShooter FPWeaponShooter
	{
		get
		{
			if (this.m_FPWeaponShooter == null)
			{
				this.m_FPWeaponShooter = (vp_FPWeaponShooter)base.Transform.GetComponent(typeof(vp_FPWeaponShooter));
			}
			return this.m_FPWeaponShooter;
		}
	}

	// Token: 0x170014D6 RID: 5334
	// (get) Token: 0x0600B162 RID: 45410 RVA: 0x00423C08 File Offset: 0x00421E08
	public Transform FirePosition
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_FirePosition == null)
			{
				GameObject gameObject = new GameObject("ThrownWeaponFirePosition");
				this.m_FirePosition = gameObject.transform;
				this.m_FirePosition.parent = Camera.main.transform;
			}
			return this.m_FirePosition;
		}
	}

	// Token: 0x0600B163 RID: 45411 RVA: 0x00423C55 File Offset: 0x00421E55
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_OriginalLookDownActive = this.FPWeapon.LookDownActive;
	}

	// Token: 0x0600B164 RID: 45412 RVA: 0x00423C70 File Offset: 0x00421E70
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void RewindAnimation()
	{
		if (!base.Player.IsFirstPerson.Get())
		{
			return;
		}
		if (this.FPWeapon == null)
		{
			return;
		}
		if (this.FPWeapon.WeaponModel == null)
		{
			return;
		}
		if (this.FPWeapon.WeaponModel.GetComponent<Animation>() == null)
		{
			return;
		}
		if (this.FPWeaponShooter == null)
		{
			return;
		}
		if (this.FPWeaponShooter.AnimationFire == null)
		{
			return;
		}
		this.FPWeapon.WeaponModel.GetComponent<Animation>()[this.FPWeaponShooter.AnimationFire.name].time = 0f;
		this.FPWeapon.WeaponModel.GetComponent<Animation>().Play();
		this.FPWeapon.WeaponModel.GetComponent<Animation>().Sample();
		this.FPWeapon.WeaponModel.GetComponent<Animation>().Stop();
	}

	// Token: 0x0600B165 RID: 45413 RVA: 0x00423D64 File Offset: 0x00421F64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Attack()
	{
		base.OnStart_Attack();
		if (base.Player.IsFirstPerson.Get())
		{
			base.Shooter.m_ProjectileSpawnPoint = this.FirePosition.gameObject;
			this.FirePosition.localPosition = this.FirePositionOffset;
			this.FirePosition.localEulerAngles = Vector3.zero;
		}
		else
		{
			base.Shooter.m_ProjectileSpawnPoint = base.Weapon.Weapon3rdPersonModel;
		}
		this.FPWeapon.LookDownActive = false;
		vp_Timer.In(base.Shooter.ProjectileSpawnDelay, delegate
		{
			if (!base.HaveAmmoForCurrentWeapon)
			{
				this.FPWeapon.SetState("ReWield", true, false, false);
				this.FPWeapon.Refresh();
				vp_Timer.In(1f, delegate
				{
					if (!base.Player.SetNextWeapon.Try())
					{
						vp_Timer.In(0.5f, delegate
						{
							this.RewindAnimation();
							base.Player.SetWeapon.Start(0f);
						}, this.m_Timer2);
					}
				}, null);
				return;
			}
			if (base.Player.IsFirstPerson.Get())
			{
				this.FPWeapon.SetState("ReWield", true, false, false);
				this.FPWeapon.Refresh();
				vp_Timer.In(1f, delegate
				{
					this.RewindAnimation();
					this.FPWeapon.Rendering = true;
					this.FPWeapon.SetState("ReWield", false, false, false);
					this.FPWeapon.Refresh();
				}, this.m_Timer3);
				return;
			}
			vp_Timer.In(0.5f, delegate
			{
				base.Player.Attack.Stop(0f);
			}, this.m_Timer4);
		}, this.m_Timer1);
	}

	// Token: 0x0600B166 RID: 45414 RVA: 0x00423E0B File Offset: 0x0042200B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_SetWeapon()
	{
		this.RewindAnimation();
	}

	// Token: 0x0600B167 RID: 45415 RVA: 0x00423E13 File Offset: 0x00422013
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStop_Attack()
	{
		base.OnStop_Attack();
		this.FPWeapon.LookDownActive = this.m_OriginalLookDownActive;
	}

	// Token: 0x040084D7 RID: 34007
	public Vector3 FirePositionOffset = new Vector3(0.35f, 0f, 0f);

	// Token: 0x040084D8 RID: 34008
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_OriginalLookDownActive;

	// Token: 0x040084D9 RID: 34009
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer1 = new vp_Timer.Handle();

	// Token: 0x040084DA RID: 34010
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer2 = new vp_Timer.Handle();

	// Token: 0x040084DB RID: 34011
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer3 = new vp_Timer.Handle();

	// Token: 0x040084DC RID: 34012
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer4 = new vp_Timer.Handle();

	// Token: 0x040084DD RID: 34013
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_FPWeapon;

	// Token: 0x040084DE RID: 34014
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeaponShooter m_FPWeaponShooter;

	// Token: 0x040084DF RID: 34015
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_FirePosition;
}
