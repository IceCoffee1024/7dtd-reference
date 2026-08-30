using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018AF RID: 6319
	public class AStarPathFinderThread : PathFinderThread
	{
		// Token: 0x0600C2A6 RID: 49830 RVA: 0x0047FF4A File Offset: 0x0047E14A
		public AStarPathFinderThread()
		{
			PathFinderThread.Instance = this;
		}

		// Token: 0x0600C2A7 RID: 49831 RVA: 0x0047FF7A File Offset: 0x0047E17A
		public override int GetFinishedCount()
		{
			return this.finishedPaths.Count;
		}

		// Token: 0x0600C2A8 RID: 49832 RVA: 0x0047FF87 File Offset: 0x0047E187
		public override int GetQueueCount()
		{
			return this.entityWaitQueue.list.Count;
		}

		// Token: 0x0600C2A9 RID: 49833 RVA: 0x0047FF9C File Offset: 0x0047E19C
		public override void StartWorkerThreads()
		{
			this.threadInfo = ThreadManager.StartThread("Pathfinder", null, new ThreadManager.ThreadFunctionLoopDelegate(this.thread_Pathfinder), null, null, null, false, false);
		}

		// Token: 0x0600C2AA RID: 49834 RVA: 0x0047FFCC File Offset: 0x0047E1CC
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

		// Token: 0x0600C2AB RID: 49835 RVA: 0x00480024 File Offset: 0x0047E224
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

		// Token: 0x0600C2AC RID: 49836 RVA: 0x00480190 File Offset: 0x0047E390
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

		// Token: 0x0600C2AD RID: 49837 RVA: 0x004801D8 File Offset: 0x0047E3D8
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

		// Token: 0x0600C2AE RID: 49838 RVA: 0x00480264 File Offset: 0x0047E464
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

		// Token: 0x0600C2AF RID: 49839 RVA: 0x004802CC File Offset: 0x0047E4CC
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

		// Token: 0x04009321 RID: 37665
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo threadInfo;

		// Token: 0x04009322 RID: 37666
		[PublicizedFrom(EAccessModifier.Private)]
		public AutoResetEvent writerThreadWaitHandle = new AutoResetEvent(false);

		// Token: 0x04009323 RID: 37667
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetList<int> entityWaitQueue = new HashSetList<int>();

		// Token: 0x04009324 RID: 37668
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, PathInfo> finishedPaths = new Dictionary<int, PathInfo>();
	}
}
