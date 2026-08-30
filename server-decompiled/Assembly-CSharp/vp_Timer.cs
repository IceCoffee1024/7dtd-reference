using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200158B RID: 5515
public class vp_Timer : MonoBehaviour
{
	// Token: 0x170013EC RID: 5100
	// (get) Token: 0x0600AC77 RID: 44151 RVA: 0x00403418 File Offset: 0x00401618
	public bool WasAddedCorrectly
	{
		get
		{
			return Application.isPlaying && !(base.gameObject != vp_Timer.m_MainObject);
		}
	}

	// Token: 0x0600AC78 RID: 44152 RVA: 0x00403438 File Offset: 0x00401638
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (!this.WasAddedCorrectly)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x0600AC79 RID: 44153 RVA: 0x0040344C File Offset: 0x0040164C
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

	// Token: 0x0600AC7A RID: 44154 RVA: 0x00403575 File Offset: 0x00401775
	public static void In(float delay, vp_Timer.Callback callback, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AC7B RID: 44155 RVA: 0x00403587 File Offset: 0x00401787
	public static void In(float delay, vp_Timer.Callback callback, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AC7C RID: 44156 RVA: 0x00403599 File Offset: 0x00401799
	public static void In(float delay, vp_Timer.Callback callback, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, interval);
	}

	// Token: 0x0600AC7D RID: 44157 RVA: 0x004035A8 File Offset: 0x004017A8
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, 1, -1f);
	}

	// Token: 0x0600AC7E RID: 44158 RVA: 0x004035BA File Offset: 0x004017BA
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, -1f);
	}

	// Token: 0x0600AC7F RID: 44159 RVA: 0x004035CD File Offset: 0x004017CD
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, interval);
	}

	// Token: 0x0600AC80 RID: 44160 RVA: 0x004035DD File Offset: 0x004017DD
	public static void Start(vp_Timer.Handle timerHandle)
	{
		vp_Timer.Schedule(315360000f, delegate
		{
		}, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x0600AC81 RID: 44161 RVA: 0x00403614 File Offset: 0x00401814
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

	// Token: 0x0600AC82 RID: 44162 RVA: 0x0040379D File Offset: 0x0040199D
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

	// Token: 0x0600AC83 RID: 44163 RVA: 0x004037B4 File Offset: 0x004019B4
	public static void CancelAll()
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			vp_Timer.m_Active[i].Id = 0;
		}
	}

	// Token: 0x0600AC84 RID: 44164 RVA: 0x004037EC File Offset: 0x004019EC
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

	// Token: 0x0600AC85 RID: 44165 RVA: 0x00403839 File Offset: 0x00401A39
	public static void DestroyAll()
	{
		vp_Timer.m_Active.Clear();
		vp_Timer.m_Pool.Clear();
	}

	// Token: 0x0600AC86 RID: 44166 RVA: 0x0040384F File Offset: 0x00401A4F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		SceneManager.sceneLoaded += this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AC87 RID: 44167 RVA: 0x00403862 File Offset: 0x00401A62
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		SceneManager.sceneLoaded -= this.NotifyLevelWasLoaded;
	}

	// Token: 0x0600AC88 RID: 44168 RVA: 0x00403878 File Offset: 0x00401A78
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

	// Token: 0x0600AC89 RID: 44169 RVA: 0x004038C0 File Offset: 0x00401AC0
	public static vp_Timer.Stats EditorGetStats()
	{
		vp_Timer.Stats stats;
		stats.Created = vp_Timer.m_Active.Count + vp_Timer.m_Pool.Count;
		stats.Inactive = vp_Timer.m_Pool.Count;
		stats.Active = vp_Timer.m_Active.Count;
		return stats;
	}

	// Token: 0x0600AC8A RID: 44170 RVA: 0x0040390C File Offset: 0x00401B0C
	public static string EditorGetMethodInfo(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return "Argument out of range.";
		}
		return vp_Timer.m_Active[eventIndex].MethodInfo;
	}

	// Token: 0x0600AC8B RID: 44171 RVA: 0x00403937 File Offset: 0x00401B37
	public static int EditorGetMethodId(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return 0;
		}
		return vp_Timer.m_Active[eventIndex].Id;
	}

	// Token: 0x04007FA1 RID: 32673
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject m_MainObject = null;

	// Token: 0x04007FA2 RID: 32674
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Active = new List<vp_Timer.Event>();

	// Token: 0x04007FA3 RID: 32675
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<vp_Timer.Event> m_Pool = new List<vp_Timer.Event>();

	// Token: 0x04007FA4 RID: 32676
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static vp_Timer.Event m_NewEvent = null;

	// Token: 0x04007FA5 RID: 32677
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventCount = 0;

	// Token: 0x04007FA6 RID: 32678
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventBatch = 0;

	// Token: 0x04007FA7 RID: 32679
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int m_EventIterator = 0;

	// Token: 0x04007FA8 RID: 32680
	public static int MaxEventsPerFrame = 500;

	// Token: 0x0200158C RID: 5516
	// (Invoke) Token: 0x0600AC8F RID: 44175
	public delegate void Callback();

	// Token: 0x0200158D RID: 5517
	// (Invoke) Token: 0x0600AC93 RID: 44179
	public delegate void ArgCallback(object args);

	// Token: 0x0200158E RID: 5518
	public struct Stats
	{
		// Token: 0x04007FA9 RID: 32681
		public int Created;

		// Token: 0x04007FAA RID: 32682
		public int Inactive;

		// Token: 0x04007FAB RID: 32683
		public int Active;
	}

	// Token: 0x0200158F RID: 5519
	[PublicizedFrom(EAccessModifier.Private)]
	public class Event
	{
		// Token: 0x0600AC96 RID: 44182 RVA: 0x0040399C File Offset: 0x00401B9C
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

		// Token: 0x0600AC97 RID: 44183 RVA: 0x00403A40 File Offset: 0x00401C40
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

		// Token: 0x0600AC98 RID: 44184 RVA: 0x00403A9E File Offset: 0x00401C9E
		[PublicizedFrom(EAccessModifier.Private)]
		public void Destroy()
		{
			vp_Timer.m_Active.Remove(this);
			vp_Timer.m_Pool.Remove(this);
		}

		// Token: 0x0600AC99 RID: 44185 RVA: 0x00403AB8 File Offset: 0x00401CB8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Error(string message)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") " + message);
		}

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x0600AC9A RID: 44186 RVA: 0x00403ADC File Offset: 0x00401CDC
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

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x0600AC9B RID: 44187 RVA: 0x00403B84 File Offset: 0x00401D84
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

		// Token: 0x04007FAC RID: 32684
		public int Id;

		// Token: 0x04007FAD RID: 32685
		public vp_Timer.Callback Function;

		// Token: 0x04007FAE RID: 32686
		public vp_Timer.ArgCallback ArgFunction;

		// Token: 0x04007FAF RID: 32687
		public object Arguments;

		// Token: 0x04007FB0 RID: 32688
		public int Iterations = 1;

		// Token: 0x04007FB1 RID: 32689
		public float Interval = -1f;

		// Token: 0x04007FB2 RID: 32690
		public float DueTime;

		// Token: 0x04007FB3 RID: 32691
		public float StartTime;

		// Token: 0x04007FB4 RID: 32692
		public float LifeTime;

		// Token: 0x04007FB5 RID: 32693
		public bool Paused;

		// Token: 0x04007FB6 RID: 32694
		public bool CancelOnLoad = true;
	}

	// Token: 0x02001590 RID: 5520
	public class Handle
	{
		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x0600AC9D RID: 44189 RVA: 0x00403C64 File Offset: 0x00401E64
		// (set) Token: 0x0600AC9E RID: 44190 RVA: 0x00403C7B File Offset: 0x00401E7B
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

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x0600AC9F RID: 44191 RVA: 0x00403C91 File Offset: 0x00401E91
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

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x0600ACA0 RID: 44192 RVA: 0x00403CAC File Offset: 0x00401EAC
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

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x0600ACA1 RID: 44193 RVA: 0x00403CC2 File Offset: 0x00401EC2
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

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x0600ACA2 RID: 44194 RVA: 0x00403CDD File Offset: 0x00401EDD
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

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x0600ACA3 RID: 44195 RVA: 0x00403CF9 File Offset: 0x00401EF9
		public float Delay
		{
			get
			{
				return Mathf.Round((this.m_FirstDueTime - this.TimeOfInitiation) * 1000f) / 1000f;
			}
		}

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x0600ACA4 RID: 44196 RVA: 0x00403D19 File Offset: 0x00401F19
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

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x0600ACA5 RID: 44197 RVA: 0x00403D34 File Offset: 0x00401F34
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

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x0600ACA6 RID: 44198 RVA: 0x00403D55 File Offset: 0x00401F55
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

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x0600ACA7 RID: 44199 RVA: 0x00403D86 File Offset: 0x00401F86
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

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x0600ACA8 RID: 44200 RVA: 0x00403DBB File Offset: 0x00401FBB
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

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x0600ACA9 RID: 44201 RVA: 0x00403DD6 File Offset: 0x00401FD6
		public int IterationsTotal
		{
			get
			{
				return this.m_StartIterations;
			}
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x0600ACAA RID: 44202 RVA: 0x00403DDE File Offset: 0x00401FDE
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

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x0600ACAB RID: 44203 RVA: 0x00403DF5 File Offset: 0x00401FF5
		// (set) Token: 0x0600ACAC RID: 44204 RVA: 0x00403E00 File Offset: 0x00402000
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

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x0600ACAD RID: 44205 RVA: 0x00403EE6 File Offset: 0x004020E6
		public bool Active
		{
			get
			{
				return this.m_Event != null && this.Id != 0 && this.m_Event.Id != 0 && this.m_Event.Id == this.Id;
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x0600ACAE RID: 44206 RVA: 0x00403F1A File Offset: 0x0040211A
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

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x0600ACAF RID: 44207 RVA: 0x00403F35 File Offset: 0x00402135
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

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x0600ACB0 RID: 44208 RVA: 0x00403F50 File Offset: 0x00402150
		// (set) Token: 0x0600ACB1 RID: 44209 RVA: 0x00403F67 File Offset: 0x00402167
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

		// Token: 0x0600ACB2 RID: 44210 RVA: 0x00403F9F File Offset: 0x0040219F
		public void Cancel()
		{
			vp_Timer.Cancel(this);
		}

		// Token: 0x0600ACB3 RID: 44211 RVA: 0x00403FA7 File Offset: 0x004021A7
		public void Execute()
		{
			this.m_Event.DueTime = Time.time;
		}

		// Token: 0x04007FB7 RID: 32695
		[PublicizedFrom(EAccessModifier.Private)]
		public vp_Timer.Event m_Event;

		// Token: 0x04007FB8 RID: 32696
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_Id;

		// Token: 0x04007FB9 RID: 32697
		[PublicizedFrom(EAccessModifier.Private)]
		public int m_StartIterations = 1;

		// Token: 0x04007FBA RID: 32698
		[PublicizedFrom(EAccessModifier.Private)]
		public float m_FirstDueTime;
	}
}
