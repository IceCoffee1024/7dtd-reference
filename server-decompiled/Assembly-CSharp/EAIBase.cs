using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000432 RID: 1074
[Preserve]
public abstract class EAIBase
{
	// Token: 0x060020E0 RID: 8416 RVA: 0x000C7093 File Offset: 0x000C5293
	public virtual void Init(EntityAlive _theEntity)
	{
		this.executeDelay = 0.5f;
		this.manager = _theEntity.aiManager;
		this.theEntity = _theEntity;
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetData(Dictionary<string, string> data)
	{
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000C70B4 File Offset: 0x000C52B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void GetData(Dictionary<string, string> data, string name, ref float value)
	{
		string text;
		float num;
		if (data.TryGetValue(name, out text) && StringParsers.TryParseFloat(text, out num))
		{
			value = num;
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000C70DC File Offset: 0x000C52DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void GetData(Dictionary<string, string> data, string name, ref int value)
	{
		string text;
		int num;
		if (data.TryGetValue(name, out text) && StringParsers.TryParseSInt32(text, out num))
		{
			value = num;
		}
	}

	// Token: 0x060020E4 RID: 8420
	public abstract bool CanExecute();

	// Token: 0x060020E5 RID: 8421 RVA: 0x000C7101 File Offset: 0x000C5301
	public virtual bool Continue()
	{
		return this.CanExecute();
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsContinuous()
	{
		return true;
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Start()
	{
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset()
	{
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsPathUsageBlocked(PathEntity _path)
	{
		return false;
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x000C7109 File Offset: 0x000C5309
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector3 GetTargetPos(EntityAlive theEntity)
	{
		if (theEntity.GetAttackTarget() != null)
		{
			return theEntity.GetAttackTarget().position;
		}
		return theEntity.InvestigatePosition;
	}

	// Token: 0x060020EC RID: 8428 RVA: 0x000C712B File Offset: 0x000C532B
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool EntityHasTarget(EntityAlive theEntity)
	{
		return theEntity.GetAttackTarget() != null || theEntity.HasInvestigatePosition;
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x060020ED RID: 8429 RVA: 0x000C7143 File Offset: 0x000C5343
	public GameRandom Random
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.manager.random;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x060020EE RID: 8430 RVA: 0x000C7150 File Offset: 0x000C5350
	public float RandomFloat
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.manager.random.RandomFloat;
		}
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x000C7162 File Offset: 0x000C5362
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetRandom(int maxExclusive)
	{
		return this.manager.random.RandomRange(maxExclusive);
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x000C7175 File Offset: 0x000C5375
	public override string ToString()
	{
		if (this.shortedTypeName == null)
		{
			this.shortedTypeName = this.GetTypeName().Substring(3);
		}
		return this.shortedTypeName;
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x000C7197 File Offset: 0x000C5397
	public string GetTypeName()
	{
		if (this.cachedTypeName == null)
		{
			this.cachedTypeName = base.GetType().Name;
		}
		return this.cachedTypeName;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAIBase()
	{
	}

	// Token: 0x04001691 RID: 5777
	public EAIManager manager;

	// Token: 0x04001692 RID: 5778
	public EntityAlive theEntity;

	// Token: 0x04001693 RID: 5779
	public float executeWaitTime;

	// Token: 0x04001694 RID: 5780
	public float executeDelay;

	// Token: 0x04001695 RID: 5781
	public int MutexBits;

	// Token: 0x04001696 RID: 5782
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedTypeName;

	// Token: 0x04001697 RID: 5783
	[PublicizedFrom(EAccessModifier.Private)]
	public string shortedTypeName;
}
