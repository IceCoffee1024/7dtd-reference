using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001602 RID: 5634
public class vp_FPWeaponMeleeAttack : vp_Component
{
	// Token: 0x1700148E RID: 5262
	// (get) Token: 0x0600B0B2 RID: 45234 RVA: 0x0041F188 File Offset: 0x0041D388
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

	// Token: 0x0600B0B3 RID: 45235 RVA: 0x0041F1C0 File Offset: 0x0041D3C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_Controller = (vp_FPController)base.Root.GetComponent(typeof(vp_FPController));
		this.m_Camera = (vp_FPCamera)base.Root.GetComponentInChildren(typeof(vp_FPCamera));
		this.m_Weapon = (vp_FPWeapon)base.Transform.GetComponent(typeof(vp_FPWeapon));
	}

	// Token: 0x0600B0B4 RID: 45236 RVA: 0x0041F233 File Offset: 0x0041D433
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.UpdateAttack();
	}

	// Token: 0x0600B0B5 RID: 45237 RVA: 0x0041F244 File Offset: 0x0041D444
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

	// Token: 0x0600B0B6 RID: 45238 RVA: 0x0041F300 File Offset: 0x0041D500
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

	// Token: 0x0600B0B7 RID: 45239 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Attack()
	{
	}

	// Token: 0x0600B0B8 RID: 45240 RVA: 0x0041F378 File Offset: 0x0041D578
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

	// Token: 0x0600B0B9 RID: 45241 RVA: 0x0041F46C File Offset: 0x0041D66C
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

	// Token: 0x0600B0BA RID: 45242 RVA: 0x0041F4F4 File Offset: 0x0041D6F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyRecoil()
	{
		this.m_Weapon.StopSprings();
		this.m_Weapon.AddForce(this.ImpactPositionSpringRecoil, this.ImpactRotationSpringRecoil);
		this.m_Weapon.AddForce2(this.ImpactPositionSpring2Recoil, this.ImpactRotationSpring2Recoil);
		this.Reset();
	}

	// Token: 0x0600B0BB RID: 45243 RVA: 0x0041F540 File Offset: 0x0041D740
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

	// Token: 0x04008452 RID: 33874
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_Weapon;

	// Token: 0x04008453 RID: 33875
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x04008454 RID: 33876
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x04008455 RID: 33877
	public string WeaponStatePull = "Pull";

	// Token: 0x04008456 RID: 33878
	public string WeaponStateSwing = "Swing";

	// Token: 0x04008457 RID: 33879
	public float SwingDelay = 0.5f;

	// Token: 0x04008458 RID: 33880
	public float SwingDuration = 0.5f;

	// Token: 0x04008459 RID: 33881
	public float SwingRate = 1f;

	// Token: 0x0400845A RID: 33882
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedSwingTime;

	// Token: 0x0400845B RID: 33883
	public int SwingSoftForceFrames = 50;

	// Token: 0x0400845C RID: 33884
	public Vector3 SwingPositionSoftForce = new Vector3(-0.5f, -0.1f, 0.3f);

	// Token: 0x0400845D RID: 33885
	public Vector3 SwingRotationSoftForce = new Vector3(50f, -25f, 0f);

	// Token: 0x0400845E RID: 33886
	public float ImpactTime = 0.11f;

	// Token: 0x0400845F RID: 33887
	public Vector3 ImpactPositionSpringRecoil = new Vector3(0.01f, 0.03f, -0.05f);

	// Token: 0x04008460 RID: 33888
	public Vector3 ImpactPositionSpring2Recoil = Vector3.zero;

	// Token: 0x04008461 RID: 33889
	public Vector3 ImpactRotationSpringRecoil = Vector3.zero;

	// Token: 0x04008462 RID: 33890
	public Vector3 ImpactRotationSpring2Recoil = new Vector3(0f, 0f, 10f);

	// Token: 0x04008463 RID: 33891
	public string DamageMethodName = "Damage";

	// Token: 0x04008464 RID: 33892
	public float Damage = 5f;

	// Token: 0x04008465 RID: 33893
	public float DamageRadius = 0.3f;

	// Token: 0x04008466 RID: 33894
	public float DamageRange = 2f;

	// Token: 0x04008467 RID: 33895
	public float DamageForce = 1000f;

	// Token: 0x04008468 RID: 33896
	public bool AttackPickRandomState = true;

	// Token: 0x04008469 RID: 33897
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_AttackCurrent;

	// Token: 0x0400846A RID: 33898
	public float SparkFactor = 0.1f;

	// Token: 0x0400846B RID: 33899
	public GameObject m_DustPrefab;

	// Token: 0x0400846C RID: 33900
	public GameObject m_SparkPrefab;

	// Token: 0x0400846D RID: 33901
	public GameObject m_DebrisPrefab;

	// Token: 0x0400846E RID: 33902
	public List<global::UnityEngine.Object> SoundSwing = new List<global::UnityEngine.Object>();

	// Token: 0x0400846F RID: 33903
	public List<global::UnityEngine.Object> SoundImpact = new List<global::UnityEngine.Object>();

	// Token: 0x04008470 RID: 33904
	public Vector2 SoundSwingPitch = new Vector2(0.5f, 1.5f);

	// Token: 0x04008471 RID: 33905
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04008472 RID: 33906
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDelayTimer = new vp_Timer.Handle();

	// Token: 0x04008473 RID: 33907
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ImpactTimer = new vp_Timer.Handle();

	// Token: 0x04008474 RID: 33908
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDurationTimer = new vp_Timer.Handle();

	// Token: 0x04008475 RID: 33909
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ResetTimer = new vp_Timer.Handle();

	// Token: 0x04008476 RID: 33910
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
