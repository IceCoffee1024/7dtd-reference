using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018B8 RID: 6328
	public class PathFinderThread
	{
		// Token: 0x0600C2EB RID: 49899 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFinishedCount()
		{
			return 0;
		}

		// Token: 0x0600C2EC RID: 49900 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetQueueCount()
		{
			return 0;
		}

		// Token: 0x0600C2ED RID: 49901 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartWorkerThreads()
		{
		}

		// Token: 0x0600C2EE RID: 49902 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600C2EF RID: 49903 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsCalculatingPath(int _entityId)
		{
			return false;
		}

		// Token: 0x0600C2F0 RID: 49904 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C2F1 RID: 49905 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
		}

		// Token: 0x0600C2F2 RID: 49906 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void FindPath(PathInfo _pathInfo)
		{
		}

		// Token: 0x0600C2F3 RID: 49907 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual PathInfo GetPath(int _entityId)
		{
			return null;
		}

		// Token: 0x0600C2F4 RID: 49908 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void RemovePathsFor(int _entityId)
		{
		}

		// Token: 0x04009339 RID: 37689
		public static PathFinderThread Instance;
	}
}
