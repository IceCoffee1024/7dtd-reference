using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001905 RID: 6405
	public class ASPPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C59C RID: 50588 RVA: 0x0048B5A3 File Offset: 0x004897A3
		public ASPPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C59D RID: 50589 RVA: 0x0048B5C7 File Offset: 0x004897C7
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C59E RID: 50590 RVA: 0x0048B5D4 File Offset: 0x004897D4
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C59F RID: 50591 RVA: 0x0048B5E6 File Offset: 0x004897E6
		public override void StartWorkerThreads()
		{
			this.coroutine = GameManager.Instance.StartCoroutine(this.FindPaths());
		}

		// Token: 0x0600C5A0 RID: 50592 RVA: 0x0048B5FE File Offset: 0x004897FE
		public override void Cleanup()
		{
			GameManager.Instance.StopCoroutine(this.coroutine);
			this.entityWaitQueue.Clear();
			this.finishedPaths.Clear();
		}

		// Token: 0x0600C5A1 RID: 50593 RVA: 0x0048B626 File Offset: 0x00489826
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

		// Token: 0x0600C5A2 RID: 50594 RVA: 0x0048B635 File Offset: 0x00489835
		public override void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			this.finishedPaths[_entity.entityId] = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
		}

		// Token: 0x0600C5A3 RID: 50595 RVA: 0x0048B668 File Offset: 0x00489868
		public override void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			PathInfoSingleTarget pathInfoSingleTarget = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
			pathInfoSingleTarget.SetStartPos(_startPos);
			this.finishedPaths[_entity.entityId] = pathInfoSingleTarget;
		}

		// Token: 0x0600C5A4 RID: 50596 RVA: 0x0048B6AD File Offset: 0x004898AD
		public override void FindPath(PathInfo _pathInfo)
		{
			this.entityWaitQueue.Add(_pathInfo.entity.entityId);
			this.finishedPaths[_pathInfo.entity.entityId] = _pathInfo;
		}

		// Token: 0x0600C5A5 RID: 50597 RVA: 0x0048B6DC File Offset: 0x004898DC
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

		// Token: 0x0600C5A6 RID: 50598 RVA: 0x0048B712 File Offset: 0x00489912
		public override bool IsCalculatingPath(int _entityId)
		{
			return this.finishedPaths.ContainsKey(_entityId);
		}

		// Token: 0x0600C5A7 RID: 50599 RVA: 0x0048B720 File Offset: 0x00489920
		public override void RemovePathsFor(int _entityId)
		{
			this.finishedPaths.Remove(_entityId);
			this.entityWaitQueue.Remove(_entityId);
		}

		// Token: 0x04009518 RID: 38168
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine coroutine;

		// Token: 0x04009519 RID: 38169
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x0400951A RID: 38170
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
