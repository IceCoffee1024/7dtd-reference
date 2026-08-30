using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020012E1 RID: 4833
public static class ThreadManager
{
	// Token: 0x140000FF RID: 255
	// (add) Token: 0x06009952 RID: 39250 RVA: 0x003A2268 File Offset: 0x003A0468
	// (remove) Token: 0x06009953 RID: 39251 RVA: 0x003A229C File Offset: 0x003A049C
	public static event Action UpdateEv;

	// Token: 0x14000100 RID: 256
	// (add) Token: 0x06009954 RID: 39252 RVA: 0x003A22D0 File Offset: 0x003A04D0
	// (remove) Token: 0x06009955 RID: 39253 RVA: 0x003A2304 File Offset: 0x003A0504
	public static event Action LateUpdateEv;

	// Token: 0x06009956 RID: 39254 RVA: 0x003A2337 File Offset: 0x003A0537
	public static void ReleaseTaskInfo(ThreadManager.TaskInfo _info)
	{
		if (((_info != null) ? _info.evStopped : null) != null)
		{
			_info.evStopped.Close();
			_info.evStopped = null;
		}
	}

	// Token: 0x1700122A RID: 4650
	// (get) Token: 0x06009957 RID: 39255 RVA: 0x003A2359 File Offset: 0x003A0559
	public static TaskFactory MainThreadFactory
	{
		get
		{
			return ThreadManager.MainThreadScheduler.Factory;
		}
	}

	// Token: 0x06009958 RID: 39256 RVA: 0x003A2368 File Offset: 0x003A0568
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

	// Token: 0x06009959 RID: 39257 RVA: 0x003A2468 File Offset: 0x003A0668
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadDelegate, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, _threadDelegate, null, null, null, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x0600995A RID: 39258 RVA: 0x003A2488 File Offset: 0x003A0688
	public static ThreadManager.ThreadInfo StartThread(string _name, ThreadManager.ThreadFunctionDelegate _threadInit, ThreadManager.ThreadFunctionLoopDelegate _threadLoop, ThreadManager.ThreadFunctionEndDelegate _threadEnd, object _parameter = null, ThreadManager.ExitCallbackThread _exitCallback = null, bool _useRealThread = false, bool _isSilent = false)
	{
		return ThreadManager.startThread(_name, null, _threadInit, _threadLoop, _threadEnd, _parameter, _exitCallback, _useRealThread, _isSilent);
	}

	// Token: 0x0600995B RID: 39259 RVA: 0x003A24A8 File Offset: 0x003A06A8
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

	// Token: 0x0600995C RID: 39260 RVA: 0x003A2644 File Offset: 0x003A0844
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

	// Token: 0x0600995D RID: 39261 RVA: 0x003A268C File Offset: 0x003A088C
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

	// Token: 0x0600995E RID: 39262 RVA: 0x003A2778 File Offset: 0x003A0978
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

	// Token: 0x0600995F RID: 39263 RVA: 0x003A27DC File Offset: 0x003A09DC
	public static void AddSingleTaskMainThread(string _name, Action _func)
	{
		ThreadManager.AddSingleTaskMainThread(_name, delegate(object _)
		{
			_func();
		}, null);
	}

	// Token: 0x06009960 RID: 39264 RVA: 0x003A2809 File Offset: 0x003A0A09
	public static void RunTaskAfterFrames(Action _func, int _inFrames = 1)
	{
		ThreadManager.StartCoroutine(ThreadManager.runAfterFrames(_func, _inFrames));
	}

	// Token: 0x06009961 RID: 39265 RVA: 0x003A2818 File Offset: 0x003A0A18
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

	// Token: 0x06009962 RID: 39266 RVA: 0x003A28FC File Offset: 0x003A0AFC
	public static void LateUpdate()
	{
		Action lateUpdateEv = ThreadManager.LateUpdateEv;
		if (lateUpdateEv == null)
		{
			return;
		}
		lateUpdateEv();
	}

	// Token: 0x06009963 RID: 39267 RVA: 0x003A2910 File Offset: 0x003A0B10
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

	// Token: 0x06009964 RID: 39268 RVA: 0x003A29C8 File Offset: 0x003A0BC8
	public static void SetMonoBehaviour(MonoBehaviour _monoBehaviour)
	{
		ThreadManager.monoBehaviour = _monoBehaviour;
	}

