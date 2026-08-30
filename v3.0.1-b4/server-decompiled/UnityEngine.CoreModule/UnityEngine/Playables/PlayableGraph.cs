using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x020004A5 RID: 1189
	[NativeHeader("Runtime/Director/Core/HPlayableGraph.h")]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[NativeHeader("Runtime/Export/Director/PlayableGraph.bindings.h")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Director/Core/HPlayableOutput.h")]
	public struct PlayableGraph
	{
		// Token: 0x060028C7 RID: 10439 RVA: 0x00045D9C File Offset: 0x00043F9C
		public Playable GetRootPlayable(int index)
		{
			PlayableHandle rootPlayableInternal = this.GetRootPlayableInternal(index);
			return new Playable(rootPlayableInternal);
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x00045DBC File Offset: 0x00043FBC
		public bool Connect<U, V>(U source, int sourceOutputPort, V destination, int destinationInputPort) where U : struct, IPlayable where V : struct, IPlayable
		{
			return this.ConnectInternal(source.GetHandle(), sourceOutputPort, destination.GetHandle(), destinationInputPort);
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x00045DF1 File Offset: 0x00043FF1
		public void Disconnect<U>(U input, int inputPort) where U : struct, IPlayable
		{
			this.DisconnectInternal(input.GetHandle(), inputPort);
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x00045E09 File Offset: 0x00044009
		public void DestroyPlayable<U>(U playable) where U : struct, IPlayable
		{
			this.DestroyPlayableInternal(playable.GetHandle());
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x00045E20 File Offset: 0x00044020
		public void DestroySubgraph<U>(U playable) where U : struct, IPlayable
		{
			this.DestroySubgraphInternal(playable.GetHandle());
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x00045E37 File Offset: 0x00044037
		public void DestroyOutput<U>(U output) where U : struct, IPlayableOutput
		{
			this.DestroyOutputInternal(output.GetHandle());
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x00045E50 File Offset: 0x00044050
		public int GetOutputCountByType<T>() where T : struct, IPlayableOutput
		{
			return this.GetOutputCountByTypeInternal(typeof(T));
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x00045E74 File Offset: 0x00044074
		public PlayableOutput GetOutput(int index)
		{
			PlayableOutputHandle playableOutputHandle;
			bool flag = !this.GetOutputInternal(index, out playableOutputHandle);
			PlayableOutput playableOutput;
			if (flag)
			{
				playableOutput = PlayableOutput.Null;
			}
			else
			{
				playableOutput = new PlayableOutput(playableOutputHandle);
			}
			return playableOutput;
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x00045EA4 File Offset: 0x000440A4
		public PlayableOutput GetOutputByType<T>(int index) where T : struct, IPlayableOutput
		{
			PlayableOutputHandle playableOutputHandle;
			bool flag = !this.GetOutputByTypeInternal(typeof(T), index, out playableOutputHandle);
			PlayableOutput playableOutput;
			if (flag)
			{
				playableOutput = PlayableOutput.Null;
			}
			else
			{
				playableOutput = new PlayableOutput(playableOutputHandle);
			}
			return playableOutput;
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x00045EDE File Offset: 0x000440DE
		public void Evaluate()
		{
			this.Evaluate(0f);
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x00045EF0 File Offset: 0x000440F0
		public static PlayableGraph Create()
		{
			return PlayableGraph.Create(null);
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x00045F08 File Offset: 0x00044108
		public static PlayableGraph Create(string name)
		{
			PlayableGraph playableGraph;
			PlayableGraph.Create_Injected(name, out playableGraph);
			return playableGraph;
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x00045F1E File Offset: 0x0004411E
		[FreeFunction("PlayableGraphBindings::Destroy", HasExplicitThis = true, ThrowsException = true)]
		public void Destroy()
		{
			PlayableGraph.Destroy_Injected(ref this);
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x00045F26 File Offset: 0x00044126
		public bool IsValid()
		{
			return PlayableGraph.IsValid_Injected(ref this);
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x00045F2E File Offset: 0x0004412E
		[FreeFunction("PlayableGraphBindings::IsPlaying", HasExplicitThis = true, ThrowsException = true)]
		public bool IsPlaying()
		{
			return PlayableGraph.IsPlaying_Injected(ref this);
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x00045F36 File Offset: 0x00044136
		[FreeFunction("PlayableGraphBindings::IsDone", HasExplicitThis = true, ThrowsException = true)]
		public bool IsDone()
		{
			return PlayableGraph.IsDone_Injected(ref this);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x00045F3E File Offset: 0x0004413E
		[FreeFunction("PlayableGraphBindings::Play", HasExplicitThis = true, ThrowsException = true)]
		public void Play()
		{
			PlayableGraph.Play_Injected(ref this);
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x00045F46 File Offset: 0x00044146
		[FreeFunction("PlayableGraphBindings::Stop", HasExplicitThis = true, ThrowsException = true)]
		public void Stop()
		{
			PlayableGraph.Stop_Injected(ref this);
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x00045F4E File Offset: 0x0004414E
		[FreeFunction("PlayableGraphBindings::Evaluate", HasExplicitThis = true, ThrowsException = true)]
		public void Evaluate([DefaultValue("0")] float deltaTime)
		{
			PlayableGraph.Evaluate_Injected(ref this, deltaTime);
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x00045F57 File Offset: 0x00044157
		[FreeFunction("PlayableGraphBindings::GetTimeUpdateMode", HasExplicitThis = true, ThrowsException = true)]
		public DirectorUpdateMode GetTimeUpdateMode()
		{
			return PlayableGraph.GetTimeUpdateMode_Injected(ref this);
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x00045F5F File Offset: 0x0004415F
		[FreeFunction("PlayableGraphBindings::SetTimeUpdateMode", HasExplicitThis = true, ThrowsException = true)]
		public void SetTimeUpdateMode(DirectorUpdateMode value)
		{
			PlayableGraph.SetTimeUpdateMode_Injected(ref this, value);
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x00045F68 File Offset: 0x00044168
		[FreeFunction("PlayableGraphBindings::GetResolver", HasExplicitThis = true, ThrowsException = true)]
		public IExposedPropertyTable GetResolver()
		{
			return PlayableGraph.GetResolver_Injected(ref this);
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x00045F70 File Offset: 0x00044170
		[FreeFunction("PlayableGraphBindings::SetResolver", HasExplicitThis = true, ThrowsException = true)]
		public void SetResolver(IExposedPropertyTable value)
		{
			PlayableGraph.SetResolver_Injected(ref this, value);
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x00045F79 File Offset: 0x00044179
		[FreeFunction("PlayableGraphBindings::GetPlayableCount", HasExplicitThis = true, ThrowsException = true)]
		public int GetPlayableCount()
		{
			return PlayableGraph.GetPlayableCount_Injected(ref this);
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x00045F81 File Offset: 0x00044181
		[FreeFunction("PlayableGraphBindings::GetRootPlayableCount", HasExplicitThis = true, ThrowsException = true)]
		public int GetRootPlayableCount()
		{
			return PlayableGraph.GetRootPlayableCount_Injected(ref this);
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x00045F89 File Offset: 0x00044189
		[FreeFunction("PlayableGraphBindings::SynchronizeEvaluation", HasExplicitThis = true, ThrowsException = true)]
		internal void SynchronizeEvaluation(PlayableGraph playable)
		{
			PlayableGraph.SynchronizeEvaluation_Injected(ref this, ref playable);
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x00045F93 File Offset: 0x00044193
		[FreeFunction("PlayableGraphBindings::GetOutputCount", HasExplicitThis = true, ThrowsException = true)]
		public int GetOutputCount()
		{
			return PlayableGraph.GetOutputCount_Injected(ref this);
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x00045F9C File Offset: 0x0004419C
		[FreeFunction("PlayableGraphBindings::CreatePlayableHandle", HasExplicitThis = true, ThrowsException = true)]
		internal PlayableHandle CreatePlayableHandle()
		{
			PlayableHandle playableHandle;
			PlayableGraph.CreatePlayableHandle_Injected(ref this, out playableHandle);
			return playableHandle;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x00045FB2 File Offset: 0x000441B2
		[FreeFunction("PlayableGraphBindings::CreateScriptOutputInternal", HasExplicitThis = true, ThrowsException = true)]
		internal bool CreateScriptOutputInternal(string name, out PlayableOutputHandle handle)
		{
			return PlayableGraph.CreateScriptOutputInternal_Injected(ref this, name, out handle);
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x00045FBC File Offset: 0x000441BC
		[FreeFunction("PlayableGraphBindings::GetRootPlayableInternal", HasExplicitThis = true, ThrowsException = true)]
		internal PlayableHandle GetRootPlayableInternal(int index)
		{
			PlayableHandle playableHandle;
			PlayableGraph.GetRootPlayableInternal_Injected(ref this, index, out playableHandle);
			return playableHandle;
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x00045FD3 File Offset: 0x000441D3
		[FreeFunction("PlayableGraphBindings::DestroyOutputInternal", HasExplicitThis = true, ThrowsException = true)]
		internal void DestroyOutputInternal(PlayableOutputHandle handle)
		{
			PlayableGraph.DestroyOutputInternal_Injected(ref this, ref handle);
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x00045FDD File Offset: 0x000441DD
		[FreeFunction("PlayableGraphBindings::IsMatchFrameRateEnabled", HasExplicitThis = true, ThrowsException = true)]
		internal bool IsMatchFrameRateEnabled()
		{
			return PlayableGraph.IsMatchFrameRateEnabled_Injected(ref this);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x00045FE5 File Offset: 0x000441E5
		[FreeFunction("PlayableGraphBindings::EnableMatchFrameRate", HasExplicitThis = true, ThrowsException = true)]
		internal void EnableMatchFrameRate(FrameRate frameRate)
		{
			PlayableGraph.EnableMatchFrameRate_Injected(ref this, ref frameRate);
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x00045FEF File Offset: 0x000441EF
		[FreeFunction("PlayableGraphBindings::DisableMatchFrameRate", HasExplicitThis = true, ThrowsException = true)]
		internal void DisableMatchFrameRate()
		{
			PlayableGraph.DisableMatchFrameRate_Injected(ref this);
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x00045FF8 File Offset: 0x000441F8
		[FreeFunction("PlayableGraphBindings::GetFrameRate", HasExplicitThis = true, ThrowsException = true)]
		internal FrameRate GetFrameRate()
		{
			FrameRate frameRate;
			PlayableGraph.GetFrameRate_Injected(ref this, out frameRate);
			return frameRate;
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x0004600E File Offset: 0x0004420E
		[FreeFunction("PlayableGraphBindings::GetOutputInternal", HasExplicitThis = true, ThrowsException = true)]
		private bool GetOutputInternal(int index, out PlayableOutputHandle handle)
		{
			return PlayableGraph.GetOutputInternal_Injected(ref this, index, out handle);
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x00046018 File Offset: 0x00044218
		[FreeFunction("PlayableGraphBindings::GetOutputCountByTypeInternal", HasExplicitThis = true, ThrowsException = true)]
		private int GetOutputCountByTypeInternal(Type outputType)
		{
			return PlayableGraph.GetOutputCountByTypeInternal_Injected(ref this, outputType);
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x00046021 File Offset: 0x00044221
		[FreeFunction("PlayableGraphBindings::GetOutputByTypeInternal", HasExplicitThis = true, ThrowsException = true)]
		private bool GetOutputByTypeInternal(Type outputType, int index, out PlayableOutputHandle handle)
		{
			return PlayableGraph.GetOutputByTypeInternal_Injected(ref this, outputType, index, out handle);
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0004602C File Offset: 0x0004422C
		[FreeFunction("PlayableGraphBindings::ConnectInternal", HasExplicitThis = true, ThrowsException = true)]
		private bool ConnectInternal(PlayableHandle source, int sourceOutputPort, PlayableHandle destination, int destinationInputPort)
		{
			return PlayableGraph.ConnectInternal_Injected(ref this, ref source, sourceOutputPort, ref destination, destinationInputPort);
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x0004603B File Offset: 0x0004423B
		[FreeFunction("PlayableGraphBindings::DisconnectInternal", HasExplicitThis = true, ThrowsException = true)]
		private void DisconnectInternal(PlayableHandle playable, int inputPort)
		{
			PlayableGraph.DisconnectInternal_Injected(ref this, ref playable, inputPort);
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x00046046 File Offset: 0x00044246
		[FreeFunction("PlayableGraphBindings::DestroyPlayableInternal", HasExplicitThis = true, ThrowsException = true)]
		private void DestroyPlayableInternal(PlayableHandle playable)
		{
			PlayableGraph.DestroyPlayableInternal_Injected(ref this, ref playable);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x00046050 File Offset: 0x00044250
		[FreeFunction("PlayableGraphBindings::DestroySubgraphInternal", HasExplicitThis = true, ThrowsException = true)]
		private void DestroySubgraphInternal(PlayableHandle playable)
		{
			PlayableGraph.DestroySubgraphInternal_Injected(ref this, ref playable);
		}

		// Token: 0x060028F1 RID: 10481
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Create_Injected(string name, out PlayableGraph ret);

		// Token: 0x060028F2 RID: 10482
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028F3 RID: 10483
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValid_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028F4 RID: 10484
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsPlaying_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028F5 RID: 10485
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDone_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028F6 RID: 10486
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Play_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028F7 RID: 10487
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Stop_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028F8 RID: 10488
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Evaluate_Injected(ref PlayableGraph _unity_self, [DefaultValue("0")] float deltaTime);

		// Token: 0x060028F9 RID: 10489
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern DirectorUpdateMode GetTimeUpdateMode_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028FA RID: 10490
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTimeUpdateMode_Injected(ref PlayableGraph _unity_self, DirectorUpdateMode value);

		// Token: 0x060028FB RID: 10491
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IExposedPropertyTable GetResolver_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028FC RID: 10492
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetResolver_Injected(ref PlayableGraph _unity_self, IExposedPropertyTable value);

		// Token: 0x060028FD RID: 10493
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPlayableCount_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028FE RID: 10494
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRootPlayableCount_Injected(ref PlayableGraph _unity_self);

		// Token: 0x060028FF RID: 10495
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SynchronizeEvaluation_Injected(ref PlayableGraph _unity_self, ref PlayableGraph playable);

		// Token: 0x06002900 RID: 10496
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetOutputCount_Injected(ref PlayableGraph _unity_self);

		// Token: 0x06002901 RID: 10497
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreatePlayableHandle_Injected(ref PlayableGraph _unity_self, out PlayableHandle ret);

		// Token: 0x06002902 RID: 10498
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CreateScriptOutputInternal_Injected(ref PlayableGraph _unity_self, string name, out PlayableOutputHandle handle);

		// Token: 0x06002903 RID: 10499
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRootPlayableInternal_Injected(ref PlayableGraph _unity_self, int index, out PlayableHandle ret);

		// Token: 0x06002904 RID: 10500
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyOutputInternal_Injected(ref PlayableGraph _unity_self, ref PlayableOutputHandle handle);

		// Token: 0x06002905 RID: 10501
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsMatchFrameRateEnabled_Injected(ref PlayableGraph _unity_self);

		// Token: 0x06002906 RID: 10502
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableMatchFrameRate_Injected(ref PlayableGraph _unity_self, ref FrameRate frameRate);

		// Token: 0x06002907 RID: 10503
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisableMatchFrameRate_Injected(ref PlayableGraph _unity_self);

		// Token: 0x06002908 RID: 10504
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetFrameRate_Injected(ref PlayableGraph _unity_self, out FrameRate ret);

		// Token: 0x06002909 RID: 10505
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetOutputInternal_Injected(ref PlayableGraph _unity_self, int index, out PlayableOutputHandle handle);

		// Token: 0x0600290A RID: 10506
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetOutputCountByTypeInternal_Injected(ref PlayableGraph _unity_self, Type outputType);

		// Token: 0x0600290B RID: 10507
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetOutputByTypeInternal_Injected(ref PlayableGraph _unity_self, Type outputType, int index, out PlayableOutputHandle handle);

		// Token: 0x0600290C RID: 10508
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ConnectInternal_Injected(ref PlayableGraph _unity_self, ref PlayableHandle source, int sourceOutputPort, ref PlayableHandle destination, int destinationInputPort);

		// Token: 0x0600290D RID: 10509
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisconnectInternal_Injected(ref PlayableGraph _unity_self, ref PlayableHandle playable, int inputPort);

		// Token: 0x0600290E RID: 10510
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyPlayableInternal_Injected(ref PlayableGraph _unity_self, ref PlayableHandle playable);

		// Token: 0x0600290F RID: 10511
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroySubgraphInternal_Injected(ref PlayableGraph _unity_self, ref PlayableHandle playable);

		// Token: 0x04000F7E RID: 3966
		internal IntPtr m_Handle;

		// Token: 0x04000F7F RID: 3967
		internal uint m_Version;
	}
}
