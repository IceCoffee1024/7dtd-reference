using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001615 RID: 5653
[Preserve]
public class vp_FPEarthquake : MonoBehaviour
{
	// Token: 0x170014AF RID: 5295
	// (get) Token: 0x0600B088 RID: 45192 RVA: 0x0041E1D3 File Offset: 0x0041C3D3
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

	// Token: 0x0600B089 RID: 45193 RVA: 0x0041E1F4 File Offset: 0x0041C3F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600B08A RID: 45194 RVA: 0x0041E210 File Offset: 0x0041C410
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B08B RID: 45195 RVA: 0x0041E22C File Offset: 0x0041C42C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FixedUpdate()
	{
		if (Time.timeScale != 0f)
		{
			this.UpdateEarthQuake();
		}
	}

	// Token: 0x0600B08C RID: 45196 RVA: 0x0041E240 File Offset: 0x0041C440
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

	// Token: 0x0600B08D RID: 45197 RVA: 0x0041E324 File Offset: 0x0041C524
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_CameraEarthQuake()
	{
		Vector3 vector = (Vector3)this.FPPlayer.CameraEarthQuake.Argument;
		this.m_Magnitude.x = vector.x;
		this.m_Magnitude.y = vector.y;
		this.m_Endtime = Time.time + vector.z;
		this.FPPlayer.CameraEarthQuake.AutoDuration = vector.z;
	}

	// Token: 0x0600B08E RID: 45198 RVA: 0x0041E391 File Offset: 0x0041C591
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.FPPlayer.CameraEarthQuake.TryStart<Vector3>(new Vector3(impact * 0.5f, impact * 0.5f, 1f));
	}

	// Token: 0x170014B0 RID: 5296
	// (get) Token: 0x0600B08F RID: 45199 RVA: 0x0041E3BC File Offset: 0x0041C5BC
	// (set) Token: 0x0600B090 RID: 45200 RVA: 0x0041E3C4 File Offset: 0x0041C5C4
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

	// Token: 0x040083F0 RID: 33776
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraEarthQuakeForce;

	// Token: 0x040083F1 RID: 33777
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Endtime;

	// Token: 0x040083F2 RID: 33778
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_Magnitude = Vector2.zero;

	// Token: 0x040083F3 RID: 33779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
