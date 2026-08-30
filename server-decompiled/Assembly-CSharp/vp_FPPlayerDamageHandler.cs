using System;
using UnityEngine;

// Token: 0x020015FE RID: 5630
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
public class vp_FPPlayerDamageHandler : vp_PlayerDamageHandler
{
	// Token: 0x17001484 RID: 5252
	// (get) Token: 0x0600B06B RID: 45163 RVA: 0x0041C742 File Offset: 0x0041A942
	public vp_FPPlayerEventHandler FPPlayer
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_FPPlayer == null)
			{
				this.m_FPPlayer = base.transform.GetComponent<vp_FPPlayerEventHandler>();
			}
			return this.m_FPPlayer;
		}
	}

	// Token: 0x17001485 RID: 5253
	// (get) Token: 0x0600B06C RID: 45164 RVA: 0x0041C769 File Offset: 0x0041A969
	public vp_FPCamera FPCamera
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_FPCamera == null)
			{
				this.m_FPCamera = base.transform.GetComponentInChildren<vp_FPCamera>();
			}
			return this.m_FPCamera;
		}
	}

	// Token: 0x17001486 RID: 5254
	// (get) Token: 0x0600B06D RID: 45165 RVA: 0x0041C790 File Offset: 0x0041A990
	public CharacterController CharacterController
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_CharacterController == null)
			{
				this.m_CharacterController = base.transform.root.GetComponentInChildren<CharacterController>();
			}
			return this.m_CharacterController;
		}
	}

	// Token: 0x0600B06E RID: 45166 RVA: 0x0041C7BC File Offset: 0x0041A9BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
		this.RefreshColliders();
	}

	// Token: 0x0600B06F RID: 45167 RVA: 0x0041C7DE File Offset: 0x0041A9DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B070 RID: 45168 RVA: 0x0041C7FA File Offset: 0x0041A9FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.FPPlayer.Dead.Active && Time.timeScale < 1f)
		{
			vp_TimeUtility.FadeTimeScale(1f, 0.05f);
		}
	}

	// Token: 0x0600B071 RID: 45169 RVA: 0x0041C82C File Offset: 0x0041AA2C
	public override void Damage(float damage)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		base.Damage(damage);
		this.FPPlayer.HUDDamageFlash.Send(new vp_DamageInfo(damage, null));
		this.FPPlayer.HeadImpact.Send((global::UnityEngine.Random.value < 0.5f) ? (damage * this.CameraShakeFactor) : (-(damage * this.CameraShakeFactor)));
	}

	// Token: 0x0600B072 RID: 45170 RVA: 0x0041C8A8 File Offset: 0x0041AAA8
	public override void Damage(vp_DamageInfo damageInfo)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		base.Damage(damageInfo);
		this.FPPlayer.HUDDamageFlash.Send(damageInfo);
		if (damageInfo.Source != null)
		{
			this.m_DamageAngle = vp_3DUtility.LookAtAngleHorizontal(this.FPCamera.Transform.position, this.FPCamera.Transform.forward, damageInfo.Source.position);
			this.m_DamageAngleFactor = ((Mathf.Abs(this.m_DamageAngle) > 30f) ? 1f : Mathf.Lerp(0f, 1f, Mathf.Abs(this.m_DamageAngle) * 0.033f));
			this.FPPlayer.HeadImpact.Send(damageInfo.Damage * this.CameraShakeFactor * this.m_DamageAngleFactor * (float)((this.m_DamageAngle < 0f) ? 1 : (-1)));
		}
	}

	// Token: 0x0600B073 RID: 45171 RVA: 0x0041C9AB File Offset: 0x0041ABAB
	public override void Die()
	{
		base.Die();
		if (!base.enabled || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		this.FPPlayer.InputAllowGameplay.Set(false);
	}

	// Token: 0x0600B074 RID: 45172 RVA: 0x0041C9E0 File Offset: 0x0041ABE0
	public virtual void RefreshColliders()
	{
		if (this.CharacterController != null && this.CharacterController.enabled)
		{
			foreach (Collider collider in base.Colliders)
			{
				if (collider.enabled)
				{
					Physics.IgnoreCollision(this.CharacterController, collider, true);
				}
			}
		}
	}

	// Token: 0x0600B075 RID: 45173 RVA: 0x0041CA5C File Offset: 0x0041AC5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Reset()
	{
		base.Reset();
		if (!Application.isPlaying)
		{
			return;
		}
		this.FPPlayer.InputAllowGameplay.Set(true);
		this.FPPlayer.HUDDamageFlash.Send(null);
		this.RefreshColliders();
	}

	// Token: 0x0600B076 RID: 45174 RVA: 0x0041CAA9 File Offset: 0x0041ACA9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x0600B077 RID: 45175 RVA: 0x0041CAA9 File Offset: 0x0041ACA9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x040083D2 RID: 33746
	public float CameraShakeFactor = 0.02f;

	// Token: 0x040083D3 RID: 33747
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DamageAngle;

	// Token: 0x040083D4 RID: 33748
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_DamageAngleFactor = 1f;

	// Token: 0x040083D5 RID: 33749
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;

	// Token: 0x040083D6 RID: 33750
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x040083D7 RID: 33751
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;
}
