using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200041F RID: 1055
[Preserve]
public sealed class AIDirectorSmellMarker : IAIDirectorMarker, IMemoryPoolableObject
{
	// Token: 0x0600206A RID: 8298 RVA: 0x000C41B8 File Offset: 0x000C23B8
	public void Reference()
	{
		this.m_refCount++;
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x000C41C8 File Offset: 0x000C23C8
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

	// Token: 0x0600206C RID: 8300 RVA: 0x000C41FC File Offset: 0x000C23FC
	public void Reset()
	{
		this.m_playerState = null;
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x000C4208 File Offset: 0x000C2408
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

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x0600206F RID: 8303 RVA: 0x000C42F4 File Offset: 0x000C24F4
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

	// Token: 0x06002070 RID: 8304 RVA: 0x000C430C File Offset: 0x000C250C
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

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06002071 RID: 8305 RVA: 0x000C4366 File Offset: 0x000C2566
	public Vector3 Position
	{
		get
		{
			return this.m_pos;
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06002072 RID: 8306 RVA: 0x000C436E File Offset: 0x000C256E
	public Vector3 TargetPosition
	{
		get
		{
			return this.m_targetPos;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06002073 RID: 8307 RVA: 0x000C4376 File Offset: 0x000C2576
	public bool Valid
	{
		get
		{
			return this.m_validTime > 0.0 && (this.Player == null || !this.Player.IsDead());
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06002074 RID: 8308 RVA: 0x000C43A9 File Offset: 0x000C25A9
	public float MaxRadius
	{
		get
		{
			return (float)this.m_radius;
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06002075 RID: 8309 RVA: 0x000C43B2 File Offset: 0x000C25B2
	public float Radius
	{
		get
		{
			return (float)this.m_effectiveRadius;
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06002076 RID: 8310 RVA: 0x000C43BB File Offset: 0x000C25BB
	public float TimeToLive
	{
		get
		{
			return (float)this.m_ttl;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06002077 RID: 8311 RVA: 0x000C43C4 File Offset: 0x000C25C4
	public float ValidTime
	{
		get
		{
			return (float)this.m_validTime;
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06002078 RID: 8312 RVA: 0x000C43CD File Offset: 0x000C25CD
	public float Speed
	{
		get
		{
			return (float)this.m_speed;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06002079 RID: 8313 RVA: 0x000C43D6 File Offset: 0x000C25D6
	public int Priority
	{
		get
		{
			return this.m_priority;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x0600207A RID: 8314 RVA: 0x000C43DE File Offset: 0x000C25DE
	public bool InterruptsNonPlayerAttack
	{
		get
		{
			return this.m_interruptsNonPlayerAttack;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x0600207B RID: 8315 RVA: 0x000C43E6 File Offset: 0x000C25E6
	public bool IsDistraction
	{
		get
		{
			return this.m_isDistraction;
		}
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static AIDirectorSmellMarker Allocate(AIDirectorPlayerState ps, Vector3 position, Vector3 targetPosition, double radius, double strength, double speed, int priority, double ttl, bool interruptsNonPlayerAttack, bool isDistraction)
	{
		return null;
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x000C43F0 File Offset: 0x000C25F0
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

	// Token: 0x04001627 RID: 5671
	public const int kMax = 256;

	// Token: 0x04001628 RID: 5672
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryPooledObject<AIDirectorSmellMarker> s_pool = new MemoryPooledObject<AIDirectorSmellMarker>(256);

	// Token: 0x04001629 RID: 5673
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_radius;

	// Token: 0x0400162A RID: 5674
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_strength;

	// Token: 0x0400162B RID: 5675
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_speed;

	// Token: 0x0400162C RID: 5676
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_ttl;

	// Token: 0x0400162D RID: 5677
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_validTime;

	// Token: 0x0400162E RID: 5678
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_time;

	// Token: 0x0400162F RID: 5679
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_lifetime;

	// Token: 0x04001630 RID: 5680
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_effectiveRadius;

	// Token: 0x04001631 RID: 5681
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_effectiveStrength;

	// Token: 0x04001632 RID: 5682
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_priority;

	// Token: 0x04001633 RID: 5683
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_refCount;

	// Token: 0x04001634 RID: 5684
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_pos;

	// Token: 0x04001635 RID: 5685
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_targetPos;

	// Token: 0x04001636 RID: 5686
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerState m_playerState;

	// Token: 0x04001637 RID: 5687
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_interruptsNonPlayerAttack;

	// Token: 0x04001638 RID: 5688
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isDistraction;
}
