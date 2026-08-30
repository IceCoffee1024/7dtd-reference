using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020012C7 RID: 4807
public static class ThreadManager
{
	// Token: 0x140000FE RID: 254
	// (add) Token: 0x06009857 RID: 38999 RVA: 0x0039D018 File Offset: 0x0039B218
	// (remove) Token: 0x06009858 RID: 39000 RVA: 0x0039D04C File Offset: 0x0039B24C
	public static event Action UpdateEv;

	// Token: 0x140000FF RID: 255
	// (add) Token: 0x06009859 RID: 39001 RVA: 0x0039D080 File Offset: 0x0039B280
	// (remove) Token: 0x0600985A RID: 39002 RVA: 0x0039D0B4 File Offset: 0x0039B2B4
	public static event Action LateUpdateEv;

	// Token: 0x0600985B RID: 39003 RVA: 0x0039D0E7 File Offset: 0x0039B2E7
	public static void ReleaseTaskInfo(ThreadManager.TaskInfo _info)
	{
		if (((_info != null) ? _info.evStopped : null) != null)
		{
			_info.evStopped.Close();
			_info.evStopped = null;
		}
	}

	// Token: 0x170011F1 RID: 4593
	// (get) Token: 0x0600985C RID: 39004 RVA: 0x0039D109 File Offset: 0x0039B309
	public static TaskFactory MainThreadFactory
	{
		get
		{
			return ThreadManager.MainThreadScheduler.Factory;
		}
	}

	// Token: 0x0600985D RID: 39005 RVA: 0x0039D118 File Offset: 0x0039B318
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

	// Token: 0x0600985E RID: 39006 RVA: 0x0039D218 File Offset: 0x0039B418
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadDelegate, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, _threadDelegate, null, null, null, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x0600985F RID: 39007 RVA: 0x0039D238 File Offset: 0x0039B438
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadInit, ThreadManager.ThreadFunctionLoopDelegate _threadLoop, ThreadManager.ThreadFunctionEndDelegate _threadEnd, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, null, _threadInit, _threadLoop, _threadEnd, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x06009860 RID: 39008 RVA: 0x0039D258 File Offset: 0x0039B458
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

	// Token: 0x06009861 RID: 39009 RVA: 0x0039D3F4 File Offset: 0x0039B5F4
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

	// Token: 0x06009862 RID: 39010 RVA: 0x0039D43C File Offset: 0x0039B63C
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

	// Token: 0x06009863 RID: 39011 RVA: 0x0039D528 File Offset: 0x0039B728
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

	// Token: 0x06009864 RID: 39012 RVA: 0x0039D58C File Offset: 0x0039B78C
	public static void AddSingleTaskMainThread(string _name, Action _func)
	{
		ThreadManager.AddSingleTaskMainThread(_name, delegate(object _)
		{
			_func();
		}, null);
	}

	// Token: 0x06009865 RID: 39013 RVA: 0x0039D5B9 File Offset: 0x0039B7B9
	public static void RunTaskAfterFrames(Action _func, int _inFrames = 1)
	{
		ThreadManager.StartCoroutine(ThreadManager.runAfterFrames(_func, _inFrames));
	}

	// Token: 0x06009866 RID: 39014 RVA: 0x0039D5C8 File Offset: 0x0039B7C8
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

	// Token: 0x06009867 RID: 39015 RVA: 0x0039D6AC File Offset: 0x0039B8AC
	public static void LateUpdate()
	{
		Action lateUpdateEv = ThreadManager.LateUpdateEv;
		if (lateUpdateEv == null)
		{
			return;
		}
		lateUpdateEv();
	}

	// Token: 0x06009868 RID: 39016 RVA: 0x0039D6C0 File Offset: 0x0039B8C0
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

	// Token: 0x06009869 RID: 39017 RVA: 0x0039D778 File Offset: 0x0039B978
	public static void SetMonoBehaviour(MonoBehaviour _monoBehaviour)
	{
		ThreadManager.monoBehaviour = _monoBehaviour;
	}

	// Token: 0x0600986A RID: 39018 RVA: 0x0039D780 File Offset: 0x0039B980
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

