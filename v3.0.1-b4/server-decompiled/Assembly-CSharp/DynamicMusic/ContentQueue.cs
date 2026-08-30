using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A1B RID: 6683
	[Preserve]
	public class ContentQueue
	{
		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x0600CA58 RID: 51800 RVA: 0x0001FF5D File Offset: 0x0001E15D
		[Preserve]
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CA59 RID: 51801 RVA: 0x004A1A54 File Offset: 0x0049FC54
		public ContentQueue(SectionType _section, LayerType _layer)
		{
			this.section = _section;
			this.layer = _layer;
			this.count = (from e in Content.AllContent.OfType<LayeredContent>()
				where e.Section == this.section && e.Layer == this.layer
				select e).Count<LayeredContent>();
		}

		// Token: 0x0600CA5A RID: 51802 RVA: 0x004A1AA8 File Offset: 0x0049FCA8
		public LayeredContent Next()
		{
			if (this.queue.Count < this.count / 2)
			{
				(from e in Content.AllContent.OfType<LayeredContent>()
					where e.Section == this.section && e.Layer == this.layer && !this.queue.Contains(e)
					orderby ContentQueue.rng.RandomInt
					select e).ToList<LayeredContent>().ForEach(new Action<LayeredContent>(this.queue.Enqueue));
			}
			return this.queue.Dequeue();
		}

		// Token: 0x0600CA5B RID: 51803 RVA: 0x004A1B2F File Offset: 0x0049FD2F
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x040099C2 RID: 39362
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x040099C3 RID: 39363
		[PublicizedFrom(EAccessModifier.Private)]
		public SectionType section;

		// Token: 0x040099C4 RID: 39364
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerType layer;

		// Token: 0x040099C5 RID: 39365
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x040099C6 RID: 39366
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<LayeredContent> queue = new Queue<LayeredContent>();
	}
}
