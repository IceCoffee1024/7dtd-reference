using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014D8 RID: 5336
public class TrajectorySimulation
{
	// Token: 0x0600A7AA RID: 42922 RVA: 0x003E5780 File Offset: 0x003E3980
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

	// Token: 0x0600A7AB RID: 42923 RVA: 0x003E57AC File Offset: 0x003E39AC
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

	// Token: 0x0600A7AC RID: 42924 RVA: 0x003E5854 File Offset: 0x003E3A54
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

	// Token: 0x0600A7AD RID: 42925 RVA: 0x003E58F8 File Offset: 0x003E3AF8
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

	// Token: 0x0600A7AE RID: 42926 RVA: 0x003E5DDF File Offset: 0x003E3FDF
	public static void Cleanup()
	{
		TrajectorySimulation.Trajectories.Clear();
	}

	// Token: 0x0600A7AF RID: 42927 RVA: 0x003E5DEB File Offset: 0x003E3FEB
	public static void Queue(TrajectorySimulation trajectory)
	{
		if (!TrajectorySimulation.Trajectories.Contains(trajectory))
		{
			TrajectorySimulation.Trajectories.Enqueue(trajectory);
		}
		trajectory.TrajectoryState = TrajectorySimulation.State.ReadyToSimulate;
	}

	// Token: 0x0600A7B0 RID: 42928 RVA: 0x003E5E0C File Offset: 0x003E400C
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

	// Token: 0x04007C41 RID: 31809
	public TrajectorySimulation.State TrajectoryState;

	// Token: 0x04007C42 RID: 31810
	public readonly EntityAlive Owner;

	// Token: 0x04007C43 RID: 31811
	public readonly EntityAlive Target;

	// Token: 0x04007C44 RID: 31812
	public readonly float MaxSimTime;

	// Token: 0x04007C45 RID: 31813
	public readonly TrajectorySimulation.ProjectileInfo Projectile;

	// Token: 0x04007C46 RID: 31814
	public readonly float RenderDuration;

	// Token: 0x04007C47 RID: 31815
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<TrajectorySimulation> OnSimulationComplete;

	// Token: 0x04007C48 RID: 31816
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 LaunchPosition;

	// Token: 0x04007C49 RID: 31817
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 TargetPosition;

	// Token: 0x04007C4A RID: 31818
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Trajectory.AimTrajectory[] SimTrajectories = new Trajectory.AimTrajectory[2];

	// Token: 0x04007C4B RID: 31819
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryIndex;

	// Token: 0x04007C4C RID: 31820
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryCount;

	// Token: 0x04007C4D RID: 31821
	[PublicizedFrom(EAccessModifier.Private)]
	public float SimTime;

	// Token: 0x04007C4E RID: 31822
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimVelocity;

	// Token: 0x04007C4F RID: 31823
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimLastPos;

	// Token: 0x04007C50 RID: 31824
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimCurrentPos;

	// Token: 0x04007C51 RID: 31825
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane CullPlane;

	// Token: 0x04007C52 RID: 31826
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxOwnerDelta = 16f;

	// Token: 0x04007C53 RID: 31827
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxTargetDelta = 16f;

	// Token: 0x04007C54 RID: 31828
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSimInterval = 0.05f;

	// Token: 0x04007C55 RID: 31829
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Queue<TrajectorySimulation> Trajectories = new Queue<TrajectorySimulation>();

	// Token: 0x020014D9 RID: 5337
	public enum State
	{
		// Token: 0x04007C57 RID: 31831
		Init,
		// Token: 0x04007C58 RID: 31832
		Invalid,
		// Token: 0x04007C59 RID: 31833
		ReadyToSimulate,
		// Token: 0x04007C5A RID: 31834
		Simulating,
		// Token: 0x04007C5B RID: 31835
		DoneWithSolution,
		// Token: 0x04007C5C RID: 31836
		DoneWithoutSolution
	}

	// Token: 0x020014DA RID: 5338
	public struct ProjectileInfo
	{
		// Token: 0x04007C5D RID: 31837
		public float ProjectileSpeed;

		// Token: 0x04007C5E RID: 31838
		public float ProjectileGravity;

		// Token: 0x04007C5F RID: 31839
		public float ProjectileRadius;

		// Token: 0x04007C60 RID: 31840
		public float ProjectileTargetRadius;
	}
}
