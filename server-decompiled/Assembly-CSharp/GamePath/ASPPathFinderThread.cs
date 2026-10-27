using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001936 RID: 6454
	public class ASPPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C6B0 RID: 50864 RVA: 0x0048EF4F File Offset: 0x0048D14F
		public ASPPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C6B1 RID: 50865 RVA: 0x0048EF73 File Offset: 0x0048D173
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C6B2 RID: 50866 RVA: 0x0048EF80 File Offset: 0x0048D180
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C6B3 RID: 50867 RVA: 0x0048EF92 File Offset: 0x0048D192
		public override void StartWorkerThreads()
		{
			this.coroutine = GameManager.Instance.StartCoroutine(this.FindPaths());
		}

		// Token: 0x0600C6B4 RID: 50868 RVA: 0x0048EFAA File Offset: 0x0048D1AA
		public override void Cleanup()
		{
			GameManager.Instance.StopCoroutine(this.coroutine);
			this.entityWaitQueue.Clear();
			this.finishedPaths.Clear();
		}

		// Token: 0x0600C6B5 RID: 50869 RVA: 0x0048EFD2 File Offset: 0x0048D1D2
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator FindPaths()
		{
			for (;;)
			{
				int num = 0;
				while (num < 8 && this.entityWaitQueue.list.Count != 0)
				{
					int num2 = this.entityWaitQueue.list[0];
					this.entityWaitQueue.Remove(num2);
					PathInfo pathInfo;
					if (!this.finishedPaths.TryGetValue(num2, out pathInfo))
					{
						Log.Warning("{0} path dup id {1}", new object[]
						{
							GameManager.frameCount,
							num2
						});
					}
					else
					{
						pathInfo.entity.navigator.GetPathTo(pathInfo);
						if (pathInfo.state == PathInfo.State.Queued)
						{
							this.finishedPaths.Remove(num2);
						}
					}
					num++;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600C6B6 RID: 50870 RVA: 0x0048EFE1 File Offset: 0x0048D1E1
		public override void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			this.finishedPaths[_entity.entityId] = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
		}

		// Token: 0x0600C6B7 RID: 50871 RVA: 0x0048F014 File Offset: 0x0048D214
		public override void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			PathInfoSingleTarget pathInfoSingleTarget = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
			pathInfoSingleTarget.SetStartPos(_startPos);
			this.finishedPaths[_entity.entityId] = pathInfoSingleTarget;
		}

		// Token: 0x0600C6B8 RID: 50872 RVA: 0x0048F059 File Offset: 0x0048D259
		public override void FindPath(PathInfo _pathInfo)
		{
			this.entityWaitQueue.Add(_pathInfo.entity.entityId);
			this.finishedPaths[_pathInfo.entity.entityId] = _pathInfo;
		}

		// Token: 0x0600C6B9 RID: 50873 RVA: 0x0048F088 File Offset: 0x0048D288
		public override PathInfo GetPath(int _entityId)
		{
			PathInfo pathInfo;
			if (this.finishedPaths.TryGetValue(_entityId, out pathInfo) && pathInfo.state == PathInfo.State.Done)
			{
				this.finishedPaths.Remove(_entityId);
				return pathInfo;
			}
			return null;
		}

		// Token: 0x0600C6BA RID: 50874 RVA: 0x0048F0BE File Offset: 0x0048D2BE
		public override bool IsCalculatingPath(int _entityId)
		{
			return this.finishedPaths.ContainsKey(_entityId);
		}

		// Token: 0x0600C6BB RID: 50875 RVA: 0x0048F0CC File Offset: 0x0048D2CC
		public override void RemovePathsFor(int _entityId)
		{
			this.finishedPaths.Remove(_entityId);
			this.entityWaitQueue.Remove(_entityId);
		}

		// Token: 0x04009601 RID: 38401
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine coroutine;

		// Token: 0x04009602 RID: 38402
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x04009603 RID: 38403
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
