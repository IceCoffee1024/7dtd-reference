using System;
using UnityEngine;

// Token: 0x02001636 RID: 5686
public class vp_FPWeaponThrower : vp_WeaponThrower
{
	// Token: 0x170014E0 RID: 5344
	// (get) Token: 0x0600B1B8 RID: 45496 RVA: 0x00425F9D File Offset: 0x0042419D
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

	// Token: 0x170014E1 RID: 5345
	// (get) Token: 0x0600B1B9 RID: 45497 RVA: 0x00425FD3 File Offset: 0x004241D3
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

	// Token: 0x170014E2 RID: 5346
	// (get) Token: 0x0600B1BA RID: 45498 RVA: 0x0042600C File Offset: 0x0042420C
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

	// Token: 0x0600B1BB RID: 45499 RVA: 0x00426059 File Offset: 0x00424259
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_OriginalLookDownActive = this.FPWeapon.LookDownActive;
	}

	// Token: 0x0600B1BC RID: 45500 RVA: 0x00426074 File Offset: 0x00424274
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

	// Token: 0x0600B1BD RID: 45501 RVA: 0x00426168 File Offset: 0x00424368
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

	// Token: 0x0600B1BE RID: 45502 RVA: 0x0042620F File Offset: 0x0042440F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_SetWeapon()
	{
		this.RewindAnimation();
	}

	// Token: 0x0600B1BF RID: 45503 RVA: 0x00426217 File Offset: 0x00424417
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStop_Attack()
	{
		base.OnStop_Attack();
		this.FPWeapon.LookDownActive = this.m_OriginalLookDownActive;
	}

	// Token: 0x04008518 RID: 34072
	public Vector3 FirePositionOffset = new Vector3(0.35f, 0f, 0f);

	// Token: 0x04008519 RID: 34073
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_OriginalLookDownActive;

	// Token: 0x0400851A RID: 34074
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer1 = new vp_Timer.Handle();

	// Token: 0x0400851B RID: 34075
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer2 = new vp_Timer.Handle();

	// Token: 0x0400851C RID: 34076
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer3 = new vp_Timer.Handle();

	// Token: 0x0400851D RID: 34077
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer4 = new vp_Timer.Handle();

	// Token: 0x0400851E RID: 34078
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_FPWeapon;

	// Token: 0x0400851F RID: 34079
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeaponShooter m_FPWeaponShooter;

	// Token: 0x04008520 RID: 34080
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_FirePosition;
}
