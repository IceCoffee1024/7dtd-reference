using System;
using UnityEngine.Scripting;

// Token: 0x02000459 RID: 1113
[Preserve]
public abstract class EAITarget : EAIBase
{
	// Token: 0x060021EB RID: 8683 RVA: 0x000CD2A0 File Offset: 0x000CB4A0
	public void Init(EntityAlive _theEntity, float _maxXZDistance, bool _bNeedToSee)
	{
		base.Init(_theEntity);
		this.seeCounter = 0;
		this.maxXZDistance = _maxXZDistance;
		this.bNeedToSee = _bNeedToSee;
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000CD2BE File Offset: 0x000CB4BE
	public override void Start()
	{
		this.seeCounter = 0;
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000CD2C8 File Offset: 0x000CB4C8
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

	// Token: 0x060021EE RID: 8686 RVA: 0x000CD358 File Offset: 0x000CB558
	public override void Reset()
	{
		this.theEntity.SetAttackTarget(null, 0);
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000CD368 File Offset: 0x000CB568
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

	// Token: 0x060021F0 RID: 8688 RVA: 0x000C6991 File Offset: 0x000C4B91
	[PublicizedFrom(EAccessModifier.Protected)]
	public EAITarget()
	{
	}

	// Token: 0x04001771 RID: 6001
	[PublicizedFrom(EAccessModifier.Protected)]
	public float maxXZDistance;

	// Token: 0x04001772 RID: 6002
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNeedToSee;

	// Token: 0x04001773 RID: 6003
	[PublicizedFrom(EAccessModifier.Private)]
	public int seeCounter;
}
