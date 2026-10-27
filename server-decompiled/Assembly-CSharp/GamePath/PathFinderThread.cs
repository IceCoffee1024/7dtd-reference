using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200193B RID: 6459
	public class PathFinderThread
	{
		// Token: 0x0600C6DE RID: 50910 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFinishedCount()
		{
			return 0;
		}

		// Token: 0x0600C6DF RID: 50911 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetQueueCount()
		{
			return 0;
		}

		// Token: 0x0600C6E0 RID: 50912 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartWorkerThreads()
		{
		}

		// Token: 0x0600C6E1 RID: 50913 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600C6E2 RID: 50914 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsCalculatingPath(int _entityId)
		{
			return false;
		}

		// Token: 0x0600C6E3 RID: 50915 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C6E4 RID: 50916 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C6E5 RID: 50917 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C6E6 RID: 50918 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual PathInfo GetPath(int _entityId)
		{
			return null;
		}

		// Token: 0x0600C6E7 RID: 50919 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void RemovePathsFor(int _entityId)
		{
		}

		// Token: 0x04009611 RID: 38417
		public static PathFinderThread Instance;
	}
}
