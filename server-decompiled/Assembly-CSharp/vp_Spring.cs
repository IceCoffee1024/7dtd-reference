using System;
using UnityEngine;

// Token: 0x0200159E RID: 5534
public class vp_Spring
{
	// Token: 0x1700142D RID: 5165
	// (get) Token: 0x0600ACC4 RID: 44228 RVA: 0x004064A2 File Offset: 0x004046A2
	// (set) Token: 0x0600ACC5 RID: 44229 RVA: 0x004064AA File Offset: 0x004046AA
	public bool SdtdStopping { get; set; }

	// Token: 0x1700142E RID: 5166
	// (get) Token: 0x0600ACC6 RID: 44230 RVA: 0x004064B3 File Offset: 0x004046B3
	// (set) Token: 0x0600ACC7 RID: 44231 RVA: 0x004064BB File Offset: 0x004046BB
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

	// Token: 0x0600ACC8 RID: 44232 RVA: 0x004064CC File Offset: 0x004046CC
	public vp_Spring(Transform transform, vp_Spring.UpdateMode mode, bool autoUpdate = true)
	{
		this.Mode = mode;
		this.Transform = transform;
		this.m_AutoUpdate = autoUpdate;
	}

	// Token: 0x0600ACC9 RID: 44233 RVA: 0x004065C0 File Offset: 0x004047C0
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

	// Token: 0x0600ACCA RID: 44234 RVA: 0x00406691 File Offset: 0x00404891
	[PublicizedFrom(EAccessModifier.Private)]
	public void Position()
	{
		this.m_Transform.localPosition = this.State;
	}

	// Token: 0x0600ACCB RID: 44235 RVA: 0x004066A4 File Offset: 0x004048A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Rotation()
	{
		this.m_Transform.localEulerAngles = this.State;
	}

	// Token: 0x0600ACCC RID: 44236 RVA: 0x004066B7 File Offset: 0x004048B7
	[PublicizedFrom(EAccessModifier.Private)]
	public void Scale()
	{
		this.m_Transform.localScale = this.State;
	}

	// Token: 0x0600ACCD RID: 44237 RVA: 0x004066CA File Offset: 0x004048CA
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveLocal()
	{
		this.m_Transform.localPosition += this.State;
	}

	// Token: 0x0600ACCE RID: 44238 RVA: 0x004066E8 File Offset: 0x004048E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveGlobal()
	{
		this.m_Transform.position += this.State;
	}

	// Token: 0x0600ACCF RID: 44239 RVA: 0x00406706 File Offset: 0x00404906
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionAdditiveSelf()
	{
		this.m_Transform.Translate(this.State, this.m_Transform);
	}

	// Token: 0x0600ACD0 RID: 44240 RVA: 0x0040671F File Offset: 0x0040491F
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveLocal()
	{
		this.m_Transform.localEulerAngles += this.State;
	}

	// Token: 0x0600ACD1 RID: 44241 RVA: 0x0040673D File Offset: 0x0040493D
	[PublicizedFrom(EAccessModifier.Private)]
	public void RotationAdditiveGlobal()
	{
		this.m_Transform.eulerAngles += this.State;
	}

	// Token: 0x0600ACD2 RID: 44242 RVA: 0x0040675B File Offset: 0x0040495B
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScaleAdditiveLocal()
	{
		this.m_Transform.localScale += this.State;
	}

	// Token: 0x0600ACD3 RID: 44243 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void None()
	{
	}

	// Token: 0x0600ACD4 RID: 44244 RVA: 0x0040677C File Offset: 0x0040497C
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

	// Token: 0x0600ACD5 RID: 44245 RVA: 0x00406994 File Offset: 0x00404B94
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

	// Token: 0x0600ACD6 RID: 44246 RVA: 0x00406AB9 File Offset: 0x00404CB9
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddForceInternal(Vector3 force)
	{
		force *= this.m_VelocityFadeInCap;
		this.m_Velocity += force;
		this.m_Velocity = Vector3.ClampMagnitude(this.m_Velocity, this.MaxVelocity);
		this.Move();
	}

	// Token: 0x0600ACD7 RID: 44247 RVA: 0x00406AF8 File Offset: 0x00404CF8
	public void AddForce(Vector3 force)
	{
		if (Time.timeScale < 1f)
		{
			this.AddSoftForce(force, 1f);
			return;
		}
		this.AddForceInternal(force);
	}

	// Token: 0x0600ACD8 RID: 44248 RVA: 0x00406B1C File Offset: 0x00404D1C
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

