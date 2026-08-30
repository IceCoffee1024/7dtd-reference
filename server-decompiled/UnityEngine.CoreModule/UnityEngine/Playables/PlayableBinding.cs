using System;
using System.ComponentModel;
using UnityEngine.Bindings;

namespace UnityEngine.Playables
{
	// Token: 0x020004A0 RID: 1184
	public struct PlayableBinding
	{
		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x00045574 File Offset: 0x00043774
		// (set) Token: 0x0600288C RID: 10380 RVA: 0x0004558C File Offset: 0x0004378C
		public string streamName
		{
			get
			{
				return this.m_StreamName;
			}
			set
			{
				this.m_StreamName = value;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x00045598 File Offset: 0x00043798
		// (set) Token: 0x0600288E RID: 10382 RVA: 0x000455B0 File Offset: 0x000437B0
		public Object sourceObject
		{
			get
			{
				return this.m_SourceObject;
			}
			set
			{
				this.m_SourceObject = value;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x000455BC File Offset: 0x000437BC
		public Type outputTargetType
		{
			get
			{
				return this.m_SourceBindingType;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002890 RID: 10384 RVA: 0x000455D4 File Offset: 0x000437D4
		// (set) Token: 0x06002891 RID: 10385 RVA: 0x00002669 File Offset: 0x00000869
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("sourceBindingType is no longer supported on PlayableBinding. Use outputBindingType instead to get the required output target type, and the appropriate binding create method (e.g. AnimationPlayableBinding.Create(name, key)) to create PlayableBindings", true)]
		public Type sourceBindingType
		{
			get
			{
				return this.m_SourceBindingType;
			}
			set
			{
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002892 RID: 10386 RVA: 0x000455EC File Offset: 0x000437EC
		// (set) Token: 0x06002893 RID: 10387 RVA: 0x00002669 File Offset: 0x00000869
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("streamType is no longer supported on PlayableBinding. Use the appropriate binding create method (e.g. AnimationPlayableBinding.Create(name, key)) instead.", true)]
		public DataStreamType streamType
		{
			get
			{
				return DataStreamType.None;
			}
			set
			{
			}
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x00045600 File Offset: 0x00043800
		internal PlayableOutput CreateOutput(PlayableGraph graph)
		{
			bool flag = this.m_CreateOutputMethod != null;
			PlayableOutput playableOutput;
			if (flag)
			{
				playableOutput = this.m_CreateOutputMethod(graph, this.m_StreamName);
			}
			else
			{
				playableOutput = PlayableOutput.Null;
			}
			return playableOutput;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x0004563C File Offset: 0x0004383C
		[VisibleToOtherModules]
		internal static PlayableBinding CreateInternal(string name, Object sourceObject, Type sourceType, PlayableBinding.CreateOutputMethod createFunction)
		{
			return new PlayableBinding
			{
				m_StreamName = name,
				m_SourceObject = sourceObject,
				m_SourceBindingType = sourceType,
				m_CreateOutputMethod = createFunction
			};
		}

		// Token: 0x04000F70 RID: 3952
		private string m_StreamName;

		// Token: 0x04000F71 RID: 3953
		private Object m_SourceObject;

		// Token: 0x04000F72 RID: 3954
		private Type m_SourceBindingType;

		// Token: 0x04000F73 RID: 3955
		private PlayableBinding.CreateOutputMethod m_CreateOutputMethod;

		// Token: 0x04000F74 RID: 3956
		public static readonly PlayableBinding[] None = new PlayableBinding[0];

		// Token: 0x04000F75 RID: 3957
		public static readonly double DefaultDuration = double.PositiveInfinity;

		// Token: 0x020004A1 RID: 1185
		// (Invoke) Token: 0x06002898 RID: 10392
		[VisibleToOtherModules]
		internal delegate PlayableOutput CreateOutputMethod(PlayableGraph graph, string name);
	}
}
