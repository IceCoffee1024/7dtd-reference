using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014BD RID: 5309
public class TrajectorySimulation
{
	// Token: 0x0600A6AD RID: 42669 RVA: 0x003E0440 File Offset: 0x003DE640
	public bool TryGetSolution(out Trajectory.AimTrajectory trajectory)
	{
		if (this.TrajectoryState == TrajectorySimulation.State.DoneWithSolution)
		{
			trajectory = this.SimTrajectories[this.SimTrajectoryIndex];
			return true;
		}
		trajectory = default(Trajectory.AimTrajectory);
		return false;
	}

	// Token: 0x0600A6AE RID: 42670 RVA: 0x003E046C File Offset: 0x003DE66C
	public TrajectorySimulation(EntityAlive owner, EntityAlive target, float maxSimTime, TrajectorySimulation.ProjectileInfo projectile, float renderDuration, Action<TrajectorySimulation> onSimulationComplete)
	{
		this.TrajectoryState = TrajectorySimulation.State.ReadyToSimulate;
		this.Owner = owner;
		this.Target = target;
		this.MaxSimTime = maxSimTime;
		this.Projectile = projectile;
		this.RenderDuration = renderDuration;
		this.LaunchPosition = this.Owner.position;
		this.TargetPosition = this.Target.position;
		this.OnSimulationComplete = onSimulationComplete;
		this.CullPlane = new Plane(Vector3.up, this.TargetPosition - Vector3.up * this.Projectile.ProjectileTargetRadius);
	}

	// Token: 0x0600A6AF RID: 42671 RVA: 0x003E0514 File Offset: 0x003DE714
	public bool IsValid()
	{
		if (this.TrajectoryState == TrajectorySimulation.State.Invalid)
		{
			return false;
		}
		if (!(this.Owner != null) || !(this.Target != null))
		{
			return false;
		}
		Vector3 position = this.Owner.transform.position;
		Vector3 position2 = this.Target.transform.position;
		float sqrMagnitude = (position - this.LaunchPosition).sqrMagnitude;
		float sqrMagnitude2 = (position2 - this.TargetPosition).sqrMagnitude;
		if (sqrMagnitude > 16f)
		{
			this.TrajectoryState = TrajectorySimulation.State.Invalid;
			return false;
		}
		if (sqrMagnitude2 > 16f)
		{
			this.TrajectoryState = TrajectorySimulation.State.Invalid;
			return false;
		}
		return true;
	}

