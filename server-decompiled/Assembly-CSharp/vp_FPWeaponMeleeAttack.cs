using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E7 RID: 5607
public class vp_FPWeaponMeleeAttack : vp_Component
{
	// Token: 0x17001455 RID: 5205
	// (get) Token: 0x0600AFB5 RID: 44981 RVA: 0x00419E48 File Offset: 0x00418048
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

	// Token: 0x0600AFB6 RID: 44982 RVA: 0x00419E80 File Offset: 0x00418080
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_Controller = (vp_FPController)base.Root.GetComponent(typeof(vp_FPController));
		this.m_Camera = (vp_FPCamera)base.Root.GetComponentInChildren(typeof(vp_FPCamera));
		this.m_Weapon = (vp_FPWeapon)base.Transform.GetComponent(typeof(vp_FPWeapon));
	}

	// Token: 0x0600AFB7 RID: 44983 RVA: 0x00419EF3 File Offset: 0x004180F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.UpdateAttack();
	}

	// Token: 0x0600AFB8 RID: 44984 RVA: 0x00419F04 File Offset: 0x00418104
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

	// Token: 0x0600AFB9 RID: 44985 RVA: 0x00419FC0 File Offset: 0x004181C0
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

	// Token: 0x0600AFBA RID: 44986 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Attack()
	{
	}

	// Token: 0x0600AFBB RID: 44987 RVA: 0x0041A038 File Offset: 0x00418238
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

	// Token: 0x0600AFBC RID: 44988 RVA: 0x0041A12C File Offset: 0x0041832C
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

	// Token: 0x0600AFBD RID: 44989 RVA: 0x0041A1B4 File Offset: 0x004183B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyRecoil()
	{
		this.m_Weapon.StopSprings();
		this.m_Weapon.AddForce(this.ImpactPositionSpringRecoil, this.ImpactRotationSpringRecoil);
		this.m_Weapon.AddForce2(this.ImpactPositionSpring2Recoil, this.ImpactRotationSpring2Recoil);
		this.Reset();
	}

	// Token: 0x0600AFBE RID: 44990 RVA: 0x0041A200 File Offset: 0x00418400
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

	// Token: 0x04008379 RID: 33657
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_Weapon;

	// Token: 0x0400837A RID: 33658
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x0400837B RID: 33659
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x0400837C RID: 33660
	public string WeaponStatePull = "Pull";

	// Token: 0x0400837D RID: 33661
	public string WeaponStateSwing = "Swing";

	// Token: 0x0400837E RID: 33662
	public float SwingDelay = 0.5f;

	// Token: 0x0400837F RID: 33663
	public float SwingDuration = 0.5f;

	// Token: 0x04008380 RID: 33664
	public float SwingRate = 1f;

	// Token: 0x04008381 RID: 33665
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedSwingTime;

	// Token: 0x04008382 RID: 33666
	public int SwingSoftForceFrames = 50;

	// Token: 0x04008383 RID: 33667
	public Vector3 SwingPositionSoftForce = new Vector3(-0.5f, -0.1f, 0.3f);

	// Token: 0x04008384 RID: 33668
	public Vector3 SwingRotationSoftForce = new Vector3(50f, -25f, 0f);

	// Token: 0x04008385 RID: 33669
	public float ImpactTime = 0.11f;

	// Token: 0x04008386 RID: 33670
	public Vector3 ImpactPositionSpringRecoil = new Vector3(0.01f, 0.03f, -0.05f);

	// Token: 0x04008387 RID: 33671
	public Vector3 ImpactPositionSpring2Recoil = Vector3.zero;

	// Token: 0x04008388 RID: 33672
	public Vector3 ImpactRotationSpringRecoil = Vector3.zero;

	// Token: 0x04008389 RID: 33673
	public Vector3 ImpactRotationSpring2Recoil = new Vector3(0f, 0f, 10f);

	// Token: 0x0400838A RID: 33674
	public string DamageMethodName = "Damage";

	// Token: 0x0400838B RID: 33675
	public float Damage = 5f;

	// Token: 0x0400838C RID: 33676
	public float DamageRadius = 0.3f;

	// Token: 0x0400838D RID: 33677
	public float DamageRange = 2f;

	// Token: 0x0400838E RID: 33678
	public float DamageForce = 1000f;

	// Token: 0x0400838F RID: 33679
	public bool AttackPickRandomState = true;

	// Token: 0x04008390 RID: 33680
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_AttackCurrent;

	// Token: 0x04008391 RID: 33681
	public float SparkFactor = 0.1f;

	// Token: 0x04008392 RID: 33682
	public GameObject m_DustPrefab;

	// Token: 0x04008393 RID: 33683
	public GameObject m_SparkPrefab;

	// Token: 0x04008394 RID: 33684
	public GameObject m_DebrisPrefab;

	// Token: 0x04008395 RID: 33685
	public List<global::UnityEngine.Object> SoundSwing = new List<global::UnityEngine.Object>();

	// Token: 0x04008396 RID: 33686
	public List<global::UnityEngine.Object> SoundImpact = new List<global::UnityEngine.Object>();

	// Token: 0x04008397 RID: 33687
	public Vector2 SoundSwingPitch = new Vector2(0.5f, 1.5f);

	// Token: 0x04008398 RID: 33688
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04008399 RID: 33689
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDelayTimer = new vp_Timer.Handle();

	// Token: 0x0400839A RID: 33690
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ImpactTimer = new vp_Timer.Handle();

	// Token: 0x0400839B RID: 33691
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDurationTimer = new vp_Timer.Handle();

	// Token: 0x0400839C RID: 33692
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ResetTimer = new vp_Timer.Handle();

	// Token: 0x0400839D RID: 33693
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
