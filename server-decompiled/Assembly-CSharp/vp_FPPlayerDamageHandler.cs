using System;
using UnityEngine;

// Token: 0x0200162F RID: 5679
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
public class vp_FPPlayerDamageHandler : vp_PlayerDamageHandler
{
	// Token: 0x170014D3 RID: 5331
	// (get) Token: 0x0600B155 RID: 45397 RVA: 0x00422822 File Offset: 0x00420A22
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

	// Token: 0x170014D4 RID: 5332
	// (get) Token: 0x0600B156 RID: 45398 RVA: 0x00422849 File Offset: 0x00420A49
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

	// Token: 0x170014D5 RID: 5333
	// (get) Token: 0x0600B157 RID: 45399 RVA: 0x00422870 File Offset: 0x00420A70
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

	// Token: 0x0600B158 RID: 45400 RVA: 0x0042289C File Offset: 0x00420A9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
		this.RefreshColliders();
	}

	// Token: 0x0600B159 RID: 45401 RVA: 0x004228BE File Offset: 0x00420ABE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B15A RID: 45402 RVA: 0x004228DA File Offset: 0x00420ADA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.FPPlayer.Dead.Active && Time.timeScale < 1f)
		{
			vp_TimeUtility.FadeTimeScale(1f, 0.05f);
		}
	}

	// Token: 0x0600B15B RID: 45403 RVA: 0x0042290C File Offset: 0x00420B0C
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

	// Token: 0x0600B15C RID: 45404 RVA: 0x00422988 File Offset: 0x00420B88
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

	// Token: 0x0600B15D RID: 45405 RVA: 0x00422A8B File Offset: 0x00420C8B
	public override void Die()
	{
		base.Die();
		if (!base.enabled || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		this.FPPlayer.InputAllowGameplay.Set(false);
	}

	// Token: 0x0600B15E RID: 45406 RVA: 0x00422AC0 File Offset: 0x00420CC0
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

	// Token: 0x0600B15F RID: 45407 RVA: 0x00422B3C File Offset: 0x00420D3C
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

	// Token: 0x0600B160 RID: 45408 RVA: 0x00422B89 File Offset: 0x00420D89
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x0600B161 RID: 45409 RVA: 0x00422B89 File Offset: 0x00420D89
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Crouch()
	{
		this.RefreshColliders();
	}

	// Token: 0x04008469 RID: 33897
	public float CameraShakeFactor = 0.02f;

	// Token: 0x0400846A RID: 33898
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DamageAngle;

	// Token: 0x0400846B RID: 33899
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_DamageAngleFactor = 1f;

	// Token: 0x0400846C RID: 33900
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;

	// Token: 0x0400846D RID: 33901
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_FPCamera;

	// Token: 0x0400846E RID: 33902
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CharacterController m_CharacterController;
}
