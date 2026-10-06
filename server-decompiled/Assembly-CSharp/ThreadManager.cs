using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x02001301 RID: 4865
public static class ThreadManager
{
	// Token: 0x140000FE RID: 254
	// (add) Token: 0x060099FE RID: 39422 RVA: 0x003A6E18 File Offset: 0x003A5018
	// (remove) Token: 0x060099FF RID: 39423 RVA: 0x003A6E4C File Offset: 0x003A504C
	public static event Action UpdateEv;

	// Token: 0x140000FF RID: 255
	// (add) Token: 0x06009A00 RID: 39424 RVA: 0x003A6E80 File Offset: 0x003A5080
	// (remove) Token: 0x06009A01 RID: 39425 RVA: 0x003A6EB4 File Offset: 0x003A50B4
	public static event Action LateUpdateEv;

	// Token: 0x06009A02 RID: 39426 RVA: 0x003A6EE7 File Offset: 0x003A50E7
	public static void ReleaseTaskInfo(ThreadManager.TaskInfo _info)
	{
		if (((_info != null) ? _info.evStopped : null) != null)
		{
			_info.evStopped.Close();
			_info.evStopped = null;
		}
	}

	// Token: 0x1700126F RID: 4719
	// (get) Token: 0x06009A03 RID: 39427 RVA: 0x003A6F09 File Offset: 0x003A5109
	public static TaskFactory MainThreadFactory
	{
		get
		{
			return ThreadManager.MainThreadScheduler.Factory;
		}
	}

