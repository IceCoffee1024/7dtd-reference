using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015F6 RID: 5622
[Preserve]
public class vp_FPEarthquake : MonoBehaviour
{
	// Token: 0x1700146C RID: 5228
	// (get) Token: 0x0600AFF6 RID: 45046 RVA: 0x0041A4F7 File Offset: 0x004186F7
	public vp_FPPlayerEventHandler FPPlayer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_FPPlayer == null)
			{
				this.m_FPPlayer = global::UnityEngine.Object.FindAnyObjectByType<vp_FPPlayerEventHandler>();
			}
			return this.m_FPPlayer;
		}
	}

	// Token: 0x0600AFF7 RID: 45047 RVA: 0x0041A518 File Offset: 0x00418718
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600AFF8 RID: 45048 RVA: 0x0041A534 File Offset: 0x00418734
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600AFF9 RID: 45049 RVA: 0x0041A550 File Offset: 0x00418750
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FixedUpdate()
	{
		if (Time.timeScale != 0f)
		{
			this.UpdateEarthQuake();
		}
	}

	// Token: 0x0600AFFA RID: 45050 RVA: 0x0041A564 File Offset: 0x00418764
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateEarthQuake()
	{
		if (!this.FPPlayer.CameraEarthQuake.Active)
		{
			this.m_CameraEarthQuakeForce = Vector3.zero;
			return;
		}
		this.m_CameraEarthQuakeForce = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(1f), this.m_Magnitude.x * (Vector3.right + Vector3.forward) * Mathf.Min(this.m_Endtime - Time.time, 1f) * Time.timeScale);
		this.m_CameraEarthQuakeForce.y = 0f;
		if (global::UnityEngine.Random.value < 0.3f * Time.timeScale)
		{
			this.m_CameraEarthQuakeForce.y = global::UnityEngine.Random.Range(0f, this.m_Magnitude.y * 0.35f) * Mathf.Min(this.m_Endtime - Time.time, 1f);
		}
	}

	// Token: 0x0600AFFB RID: 45051 RVA: 0x0041A648 File Offset: 0x00418848
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_CameraEarthQuake()
	{
		Vector3 vector = (Vector3)this.FPPlayer.CameraEarthQuake.Argument;
		this.m_Magnitude.x = vector.x;
		this.m_Magnitude.y = vector.y;
		this.m_Endtime = Time.time + vector.z;
		this.FPPlayer.CameraEarthQuake.AutoDuration = vector.z;
	}

	// Token: 0x0600AFFC RID: 45052 RVA: 0x0041A6B5 File Offset: 0x004188B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.FPPlayer.CameraEarthQuake.TryStart<Vector3>(new Vector3(impact * 0.5f, impact * 0.5f, 1f));
	}

	// Token: 0x1700146D RID: 5229
	// (get) Token: 0x0600AFFD RID: 45053 RVA: 0x0041A6E0 File Offset: 0x004188E0
	// (set) Token: 0x0600AFFE RID: 45054 RVA: 0x0041A6E8 File Offset: 0x004188E8
	public virtual Vector3 OnValue_CameraEarthQuakeForce
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CameraEarthQuakeForce;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_CameraEarthQuakeForce = value;
		}
	}

	// Token: 0x0400839A RID: 33690
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraEarthQuakeForce;

	// Token: 0x0400839B RID: 33691
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Endtime;

	// Token: 0x0400839C RID: 33692
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_Magnitude = Vector2.zero;

	// Token: 0x0400839D RID: 33693
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
