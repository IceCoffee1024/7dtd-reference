using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200042C RID: 1068
[Preserve]
public abstract class EAIBase
{
	// Token: 0x060020D4 RID: 8404 RVA: 0x000C67D3 File Offset: 0x000C49D3
	public virtual void Init(EntityAlive _theEntity)
	{
		this.executeDelay = 0.5f;
		this.manager = _theEntity.aiManager;
		this.theEntity = _theEntity;
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetData(Dictionary<string, string> data)
	{
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x000C67F4 File Offset: 0x000C49F4
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

	// Token: 0x060020D7 RID: 8407 RVA: 0x000C681C File Offset: 0x000C4A1C
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

	// Token: 0x060020D8 RID: 8408
	public abstract bool CanExecute();

	// Token: 0x060020D9 RID: 8409 RVA: 0x000C6841 File Offset: 0x000C4A41
	public virtual bool Continue()
	{
		return this.CanExecute();
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsContinuous()
	{
		return true;
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Start()
	{
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset()
	{
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsPathUsageBlocked(PathEntity _path)
	{
		return false;
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000C6849 File Offset: 0x000C4A49
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector3 GetTargetPos(EntityAlive theEntity)
	{
		if (theEntity.GetAttackTarget() != null)
		{
			return theEntity.GetAttackTarget().position;
		}
		return theEntity.InvestigatePosition;
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000C686B File Offset: 0x000C4A6B
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool EntityHasTarget(EntityAlive theEntity)
	{
		return theEntity.GetAttackTarget() != null || theEntity.HasInvestigatePosition;
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x060020E1 RID: 8417 RVA: 0x000C6883 File Offset: 0x000C4A83
	public GameRandom Random
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.manager.random;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x060020E2 RID: 8418 RVA: 0x000C6890 File Offset: 0x000C4A90
	public float RandomFloat
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.manager.random.RandomFloat;
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000C68A2 File Offset: 0x000C4AA2
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetRandom(int maxExclusive)
	{
		return this.manager.random.RandomRange(maxExclusive);
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000C68B5 File Offset: 0x000C4AB5
	public override string ToString()
	{
		if (this.shortedTypeName == null)
		{
			this.shortedTypeName = this.GetTypeName().Substring(3);
		}
		return this.shortedTypeName;
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000C68D7 File Offset: 0x000C4AD7
	public string GetTypeName()
	{
		if (this.cachedTypeName == null)
		{
			this.cachedTypeName = base.GetType().Name;
		}
		return this.cachedTypeName;
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAIBase()
	{
	}

	// Token: 0x04001685 RID: 5765
	public EAIManager manager;

	// Token: 0x04001686 RID: 5766
	public EntityAlive theEntity;

	// Token: 0x04001687 RID: 5767
	public float executeWaitTime;

	// Token: 0x04001688 RID: 5768
	public float executeDelay;

	// Token: 0x04001689 RID: 5769
	public int MutexBits;

	// Token: 0x0400168A RID: 5770
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedTypeName;

	// Token: 0x0400168B RID: 5771
	[PublicizedFrom(EAccessModifier.Private)]
	public string shortedTypeName;
}
