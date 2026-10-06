using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015AD RID: 5549
public class vp_Timer : MonoBehaviour
{
	// Token: 0x17001431 RID: 5169
	// (get) Token: 0x0600AD2A RID: 44330 RVA: 0x0040826C File Offset: 0x0040646C
	public bool WasAddedCorrectly
	{
		get
		{
			return Application.isPlaying && !(base.gameObject != vp_Timer.m_MainObject);
		}
	}

	// Token: 0x0600AD2B RID: 44331 RVA: 0x0040828C File Offset: 0x0040648C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (!this.WasAddedCorrectly)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x0600AD2C RID: 44332 RVA: 0x004082A0 File Offset: 0x004064A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		vp_Timer.m_EventBatch = 0;
		while (vp_Timer.m_Active.Count > 0 && vp_Timer.m_EventBatch < vp_Timer.MaxEventsPerFrame)
		{
			if (vp_Timer.m_EventIterator < 0)
			{
				vp_Timer.m_EventIterator = vp_Timer.m_Active.Count - 1;
				return;
			}
			if (vp_Timer.m_EventIterator > vp_Timer.m_Active.Count - 1)
			{
				vp_Timer.m_EventIterator = vp_Timer.m_Active.Count - 1;
			}
			if (Time.time >= vp_Timer.m_Active[vp_Timer.m_EventIterator].DueTime || vp_Timer.m_Active[vp_Timer.m_EventIterator].Id == 0)
			{
				vp_Timer.m_Active[vp_Timer.m_EventIterator].Execute();
			}
			else if (vp_Timer.m_Active[vp_Timer.m_EventIterator].Paused)
			{
				vp_Timer.m_Active[vp_Timer.m_EventIterator].DueTime += Time.deltaTime;
			}
			else
			{
				vp_Timer.m_Active[vp_Timer.m_EventIterator].LifeTime += Time.deltaTime;
			}
			vp_Timer.m_EventIterator--;
			vp_Timer.m_EventBatch++;
		}
	}

	// Token: 0x0600AD2D RID: 44333 RVA: 0x004083C9 File Offset: 0x004065C9
	public static void In(float delay, vp_Timer.Callback callback, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AD2E RID: 44334 RVA: 0x004083DB File Offset: 0x004065DB
	public static void In(float delay, vp_Timer.Callback callback, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AD2F RID: 44335 RVA: 0x004083ED File Offset: 0x004065ED
	public static void In(float delay, vp_Timer.Callback callback, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, interval);
	}

	// Token: 0x0600AD30 RID: 44336 RVA: 0x004083FC File Offset: 0x004065FC
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, 1, -1f);
	}

	// Token: 0x0600AD31 RID: 44337 RVA: 0x0040840E File Offset: 0x0040660E
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AD32 RID: 44338 RVA: 0x00408421 File Offset: 0x00406621
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, interval);
	}

	// Token: 0x0600AD33 RID: 44339 RVA: 0x00408431 File Offset: 0x00406631
	public static void Start(vp_Timer.Handle timerHandle)
	{
		vp_Timer.Schedule(315360000f, delegate
		{
		}, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AD34 RID: 44340 RVA: 0x00408468 File Offset: 0x00406668
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Schedule(float time, vp_Timer.Callback func, vp_Timer.ArgCallback argFunc, object args, vp_Timer.Handle timerHandle, int iterations, float interval)
	{
		if (func == null && argFunc == null)
		{
			Debug.LogError("Error: (vp_Timer) Aborted event because function is null.");
			return;
		}
		if (vp_Timer.m_MainObject == null)
		{
			vp_Timer.m_MainObject = new GameObject("Timers");
			vp_Timer.m_MainObject.AddComponent<vp_Timer>();
			global::UnityEngine.Object.DontDestroyOnLoad(vp_Timer.m_MainObject);
		}
		time = Mathf.Max(0f, time);
		iterations = Mathf.Max(0, iterations);
		interval = ((interval == -1f) ? time : Mathf.Max(0f, interval));
		vp_Timer.m_NewEvent = null;
		if (vp_Timer.m_Pool.Count > 0)
		{
			vp_Timer.m_NewEvent = vp_Timer.m_Pool[0];
			vp_Timer.m_Pool.Remove(vp_Timer.m_NewEvent);
		}
		else
		{
			vp_Timer.m_NewEvent = new vp_Timer.Event();
		}
		vp_Timer.m_EventCount++;
		vp_Timer.m_NewEvent.Id = vp_Timer.m_EventCount;
		if (func != null)
		{
			vp_Timer.m_NewEvent.Function = func;
		}
		else if (argFunc != null)
		{
			vp_Timer.m_NewEvent.ArgFunction = argFunc;
			vp_Timer.m_NewEvent.Arguments = args;
		}
		vp_Timer.m_NewEvent.StartTime = Time.time;
		vp_Timer.m_NewEvent.DueTime = Time.time + time;
		vp_Timer.m_NewEvent.Iterations = iterations;
		vp_Timer.m_NewEvent.Interval = interval;
		vp_Timer.m_NewEvent.LifeTime = 0f;
		vp_Timer.m_NewEvent.Paused = false;
		vp_Timer.m_Active.Add(vp_Timer.m_NewEvent);
		if (timerHandle != null)
		{
			if (timerHandle.Active)
			{
				timerHandle.Cancel();
			}
			timerHandle.Id = vp_Timer.m_NewEvent.Id;
		}
	}

	// Token: 0x0600AD35 RID: 44341 RVA: 0x004085F1 File Offset: 0x004067F1
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Cancel(vp_Timer.Handle handle)
	{
		if (handle == null)
		{
			return;
		}
		if (handle.Active)
		{
			handle.Id = 0;
			return;
		}
	}

	// Token: 0x0600AD36 RID: 44342 RVA: 0x00408608 File Offset: 0x00406808
	public static void CancelAll()
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			vp_Timer.m_Active[i].Id = 0;
		}
	}

	// Token: 0x0600AD37 RID: 44343 RVA: 0x00408640 File Offset: 0x00406840
	public static void CancelAll(string methodName)
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			if (vp_Timer.m_Active[i].MethodName == methodName)
			{
				vp_Timer.m_Active[i].Id = 0;
			}
		}
	}

	// Token: 0x0600AD38 RID: 44344 RVA: 0x0040868D File Offset: 0x0040688D
	public static void DestroyAll()
	{
		vp_Timer.m_Active.Clear();
		vp_Timer.m_Pool.Clear();
	}

	// Token: 0x0600AD39 RID: 44345 RVA: 0x004086A3 File Offset: 0x004068A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD3A RID: 44346 RVA: 0x004086B6 File Offset: 0x004068B6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD3B RID: 44347 RVA: 0x004086CC File Offset: 0x004068CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void NotifyLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			if (vp_Timer.m_Active[i].CancelOnLoad)
			{
				vp_Timer.m_Active[i].Id = 0;
			}
		}
	}

	// Token: 0x0600AD3C RID: 44348 RVA: 0x00408714 File Offset: 0x00406914
	public static vp_Timer.Stats EditorGetStats()
	{
		vp_Timer.Stats stats;
		stats.Created = vp_Timer.m_Active.Count + vp_Timer.m_Pool.Count;
		stats.Inactive = vp_Timer.m_Pool.Count;
		stats.Active = vp_Timer.m_Active.Count;
		return stats;
	}

	// Token: 0x0600AD3D RID: 44349 RVA: 0x00408760 File Offset: 0x00406960
	public static string EditorGetMethodInfo(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return "Argument out of range.";
		}
		return vp_Timer.m_Active[eventIndex].MethodInfo;
	}

	// Token: 0x0600AD3E RID: 44350 RVA: 0x0040878B File Offset: 0x0040698B
	public static int EditorGetMethodId(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return 0;
		}
		return vp_Timer.m_Active[eventIndex].Id;
	}

	// Token: 0x04008024 RID: 32804
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject m_MainObject = null;

	// Token: 0x04008025 RID: 32805
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Active = new List<vp_Timer.Event>();

	// Token: 0x04008026 RID: 32806
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Pool = new List<vp_Timer.Event>();

	// Token: 0x04008027 RID: 32807
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_Timer.Event m_NewEvent = null;

	// Token: 0x04008028 RID: 32808
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventCount = 0;

	// Token: 0x04008029 RID: 32809
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventBatch = 0;

	// Token: 0x0400802A RID: 32810
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventIterator = 0;

	// Token: 0x0400802B RID: 32811
	public static int MaxEventsPerFrame = 500;

	// Token: 0x020015AE RID: 5550
	// (Invoke) Token: 0x0600AD42 RID: 44354
	public delegate void Callback();

	// Token: 0x020015AF RID: 5551
	// (Invoke) Token: 0x0600AD46 RID: 44358
	public delegate void ArgCallback(object args);

	// Token: 0x020015B0 RID: 5552
	public struct Stats
	{
		// Token: 0x0400802C RID: 32812
		public int Created;

		// Token: 0x0400802D RID: 32813
		public int Inactive;

		// Token: 0x0400802E RID: 32814
		public int Active;
	}

	// Token: 0x020015B1 RID: 5553
	[PublicizedFrom(EAccessModifier.Private)]
	public class Event
	{
		// Token: 0x0600AD49 RID: 44361 RVA: 0x004087F0 File Offset: 0x004069F0
		public void Execute()
		{
			if (this.Id == 0 || this.DueTime == 0f)
			{
				this.Recycle();
				return;
			}
			if (this.Function != null)
			{
				this.Function();
			}
			else
			{
				if (this.ArgFunction == null)
				{
					this.Error("Aborted event because function is null.");
					this.Recycle();
					return;
				}
				this.ArgFunction(this.Arguments);
			}
			if (this.Iterations > 0)
			{
				this.Iterations--;
				if (this.Iterations < 1)
				{
					this.Recycle();
					return;
				}
			}
			this.DueTime = Time.time + this.Interval;
		}

		// Token: 0x0600AD4A RID: 44362 RVA: 0x00408894 File Offset: 0x00406A94
		[PublicizedFrom(EAccessModifier.Private)]
		public void Recycle()
		{
			this.Id = 0;
			this.DueTime = 0f;
			this.StartTime = 0f;
			this.CancelOnLoad = true;
			this.Function = null;
			this.ArgFunction = null;
			this.Arguments = null;
			if (vp_Timer.m_Active.Remove(this))
			{
				vp_Timer.m_Pool.Add(this);
			}
		}

		// Token: 0x0600AD4B RID: 44363 RVA: 0x004088F2 File Offset: 0x00406AF2
		[PublicizedFrom(EAccessModifier.Private)]
		public void Destroy()
		{
			vp_Timer.m_Active.Remove(this);
			vp_Timer.m_Pool.Remove(this);
		}

		// Token: 0x0600AD4C RID: 44364 RVA: 0x0040890C File Offset: 0x00406B0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Error(string message)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") " + message);
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x0600AD4D RID: 44365 RVA: 0x00408930 File Offset: 0x00406B30
		public string MethodName
		{
			get
			{
				if (this.Function != null)
				{
					if (this.Function.Method != null)
					{
						if (this.Function.Method.Name[0] == '<')
						{
							return "delegate";
						}
						return this.Function.Method.Name;
					}
				}
				else if (this.ArgFunction != null && this.ArgFunction.Method != null)
				{
					if (this.ArgFunction.Method.Name[0] == '<')
					{
						return "delegate";
					}
					return this.ArgFunction.Method.Name;
				}
				return null;
			}
		}

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x0600AD4E RID: 44366 RVA: 0x004089D8 File Offset: 0x00406BD8
		public string MethodInfo
		{
			get
			{
				string text = this.MethodName;
				if (!string.IsNullOrEmpty(text))
				{
					text += "(";
					if (this.Arguments != null)
					{
						if (this.Arguments.GetType().IsArray)
						{
							object[] array = (object[])this.Arguments;
							foreach (object obj in array)
							{
								text += obj.ToString();
								if (Array.IndexOf<object>(array, obj) < array.Length - 1)
								{
									text += ", ";
								}
							}
						}
						else
						{
							string text2 = text;
							object arguments = this.Arguments;
							text = text2 + ((arguments != null) ? arguments.ToString() : null);
						}
					}
					text += ")";
				}
				else
				{
					text = "(function = null)";
				}
				return text;
			}
		}

		// Token: 0x0400802F RID: 32815
		public int Id;

		// Token: 0x04008030 RID: 32816
		public vp_Timer.Callback Function;

		// Token: 0x04008031 RID: 32817
		public vp_Timer.ArgCallback ArgFunction;

		// Token: 0x04008032 RID: 32818
		public object Arguments;

		// Token: 0x04008033 RID: 32819
		public int Iterations = 1;

		// Token: 0x04008034 RID: 32820
		public float Interval = -1f;

		// Token: 0x04008035 RID: 32821
		public float DueTime;

		// Token: 0x04008036 RID: 32822
		public float StartTime;

		// Token: 0x04008037 RID: 32823
		public float LifeTime;

		// Token: 0x04008038 RID: 32824
		public bool Paused;

		// Token: 0x04008039 RID: 32825
		public bool CancelOnLoad = true;
	}

	// Token: 0x020015B2 RID: 5554
	public class Handle
	{
		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x0600AD50 RID: 44368 RVA: 0x00408AB8 File Offset: 0x00406CB8
		// (set) Token: 0x0600AD51 RID: 44369 RVA: 0x00408ACF File Offset: 0x00406CCF
		public bool Paused
		{
			get
			{
				return this.Active && this.m_Event.Paused;
			}
			set
			{
				if (this.Active)
				{
					this.m_Event.Paused = value;
				}
			}
		}

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x0600AD52 RID: 44370 RVA: 0x00408AE5 File Offset: 0x00406CE5
		public float TimeOfInitiation
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.StartTime;
				}
				return 0f;
			}
		}

		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x0600AD53 RID: 44371 RVA: 0x00408B00 File Offset: 0x00406D00
		public float TimeOfFirstIteration
		{
			get
			{
				if (this.Active)
				{
					return this.m_FirstDueTime;
				}
				return 0f;
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x0600AD54 RID: 44372 RVA: 0x00408B16 File Offset: 0x00406D16
		public float TimeOfNextIteration
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.DueTime;
				}
				return 0f;
			}
		}

		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x0600AD55 RID: 44373 RVA: 0x00408B31 File Offset: 0x00406D31
		public float TimeOfLastIteration
		{
			get
			{
				if (this.Active)
				{
					return Time.time + this.DurationLeft;
				}
				return 0f;
			}
		}

		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x0600AD56 RID: 44374 RVA: 0x00408B4D File Offset: 0x00406D4D
		public float Delay
		{
			get
			{
				return Mathf.Round((this.m_FirstDueTime - this.TimeOfInitiation) * 1000f) / 1000f;
			}
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x0600AD57 RID: 44375 RVA: 0x00408B6D File Offset: 0x00406D6D
		public float Interval
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.Interval;
				}
				return 0f;
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x0600AD58 RID: 44376 RVA: 0x00408B88 File Offset: 0x00406D88
		public float TimeUntilNextIteration
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.DueTime - Time.time;
				}
				return 0f;
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x0600AD59 RID: 44377 RVA: 0x00408BA9 File Offset: 0x00406DA9
		public float DurationLeft
		{
			get
			{
				if (this.Active)
				{
					return this.TimeUntilNextIteration + (float)(this.m_Event.Iterations - 1) * this.m_Event.Interval;
				}
				return 0f;
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x0600AD5A RID: 44378 RVA: 0x00408BDA File Offset: 0x00406DDA
		public float DurationTotal
		{
			get
			{
				if (this.Active)
				{
					return this.Delay + (float)this.m_StartIterations * ((this.m_StartIterations > 1) ? this.Interval : 0f);
				}
				return 0f;
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x0600AD5B RID: 44379 RVA: 0x00408C0F File Offset: 0x00406E0F
		public float Duration
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.LifeTime;
				}
				return 0f;
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x0600AD5C RID: 44380 RVA: 0x00408C2A File Offset: 0x00406E2A
		public int IterationsTotal
		{
			get
			{
				return this.m_StartIterations;
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x0600AD5D RID: 44381 RVA: 0x00408C32 File Offset: 0x00406E32
		public int IterationsLeft
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.Iterations;
				}
				return 0;
			}
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x0600AD5E RID: 44382 RVA: 0x00408C49 File Offset: 0x00406E49
		// (set) Token: 0x0600AD5F RID: 44383 RVA: 0x00408C54 File Offset: 0x00406E54
		public int Id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
				if (this.m_Id == 0)
				{
					this.m_Event.DueTime = 0f;
					return;
				}
				this.m_Event = null;
				for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
				{
					if (vp_Timer.m_Active[i].Id == this.m_Id)
					{
						this.m_Event = vp_Timer.m_Active[i];
						break;
					}
				}
				if (this.m_Event == null)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Error: (",
						(this != null) ? this.ToString() : null,
						") Failed to assign event with Id '",
						this.m_Id.ToString(),
						"'."
					}));
				}
				this.m_StartIterations = this.m_Event.Iterations;
				this.m_FirstDueTime = this.m_Event.DueTime;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x0600AD60 RID: 44384 RVA: 0x00408D3A File Offset: 0x00406F3A
		public bool Active
		{
			get
			{
				return this.m_Event != null && this.Id != 0 && this.m_Event.Id != 0 && this.m_Event.Id == this.Id;
			}
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x0600AD61 RID: 44385 RVA: 0x00408D6E File Offset: 0x00406F6E
		public string MethodName
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.MethodName;
				}
				return "";
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x0600AD62 RID: 44386 RVA: 0x00408D89 File Offset: 0x00406F89
		public string MethodInfo
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.MethodInfo;
				}
				return "";
			}
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x0600AD63 RID: 44387 RVA: 0x00408DA4 File Offset: 0x00406FA4
		// (set) Token: 0x0600AD64 RID: 44388 RVA: 0x00408DBB File Offset: 0x00406FBB
		public bool CancelOnLoad
		{
			get
			{
				return !this.Active || this.m_Event.CancelOnLoad;
			}
			set
			{
				if (this.Active)
				{
					this.m_Event.CancelOnLoad = value;
					return;
				}
				Debug.LogWarning("Warning: (" + ((this != null) ? this.ToString() : null) + ") Tried to set CancelOnLoad on inactive timer handle.");
			}
		}

		// Token: 0x0600AD65 RID: 44389 RVA: 0x00408DF3 File Offset: 0x00406FF3
		public void Cancel()
		{
			vp_Timer.Cancel(this);
		}

		// Token: 0x0600AD66 RID: 44390 RVA: 0x00408DFB File Offset: 0x00406FFB
		public void Execute()
		{
			this.m_Event.DueTime = Time.time;
		}

		// Token: 0x0400803A RID: 32826
		[PublicizedFrom(EAccessModifier.Private)]
		public vp_Timer.Event m_Event;

		// Token: 0x0400803B RID: 32827
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_Id;

		// Token: 0x0400803C RID: 32828
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_StartIterations = 1;

		// Token: 0x0400803D RID: 32829
		[PublicizedFrom(EAccessModifier.Private)]
		public float m_FirstDueTime;
	}
}
