using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001932 RID: 6450
	public class AStarPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C699 RID: 50841 RVA: 0x0048DF6A File Offset: 0x0048C16A
		public AStarPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C69A RID: 50842 RVA: 0x0048DF9A File Offset: 0x0048C19A
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C69B RID: 50843 RVA: 0x0048DFA7 File Offset: 0x0048C1A7
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C69C RID: 50844 RVA: 0x0048DFBC File Offset: 0x0048C1BC
		public override void StartWorkerThreads()
		{
			this.threadInfo = ThreadManager.StartThread("Pathfinder", null, new ThreadManager.ThreadFunctionLoopDelegate(this.thread_Pathfinder), null, null, null, false, false);
		}

		// Token: 0x0600C69D RID: 50845 RVA: 0x0048DFEC File Offset: 0x0048C1EC
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

		// Token: 0x0600C69E RID: 50846 RVA: 0x0048E044 File Offset: 0x0048C244
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

		// Token: 0x0600C69F RID: 50847 RVA: 0x0048E1B0 File Offset: 0x0048C3B0
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

		// Token: 0x0600C6A0 RID: 50848 RVA: 0x0048E1F8 File Offset: 0x0048C3F8
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

		// Token: 0x0600C6A1 RID: 50849 RVA: 0x0048E284 File Offset: 0x0048C484
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

		// Token: 0x0600C6A2 RID: 50850 RVA: 0x0048E2EC File Offset: 0x0048C4EC
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

		// Token: 0x040095F9 RID: 38393
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo threadInfo;

		// Token: 0x040095FA RID: 38394
		[PublicizedFrom(EAccessModifier.Private)]
		public AutoResetEvent writerThreadWaitHandle = new AutoResetEvent(false);

		// Token: 0x040095FB RID: 38395
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x040095FC RID: 38396
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
