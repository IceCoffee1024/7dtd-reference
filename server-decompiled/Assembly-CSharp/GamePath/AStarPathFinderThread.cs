using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001901 RID: 6401
	public class AStarPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C585 RID: 50565 RVA: 0x0048A5BE File Offset: 0x004887BE
		public AStarPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C586 RID: 50566 RVA: 0x0048A5EE File Offset: 0x004887EE
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C587 RID: 50567 RVA: 0x0048A5FB File Offset: 0x004887FB
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C588 RID: 50568 RVA: 0x0048A610 File Offset: 0x00488810
		public override void StartWorkerThreads()
		{
			this.threadInfo = ThreadManager.StartThread("Pathfinder", null, new ThreadManager.ThreadFunctionLoopDelegate(this.thread_Pathfinder), null, null, null, false, false);
		}

		// Token: 0x0600C589 RID: 50569 RVA: 0x0048A640 File Offset: 0x00488840
		public override void Cleanup()
		{
			this.threadInfo.RequestTermination();
			this.writerThreadWaitHandle.Set();
			this.threadInfo.WaitForEnd(30);
			this.threadInfo = null;
			this.entityWaitQueue.Clear();
			this.finishedPaths.Clear();
			this.writerThreadWaitHandle = null;
		}

		// Token: 0x0600C58A RID: 50570 RVA: 0x0048A698 File Offset: 0x00488898
		[PublicizedFrom(EAccessModifier.Protected)]
		public int thread_Pathfinder(ThreadManager.ThreadInfo _threadInfo)
		{
			while (!_threadInfo.TerminationRequested())
			{
				try
				{
					if (this.entityWaitQueue.list.Count == 0)
					{
						this.writerThreadWaitHandle.WaitOne();
					}
					PathInfo pathInfo = null;
					Dictionary<int, PathInfo> dictionary = this.finishedPaths;
					lock (dictionary)
					{
						if (this.entityWaitQueue.list.Count <= 0)
						{
							continue;
						}
						int num = this.entityWaitQueue.list[0];
						this.entityWaitQueue.Remove(num);
						if (!this.finishedPaths.ContainsKey(num))
						{
							continue;
						}
						pathInfo = this.finishedPaths[num];
					}
					pathInfo.entity.navigator.GetPathTo(pathInfo);
					dictionary = this.finishedPaths;
					lock (dictionary)
					{
						if (pathInfo.path == null)
						{
							this.finishedPaths.Remove(pathInfo.entity.entityId);
						}
						else
						{
							this.finishedPaths[pathInfo.entity.entityId] = pathInfo;
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception in PathFinder thread: " + ex.Message);
					Log.Error(ex.StackTrace);
				}
			}
			return -1;
		}

		// Token: 0x0600C58B RID: 50571 RVA: 0x0048A804 File Offset: 0x00488A04
		public override bool IsCalculatingPath(int _entityId)
		{
			Dictionary<int, PathInfo> dictionary = this.finishedPaths;
			bool flag2;
			lock (dictionary)
			{
				flag2 = this.finishedPaths.ContainsKey(_entityId);
			}
			return flag2;
		}

		// Token: 0x0600C58C RID: 50572 RVA: 0x0048A84C File Offset: 0x00488A4C
		public override void FindPath(EntityAlive _entity, Vector3 _target, float _speed, bool _canBreak, EAIBase _aiTask)
		{
			Dictionary<int, PathInfo> dictionary = this.finishedPaths;
			lock (dictionary)
			{
				if (!this.entityWaitQueue.hashSet.Contains(_entity.entityId))
				{
					this.entityWaitQueue.Add(_entity.entityId);
				}
				this.finishedPaths[_entity.entityId] = new PathInfoSingleTarget(_entity, _target, _canBreak, _speed, _aiTask);
			}
			this.writerThreadWaitHandle.Set();
		}

		// Token: 0x0600C58D RID: 50573 RVA: 0x0048A8D8 File Offset: 0x00488AD8
		public override PathInfo GetPath(int _entityId)
		{
			Dictionary<int, PathInfo> dictionary = this.finishedPaths;
			lock (dictionary)
			{
				PathInfo pathInfo;
				if (this.finishedPaths.TryGetValue(_entityId, out pathInfo) && pathInfo.path != null)
				{
					this.finishedPaths.Remove(_entityId);
					return pathInfo;
				}
			}
			return null;
		}

		// Token: 0x0600C58E RID: 50574 RVA: 0x0048A940 File Offset: 0x00488B40
		public override void RemovePathsFor(int _entityId)
		{
			Dictionary<int, PathInfo> dictionary = this.finishedPaths;
			lock (dictionary)
			{
				this.finishedPaths.Remove(_entityId);
				if (this.entityWaitQueue.hashSet.Contains(_entityId))
				{
					this.entityWaitQueue.Remove(_entityId);
				}
			}
		}

		// Token: 0x04009510 RID: 38160
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo threadInfo;

		// Token: 0x04009511 RID: 38161
		[PublicizedFrom(EAccessModifier.Private)]
		public AutoResetEvent writerThreadWaitHandle = new AutoResetEvent(false);

		// Token: 0x04009512 RID: 38162
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x04009513 RID: 38163
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
