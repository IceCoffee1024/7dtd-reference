using System;
using UnityEngine.Scripting;

// Token: 0x0200045D RID: 1117
[Preserve]
public abstract class EAITarget : EAIBase
{
	// Token: 0x060021DF RID: 8671 RVA: 0x000CD700 File Offset: 0x000CB900
	public void Init(EntityAlive _theEntity, float _maxXZDistance, bool _bNeedToSee)
	{
		base.Init(_theEntity);
		this.seeCounter = 0;
		this.maxXZDistance = _maxXZDistance;
		this.bNeedToSee = _bNeedToSee;
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000CD71E File Offset: 0x000CB91E
	public override void Start()
	{
		this.seeCounter = 0;
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000CD728 File Offset: 0x000CB928
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

	// Token: 0x060021E2 RID: 8674 RVA: 0x000CD7B8 File Offset: 0x000CB9B8
	public override void Reset()
	{
		this.theEntity.SetAttackTarget(null, 0);
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000CD7C8 File Offset: 0x000CB9C8
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

	// Token: 0x060021E4 RID: 8676 RVA: 0x000C6DF1 File Offset: 0x000C4FF1
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAITarget()
	{
	}

	// Token: 0x04001778 RID: 6008
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxXZDistance;

	// Token: 0x04001779 RID: 6009
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNeedToSee;

	// Token: 0x0400177A RID: 6010
	[PublicizedFrom(EAccessModifier.Private)]
	public int seeCounter;
}
