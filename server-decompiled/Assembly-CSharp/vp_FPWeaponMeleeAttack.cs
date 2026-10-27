using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001633 RID: 5683
public class vp_FPWeaponMeleeAttack : vp_Component
{
	// Token: 0x170014DD RID: 5341
	// (get) Token: 0x0600B19C RID: 45468 RVA: 0x00425268 File Offset: 0x00423468
	public vp_FPPlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_Player == null && base.EventHandler != null)
			{
				this.m_Player = (vp_FPPlayerEventHandler)base.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x0600B19D RID: 45469 RVA: 0x004252A0 File Offset: 0x004234A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_Controller = (vp_FPController)base.Root.GetComponent(typeof(vp_FPController));
		this.m_Camera = (vp_FPCamera)base.Root.GetComponentInChildren(typeof(vp_FPCamera));
		this.m_Weapon = (vp_FPWeapon)base.Transform.GetComponent(typeof(vp_FPWeapon));
	}

	// Token: 0x0600B19E RID: 45470 RVA: 0x00425313 File Offset: 0x00423513
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.UpdateAttack();
	}

	// Token: 0x0600B19F RID: 45471 RVA: 0x00425324 File Offset: 0x00423524
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateAttack()
	{
		if (!this.Player.Attack.Active)
		{
			return;
		}
		if (this.Player.SetWeapon.Active)
		{
			return;
		}
		if (this.m_Weapon == null)
		{
			return;
		}
		if (!this.m_Weapon.Wielded)
		{
			return;
		}
		if (Time.time < this.m_NextAllowedSwingTime)
		{
			return;
		}
		this.m_NextAllowedSwingTime = Time.time + this.SwingRate;
		if (this.AttackPickRandomState)
		{
			this.PickAttack();
		}
		this.m_Weapon.SetState(this.WeaponStatePull, true, false, false);
		this.m_Weapon.Refresh();
		vp_Timer.In(this.SwingDelay, delegate
		{
			if (this.SoundSwing.Count > 0)
			{
				base.Audio.pitch = global::UnityEngine.Random.Range(this.SoundSwingPitch.x, this.SoundSwingPitch.y) * Time.timeScale;
				base.Audio.clip = (AudioClip)this.SoundSwing[global::UnityEngine.Random.Range(0, this.SoundSwing.Count)];
				base.Audio.Play();
			}
			this.m_Weapon.SetState(this.WeaponStatePull, false, false, false);
			this.m_Weapon.SetState(this.WeaponStateSwing, true, false, false);
			this.m_Weapon.Refresh();
			this.m_Weapon.AddSoftForce(this.SwingPositionSoftForce, this.SwingRotationSoftForce, this.SwingSoftForceFrames);
			vp_Timer.In(this.ImpactTime, delegate
			{
				RaycastHit raycastHit;
				Physics.SphereCast(new Ray(new Vector3(this.m_Controller.Transform.position.x, this.m_Camera.Transform.position.y, this.m_Controller.Transform.position.z), this.m_Camera.Transform.forward), this.DamageRadius, out raycastHit, this.DamageRange, -538750989);
				if (raycastHit.collider != null)
				{
					this.SpawnImpactFX(raycastHit);
					this.ApplyDamage(raycastHit);
					this.ApplyRecoil();
					return;
				}
				vp_Timer.In(this.SwingDuration - this.ImpactTime, delegate
				{
					this.m_Weapon.StopSprings();
					this.Reset();
				}, this.SwingDurationTimer);
			}, this.ImpactTimer);
		}, this.SwingDelayTimer);
	}

	// Token: 0x0600B1A0 RID: 45472 RVA: 0x004253E0 File Offset: 0x004235E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void PickAttack()
	{
		int num = this.States.Count - 1;
		do
		{
			num = global::UnityEngine.Random.Range(0, this.States.Count - 1);
		}
		while (this.States.Count > 1 && num == this.m_AttackCurrent && global::UnityEngine.Random.value < 0.5f);
		this.m_AttackCurrent = num;
		base.SetState(this.States[this.m_AttackCurrent].Name, true, false, false);
	}

	// Token: 0x0600B1A1 RID: 45473 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Attack()
	{
	}

	// Token: 0x0600B1A2 RID: 45474 RVA: 0x00425458 File Offset: 0x00423658
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnImpactFX(RaycastHit hit)
	{
		Quaternion quaternion = Quaternion.LookRotation(hit.normal);
		if (this.m_DustPrefab != null)
		{
			vp_Utility.Instantiate(this.m_DustPrefab, hit.point, quaternion);
		}
		if (this.m_SparkPrefab != null && global::UnityEngine.Random.value < this.SparkFactor)
		{
			vp_Utility.Instantiate(this.m_SparkPrefab, hit.point, quaternion);
		}
		if (this.m_DebrisPrefab != null)
		{
			vp_Utility.Instantiate(this.m_DebrisPrefab, hit.point, quaternion);
		}
		if (this.SoundImpact.Count > 0)
		{
			base.Audio.pitch = global::UnityEngine.Random.Range(this.SoundImpactPitch.x, this.SoundImpactPitch.y) * Time.timeScale;
			base.Audio.PlayOneShot((AudioClip)this.SoundImpact[global::UnityEngine.Random.Range(0, this.SoundImpact.Count)]);
		}
	}

	// Token: 0x0600B1A3 RID: 45475 RVA: 0x0042554C File Offset: 0x0042374C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyDamage(RaycastHit hit)
	{
		hit.collider.SendMessage(this.DamageMethodName, this.Damage, SendMessageOptions.DontRequireReceiver);
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (attachedRigidbody != null && !attachedRigidbody.isKinematic)
		{
			attachedRigidbody.AddForceAtPosition(this.m_Camera.Transform.forward * this.DamageForce / Time.timeScale / vp_TimeUtility.AdjustedTimeScale, hit.point);
		}
	}

	// Token: 0x0600B1A4 RID: 45476 RVA: 0x004255D4 File Offset: 0x004237D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyRecoil()
	{
		this.m_Weapon.StopSprings();
		this.m_Weapon.AddForce(this.ImpactPositionSpringRecoil, this.ImpactRotationSpringRecoil);
		this.m_Weapon.AddForce2(this.ImpactPositionSpring2Recoil, this.ImpactRotationSpring2Recoil);
		this.Reset();
	}

	// Token: 0x0600B1A5 RID: 45477 RVA: 0x00425620 File Offset: 0x00423820
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reset()
	{
		vp_Timer.In(0.05f, delegate
		{
			if (this.m_Weapon != null)
			{
				this.m_Weapon.SetState(this.WeaponStatePull, false, false, false);
				this.m_Weapon.SetState(this.WeaponStateSwing, false, false, false);
				this.m_Weapon.Refresh();
				if (this.AttackPickRandomState)
				{
					base.ResetState();
				}
			}
		}, this.ResetTimer);
	}

	// Token: 0x040084E9 RID: 34025
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_Weapon;

	// Token: 0x040084EA RID: 34026
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x040084EB RID: 34027
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040084EC RID: 34028
	public string WeaponStatePull = "Pull";

	// Token: 0x040084ED RID: 34029
	public string WeaponStateSwing = "Swing";

	// Token: 0x040084EE RID: 34030
	public float SwingDelay = 0.5f;

	// Token: 0x040084EF RID: 34031
	public float SwingDuration = 0.5f;

	// Token: 0x040084F0 RID: 34032
	public float SwingRate = 1f;

	// Token: 0x040084F1 RID: 34033
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedSwingTime;

	// Token: 0x040084F2 RID: 34034
	public int SwingSoftForceFrames = 50;

	// Token: 0x040084F3 RID: 34035
	public Vector3 SwingPositionSoftForce = new Vector3(-0.5f, -0.1f, 0.3f);

	// Token: 0x040084F4 RID: 34036
	public Vector3 SwingRotationSoftForce = new Vector3(50f, -25f, 0f);

	// Token: 0x040084F5 RID: 34037
	public float ImpactTime = 0.11f;

	// Token: 0x040084F6 RID: 34038
	public Vector3 ImpactPositionSpringRecoil = new Vector3(0.01f, 0.03f, -0.05f);

	// Token: 0x040084F7 RID: 34039
	public Vector3 ImpactPositionSpring2Recoil = Vector3.zero;

	// Token: 0x040084F8 RID: 34040
	public Vector3 ImpactRotationSpringRecoil = Vector3.zero;

	// Token: 0x040084F9 RID: 34041
	public Vector3 ImpactRotationSpring2Recoil = new Vector3(0f, 0f, 10f);

	// Token: 0x040084FA RID: 34042
	public string DamageMethodName = "Damage";

	// Token: 0x040084FB RID: 34043
	public float Damage = 5f;

	// Token: 0x040084FC RID: 34044
	public float DamageRadius = 0.3f;

	// Token: 0x040084FD RID: 34045
	public float DamageRange = 2f;

	// Token: 0x040084FE RID: 34046
	public float DamageForce = 1000f;

	// Token: 0x040084FF RID: 34047
	public bool AttackPickRandomState = true;

	// Token: 0x04008500 RID: 34048
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_AttackCurrent;

	// Token: 0x04008501 RID: 34049
	public float SparkFactor = 0.1f;

	// Token: 0x04008502 RID: 34050
	public GameObject m_DustPrefab;

	// Token: 0x04008503 RID: 34051
	public GameObject m_SparkPrefab;

	// Token: 0x04008504 RID: 34052
	public GameObject m_DebrisPrefab;

	// Token: 0x04008505 RID: 34053
	public List<global::UnityEngine.Object> SoundSwing = new List<global::UnityEngine.Object>();

	// Token: 0x04008506 RID: 34054
	public List<global::UnityEngine.Object> SoundImpact = new List<global::UnityEngine.Object>();

	// Token: 0x04008507 RID: 34055
	public Vector2 SoundSwingPitch = new Vector2(0.5f, 1.5f);

	// Token: 0x04008508 RID: 34056
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04008509 RID: 34057
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDelayTimer = new vp_Timer.Handle();

	// Token: 0x0400850A RID: 34058
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ImpactTimer = new vp_Timer.Handle();

	// Token: 0x0400850B RID: 34059
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDurationTimer = new vp_Timer.Handle();

	// Token: 0x0400850C RID: 34060
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ResetTimer = new vp_Timer.Handle();

	// Token: 0x0400850D RID: 34061
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
