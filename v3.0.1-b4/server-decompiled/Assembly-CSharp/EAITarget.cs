using System;
using UnityEngine.Scripting;

// Token: 0x02000455 RID: 1109
[Preserve]
public abstract class EAITarget : EAIBase
{
	// Token: 0x060021CE RID: 8654 RVA: 0x000CCB04 File Offset: 0x000CAD04
	public void Init(EntityAlive _theEntity, float _maxXZDistance, bool _bNeedToSee)
	{
		base.Init(_theEntity);
		this.seeCounter = 0;
		this.maxXZDistance = _maxXZDistance;
		this.bNeedToSee = _bNeedToSee;
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x000CCB22 File Offset: 0x000CAD22
	public override void Start()
	{
		this.seeCounter = 0;
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x000CCB2C File Offset: 0x000CAD2C
	public override bool Continue()
	{
		EntityAlive attackTarget = this.theEntity.GetAttackTarget();
		if (attackTarget == null)
		{
			return false;
		}
		if (!attackTarget.IsAlive())
		{
			return false;
		}
		if (this.maxXZDistance > 0f && this.theEntity.GetDistanceSq(attackTarget) > this.maxXZDistance * this.maxXZDistance)
		{
			return false;
		}
		if (this.bNeedToSee)
		{
			if (!this.theEntity.CanSee(attackTarget))
			{
				int num = this.seeCounter + 1;
				this.seeCounter = num;
				if (num > 600)
				{
					return false;
				}
			}
			else
			{
				this.seeCounter = 0;
			}
		}
		return true;
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000CCBBC File Offset: 0x000CADBC
	public override void Reset()
	{
		this.theEntity.SetAttackTarget(null, 0);
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000CCBCC File Offset: 0x000CADCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool check(EntityAlive _e)
	{
		if (_e == null)
		{
			return false;
		}
		if (_e == this.theEntity)
		{
			return false;
		}
		if (!_e.IsAlive())
		{
			return false;
		}
		if (_e.IsIgnoredByAI())
		{
			return false;
		}
		Vector3i vector3i = World.worldToBlockPos(_e.position);
		if (!this.theEntity.isWithinHomeDistance(vector3i.x, vector3i.y, vector3i.z))
		{
			return false;
		}
		if (this.bNeedToSee && !this.theEntity.CanSee(_e))
		{
			return false;
		}
		EntityPlayer entityPlayer = _e as EntityPlayer;
		return !(entityPlayer != null) || this.theEntity.CanSeeStealth(this.manager.GetSeeDistance(entityPlayer), entityPlayer.Stealth.lightLevel);
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000C6185 File Offset: 0x000C4385
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAITarget()
	{
	}

	// Token: 0x0400175B RID: 5979
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxXZDistance;

	// Token: 0x0400175C RID: 5980
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNeedToSee;

	// Token: 0x0400175D RID: 5981
	[PublicizedFrom(EAccessModifier.Private)]
	public int seeCounter;
}
