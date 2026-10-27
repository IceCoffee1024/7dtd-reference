using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x02001313 RID: 4883
public static class ThreadManager
{
	// Token: 0x140000FE RID: 254
	// (add) Token: 0x06009A56 RID: 39510 RVA: 0x003A921C File Offset: 0x003A741C
	// (remove) Token: 0x06009A57 RID: 39511 RVA: 0x003A9250 File Offset: 0x003A7450
	public static event Action UpdateEv;

	// Token: 0x140000FF RID: 255
	// (add) Token: 0x06009A58 RID: 39512 RVA: 0x003A9284 File Offset: 0x003A7484
	// (remove) Token: 0x06009A59 RID: 39513 RVA: 0x003A92B8 File Offset: 0x003A74B8
	public static event Action LateUpdateEv;

	// Token: 0x06009A5A RID: 39514 RVA: 0x003A92EB File Offset: 0x003A74EB
	public static void ReleaseTaskInfo(ThreadManager.TaskInfo _info)
	{
		if (((_info != null) ? _info.evStopped : null) != null)
		{
			_info.evStopped.Close();
			_info.evStopped = null;
		}
	}

	// Token: 0x1700127B RID: 4731
	// (get) Token: 0x06009A5B RID: 39515 RVA: 0x003A930D File Offset: 0x003A750D
	public static TaskFactory MainThreadFactory
	{
		get
		{
			return ThreadManager.MainThreadScheduler.Factory;
		}
	}

	// Token: 0x06009A5C RID: 39516 RVA: 0x003A931C File Offset: 0x003A751C
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

	// Token: 0x06009A5D RID: 39517 RVA: 0x003A941C File Offset: 0x003A761C
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadDelegate, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, _threadDelegate, null, null, null, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x06009A5E RID: 39518 RVA: 0x003A943C File Offset: 0x003A763C
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadInit, ThreadManager.ThreadFunctionLoopDelegate _threadLoop, ThreadManager.ThreadFunctionEndDelegate _threadEnd, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, null, _threadInit, _threadLoop, _threadEnd, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x06009A5F RID: 39519 RVA: 0x003A945C File Offset: 0x003A765C
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

	// Token: 0x06009A60 RID: 39520 RVA: 0x003A95F8 File Offset: 0x003A77F8
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

	// Token: 0x06009A61 RID: 39521 RVA: 0x003A9640 File Offset: 0x003A7840
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

	// Token: 0x06009A62 RID: 39522 RVA: 0x003A972C File Offset: 0x003A792C
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

	// Token: 0x06009A63 RID: 39523 RVA: 0x003A9790 File Offset: 0x003A7990
	public static void AddSingleTaskMainThread(string _name, Action _func)
	{
		ThreadManager.AddSingleTaskMainThread(_name, delegate(object _)
		{
			_func();
		}, null);
	}

	// Token: 0x06009A64 RID: 39524 RVA: 0x003A97BD File Offset: 0x003A79BD
	public static void RunTaskAfterFrames(Action _func, int _inFrames = 1)
	{
		ThreadManager.StartCoroutine(ThreadManager.runAfterFrames(_func, _inFrames));
	}

	// Token: 0x06009A65 RID: 39525 RVA: 0x003A97CC File Offset: 0x003A79CC
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

	// Token: 0x06009A66 RID: 39526 RVA: 0x003A98B0 File Offset: 0x003A7AB0
	public static void LateUpdate()
	{
		Action lateUpdateEv = ThreadManager.LateUpdateEv;
		if (lateUpdateEv == null)
		{
			return;
		}
		lateUpdateEv();
	}

	// Token: 0x06009A67 RID: 39527 RVA: 0x003A98C4 File Offset: 0x003A7AC4
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

	// Token: 0x06009A68 RID: 39528 RVA: 0x003A997C File Offset: 0x003A7B7C
	public static void SetMonoBehaviour(MonoBehaviour _monoBehaviour)
	{
		ThreadManager.monoBehaviour = _monoBehaviour;
	}

	// Token: 0x06009A69 RID: 39529 RVA: 0x003A9984 File Offset: 0x003A7B84
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

