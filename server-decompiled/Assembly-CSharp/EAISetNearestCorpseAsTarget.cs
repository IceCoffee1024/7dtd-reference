using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000459 RID: 1113
[Preserve]
public class EAISetNearestCorpseAsTarget : EAITarget
{
	// Token: 0x060021C7 RID: 8647 RVA: 0x000CC920 File Offset: 0x000CAB20
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity, 50f, true);
		this.executeDelay = 0.8f;
		this.rndTimeout = 0;
		this.MutexBits = 1;
		this.sorter = new EAISetNearestEntityAsTargetSorter(_theEntity);
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x000CC954 File Offset: 0x000CAB54
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

	// Token: 0x060021C9 RID: 8649 RVA: 0x000CC998 File Offset: 0x000CAB98
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

	// Token: 0x060021CA RID: 8650 RVA: 0x000CCAD5 File Offset: 0x000CACD5
	public override void Start()
	{
		base.Start();
		this.theEntity.SetAttackTarget(this.targetEntity, 600);
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x000CCAF3 File Offset: 0x000CACF3
	public override bool Continue()
	{
		return this.targetEntity && this.targetEntity.IsDead() && !(this.targetEntity != this.theEntity.GetAttackTarget());
	}

	// Token: 0x04001761 RID: 5985
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetEntity;

	// Token: 0x04001762 RID: 5986
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityFlags targetFlags;

	// Token: 0x04001763 RID: 5987
	[PublicizedFrom(EAccessModifier.Private)]
	public int rndTimeout;

	// Token: 0x04001764 RID: 5988
	[PublicizedFrom(EAccessModifier.Private)]
	public EAISetNearestEntityAsTargetSorter sorter;

	// Token: 0x04001765 RID: 5989
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Entity> entityList = new List<Entity>();

	// Token: 0x04001766 RID: 5990
	public static bool ZombiesEatAnimalCorpses = true;
}
