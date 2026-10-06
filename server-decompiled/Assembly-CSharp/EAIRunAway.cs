using System;
using System.Collections.Generic;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000452 RID: 1106
[Preserve]
public abstract class EAIRunAway : EAIBase
{
	// Token: 0x0600219E RID: 8606 RVA: 0x000CBACF File Offset: 0x000C9CCF
	public EAIRunAway()
	{
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000CBADF File Offset: 0x000C9CDF
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "fleeDistance", ref this.fleeDistance);
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000CBAFA File Offset: 0x000C9CFA
	public override bool CanExecute()
	{
		return this.FindFleePos(this.GetFleeFromPos());
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000CBB08 File Offset: 0x000C9D08
	public override void Start()
	{
		this.timeoutTicks = (30 + base.GetRandom(20)) * 20;
		PathFinderThread.Instance.RemovePathsFor(this.theEntity.entityId);
		string soundAlert = this.theEntity.GetSoundAlert();
		if (soundAlert != null)
		{
			this.theEntity.PlayOneShot(soundAlert, false, false, false, null, 1f);
		}
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000CBB62 File Offset: 0x000C9D62
	public override bool Continue()
	{
		return this.timeoutTicks > 0;
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000CBB70 File Offset: 0x000C9D70
	public override void Update()
	{
		this.timeoutTicks--;
		PathEntity path = this.theEntity.navigator.getPath();
		if (!this.checkedPath && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
		{
			this.checkedPath = true;
			if (path != null)
			{
				Vector3 rawEndPos = path.rawEndPos;
				if (this.theEntity.GetDistanceSq(rawEndPos) < 1.21f)
				{
					this.FindRandomPos();
				}
			}
		}
		if (this.checkedPath && path != null && path.getCurrentPathLength() >= 2 && path.NodeCountRemaining() <= 2)
		{
			this.fleeTicks = 0;
		}
		int num = this.fleeTicks - 1;
		this.fleeTicks = num;
		if (num <= 0)
		{
			Vector3 fleeFromPos = this.GetFleeFromPos();
			this.FindFleePos(fleeFromPos);
		}
		num = this.pathTicks - 1;
		this.pathTicks = num;
		if (num <= 0 && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId))
		{
			this.pathTicks = 60;
			this.theEntity.FindPath(this.targetPos, this.theEntity.GetMoveSpeed(), false, this);
			this.checkedPath = false;
		}
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x000CBC85 File Offset: 0x000C9E85
	public override void Reset()
	{
		this.enemy = null;
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000CBC90 File Offset: 0x000C9E90
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool FindFleePos(Vector3 fleeFromPos)
	{
		Vector3 vector = this.theEntity.position - fleeFromPos;
		Vector3 vector2 = RandomPositionGenerator.CalcPositionInDirection(this.theEntity, this.theEntity.position, vector, (float)this.fleeDistance, 80f);
		if (vector2.y == 0f)
		{
			return false;
		}
		this.targetPos = vector2;
		this.fleeTicks = 80;
		this.pathTicks = 0;
		this.checkedPath = false;
		return true;
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000CBD00 File Offset: 0x000C9F00
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool FindRandomPos()
	{
		Vector3 vector = RandomPositionGenerator.CalcAround(this.theEntity, this.fleeDistance, 0);
		if (vector.y == 0f)
		{
			this.fleeTicks = 0;
			return false;
		}
		this.targetPos = vector;
		this.fleeTicks = 80;
		this.pathTicks = 0;
		this.checkedPath = false;
		return true;
	}

	// Token: 0x060021A7 RID: 8615
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract Vector3 GetFleeFromPos();

	// Token: 0x060021A8 RID: 8616 RVA: 0x000CBD54 File Offset: 0x000C9F54
	public override string ToString()
	{
		return string.Format("{0}, flee {1}, timeout {2}, {3}", new object[]
		{
			base.ToString(),
			this.fleeTicks,
			this.timeoutTicks,
			(this.enemy != null) ? this.enemy.GetDebugName() : ""
		});
	}

	// Token: 0x0400173F RID: 5951
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive enemy;

	// Token: 0x04001740 RID: 5952
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetPos;

	// Token: 0x04001741 RID: 5953
	[PublicizedFrom(EAccessModifier.Private)]
	public int timeoutTicks;

	// Token: 0x04001742 RID: 5954
	[PublicizedFrom(EAccessModifier.Private)]
	public int fleeTicks;

	// Token: 0x04001743 RID: 5955
	[PublicizedFrom(EAccessModifier.Private)]
	public int pathTicks;

	// Token: 0x04001744 RID: 5956
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkedPath;

	// Token: 0x04001745 RID: 5957
	[PublicizedFrom(EAccessModifier.Private)]
	public int fleeDistance = 20;
}
