using System;
using UnityEngine;

// Token: 0x02001561 RID: 5473
public class vp_Spring
{
	// Token: 0x170013AF RID: 5039
	// (get) Token: 0x0600AB14 RID: 43796 RVA: 0x003FC30E File Offset: 0x003FA50E
	// (set) Token: 0x0600AB15 RID: 43797 RVA: 0x003FC316 File Offset: 0x003FA516
	public bool SdtdStopping { get; set; }

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x0600AB16 RID: 43798 RVA: 0x003FC31F File Offset: 0x003FA51F
	// (set) Token: 0x0600AB17 RID: 43799 RVA: 0x003FC327 File Offset: 0x003FA527
	public Transform Transform
	{
		get
		{
			return this.m_Transform;
		}
		set
		{
			this.m_Transform = value;
			this.RefreshUpdateMode();
		}
	}

	// Token: 0x0600AB18 RID: 43800 RVA: 0x003FC338 File Offset: 0x003FA538
	public vp_Spring(Transform transform, vp_Spring.UpdateMode mode, bool autoUpdate = true)
	{
		this.Mode = mode;
		this.Transform = transform;
		this.m_AutoUpdate = autoUpdate;
	}

	// Token: 0x0600AB19 RID: 43801 RVA: 0x003FC42C File Offset: 0x003FA62C
	public void FixedUpdate()
	{
		if (this.m_VelocityFadeInEndTime > Time.time)
		{
			this.m_VelocityFadeInCap = Mathf.Clamp01(1f - (this.m_VelocityFadeInEndTime - Time.time) / this.m_VelocityFadeInLength);
		}
		else
		{
			this.m_VelocityFadeInCap = 1f;
		}
		if (this.m_SoftForceFrame[0] != Vector3.zero)
		{
			this.AddForceInternal(this.m_SoftForceFrame[0]);
			for (int i = 0; i < 120; i++)
			{
				this.m_SoftForceFrame[i] = ((i < 119) ? this.m_SoftForceFrame[i + 1] : Vector3.zero);
				if (this.m_SoftForceFrame[i] == Vector3.zero)
				{
					break;
				}
			}
		}
		this.Calculate();
		this.m_UpdateFunc();
	}

	// Token: 0x0600AB1A RID: 43802 RVA: 0x003FC4FD File Offset: 0x003FA6FD
	[PublicizedFrom(EAccessModifier.Private)]
	public void Position()
	{
		this.m_Transform.localPosition = this.State;
	}

	// Token: 0x0600AB1B RID: 43803 RVA: 0x003FC510 File Offset: 0x003FA710
	[PublicizedFrom(EAccessModifier.Private)]
	public void Rotation()
	{
		this.m_Transform.localEulerAngles = this.State;
	}

	// Token: 0x0600AB1C RID: 43804 RVA: 0x003FC523 File Offset: 0x003FA723
	[PublicizedFrom(EAccessModifier.Private)]
	public void Scale()
	{
		this.m_Transform.localScale = this.State;
	}

	// Token: 0x0600AB1D RID: 43805 RVA: 0x003FC536 File Offset: 0x003FA736
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveLocal()
	{
		this.m_Transform.localPosition += this.State;
	}

	// Token: 0x0600AB1E RID: 43806 RVA: 0x003FC554 File Offset: 0x003FA754
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveGlobal()
	{
		this.m_Transform.position += this.State;
	}

	// Token: 0x0600AB1F RID: 43807 RVA: 0x003FC572 File Offset: 0x003FA772
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveSelf()
	{
		this.m_Transform.Translate(this.State, this.m_Transform);
	}

	// Token: 0x0600AB20 RID: 43808 RVA: 0x003FC58B File Offset: 0x003FA78B
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveLocal()
	{
		this.m_Transform.localEulerAngles += this.State;
	}

	// Token: 0x0600AB21 RID: 43809 RVA: 0x003FC5A9 File Offset: 0x003FA7A9
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveGlobal()
	{
		this.m_Transform.eulerAngles += this.State;
	}

	// Token: 0x0600AB22 RID: 43810 RVA: 0x003FC5C7 File Offset: 0x003FA7C7
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScaleAdditiveLocal()
	{
		this.m_Transform.localScale += this.State;
	}

	// Token: 0x0600AB23 RID: 43811 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void None()
	{
	}