	// Token: 0x0600A6B0 RID: 42672 RVA: 0x003E05B8 File Offset: 0x003DE7B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickSimulation(int maxIterations = 20)
	{
		if (this.Owner == null || this.Target == null)
		{
			this.TrajectoryState = TrajectorySimulation.State.Invalid;
			this.OnSimulationComplete(this);
			return;
		}
		if (this.TrajectoryState == TrajectorySimulation.State.ReadyToSimulate)
		{
			this.LaunchPosition = this.Owner.getHeadPosition();
			this.TargetPosition = this.Target.position;
			this.SimTrajectoryIndex = 0;
			this.SimTrajectoryCount = Trajectory.Calculate(this.LaunchPosition, this.TargetPosition, this.Projectile.ProjectileSpeed, this.Projectile.ProjectileGravity, this.SimTrajectories);
			if (this.SimTrajectoryCount == 0)
			{
				this.TrajectoryState = TrajectorySimulation.State.DoneWithoutSolution;
				this.OnSimulationComplete(this);
				return;
			}
			this.SimTime = 0f;
			this.SimVelocity = this.SimTrajectories[this.SimTrajectoryIndex].LaunchVelocity;
			this.SimCurrentPos = (this.SimLastPos = this.Owner.getHeadPosition());
			this.TrajectoryState = TrajectorySimulation.State.Simulating;
		}
		if (this.SimTrajectoryIndex >= this.SimTrajectoryCount)
		{
			this.TrajectoryState = TrajectorySimulation.State.DoneWithoutSolution;
			this.OnSimulationComplete(this);
			return;
		}
		if (this.TrajectoryState != TrajectorySimulation.State.Simulating)
		{
			return;
		}
		World world = GameManager.Instance.World;
		while (this.SimTime < this.MaxSimTime)
		{
			if (maxIterations <= 0)
			{
				return;
			}
			if (this.CullPlane.GetDistanceToPoint(this.SimCurrentPos) < 0f)
			{
				break;
			}
			if (this.SimVelocity.magnitude < 1f)
			{
				if (this.RenderDuration > 0f)
				{
					Utils.DrawBounds(new Bounds(this.SimCurrentPos, Vector3.one * this.Projectile.ProjectileRadius), Color.magenta, this.RenderDuration);
					break;
				}
				break;
			}
			else
			{
				Vector3 vector = this.SimVelocity * 0.05f;
				this.SimCurrentPos += vector;
				this.SimVelocity.y = this.SimVelocity.y + this.Projectile.ProjectileGravity * 0.05f;
				if (this.RenderDuration > 0f)
				{
					Debug.DrawLine(this.SimLastPos, this.SimCurrentPos, Color.green, this.RenderDuration);
				}
				float num;
				Vector3 vector2 = vector.NormalizeReturnMagnitude(out num);
				Ray ray = new Ray(this.SimCurrentPos, vector2);
				if (Voxel.Raycast(world, ray, num, 1082195968, 8191, this.Projectile.ProjectileRadius))
				{
					if (Voxel.phyxRaycastHit.distance == 0f)
					{
						if (this.RenderDuration > 0f)
						{
							Utils.DrawBounds(new Bounds(Voxel.phyxRaycastHit.point, Vector3.one * this.Projectile.ProjectileRadius), Color.red, this.RenderDuration);
							break;
						}
						break;
					}
					else
					{
						this.SimCurrentPos += ray.GetPoint(Voxel.phyxRaycastHit.distance);
						this.SimVelocity = Vector3.Reflect(this.SimVelocity, Voxel.phyxRaycastHit.normal);
						this.SimVelocity *= 0.25f;
						float num2 = num - Voxel.phyxRaycastHit.distance;
						if (num2 > 0f)
						{
							this.SimCurrentPos = Voxel.phyxRaycastHit.point + Vector3.Reflect(vector2, Voxel.phyxRaycastHit.normal) * num2;
						}
						if (this.RenderDuration > 0f)
						{
							Debug.DrawRay(Voxel.phyxRaycastHit.point, Voxel.phyxRaycastHit.normal * 0.2f, Color.yellow, this.RenderDuration);
						}
						if (Vector3.Dot(Voxel.phyxRaycastHit.normal, Vector3.up) > 0.707f)
						{
							break;
						}
					}
				}
				this.SimLastPos = this.SimCurrentPos;
				this.SimTime += 0.05f;
				maxIterations--;
			}
		}
		if (MathUtils.DistanceToSegment(this.SimCurrentPos, this.TargetPosition, this.Target.getChestPosition()) < this.Projectile.ProjectileTargetRadius)
		{
			this.TrajectoryState = TrajectorySimulation.State.DoneWithSolution;
			if (this.RenderDuration > 0f)
			{
				Utils.DrawBounds(new Bounds(this.SimCurrentPos, new Vector3(1f, 0f, 1f) * this.Projectile.ProjectileRadius), Color.blue, this.RenderDuration);
			}
			this.OnSimulationComplete(this);
			return;
		}
		this.SimTrajectoryIndex++;
		if (this.SimTrajectoryIndex < this.SimTrajectoryCount)
		{
			this.SimTime = 0f;
			this.SimVelocity = this.SimTrajectories[this.SimTrajectoryIndex].LaunchVelocity;
			this.SimCurrentPos = (this.SimLastPos = this.Owner.getHeadPosition());
			this.TrajectoryState = TrajectorySimulation.State.Simulating;
			return;
		}
		this.TrajectoryState = TrajectorySimulation.State.DoneWithoutSolution;
		this.OnSimulationComplete(this);
	}

