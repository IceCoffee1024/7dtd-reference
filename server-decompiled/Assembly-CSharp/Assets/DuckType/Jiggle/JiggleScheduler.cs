using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D91 RID: 7569
	public static class JiggleScheduler
	{
		// Token: 0x0600DFD2 RID: 57298 RVA: 0x004FE79C File Offset: 0x004FC99C
		public static void Register(Jiggle jiggleBone)
		{
			JiggleScheduler.s_Records[jiggleBone] = JiggleScheduler.GetHierarchyDepth(jiggleBone.transform);
			JiggleScheduler.isDirty = true;
		}

		// Token: 0x0600DFD3 RID: 57299 RVA: 0x004FE7BA File Offset: 0x004FC9BA
		public static void Deregister(Jiggle jiggleBone)
		{
			JiggleScheduler.s_Records.Remove(jiggleBone);
			JiggleScheduler.isDirty = true;
		}

		// Token: 0x0600DFD4 RID: 57300 RVA: 0x004FE7D0 File Offset: 0x004FC9D0
		public static void Update(Jiggle jiggle)
		{
			if (JiggleScheduler.isDirty)
			{
				JiggleScheduler.isDirty = false;
				JiggleScheduler.UpdateOrderedRecords();
			}
			if (jiggle == JiggleScheduler.m_UpdateTriggerJiggle)
			{
				foreach (Jiggle jiggle2 in JiggleScheduler.s_OrderedRecords)
				{
					if (jiggle2.enabled && !jiggle2.UpdateWithPhysics)
					{
						jiggle2.ScheduledUpdate(Time.deltaTime);
					}
				}
			}
		}

		// Token: 0x0600DFD5 RID: 57301 RVA: 0x004FE858 File Offset: 0x004FCA58
		public static void FixedUpdate(Jiggle jiggle)
		{
			if (jiggle == JiggleScheduler.m_UpdateTriggerJiggle)
			{
				foreach (Jiggle jiggle2 in JiggleScheduler.s_OrderedRecords)
				{
					if (jiggle2.enabled && jiggle2.UpdateWithPhysics)
					{
						jiggle2.ScheduledUpdate(Time.fixedDeltaTime);
					}
				}
			}
		}

		// Token: 0x0600DFD6 RID: 57302 RVA: 0x004FE8CC File Offset: 0x004FCACC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void UpdateOrderedRecords()
		{
			JiggleScheduler.s_OrderedRecords = (from x in JiggleScheduler.s_Records
				orderby x.Value
				select x.Key).ToList<Jiggle>();
			JiggleScheduler.m_UpdateTriggerJiggle = JiggleScheduler.s_OrderedRecords.FirstOrDefault<Jiggle>();
		}

		// Token: 0x0600DFD7 RID: 57303 RVA: 0x004FE93F File Offset: 0x004FCB3F
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetHierarchyDepth(Transform t)
		{
			if (!(t == null))
			{
				return JiggleScheduler.GetHierarchyDepth(t.parent) + 1;
			}
			return -1;
		}

		// Token: 0x0400A98D RID: 43405
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Jiggle, int> s_Records = new Dictionary<Jiggle, int>();

		// Token: 0x0400A98E RID: 43406
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Jiggle> s_OrderedRecords = new List<Jiggle>();

		// Token: 0x0400A98F RID: 43407
		[PublicizedFrom(EAccessModifier.Private)]
		public static Jiggle m_UpdateTriggerJiggle = null;

		// Token: 0x0400A990 RID: 43408
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isDirty;
	}
}
