using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000432 RID: 1074
[Preserve]
public class EAIApproachDistraction : EAIBase
{
	// Token: 0x060020D5 RID: 8405 RVA: 0x000C6DF9 File Offset: 0x000C4FF9
	public EAIApproachDistraction()
	{
		this.MutexBits = 3;
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x000C6E08 File Offset: 0x000C5008
	public override bool CanExecute()
	{
		EntityItem pendingDistraction = this.theEntity.pendingDistraction;
		if (!pendingDistraction || pendingDistraction.itemClass == null)
		{
			return false;
		}
		if (this.theEntity.GetAttackTarget())
		{
			if (!pendingDistraction.itemClass.IsEatDistraction)
			{
				this.theEntity.pendingDistraction = null;
			}
			return false;
		}
		if ((this.theEntity.position - pendingDistraction.position).sqrMagnitude < 2.25f && !pendingDistraction.itemClass.IsEatDistraction)
		{
			this.theEntity.pendingDistraction = null;
			return false;
		}
		return true;
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x000C6EA4 File Offset: 0x000C50A4
	public override void Start()
	{
		this.theEntity.SetAttackTarget(null, 0);
		this.theEntity.IsEating = false;
		this.theEntity.distraction = this.theEntity.pendingDistraction;
		this.theEntity.pendingDistraction = null;
		this.hadPath = false;
		this.updatePath();
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x000C6EFC File Offset: 0x000C50FC
	public override bool Continue()
	{
		PathEntity path = this.theEntity.navigator.getPath();
		if (this.hadPath && path == null)
		{
			return false;
		}
		EntityItem distraction = this.theEntity.distraction;
		return !(distraction == null) && distraction.itemClass != null && (((this.theEntity.position - distraction.position).sqrMagnitude > 2.25f && (path == null || !path.isFinished())) || (distraction.itemClass.IsEatDistraction && distraction.IsDistractionActive));
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x000C6F90 File Offset: 0x000C5190
	public override void Update()
	{
		EntityItem distraction = this.theEntity.distraction;
		if (!distraction)
		{
			return;
		}
		PathEntity path = this.theEntity.getNavigator().getPath();
		if (path != null)
		{
			this.hadPath = true;
		}
		bool flag = false;
		if (path != null && !path.isFinished() && !this.theEntity.isCollidedHorizontally)
		{
			flag = true;
		}
		if (this.theEntity.IsSwimming())
		{
			flag = true;
		}
		if (Mathf.Abs(this.theEntity.speedForward) > 0.01f || Mathf.Abs(this.theEntity.speedStrafe) > 0.01f)
		{
			flag = true;
		}
		if (flag)
		{
			this.theEntity.SetLookPosition(distraction.position);
		}
		if ((this.theEntity.GetPosition() - distraction.position).sqrMagnitude <= 2.25f)
		{
			this.theEntity.IsEating = true;
			distraction.distractionEatTicks--;
			return;
		}
		int num = this.pathRecalculateTicks - 1;
		this.pathRecalculateTicks = num;
		if (num <= 0)
		{
			this.updatePath();
		}
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x000C709C File Offset: 0x000C529C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePath()
	{
		if (PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
		{
			return;
		}
		this.pathRecalculateTicks = 20 + base.GetRandom(20);
		this.theEntity.FindPath(this.theEntity.distraction.position, this.theEntity.GetMoveSpeedAggro(), true, this);
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000C70FC File Offset: 0x000C52FC
	public override void Reset()
	{
		this.theEntity.moveHelper.Stop();
		this.theEntity.SetLookPosition(Vector3.zero);
		this.theEntity.IsEating = false;
		this.theEntity.distraction = null;
		this.manager.lookTime = 2f;
	}

	// Token: 0x04001694 RID: 5780
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCloseDist = 1.5f;

	// Token: 0x04001695 RID: 5781
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTime = 2f;

	// Token: 0x04001696 RID: 5782
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadPath;

	// Token: 0x04001697 RID: 5783
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathRecalculateTicks;
}
