using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001621 RID: 5665
public class vp_FPWeaponMeleeAttack : vp_Component
{
	// Token: 0x170014D1 RID: 5329
	// (get) Token: 0x0600B144 RID: 45380 RVA: 0x00422E64 File Offset: 0x00421064
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

	// Token: 0x0600B145 RID: 45381 RVA: 0x00422E9C File Offset: 0x0042109C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_Controller = (vp_FPController)base.Root.GetComponent(typeof(vp_FPController));
		this.m_Camera = (vp_FPCamera)base.Root.GetComponentInChildren(typeof(vp_FPCamera));
		this.m_Weapon = (vp_FPWeapon)base.Transform.GetComponent(typeof(vp_FPWeapon));
	}

	// Token: 0x0600B146 RID: 45382 RVA: 0x00422F0F File Offset: 0x0042110F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.UpdateAttack();
	}

	// Token: 0x0600B147 RID: 45383 RVA: 0x00422F20 File Offset: 0x00421120
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

	// Token: 0x0600B148 RID: 45384 RVA: 0x00422FDC File Offset: 0x004211DC
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

	// Token: 0x0600B149 RID: 45385 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Attack()
	{
	}

	// Token: 0x0600B14A RID: 45386 RVA: 0x00423054 File Offset: 0x00421254
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

	// Token: 0x0600B14B RID: 45387 RVA: 0x00423148 File Offset: 0x00421348
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

	// Token: 0x0600B14C RID: 45388 RVA: 0x004231D0 File Offset: 0x004213D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyRecoil()
	{
		this.m_Weapon.StopSprings();
		this.m_Weapon.AddForce(this.ImpactPositionSpringRecoil, this.ImpactRotationSpringRecoil);
		this.m_Weapon.AddForce2(this.ImpactPositionSpring2Recoil, this.ImpactRotationSpring2Recoil);
		this.Reset();
	}

	// Token: 0x0600B14D RID: 45389 RVA: 0x0042321C File Offset: 0x0042141C
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

	// Token: 0x040084A8 RID: 33960
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPWeapon m_Weapon;

	// Token: 0x040084A9 RID: 33961
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x040084AA RID: 33962
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040084AB RID: 33963
	public string WeaponStatePull = "Pull";

	// Token: 0x040084AC RID: 33964
	public string WeaponStateSwing = "Swing";

	// Token: 0x040084AD RID: 33965
	public float SwingDelay = 0.5f;

	// Token: 0x040084AE RID: 33966
	public float SwingDuration = 0.5f;

	// Token: 0x040084AF RID: 33967
	public float SwingRate = 1f;

	// Token: 0x040084B0 RID: 33968
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedSwingTime;

	// Token: 0x040084B1 RID: 33969
	public int SwingSoftForceFrames = 50;

	// Token: 0x040084B2 RID: 33970
	public Vector3 SwingPositionSoftForce = new Vector3(-0.5f, -0.1f, 0.3f);

	// Token: 0x040084B3 RID: 33971
	public Vector3 SwingRotationSoftForce = new Vector3(50f, -25f, 0f);

	// Token: 0x040084B4 RID: 33972
	public float ImpactTime = 0.11f;

	// Token: 0x040084B5 RID: 33973
	public Vector3 ImpactPositionSpringRecoil = new Vector3(0.01f, 0.03f, -0.05f);

	// Token: 0x040084B6 RID: 33974
	public Vector3 ImpactPositionSpring2Recoil = Vector3.zero;

	// Token: 0x040084B7 RID: 33975
	public Vector3 ImpactRotationSpringRecoil = Vector3.zero;

	// Token: 0x040084B8 RID: 33976
	public Vector3 ImpactRotationSpring2Recoil = new Vector3(0f, 0f, 10f);

	// Token: 0x040084B9 RID: 33977
	public string DamageMethodName = "Damage";

	// Token: 0x040084BA RID: 33978
	public float Damage = 5f;

	// Token: 0x040084BB RID: 33979
	public float DamageRadius = 0.3f;

	// Token: 0x040084BC RID: 33980
	public float DamageRange = 2f;

	// Token: 0x040084BD RID: 33981
	public float DamageForce = 1000f;

	// Token: 0x040084BE RID: 33982
	public bool AttackPickRandomState = true;

	// Token: 0x040084BF RID: 33983
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_AttackCurrent;

	// Token: 0x040084C0 RID: 33984
	public float SparkFactor = 0.1f;

	// Token: 0x040084C1 RID: 33985
	public GameObject m_DustPrefab;

	// Token: 0x040084C2 RID: 33986
	public GameObject m_SparkPrefab;

	// Token: 0x040084C3 RID: 33987
	public GameObject m_DebrisPrefab;

	// Token: 0x040084C4 RID: 33988
	public List<global::UnityEngine.Object> SoundSwing = new List<global::UnityEngine.Object>();

	// Token: 0x040084C5 RID: 33989
	public List<global::UnityEngine.Object> SoundImpact = new List<global::UnityEngine.Object>();

	// Token: 0x040084C6 RID: 33990
	public Vector2 SoundSwingPitch = new Vector2(0.5f, 1.5f);

	// Token: 0x040084C7 RID: 33991
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x040084C8 RID: 33992
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDelayTimer = new vp_Timer.Handle();

	// Token: 0x040084C9 RID: 33993
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ImpactTimer = new vp_Timer.Handle();

	// Token: 0x040084CA RID: 33994
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle SwingDurationTimer = new vp_Timer.Handle();

	// Token: 0x040084CB RID: 33995
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle ResetTimer = new vp_Timer.Handle();

	// Token: 0x040084CC RID: 33996
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
