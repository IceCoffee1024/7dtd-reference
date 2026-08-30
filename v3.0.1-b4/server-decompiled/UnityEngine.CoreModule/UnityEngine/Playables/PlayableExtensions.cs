using System;

namespace UnityEngine.Playables
{
	// Token: 0x020004A3 RID: 1187
	public static class PlayableExtensions
	{
		// Token: 0x0600289B RID: 10395 RVA: 0x00045694 File Offset: 0x00043894
		public static bool IsNull<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsNull();
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000456BC File Offset: 0x000438BC
		public static bool IsValid<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsValid();
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000456E4 File Offset: 0x000438E4
		public static void Destroy<U>(this U playable) where U : struct, IPlayable
		{
			playable.GetHandle().Destroy();
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x00045708 File Offset: 0x00043908
		public static PlayableGraph GetGraph<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetGraph();
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x00045730 File Offset: 0x00043930
		[Obsolete("SetPlayState() has been deprecated. Use Play(), Pause() or SetDelay() instead", false)]
		public static void SetPlayState<U>(this U playable, PlayState value) where U : struct, IPlayable
		{
			bool flag = value == PlayState.Delayed;
			if (flag)
			{
				throw new ArgumentException("Can't set Delayed: use SetDelay() instead");
			}
			if (value != PlayState.Paused)
			{
				if (value == PlayState.Playing)
				{
					playable.GetHandle().Play();
				}
			}
			else
			{
				playable.GetHandle().Pause();
			}
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x00045790 File Offset: 0x00043990
		public static PlayState GetPlayState<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetPlayState();
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000457B8 File Offset: 0x000439B8
		public static void Play<U>(this U playable) where U : struct, IPlayable
		{
			playable.GetHandle().Play();
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000457DC File Offset: 0x000439DC
		public static void Pause<U>(this U playable) where U : struct, IPlayable
		{
			playable.GetHandle().Pause();
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x00045800 File Offset: 0x00043A00
		public static void SetSpeed<U>(this U playable, double value) where U : struct, IPlayable
		{
			playable.GetHandle().SetSpeed(value);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x00045828 File Offset: 0x00043A28
		public static double GetSpeed<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetSpeed();
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x00045850 File Offset: 0x00043A50
		public static void SetDuration<U>(this U playable, double value) where U : struct, IPlayable
		{
			playable.GetHandle().SetDuration(value);
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x00045878 File Offset: 0x00043A78
		public static double GetDuration<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetDuration();
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000458A0 File Offset: 0x00043AA0
		public static void SetTime<U>(this U playable, double value) where U : struct, IPlayable
		{
			playable.GetHandle().SetTime(value);
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000458C8 File Offset: 0x00043AC8
		public static double GetTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetTime();
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000458F0 File Offset: 0x00043AF0
		public static double GetPreviousTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetPreviousTime();
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x00045918 File Offset: 0x00043B18
		public static void SetDone<U>(this U playable, bool value) where U : struct, IPlayable
		{
			playable.GetHandle().SetDone(value);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x00045940 File Offset: 0x00043B40
		public static bool IsDone<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsDone();
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x00045968 File Offset: 0x00043B68
		public static void SetPropagateSetTime<U>(this U playable, bool value) where U : struct, IPlayable
		{
			playable.GetHandle().SetPropagateSetTime(value);
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x00045990 File Offset: 0x00043B90
		public static bool GetPropagateSetTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetPropagateSetTime();
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000459B8 File Offset: 0x00043BB8
		public static bool CanChangeInputs<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().CanChangeInputs();
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000459E0 File Offset: 0x00043BE0
		public static bool CanSetWeights<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().CanSetWeights();
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x00045A08 File Offset: 0x00043C08
		public static bool CanDestroy<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().CanDestroy();
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x00045A30 File Offset: 0x00043C30
		public static void SetInputCount<U>(this U playable, int value) where U : struct, IPlayable
		{
			playable.GetHandle().SetInputCount(value);
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x00045A58 File Offset: 0x00043C58
		public static int GetInputCount<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetInputCount();
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x00045A80 File Offset: 0x00043C80
		public static void SetOutputCount<U>(this U playable, int value) where U : struct, IPlayable
		{
			playable.GetHandle().SetOutputCount(value);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x00045AA8 File Offset: 0x00043CA8
		public static int GetOutputCount<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetOutputCount();
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x00045AD0 File Offset: 0x00043CD0
		public static Playable GetInput<U>(this U playable, int inputPort) where U : struct, IPlayable
		{
			return playable.GetHandle().GetInput(inputPort);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x00045AF8 File Offset: 0x00043CF8
		public static Playable GetOutput<U>(this U playable, int outputPort) where U : struct, IPlayable
		{
			return playable.GetHandle().GetOutput(outputPort);
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x00045B20 File Offset: 0x00043D20
		public static void SetInputWeight<U>(this U playable, int inputIndex, float weight) where U : struct, IPlayable
		{
			playable.GetHandle().SetInputWeight(inputIndex, weight);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x00045B48 File Offset: 0x00043D48
		public static void SetInputWeight<U, V>(this U playable, V input, float weight) where U : struct, IPlayable where V : struct, IPlayable
		{
			playable.GetHandle().SetInputWeight(input.GetHandle(), weight);
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x00045B7C File Offset: 0x00043D7C
		public static float GetInputWeight<U>(this U playable, int inputIndex) where U : struct, IPlayable
		{
			return playable.GetHandle().GetInputWeight(inputIndex);
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x00045BA4 File Offset: 0x00043DA4
		public static void ConnectInput<U, V>(this U playable, int inputIndex, V sourcePlayable, int sourceOutputIndex) where U : struct, IPlayable where V : struct, IPlayable
		{
			playable.ConnectInput(inputIndex, sourcePlayable, sourceOutputIndex, 0f);
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x00045BB8 File Offset: 0x00043DB8
		public static void ConnectInput<U, V>(this U playable, int inputIndex, V sourcePlayable, int sourceOutputIndex, float weight) where U : struct, IPlayable where V : struct, IPlayable
		{
			playable.GetGraph<U>().Connect<V, U>(sourcePlayable, sourceOutputIndex, playable, inputIndex);
			playable.SetInputWeight(inputIndex, weight);
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x00045BE4 File Offset: 0x00043DE4
		public static void DisconnectInput<U>(this U playable, int inputPort) where U : struct, IPlayable
		{
			playable.GetGraph<U>().Disconnect<U>(playable, inputPort);
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x00045C04 File Offset: 0x00043E04
		public static int AddInput<U, V>(this U playable, V sourcePlayable, int sourceOutputIndex, float weight = 0f) where U : struct, IPlayable where V : struct, IPlayable
		{
			int inputCount = playable.GetInputCount<U>();
			playable.SetInputCount(inputCount + 1);
			playable.ConnectInput(inputCount, sourcePlayable, sourceOutputIndex, weight);
			return inputCount;
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x00045C34 File Offset: 0x00043E34
		[Obsolete("SetDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public static void SetDelay<U>(this U playable, double delay) where U : struct, IPlayable
		{
			playable.GetHandle().SetDelay(delay);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x00045C5C File Offset: 0x00043E5C
		[Obsolete("GetDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public static double GetDelay<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetDelay();
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x00045C84 File Offset: 0x00043E84
		[Obsolete("IsDelayed is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public static bool IsDelayed<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsDelayed();
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x00045CAC File Offset: 0x00043EAC
		public static void SetLeadTime<U>(this U playable, float value) where U : struct, IPlayable
		{
			playable.GetHandle().SetLeadTime(value);
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x00045CD4 File Offset: 0x00043ED4
		public static float GetLeadTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetLeadTime();
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x00045CFC File Offset: 0x00043EFC
		public static PlayableTraversalMode GetTraversalMode<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetTraversalMode();
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x00045D24 File Offset: 0x00043F24
		public static void SetTraversalMode<U>(this U playable, PlayableTraversalMode mode) where U : struct, IPlayable
		{
			playable.GetHandle().SetTraversalMode(mode);
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x00045D4C File Offset: 0x00043F4C
		internal static DirectorWrapMode GetTimeWrapMode<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetTimeWrapMode();
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x00045D74 File Offset: 0x00043F74
		internal static void SetTimeWrapMode<U>(this U playable, DirectorWrapMode value) where U : struct, IPlayable
		{
			playable.GetHandle().SetTimeWrapMode(value);
		}
	}
}