	// Token: 0x0600A6B1 RID: 42673 RVA: 0x003E0A9F File Offset: 0x003DEC9F
	public static void Cleanup()
	{
		TrajectorySimulation.Trajectories.Clear();
	}

	// Token: 0x0600A6B2 RID: 42674 RVA: 0x003E0AAB File Offset: 0x003DECAB
	public static void Queue(TrajectorySimulation trajectory)
	{
		if (!TrajectorySimulation.Trajectories.Contains(trajectory))
		{
			TrajectorySimulation.Trajectories.Enqueue(trajectory);
		}
		trajectory.TrajectoryState = TrajectorySimulation.State.ReadyToSimulate;
	}

	// Token: 0x0600A6B3 RID: 42675 RVA: 0x003E0ACC File Offset: 0x003DECCC
	public static void UpdateSimulationQueue()
	{
		TrajectorySimulation trajectorySimulation;
		if (TrajectorySimulation.Trajectories.TryPeek(out trajectorySimulation))
		{
			trajectorySimulation.TickSimulation(20);
			if (trajectorySimulation.TrajectoryState != TrajectorySimulation.State.Simulating)
			{
				TrajectorySimulation.Trajectories.Dequeue();
			}
		}
	}

	// Token: 0x04007B68 RID: 31592
	public TrajectorySimulation.State TrajectoryState;

	// Token: 0x04007B69 RID: 31593
	public readonly EntityAlive Owner;

	// Token: 0x04007B6A RID: 31594
	public readonly EntityAlive Target;

	// Token: 0x04007B6B RID: 31595
	public readonly float MaxSimTime;

	// Token: 0x04007B6C RID: 31596
	public readonly TrajectorySimulation.ProjectileInfo Projectile;

	// Token: 0x04007B6D RID: 31597
	public readonly float RenderDuration;

	// Token: 0x04007B6E RID: 31598
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<TrajectorySimulation> OnSimulationComplete;

	// Token: 0x04007B6F RID: 31599
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 LaunchPosition;

	// Token: 0x04007B70 RID: 31600
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 TargetPosition;

	// Token: 0x04007B71 RID: 31601
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Trajectory.AimTrajectory[] SimTrajectories = new Trajectory.AimTrajectory[2];

	// Token: 0x04007B72 RID: 31602
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryIndex;

	// Token: 0x04007B73 RID: 31603
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryCount;

	// Token: 0x04007B74 RID: 31604
	[PublicizedFrom(EAccessModifier.Private)]
	public float SimTime;

	// Token: 0x04007B75 RID: 31605
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimVelocity;

	// Token: 0x04007B76 RID: 31606
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimLastPos;

	// Token: 0x04007B77 RID: 31607
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimCurrentPos;

	// Token: 0x04007B78 RID: 31608
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane CullPlane;

	// Token: 0x04007B79 RID: 31609
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxOwnerDelta = 16f;

	// Token: 0x04007B7A RID: 31610
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxTargetDelta = 16f;

	// Token: 0x04007B7B RID: 31611
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSimInterval = 0.05f;

	// Token: 0x04007B7C RID: 31612
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Queue<TrajectorySimulation> Trajectories = new Queue<TrajectorySimulation>();

	// Token: 0x020014BE RID: 5310
	public enum State
	{
		// Token: 0x04007B7E RID: 31614
		Init,
		// Token: 0x04007B7F RID: 31615
		Invalid,
		// Token: 0x04007B80 RID: 31616
		ReadyToSimulate,
		// Token: 0x04007B81 RID: 31617
		Simulating,
		// Token: 0x04007B82 RID: 31618
		DoneWithSolution,
		// Token: 0x04007B83 RID: 31619
		DoneWithoutSolution
	}

	// Token: 0x020014BF RID: 5311
	public struct ProjectileInfo
	{
		// Token: 0x04007B84 RID: 31620
		public float ProjectileSpeed;

		// Token: 0x04007B85 RID: 31621
		public float ProjectileGravity;

		// Token: 0x04007B86 RID: 31622
		public float ProjectileRadius;

		// Token: 0x04007B87 RID: 31623
		public float ProjectileTargetRadius;
	}
}
