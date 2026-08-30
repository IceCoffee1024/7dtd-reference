using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044D RID: 1101
[Preserve]
public class EAIPathTest : EAIBase
{
	// Token: 0x0600219E RID: 8606 RVA: 0x000CB23F File Offset: 0x000C943F
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
		this.banditEntity = _theEntity as EntityBandit;
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000CB25B File Offset: 0x000C945B
	public void SetTargetMove(Vector3 _targetWorld, EntityAlive _targetYaw, bool _canBreakBlocks)
	{
		if (this.banditEntity != null)
		{
			this.targetWorldPosition = _targetWorld;
			this.targetYaw = _targetYaw;
			this.newTarget = true;
			this.cancel = false;
			this.canBreakBlocks = _canBreakBlocks;
		}
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000CB28E File Offset: 0x000C948E
	public void CancelTargetMove()
	{
		this.targetWorldPosition = Vector3.zero;
		this.newTarget = false;
		this.cancel = true;
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000CB2A9 File Offset: 0x000C94A9
	public override bool CanExecute()
	{
		return this.newTarget;
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000CB2B1 File Offset: 0x000C94B1
	public override bool Continue()
	{
		return !this.cancel;
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000CB2BC File Offset: 0x000C94BC
	public override void Update()
	{
		base.Update();
		if (this.newTarget)
		{
			this.newTarget = false;
			PathInfoSingleTarget pathInfoSingleTarget = new PathInfoSingleTarget(this.theEntity, this.targetWorldPosition, this.canBreakBlocks, this.theEntity.GetMoveSpeed(), null);
			PathFinderThread.Instance.FindPath(pathInfoSingleTarget);
			if (this.banditEntity != null)
			{
				this.banditEntity.focusBody.SetFocus(FocusPriority.Highest, new AIFocusBody(this.targetYaw)
				{
					ConditionDistance = new AIFocusConditionDistance(5f, this.targetWorldPosition)
				});
			}
		}
		Vector3 vector = this.targetWorldPosition - Origin.position;
		Debug.DrawLine(vector, vector + Vector3.up * 2f, Color.blue);
		PathEntity path = this.theEntity.navigator.getPath();
		if (path != null)
		{
			for (int i = 0; i < path.getCurrentPathLength() - 1; i++)
			{
				Vector3 vector2 = path.getPathPointFromIndex(i).projectedLocation - Origin.position;
				Color color = new Color(1f, 1f, 0.1f);
				if (i < path.getCurrentPathIndex())
				{
					color = new Color(0.3f, 0.3f, 0f);
				}
				Utils.DrawLine(vector2, path.getPathPointFromIndex(i + 1).projectedLocation - Origin.position, color, color * 0.5f, 3, 0.1f);
			}
		}
	}

	// Token: 0x0400172D RID: 5933
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityBandit banditEntity;

	// Token: 0x0400172E RID: 5934
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetWorldPosition;

	// Token: 0x0400172F RID: 5935
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetYaw;

	// Token: 0x04001730 RID: 5936
	[PublicizedFrom(EAccessModifier.Private)]
	public bool newTarget;

	// Token: 0x04001731 RID: 5937
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancel;

	// Token: 0x04001732 RID: 5938
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canBreakBlocks;
}