	// Token: 0x06009A6A RID: 39530 RVA: 0x003A99DD File Offset: 0x003A7BDD
	public static void StopCoroutine(IEnumerator _e)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_e);
	}

	// Token: 0x06009A6B RID: 39531 RVA: 0x003A99F8 File Offset: 0x003A7BF8
	public static void StopCoroutine(Coroutine _coroutine)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_coroutine);
	}

	// Token: 0x06009A6C RID: 39532 RVA: 0x003A9A13 File Offset: 0x003A7C13
	public static void StopCoroutine(string _methodName)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_methodName);
	}

	// Token: 0x06009A6D RID: 39533 RVA: 0x003A9A30 File Offset: 0x003A7C30
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

	// Token: 0x1700127C RID: 4732
	// (get) Token: 0x06009A6E RID: 39534 RVA: 0x003A9A65 File Offset: 0x003A7C65
	public static bool IsInSyncCoroutine
	{
		get
		{
			return ThreadManager.syncCoroutineNestingLevel > 0;
		}
	}

	// Token: 0x06009A6F RID: 39535 RVA: 0x003A9A70 File Offset: 0x003A7C70
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

	// Token: 0x06009A70 RID: 39536 RVA: 0x003A9AC8 File Offset: 0x003A7CC8
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

	// Token: 0x06009A71 RID: 39537 RVA: 0x003A9ADE File Offset: 0x003A7CDE
	public static void SetMainThreadRef(Thread _mainThreadRef)
	{
		ThreadManager.MainThreadRef = _mainThreadRef;
		ThreadManager.MainThreadId = _mainThreadRef.ManagedThreadId;
	}

	// Token: 0x06009A72 RID: 39538 RVA: 0x003A9AF1 File Offset: 0x003A7CF1
	public static bool IsMainThread()
	{
		return Thread.CurrentThread.ManagedThreadId == ThreadManager.MainThreadId;
	}

	// Token: 0x06009A73 RID: 39539 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string _messagePart1, string _messagePart2 = null)
	{
	}

	// Token: 0x06009A74 RID: 39540 RVA: 0x003A9B04 File Offset: 0x003A7D04
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

	// Token: 0x04007449 RID: 29769
	public const int cEndTime = -1;

	// Token: 0x0400744A RID: 29770
	[PublicizedFrom(EAccessModifier.Private)]
	public const int threadTerminationTimeout = 30;

	// Token: 0x0400744B RID: 29771
	public static Thread MainThreadRef;

	// Token: 0x0400744C RID: 29772
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MainThreadId;

	// Token: 0x0400744D RID: 29773
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ManualTaskScheduler MainThreadScheduler = new ManualTaskScheduler();

	// Token: 0x04007450 RID: 29776
	public static Dictionary<string, ThreadManager.ThreadInfo> ActiveThreads = new Dictionary<string, ThreadManager.ThreadInfo>();

	// Token: 0x04007451 RID: 29777
	public static int QueuedCount;

	// Token: 0x04007452 RID: 29778
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectQueuedCounter = new object();

	// Token: 0x04007453 RID: 29779
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectMainThreadTasks = new object();

	// Token: 0x04007454 RID: 29780
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasks = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x04007455 RID: 29781
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasksCopy = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x04007456 RID: 29782
	[PublicizedFrom(EAccessModifier.Private)]
	public static MonoBehaviour monoBehaviour;

	// Token: 0x04007457 RID: 29783
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitCallback queuedTaskDelegate = new WaitCallback(ThreadManager.myQueuedTaskInvoke);

	// Token: 0x04007458 RID: 29784
	[PublicizedFrom(EAccessModifier.Private)]
	public static int syncCoroutineNestingLevel;

	// Token: 0x02001314 RID: 4884
	public class ThreadInfo
	{
		// Token: 0x06009A75 RID: 39541 RVA: 0x003A9B1A File Offset: 0x003A7D1A
		public void RequestTermination()
		{
			this.evRunning.Set();
		}

		// Token: 0x06009A76 RID: 39542 RVA: 0x003A9B28 File Offset: 0x003A7D28
		public bool TerminationRequested()
		{
			return this.evRunning.WaitOne(0);
		}

		// Token: 0x06009A77 RID: 39543 RVA: 0x003A9B36 File Offset: 0x003A7D36
		public bool HasTerminated()
		{
			return this.evStopped.WaitOne(0);
		}

		// Token: 0x06009A78 RID: 39544 RVA: 0x003A9B44 File Offset: 0x003A7D44
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

		// Token: 0x04007459 RID: 29785
		public object parameter;

		// Token: 0x0400745A RID: 29786
		public ThreadManager.ThreadFunctionDelegate threadDelegate;

		// Token: 0x0400745B RID: 29787
		public ThreadManager.ThreadFunctionDelegate threadInit;

		// Token: 0x0400745C RID: 29788
		public ThreadManager.ThreadFunctionLoopDelegate threadLoop;

		// Token: 0x0400745D RID: 29789
		public ThreadManager.ThreadFunctionEndDelegate threadEnd;

		// Token: 0x0400745E RID: 29790
		public string name;

		// Token: 0x0400745F RID: 29791
		public Thread thread;

		// Token: 0x04007460 RID: 29792
		public bool isSilent;

		// Token: 0x04007461 RID: 29793
		public readonly ManualResetEvent evRunning = new ManualResetEvent(false);

		// Token: 0x04007462 RID: 29794
		public readonly ManualResetEvent evStopped = new ManualResetEvent(false);

		// Token: 0x04007463 RID: 29795
		public ThreadManager.ExitCallbackThread exitCallback;

		// Token: 0x04007464 RID: 29796
		public object threadData;
	}

	// Token: 0x02001315 RID: 4885
	public class TaskInfo
	{
		// Token: 0x06009A7A RID: 39546 RVA: 0x003A9BDE File Offset: 0x003A7DDE
		public TaskInfo(bool _endEvent = true)
		{
			if (_endEvent)
			{
				this.evStopped = new ManualResetEvent(false);
			}
		}

		// Token: 0x06009A7B RID: 39547 RVA: 0x003A9BF5 File Offset: 0x003A7DF5
		public void WaitForEnd()
		{
			this.evStopped.WaitOne();
		}

		// Token: 0x04007465 RID: 29797
		public string name;

		// Token: 0x04007466 RID: 29798
		public ThreadManager.TaskFunctionDelegate taskDelegate;

		// Token: 0x04007467 RID: 29799
		public object parameter;

		// Token: 0x04007468 RID: 29800
		public ThreadManager.ExitCallbackTask exitCallback;

		// Token: 0x04007469 RID: 29801
		public ManualResetEvent evStopped;
	}

	// Token: 0x02001316 RID: 4886
	public struct MainThreadTaskInfo
	{
		// Token: 0x0400746A RID: 29802
		public string name;

		// Token: 0x0400746B RID: 29803
		public ThreadManager.MainThreadTaskFunctionDelegate taskDelegate;

		// Token: 0x0400746C RID: 29804
		public object parameter;
	}

	// Token: 0x02001317 RID: 4887
	// (Invoke) Token: 0x06009A7D RID: 39549
	public delegate int ThreadFunctionLoopDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x02001318 RID: 4888
	// (Invoke) Token: 0x06009A81 RID: 39553
	public delegate void ThreadFunctionEndDelegate(ThreadManager.ThreadInfo _threadInfo, bool _exitForException);

	// Token: 0x02001319 RID: 4889
	// (Invoke) Token: 0x06009A85 RID: 39557
	public delegate void ThreadFunctionDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x0200131A RID: 4890
	// (Invoke) Token: 0x06009A89 RID: 39561
	public delegate void TaskFunctionDelegate(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x0200131B RID: 4891
	// (Invoke) Token: 0x06009A8D RID: 39565
	public delegate void MainThreadTaskFunctionDelegate(object _parameter);

	// Token: 0x0200131C RID: 4892
	// (Invoke) Token: 0x06009A91 RID: 39569
	public delegate void ExitCallbackThread(ThreadManager.ThreadInfo _ti, Exception _e);

	// Token: 0x0200131D RID: 4893
	// (Invoke) Token: 0x06009A95 RID: 39573
	public delegate void ExitCallbackTask(ThreadManager.TaskInfo _ti, Exception _e);
}
