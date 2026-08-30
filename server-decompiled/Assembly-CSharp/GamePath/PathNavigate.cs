using System;

namespace GamePath
{
	// Token: 0x020018E7 RID: 6375
	public class PathNavigate
	{
		// Token: 0x0600C485 RID: 50309 RVA: 0x00487A12 File Offset: 0x00485C12
		public PathNavigate(EntityAlive _ea)
		{
			this.theEntity = _ea;
			this.inWater = false;
			this.canDrown = false;
		}

		// Token: 0x0600C486 RID: 50310 RVA: 0x00487A2F File Offset: 0x00485C2F
		public void setMoveSpeed(float _b)
		{
			this.speed = _b;
		}

		// Token: 0x0600C487 RID: 50311 RVA: 0x00487A38 File Offset: 0x00485C38
		public void setCanDrown(bool _b)
		{
			this.canDrown = _b;
		}

		// Token: 0x0600C488 RID: 50312 RVA: 0x00487A41 File Offset: 0x00485C41
		public bool getCanDrown()
		{
			return this.canDrown;
		}

		// Token: 0x0600C489 RID: 50313 RVA: 0x00487A49 File Offset: 0x00485C49
		public bool noPath()
		{
			return this.currentPath == null || this.currentPath.isFinished();
		}

		// Token: 0x0600C48A RID: 50314 RVA: 0x00487A60 File Offset: 0x00485C60
		public bool isPlanningPath()
		{
			return PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C48B RID: 50315 RVA: 0x00487A77 File Offset: 0x00485C77
		public bool noPathAndNotPlanningOne()
		{
			return this.noPath() && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C48C RID: 50316 RVA: 0x00487A9B File Offset: 0x00485C9B
		public bool HasPath()
		{
			return this.currentPath != null && !this.currentPath.isFinished();
		}

		// Token: 0x0600C48D RID: 50317 RVA: 0x00487AB5 File Offset: 0x00485CB5
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canNavigate()
		{
			return this.theEntity.CanNavigatePath();
		}

		// Token: 0x0600C48E RID: 50318 RVA: 0x00487AC2 File Offset: 0x00485CC2
		public void clearPath()
		{
			if (this.currentPath != null)
			{
				this.currentPath.Destruct();
			}
			this.currentPath = null;
		}

		// Token: 0x0600C48F RID: 50319 RVA: 0x00487ADE File Offset: 0x00485CDE
		public PathEntity getPath()
		{
			return this.currentPath;
		}

		// Token: 0x0600C490 RID: 50320 RVA: 0x00487AE6 File Offset: 0x00485CE6
		public void ShortenEnd(float _distance)
		{
			if (this.currentPath != null)
			{
				this.currentPath.ShortenEnd(_distance);
			}
		}

		// Token: 0x0600C491 RID: 50321 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathTo(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C492 RID: 50322 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathToEntity(PathInfo _pathInfo, EntityAlive _entity)
		{
		}

		// Token: 0x0600C493 RID: 50323 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool SetPath(PathInfo _pathInfo, float _speed)
		{
			return false;
		}

		// Token: 0x0600C494 RID: 50324 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateNavigation()
		{
		}

		// Token: 0x0600C495 RID: 50325 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CreatePath()
		{
		}

		// Token: 0x04009484 RID: 38020
		public PathInfo pathInfo;

		// Token: 0x04009485 RID: 38021
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityAlive theEntity;

		// Token: 0x04009486 RID: 38022
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathEntity currentPath;

		// Token: 0x04009487 RID: 38023
		[PublicizedFrom(EAccessModifier.Protected)]
		public float speed;

		// Token: 0x04009488 RID: 38024
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canBreakBlocks;

		// Token: 0x04009489 RID: 38025
		[PublicizedFrom(EAccessModifier.Protected)]
		public int curNavTicks;

		// Token: 0x0400948A RID: 38026
		[PublicizedFrom(EAccessModifier.Protected)]
		public int prevNavTicks;

		// Token: 0x0400948B RID: 38027
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool inWater;

		// Token: 0x0400948C RID: 38028
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canDrown;
	}
}
