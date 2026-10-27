using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020015BF RID: 5567
public class vp_Timer : MonoBehaviour
{
	// Token: 0x1700143D RID: 5181
	// (get) Token: 0x0600AD82 RID: 44418 RVA: 0x0040A670 File Offset: 0x00408870
	public bool WasAddedCorrectly
	{
		get
		{
			return Application.isPlaying && !(base.gameObject != vp_Timer.m_MainObject);
		}
	}

	// Token: 0x0600AD83 RID: 44419 RVA: 0x0040A690 File Offset: 0x00408890
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (!this.WasAddedCorrectly)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x0600AD84 RID: 44420 RVA: 0x0040A6A4 File Offset: 0x004088A4
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

	// Token: 0x0600AD85 RID: 44421 RVA: 0x0040A7CD File Offset: 0x004089CD
	public static void In(float delay, vp_Timer.Callback callback, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AD86 RID: 44422 RVA: 0x0040A7DF File Offset: 0x004089DF
	public static void In(float delay, vp_Timer.Callback callback, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AD87 RID: 44423 RVA: 0x0040A7F1 File Offset: 0x004089F1
	public static void In(float delay, vp_Timer.Callback callback, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, interval);
	}

	// Token: 0x0600AD88 RID: 44424 RVA: 0x0040A800 File Offset: 0x00408A00
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, 1, -1f);
	}

	// Token: 0x0600AD89 RID: 44425 RVA: 0x0040A812 File Offset: 0x00408A12
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AD8A RID: 44426 RVA: 0x0040A825 File Offset: 0x00408A25
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, interval);
	}

	// Token: 0x0600AD8B RID: 44427 RVA: 0x0040A835 File Offset: 0x00408A35
	public static void Start(vp_Timer.Handle timerHandle)
	{
		vp_Timer.Schedule(315360000f, delegate
		{
		}, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AD8C RID: 44428 RVA: 0x0040A86C File Offset: 0x00408A6C
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

	// Token: 0x0600AD8D RID: 44429 RVA: 0x0040A9F5 File Offset: 0x00408BF5
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

	// Token: 0x0600AD8E RID: 44430 RVA: 0x0040AA0C File Offset: 0x00408C0C
	public static void CancelAll()
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			vp_Timer.m_Active[i].Id = 0;
		}
	}

	// Token: 0x0600AD8F RID: 44431 RVA: 0x0040AA44 File Offset: 0x00408C44
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

	// Token: 0x0600AD90 RID: 44432 RVA: 0x0040AA91 File Offset: 0x00408C91
	public static void DestroyAll()
	{
		vp_Timer.m_Active.Clear();
		vp_Timer.m_Pool.Clear();
	}

	// Token: 0x0600AD91 RID: 44433 RVA: 0x0040AAA7 File Offset: 0x00408CA7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD92 RID: 44434 RVA: 0x0040AABA File Offset: 0x00408CBA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AD93 RID: 44435 RVA: 0x0040AAD0 File Offset: 0x00408CD0
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

	// Token: 0x0600AD94 RID: 44436 RVA: 0x0040AB18 File Offset: 0x00408D18
	public static vp_Timer.Stats EditorGetStats()
	{
		vp_Timer.Stats stats;
		stats.Created = vp_Timer.m_Active.Count + vp_Timer.m_Pool.Count;
		stats.Inactive = vp_Timer.m_Pool.Count;
		stats.Active = vp_Timer.m_Active.Count;
		return stats;
	}

	// Token: 0x0600AD95 RID: 44437 RVA: 0x0040AB64 File Offset: 0x00408D64
	public static string EditorGetMethodInfo(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return "Argument out of range.";
		}
		return vp_Timer.m_Active[eventIndex].MethodInfo;
	}

	// Token: 0x0600AD96 RID: 44438 RVA: 0x0040AB8F File Offset: 0x00408D8F
	public static int EditorGetMethodId(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return 0;
		}
		return vp_Timer.m_Active[eventIndex].Id;
	}

	// Token: 0x04008065 RID: 32869
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject m_MainObject = null;

	// Token: 0x04008066 RID: 32870
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Active = new List<vp_Timer.Event>();

	// Token: 0x04008067 RID: 32871
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Pool = new List<vp_Timer.Event>();

	// Token: 0x04008068 RID: 32872
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_Timer.Event m_NewEvent = null;

	// Token: 0x04008069 RID: 32873
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventCount = 0;

	// Token: 0x0400806A RID: 32874
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventBatch = 0;

	// Token: 0x0400806B RID: 32875
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventIterator = 0;

	// Token: 0x0400806C RID: 32876
	public static int MaxEventsPerFrame = 500;

	// Token: 0x020015C0 RID: 5568
	// (Invoke) Token: 0x0600AD9A RID: 44442
	public delegate void Callback();

	// Token: 0x020015C1 RID: 5569
	// (Invoke) Token: 0x0600AD9E RID: 44446
	public delegate void ArgCallback(object args);

	// Token: 0x020015C2 RID: 5570
	public struct Stats
	{
		// Token: 0x0400806D RID: 32877
		public int Created;

		// Token: 0x0400806E RID: 32878
		public int Inactive;

		// Token: 0x0400806F RID: 32879
		public int Active;
	}

	// Token: 0x020015C3 RID: 5571
	[PublicizedFrom(EAccessModifier.Private)]
	public class Event
	{
		// Token: 0x0600ADA1 RID: 44449 RVA: 0x0040ABF4 File Offset: 0x00408DF4
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

		// Token: 0x0600ADA2 RID: 44450 RVA: 0x0040AC98 File Offset: 0x00408E98
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

		// Token: 0x0600ADA3 RID: 44451 RVA: 0x0040ACF6 File Offset: 0x00408EF6
		[PublicizedFrom(EAccessModifier.Private)]
		public void Destroy()
		{
			vp_Timer.m_Active.Remove(this);
			vp_Timer.m_Pool.Remove(this);
		}

		// Token: 0x0600ADA4 RID: 44452 RVA: 0x0040AD10 File Offset: 0x00408F10
		[PublicizedFrom(EAccessModifier.Private)]
		public void Error(string message)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") " + message);
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x0600ADA5 RID: 44453 RVA: 0x0040AD34 File Offset: 0x00408F34
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

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x0600ADA6 RID: 44454 RVA: 0x0040ADDC File Offset: 0x00408FDC
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

		// Token: 0x04008070 RID: 32880
		public int Id;

		// Token: 0x04008071 RID: 32881
		public vp_Timer.Callback Function;

		// Token: 0x04008072 RID: 32882
		public vp_Timer.ArgCallback ArgFunction;

		// Token: 0x04008073 RID: 32883
		public object Arguments;

		// Token: 0x04008074 RID: 32884
		public int Iterations = 1;

		// Token: 0x04008075 RID: 32885
		public float Interval = -1f;

		// Token: 0x04008076 RID: 32886
		public float DueTime;

		// Token: 0x04008077 RID: 32887
		public float StartTime;

		// Token: 0x04008078 RID: 32888
		public float LifeTime;

		// Token: 0x04008079 RID: 32889
		public bool Paused;

		// Token: 0x0400807A RID: 32890
		public bool CancelOnLoad = true;
	}

	// Token: 0x020015C4 RID: 5572
	public class Handle
	{
		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x0600ADA8 RID: 44456 RVA: 0x0040AEBC File Offset: 0x004090BC
		// (set) Token: 0x0600ADA9 RID: 44457 RVA: 0x0040AED3 File Offset: 0x004090D3
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

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x0600ADAA RID: 44458 RVA: 0x0040AEE9 File Offset: 0x004090E9
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

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x0600ADAB RID: 44459 RVA: 0x0040AF04 File Offset: 0x00409104
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

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x0600ADAC RID: 44460 RVA: 0x0040AF1A File Offset: 0x0040911A
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

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x0600ADAD RID: 44461 RVA: 0x0040AF35 File Offset: 0x00409135
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

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x0600ADAE RID: 44462 RVA: 0x0040AF51 File Offset: 0x00409151
		public float Delay
		{
			get
			{
				return Mathf.Round((this.m_FirstDueTime - this.TimeOfInitiation) * 1000f) / 1000f;
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x0600ADAF RID: 44463 RVA: 0x0040AF71 File Offset: 0x00409171
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

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x0600ADB0 RID: 44464 RVA: 0x0040AF8C File Offset: 0x0040918C
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

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x0600ADB1 RID: 44465 RVA: 0x0040AFAD File Offset: 0x004091AD
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

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x0600ADB2 RID: 44466 RVA: 0x0040AFDE File Offset: 0x004091DE
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

		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x0600ADB3 RID: 44467 RVA: 0x0040B013 File Offset: 0x00409213
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

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x0600ADB4 RID: 44468 RVA: 0x0040B02E File Offset: 0x0040922E
		public int IterationsTotal
		{
			get
			{
				return this.m_StartIterations;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x0600ADB5 RID: 44469 RVA: 0x0040B036 File Offset: 0x00409236
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

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x0600ADB6 RID: 44470 RVA: 0x0040B04D File Offset: 0x0040924D
		// (set) Token: 0x0600ADB7 RID: 44471 RVA: 0x0040B058 File Offset: 0x00409258
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

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x0600ADB8 RID: 44472 RVA: 0x0040B13E File Offset: 0x0040933E
		public bool Active
		{
			get
			{
				return this.m_Event != null && this.Id != 0 && this.m_Event.Id != 0 && this.m_Event.Id == this.Id;
			}
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x0600ADB9 RID: 44473 RVA: 0x0040B172 File Offset: 0x00409372
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

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x0600ADBA RID: 44474 RVA: 0x0040B18D File Offset: 0x0040938D
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

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x0600ADBB RID: 44475 RVA: 0x0040B1A8 File Offset: 0x004093A8
		// (set) Token: 0x0600ADBC RID: 44476 RVA: 0x0040B1BF File Offset: 0x004093BF
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

		// Token: 0x0600ADBD RID: 44477 RVA: 0x0040B1F7 File Offset: 0x004093F7
		public void Cancel()
		{
			vp_Timer.Cancel(this);
		}

		// Token: 0x0600ADBE RID: 44478 RVA: 0x0040B1FF File Offset: 0x004093FF
		public void Execute()
		{
			this.m_Event.DueTime = Time.time;
		}

		// Token: 0x0400807B RID: 32891
		[PublicizedFrom(EAccessModifier.Private)]
		public vp_Timer.Event m_Event;

		// Token: 0x0400807C RID: 32892
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_Id;

		// Token: 0x0400807D RID: 32893
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_StartIterations = 1;

		// Token: 0x0400807E RID: 32894
		[PublicizedFrom(EAccessModifier.Private)]
		public float m_FirstDueTime;
	}
}