	// Token: 0x0600ACD9 RID: 44249 RVA: 0x00406B8C File Offset: 0x00404D8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Move()
	{
		this.State += this.m_Velocity * Time.timeScale;
		this.State.x = Mathf.Clamp(this.State.x, this.MinState.x, this.MaxState.x);
		this.State.y = Mathf.Clamp(this.State.y, this.MinState.y, this.MaxState.y);
		this.State.z = Mathf.Clamp(this.State.z, this.MinState.z, this.MaxState.z);
	}

	// Token: 0x0600ACDA RID: 44250 RVA: 0x00406C4D File Offset: 0x00404E4D
	public void Reset()
	{
		this.m_Velocity = Vector3.zero;
		this.State = this.RestState;
	}

	// Token: 0x0600ACDB RID: 44251 RVA: 0x00406C66 File Offset: 0x00404E66
	public void Stop(bool includeSoftForce = false)
	{
		this.m_Velocity = Vector3.zero;
		if (includeSoftForce)
		{
			this.StopSoftForce();
		}
	}

	// Token: 0x0600ACDC RID: 44252 RVA: 0x00406C7C File Offset: 0x00404E7C
	public void StopSoftForce()
	{
		for (int i = 0; i < 120; i++)
		{
			this.m_SoftForceFrame[i] = Vector3.zero;
		}
	}

	// Token: 0x0600ACDD RID: 44253 RVA: 0x00406CA7 File Offset: 0x00404EA7
	public void ForceVelocityFadeIn(float seconds)
	{
		this.m_VelocityFadeInLength = seconds;
		this.m_VelocityFadeInEndTime = Time.time + seconds;
		this.m_VelocityFadeInCap = 0f;
	}

	// Token: 0x04007FD5 RID: 32725
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateMode Mode;

	// Token: 0x04007FD6 RID: 32726
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_AutoUpdate = true;

	// Token: 0x04007FD7 RID: 32727
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Spring.UpdateDelegate m_UpdateFunc;

	// Token: 0x04007FD8 RID: 32728
	public Vector3 State = Vector3.zero;

	// Token: 0x04007FD9 RID: 32729
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 m_Velocity = Vector3.zero;

	// Token: 0x04007FDA RID: 32730
	public Vector3 RestState = Vector3.zero;

	// Token: 0x04007FDB RID: 32731
	public Vector3 Stiffness = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x04007FDC RID: 32732
	public Vector3 Damping = new Vector3(0.75f, 0.75f, 0.75f);

	// Token: 0x04007FDD RID: 32733
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInCap = 1f;

	// Token: 0x04007FDE RID: 32734
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInEndTime;

	// Token: 0x04007FDF RID: 32735
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_VelocityFadeInLength;

	// Token: 0x04007FE0 RID: 32736
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] m_SoftForceFrame = new Vector3[120];

	// Token: 0x04007FE1 RID: 32737
	public float MaxVelocity = 10000f;

	// Token: 0x04007FE2 RID: 32738
	public float MinVelocity = 1E-07f;

	// Token: 0x04007FE3 RID: 32739
	public Vector3 MaxState = new Vector3(10000f, 10000f, 10000f);

	// Token: 0x04007FE4 RID: 32740
	public Vector3 MinState = new Vector3(-10000f, -10000f, -10000f);

	// Token: 0x04007FE5 RID: 32741
	public float SdtdMinDeltaState = 0.0001f;

	// Token: 0x04007FE7 RID: 32743
	[PublicizedFrom(EAccessModifier.Protected)]
	public Transform m_Transform;

	// Token: 0x0200159F RID: 5535
	public enum UpdateMode
	{
		// Token: 0x04007FE9 RID: 32745
		Position,
		// Token: 0x04007FEA RID: 32746
		PositionAdditiveLocal,
		// Token: 0x04007FEB RID: 32747
		PositionAdditiveGlobal,
		// Token: 0x04007FEC RID: 32748
		PositionAdditiveSelf,
		// Token: 0x04007FED RID: 32749
		Rotation,
		// Token: 0x04007FEE RID: 32750
		RotationAdditiveLocal,
		// Token: 0x04007FEF RID: 32751
		RotationAdditiveGlobal,
		// Token: 0x04007FF0 RID: 32752
		Scale,
		// Token: 0x04007FF1 RID: 32753
		ScaleAdditiveLocal
	}

	// Token: 0x020015A0 RID: 5536
	// (Invoke) Token: 0x0600ACDF RID: 44255
	[PublicizedFrom(EAccessModifier.Protected)]
	public delegate void UpdateDelegate();
}