	// Token: 0x06009A04 RID: 39428 RVA: 0x003A6F18 File Offset: 0x003A5118
	[PublicizedFrom(EAccessModifier.Private)]
	public static ThreadManager.ThreadInfo startThread(string _name, ThreadManager.ThreadFunctionDelegate _threadDelegate, ThreadManager.ThreadFunctionDelegate _threadInit, ThreadManager.ThreadFunctionLoopDelegate _threadLoop, ThreadManager.ThreadFunctionEndDelegate _threadEnd, object _parameter, ThreadManager.ExitCallbackThread _exitCallback, bool _useRealThread = false, bool _isSilent = false)
	{
		ThreadManager.ThreadInfo threadInfo = new ThreadManager.ThreadInfo();
		threadInfo.parameter = _parameter;
		threadInfo.threadDelegate = _threadDelegate;
		threadInfo.threadInit = _threadInit;
		threadInfo.threadLoop = _threadLoop;
		threadInfo.threadEnd = _threadEnd;
		threadInfo.exitCallback = _exitCallback;
		threadInfo.isSilent = _isSilent;
		Dictionary<string, ThreadManager.ThreadInfo> activeThreads = ThreadManager.ActiveThreads;
		lock (activeThreads)
		{
			if (ThreadManager.ActiveThreads.ContainsKey(_name))
			{
				int num = 0;
				string text;
				do
				{
					num++;
					text = _name + num.ToString();
				}
				while (ThreadManager.ActiveThreads.ContainsKey(text));
				_name = text;
			}
			ThreadManager.ActiveThreads.Add(_name, threadInfo);
		}
		threadInfo.name = _name;
		if (_useRealThread)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(ThreadManager.myThreadInvoke));
			thread.Name = _name;
			thread.Start(threadInfo);
			threadInfo.thread = thread;
		}
		else
		{
			ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(ThreadManager.myThreadInvoke), threadInfo);
		}
		return threadInfo;
	}

	// Token: 0x06009A05 RID: 39429 RVA: 0x003A7018 File Offset: 0x003A5218
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadDelegate, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, _threadDelegate, null, null, null, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x06009A06 RID: 39430 RVA: 0x003A7038 File Offset: 0x003A5238
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadInit, ThreadManager.ThreadFunctionLoopDelegate _threadLoop, ThreadManager.ThreadFunctionEndDelegate _threadEnd, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, null, _threadInit, _threadLoop, _threadEnd, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x06009A07 RID: 39431 RVA: 0x003A7058 File Offset: 0x003A5258
	[PublicizedFrom(EAccessModifier.Private)]
	public static void myThreadInvoke(object _threadInfo)
	{
		ThreadManager.ThreadInfo threadInfo = (ThreadManager.ThreadInfo)_threadInfo;
		CustomSampler.Create("ThreadDelegate", false);
		if (!threadInfo.isSilent)
		{
			Log.Out("Started thread " + threadInfo.name);
		}
		Exception ex = null;
		try
		{
			if (threadInfo.threadDelegate != null)
			{
				threadInfo.threadDelegate(threadInfo);
			}
			else
			{
				if (threadInfo.threadInit != null)
				{
					threadInfo.threadInit(threadInfo);
				}
				bool flag = false;
				try
				{
					int num;
					do
					{
						num = threadInfo.threadLoop(threadInfo);
						if (num > 0)
						{
							Thread.Sleep(num);
						}
					}
					while (num >= 0);
				}
				catch (Exception ex2)
				{
					Log.Error("Exception in thread {0}:", new object[] { threadInfo.name });
					Log.Exception(ex2);
					ex = ex2;
					flag = true;
				}
				if (threadInfo.threadEnd != null)
				{
					threadInfo.threadEnd(threadInfo, flag);
				}
			}
		}
		catch (Exception ex3)
		{
			Log.Error("Exception in thread {0}:", new object[] { threadInfo.name });
			Log.Exception(ex3);
			ex = ex3;
		}
		finally
		{
			if (!threadInfo.isSilent)
			{
				Log.Out("Exited thread " + threadInfo.name);
			}
			Dictionary<string, ThreadManager.ThreadInfo> activeThreads = ThreadManager.ActiveThreads;
			lock (activeThreads)
			{
				ThreadManager.ActiveThreads.Remove(threadInfo.name);
			}
			threadInfo.evStopped.Set();
		}
		if (threadInfo.exitCallback != null)
		{
			threadInfo.exitCallback(threadInfo, ex);
		}
		Profiler.EndThreadProfiling();
	}

	// Token: 0x06009A08 RID: 39432 RVA: 0x003A71F4 File Offset: 0x003A53F4
	public static ThreadManager.TaskInfo AddSingleTask(ThreadManager.TaskFunctionDelegate _taskDelegate, object _parameter = null, ThreadManager.ExitCallbackTask _exitCallback = null, bool _endEvent = true)
	{
		ThreadManager.TaskInfo taskInfo = new ThreadManager.TaskInfo(_endEvent);
		taskInfo.taskDelegate = _taskDelegate;
		taskInfo.parameter = _parameter;
		taskInfo.exitCallback = _exitCallback;
		taskInfo.name = _taskDelegate.Method.Name;
		ThreadPool.UnsafeQueueUserWorkItem(ThreadManager.queuedTaskDelegate, taskInfo);
		return taskInfo;
	}

	// Token: 0x06009A09 RID: 39433 RVA: 0x003A723C File Offset: 0x003A543C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void myQueuedTaskInvoke(object _taskInfo)
	{
		ThreadManager.TaskInfo taskInfo = (ThreadManager.TaskInfo)_taskInfo;
		object obj = ThreadManager.lockObjectQueuedCounter;
		lock (obj)
		{
			ThreadManager.QueuedCount++;
		}
		Exception ex = null;
		try
		{
			taskInfo.taskDelegate(taskInfo);
		}
		catch (Exception ex2)
		{
			Log.Error("Exception in task");
			Log.Exception(ex2);
			ex = ex2;
		}
		finally
		{
			if (taskInfo.evStopped != null)
			{
				taskInfo.evStopped.Set();
			}
		}
		obj = ThreadManager.lockObjectQueuedCounter;
		lock (obj)
		{
			ThreadManager.QueuedCount--;
		}
		if (taskInfo.exitCallback != null)
		{
			taskInfo.exitCallback(taskInfo, ex);
		}
		Profiler.EndThreadProfiling();
	}

	// Token: 0x06009A0A RID: 39434 RVA: 0x003A7328 File Offset: 0x003A5528
	public static void AddSingleTaskMainThread(string _name, ThreadManager.MainThreadTaskFunctionDelegate _func, object _parameter = null)
	{
		ThreadManager.MainThreadTaskInfo mainThreadTaskInfo = default(ThreadManager.MainThreadTaskInfo);
		mainThreadTaskInfo.taskDelegate = _func;
		mainThreadTaskInfo.parameter = _parameter;
		mainThreadTaskInfo.name = _name;
		object obj = ThreadManager.lockObjectMainThreadTasks;
		lock (obj)
		{
			ThreadManager.mainThreadTasks.Add(mainThreadTaskInfo);
		}
	}

	// Token: 0x06009A0B RID: 39435 RVA: 0x003A738C File Offset: 0x003A558C
	public static void AddSingleTaskMainThread(string _name, Action _func)
	{
		ThreadManager.AddSingleTaskMainThread(_name, delegate(object _)
		{
			_func();
		}, null);
	}

	// Token: 0x06009A0C RID: 39436 RVA: 0x003A73B9 File Offset: 0x003A55B9
	public static void RunTaskAfterFrames(Action _func, int _inFrames = 1)
	{
		ThreadManager.StartCoroutine(ThreadManager.runAfterFrames(_func, _inFrames));
	}

	// Token: 0x06009A0D RID: 39437 RVA: 0x003A73C8 File Offset: 0x003A55C8
	public static void UpdateMainThreadTasks()
	{
		Action updateEv = ThreadManager.UpdateEv;
		if (updateEv != null)
		{
			updateEv();
		}
		try
		{
			ThreadManager.MainThreadScheduler.ProcessTasks();
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		int num = ThreadManager.mainThreadTasks.Count;
		if (num == 0)
		{
			return;
		}
		object obj = ThreadManager.lockObjectMainThreadTasks;
		lock (obj)
		{
			List<ThreadManager.MainThreadTaskInfo> list = ThreadManager.mainThreadTasks;
			ThreadManager.mainThreadTasks = ThreadManager.mainThreadTasksCopy;
			ThreadManager.mainThreadTasksCopy = list;
		}
		num = ThreadManager.mainThreadTasksCopy.Count;
		for (int i = 0; i < num; i++)
		{
			try
			{
				ThreadManager.MainThreadTaskInfo mainThreadTaskInfo = ThreadManager.mainThreadTasksCopy[i];
				mainThreadTaskInfo.taskDelegate(mainThreadTaskInfo.parameter);
			}
			catch (Exception ex2)
			{
				Log.Exception(ex2);
			}
		}
		ThreadManager.mainThreadTasksCopy.Clear();
	}

	// Token: 0x06009A0E RID: 39438 RVA: 0x003A74AC File Offset: 0x003A56AC
	public static void LateUpdate()
	{
		Action lateUpdateEv = ThreadManager.LateUpdateEv;
		if (lateUpdateEv == null)
		{
			return;
		}
		lateUpdateEv();
	}

	// Token: 0x06009A0F RID: 39439 RVA: 0x003A74C0 File Offset: 0x003A56C0
	public static void Shutdown()
	{
		Log.Out("Terminating threads");
		using (Dictionary<string, ThreadManager.ThreadInfo>.Enumerator enumerator = ThreadManager.ActiveThreads.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, ThreadManager.ThreadInfo> keyValuePair = enumerator.Current;
				keyValuePair.Value.RequestTermination();
			}
			goto IL_0082;
		}
		IL_0044:
		using (Dictionary<string, ThreadManager.ThreadInfo>.Enumerator enumerator = ThreadManager.ActiveThreads.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<string, ThreadManager.ThreadInfo> keyValuePair2 = enumerator.Current;
				keyValuePair2.Value.WaitForEnd(30);
			}
		}
		IL_0082:
		if (ThreadManager.ActiveThreads.Count <= 0)
		{
			return;
		}
		goto IL_0044;
	}

	// Token: 0x06009A10 RID: 39440 RVA: 0x003A7578 File Offset: 0x003A5778
	public static void SetMonoBehaviour(MonoBehaviour _monoBehaviour)
	{
		ThreadManager.monoBehaviour = _monoBehaviour;
	}

	// Token: 0x06009A11 RID: 39441 RVA: 0x003A7580 File Offset: 0x003A5780
	public static Coroutine StartCoroutine(IEnumerator _e)
	{
		if (!ThreadManager.IsMainThread())
		{
			ThreadManager.AddSingleTaskMainThread("Coroutine", delegate(object _taskInfo)
			{
				ThreadManager.StartCoroutine(_e);
			}, null);
			return null;
		}
		if (ThreadManager.monoBehaviour != null)
		{
			return ThreadManager.monoBehaviour.StartCoroutine(_e);
		}
		return null;
	}

	// Token: 0x06009A12 RID: 39442 RVA: 0x003A75D9 File Offset: 0x003A57D9
	public static void StopCoroutine(IEnumerator _e)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_e);
	}

	// Token: 0x06009A13 RID: 39443 RVA: 0x003A75F4 File Offset: 0x003A57F4
	public static void StopCoroutine(Coroutine _coroutine)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_coroutine);
	}

	// Token: 0x06009A14 RID: 39444 RVA: 0x003A760F File Offset: 0x003A580F
	public static void StopCoroutine(string _methodName)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_methodName);
	}

	// Token: 0x06009A15 RID: 39445 RVA: 0x003A762C File Offset: 0x003A582C
	public static void RunCoroutine(IEnumerator _e, Action _iterCallback)
	{
		while (_e.MoveNext())
		{
			object obj = _e.Current;
			IEnumerator enumerator = obj as IEnumerator;
			if (enumerator != null)
			{
				ThreadManager.RunCoroutine(enumerator, _iterCallback);
			}
			else
			{
				_iterCallback();
			}
		}
	}

	// Token: 0x17001270 RID: 4720
	// (get) Token: 0x06009A16 RID: 39446 RVA: 0x003A7661 File Offset: 0x003A5861
	public static bool IsInSyncCoroutine
	{
		get
		{
			return ThreadManager.syncCoroutineNestingLevel > 0;
		}
	}

	// Token: 0x06009A17 RID: 39447 RVA: 0x003A766C File Offset: 0x003A586C
	public static void RunCoroutineSync(IEnumerator _func)
	{
		ThreadManager.syncCoroutineNestingLevel++;
		try
		{
			while (_func.MoveNext())
			{
				object obj = _func.Current;
				IEnumerator enumerator = obj as IEnumerator;
				if (enumerator != null)
				{
					ThreadManager.RunCoroutineSync(enumerator);
				}
			}
		}
		finally
		{
			ThreadManager.syncCoroutineNestingLevel--;
		}
	}

	// Token: 0x06009A18 RID: 39448 RVA: 0x003A76C4 File Offset: 0x003A58C4
	public static IEnumerator CoroutineWrapperWithExceptionCallback(IEnumerator _enumerator, Action<Exception> _exceptionHandler)
	{
		Stack<IEnumerator> stack = new Stack<IEnumerator>();
		stack.Push(_enumerator);
		while (stack.Count > 0)
		{
			IEnumerator enumerator = stack.Peek();
			object obj;
			try
			{
				if (!enumerator.MoveNext())
				{
					stack.Pop();
					continue;
				}
				obj = enumerator.Current;
			}
			catch (Exception ex)
			{
				_exceptionHandler(ex);
				yield break;
			}
			IEnumerator enumerator2 = obj as IEnumerator;
			if (enumerator2 != null)
			{
				stack.Push(enumerator2);
			}
			else
			{
				yield return obj;
			}
		}
		yield break;
	}

	// Token: 0x06009A19 RID: 39449 RVA: 0x003A76DA File Offset: 0x003A58DA
	public static void SetMainThreadRef(Thread _mainThreadRef)
	{
		ThreadManager.MainThreadRef = _mainThreadRef;
		ThreadManager.MainThreadId = _mainThreadRef.ManagedThreadId;
	}

	// Token: 0x06009A1A RID: 39450 RVA: 0x003A76ED File Offset: 0x003A58ED
	public static bool IsMainThread()
	{
		return Thread.CurrentThread.ManagedThreadId == ThreadManager.MainThreadId;
	}

	// Token: 0x06009A1B RID: 39451 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string _messagePart1, string _messagePart2 = null)
	{
	}

	// Token: 0x06009A1C RID: 39452 RVA: 0x003A7700 File Offset: 0x003A5900
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator runAfterFrames(Action _func, int _inFrames)
	{
		int num;
		for (int i = 0; i < _inFrames; i = num + 1)
		{
			yield return null;
			num = i;
		}
		if (_func != null)
		{
			_func();
		}
		yield break;
	}

	// Token: 0x04007408 RID: 29704
	public const int cEndTime = -1;

	// Token: 0x04007409 RID: 29705
	[PublicizedFrom(EAccessModifier.Private)]
	public const int threadTerminationTimeout = 30;

	// Token: 0x0400740A RID: 29706
	public static Thread MainThreadRef;

	// Token: 0x0400740B RID: 29707
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MainThreadId;

	// Token: 0x0400740C RID: 29708
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ManualTaskScheduler MainThreadScheduler = new ManualTaskScheduler();

	// Token: 0x0400740F RID: 29711
	public static Dictionary<string, ThreadManager.ThreadInfo> ActiveThreads = new Dictionary<string, ThreadManager.ThreadInfo>();

	// Token: 0x04007410 RID: 29712
	public static int QueuedCount;

	// Token: 0x04007411 RID: 29713
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectQueuedCounter = new object();

	// Token: 0x04007412 RID: 29714
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectMainThreadTasks = new object();

	// Token: 0x04007413 RID: 29715
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasks = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x04007414 RID: 29716
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasksCopy = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x04007415 RID: 29717
	[PublicizedFrom(EAccessModifier.Private)]
	public static MonoBehaviour monoBehaviour;

	// Token: 0x04007416 RID: 29718
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitCallback queuedTaskDelegate = new WaitCallback(ThreadManager.myQueuedTaskInvoke);

	// Token: 0x04007417 RID: 29719
	[PublicizedFrom(EAccessModifier.Private)]
	public static int syncCoroutineNestingLevel;

	// Token: 0x02001302 RID: 4866
	public class ThreadInfo
	{
		// Token: 0x06009A1D RID: 39453 RVA: 0x003A7716 File Offset: 0x003A5916
		public void RequestTermination()
		{
			this.evRunning.Set();
		}

		// Token: 0x06009A1E RID: 39454 RVA: 0x003A7724 File Offset: 0x003A5924
		public bool TerminationRequested()
		{
			return this.evRunning.WaitOne(0);
		}

		// Token: 0x06009A1F RID: 39455 RVA: 0x003A7732 File Offset: 0x003A5932
		public bool HasTerminated()
		{
			return this.evStopped.WaitOne(0);
		}

		// Token: 0x06009A20 RID: 39456 RVA: 0x003A7740 File Offset: 0x003A5940
		public void WaitForEnd(int timeout = 30)
		{
			this.RequestTermination();
			if (!this.evStopped.WaitOne(timeout * 1000))
			{
				Log.Error(string.Concat(new string[]
				{
					"Thread ",
					this.name,
					" did not finish within ",
					timeout.ToString(),
					"s. Request trace: ",
					StackTraceUtility.ExtractStackTrace()
				}));
				Thread thread = this.thread;
				if (thread == null)
				{
					return;
				}
				thread.Abort();
			}
		}

		// Token: 0x04007418 RID: 29720
		public object parameter;

		// Token: 0x04007419 RID: 29721
		public ThreadManager.ThreadFunctionDelegate threadDelegate;

		// Token: 0x0400741A RID: 29722
		public ThreadManager.ThreadFunctionDelegate threadInit;

		// Token: 0x0400741B RID: 29723
		public ThreadManager.ThreadFunctionLoopDelegate threadLoop;

		// Token: 0x0400741C RID: 29724
		public ThreadManager.ThreadFunctionEndDelegate threadEnd;

		// Token: 0x0400741D RID: 29725
		public string name;

		// Token: 0x0400741E RID: 29726
		public Thread thread;

		// Token: 0x0400741F RID: 29727
		public bool isSilent;

		// Token: 0x04007420 RID: 29728
		public readonly ManualResetEvent evRunning = new ManualResetEvent(false);

		// Token: 0x04007421 RID: 29729
		public readonly ManualResetEvent evStopped = new ManualResetEvent(false);

		// Token: 0x04007422 RID: 29730
		public ThreadManager.ExitCallbackThread exitCallback;

		// Token: 0x04007423 RID: 29731
		public object threadData;
	}

	// Token: 0x02001303 RID: 4867
	public class TaskInfo
	{
		// Token: 0x06009A22 RID: 39458 RVA: 0x003A77DA File Offset: 0x003A59DA
		public TaskInfo(bool _endEvent = true)
		{
			if (_endEvent)
			{
				this.evStopped = new ManualResetEvent(false);
			}
		}

		// Token: 0x06009A23 RID: 39459 RVA: 0x003A77F1 File Offset: 0x003A59F1
		public void WaitForEnd()
		{
			this.evStopped.WaitOne();
		}

		// Token: 0x04007424 RID: 29732
		public string name;

		// Token: 0x04007425 RID: 29733
		public ThreadManager.TaskFunctionDelegate taskDelegate;

		// Token: 0x04007426 RID: 29734
		public object parameter;

		// Token: 0x04007427 RID: 29735
		public ThreadManager.ExitCallbackTask exitCallback;

		// Token: 0x04007428 RID: 29736
		public ManualResetEvent evStopped;
	}

	// Token: 0x02001304 RID: 4868
	public struct MainThreadTaskInfo
	{
		// Token: 0x04007429 RID: 29737
		public string name;

		// Token: 0x0400742A RID: 29738
		public ThreadManager.MainThreadTaskFunctionDelegate taskDelegate;

		// Token: 0x0400742B RID: 29739
		public object parameter;
	}

	// Token: 0x02001305 RID: 4869
	// (Invoke) Token: 0x06009A25 RID: 39461
	public delegate int ThreadFunctionLoopDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x02001306 RID: 4870
	// (Invoke) Token: 0x06009A29 RID: 39465
	public delegate void ThreadFunctionEndDelegate(ThreadManager.ThreadInfo _threadInfo, bool _exitForException);

	// Token: 0x02001307 RID: 4871
	// (Invoke) Token: 0x06009A2D RID: 39469
	public delegate void ThreadFunctionDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x02001308 RID: 4872
	// (Invoke) Token: 0x06009A31 RID: 39473
	public delegate void TaskFunctionDelegate(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x02001309 RID: 4873
	// (Invoke) Token: 0x06009A35 RID: 39477
	public delegate void MainThreadTaskFunctionDelegate(object _parameter);

	// Token: 0x0200130A RID: 4874
	// (Invoke) Token: 0x06009A39 RID: 39481
	public delegate void ExitCallbackThread(ThreadManager.ThreadInfo _ti, Exception _e);

	// Token: 0x0200130B RID: 4875
	// (Invoke) Token: 0x06009A3D RID: 39485
	public delegate void ExitCallbackTask(ThreadManager.TaskInfo _ti, Exception _e);
}
