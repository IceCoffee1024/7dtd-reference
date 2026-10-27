using System;

namespace GamePath
{
	// Token: 0x02001944 RID: 6468
	public class PathNavigate
	{
		// Token: 0x0600C6F1 RID: 50929 RVA: 0x0048F906 File Offset: 0x0048DB06
		public PathNavigate(EntityAlive _ea)
		{
			this.theEntity = _ea;
			this.inWater = false;
			this.canDrown = false;
		}

		// Token: 0x0600C6F2 RID: 50930 RVA: 0x0048F923 File Offset: 0x0048DB23
		public void setMoveSpeed(float _b)
		{
			this.speed = _b;
		}

		// Token: 0x0600C6F3 RID: 50931 RVA: 0x0048F92C File Offset: 0x0048DB2C
		public void setCanDrown(bool _b)
		{
			this.canDrown = _b;
		}

		// Token: 0x0600C6F4 RID: 50932 RVA: 0x0048F935 File Offset: 0x0048DB35
		public bool getCanDrown()
		{
			return this.canDrown;
		}

		// Token: 0x0600C6F5 RID: 50933 RVA: 0x0048F93D File Offset: 0x0048DB3D
		public bool noPath()
		{
			return this.currentPath == null || this.currentPath.isFinished();
		}

		// Token: 0x0600C6F6 RID: 50934 RVA: 0x0048F954 File Offset: 0x0048DB54
		public bool isPlanningPath()
		{
			return PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C6F7 RID: 50935 RVA: 0x0048F96B File Offset: 0x0048DB6B
		public bool noPathAndNotPlanningOne()
		{
			return this.noPath() && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C6F8 RID: 50936 RVA: 0x0048F98F File Offset: 0x0048DB8F
		public bool HasPath()
		{
			return this.currentPath != null && !this.currentPath.isFinished();
		}

		// Token: 0x0600C6F9 RID: 50937 RVA: 0x0048F9A9 File Offset: 0x0048DBA9
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canNavigate()
		{
			return this.theEntity.CanNavigatePath();
		}

		// Token: 0x0600C6FA RID: 50938 RVA: 0x0048F9B6 File Offset: 0x0048DBB6
		public void clearPath()
		{
			if (this.currentPath != null)
			{
				this.currentPath.Destruct();
			}
			this.currentPath = null;
		}

		// Token: 0x0600C6FB RID: 50939 RVA: 0x0048F9D2 File Offset: 0x0048DBD2
		public PathEntity getPath()
		{
			return this.currentPath;
		}

		// Token: 0x0600C6FC RID: 50940 RVA: 0x0048F9DA File Offset: 0x0048DBDA
		public void ShortenEnd(float _distance)
		{
			if (this.currentPath != null)
			{
				this.currentPath.ShortenEnd(_distance);
			}
		}

		// Token: 0x0600C6FD RID: 50941 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathTo(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C6FE RID: 50942 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathToEntity(PathInfo _pathInfo, EntityAlive _entity)
		{
		}

		// Token: 0x0600C6FF RID: 50943 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool SetPath(PathInfo _pathInfo, float _speed)
		{
			return false;
		}

		// Token: 0x0600C700 RID: 50944 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateNavigation()
		{
		}

		// Token: 0x0600C701 RID: 50945 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CreatePath()
		{
		}

		// Token: 0x04009633 RID: 38451
		public PathInfo pathInfo;

		// Token: 0x04009634 RID: 38452
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityAlive theEntity;

		// Token: 0x04009635 RID: 38453
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathEntity currentPath;

		// Token: 0x04009636 RID: 38454
		[PublicizedFrom(EAccessModifier.Protected)]
		public float speed;

		// Token: 0x04009637 RID: 38455
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canBreakBlocks;

		// Token: 0x04009638 RID: 38456
		[PublicizedFrom(EAccessModifier.Protected)]
		public int curNavTicks;

		// Token: 0x04009639 RID: 38457
		[PublicizedFrom(EAccessModifier.Protected)]
		public int prevNavTicks;

		// Token: 0x0400963A RID: 38458
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool inWater;

		// Token: 0x0400963B RID: 38459
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canDrown;
	}
}
