using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000455 RID: 1109
[Preserve]
public class EAISetAsTargetIfHurt : EAITarget
{
	// Token: 0x060021B9 RID: 8633 RVA: 0x000CC20B File Offset: 0x000CA40B
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity, 0f, false);
		this.MutexBits = 1;
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000CC224 File Offset: 0x000CA424
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

	// Token: 0x060021BB RID: 8635 RVA: 0x000CC290 File Offset: 0x000CA490
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

	// Token: 0x060021BC RID: 8636 RVA: 0x000CC474 File Offset: 0x000CA674
	public override void Start()
	{
		this.theEntity.SetAttackTarget(this.theEntity.GetRevengeTarget(), 400);
		this.viewAngleSave = this.theEntity.GetMaxViewAngle();
		this.theEntity.SetMaxViewAngle(270f);
		this.viewAngleRestoreCounter = 100;
		base.Start();
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x000CC4CB File Offset: 0x000CA6CB
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

	// Token: 0x060021BE RID: 8638 RVA: 0x000CC4F2 File Offset: 0x000CA6F2
	public override bool Continue()
	{
		return (!(this.theEntity.GetRevengeTarget() != null) || !(this.theEntity.GetAttackTarget() != this.theEntity.GetRevengeTarget())) && base.Continue();
	}

	// Token: 0x060021BF RID: 8639 RVA: 0x000CC52C File Offset: 0x000CA72C
	public override void Reset()
	{
		base.Reset();
		this.restoreViewAngle();
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x000CC53A File Offset: 0x000CA73A
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

	// Token: 0x0400174D RID: 5965
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAISetAsTargetIfHurt.TargetClass> targetClasses;

	// Token: 0x0400174E RID: 5966
	[PublicizedFrom(EAccessModifier.Private)]
	public float viewAngleSave;

	// Token: 0x0400174F RID: 5967
	[PublicizedFrom(EAccessModifier.Private)]
	public int viewAngleRestoreCounter;

	// Token: 0x02000456 RID: 1110
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TargetClass
	{
		// Token: 0x04001750 RID: 5968
		public Type type;
	}
}
