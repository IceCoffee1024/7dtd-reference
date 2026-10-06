using System;
using UnityEngine.Scripting;

// Token: 0x0200045B RID: 1115
[Preserve]
public abstract class EAITarget : EAIBase
{
	// Token: 0x060021DA RID: 8666 RVA: 0x000CD354 File Offset: 0x000CB554
	public void Init(EntityAlive _theEntity, float _maxXZDistance, bool _bNeedToSee)
	{
		base.Init(_theEntity);
		this.seeCounter = 0;
		this.maxXZDistance = _maxXZDistance;
		this.bNeedToSee = _bNeedToSee;
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000CD372 File Offset: 0x000CB572
	public override void Start()
	{
		this.seeCounter = 0;
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000CD37C File Offset: 0x000CB57C
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

	// Token: 0x060021DD RID: 8669 RVA: 0x000CD40C File Offset: 0x000CB60C
	public override void Reset()
	{
		this.theEntity.SetAttackTarget(null, 0);
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000CD41C File Offset: 0x000CB61C
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

	// Token: 0x060021DF RID: 8671 RVA: 0x000C6A45 File Offset: 0x000C4C45
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAITarget()
	{
	}

	// Token: 0x04001768 RID: 5992
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxXZDistance;

	// Token: 0x04001769 RID: 5993
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNeedToSee;

	// Token: 0x0400176A RID: 5994
	[PublicizedFrom(EAccessModifier.Private)]
	public int seeCounter;
}
