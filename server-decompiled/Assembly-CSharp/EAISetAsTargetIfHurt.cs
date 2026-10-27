using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000457 RID: 1111
[Preserve]
public class EAISetAsTargetIfHurt : EAITarget
{
	// Token: 0x060021BE RID: 8638 RVA: 0x000CC5B7 File Offset: 0x000CA7B7
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity, 0f, false);
		this.MutexBits = 1;
	}

	// Token: 0x060021BF RID: 8639 RVA: 0x000CC5D0 File Offset: 0x000CA7D0
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		this.targetClasses = new List<EAISetAsTargetIfHurt.TargetClass>();
		string text;
		if (data.TryGetValue("class", out text))
		{
			string[] array = text.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				EAISetAsTargetIfHurt.TargetClass targetClass = default(EAISetAsTargetIfHurt.TargetClass);
				targetClass.type = EntityFactory.GetEntityType(array[i]);
				this.targetClasses.Add(targetClass);
			}
		}
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x000CC63C File Offset: 0x000CA83C
	public override bool CanExecute()
	{
		EntityAlive revengeTarget = this.theEntity.GetRevengeTarget();
		EntityAlive attackTarget = this.theEntity.GetAttackTarget();
		if (revengeTarget && revengeTarget != attackTarget && revengeTarget.entityType != this.theEntity.entityType)
		{
			if (this.targetClasses != null)
			{
				bool flag = false;
				Type type = revengeTarget.GetType();
				for (int i = 0; i < this.targetClasses.Count; i++)
				{
					EAISetAsTargetIfHurt.TargetClass targetClass = this.targetClasses[i];
					if (targetClass.type != null && targetClass.type.IsAssignableFrom(type))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (attackTarget != null && attackTarget.IsAlive() && base.RandomFloat < 0.66f)
			{
				this.theEntity.SetRevengeTarget(null);
				return false;
			}
			if (base.check(revengeTarget))
			{
				return true;
			}
			Vector3 vector = this.theEntity.position - revengeTarget.position;
			float searchRadius = EntityClass.list[this.theEntity.entityClass].SearchRadius;
			vector = revengeTarget.position + vector.normalized * (searchRadius * 0.35f);
			Vector2 vector2 = this.manager.random.RandomInsideUnitCircle * searchRadius;
			vector.x += vector2.x;
			vector.z += vector2.y;
			Vector3i vector3i = World.worldToBlockPos(vector);
			int height = (int)this.theEntity.world.GetHeight(vector3i.x, vector3i.z);
			if (height > 0)
			{
				vector.y = (float)height;
			}
			int num = this.theEntity.CalcInvestigateTicks(1200, revengeTarget);
			this.theEntity.SetInvestigatePosition(vector, num, true);
			this.theEntity.SetRevengeTarget(null);
		}
		return false;
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x000CC820 File Offset: 0x000CAA20
	public override void Start()
	{
		this.theEntity.SetAttackTarget(this.theEntity.GetRevengeTarget(), 400);
		this.viewAngleSave = this.theEntity.GetMaxViewAngle();
		this.theEntity.SetMaxViewAngle(270f);
		this.viewAngleRestoreCounter = 100;
		base.Start();
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x000CC877 File Offset: 0x000CAA77
	public override void Update()
	{
		if (this.viewAngleRestoreCounter > 0)
		{
			this.viewAngleRestoreCounter--;
			if (this.viewAngleRestoreCounter == 0)
			{
				this.restoreViewAngle();
			}
		}
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000CC89E File Offset: 0x000CAA9E
	public override bool Continue()
	{
		return (!(this.theEntity.GetRevengeTarget() != null) || !(this.theEntity.GetAttackTarget() != this.theEntity.GetRevengeTarget())) && base.Continue();
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x000CC8D8 File Offset: 0x000CAAD8
	public override void Reset()
	{
		base.Reset();
		this.restoreViewAngle();
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x000CC8E6 File Offset: 0x000CAAE6
	[PublicizedFrom(EAccessModifier.Private)]
	public void restoreViewAngle()
	{
		if (this.viewAngleSave > 0f)
		{
			this.theEntity.SetMaxViewAngle(this.viewAngleSave);
			this.viewAngleSave = 0f;
			this.viewAngleRestoreCounter = 0;
		}
	}

	// Token: 0x0400175D RID: 5981
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAISetAsTargetIfHurt.TargetClass> targetClasses;

	// Token: 0x0400175E RID: 5982
	[PublicizedFrom(EAccessModifier.Private)]
	public float viewAngleSave;

	// Token: 0x0400175F RID: 5983
	[PublicizedFrom(EAccessModifier.Private)]
	public int viewAngleRestoreCounter;

	// Token: 0x02000458 RID: 1112
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TargetClass
	{
		// Token: 0x04001760 RID: 5984
		public Type type;
	}
}
