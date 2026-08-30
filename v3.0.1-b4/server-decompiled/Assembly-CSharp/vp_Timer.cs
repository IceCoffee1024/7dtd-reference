using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02001570 RID: 5488
public class vp_Timer : MonoBehaviour
{
	// Token: 0x170013B3 RID: 5043
	// (get) Token: 0x0600AB7A RID: 43898 RVA: 0x003FE0D8 File Offset: 0x003FC2D8
	public bool WasAddedCorrectly
	{
		get
		{
			return Application.isPlaying && !(base.gameObject != vp_Timer.m_MainObject);
		}
	}

	// Token: 0x0600AB7B RID: 43899 RVA: 0x003FE0F8 File Offset: 0x003FC2F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (!this.WasAddedCorrectly)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x0600AB7C RID: 43900 RVA: 0x003FE10C File Offset: 0x003FC30C
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

	// Token: 0x0600AB7D RID: 43901 RVA: 0x003FE235 File Offset: 0x003FC435
	public static void In(float delay, vp_Timer.Callback callback, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AB7E RID: 43902 RVA: 0x003FE247 File Offset: 0x003FC447
	public static void In(float delay, vp_Timer.Callback callback, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AB7F RID: 43903 RVA: 0x003FE259 File Offset: 0x003FC459
	public static void In(float delay, vp_Timer.Callback callback, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, interval);
	}

	// Token: 0x0600AB80 RID: 43904 RVA: 0x003FE268 File Offset: 0x003FC468
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, 1, -1f);
	}

	// Token: 0x0600AB81 RID: 43905 RVA: 0x003FE27A File Offset: 0x003FC47A
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AB82 RID: 43906 RVA: 0x003FE28D File Offset: 0x003FC48D
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, interval);
	}

	// Token: 0x0600AB83 RID: 43907 RVA: 0x003FE29D File Offset: 0x003FC49D
	public static void Start(vp_Timer.Handle timerHandle)
	{
		vp_Timer.Schedule(315360000f, delegate
		{
		}, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AB84 RID: 43908 RVA: 0x003FE2D4 File Offset: 0x003FC4D4
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

	// Token: 0x0600AB85 RID: 43909 RVA: 0x003FE45D File Offset: 0x003FC65D
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

	// Token: 0x0600AB86 RID: 43910 RVA: 0x003FE474 File Offset: 0x003FC674
	public static void CancelAll()
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			vp_Timer.m_Active[i].Id = 0;
		}
	}

	// Token: 0x0600AB87 RID: 43911 RVA: 0x003FE4AC File Offset: 0x003FC6AC
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

	// Token: 0x0600AB88 RID: 43912 RVA: 0x003FE4F9 File Offset: 0x003FC6F9
	public static void DestroyAll()
	{
		vp_Timer.m_Active.Clear();
		vp_Timer.m_Pool.Clear();
	}

	// Token: 0x0600AB89 RID: 43913 RVA: 0x003FE50F File Offset: 0x003FC70F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AB8A RID: 43914 RVA: 0x003FE522 File Offset: 0x003FC722
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AB8B RID: 43915 RVA: 0x003FE538 File Offset: 0x003FC738
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

	// Token: 0x0600AB8C RID: 43916 RVA: 0x003FE580 File Offset: 0x003FC780
	public static vp_Timer.Stats EditorGetStats()
	{
		vp_Timer.Stats stats;
		stats.Created = vp_Timer.m_Active.Count + vp_Timer.m_Pool.Count;
		stats.Inactive = vp_Timer.m_Pool.Count;
		stats.Active = vp_Timer.m_Active.Count;
		return stats;
	}

	// Token: 0x0600AB8D RID: 43917 RVA: 0x003FE5CC File Offset: 0x003FC7CC
	public static string EditorGetMethodInfo(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return "Argument out of range.";
		}
		return vp_Timer.m_Active[eventIndex].MethodInfo;
	}

	// Token: 0x0600AB8E RID: 43918 RVA: 0x003FE5F7 File Offset: 0x003FC7F7
	public static int EditorGetMethodId(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return 0;
		}
		return vp_Timer.m_Active[eventIndex].Id;
	}

	// Token: 0x04007EC8 RID: 32456
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject m_MainObject = null;

	// Token: 0x04007EC9 RID: 32457
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Active = new List<vp_Timer.Event>();

	// Token: 0x04007ECA RID: 32458
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Pool = new List<vp_Timer.Event>();

	// Token: 0x04007ECB RID: 32459
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_Timer.Event m_NewEvent = null;

	// Token: 0x04007ECC RID: 32460
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventCount = 0;

	// Token: 0x04007ECD RID: 32461
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventBatch = 0;

	// Token: 0x04007ECE RID: 32462
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventIterator = 0;

	// Token: 0x04007ECF RID: 32463
	public static int MaxEventsPerFrame = 500;

	// Token: 0x02001571 RID: 5489
	// (Invoke) Token: 0x0600AB92 RID: 43922
	public delegate void Callback();

	// Token: 0x02001572 RID: 5490
	// (Invoke) Token: 0x0600AB96 RID: 43926
	public delegate void ArgCallback(object args);

	// Token: 0x02001573 RID: 5491
	public struct Stats
	{
		// Token: 0x04007ED0 RID: 32464
		public int Created;

		// Token: 0x04007ED1 RID: 32465
		public int Inactive;

		// Token: 0x04007ED2 RID: 32466
		public int Active;
	}

	// Token: 0x02001574 RID: 5492
	[PublicizedFrom(EAccessModifier.Private)]
	public class Event
	{
		// Token: 0x0600AB99 RID: 43929 RVA: 0x003FE65C File Offset: 0x003FC85C
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

		// Token: 0x0600AB9A RID: 43930 RVA: 0x003FE700 File Offset: 0x003FC900
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

		// Token: 0x0600AB9B RID: 43931 RVA: 0x003FE75E File Offset: 0x003FC95E
		[PublicizedFrom(EAccessModifier.Private)]
		public void Destroy()
		{
			vp_Timer.m_Active.Remove(this);
			vp_Timer.m_Pool.Remove(this);
		}

		// Token: 0x0600AB9C RID: 43932 RVA: 0x003FE778 File Offset: 0x003FC978
		[PublicizedFrom(EAccessModifier.Private)]
		public void Error(string message)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") " + message);
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x0600AB9D RID: 43933 RVA: 0x003FE79C File Offset: 0x003FC99C
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

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x0600AB9E RID: 43934 RVA: 0x003FE844 File Offset: 0x003FCA44
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

		// Token: 0x04007ED3 RID: 32467
		public int Id;

		// Token: 0x04007ED4 RID: 32468
		public vp_Timer.Callback Function;

		// Token: 0x04007ED5 RID: 32469
		public vp_Timer.ArgCallback ArgFunction;

		// Token: 0x04007ED6 RID: 32470
		public object Arguments;

		// Token: 0x04007ED7 RID: 32471
		public int Iterations = 1;

		// Token: 0x04007ED8 RID: 32472
		public float Interval = -1f;

		// Token: 0x04007ED9 RID: 32473
		public float DueTime;

		// Token: 0x04007EDA RID: 32474
		public float StartTime;

		// Token: 0x04007EDB RID: 32475
		public float LifeTime;

		// Token: 0x04007EDC RID: 32476
		public bool Paused;

		// Token: 0x04007EDD RID: 32477
		public bool CancelOnLoad = true;
	}

	// Token: 0x02001575 RID: 5493
	public class Handle
	{
		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x0600ABA0 RID: 43936 RVA: 0x003FE924 File Offset: 0x003FCB24
		// (set) Token: 0x0600ABA1 RID: 43937 RVA: 0x003FE93B File Offset: 0x003FCB3B
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

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x0600ABA2 RID: 43938 RVA: 0x003FE951 File Offset: 0x003FCB51
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

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x0600ABA3 RID: 43939 RVA: 0x003FE96C File Offset: 0x003FCB6C
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

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x0600ABA4 RID: 43940 RVA: 0x003FE982 File Offset: 0x003FCB82
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

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x0600ABA5 RID: 43941 RVA: 0x003FE99D File Offset: 0x003FCB9D
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

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x0600ABA6 RID: 43942 RVA: 0x003FE9B9 File Offset: 0x003FCBB9
		public float Delay
		{
			get
			{
				return Mathf.Round((this.m_FirstDueTime - this.TimeOfInitiation) * 1000f) / 1000f;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x0600ABA7 RID: 43943 RVA: 0x003FE9D9 File Offset: 0x003FCBD9
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

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x0600ABA8 RID: 43944 RVA: 0x003FE9F4 File Offset: 0x003FCBF4
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

		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x0600ABA9 RID: 43945 RVA: 0x003FEA15 File Offset: 0x003FCC15
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

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x0600ABAA RID: 43946 RVA: 0x003FEA46 File Offset: 0x003FCC46
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

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x0600ABAB RID: 43947 RVA: 0x003FEA7B File Offset: 0x003FCC7B
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

		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x0600ABAC RID: 43948 RVA: 0x003FEA96 File Offset: 0x003FCC96
		public int IterationsTotal
		{
			get
			{
				return this.m_StartIterations;
			}
		}

		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x0600ABAD RID: 43949 RVA: 0x003FEA9E File Offset: 0x003FCC9E
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

		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x0600ABAE RID: 43950 RVA: 0x003FEAB5 File Offset: 0x003FCCB5
		// (set) Token: 0x0600ABAF RID: 43951 RVA: 0x003FEAC0 File Offset: 0x003FCCC0
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

		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x0600ABB0 RID: 43952 RVA: 0x003FEBA6 File Offset: 0x003FCDA6
		public bool Active
		{
			get
			{
				return this.m_Event != null && this.Id != 0 && this.m_Event.Id != 0 && this.m_Event.Id == this.Id;
			}
		}

		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x0600ABB1 RID: 43953 RVA: 0x003FEBDA File Offset: 0x003FCDDA
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

		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x0600ABB2 RID: 43954 RVA: 0x003FEBF5 File Offset: 0x003FCDF5
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

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x0600ABB3 RID: 43955 RVA: 0x003FEC10 File Offset: 0x003FCE10
		// (set) Token: 0x0600ABB4 RID: 43956 RVA: 0x003FEC27 File Offset: 0x003FCE27
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

		// Token: 0x0600ABB5 RID: 43957 RVA: 0x003FEC5F File Offset: 0x003FCE5F
		public void Cancel()
		{
			vp_Timer.Cancel(this);
		}

		// Token: 0x0600ABB6 RID: 43958 RVA: 0x003FEC67 File Offset: 0x003FCE67
		public void Execute()
		{
			this.m_Event.DueTime = Time.time;
		}

		// Token: 0x04007EDE RID: 32478
		[PublicizedFrom(EAccessModifier.Private)]
		public vp_Timer.Event m_Event;

		// Token: 0x04007EDF RID: 32479
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_Id;

		// Token: 0x04007EE0 RID: 32480
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_StartIterations = 1;

		// Token: 0x04007EE1 RID: 32481
		[PublicizedFrom(EAccessModifier.Private)]
		public float m_FirstDueTime;
	}
}
