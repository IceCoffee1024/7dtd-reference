using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001DC2 RID: 7618
	public static class JiggleScheduler
	{
		// Token: 0x0600E0E6 RID: 57574 RVA: 0x00502334 File Offset: 0x00500534
		public static void Register(Jiggle jiggleBone)
		{
			JiggleScheduler.s_Records[jiggleBone] = JiggleScheduler.GetHierarchyDepth(jiggleBone.transform);
			JiggleScheduler.isDirty = true;
		}

		// Token: 0x0600E0E7 RID: 57575 RVA: 0x00502352 File Offset: 0x00500552
		public static void Deregister(Jiggle jiggleBone)
		{
			JiggleScheduler.s_Records.Remove(jiggleBone);
			JiggleScheduler.isDirty = true;
		}

		// Token: 0x0600E0E8 RID: 57576 RVA: 0x00502368 File Offset: 0x00500568
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

		// Token: 0x0600E0E9 RID: 57577 RVA: 0x005023F0 File Offset: 0x005005F0
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

		// Token: 0x0600E0EA RID: 57578 RVA: 0x00502464 File Offset: 0x00500664
		[PublicizedFrom(EAccessModifier.Private)]
		public static void UpdateOrderedRecords()
		{
			JiggleScheduler.s_OrderedRecords = (from x in JiggleScheduler.s_Records
				orderby x.Value
				select x.Key).ToList<Jiggle>();
			JiggleScheduler.m_UpdateTriggerJiggle = JiggleScheduler.s_OrderedRecords.FirstOrDefault<Jiggle>();
		}

		// Token: 0x0600E0EB RID: 57579 RVA: 0x005024D7 File Offset: 0x005006D7
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetHierarchyDepth(Transform t)
		{
			if (!(t == null))
			{
				return JiggleScheduler.GetHierarchyDepth(t.parent) + 1;
			}
			return -1;
		}

		// Token: 0x0400AA76 RID: 43638
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Jiggle, int> s_Records = new Dictionary<Jiggle, int>();

		// Token: 0x0400AA77 RID: 43639
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Jiggle> s_OrderedRecords = new List<Jiggle>();

		// Token: 0x0400AA78 RID: 43640
		[PublicizedFrom(EAccessModifier.Private)]
		public static Jiggle m_UpdateTriggerJiggle = null;

		// Token: 0x0400AA79 RID: 43641
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isDirty;
	}
}
