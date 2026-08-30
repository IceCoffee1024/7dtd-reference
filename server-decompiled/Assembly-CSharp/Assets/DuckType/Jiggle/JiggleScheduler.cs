using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D5F RID: 7519
	public static class JiggleScheduler
	{
		// Token: 0x0600DE59 RID: 56921 RVA: 0x004F9BE0 File Offset: 0x004F7DE0
		public static void Register(Jiggle jiggleBone)
		{
			JiggleScheduler.s_Records[jiggleBone] = JiggleScheduler.GetHierarchyDepth(jiggleBone.transform);
			JiggleScheduler.isDirty = true;
		}

		// Token: 0x0600DE5A RID: 56922 RVA: 0x004F9BFE File Offset: 0x004F7DFE
		public static void Deregister(Jiggle jiggleBone)
		{
			JiggleScheduler.s_Records.Remove(jiggleBone);
			JiggleScheduler.isDirty = true;
		}

		// Token: 0x0600DE5B RID: 56923 RVA: 0x004F9C14 File Offset: 0x004F7E14
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

		// Token: 0x0600DE5C RID: 56924 RVA: 0x004F9C9C File Offset: 0x004F7E9C
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

		// Token: 0x0600DE5D RID: 56925 RVA: 0x004F9D10 File Offset: 0x004F7F10
		[PublicizedFrom(EAccessModifier.Private)]
		public static void UpdateOrderedRecords()
		{
			JiggleScheduler.s_OrderedRecords = (from x in JiggleScheduler.s_Records
				orderby x.Value
				select x.Key).ToList<Jiggle>();
			JiggleScheduler.m_UpdateTriggerJiggle = JiggleScheduler.s_OrderedRecords.FirstOrDefault<Jiggle>();
		}

		// Token: 0x0600DE5E RID: 56926 RVA: 0x004F9D83 File Offset: 0x004F7F83
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetHierarchyDepth(Transform t)
		{
			if (!(t == null))
			{
				return JiggleScheduler.GetHierarchyDepth(t.parent) + 1;
			}
			return -1;
		}

		// Token: 0x0400A8B6 RID: 43190
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Jiggle, int> s_Records = new Dictionary<Jiggle, int>();

		// Token: 0x0400A8B7 RID: 43191
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Jiggle> s_OrderedRecords = new List<Jiggle>();

		// Token: 0x0400A8B8 RID: 43192
		[PublicizedFrom(EAccessModifier.Private)]
		public static Jiggle m_UpdateTriggerJiggle = null;

		// Token: 0x0400A8B9 RID: 43193
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isDirty;
	}
}
