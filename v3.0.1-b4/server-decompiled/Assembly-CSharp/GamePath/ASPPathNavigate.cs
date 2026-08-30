using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018B5 RID: 6325
	[PublicizedFrom(EAccessModifier.Internal)]
	public class ASPPathNavigate : PathNavigate
	{
		// Token: 0x0600C2CF RID: 49871 RVA: 0x004811CF File Offset: 0x0047F3CF
		public ASPPathNavigate(EntityAlive _ea)
			: base(_ea)
		{
		}

		// Token: 0x0600C2D0 RID: 49872 RVA: 0x004811D8 File Offset: 0x0047F3D8
		public override void GetPathTo(PathInfo _pathInfo)
		{
			if (this.pathFinder != null)
			{
				this.pathFinder.Cancel();
				this.pathFinder = null;
			}
			this.pathInfo = _pathInfo;
			if (!base.canNavigate())
			{
				return;
			}
			this.CreatePath();
		}

		// Token: 0x0600C2D1 RID: 49873 RVA: 0x0048120C File Offset: 0x0047F40C
		public override bool SetPath(PathInfo _pathInfo, float _speed)
		{
			PathEntity pathEntity = ((_pathInfo != null) ? _pathInfo.path : null);
			if (pathEntity == null)
			{
				if (this.currentPath != null)
				{
					this.currentPath.Destruct();
				}
				this.currentPath = null;
				return false;
			}
			if (this.currentPath != null)
			{
				this.currentPath.Destruct();
			}
			this.currentPath = pathEntity;
			if (this.currentPath.getCurrentPathLength() == 0)
			{
				return true;
			}
			this.ImprovePath();
			this.speed = _speed;
			this.canBreakBlocks = _pathInfo.canBreakBlocks;
			return true;
		}

		// Token: 0x0600C2D2 RID: 49874 RVA: 0x00481288 File Offset: 0x0047F488
		[PublicizedFrom(EAccessModifier.Private)]
		public void ImprovePath()
		{
			PathPoint[] points = this.currentPath.points;
			int num = points.Length;
			for (int i = 0; i < num; i++)
			{
				points[i].ProjectToGround(this.theEntity);
			}
			if (num >= 2)
			{
				Vector3 projectedLocation = points[0].projectedLocation;
				Vector3 projectedLocation2 = points[1].projectedLocation;
				if (projectedLocation2.y - projectedLocation.y < 0.6f)
				{
					points[0].projectedLocation = VectorMath.ClosestPointOnSegment(projectedLocation, projectedLocation2, this.theEntity.position);
				}
			}
		}

		// Token: 0x0600C2D3 RID: 49875 RVA: 0x00481307 File Offset: 0x0047F507
		public override void UpdateNavigation()
		{
			if (base.noPath())
			{
				return;
			}
			this.pathFollow();
			if (base.noPath())
			{
				return;
			}
			this.theEntity.moveHelper.SetMoveTo(this.currentPath, this.speed, this.canBreakBlocks);
		}

		// Token: 0x0600C2D4 RID: 49876 RVA: 0x00481344 File Offset: 0x0047F544
		[PublicizedFrom(EAccessModifier.Private)]
		public void pathFollow()
		{
			Vector3 vector = this.currentPath.CurrentPoint.ProjectToGround(this.theEntity);
			Vector3 position = this.theEntity.position;
			Vector3 vector2 = VectorMath.ClosestPointOnSegment(this.theEntity.prevPos, position, vector);
			Vector3 vector3 = vector - vector2;
			float num = Utils.FastAbs(vector3.y);
			vector3.y = 0f;
			float num2 = this.theEntity.radius * 0.6f;
			float num3 = 0.15f;
			float num4 = 2f;
			int currentPathIndex = this.currentPath.getCurrentPathIndex();
			int currentPathLength = this.currentPath.getCurrentPathLength();
			if (currentPathIndex + 1 < currentPathLength)
			{
				num3 = ((this.theEntity.moveHelper.SideStepAngle != 0f) ? 0.49f : 0.33f);
			}
			if (this.theEntity.isSwimming)
			{
				num3 = 0.9f;
				num4 = 0.7f;
			}
			if (this.theEntity.IsInElevator())
			{
				num4 = 0.2f;
			}
			num2 = Utils.FastMax(num3, num2);
			bool flag = false;
			PathPoint nextPoint = this.currentPath.NextPoint;
			if (nextPoint != null)
			{
				Vector3 vector4 = nextPoint.ProjectToGround(this.theEntity);
				if ((VectorMath.ClosestPointOnSegment(vector, vector4, position) - position).sqrMagnitude < 0.040000003f)
				{
					flag = true;
				}
				if (vector.y - vector4.y > 2f)
				{
					Plane plane = new Plane(vector4 - vector, vector);
					if (plane.SameSide(position, vector4))
					{
						flag = true;
					}
				}
			}
			if (flag || (vector3.sqrMagnitude <= num2 * num2 && num <= num4))
			{
				if (currentPathIndex + 1 < currentPathLength)
				{
					this.currentPath.setCurrentPathIndex(currentPathIndex + 1, this.theEntity, position);
					return;
				}
				this.currentPath.setCurrentPathIndex(currentPathLength, this.theEntity, position);
			}
		}

		// Token: 0x0600C2D5 RID: 49877 RVA: 0x00481510 File Offset: 0x0047F710
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void CreatePath()
		{
			EntityAlive theEntity = this.theEntity;
			this.pathFinder = new ASPPathFinder(this.pathInfo, this.canDrown, theEntity.bCanClimbLadders, theEntity.bCanClimbVertical);
			if (this.pathInfo.hasStart)
			{
				this.pathFinder.Calculate(this.pathInfo.startPos);
				return;
			}
			Vector3 vector = theEntity.position + theEntity.motion * 2.5f;
			this.pathFinder.Calculate(vector);
		}

		// Token: 0x0400932F RID: 37679
		[PublicizedFrom(EAccessModifier.Private)]
		public ASPPathFinder pathFinder;
	}
}
