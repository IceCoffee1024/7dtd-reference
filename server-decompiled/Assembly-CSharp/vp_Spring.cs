using System;
using UnityEngine;

// Token: 0x0200157C RID: 5500
public class vp_Spring
{
	// Token: 0x170013E8 RID: 5096
	// (get) Token: 0x0600AC11 RID: 44049 RVA: 0x0040164E File Offset: 0x003FF84E
	// (set) Token: 0x0600AC12 RID: 44050 RVA: 0x00401656 File Offset: 0x003FF856
	public bool SdtdStopping { get; set; }

	// Token: 0x170013E9 RID: 5097
	// (get) Token: 0x0600AC13 RID: 44051 RVA: 0x0040165F File Offset: 0x003FF85F
	// (set) Token: 0x0600AC14 RID: 44052 RVA: 0x00401667 File Offset: 0x003FF867
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

	// Token: 0x0600AC15 RID: 44053 RVA: 0x00401678 File Offset: 0x003FF878
	public vp_Spring(Transform transform, vp_Spring.UpdateMode mode, bool autoUpdate = true)
	{
		this.Mode = mode;
		this.Transform = transform;
		this.m_AutoUpdate = autoUpdate;
	}

	// Token: 0x0600AC16 RID: 44054 RVA: 0x0040176C File Offset: 0x003FF96C
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

	// Token: 0x0600AC17 RID: 44055 RVA: 0x0040183D File Offset: 0x003FFA3D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Position()
	{
		this.m_Transform.localPosition = this.State;
	}

	// Token: 0x0600AC18 RID: 44056 RVA: 0x00401850 File Offset: 0x003FFA50
	[PublicizedFrom(EAccessModifier.Private)]
	public void Rotation()
	{
		this.m_Transform.localEulerAngles = this.State;
	}

	// Token: 0x0600AC19 RID: 44057 RVA: 0x00401863 File Offset: 0x003FFA63
	[PublicizedFrom(EAccessModifier.Private)]
	public void Scale()
	{
		this.m_Transform.localScale = this.State;
	}

	// Token: 0x0600AC1A RID: 44058 RVA: 0x00401876 File Offset: 0x003FFA76
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveLocal()
	{
		this.m_Transform.localPosition += this.State;
	}

	// Token: 0x0600AC1B RID: 44059 RVA: 0x00401894 File Offset: 0x003FFA94
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveGlobal()
	{
		this.m_Transform.position += this.State;
	}

	// Token: 0x0600AC1C RID: 44060 RVA: 0x004018B2 File Offset: 0x003FFAB2
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveSelf()
	{
		this.m_Transform.Translate(this.State, this.m_Transform);
	}

	// Token: 0x0600AC1D RID: 44061 RVA: 0x004018CB File Offset: 0x003FFACB
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveLocal()
	{
		this.m_Transform.localEulerAngles += this.State;
	}

	// Token: 0x0600AC1E RID: 44062 RVA: 0x004018E9 File Offset: 0x003FFAE9
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveGlobal()
	{
		this.m_Transform.eulerAngles += this.State;
	}

	// Token: 0x0600AC1F RID: 44063 RVA: 0x00401907 File Offset: 0x003FFB07
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScaleAdditiveLocal()
	{
		this.m_Transform.localScale += this.State;
	}

	// Token: 0x0600AC20 RID: 44064 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void None()
	{
	}

	// Token: 0x0600AC21 RID: 44065 RVA: 0x00401928 File Offset: 0x003FFB28
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

	// Token: 0x0600AC22 RID: 44066 RVA: 0x00401B40 File Offset: 0x003FFD40
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

	// Token: 0x0600AC23 RID: 44067 RVA: 0x00401C65 File Offset: 0x003FFE65
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddForceInternal(Vector3 force)
	{
		force *= this.m_VelocityFadeInCap;
		this.m_Velocity += force;
		this.m_Velocity = Vector3.ClampMagnitude(this.m_Velocity, this.MaxVelocity);
		this.Move();
	}

	// Token: 0x0600AC24 RID: 44068 RVA: 0x00401CA4 File Offset: 0x003FFEA4
	public void AddForce(Vector3 force)
	{
		if (Time.timeScale < 1f)
		{
			this.AddSoftForce(force, 1f);
			return;
		}
		this.AddForceInternal(force);
	}

	// Token: 0x0600AC25 RID: 44069 RVA: 0x00401CC8 File Offset: 0x003FFEC8
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

