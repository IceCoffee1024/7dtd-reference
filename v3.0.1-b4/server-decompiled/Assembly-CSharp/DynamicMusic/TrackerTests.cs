using System;
using System.Collections;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A65 RID: 6757
	public static class TrackerTests
	{
		// Token: 0x0600CB8D RID: 52109 RVA: 0x004A4950 File Offset: 0x004A2B50
		public static void Run(int num)
		{
			switch (num)
			{
			case 0:
				GameManager.Instance.StartCoroutine(TrackerTests.MusicTimeTrackerTest());
				return;
			case 1:
				GameManager.Instance.StartCoroutine(TrackerTests.DayTimeTrackerTest());
				return;
			case 2:
				GameManager.Instance.StartCoroutine(TrackerTests.PlayerTrackerTest());
				return;
			case 3:
				GameManager.Instance.StartCoroutine(TrackerTests.SelectorTest());
				return;
			case 4:
				GameManager.Instance.StartCoroutine(TrackerTests.ConductorTest());
				return;
			case 5:
				GameManager.Instance.StartCoroutine(TrackerTests.RealTimeConductorTest());
				return;
			default:
				return;
			}
		}

		// Token: 0x0600CB8E RID: 52110 RVA: 0x004A49E1 File Offset: 0x004A2BE1
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator MusicTimeTrackerTest()
		{
			TrackerTests.isFinished = false;
			IMultiNotifiableFilter musicTimeTracker = Factory.CreateMusicTimeTracker();
			int num;
			for (int i = 0; i < 2; i = num + 1)
			{
				musicTimeTracker.Notify(MusicActionType.Play);
				yield return new WaitForSeconds(30f);
				musicTimeTracker.Notify(MusicActionType.Pause);
				yield return new WaitForSeconds(30f);
				musicTimeTracker.Notify(MusicActionType.UnPause);
				yield return new WaitForSeconds(30f);
				musicTimeTracker.Notify(MusicActionType.Stop);
				yield return new WaitForSeconds(30f);
				num = i;
			}
			musicTimeTracker.Notify();
			yield break;
		}

		// Token: 0x0600CB8F RID: 52111 RVA: 0x004A49E9 File Offset: 0x004A2BE9
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator DayTimeTrackerTest()
		{
			TrackerTests.isFinished = false;
			List<SectionType> sectionTypes = new List<SectionType> { SectionType.Exploration };
			DayTimeTracker dtt = Factory.CreateDayTimeTracker();
			do
			{
				yield return new WaitUntil(() => TrackerTests.continueTest);
				TrackerTests.continueTest = false;
				dtt.Filter(sectionTypes);
			}
			while (!TrackerTests.isFinished);
			yield break;
		}

		// Token: 0x0600CB90 RID: 52112 RVA: 0x004A49F1 File Offset: 0x004A2BF1
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator PlayerTrackerTest()
		{
			TrackerTests.isFinished = false;
			IFilter<SectionType> playerTracker = Factory.CreatePlayerTracker();
			while (!TrackerTests.isFinished)
			{
				List<SectionType> list = new List<SectionType>(TrackerTests.sections);
				playerTracker.Filter(list);
				yield return new WaitUntil(() => TrackerTests.continueTest);
				TrackerTests.continueTest = false;
			}
			yield break;
		}

		// Token: 0x0600CB91 RID: 52113 RVA: 0x004A49F9 File Offset: 0x004A2BF9
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator SelectorTest()
		{
			TrackerTests.isFinished = false;
			ISectionSelector sectionSelector = Factory.CreateSectionSelector();
			while (!TrackerTests.isFinished)
			{
				SectionType sectionType = sectionSelector.Select();
				Log.Out(string.Format("Selected Section: {0}", sectionType));
				yield return new WaitUntil(() => TrackerTests.continueTest);
				TrackerTests.continueTest = false;
			}
			yield break;
		}

		// Token: 0x0600CB92 RID: 52114 RVA: 0x004A4A01 File Offset: 0x004A2C01
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator ConductorTest()
		{
			TrackerTests.isFinished = false;
			Conductor c = Factory.CreateConductor();
			while (!TrackerTests.isFinished)
			{
				yield return new WaitUntil(() => TrackerTests.continueTest);
				c.Update();
				TrackerTests.continueTest = false;
			}
			yield break;
		}

		// Token: 0x0600CB93 RID: 52115 RVA: 0x004A4A09 File Offset: 0x004A2C09
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator RealTimeConductorTest()
		{
			TrackerTests.isFinished = false;
			Conductor c = Factory.CreateConductor();
			while (!TrackerTests.isFinished)
			{
				c.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x04009A4C RID: 39500
		public static bool continueTest = false;

		// Token: 0x04009A4D RID: 39501
		public static bool isFinished = false;

		// Token: 0x04009A4E RID: 39502
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<SectionType> sections = new List<SectionType>
		{
			SectionType.Exploration,
			SectionType.Suspense,
			SectionType.Combat
		};
	}
}
