using System;
using UnityEngine;

// Token: 0x02001605 RID: 5637
public class vp_FPWeaponThrower : vp_WeaponThrower
{
	// Token: 0x17001491 RID: 5265
	// (get) Token: 0x0600B0CE RID: 45262 RVA: 0x0041FEBD File Offset: 0x0041E0BD
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

	// Token: 0x17001492 RID: 5266
	// (get) Token: 0x0600B0CF RID: 45263 RVA: 0x0041FEF3 File Offset: 0x0041E0F3
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

	// Token: 0x17001493 RID: 5267
	// (get) Token: 0x0600B0D0 RID: 45264 RVA: 0x0041FF2C File Offset: 0x0041E12C
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

	// Token: 0x0600B0D1 RID: 45265 RVA: 0x0041FF79 File Offset: 0x0041E179
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_OriginalLookDownActive = this.FPWeapon.LookDownActive;
	}

	// Token: 0x0600B0D2 RID: 45266 RVA: 0x0041FF94 File Offset: 0x0041E194
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

	// Token: 0x0600B0D3 RID: 45267 RVA: 0x00420088 File Offset: 0x0041E288
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

	// Token: 0x0600B0D4 RID: 45268 RVA: 0x0042012F File Offset: 0x0041E32F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_SetWeapon()
	{
		this.RewindAnimation();
	}

	// Token: 0x0600B0D5 RID: 45269 RVA: 0x00420137 File Offset: 0x0041E337
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStop_Attack()
	{
		base.OnStop_Attack();
		this.FPWeapon.LookDownActive = this.m_OriginalLookDownActive;
	}

	// Token: 0x04008481 RID: 33921
	public Vector3 FirePositionOffset = new Vector3(0.35f, 0f, 0f);

	// Token: 0x04008482 RID: 33922
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_OriginalLookDownActive;

	// Token: 0x04008483 RID: 33923
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer1 = new vp_Timer.Handle();

	// Token: 0x04008484 RID: 33924
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer2 = new vp_Timer.Handle();

	// Token: 0x04008485 RID: 33925
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer3 = new vp_Timer.Handle();

	// Token: 0x04008486 RID: 33926
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer4 = new vp_Timer.Handle();

	// Token: 0x04008487 RID: 33927
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_FPWeapon;

	// Token: 0x04008488 RID: 33928
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeaponShooter m_FPWeaponShooter;

	// Token: 0x04008489 RID: 33929
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_FirePosition;
}
