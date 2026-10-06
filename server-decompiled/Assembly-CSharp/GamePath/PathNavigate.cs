using System;

namespace GamePath
{
	// Token: 0x02001913 RID: 6419
	public class PathNavigate
	{
		// Token: 0x0600C5DD RID: 50653 RVA: 0x0048BF5A File Offset: 0x0048A15A
		public PathNavigate(EntityAlive _ea)
		{
			this.theEntity = _ea;
			this.inWater = false;
			this.canDrown = false;
		}

		// Token: 0x0600C5DE RID: 50654 RVA: 0x0048BF77 File Offset: 0x0048A177
		public void setMoveSpeed(float _b)
		{
			this.speed = _b;
		}

		// Token: 0x0600C5DF RID: 50655 RVA: 0x0048BF80 File Offset: 0x0048A180
		public void setCanDrown(bool _b)
		{
			this.canDrown = _b;
		}

		// Token: 0x0600C5E0 RID: 50656 RVA: 0x0048BF89 File Offset: 0x0048A189
		public bool getCanDrown()
		{
			return this.canDrown;
		}

		// Token: 0x0600C5E1 RID: 50657 RVA: 0x0048BF91 File Offset: 0x0048A191
		public bool noPath()
		{
			return this.currentPath == null || this.currentPath.isFinished();
		}

		// Token: 0x0600C5E2 RID: 50658 RVA: 0x0048BFA8 File Offset: 0x0048A1A8
		public bool isPlanningPath()
		{
			return PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C5E3 RID: 50659 RVA: 0x0048BFBF File Offset: 0x0048A1BF
		public bool noPathAndNotPlanningOne()
		{
			return this.noPath() && !PathFinderThread.Instance.IsCalculatingPath(this.theEntity.entityId);
		}

		// Token: 0x0600C5E4 RID: 50660 RVA: 0x0048BFE3 File Offset: 0x0048A1E3
		public bool HasPath()
		{
			return this.currentPath != null && !this.currentPath.isFinished();
		}

		// Token: 0x0600C5E5 RID: 50661 RVA: 0x0048BFFD File Offset: 0x0048A1FD
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canNavigate()
		{
			return this.theEntity.CanNavigatePath();
		}

		// Token: 0x0600C5E6 RID: 50662 RVA: 0x0048C00A File Offset: 0x0048A20A
		public void clearPath()
		{
			if (this.currentPath != null)
			{
				this.currentPath.Destruct();
			}
			this.currentPath = null;
		}

		// Token: 0x0600C5E7 RID: 50663 RVA: 0x0048C026 File Offset: 0x0048A226
		public PathEntity getPath()
		{
			return this.currentPath;
		}

		// Token: 0x0600C5E8 RID: 50664 RVA: 0x0048C02E File Offset: 0x0048A22E
		public void ShortenEnd(float _distance)
		{
			if (this.currentPath != null)
			{
				this.currentPath.ShortenEnd(_distance);
			}
		}

		// Token: 0x0600C5E9 RID: 50665 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathTo(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C5EA RID: 50666 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void GetPathToEntity(PathInfo _pathInfo, EntityAlive _entity)
		{
		}

		// Token: 0x0600C5EB RID: 50667 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool SetPath(PathInfo _pathInfo, float _speed)
		{
			return false;
		}

		// Token: 0x0600C5EC RID: 50668 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void UpdateNavigation()
		{
		}

		// Token: 0x0600C5ED RID: 50669 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CreatePath()
		{
		}

		// Token: 0x0400954A RID: 38218
		public PathInfo pathInfo;

		// Token: 0x0400954B RID: 38219
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityAlive theEntity;

		// Token: 0x0400954C RID: 38220
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathEntity currentPath;

		// Token: 0x0400954D RID: 38221
		[PublicizedFrom(EAccessModifier.Protected)]
		public float speed;

		// Token: 0x0400954E RID: 38222
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canBreakBlocks;

		// Token: 0x0400954F RID: 38223
		[PublicizedFrom(EAccessModifier.Protected)]
		public int curNavTicks;

		// Token: 0x04009550 RID: 38224
		[PublicizedFrom(EAccessModifier.Protected)]
		public int prevNavTicks;

		// Token: 0x04009551 RID: 38225
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool inWater;

		// Token: 0x04009552 RID: 38226
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canDrown;
	}
}
