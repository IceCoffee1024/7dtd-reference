using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001627 RID: 5671
[Preserve]
public class vp_FPEarthquake : MonoBehaviour
{
	// Token: 0x170014BB RID: 5307
	// (get) Token: 0x0600B0E0 RID: 45280 RVA: 0x004205D7 File Offset: 0x0041E7D7
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

	// Token: 0x0600B0E1 RID: 45281 RVA: 0x004205F8 File Offset: 0x0041E7F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600B0E2 RID: 45282 RVA: 0x00420614 File Offset: 0x0041E814
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B0E3 RID: 45283 RVA: 0x00420630 File Offset: 0x0041E830
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FixedUpdate()
	{
		if (Time.timeScale != 0f)
		{
			this.UpdateEarthQuake();
		}
	}

	// Token: 0x0600B0E4 RID: 45284 RVA: 0x00420644 File Offset: 0x0041E844
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

	// Token: 0x0600B0E5 RID: 45285 RVA: 0x00420728 File Offset: 0x0041E928
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_CameraEarthQuake()
	{
		Vector3 vector = (Vector3)this.FPPlayer.CameraEarthQuake.Argument;
		this.m_Magnitude.x = vector.x;
		this.m_Magnitude.y = vector.y;
		this.m_Endtime = Time.time + vector.z;
		this.FPPlayer.CameraEarthQuake.AutoDuration = vector.z;
	}

	// Token: 0x0600B0E6 RID: 45286 RVA: 0x00420795 File Offset: 0x0041E995
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.FPPlayer.CameraEarthQuake.TryStart<Vector3>(new Vector3(impact * 0.5f, impact * 0.5f, 1f));
	}

	// Token: 0x170014BC RID: 5308
	// (get) Token: 0x0600B0E7 RID: 45287 RVA: 0x004207C0 File Offset: 0x0041E9C0
	// (set) Token: 0x0600B0E8 RID: 45288 RVA: 0x004207C8 File Offset: 0x0041E9C8
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

	// Token: 0x04008431 RID: 33841
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraEarthQuakeForce;

	// Token: 0x04008432 RID: 33842
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Endtime;

	// Token: 0x04008433 RID: 33843
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_Magnitude = Vector2.zero;

	// Token: 0x04008434 RID: 33844
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
