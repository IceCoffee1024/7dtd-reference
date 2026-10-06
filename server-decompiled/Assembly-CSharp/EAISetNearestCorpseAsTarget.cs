using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000457 RID: 1111
[Preserve]
public class EAISetNearestCorpseAsTarget : EAITarget
{
	// Token: 0x060021C2 RID: 8642 RVA: 0x000CC574 File Offset: 0x000CA774
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity, 50f, true);
		this.executeDelay = 0.8f;
		this.rndTimeout = 0;
		this.MutexBits = 1;
		this.sorter = new EAISetNearestEntityAsTargetSorter(_theEntity);
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000CC5A8 File Offset: 0x000CA7A8
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

	// Token: 0x060021C4 RID: 8644 RVA: 0x000CC5EC File Offset: 0x000CA7EC
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

	// Token: 0x060021C5 RID: 8645 RVA: 0x000CC729 File Offset: 0x000CA929
	public override void Start()
	{
		base.Start();
		this.theEntity.SetAttackTarget(this.targetEntity, 600);
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x000CC747 File Offset: 0x000CA947
	public override bool Continue()
	{
		return this.targetEntity && this.targetEntity.IsDead() && !(this.targetEntity != this.theEntity.GetAttackTarget());
	}

	// Token: 0x04001751 RID: 5969
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetEntity;

	// Token: 0x04001752 RID: 5970
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityFlags targetFlags;

	// Token: 0x04001753 RID: 5971
	[PublicizedFrom(EAccessModifier.Private)]
	public int rndTimeout;

	// Token: 0x04001754 RID: 5972
	[PublicizedFrom(EAccessModifier.Private)]
	public EAISetNearestEntityAsTargetSorter sorter;

	// Token: 0x04001755 RID: 5973
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Entity> entityList = new List<Entity>();

	// Token: 0x04001756 RID: 5974
	public static bool ZombiesEatAnimalCorpses = true;
}
