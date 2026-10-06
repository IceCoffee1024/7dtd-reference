using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200190A RID: 6410
	public class PathFinderThread
	{
		// Token: 0x0600C5CA RID: 50634 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFinishedCount()
		{
			return 0;
		}

		// Token: 0x0600C5CB RID: 50635 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetQueueCount()
		{
			return 0;
		}

		// Token: 0x0600C5CC RID: 50636 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartWorkerThreads()
		{
		}

		// Token: 0x0600C5CD RID: 50637 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600C5CE RID: 50638 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsCalculatingPath(int _entityId)
		{
			return false;
		}

		// Token: 0x0600C5CF RID: 50639 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C5D0 RID: 50640 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C5D1 RID: 50641 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C5D2 RID: 50642 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual PathInfo GetPath(int _entityId)
		{
			return null;
		}

		// Token: 0x0600C5D3 RID: 50643 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void RemovePathsFor(int _entityId)
		{
		}

		// Token: 0x04009528 RID: 38184
		public static PathFinderThread Instance;
	}
}