	// Token: 0x0600986B RID: 39019 RVA: 0x0039D7D9 File Offset: 0x0039B9D9
	public static void StopCoroutine(IEnumerator _e)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_e);
	}

	// Token: 0x0600986C RID: 39020 RVA: 0x0039D7F4 File Offset: 0x0039B9F4
	public static void StopCoroutine(Coroutine _coroutine)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_coroutine);
	}

	// Token: 0x0600986D RID: 39021 RVA: 0x0039D80F File Offset: 0x0039BA0F
	public static void StopCoroutine(string _methodName)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_methodName);
	}

	// Token: 0x0600986E RID: 39022 RVA: 0x0039D82C File Offset: 0x0039BA2C
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

	// Token: 0x170011F2 RID: 4594
	// (get) Token: 0x0600986F RID: 39023 RVA: 0x0039D861 File Offset: 0x0039BA61
	public static bool IsInSyncCoroutine
	{
		get
		{
			return ThreadManager.syncCoroutineNestingLevel > 0;
		}
	}

	// Token: 0x06009870 RID: 39024 RVA: 0x0039D86C File Offset: 0x0039BA6C
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

	// Token: 0x06009871 RID: 39025 RVA: 0x0039D8C4 File Offset: 0x0039BAC4
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

	// Token: 0x06009872 RID: 39026 RVA: 0x0039D8DA File Offset: 0x0039BADA
	public static void SetMainThreadRef(Thread _mainThreadRef)
	{
		ThreadManager.MainThreadRef = _mainThreadRef;
		ThreadManager.MainThreadId = _mainThreadRef.ManagedThreadId;
	}

	// Token: 0x06009873 RID: 39027 RVA: 0x0039D8ED File Offset: 0x0039BAED
	public static bool IsMainThread()
	{
		return Thread.CurrentThread.ManagedThreadId == ThreadManager.MainThreadId;
	}

	// Token: 0x06009874 RID: 39028 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string _messagePart1, string _messagePart2 = null)
	{
	}

	// Token: 0x06009875 RID: 39029 RVA: 0x0039D900 File Offset: 0x0039BB00
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

	// Token: 0x040072B6 RID: 29366
	public const int cEndTime = -1;

	// Token: 0x040072B7 RID: 29367
	[PublicizedFrom(EAccessModifier.Private)]
	public const int threadTerminationTimeout = 30;

	// Token: 0x040072B8 RID: 29368
	public static Thread MainThreadRef;

	// Token: 0x040072B9 RID: 29369
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MainThreadId;

	// Token: 0x040072BA RID: 29370
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ManualTaskScheduler MainThreadScheduler = new ManualTaskScheduler();

	// Token: 0x040072BD RID: 29373
	public static Dictionary<string, ThreadManager.ThreadInfo> ActiveThreads = new Dictionary<string, ThreadManager.ThreadInfo>();

	// Token: 0x040072BE RID: 29374
	public static int QueuedCount;

	// Token: 0x040072BF RID: 29375
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectQueuedCounter = new object();

	// Token: 0x040072C0 RID: 29376
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectMainThreadTasks = new object();

	// Token: 0x040072C1 RID: 29377
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasks = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x040072C2 RID: 29378
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasksCopy = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x040072C3 RID: 29379
	[PublicizedFrom(EAccessModifier.Private)]
	public static MonoBehaviour monoBehaviour;

	// Token: 0x040072C4 RID: 29380
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitCallback queuedTaskDelegate = new WaitCallback(ThreadManager.myQueuedTaskInvoke);

	// Token: 0x040072C5 RID: 29381
	[PublicizedFrom(EAccessModifier.Private)]
	public static int syncCoroutineNestingLevel;

	// Token: 0x020012C8 RID: 4808
	public class ThreadInfo
	{
		// Token: 0x06009876 RID: 39030 RVA: 0x0039D916 File Offset: 0x0039BB16
		public void RequestTermination()
		{
			this.evRunning.Set();
		}

		// Token: 0x06009877 RID: 39031 RVA: 0x0039D924 File Offset: 0x0039BB24
		public bool TerminationRequested()
		{
			return this.evRunning.WaitOne(0);
		}

		// Token: 0x06009878 RID: 39032 RVA: 0x0039D932 File Offset: 0x0039BB32
		public bool HasTerminated()
		{
			return this.evStopped.WaitOne(0);
		}

		// Token: 0x06009879 RID: 39033 RVA: 0x0039D940 File Offset: 0x0039BB40
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

		// Token: 0x040072C6 RID: 29382
		public object parameter;

		// Token: 0x040072C7 RID: 29383
		public ThreadManager.ThreadFunctionDelegate threadDelegate;

		// Token: 0x040072C8 RID: 29384
		public ThreadManager.ThreadFunctionDelegate threadInit;

		// Token: 0x040072C9 RID: 29385
		public ThreadManager.ThreadFunctionLoopDelegate threadLoop;

		// Token: 0x040072CA RID: 29386
		public ThreadManager.ThreadFunctionEndDelegate threadEnd;

		// Token: 0x040072CB RID: 29387
		public string name;

		// Token: 0x040072CC RID: 29388
		public Thread thread;

		// Token: 0x040072CD RID: 29389
		public bool isSilent;

		// Token: 0x040072CE RID: 29390
		public readonly ManualResetEvent evRunning = new ManualResetEvent(false);

		// Token: 0x040072CF RID: 29391
		public readonly ManualResetEvent evStopped = new ManualResetEvent(false);

		// Token: 0x040072D0 RID: 29392
		public ThreadManager.ExitCallbackThread exitCallback;

		// Token: 0x040072D1 RID: 29393
		public object threadData;
	}

	// Token: 0x020012C9 RID: 4809
	public class TaskInfo
	{
		// Token: 0x0600987B RID: 39035 RVA: 0x0039D9DA File Offset: 0x0039BBDA
		public TaskInfo(bool _endEvent = true)
		{
			if (_endEvent)
			{
				this.evStopped = new ManualResetEvent(false);
			}
		}

		// Token: 0x0600987C RID: 39036 RVA: 0x0039D9F1 File Offset: 0x0039BBF1
		public void WaitForEnd()
		{
			this.evStopped.WaitOne();
		}

		// Token: 0x040072D2 RID: 29394
		public string name;

		// Token: 0x040072D3 RID: 29395
		public ThreadManager.TaskFunctionDelegate taskDelegate;

		// Token: 0x040072D4 RID: 29396
		public object parameter;

		// Token: 0x040072D5 RID: 29397
		public ThreadManager.ExitCallbackTask exitCallback;

		// Token: 0x040072D6 RID: 29398
		public ManualResetEvent evStopped;
	}

	// Token: 0x020012CA RID: 4810
	public struct MainThreadTaskInfo
	{
		// Token: 0x040072D7 RID: 29399
		public string name;

		// Token: 0x040072D8 RID: 29400
		public ThreadManager.MainThreadTaskFunctionDelegate taskDelegate;

		// Token: 0x040072D9 RID: 29401
		public object parameter;
	}

	// Token: 0x020012CB RID: 4811
	// (Invoke) Token: 0x0600987E RID: 39038
	public delegate int ThreadFunctionLoopDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x020012CC RID: 4812
	// (Invoke) Token: 0x06009882 RID: 39042
	public delegate void ThreadFunctionEndDelegate(ThreadManager.ThreadInfo _threadInfo, bool _exitForException);

	// Token: 0x020012CD RID: 4813
	// (Invoke) Token: 0x06009886 RID: 39046
	public delegate void ThreadFunctionDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x020012CE RID: 4814
	// (Invoke) Token: 0x0600988A RID: 39050
	public delegate void TaskFunctionDelegate(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x020012CF RID: 4815
	// (Invoke) Token: 0x0600988E RID: 39054
	public delegate void MainThreadTaskFunctionDelegate(object _parameter);

	// Token: 0x020012D0 RID: 4816
	// (Invoke) Token: 0x06009892 RID: 39058
	public delegate void ExitCallbackThread(ThreadManager.ThreadInfo _ti, Exception _e);

	// Token: 0x020012D1 RID: 4817
	// (Invoke) Token: 0x06009896 RID: 39062
	public delegate void ExitCallbackTask(ThreadManager.TaskInfo _ti, Exception _e);
}
