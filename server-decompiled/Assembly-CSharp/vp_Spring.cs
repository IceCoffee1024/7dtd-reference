using System;
using UnityEngine;

// Token: 0x020015B0 RID: 5552
public class vp_Spring
{
	// Token: 0x17001439 RID: 5177
	// (get) Token: 0x0600AD1C RID: 44316 RVA: 0x004088A6 File Offset: 0x00406AA6
	// (set) Token: 0x0600AD1D RID: 44317 RVA: 0x004088AE File Offset: 0x00406AAE
	public bool SdtdStopping { get; set; }

	// Token: 0x1700143A RID: 5178
	// (get) Token: 0x0600AD1E RID: 44318 RVA: 0x004088B7 File Offset: 0x00406AB7
	// (set) Token: 0x0600AD1F RID: 44319 RVA: 0x004088BF File Offset: 0x00406ABF
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

	// Token: 0x0600AD20 RID: 44320 RVA: 0x004088D0 File Offset: 0x00406AD0
	public vp_Spring(Transform transform, vp_Spring.UpdateMode mode, bool autoUpdate = true)
	{
		this.Mode = mode;
		this.Transform = transform;
		this.m_AutoUpdate = autoUpdate;
	}

	// Token: 0x0600AD21 RID: 44321 RVA: 0x004089C4 File Offset: 0x00406BC4
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

	// Token: 0x0600AD22 RID: 44322 RVA: 0x00408A95 File Offset: 0x00406C95
	[PublicizedFrom(EAccessModifier.Private)]
	public void Position()
	{
		this.m_Transform.localPosition = this.State;
	}

	// Token: 0x0600AD23 RID: 44323 RVA: 0x00408AA8 File Offset: 0x00406CA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Rotation()
	{
		this.m_Transform.localEulerAngles = this.State;
	}

	// Token: 0x0600AD24 RID: 44324 RVA: 0x00408ABB File Offset: 0x00406CBB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Scale()
	{
		this.m_Transform.localScale = this.State;
	}

	// Token: 0x0600AD25 RID: 44325 RVA: 0x00408ACE File Offset: 0x00406CCE
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveLocal()
	{
		this.m_Transform.localPosition += this.State;
	}

	// Token: 0x0600AD26 RID: 44326 RVA: 0x00408AEC File Offset: 0x00406CEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveGlobal()
	{
		this.m_Transform.position += this.State;
	}

	// Token: 0x0600AD27 RID: 44327 RVA: 0x00408B0A File Offset: 0x00406D0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveSelf()
	{
		this.m_Transform.Translate(this.State, this.m_Transform);
	}

	// Token: 0x0600AD28 RID: 44328 RVA: 0x00408B23 File Offset: 0x00406D23
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveLocal()
	{
		this.m_Transform.localEulerAngles += this.State;
	}

	// Token: 0x0600AD29 RID: 44329 RVA: 0x00408B41 File Offset: 0x00406D41
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveGlobal()
	{
		this.m_Transform.eulerAngles += this.State;
	}

	// Token: 0x0600AD2A RID: 44330 RVA: 0x00408B5F File Offset: 0x00406D5F
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScaleAdditiveLocal()
	{
		this.m_Transform.localScale += this.State;
	}

	// Token: 0x0600AD2B RID: 44331 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void None()
	{
	}

	// Token: 0x0600AD2C RID: 44332 RVA: 0x00408B80 File Offset: 0x00406D80
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

	// Token: 0x0600AD2D RID: 44333 RVA: 0x00408D98 File Offset: 0x00406F98
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

	// Token: 0x0600AD2E RID: 44334 RVA: 0x00408EBD File Offset: 0x004070BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddForceInternal(Vector3 force)
	{
		force *= this.m_VelocityFadeInCap;
		this.m_Velocity += force;
		this.m_Velocity = Vector3.ClampMagnitude(this.m_Velocity, this.MaxVelocity);
		this.Move();
	}

	// Token: 0x0600AD2F RID: 44335 RVA: 0x00408EFC File Offset: 0x004070FC
	public void AddForce(Vector3 force)
	{
		if (Time.timeScale < 1f)
		{
			this.AddSoftForce(force, 1f);
			return;
		}
		this.AddForceInternal(force);
	}

	// Token: 0x0600AD30 RID: 44336 RVA: 0x00408F20 File Offset: 0x00407120
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

