using System;

namespace UnityEngine.Playables
{
	// Token: 0x02000490 RID: 1168
	public struct FrameData
	{
		// Token: 0x0600283F RID: 10303 RVA: 0x00044F9C File Offset: 0x0004319C
		private bool HasFlags(FrameData.Flags flag)
		{
			return (this.m_Flags & flag) == flag;
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x00044FBC File Offset: 0x000431BC
		public ulong frameId
		{
			get
			{
				return this.m_FrameID;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x00044FD4 File Offset: 0x000431D4
		public float deltaTime
		{
			get
			{
				return (float)this.m_DeltaTime;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002842 RID: 10306 RVA: 0x00044FF0 File Offset: 0x000431F0
		public float weight
		{
			get
			{
				return this.m_Weight;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002843 RID: 10307 RVA: 0x00045008 File Offset: 0x00043208
		public float effectiveWeight
		{
			get
			{
				return this.m_EffectiveWeight;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002844 RID: 10308 RVA: 0x00045020 File Offset: 0x00043220
		[Obsolete("effectiveParentDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public double effectiveParentDelay
		{
			get
			{
				return this.m_EffectiveParentDelay;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002845 RID: 10309 RVA: 0x00045038 File Offset: 0x00043238
		public float effectiveParentSpeed
		{
			get
			{
				return this.m_EffectiveParentSpeed;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x00045050 File Offset: 0x00043250
		public float effectiveSpeed
		{
			get
			{
				return this.m_EffectiveSpeed;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x00045068 File Offset: 0x00043268
		public FrameData.EvaluationType evaluationType
		{
			get
			{
				return this.HasFlags(FrameData.Flags.Evaluate) ? FrameData.EvaluationType.Evaluate : FrameData.EvaluationType.Playback;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002848 RID: 10312 RVA: 0x00045088 File Offset: 0x00043288
		public bool seekOccurred
		{
			get
			{
				return this.HasFlags(FrameData.Flags.SeekOccured);
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002849 RID: 10313 RVA: 0x000450A4 File Offset: 0x000432A4
		public bool timeLooped
		{
			get
			{
				return this.HasFlags(FrameData.Flags.Loop);
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x0600284A RID: 10314 RVA: 0x000450C0 File Offset: 0x000432C0
		public bool timeHeld
		{
			get
			{
				return this.HasFlags(FrameData.Flags.Hold);
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x0600284B RID: 10315 RVA: 0x000450DC File Offset: 0x000432DC
		public PlayableOutput output
		{
			get
			{
				return this.m_Output;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x000450F4 File Offset: 0x000432F4
		public PlayState effectivePlayState
		{
			get
			{
				bool flag = this.HasFlags(FrameData.Flags.EffectivePlayStateDelayed);
				PlayState playState;
				if (flag)
				{
					playState = PlayState.Delayed;
				}
				else
				{
					bool flag2 = this.HasFlags(FrameData.Flags.EffectivePlayStatePlaying);
					if (flag2)
					{
						playState = PlayState.Playing;
					}
					else
					{
						playState = PlayState.Paused;
					}
				}
				return playState;
			}
		}

		// Token: 0x04000F48 RID: 3912
		internal ulong m_FrameID;

		// Token: 0x04000F49 RID: 3913
		internal double m_DeltaTime;

		// Token: 0x04000F4A RID: 3914
		internal float m_Weight;

		// Token: 0x04000F4B RID: 3915
		internal float m_EffectiveWeight;

		// Token: 0x04000F4C RID: 3916
		internal double m_EffectiveParentDelay;

		// Token: 0x04000F4D RID: 3917
		internal float m_EffectiveParentSpeed;

		// Token: 0x04000F4E RID: 3918
		internal float m_EffectiveSpeed;

		// Token: 0x04000F4F RID: 3919
		internal FrameData.Flags m_Flags;

		// Token: 0x04000F50 RID: 3920
		internal PlayableOutput m_Output;

		// Token: 0x02000491 RID: 1169
		[Flags]
		internal enum Flags
		{
			// Token: 0x04000F52 RID: 3922
			Evaluate = 1,
			// Token: 0x04000F53 RID: 3923
			SeekOccured = 2,
			// Token: 0x04000F54 RID: 3924
			Loop = 4,
			// Token: 0x04000F55 RID: 3925
			Hold = 8,
			// Token: 0x04000F56 RID: 3926
			EffectivePlayStateDelayed = 16,
			// Token: 0x04000F57 RID: 3927
			EffectivePlayStatePlaying = 32
		}

		// Token: 0x02000492 RID: 1170
		public enum EvaluationType
		{
			// Token: 0x04000F59 RID: 3929
			Evaluate,
			// Token: 0x04000F5A RID: 3930
			Playback
		}
	}
}
