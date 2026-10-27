using System;
using System.Collections;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AE9 RID: 6889
	public static class TrackerTests
	{
		// Token: 0x0600CF82 RID: 53122 RVA: 0x004B2D0C File Offset: 0x004B0F0C
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

		// Token: 0x0600CF83 RID: 53123 RVA: 0x004B2D9D File Offset: 0x004B0F9D
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

		// Token: 0x0600CF84 RID: 53124 RVA: 0x004B2DA5 File Offset: 0x004B0FA5
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

		// Token: 0x0600CF85 RID: 53125 RVA: 0x004B2DAD File Offset: 0x004B0FAD
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

		// Token: 0x0600CF86 RID: 53126 RVA: 0x004B2DB5 File Offset: 0x004B0FB5
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

		// Token: 0x0600CF87 RID: 53127 RVA: 0x004B2DBD File Offset: 0x004B0FBD
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

		// Token: 0x0600CF88 RID: 53128 RVA: 0x004B2DC5 File Offset: 0x004B0FC5
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

		// Token: 0x04009D2C RID: 40236
		public static bool continueTest = false;

		// Token: 0x04009D2D RID: 40237
		public static bool isFinished = false;

		// Token: 0x04009D2E RID: 40238
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<SectionType> sections = new List<SectionType>
		{
			SectionType.Exploration,
			SectionType.Suspense,
			SectionType.Combat
		};
	}
}
