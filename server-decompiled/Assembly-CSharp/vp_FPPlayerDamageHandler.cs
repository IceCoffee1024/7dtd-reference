using System;
using UnityEngine;

// Token: 0x0200161D RID: 5661
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
public class vp_FPPlayerDamageHandler : vp_PlayerDamageHandler
{
	// Token: 0x170014C7 RID: 5319
	// (get) Token: 0x0600B0FD RID: 45309 RVA: 0x0042041E File Offset: 0x0041E61E
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

	// Token: 0x170014C8 RID: 5320
	// (get) Token: 0x0600B0FE RID: 45310 RVA: 0x00420445 File Offset: 0x0041E645
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

	// Token: 0x170014C9 RID: 5321
	// (get) Token: 0x0600B0FF RID: 45311 RVA: 0x0042046C File Offset: 0x0041E66C
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

	// Token: 0x0600B100 RID: 45312 RVA: 0x00420498 File Offset: 0x0041E698
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
		this.RefreshColliders();
	}

	// Token: 0x0600B101 RID: 45313 RVA: 0x004204BA File Offset: 0x0041E6BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B102 RID: 45314 RVA: 0x004204D6 File Offset: 0x0041E6D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.FPPlayer.Dead.Active && Time.timeScale < 1f)
		{
			vp_TimeUtility.FadeTimeScale(1f, 0.05f);
		}
	}

	// Token: 0x0600B103 RID: 45315 RVA: 0x00420508 File Offset: 0x0041E708
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

	// Token: 0x0600B104 RID: 45316 RVA: 0x00420584 File Offset: 0x0041E784
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

	// Token: 0x0600B105 RID: 45317 RVA: 0x00420687 File Offset: 0x0041E887
	public override void Die()
	{
		base.Die();
		if (!base.enabled || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		this.FPPlayer.InputAllowGameplay.Set(false);
	}

	// Token: 0x0600B106 RID: 45318 RVA: 0x004206BC File Offset: 0x0041E8BC
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

	// Token: 0x0600B107 RID: 45319 RVA: 0x00420738 File Offset: 0x0041E938
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

	// Token: 0x0600B108 RID: 45320 RVA: 0x00420785 File Offset: 0x0041E985
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x0600B109 RID: 45321 RVA: 0x00420785 File Offset: 0x0041E985
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x04008428 RID: 33832
	public float CameraShakeFactor = 0.02f;

	// Token: 0x04008429 RID: 33833
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DamageAngle;

	// Token: 0x0400842A RID: 33834
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_DamageAngleFactor = 1f;

	// Token: 0x0400842B RID: 33835
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;

	// Token: 0x0400842C RID: 33836
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x0400842D RID: 33837
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;
}
