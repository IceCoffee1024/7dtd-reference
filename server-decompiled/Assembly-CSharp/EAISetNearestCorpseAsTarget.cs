using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000451 RID: 1105
[Preserve]
public class EAISetNearestCorpseAsTarget : EAITarget
{
	// Token: 0x060021B6 RID: 8630 RVA: 0x000CBD24 File Offset: 0x000C9F24
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity, 50f, true);
		this.executeDelay = 0.8f;
		this.rndTimeout = 0;
		this.MutexBits = 1;
		this.sorter = new EAISetNearestEntityAsTargetSorter(_theEntity);
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x000CBD58 File Offset: 0x000C9F58
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		string text;
		if (data.TryGetValue("flags", out text))
		{
			EntityClass.ParseEntityFlags(text, ref this.targetFlags);
		}
		base.GetData(data, "maxDistance2d", ref this.maxXZDistance);
	}

	// Token: 0x060021B8 RID: 8632 RVA: 0x000CBD9C File Offset: 0x000C9F9C
	public override bool CanExecute()
	{
		if (this.theEntity.HasInvestigatePosition)
		{
			return false;
		}
		if (this.theEntity.IsSleeping)
		{
			return false;
		}
		if (this.rndTimeout > 0 && base.GetRandom(this.rndTimeout) != 0)
		{
			return false;
		}
		EntityAlive attackTarget = this.theEntity.GetAttackTarget();
		if (attackTarget is EntityPlayer && attackTarget.IsAlive() && base.RandomFloat < 0.95f)
		{
			return false;
		}
		float num = (this.theEntity.IsSleeper ? 7f : this.maxXZDistance);
		this.theEntity.world.GetEntitiesAround(this.targetFlags, this.targetFlags, this.theEntity.position, num, EAISetNearestCorpseAsTarget.entityList);
		EAISetNearestCorpseAsTarget.entityList.Sort(this.sorter);
		EntityAlive entityAlive = null;
		for (int i = 0; i < EAISetNearestCorpseAsTarget.entityList.Count; i++)
		{
			EntityAlive entityAlive2 = EAISetNearestCorpseAsTarget.entityList[i] as EntityAlive;
			if (entityAlive2 && entityAlive2.IsDead() && (EAISetNearestCorpseAsTarget.ZombiesEatAnimalCorpses || (!(entityAlive2 is EntityAnimal) && !(entityAlive2 is EntityEnemyAnimal))))
			{
				entityAlive = entityAlive2;
				break;
			}
		}
		EAISetNearestCorpseAsTarget.entityList.Clear();
		this.targetEntity = entityAlive;
		return this.targetEntity != null;
	}

	// Token: 0x060021B9 RID: 8633 RVA: 0x000CBED9 File Offset: 0x000CA0D9
	public override void Start()
	{
		base.Start();
		this.theEntity.SetAttackTarget(this.targetEntity, 600);
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000CBEF7 File Offset: 0x000CA0F7
	public override bool Continue()
	{
		return this.targetEntity && this.targetEntity.IsDead() && !(this.targetEntity != this.theEntity.GetAttackTarget());
	}

	// Token: 0x04001744 RID: 5956
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetEntity;

	// Token: 0x04001745 RID: 5957
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityFlags targetFlags;

	// Token: 0x04001746 RID: 5958
	[PublicizedFrom(EAccessModifier.Private)]
	public int rndTimeout;

	// Token: 0x04001747 RID: 5959
	[PublicizedFrom(EAccessModifier.Private)]
	public EAISetNearestEntityAsTargetSorter sorter;

	// Token: 0x04001748 RID: 5960
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Entity> entityList = new List<Entity>();

	// Token: 0x04001749 RID: 5961
	public static bool ZombiesEatAnimalCorpses = true;
}
