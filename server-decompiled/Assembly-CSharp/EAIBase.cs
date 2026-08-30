using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000430 RID: 1072
[Preserve]
public abstract class EAIBase
{
	// Token: 0x060020F1 RID: 8433 RVA: 0x000C6FDF File Offset: 0x000C51DF
	public virtual void Init(EntityAlive _theEntity)
	{
		this.executeDelay = 0.5f;
		this.manager = _theEntity.aiManager;
		this.theEntity = _theEntity;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetData(Dictionary<string, string> data)
	{
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000C7000 File Offset: 0x000C5200
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

	// Token: 0x060020F4 RID: 8436 RVA: 0x000C7028 File Offset: 0x000C5228
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

	// Token: 0x060020F5 RID: 8437
	public abstract bool CanExecute();

	// Token: 0x060020F6 RID: 8438 RVA: 0x000C704D File Offset: 0x000C524D
	public virtual bool Continue()
	{
		return this.CanExecute();
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool IsContinuous()
	{
		return true;
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Start()
	{
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset()
	{
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsPathUsageBlocked(PathEntity _path)
	{
		return false;
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x000C7055 File Offset: 0x000C5255
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector3 GetTargetPos(EntityAlive theEntity)
	{
		if (theEntity.GetAttackTarget() != null)
		{
			return theEntity.GetAttackTarget().position;
		}
		return theEntity.InvestigatePosition;
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x000C7077 File Offset: 0x000C5277
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool EntityHasTarget(EntityAlive theEntity)
	{
		return theEntity.GetAttackTarget() != null || theEntity.HasInvestigatePosition;
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060020FE RID: 8446 RVA: 0x000C708F File Offset: 0x000C528F
	public GameRandom Random
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.manager.random;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060020FF RID: 8447 RVA: 0x000C709C File Offset: 0x000C529C
	public float RandomFloat
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.manager.random.RandomFloat;
		}
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000C70AE File Offset: 0x000C52AE
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetRandom(int maxExclusive)
	{
		return this.manager.random.RandomRange(maxExclusive);
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000C70C1 File Offset: 0x000C52C1
	public override string ToString()
	{
		if (this.shortedTypeName == null)
		{
			this.shortedTypeName = this.GetTypeName().Substring(3);
		}
		return this.shortedTypeName;
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x000C70E3 File Offset: 0x000C52E3
	public string GetTypeName()
	{
		if (this.cachedTypeName == null)
		{
			this.cachedTypeName = base.GetType().Name;
		}
		return this.cachedTypeName;
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAIBase()
	{
	}

	// Token: 0x0400169A RID: 5786
	public EAIManager manager;

	// Token: 0x0400169B RID: 5787
	public EntityAlive theEntity;

	// Token: 0x0400169C RID: 5788
	public float executeWaitTime;

	// Token: 0x0400169D RID: 5789
	public float executeDelay;

	// Token: 0x0400169E RID: 5790
	public int MutexBits;

	// Token: 0x0400169F RID: 5791
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedTypeName;

	// Token: 0x040016A0 RID: 5792
	[PublicizedFrom(EAccessModifier.Private)]
	public string shortedTypeName;
}
