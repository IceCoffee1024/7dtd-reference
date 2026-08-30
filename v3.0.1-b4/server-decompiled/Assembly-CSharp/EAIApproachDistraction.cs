using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200042A RID: 1066
[Preserve]
public class EAIApproachDistraction : EAIBase
{
	// Token: 0x060020C4 RID: 8388 RVA: 0x000C618D File Offset: 0x000C438D
	public EAIApproachDistraction()
	{
		this.MutexBits = 3;
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x000C619C File Offset: 0x000C439C
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

	// Token: 0x060020C6 RID: 8390 RVA: 0x000C6238 File Offset: 0x000C4438
	public override void Start()
	{
		this.theEntity.SetAttackTarget(null, 0);
		this.theEntity.IsEating = false;
		this.theEntity.distraction = this.theEntity.pendingDistraction;
		this.theEntity.pendingDistraction = null;
		this.hadPath = false;
		this.updatePath();
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x000C6290 File Offset: 0x000C4490
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

	// Token: 0x060020C8 RID: 8392 RVA: 0x000C6324 File Offset: 0x000C4524
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

	// Token: 0x060020C9 RID: 8393 RVA: 0x000C6430 File Offset: 0x000C4630
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

	// Token: 0x060020CA RID: 8394 RVA: 0x000C6490 File Offset: 0x000C4690
	public override void Reset()
	{
		this.theEntity.moveHelper.Stop();
		this.theEntity.SetLookPosition(Vector3.zero);
		this.theEntity.IsEating = false;
		this.theEntity.distraction = null;
		this.manager.lookTime = 2f;
	}

	// Token: 0x04001678 RID: 5752
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCloseDist = 1.5f;

	// Token: 0x04001679 RID: 5753
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTime = 2f;

	// Token: 0x0400167A RID: 5754
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadPath;

	// Token: 0x0400167B RID: 5755
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathRecalculateTicks;
}
