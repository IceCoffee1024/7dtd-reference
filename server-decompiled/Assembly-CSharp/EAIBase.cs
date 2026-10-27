using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000434 RID: 1076
[Preserve]
public abstract class EAIBase
{
	// Token: 0x060020E5 RID: 8421 RVA: 0x000C743F File Offset: 0x000C563F
	public virtual void Init(EntityAlive _theEntity)
	{
		this.executeDelay = 0.5f;
		this.manager = _theEntity.aiManager;
		this.theEntity = _theEntity;
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetData(Dictionary<string, string> data)
	{
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000C7460 File Offset: 0x000C5660
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

	// Token: 0x060020E8 RID: 8424 RVA: 0x000C7488 File Offset: 0x000C5688
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

	// Token: 0x060020E9 RID: 8425
	public abstract bool CanExecute();

	// Token: 0x060020EA RID: 8426 RVA: 0x000C74AD File Offset: 0x000C56AD
	public virtual bool Continue()
	{
		return this.CanExecute();
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsContinuous()
	{
		return true;
	}

	// Token: 0x060020EC RID: 8428 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Start()
	{
	}

	// Token: 0x060020ED RID: 8429 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset()
	{
	}

	// Token: 0x060020EE RID: 8430 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsPathUsageBlocked(PathEntity _path)
	{
		return false;
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x000C74B5 File Offset: 0x000C56B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector3 GetTargetPos(EntityAlive theEntity)
	{
		if (theEntity.GetAttackTarget() != null)
		{
			return theEntity.GetAttackTarget().position;
		}
		return theEntity.InvestigatePosition;
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x000C74D7 File Offset: 0x000C56D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool EntityHasTarget(EntityAlive theEntity)
	{
		return theEntity.GetAttackTarget() != null || theEntity.HasInvestigatePosition;
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000C74EF File Offset: 0x000C56EF
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
	// (get) Token: 0x060020F3 RID: 8435 RVA: 0x000C74FC File Offset: 0x000C56FC
	public float RandomFloat
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.manager.random.RandomFloat;
		}
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x000C750E File Offset: 0x000C570E
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetRandom(int maxExclusive)
	{
		return this.manager.random.RandomRange(maxExclusive);
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x000C7521 File Offset: 0x000C5721
	public override string ToString()
	{
		if (this.shortedTypeName == null)
		{
			this.shortedTypeName = this.GetTypeName().Substring(3);
		}
		return this.shortedTypeName;
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000C7543 File Offset: 0x000C5743
	public string GetTypeName()
	{
		if (this.cachedTypeName == null)
		{
			this.cachedTypeName = base.GetType().Name;
		}
		return this.cachedTypeName;
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAIBase()
	{
	}

	// Token: 0x040016A1 RID: 5793
	public EAIManager manager;

	// Token: 0x040016A2 RID: 5794
	public EntityAlive theEntity;

	// Token: 0x040016A3 RID: 5795
	public float executeWaitTime;

	// Token: 0x040016A4 RID: 5796
	public float executeDelay;

	// Token: 0x040016A5 RID: 5797
	public int MutexBits;

	// Token: 0x040016A6 RID: 5798
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedTypeName;

	// Token: 0x040016A7 RID: 5799
	[PublicizedFrom(EAccessModifier.Private)]
	public string shortedTypeName;
}
