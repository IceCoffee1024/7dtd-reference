using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018B3 RID: 6323
	public class ASPPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C2BD RID: 49853 RVA: 0x00480F2F File Offset: 0x0047F12F
		public ASPPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C2BE RID: 49854 RVA: 0x00480F53 File Offset: 0x0047F153
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C2BF RID: 49855 RVA: 0x00480F60 File Offset: 0x0047F160
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C2C0 RID: 49856 RVA: 0x00480F72 File Offset: 0x0047F172
		public override void StartWorkerThreads()
		{
			this.coroutine = GameManager.Instance.StartCoroutine(this.FindPaths());
		}

		// Token: 0x0600C2C1 RID: 49857 RVA: 0x00480F8A File Offset: 0x0047F18A
		public override void Cleanup()
		{
			GameManager.Instance.StopCoroutine(this.coroutine);
			this.entityWaitQueue.Clear();
			this.finishedPaths.Clear();
		}

		// Token: 0x0600C2C2 RID: 49858 RVA: 0x00480FB2 File Offset: 0x0047F1B2
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

		// Token: 0x0600C2C3 RID: 49859 RVA: 0x00480FC1 File Offset: 0x0047F1C1
		public override void FindPath(EntityAlive _entity, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			this.finishedPaths[_entity.entityId] = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
		}

		// Token: 0x0600C2C4 RID: 49860 RVA: 0x00480FF4 File Offset: 0x0047F1F4
		public override void FindPath(EntityAlive _entity, Vector3 _startPos, Vector3 _targetPos, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			this.entityWaitQueue.Add(_entity.entityId);
			PathInfoSingleTarget pathInfoSingleTarget = new PathInfoSingleTarget(_entity, _targetPos, _canBreak, _speed, _aiTask);
			pathInfoSingleTarget.SetStartPos(_startPos);
			this.finishedPaths[_entity.entityId] = pathInfoSingleTarget;
		}

		// Token: 0x0600C2C5 RID: 49861 RVA: 0x00481039 File Offset: 0x0047F239
		public override void FindPath(PathInfo _pathInfo)
		{
			this.entityWaitQueue.Add(_pathInfo.entity.entityId);
			this.finishedPaths[_pathInfo.entity.entityId] = _pathInfo;
		}

		// Token: 0x0600C2C6 RID: 49862 RVA: 0x00481068 File Offset: 0x0047F268
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

		// Token: 0x0600C2C7 RID: 49863 RVA: 0x0048109E File Offset: 0x0047F29E
		public override bool IsCalculatingPath(int _entityId)
		{
			return this.finishedPaths.ContainsKey(_entityId);
		}

		// Token: 0x0600C2C8 RID: 49864 RVA: 0x004810AC File Offset: 0x0047F2AC
		public override void RemovePathsFor(int _entityId)
		{
			this.finishedPaths.Remove(_entityId);
			this.entityWaitQueue.Remove(_entityId);
		}

		// Token: 0x04009329 RID: 37673
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine coroutine;

		// Token: 0x0400932A RID: 37674
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x0400932B RID: 37675
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
