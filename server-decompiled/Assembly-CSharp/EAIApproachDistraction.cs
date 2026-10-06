using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000430 RID: 1072
[Preserve]
public class EAIApproachDistraction : EAIBase
{
	// Token: 0x060020D0 RID: 8400 RVA: 0x000C6A4D File Offset: 0x000C4C4D
	public EAIApproachDistraction()
	{
		this.MutexBits = 3;
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x000C6A5C File Offset: 0x000C4C5C
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

	// Token: 0x060020D2 RID: 8402 RVA: 0x000C6AF8 File Offset: 0x000C4CF8
	public override void Start()
	{
		this.theEntity.SetAttackTarget(null, 0);
		this.theEntity.IsEating = false;
		this.theEntity.distraction = this.theEntity.pendingDistraction;
		this.theEntity.pendingDistraction = null;
		this.hadPath = false;
		this.updatePath();
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x000C6B50 File Offset: 0x000C4D50
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

	// Token: 0x060020D4 RID: 8404 RVA: 0x000C6BE4 File Offset: 0x000C4DE4
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

	// Token: 0x060020D5 RID: 8405 RVA: 0x000C6CF0 File Offset: 0x000C4EF0
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

	// Token: 0x060020D6 RID: 8406 RVA: 0x000C6D50 File Offset: 0x000C4F50
	public override void Reset()
	{
		this.theEntity.moveHelper.Stop();
		this.theEntity.SetLookPosition(Vector3.zero);
		this.theEntity.IsEating = false;
		this.theEntity.distraction = null;
		this.manager.lookTime = 2f;
	}

	// Token: 0x04001684 RID: 5764
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cCloseDist = 1.5f;

	// Token: 0x04001685 RID: 5765
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLookTime = 2f;

	// Token: 0x04001686 RID: 5766
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadPath;

	// Token: 0x04001687 RID: 5767
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathRecalculateTicks;
}
