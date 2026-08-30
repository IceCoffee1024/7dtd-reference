using System;
using System.Collections.Generic;
using GamePath;
using RaycastPathing;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class EAIDroneItemTask : EAIItemTask
{
	// Token: 0x06002125 RID: 8485 RVA: 0x000C7FF7 File Offset: 0x000C61F7
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.drone = _theEntity as EntityDrone;
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x000C800C File Offset: 0x000C620C
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "enterTime", ref this.attackEnterTime);
		base.GetData(data, "actionTime", ref this.actionTime);
		base.GetData(data, "exitTime", ref this.attackExitTime);
		this.attackEnterTimer = this.attackEnterTime;
		base.GetData(data, "fleeDist", ref this.fleeDist);
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000C8074 File Offset: 0x000C6274
	public override bool CanExecute()
	{
		return this.drone && PathFinderThread.Instance != null && base.CanExecute();
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000C8094 File Offset: 0x000C6294
	public override bool Continue()
	{
		return this.drone && base.Continue();
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000C80AC File Offset: 0x000C62AC
	public override void Update()
	{
		if (!this.drone)
		{
			return;
		}
		base.Update();
		if (this.attackEnterTimer > 0f)
		{
			this.attackEnterTimer -= 0.05f;
			if (this.attackEnterTimer <= 0f)
			{
				this.OnAttackEnter();
			}
		}
		if (this.actionTimer > 0f)
		{
			this.actionTimer -= 0.05f;
			if (this.actionTimer <= 0f)
			{
				this.OnAttackAction();
			}
		}
		if (this.attackExitTimer > 0f)
		{
			this.attackExitTimer -= 0.05f;
			if (this.attackExitTimer <= 0f)
			{
				this.OnAttackExit();
			}
		}
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x000C8163 File Offset: 0x000C6363
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x000C816B File Offset: 0x000C636B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAttackEnter()
	{
		this.actionTimer = this.actionTime;
		this.drone.GetInstalledWeapon(this.ItemKey).PlayBark();
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x000C818F File Offset: 0x000C638F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAttackAction()
	{
		this.attackExitTimer = this.attackExitTime;
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000C81A0 File Offset: 0x000C63A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAttackExit()
	{
		this.attackEnterTimer = this.attackEnterTime;
		this.actionTimer = 0f;
		this.attackExitTimer = 0f;
		this.drone.SetRevengeTarget(null);
		this.drone.SetAttackTarget(null, 0);
		this.weaponDischarged = false;
		this.ClearPath();
		this.drone.SetState((this.drone.OrderState == EntityDrone.Orders.Stay) ? EntityDrone.State.Sentry : EntityDrone.State.Idle, true);
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000C8214 File Offset: 0x000C6414
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool GetPath(Vector3 seekPos, float seekDist, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		return EntityDrone.GetPath(this.currentPath, this.drone, this.drone.position, seekPos, this.drone.SpeedFlying, this, seekDist, pointRadius, debugDraw, duration);
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x000C8255 File Offset: 0x000C6455
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] GetGroupPositions(EntityAlive target, float dist, bool debugDraw = false, float duration = 0f)
	{
		return EntityDrone.GetGroupPositions(target, dist, debugDraw, duration);
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x000C8261 File Offset: 0x000C6461
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsPositionBlocked(Vector3 start, Vector3 end, int layerMask = 1073807360, bool debugDraw = false, float duration = 0f)
	{
		return EntityDrone.IsPositionBlocked(start, end, layerMask, debugDraw, 0f);
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000C8272 File Offset: 0x000C6472
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ClearPath()
	{
		this.currentPath.Clear();
		this.drone.OnPathInterupted();
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000C828C File Offset: 0x000C648C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FollowPlannedPath(float speed, float pointRadius = 0.1f, bool debugDraw = false, float duration = 0f)
	{
		if (this.currentPath.Count > 0)
		{
			this.currentPathTarget = this.currentPath[0];
			this.drone.RotateTo((this.currentPathTarget - this.drone.position).normalized);
			this.drone.Move(this.currentPathTarget, pointRadius);
			if (this.drone.IsInRange(this.currentPathTarget, pointRadius))
			{
				this.currentPath.RemoveAt(0);
				return;
			}
			if (debugDraw && this.currentPath.Count > 1)
			{
				RaycastPathUtils.DrawLine(this.currentPath[0], this.currentPath[1], Color.green, 1f);
			}
			if (this.pathTracker.IsStuck(this.drone.position, this.currentPath[0], 0.5f, false))
			{
				if (this.currentPath.Count > 1)
				{
					this.drone.TeleportToPosition(this.currentPath[1]);
					this.currentPath.RemoveRange(0, 2);
					return;
				}
				this.drone.TeleportToPosition(this.currentPath[0]);
				this.currentPath.RemoveAt(0);
				return;
			}
		}
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000C83D4 File Offset: 0x000C65D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool DoMoveIntoAtkPos(EntityAlive avaliableTarget, float seekDist, Vector3 seekForward, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		Vector3 position = this.drone.position;
		Vector3 chestPosition = avaliableTarget.getChestPosition();
		Vector3 normalized = (chestPosition - position).normalized;
		bool flag = Vector3.Dot(seekForward, normalized) >= 0.9f;
		float magnitude = (chestPosition - position).magnitude;
		Utils.DrawCircleLinesHorzontal(position - Origin.position, seekDist, Color.white, Color.red, 24, 0.05f);
		if (this.currentPath.Count == 0)
		{
			Vector3[] groupPositions = this.GetGroupPositions(avaliableTarget, 1.414f, false, 0f);
			Array.Sort<Vector3>(groupPositions, (Vector3 x, Vector3 y) => Vector3.Distance(position, x).CompareTo(Vector3.Distance(position, y)));
			Vector3 vector = groupPositions[0];
			this.GetPath(vector, seekDist, pointRadius, debugDraw, duration);
			if (this.currentPath.Count > 0)
			{
				this.currentPathDest = this.currentPath[this.currentPath.Count - 1];
			}
			else
			{
				this.currentPathDest = vector;
			}
		}
		else
		{
			if ((chestPosition - this.currentPathDest).magnitude > seekDist)
			{
				this.ClearPath();
			}
			if (this.IsPositionBlocked(position, chestPosition, 1073807360, false, 0f) || (float)this.currentPath.Count > seekDist + 1f || (this.currentPath.Count > 0 && magnitude > seekDist + 1.414f))
			{
				this.FollowPlannedPath(this.drone.SpeedFlying, pointRadius, false, 0f);
			}
			else if (magnitude >= seekDist)
			{
				this.drone.RotateTo(normalized);
				this.drone.Move(chestPosition, pointRadius);
			}
		}
		if (!this.IsPositionBlocked(position, chestPosition, 1073807360, false, 0f) && magnitude <= seekDist)
		{
			this.drone.RotateTo(normalized);
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000C85C0 File Offset: 0x000C67C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool DoFleeFromTargetEntity(EntityAlive avaliableTarget, float seekDist, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		Vector3 position = this.drone.position;
		avaliableTarget.getChestPosition();
		if (this.currentPath.Count == 0)
		{
			Vector3[] groupPositions = this.GetGroupPositions(avaliableTarget, (this.fleeDist > 0f) ? this.fleeDist : 10f, false, 0f);
			Vector3 vector = groupPositions[this.drone.rand.RandomRange(0, groupPositions.Length)];
			this.GetPath(vector, seekDist, pointRadius, debugDraw, duration);
			if (this.currentPath.Count > 0)
			{
				this.currentPathDest = this.currentPath[this.currentPath.Count - 1];
			}
			else
			{
				this.currentPathDest = vector;
			}
		}
		else
		{
			bool flag = !EntityDrone.IsPositionBlocked(position, this.currentPathDest, 1073807360, false, 0f);
			float magnitude = (this.currentPathDest - position).magnitude;
			if (magnitude > 5f || !flag)
			{
				this.FollowPlannedPath(this.drone.SpeedFlying, pointRadius, false, 0f);
			}
			else
			{
				this.drone.RotateTo((this.currentPathDest - position).normalized);
				this.drone.Move(this.currentPathDest, pointRadius);
			}
			if (magnitude <= seekDist)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040016C1 RID: 5825
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityDrone drone;

	// Token: 0x040016C2 RID: 5826
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<Vector3> currentPath = new List<Vector3>();

	// Token: 0x040016C3 RID: 5827
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 currentPathTarget;

	// Token: 0x040016C4 RID: 5828
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 currentPathDest;

	// Token: 0x040016C5 RID: 5829
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool weaponDischarged;

	// Token: 0x040016C6 RID: 5830
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackEnterTime;

	// Token: 0x040016C7 RID: 5831
	[PublicizedFrom(EAccessModifier.Protected)]
	public float actionTime;

	// Token: 0x040016C8 RID: 5832
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackExitTime;

	// Token: 0x040016C9 RID: 5833
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackEnterTimer;

	// Token: 0x040016CA RID: 5834
	[PublicizedFrom(EAccessModifier.Protected)]
	public float actionTimer;

	// Token: 0x040016CB RID: 5835
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackExitTimer;

	// Token: 0x040016CC RID: 5836
	[PublicizedFrom(EAccessModifier.Private)]
	public float fleeDist;

	// Token: 0x040016CD RID: 5837
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone.PathTracker pathTracker = new EntityDrone.PathTracker();
}
