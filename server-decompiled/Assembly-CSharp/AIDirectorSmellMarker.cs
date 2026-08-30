using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000419 RID: 1049
[Preserve]
public sealed class AIDirectorSmellMarker : IAIDirectorMarker, IMemoryPoolableObject
{
	// Token: 0x0600205E RID: 8286 RVA: 0x000C3968 File Offset: 0x000C1B68
	public void Reference()
	{
		this.m_refCount++;
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000C3978 File Offset: 0x000C1B78
	public bool Release()
	{
		int num = this.m_refCount - 1;
		this.m_refCount = num;
		if (num == 0)
		{
			this.Reset();
			AIDirectorSmellMarker.s_pool.Free(this);
			return true;
		}
		return false;
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000C39AC File Offset: 0x000C1BAC
	public void Reset()
	{
		this.m_playerState = null;
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000C39B8 File Offset: 0x000C1BB8
	public void Tick(double dt)
	{
		this.m_ttl -= dt;
		if (this.m_ttl < 0.0)
		{
			this.m_ttl = 0.0;
		}
		this.m_validTime -= dt;
		if (this.m_validTime < 0.0)
		{
			this.m_validTime = 0.0;
		}
		this.m_time += dt;
		if (this.m_time > this.m_lifetime)
		{
			this.m_time = this.m_lifetime;
		}
		this.m_effectiveRadius = ((this.m_speed > 0.0) ? Math.Min(this.m_radius, this.m_speed * this.m_time) : this.m_radius);
		this.m_effectiveStrength = this.m_strength * (1.0 - this.m_time / this.m_lifetime);
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06002063 RID: 8291 RVA: 0x000C3AA4 File Offset: 0x000C1CA4
	public EntityPlayer Player
	{
		get
		{
			if (this.m_playerState != null)
			{
				return this.m_playerState.Player;
			}
			return null;
		}
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x000C3ABC File Offset: 0x000C1CBC
	public double IntensityForPosition(Vector3 position)
	{
		double num = (double)(this.m_pos - position).magnitude;
		if (num > this.m_effectiveRadius)
		{
			return 0.0;
		}
		double num2 = 1.0;
		if (num > 0.0)
		{
			num2 /= num * num;
		}
		return this.m_effectiveStrength * num2;
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06002065 RID: 8293 RVA: 0x000C3B16 File Offset: 0x000C1D16
	public Vector3 Position
	{
		get
		{
			return this.m_pos;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x000C3B1E File Offset: 0x000C1D1E
	public Vector3 TargetPosition
	{
		get
		{
			return this.m_targetPos;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06002067 RID: 8295 RVA: 0x000C3B26 File Offset: 0x000C1D26
	public bool Valid
	{
		get
		{
			return this.m_validTime > 0.0 && (this.Player == null || !this.Player.IsDead());
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06002068 RID: 8296 RVA: 0x000C3B59 File Offset: 0x000C1D59
	public float MaxRadius
	{
		get
		{
			return (float)this.m_radius;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06002069 RID: 8297 RVA: 0x000C3B62 File Offset: 0x000C1D62
	public float Radius
	{
		get
		{
			return (float)this.m_effectiveRadius;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x0600206A RID: 8298 RVA: 0x000C3B6B File Offset: 0x000C1D6B
	public float TimeToLive
	{
		get
		{
			return (float)this.m_ttl;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x0600206B RID: 8299 RVA: 0x000C3B74 File Offset: 0x000C1D74
	public float ValidTime
	{
		get
		{
			return (float)this.m_validTime;
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x0600206C RID: 8300 RVA: 0x000C3B7D File Offset: 0x000C1D7D
	public float Speed
	{
		get
		{
			return (float)this.m_speed;
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x0600206D RID: 8301 RVA: 0x000C3B86 File Offset: 0x000C1D86
	public int Priority
	{
		get
		{
			return this.m_priority;
		}
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x0600206E RID: 8302 RVA: 0x000C3B8E File Offset: 0x000C1D8E
	public bool InterruptsNonPlayerAttack
	{
		get
		{
			return this.m_interruptsNonPlayerAttack;
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x0600206F RID: 8303 RVA: 0x000C3B96 File Offset: 0x000C1D96
	public bool IsDistraction
	{
		get
		{
			return this.m_isDistraction;
		}
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public static AIDirectorSmellMarker Allocate(AIDirectorPlayerState ps, Vector3 position, Vector3 targetPosition, double radius, double strength, double speed, int priority, double ttl, bool interruptsNonPlayerAttack, bool isDistraction)
	{
		return null;
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x000C3BA0 File Offset: 0x000C1DA0
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorSmellMarker Construct(AIDirectorPlayerState ps, Vector3 position, Vector3 targetPosition, double radius, double strength, double speed, int priority, double ttl, bool interruptsNonPlayerAttack, bool isDistraction)
	{
		this.m_refCount = 1;
		this.m_playerState = ps;
		this.m_pos = position;
		this.m_targetPos = targetPosition;
		this.m_radius = radius;
		this.m_strength = strength;
		this.m_speed = speed;
		this.m_priority = priority;
		this.m_validTime = ttl;
		this.m_lifetime = ttl;
		this.m_time = 0.0;
		this.m_effectiveRadius = 0.0;
		this.m_effectiveStrength = strength;
		this.m_interruptsNonPlayerAttack = interruptsNonPlayerAttack;
		this.m_isDistraction = isDistraction;
		if (isDistraction)
		{
			this.m_ttl = (double)Mathf.Max((float)ttl, 20f);
		}
		else
		{
			this.m_ttl = (double)Constants.cEnemySenseMemory;
		}
		return this;
	}

	// Token: 0x0400161B RID: 5659
	public const int kMax = 256;

	// Token: 0x0400161C RID: 5660
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryPooledObject<AIDirectorSmellMarker> s_pool = new MemoryPooledObject<AIDirectorSmellMarker>(256);

	// Token: 0x0400161D RID: 5661
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_radius;

	// Token: 0x0400161E RID: 5662
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_strength;

	// Token: 0x0400161F RID: 5663
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_speed;

	// Token: 0x04001620 RID: 5664
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_ttl;

	// Token: 0x04001621 RID: 5665
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_validTime;

	// Token: 0x04001622 RID: 5666
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_time;

	// Token: 0x04001623 RID: 5667
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_lifetime;

	// Token: 0x04001624 RID: 5668
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_effectiveRadius;

	// Token: 0x04001625 RID: 5669
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_effectiveStrength;

	// Token: 0x04001626 RID: 5670
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_priority;

	// Token: 0x04001627 RID: 5671
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_refCount;

	// Token: 0x04001628 RID: 5672
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_pos;

	// Token: 0x04001629 RID: 5673
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_targetPos;

	// Token: 0x0400162A RID: 5674
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerState m_playerState;

	// Token: 0x0400162B RID: 5675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_interruptsNonPlayerAttack;

	// Token: 0x0400162C RID: 5676
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isDistraction;
}
