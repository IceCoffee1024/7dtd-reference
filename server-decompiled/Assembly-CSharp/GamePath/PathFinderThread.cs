using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018DE RID: 6366
	public class PathFinderThread
	{
		// Token: 0x0600C472 RID: 50290 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFinishedCount()
		{
			return 0;
		}

		// Token: 0x0600C473 RID: 50291 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetQueueCount()
		{
			return 0;
		}

		// Token: 0x0600C474 RID: 50292 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartWorkerThreads()
		{
		}

		// Token: 0x0600C475 RID: 50293 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600C476 RID: 50294 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsCalculatingPath(int _entityId)
		{
			return false;
		}

		// Token: 0x0600C477 RID: 50295 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C478 RID: 50296 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C479 RID: 50297 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C47A RID: 50298 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual PathInfo GetPath(int _entityId)
		{
			return null;
		}

		// Token: 0x0600C47B RID: 50299 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void RemovePathsFor(int _entityId)
		{
		}

		// Token: 0x04009462 RID: 37986
		public static PathFinderThread Instance;
	}
}