	// Token: 0x0600AD31 RID: 44337 RVA: 0x00408F90 File Offset: 0x00407190
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Move()
	{
		this.State += this.m_Velocity * Time.timeScale;
		this.State.x = Mathf.Clamp(this.State.x, this.MinState.x, this.MaxState.x);
		this.State.y = Mathf.Clamp(this.State.y, this.MinState.y, this.MaxState.y);
		this.State.z = Mathf.Clamp(this.State.z, this.MinState.z, this.MaxState.z);
	}

	// Token: 0x0600AD32 RID: 44338 RVA: 0x00409051 File Offset: 0x00407251
	public void Reset()
	{
		this.m_Velocity = Vector3.zero;
		this.State = this.RestState;
	}

	// Token: 0x0600AD33 RID: 44339 RVA: 0x0040906A File Offset: 0x0040726A
	public void Stop(bool includeSoftForce = false)
	{
		this.m_Velocity = Vector3.zero;
		if (includeSoftForce)
		{
			this.StopSoftForce();
		}
	}

	// Token: 0x0600AD34 RID: 44340 RVA: 0x00409080 File Offset: 0x00407280
	public void StopSoftForce()
	{
		for (int i = 0; i < 120; i++)
		{
			this.m_SoftForceFrame[i] = Vector3.zero;
		}
	}

	// Token: 0x0600AD35 RID: 44341 RVA: 0x004090AB File Offset: 0x004072AB
	public void ForceVelocityFadeIn(float seconds)
	{
		this.m_VelocityFadeInLength = seconds;
		this.m_VelocityFadeInEndTime = Time.time + seconds;
		this.m_VelocityFadeInCap = 0f;
	}

	// Token: 0x04008016 RID: 32790
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateMode Mode;

	// Token: 0x04008017 RID: 32791
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_AutoUpdate = true;

	// Token: 0x04008018 RID: 32792
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateDelegate m_UpdateFunc;

	// Token: 0x04008019 RID: 32793
	public Vector3 State = Vector3.zero;

	// Token: 0x0400801A RID: 32794
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 m_Velocity = Vector3.zero;

	// Token: 0x0400801B RID: 32795
	public Vector3 RestState = Vector3.zero;

	// Token: 0x0400801C RID: 32796
	public Vector3 Stiffness = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x0400801D RID: 32797
	public Vector3 Damping = new Vector3(0.75f, 0.75f, 0.75f);

	// Token: 0x0400801E RID: 32798
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInCap = 1f;

	// Token: 0x0400801F RID: 32799
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInEndTime;

	// Token: 0x04008020 RID: 32800
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInLength;

	// Token: 0x04008021 RID: 32801
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] m_SoftForceFrame = new Vector3[120];

	// Token: 0x04008022 RID: 32802
	public float MaxVelocity = 10000f;

	// Token: 0x04008023 RID: 32803
	public float MinVelocity = 1E-07f;

	// Token: 0x04008024 RID: 32804
	public Vector3 MaxState = new Vector3(10000f, 10000f, 10000f);

	// Token: 0x04008025 RID: 32805
	public Vector3 MinState = new Vector3(-10000f, -10000f, -10000f);

	// Token: 0x04008026 RID: 32806
	public float SdtdMinDeltaState = 0.0001f;

	// Token: 0x04008028 RID: 32808
	[PublicizedFrom(EAccessModifier.Protected)]
	public Transform m_Transform;

	// Token: 0x020015B1 RID: 5553
	public enum UpdateMode
	{
		// Token: 0x0400802A RID: 32810
		Position,
		// Token: 0x0400802B RID: 32811
		PositionAdditiveLocal,
		// Token: 0x0400802C RID: 32812
		PositionAdditiveGlobal,
		// Token: 0x0400802D RID: 32813
		PositionAdditiveSelf,
		// Token: 0x0400802E RID: 32814
		Rotation,
		// Token: 0x0400802F RID: 32815
		RotationAdditiveLocal,
		// Token: 0x04008030 RID: 32816
		RotationAdditiveGlobal,
		// Token: 0x04008031 RID: 32817
		Scale,
		// Token: 0x04008032 RID: 32818
		ScaleAdditiveLocal
	}

	// Token: 0x020015B2 RID: 5554
	// (Invoke) Token: 0x0600AD37 RID: 44343
	[PublicizedFrom(EAccessModifier.Protected)]
	public delegate void UpdateDelegate();
}
