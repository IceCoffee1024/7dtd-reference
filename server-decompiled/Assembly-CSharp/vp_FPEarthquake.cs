using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020015DB RID: 5595
[Preserve]
public class vp_FPEarthquake : MonoBehaviour
{
	// Token: 0x17001433 RID: 5171
	// (get) Token: 0x0600AEF9 RID: 44793 RVA: 0x004151B7 File Offset: 0x004133B7
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

	// Token: 0x0600AEFA RID: 44794 RVA: 0x004151D8 File Offset: 0x004133D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600AEFB RID: 44795 RVA: 0x004151F4 File Offset: 0x004133F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600AEFC RID: 44796 RVA: 0x00415210 File Offset: 0x00413410
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FixedUpdate()
	{
		if (Time.timeScale != 0f)
		{
			this.UpdateEarthQuake();
		}
	}

	// Token: 0x0600AEFD RID: 44797 RVA: 0x00415224 File Offset: 0x00413424
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

	// Token: 0x0600AEFE RID: 44798 RVA: 0x00415308 File Offset: 0x00413508
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_CameraEarthQuake()
	{
		Vector3 vector = (Vector3)this.FPPlayer.CameraEarthQuake.Argument;
		this.m_Magnitude.x = vector.x;
		this.m_Magnitude.y = vector.y;
		this.m_Endtime = Time.time + vector.z;
		this.FPPlayer.CameraEarthQuake.AutoDuration = vector.z;
	}

	// Token: 0x0600AEFF RID: 44799 RVA: 0x00415375 File Offset: 0x00413575
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_CameraBombShake(float impact)
	{
		this.FPPlayer.CameraEarthQuake.TryStart<Vector3>(new Vector3(impact * 0.5f, impact * 0.5f, 1f));
	}

	// Token: 0x17001434 RID: 5172
	// (get) Token: 0x0600AF00 RID: 44800 RVA: 0x004153A0 File Offset: 0x004135A0
	// (set) Token: 0x0600AF01 RID: 44801 RVA: 0x004153A8 File Offset: 0x004135A8
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

	// Token: 0x040082C1 RID: 33473
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CameraEarthQuakeForce;

	// Token: 0x040082C2 RID: 33474
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Endtime;

	// Token: 0x040082C3 RID: 33475
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_Magnitude = Vector2.zero;

	// Token: 0x040082C4 RID: 33476
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
