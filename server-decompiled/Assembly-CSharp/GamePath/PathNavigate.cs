using System;

namespace GamePath
{
	// Token: 0x020018C1 RID: 6337
	public class PathNavigate
	{
		// Token: 0x0600C2FE RID: 49918 RVA: 0x004818E6 File Offset: 0x0047FAE6
		public PathNavigate(EntityAlive _ea)
		{
			this.theEntity = _ea;
			this.inWater = false;
			this.canDrown = false;
		}

		// Token: 0x0600C2FF RID: 49919 RVA: 0x00481903 File Offset: 0x0047FB03
		public void setMoveSpeed(float _b)
		{
			this.speed = _b;
		}

		// Token: 0x0600C300 RID: 49920 RVA: 0x0048190C File Offset: 0x0047FB0C
		public void setCanDrown(bool _b)
		{
			this.canDrown = _b;
		}

		// Token: 0x0600C301 RID: 49921 RVA: 0x00481915 File Offset: 0x0047FB15
		public bool getCanDrown()
		{
			return this.canDrown;
		}

		// Token: 0x0600C302 RID: 49922 RVA: 0x0048191D File Offset: 0x0047FB1D
		public bool noPath()
		{
			return this.currentPath == null || this.currentPath.isFinished();
		}

		// Token: 0x0600C303 RID: 49923 RVA: 0x00481934 File Offset: 0x0047FB34
		public bool isPlanningPath()
		{
			return PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C304 RID: 49924 RVA: 0x0048194B File Offset: 0x0047FB4B
		public bool noPathAndNotPlanningOne()
		{
			return this.noPath() && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C305 RID: 49925 RVA: 0x0048196F File Offset: 0x0047FB6F
		public bool HasPath()
		{
			return this.currentPath != null && !this.currentPath.isFinished();
		}

		// Token: 0x0600C306 RID: 49926 RVA: 0x00481989 File Offset: 0x0047FB89
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canNavigate()
		{
			return this.theEntity.CanNavigatePath();
		}

		// Token: 0x0600C307 RID: 49927 RVA: 0x00481996 File Offset: 0x0047FB96
		public void clearPath()
		{
			if (this.currentPath != null)
			{
				this.currentPath.Destruct();
			}
			this.currentPath = null;
		}

		// Token: 0x0600C308 RID: 49928 RVA: 0x004819B2 File Offset: 0x0047FBB2
		public PathEntity getPath()
		{
			return this.currentPath;
		}

		// Token: 0x0600C309 RID: 49929 RVA: 0x004819BA File Offset: 0x0047FBBA
		public void ShortenEnd(float _distance)
		{
			if (this.currentPath != null)
			{
				this.currentPath.ShortenEnd(_distance);
			}
		}

		// Token: 0x0600C30A RID: 49930 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathTo(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C30B RID: 49931 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathToEntity(PathInfo _pathInfo, EntityAlive _entity)
		{
		}

		// Token: 0x0600C30C RID: 49932 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool SetPath(PathInfo _pathInfo, float _speed)
		{
			return false;
		}

		// Token: 0x0600C30D RID: 49933 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateNavigation()
		{
		}

		// Token: 0x0600C30E RID: 49934 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CreatePath()
		{
		}

		// Token: 0x0400935B RID: 37723
		public PathInfo pathInfo;

		// Token: 0x0400935C RID: 37724
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityAlive theEntity;

		// Token: 0x0400935D RID: 37725
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathEntity currentPath;

		// Token: 0x0400935E RID: 37726
		[PublicizedFrom(EAccessModifier.Protected)]
		public float speed;

		// Token: 0x0400935F RID: 37727
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canBreakBlocks;

		// Token: 0x04009360 RID: 37728
		[PublicizedFrom(EAccessModifier.Protected)]
		public int curNavTicks;

		// Token: 0x04009361 RID: 37729
		[PublicizedFrom(EAccessModifier.Protected)]
		public int prevNavTicks;

		// Token: 0x04009362 RID: 37730
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool inWater;

		// Token: 0x04009363 RID: 37731
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canDrown;
	}
}
