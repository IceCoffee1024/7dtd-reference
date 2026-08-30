using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018D5 RID: 6357
	public class AStarPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C42D RID: 50221 RVA: 0x00486076 File Offset: 0x00484276
		public AStarPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C42E RID: 50222 RVA: 0x004860A6 File Offset: 0x004842A6
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C42F RID: 50223 RVA: 0x004860B3 File Offset: 0x004842B3
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C430 RID: 50224 RVA: 0x004860C8 File Offset: 0x004842C8
		public override void StartWorkerThreads()
		{
			this.threadInfo = ThreadManager.StartThread("Pathfinder", null, new ThreadManager.ThreadFunctionLoopDelegate(this.thread_Pathfinder), null, null, null, false, false);
		}

		// Token: 0x0600C431 RID: 50225 RVA: 0x004860F8 File Offset: 0x004842F8
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

		// Token: 0x0600C432 RID: 50226 RVA: 0x00486150 File Offset: 0x00484350
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

		// Token: 0x0600C433 RID: 50227 RVA: 0x004862BC File Offset: 0x004844BC
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

		// Token: 0x0600C434 RID: 50228 RVA: 0x00486304 File Offset: 0x00484504
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

		// Token: 0x0600C435 RID: 50229 RVA: 0x00486390 File Offset: 0x00484590
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

		// Token: 0x0600C436 RID: 50230 RVA: 0x004863F8 File Offset: 0x004845F8
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

		// Token: 0x0400944A RID: 37962
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo threadInfo;

		// Token: 0x0400944B RID: 37963
		[PublicizedFrom(EAccessModifier.Private)]
		public AutoResetEvent writerThreadWaitHandle = new AutoResetEvent(false);

		// Token: 0x0400944C RID: 37964
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x0400944D RID: 37965
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
