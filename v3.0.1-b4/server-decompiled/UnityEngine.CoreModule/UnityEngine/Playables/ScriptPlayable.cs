using System;

namespace UnityEngine.Playables
{
	// Token: 0x020004AB RID: 1195
	public struct ScriptPlayable<T> : IPlayable, IEquatable<ScriptPlayable<T>> where T : class, IPlayableBehaviour, new()
	{
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x00046A4C File Offset: 0x00044C4C
		public static ScriptPlayable<T> Null
		{
			get
			{
				return ScriptPlayable<T>.m_NullPlayable;
			}
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x00046A64 File Offset: 0x00044C64
		public static ScriptPlayable<T> Create(PlayableGraph graph, int inputCount = 0)
		{
			PlayableHandle playableHandle = ScriptPlayable<T>.CreateHandle(graph, default(T), inputCount);
			return new ScriptPlayable<T>(playableHandle);
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x00046A90 File Offset: 0x00044C90
		public static ScriptPlayable<T> Create(PlayableGraph graph, T template, int inputCount = 0)
		{
			PlayableHandle playableHandle = ScriptPlayable<T>.CreateHandle(graph, template, inputCount);
			return new ScriptPlayable<T>(playableHandle);
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x00046AB4 File Offset: 0x00044CB4
		private static PlayableHandle CreateHandle(PlayableGraph graph, T template, int inputCount)
		{
			bool flag = template == null;
			object obj;
			if (flag)
			{
				obj = ScriptPlayable<T>.CreateScriptInstance();
			}
			else
			{
				obj = ScriptPlayable<T>.CloneScriptInstance(template);
			}
			bool flag2 = obj == null;
			PlayableHandle playableHandle;
			if (flag2)
			{
				string text = "Could not create a ScriptPlayable of Type ";
				Type typeFromHandle = typeof(T);
				Debug.LogError(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
				playableHandle = PlayableHandle.Null;
			}
			else
			{
				PlayableHandle playableHandle2 = graph.CreatePlayableHandle();
				bool flag3 = !playableHandle2.IsValid();
				if (flag3)
				{
					playableHandle = PlayableHandle.Null;
				}
				else
				{
					playableHandle2.SetInputCount(inputCount);
					playableHandle2.SetScriptInstance(obj);
					playableHandle = playableHandle2;
				}
			}
			return playableHandle;
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x00046B5C File Offset: 0x00044D5C
		private static object CreateScriptInstance()
		{
			bool flag = typeof(ScriptableObject).IsAssignableFrom(typeof(T));
			IPlayableBehaviour playableBehaviour;
			if (flag)
			{
				playableBehaviour = ScriptableObject.CreateInstance(typeof(T)) as T;
			}
			else
			{
				playableBehaviour = new T();
			}
			return playableBehaviour;
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x00046BBC File Offset: 0x00044DBC
		private static object CloneScriptInstance(IPlayableBehaviour source)
		{
			Object @object = source as Object;
			bool flag = @object != null;
			object obj;
			if (flag)
			{
				obj = ScriptPlayable<T>.CloneScriptInstanceFromEngineObject(@object);
			}
			else
			{
				ICloneable cloneable = source as ICloneable;
				bool flag2 = cloneable != null;
				if (flag2)
				{
					obj = ScriptPlayable<T>.CloneScriptInstanceFromIClonable(cloneable);
				}
				else
				{
					obj = null;
				}
			}
			return obj;
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x00046C04 File Offset: 0x00044E04
		private static object CloneScriptInstanceFromEngineObject(Object source)
		{
			Object @object = Object.Instantiate(source);
			bool flag = @object != null;
			if (flag)
			{
				@object.hideFlags |= HideFlags.DontSave;
			}
			return @object;
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x00046C3C File Offset: 0x00044E3C
		private static object CloneScriptInstanceFromIClonable(ICloneable source)
		{
			return source.Clone();
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x00046C54 File Offset: 0x00044E54
		internal ScriptPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !typeof(T).IsAssignableFrom(handle.GetPlayableType());
				if (flag2)
				{
					throw new InvalidCastException(string.Format("Incompatible handle: Trying to assign a playable data of type `{0}` that is not compatible with the PlayableBehaviour of type `{1}`.", handle.GetPlayableType(), typeof(T)));
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x00046CB4 File Offset: 0x00044EB4
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x00046CCC File Offset: 0x00044ECC
		public T GetBehaviour()
		{
			return this.m_Handle.GetObject<T>();
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x00046CEC File Offset: 0x00044EEC
		public static implicit operator Playable(ScriptPlayable<T> playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x00046D0C File Offset: 0x00044F0C
		public static explicit operator ScriptPlayable<T>(Playable playable)
		{
			return new ScriptPlayable<T>(playable.GetHandle());
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x00046D2C File Offset: 0x00044F2C
		public bool Equals(ScriptPlayable<T> other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x04000F8C RID: 3980
		private PlayableHandle m_Handle;

		// Token: 0x04000F8D RID: 3981
		private static readonly ScriptPlayable<T> m_NullPlayable = new ScriptPlayable<T>(PlayableHandle.Null);
	}
}
