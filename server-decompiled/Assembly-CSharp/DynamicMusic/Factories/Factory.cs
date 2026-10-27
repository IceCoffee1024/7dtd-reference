using System;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic.Factories
{
	// Token: 0x02001B03 RID: 6915
	public static class Factory
	{
		// Token: 0x0600D038 RID: 53304 RVA: 0x004B4E6A File Offset: 0x004B306A
		[Preserve]
		public static Conductor CreateConductor()
		{
			return new Conductor();
		}

		// Token: 0x0600D039 RID: 53305 RVA: 0x004B4E71 File Offset: 0x004B3071
		[Preserve]
		public static ISectionSelector CreateSectionSelector()
		{
			return new SectionSelector();
		}

		// Token: 0x0600D03A RID: 53306 RVA: 0x004B4E78 File Offset: 0x004B3078
		[Preserve]
		public static IThreatLevel CreateThreatLevel()
		{
			return default(ThreatLevel);
		}

		// Token: 0x0600D03B RID: 53307 RVA: 0x004B4E93 File Offset: 0x004B3093
		[Preserve]
		public static IFilter<SectionType> CreatePlayerTracker()
		{
			return new PlayerTracker();
		}

		// Token: 0x0600D03C RID: 53308 RVA: 0x004B4E9A File Offset: 0x004B309A
		[Preserve]
		public static DayTimeTracker CreateDayTimeTracker()
		{
			return new DayTimeTracker();
		}

		// Token: 0x0600D03D RID: 53309 RVA: 0x004B4EA1 File Offset: 0x004B30A1
		[Preserve]
		public static IMultiNotifiableFilter CreateMusicTimeTracker()
		{
			return new MusicTimeTracker();
		}

		// Token: 0x0600D03E RID: 53310 RVA: 0x004B4EA8 File Offset: 0x004B30A8
		[Preserve]
		public static ISection CreateSection<T>(SectionType _sectionType) where T : ISection, new()
		{
			T t = new T();
			t.Sect = _sectionType;
			t.Init();
			return t;
		}

		// Token: 0x0600D03F RID: 53311 RVA: 0x004B4EDC File Offset: 0x004B30DC
		[Preserve]
		public static IConfiguration CreateConfiguration()
		{
			return new Configuration();
		}
	}
}