	// Token: 0x06009965 RID: 39269 RVA: 0x003A29D0 File Offset: 0x003A0BD0
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

	// Token: 0x06009966 RID: 39270 RVA: 0x003A2A29 File Offset: 0x003A0C29
	public static void StopCoroutine(IEnumerator _e)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_e);
	}

	// Token: 0x06009967 RID: 39271 RVA: 0x003A2A44 File Offset: 0x003A0C44
	public static void StopCoroutine(Coroutine _coroutine)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_coroutine);
	}

	// Token: 0x06009968 RID: 39272 RVA: 0x003A2A5F File Offset: 0x003A0C5F
	public static void StopCoroutine(string _methodName)
	{
		if (ThreadManager.monoBehaviour == null)
		{
			return;
		}
		ThreadManager.monoBehaviour.StopCoroutine(_methodName);
	}

	// Token: 0x06009969 RID: 39273 RVA: 0x003A2A7C File Offset: 0x003A0C7C
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

	// Token: 0x1700122B RID: 4651
	// (get) Token: 0x0600996A RID: 39274 RVA: 0x003A2AB1 File Offset: 0x003A0CB1
	public static bool IsInSyncCoroutine
	{
		get
		{
			return ThreadManager.syncCoroutineNestingLevel > 0;
		}
	}

	// Token: 0x0600996B RID: 39275 RVA: 0x003A2ABC File Offset: 0x003A0CBC
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

	// Token: 0x0600996C RID: 39276 RVA: 0x003A2B14 File Offset: 0x003A0D14
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

	// Token: 0x0600996D RID: 39277 RVA: 0x003A2B2A File Offset: 0x003A0D2A
	public static void SetMainThreadRef(Thread _mainThreadRef)
	{
		ThreadManager.MainThreadRef = _mainThreadRef;
		ThreadManager.MainThreadId = _mainThreadRef.ManagedThreadId;
	}

	// Token: 0x0600996E RID: 39278 RVA: 0x003A2B3D File Offset: 0x003A0D3D
	public static bool IsMainThread()
	{
		return Thread.CurrentThread.ManagedThreadId == ThreadManager.MainThreadId;
	}

	// Token: 0x0600996F RID: 39279 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string _messagePart1, string _messagePart2 = null)
	{
	}

	// Token: 0x06009970 RID: 39280 RVA: 0x003A2B50 File Offset: 0x003A0D50
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

	// Token: 0x0400738D RID: 29581
	public const int cEndTime = -1;

	// Token: 0x0400738E RID: 29582
	[PublicizedFrom(EAccessModifier.Private)]
	public const int threadTerminationTimeout = 30;

	// Token: 0x0400738F RID: 29583
	public static Thread MainThreadRef;

	// Token: 0x04007390 RID: 29584
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MainThreadId;

	// Token: 0x04007391 RID: 29585
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ManualTaskScheduler MainThreadScheduler = new ManualTaskScheduler();

	// Token: 0x04007394 RID: 29588
	public static Dictionary<string, ThreadManager.ThreadInfo> ActiveThreads = new Dictionary<string, ThreadManager.ThreadInfo>();

	// Token: 0x04007395 RID: 29589
	public static int QueuedCount;

	// Token: 0x04007396 RID: 29590
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectQueuedCounter = new object();

	// Token: 0x04007397 RID: 29591
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObjectMainThreadTasks = new object();

	// Token: 0x04007398 RID: 29592
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasks = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x04007399 RID: 29593
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<ThreadManager.MainThreadTaskInfo> mainThreadTasksCopy = new List<ThreadManager.MainThreadTaskInfo>(150);

	// Token: 0x0400739A RID: 29594
	[PublicizedFrom(EAccessModifier.Private)]
	public static MonoBehaviour monoBehaviour;

	// Token: 0x0400739B RID: 29595
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitCallback queuedTaskDelegate = new WaitCallback(ThreadManager.myQueuedTaskInvoke);

	// Token: 0x0400739C RID: 29596
	[PublicizedFrom(EAccessModifier.Private)]
	public static int syncCoroutineNestingLevel;

	// Token: 0x020012E2 RID: 4834
	public class ThreadInfo
	{
		// Token: 0x06009971 RID: 39281 RVA: 0x003A2B66 File Offset: 0x003A0D66
		public void RequestTermination()
		{
			this.evRunning.Set();
		}

		// Token: 0x06009972 RID: 39282 RVA: 0x003A2B74 File Offset: 0x003A0D74
		public bool TerminationRequested()
		{
			return this.evRunning.WaitOne(0);
		}

		// Token: 0x06009973 RID: 39283 RVA: 0x003A2B82 File Offset: 0x003A0D82
		public bool HasTerminated()
		{
			return this.evStopped.WaitOne(0);
		}

		// Token: 0x06009974 RID: 39284 RVA: 0x003A2B90 File Offset: 0x003A0D90
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

		// Token: 0x0400739D RID: 29597
		public object parameter;

		// Token: 0x0400739E RID: 29598
		public ThreadManager.ThreadFunctionDelegate threadDelegate;

		// Token: 0x0400739F RID: 29599
		public ThreadManager.ThreadFunctionDelegate threadInit;

		// Token: 0x040073A0 RID: 29600
		public ThreadManager.ThreadFunctionLoopDelegate threadLoop;

		// Token: 0x040073A1 RID: 29601
		public ThreadManager.ThreadFunctionEndDelegate threadEnd;

		// Token: 0x040073A2 RID: 29602
		public string name;

		// Token: 0x040073A3 RID: 29603
		public Thread thread;

		// Token: 0x040073A4 RID: 29604
		public bool isSilent;

		// Token: 0x040073A5 RID: 29605
		public readonly ManualResetEvent evRunning = new ManualResetEvent(false);

		// Token: 0x040073A6 RID: 29606
		public readonly ManualResetEvent evStopped = new ManualResetEvent(false);

		// Token: 0x040073A7 RID: 29607
		public ThreadManager.ExitCallbackThread exitCallback;

		// Token: 0x040073A8 RID: 29608
		public object threadData;
	}

	// Token: 0x020012E3 RID: 4835
	public class TaskInfo
	{
		// Token: 0x06009976 RID: 39286 RVA: 0x003A2C2A File Offset: 0x003A0E2A
		public TaskInfo(bool _endEvent = true)
		{
			if (_endEvent)
			{
				this.evStopped = new ManualResetEvent(false);
			}
		}

		// Token: 0x06009977 RID: 39287 RVA: 0x003A2C41 File Offset: 0x003A0E41
		public void WaitForEnd()
		{
			this.evStopped.WaitOne();
		}

		// Token: 0x040073A9 RID: 29609
		public string name;

		// Token: 0x040073AA RID: 29610
		public ThreadManager.TaskFunctionDelegate taskDelegate;

		// Token: 0x040073AB RID: 29611
		public object parameter;

		// Token: 0x040073AC RID: 29612
		public ThreadManager.ExitCallbackTask exitCallback;

		// Token: 0x040073AD RID: 29613
		public ManualResetEvent evStopped;
	}

	// Token: 0x020012E4 RID: 4836
	public struct MainThreadTaskInfo
	{
		// Token: 0x040073AE RID: 29614
		public string name;

		// Token: 0x040073AF RID: 29615
		public ThreadManager.MainThreadTaskFunctionDelegate taskDelegate;

		// Token: 0x040073B0 RID: 29616
		public object parameter;
	}

	// Token: 0x020012E5 RID: 4837
	// (Invoke) Token: 0x06009979 RID: 39289
	public delegate int ThreadFunctionLoopDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x020012E6 RID: 4838
	// (Invoke) Token: 0x0600997D RID: 39293
	public delegate void ThreadFunctionEndDelegate(ThreadManager.ThreadInfo _threadInfo, bool _exitForException);

	// Token: 0x020012E7 RID: 4839
	// (Invoke) Token: 0x06009981 RID: 39297
	public delegate void ThreadFunctionDelegate(ThreadManager.ThreadInfo _threadInfo);

	// Token: 0x020012E8 RID: 4840
	// (Invoke) Token: 0x06009985 RID: 39301
	public delegate void TaskFunctionDelegate(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x020012E9 RID: 4841
	// (Invoke) Token: 0x06009989 RID: 39305
	public delegate void MainThreadTaskFunctionDelegate(object _parameter);

	// Token: 0x020012EA RID: 4842
	// (Invoke) Token: 0x0600998D RID: 39309
	public delegate void ExitCallbackThread(ThreadManager.ThreadInfo _ti, Exception _e);

	// Token: 0x020012EB RID: 4843
	// (Invoke) Token: 0x06009991 RID: 39313
	public delegate void ExitCallbackTask(ThreadManager.TaskInfo _ti, Exception _e);
}
