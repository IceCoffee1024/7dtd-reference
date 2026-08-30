using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018D9 RID: 6361
	public class ASPPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C444 RID: 50244 RVA: 0x0048705B File Offset: 0x0048525B
		public ASPPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C445 RID: 50245 RVA: 0x0048707F File Offset: 0x0048527F
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C446 RID: 50246 RVA: 0x0048708C File Offset: 0x0048528C
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C447 RID: 50247 RVA: 0x0048709E File Offset: 0x0048529E
		public override void StartWorkerThreads()
		{
			this.coroutine = GameManager.Instance.StartCoroutine(this.FindPaths());
		}

		// Token: 0x0600C448 RID: 50248 RVA: 0x004870B6 File Offset: 0x004852B6
		public override void Cleanup()
		{
			GameManager.Instance.StopCoroutine(this.coroutine);
			this.entityWaitQueue.Clear();
			this.finishedPaths.Clear();
		}

		// Token: 0x0600C449 RID: 50249 RVA: 0x004870DE File Offset: 0x004852DE
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

		// Token: 0x0600C44A RID: 50250 RVA: 0x004870ED File Offset: 0x004852ED
		public override void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			this.finishedPaths[_entity.entityId] = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
		}

		// Token: 0x0600C44B RID: 50251 RVA: 0x00487120 File Offset: 0x00485320
		public override void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			PathInfoSingleTarget pathInfoSingleTarget = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
			pathInfoSingleTarget.SetStartPos(_startPos);
			this.finishedPaths[_entity.entityId] = pathInfoSingleTarget;
		}

		// Token: 0x0600C44C RID: 50252 RVA: 0x00487165 File Offset: 0x00485365
		public override void FindPath(PathInfo _pathInfo)
		{
			this.entityWaitQueue.Add(_pathInfo.entity.entityId);
			this.finishedPaths[_pathInfo.entity.entityId] = _pathInfo;
		}

		// Token: 0x0600C44D RID: 50253 RVA: 0x00487194 File Offset: 0x00485394
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

		// Token: 0x0600C44E RID: 50254 RVA: 0x004871CA File Offset: 0x004853CA
		public override bool IsCalculatingPath(int _entityId)
		{
			return this.finishedPaths.ContainsKey(_entityId);
		}

		// Token: 0x0600C44F RID: 50255 RVA: 0x004871D8 File Offset: 0x004853D8
		public override void RemovePathsFor(int _entityId)
		{
			this.finishedPaths.Remove(_entityId);
			this.entityWaitQueue.Remove(_entityId);
		}

		// Token: 0x04009452 RID: 37970
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine coroutine;

		// Token: 0x04009453 RID: 37971
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x04009454 RID: 37972
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
