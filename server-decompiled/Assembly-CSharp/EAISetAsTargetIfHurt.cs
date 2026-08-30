using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044F RID: 1103
[Preserve]
public class EAISetAsTargetIfHurt : EAITarget
{
	// Token: 0x060021AD RID: 8621 RVA: 0x000CB9BB File Offset: 0x000C9BBB
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity, 0f, false);
		this.MutexBits = 1;
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x000CB9D4 File Offset: 0x000C9BD4
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

	// Token: 0x060021AF RID: 8623 RVA: 0x000CBA40 File Offset: 0x000C9C40
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

	// Token: 0x060021B0 RID: 8624 RVA: 0x000CBC24 File Offset: 0x000C9E24
	public override void Start()
	{
		this.theEntity.SetAttackTarget(this.theEntity.GetRevengeTarget(), 400);
		this.viewAngleSave = this.theEntity.GetMaxViewAngle();
		this.theEntity.SetMaxViewAngle(270f);
		this.viewAngleRestoreCounter = 100;
		base.Start();
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x000CBC7B File Offset: 0x000C9E7B
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

	// Token: 0x060021B2 RID: 8626 RVA: 0x000CBCA2 File Offset: 0x000C9EA2
	public override bool Continue()
	{
		return (!(this.theEntity.GetRevengeTarget() != null) || !(this.theEntity.GetAttackTarget() != this.theEntity.GetRevengeTarget())) && base.Continue();
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x000CBCDC File Offset: 0x000C9EDC
	public override void Reset()
	{
		base.Reset();
		this.restoreViewAngle();
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x000CBCEA File Offset: 0x000C9EEA
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

	// Token: 0x04001740 RID: 5952
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAISetAsTargetIfHurt.TargetClass> targetClasses;

	// Token: 0x04001741 RID: 5953
	[PublicizedFrom(EAccessModifier.Private)]
	public float viewAngleSave;

	// Token: 0x04001742 RID: 5954
	[PublicizedFrom(EAccessModifier.Private)]
	public int viewAngleRestoreCounter;

	// Token: 0x02000450 RID: 1104
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TargetClass
	{
		// Token: 0x04001743 RID: 5955
		public Type type;
	}
}
