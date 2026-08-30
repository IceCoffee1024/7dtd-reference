using System;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic.Factories
{
	// Token: 0x02001AA6 RID: 6822
	public static class Factory
	{
		// Token: 0x0600CDCC RID: 52684 RVA: 0x004ACEA2 File Offset: 0x004AB0A2
		[Preserve]
		public static Conductor CreateConductor()
		{
			return new Conductor();
		}

		// Token: 0x0600CDCD RID: 52685 RVA: 0x004ACEA9 File Offset: 0x004AB0A9
		[Preserve]
		public static ISectionSelector CreateSectionSelector()
		{
			return new SectionSelector();
		}

		// Token: 0x0600CDCE RID: 52686 RVA: 0x004ACEB0 File Offset: 0x004AB0B0
		[Preserve]
		public static IThreatLevel CreateThreatLevel()
		{
			return default(ThreatLevel);
		}

		// Token: 0x0600CDCF RID: 52687 RVA: 0x004ACECB File Offset: 0x004AB0CB
		[Preserve]
		public static IFilter<SectionType> CreatePlayerTracker()
		{
			return new PlayerTracker();
		}

		// Token: 0x0600CDD0 RID: 52688 RVA: 0x004ACED2 File Offset: 0x004AB0D2
		[Preserve]
		public static DayTimeTracker CreateDayTimeTracker()
		{
			return new DayTimeTracker();
		}

		// Token: 0x0600CDD1 RID: 52689 RVA: 0x004ACED9 File Offset: 0x004AB0D9
		[Preserve]
		public static IMultiNotifiableFilter CreateMusicTimeTracker()
		{
			return new MusicTimeTracker();
		}

		// Token: 0x0600CDD2 RID: 52690 RVA: 0x004ACEE0 File Offset: 0x004AB0E0
		[Preserve]
		public static ISection CreateSection<T>(SectionType _sectionType) where T : ISection, new()
		{
			T t = new T();
			t.Sect = _sectionType;
			t.Init();
			return t;
		}

		// Token: 0x0600CDD3 RID: 52691 RVA: 0x004ACF14 File Offset: 0x004AB114
		[Preserve]
		public static IConfiguration CreateConfiguration()
		{
			return new Configuration();
		}
	}
}
