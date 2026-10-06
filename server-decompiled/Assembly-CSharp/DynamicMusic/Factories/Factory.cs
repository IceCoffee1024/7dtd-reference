using System;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic.Factories
{
	// Token: 0x02001AD2 RID: 6866
	public static class Factory
	{
		// Token: 0x0600CF24 RID: 53028 RVA: 0x004B136E File Offset: 0x004AF56E
		[Preserve]
		public static Conductor CreateConductor()
		{
			return new Conductor();
		}

		// Token: 0x0600CF25 RID: 53029 RVA: 0x004B1375 File Offset: 0x004AF575
		[Preserve]
		public static ISectionSelector CreateSectionSelector()
		{
			return new SectionSelector();
		}

		// Token: 0x0600CF26 RID: 53030 RVA: 0x004B137C File Offset: 0x004AF57C
		[Preserve]
		public static IThreatLevel CreateThreatLevel()
		{
			return default(ThreatLevel);
		}

		// Token: 0x0600CF27 RID: 53031 RVA: 0x004B1397 File Offset: 0x004AF597
		[Preserve]
		public static IFilter<SectionType> CreatePlayerTracker()
		{
			return new PlayerTracker();
		}

		// Token: 0x0600CF28 RID: 53032 RVA: 0x004B139E File Offset: 0x004AF59E
		[Preserve]
		public static DayTimeTracker CreateDayTimeTracker()
		{
			return new DayTimeTracker();
		}

		// Token: 0x0600CF29 RID: 53033 RVA: 0x004B13A5 File Offset: 0x004AF5A5
		[Preserve]
		public static IMultiNotifiableFilter CreateMusicTimeTracker()
		{
			return new MusicTimeTracker();
		}

		// Token: 0x0600CF2A RID: 53034 RVA: 0x004B13AC File Offset: 0x004AF5AC
		[Preserve]
		public static ISection CreateSection<T>(SectionType _sectionType) where T : ISection, new()
		{
			T t = new T();
			t.Sect = _sectionType;
			t.Init();
			return t;
		}

		// Token: 0x0600CF2B RID: 53035 RVA: 0x004B13E0 File Offset: 0x004AF5E0
		[Preserve]
		public static IConfiguration CreateConfiguration()
		{
			return new Configuration();
		}
	}
}