	// Token: 0x0600AC26 RID: 44070 RVA: 0x00401D38 File Offset: 0x003FFF38
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Move()
	{
		this.State += this.m_Velocity * Time.timeScale;
		this.State.x = Mathf.Clamp(this.State.x, this.MinState.x, this.MaxState.x);
		this.State.y = Mathf.Clamp(this.State.y, this.MinState.y, this.MaxState.y);
		this.State.z = Mathf.Clamp(this.State.z, this.MinState.z, this.MaxState.z);
	}

	// Token: 0x0600AC27 RID: 44071 RVA: 0x00401DF9 File Offset: 0x003FFFF9
	public void Reset()
	{
		this.m_Velocity = Vector3.zero;
		this.State = this.RestState;
	}

	// Token: 0x0600AC28 RID: 44072 RVA: 0x00401E12 File Offset: 0x00400012
	public void Stop(bool includeSoftForce = false)
	{
		this.m_Velocity = Vector3.zero;
		if (includeSoftForce)
		{
			this.StopSoftForce();
		}
	}

	// Token: 0x0600AC29 RID: 44073 RVA: 0x00401E28 File Offset: 0x00400028
	public void StopSoftForce()
	{
		for (int i = 0; i < 120; i++)
		{
			this.m_SoftForceFrame[i] = Vector3.zero;
		}
	}

	// Token: 0x0600AC2A RID: 44074 RVA: 0x00401E53 File Offset: 0x00400053
	public void ForceVelocityFadeIn(float seconds)
	{
		this.m_VelocityFadeInLength = seconds;
		this.m_VelocityFadeInEndTime = Time.time + seconds;
		this.m_VelocityFadeInCap = 0f;
	}

	// Token: 0x04007F52 RID: 32594
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateMode Mode;

	// Token: 0x04007F53 RID: 32595
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_AutoUpdate = true;

	// Token: 0x04007F54 RID: 32596
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateDelegate m_UpdateFunc;

	// Token: 0x04007F55 RID: 32597
	public Vector3 State = Vector3.zero;

	// Token: 0x04007F56 RID: 32598
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 m_Velocity = Vector3.zero;

	// Token: 0x04007F57 RID: 32599
	public Vector3 RestState = Vector3.zero;

	// Token: 0x04007F58 RID: 32600
	public Vector3 Stiffness = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x04007F59 RID: 32601
	public Vector3 Damping = new Vector3(0.75f, 0.75f, 0.75f);

	// Token: 0x04007F5A RID: 32602
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInCap = 1f;

	// Token: 0x04007F5B RID: 32603
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInEndTime;

	// Token: 0x04007F5C RID: 32604
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInLength;

	// Token: 0x04007F5D RID: 32605
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] m_SoftForceFrame = new Vector3[120];

	// Token: 0x04007F5E RID: 32606
	public float MaxVelocity = 10000f;

	// Token: 0x04007F5F RID: 32607
	public float MinVelocity = 1E-07f;

	// Token: 0x04007F60 RID: 32608
	public Vector3 MaxState = new Vector3(10000f, 10000f, 10000f);

	// Token: 0x04007F61 RID: 32609
	public Vector3 MinState = new Vector3(-10000f, -10000f, -10000f);

	// Token: 0x04007F62 RID: 32610
	public float SdtdMinDeltaState = 0.0001f;

	// Token: 0x04007F64 RID: 32612
	[PublicizedFrom(EAccessModifier.Protected)]
	public Transform m_Transform;

	// Token: 0x0200157D RID: 5501
	public enum UpdateMode
	{
		// Token: 0x04007F66 RID: 32614
		Position,
		// Token: 0x04007F67 RID: 32615
		PositionAdditiveLocal,
		// Token: 0x04007F68 RID: 32616
		PositionAdditiveGlobal,
		// Token: 0x04007F69 RID: 32617
		PositionAdditiveSelf,
		// Token: 0x04007F6A RID: 32618
		Rotation,
		// Token: 0x04007F6B RID: 32619
		RotationAdditiveLocal,
		// Token: 0x04007F6C RID: 32620
		RotationAdditiveGlobal,
		// Token: 0x04007F6D RID: 32621
		Scale,
		// Token: 0x04007F6E RID: 32622
		ScaleAdditiveLocal
	}

	// Token: 0x0200157E RID: 5502
	// (Invoke) Token: 0x0600AC2C RID: 44076
	[PublicizedFrom(EAccessModifier.Protected)]
	public delegate void UpdateDelegate();
}