	// Token: 0x0600AB24 RID: 43812 RVA: 0x003FC5E8 File Offset: 0x003FA7E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void RefreshUpdateMode()
	{
		this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.None);
		switch (this.Mode)
		{
		case vp_Spring.UpdateMode.Position:
			this.State = this.m_Transform.localPosition;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.Position);
			}
			break;
		case vp_Spring.UpdateMode.PositionAdditiveLocal:
			this.State = this.m_Transform.localPosition;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.PositionAdditiveLocal);
			}
			break;
		case vp_Spring.UpdateMode.PositionAdditiveGlobal:
			this.State = this.m_Transform.position;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.PositionAdditiveGlobal);
			}
			break;
		case vp_Spring.UpdateMode.PositionAdditiveSelf:
			this.State = this.m_Transform.position;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.PositionAdditiveSelf);
			}
			break;
		case vp_Spring.UpdateMode.Rotation:
			this.State = this.m_Transform.localEulerAngles;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.Rotation);
			}
			break;
		case vp_Spring.UpdateMode.RotationAdditiveLocal:
			this.State = this.m_Transform.localEulerAngles;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.RotationAdditiveLocal);
			}
			break;
		case vp_Spring.UpdateMode.RotationAdditiveGlobal:
			this.State = this.m_Transform.eulerAngles;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.RotationAdditiveGlobal);
			}
			break;
		case vp_Spring.UpdateMode.Scale:
			this.State = this.m_Transform.localScale;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.Scale);
			}
			break;
		case vp_Spring.UpdateMode.ScaleAdditiveLocal:
			this.State = this.m_Transform.localScale;
			if (this.m_AutoUpdate)
			{
				this.m_UpdateFunc = new vp_Spring.UpdateDelegate(this.ScaleAdditiveLocal);
			}
			break;
		}
		this.RestState = this.State;
	}

	// Token: 0x0600AB25 RID: 43813 RVA: 0x003FC800 File Offset: 0x003FAA00
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Calculate()
	{
		if ((!this.SdtdStopping) ? (this.State == this.RestState) : (this.m_Velocity.sqrMagnitude <= this.MinVelocity * this.MinVelocity && (this.RestState - this.State).sqrMagnitude <= this.SdtdMinDeltaState * this.SdtdMinDeltaState))
		{
			return;
		}
		this.m_Velocity += Vector3.Scale(this.RestState - this.State, this.Stiffness);
		this.m_Velocity = Vector3.Scale(this.m_Velocity, this.Damping);
		this.m_Velocity = Vector3.ClampMagnitude(this.m_Velocity, this.MaxVelocity);
		if (this.m_Velocity.sqrMagnitude > this.MinVelocity * this.MinVelocity || (this.SdtdStopping && (this.RestState - this.State).sqrMagnitude > this.SdtdMinDeltaState * this.SdtdMinDeltaState))
		{
			this.Move();
			return;
		}
		this.Reset();
	}

	// Token: 0x0600AB26 RID: 43814 RVA: 0x003FC925 File Offset: 0x003FAB25
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddForceInternal(Vector3 force)
	{
		force *= this.m_VelocityFadeInCap;
		this.m_Velocity += force;
		this.m_Velocity = Vector3.ClampMagnitude(this.m_Velocity, this.MaxVelocity);
		this.Move();
	}

	// Token: 0x0600AB27 RID: 43815 RVA: 0x003FC964 File Offset: 0x003FAB64
	public void AddForce(Vector3 force)
	{
		if (Time.timeScale < 1f)
		{
			this.AddSoftForce(force, 1f);
			return;
		}
		this.AddForceInternal(force);
	}

	// Token: 0x0600AB28 RID: 43816 RVA: 0x003FC988 File Offset: 0x003FAB88
	public void AddSoftForce(Vector3 force, float frames)
	{
		force /= Time.timeScale;
		frames = Mathf.Clamp(frames, 1f, 120f);
		this.AddForceInternal(force / frames);
		for (int i = 0; i < Mathf.RoundToInt(frames) - 1; i++)
		{
			this.m_SoftForceFrame[i] += force / frames;
		}
	}

	// Token: 0x0600AB29 RID: 43817 RVA: 0x003FC9F8 File Offset: 0x003FABF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Move()
	{
		this.State += this.m_Velocity * Time.timeScale;
		this.State.x = Mathf.Clamp(this.State.x, this.MinState.x, this.MaxState.x);
		this.State.y = Mathf.Clamp(this.State.y, this.MinState.y, this.MaxState.y);
		this.State.z = Mathf.Clamp(this.State.z, this.MinState.z, this.MaxState.z);
	}

	// Token: 0x0600AB2A RID: 43818 RVA: 0x003FCAB9 File Offset: 0x003FACB9
	public void Reset()
	{
		this.m_Velocity = Vector3.zero;
		this.State = this.RestState;
	}

	// Token: 0x0600AB2B RID: 43819 RVA: 0x003FCAD2 File Offset: 0x003FACD2
	public void Stop(bool includeSoftForce = false)
	{
		this.m_Velocity = Vector3.zero;
		if (includeSoftForce)
		{
			this.StopSoftForce();
		}
	}

	// Token: 0x0600AB2C RID: 43820 RVA: 0x003FCAE8 File Offset: 0x003FACE8
	public void StopSoftForce()
	{
		for (int i = 0; i < 120; i++)
		{
			this.m_SoftForceFrame[i] = Vector3.zero;
		}
	}

	// Token: 0x0600AB2D RID: 43821 RVA: 0x003FCB13 File Offset: 0x003FAD13
	public void ForceVelocityFadeIn(float seconds)
	{
		this.m_VelocityFadeInLength = seconds;
		this.m_VelocityFadeInEndTime = Time.time + seconds;
		this.m_VelocityFadeInCap = 0f;
	}

	// Token: 0x04007E79 RID: 32377
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateMode Mode;

	// Token: 0x04007E7A RID: 32378
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_AutoUpdate = true;

	// Token: 0x04007E7B RID: 32379
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateDelegate m_UpdateFunc;

	// Token: 0x04007E7C RID: 32380
	public Vector3 State = Vector3.zero;

	// Token: 0x04007E7D RID: 32381
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 m_Velocity = Vector3.zero;

	// Token: 0x04007E7E RID: 32382
	public Vector3 RestState = Vector3.zero;

	// Token: 0x04007E7F RID: 32383
	public Vector3 Stiffness = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x04007E80 RID: 32384
	public Vector3 Damping = new Vector3(0.75f, 0.75f, 0.75f);

	// Token: 0x04007E81 RID: 32385
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInCap = 1f;

	// Token: 0x04007E82 RID: 32386
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInEndTime;

	// Token: 0x04007E83 RID: 32387
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInLength;

	// Token: 0x04007E84 RID: 32388
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] m_SoftForceFrame = new Vector3[120];

	// Token: 0x04007E85 RID: 32389
	public float MaxVelocity = 10000f;

	// Token: 0x04007E86 RID: 32390
	public float MinVelocity = 1E-07f;

	// Token: 0x04007E87 RID: 32391
	public Vector3 MaxState = new Vector3(10000f, 10000f, 10000f);

	// Token: 0x04007E88 RID: 32392
	public Vector3 MinState = new Vector3(-10000f, -10000f, -10000f);

	// Token: 0x04007E89 RID: 32393
	public float SdtdMinDeltaState = 0.0001f;

	// Token: 0x04007E8B RID: 32395
	[PublicizedFrom(EAccessModifier.Protected)]
	public Transform m_Transform;

	// Token: 0x02001562 RID: 5474
	public enum UpdateMode
	{
		// Token: 0x04007E8D RID: 32397
		Position,
		// Token: 0x04007E8E RID: 32398
		PositionAdditiveLocal,
		// Token: 0x04007E8F RID: 32399
		PositionAdditiveGlobal,
		// Token: 0x04007E90 RID: 32400
		PositionAdditiveSelf,
		// Token: 0x04007E91 RID: 32401
		Rotation,
		// Token: 0x04007E92 RID: 32402
		RotationAdditiveLocal,
		// Token: 0x04007E93 RID: 32403
		RotationAdditiveGlobal,
		// Token: 0x04007E94 RID: 32404
		Scale,
		// Token: 0x04007E95 RID: 32405
		ScaleAdditiveLocal
	}

	// Token: 0x02001563 RID: 5475
	// (Invoke) Token: 0x0600AB2F RID: 43823
	[PublicizedFrom(EAccessModifier.Protected)]
	public delegate void UpdateDelegate();
}
