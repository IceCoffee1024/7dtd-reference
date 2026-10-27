using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000421 RID: 1057
[Preserve]
public sealed class AIDirectorSmellMarker : IAIDirectorMarker, IMemoryPoolableObject
{
	// Token: 0x0600206F RID: 8303 RVA: 0x000C4564 File Offset: 0x000C2764
	public void Reference()
	{
		this.m_refCount++;
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x000C4574 File Offset: 0x000C2774
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

	// Token: 0x06002071 RID: 8305 RVA: 0x000C45A8 File Offset: 0x000C27A8
	public void Reset()
	{
		this.m_playerState = null;
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x000C45B4 File Offset: 0x000C27B4
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
	// (get) Token: 0x06002074 RID: 8308 RVA: 0x000C46A0 File Offset: 0x000C28A0
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

	// Token: 0x06002075 RID: 8309 RVA: 0x000C46B8 File Offset: 0x000C28B8
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
	// (get) Token: 0x06002076 RID: 8310 RVA: 0x000C4712 File Offset: 0x000C2912
	public Vector3 Position
	{
		get
		{
			return this.m_pos;
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06002077 RID: 8311 RVA: 0x000C471A File Offset: 0x000C291A
	public Vector3 TargetPosition
	{
		get
		{
			return this.m_targetPos;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06002078 RID: 8312 RVA: 0x000C4722 File Offset: 0x000C2922
	public bool Valid
	{
		get
		{
			return this.m_validTime > 0.0 && (this.Player == null || !this.Player.IsDead());
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06002079 RID: 8313 RVA: 0x000C4755 File Offset: 0x000C2955
	public float MaxRadius
	{
		get
		{
			return (float)this.m_radius;
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600207A RID: 8314 RVA: 0x000C475E File Offset: 0x000C295E
	public float Radius
	{
		get
		{
			return (float)this.m_effectiveRadius;
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x0600207B RID: 8315 RVA: 0x000C4767 File Offset: 0x000C2967
	public float TimeToLive
	{
		get
		{
			return (float)this.m_ttl;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x0600207C RID: 8316 RVA: 0x000C4770 File Offset: 0x000C2970
	public float ValidTime
	{
		get
		{
			return (float)this.m_validTime;
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x0600207D RID: 8317 RVA: 0x000C4779 File Offset: 0x000C2979
	public float Speed
	{
		get
		{
			return (float)this.m_speed;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x0600207E RID: 8318 RVA: 0x000C4782 File Offset: 0x000C2982
	public int Priority
	{
		get
		{
			return this.m_priority;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x0600207F RID: 8319 RVA: 0x000C478A File Offset: 0x000C298A
	public bool InterruptsNonPlayerAttack
	{
		get
		{
			return this.m_interruptsNonPlayerAttack;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06002080 RID: 8320 RVA: 0x000C4792 File Offset: 0x000C2992
	public bool IsDistraction
	{
		get
		{
			return this.m_isDistraction;
		}
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x000200DA File Offset: 0x0001E2DA
	public static AIDirectorSmellMarker Allocate(AIDirectorPlayerState ps, Vector3 position, Vector3 targetPosition, double radius, double strength, double speed, int priority, double ttl, bool interruptsNonPlayerAttack, bool isDistraction)
	{
		return null;
	}

	// Token: 0x06002082 RID: 8322 RVA: 0x000C479C File Offset: 0x000C299C
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

	// Token: 0x04001637 RID: 5687
	public const int kMax = 256;

	// Token: 0x04001638 RID: 5688
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryPooledObject<AIDirectorSmellMarker> s_pool = new MemoryPooledObject<AIDirectorSmellMarker>(256);

	// Token: 0x04001639 RID: 5689
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_radius;

	// Token: 0x0400163A RID: 5690
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_strength;

	// Token: 0x0400163B RID: 5691
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_speed;

	// Token: 0x0400163C RID: 5692
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_ttl;

	// Token: 0x0400163D RID: 5693
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_validTime;

	// Token: 0x0400163E RID: 5694
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_time;

	// Token: 0x0400163F RID: 5695
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_lifetime;

	// Token: 0x04001640 RID: 5696
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_effectiveRadius;

	// Token: 0x04001641 RID: 5697
	[PublicizedFrom(EAccessModifier.Private)]
	public double m_effectiveStrength;

	// Token: 0x04001642 RID: 5698
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_priority;

	// Token: 0x04001643 RID: 5699
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_refCount;

	// Token: 0x04001644 RID: 5700
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_pos;

	// Token: 0x04001645 RID: 5701
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_targetPos;

	// Token: 0x04001646 RID: 5702
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerState m_playerState;

	// Token: 0x04001647 RID: 5703
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_interruptsNonPlayerAttack;

	// Token: 0x04001648 RID: 5704
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isDistraction;
}
