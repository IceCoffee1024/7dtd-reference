using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A42 RID: 6722
	[Preserve]
	public class ContentQueue
	{
		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x0600CBE1 RID: 52193 RVA: 0x00020021 File Offset: 0x0001E221
		[Preserve]
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CBE2 RID: 52194 RVA: 0x004A7E48 File Offset: 0x004A6048
		public ContentQueue(SectionType _section, LayerType _layer)
		{
			this.section = _section;
			this.layer = _layer;
			this.count = (from e in Content.AllContent.OfType<LayeredContent>()
				where e.Section == this.section && e.Layer == this.layer
				select e).Count<LayeredContent>();
		}

		// Token: 0x0600CBE3 RID: 52195 RVA: 0x004A7E9C File Offset: 0x004A609C
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

		// Token: 0x0600CBE4 RID: 52196 RVA: 0x004A7F23 File Offset: 0x004A6123
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x04009AF3 RID: 39667
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009AF4 RID: 39668
		[PublicizedFrom(EAccessModifier.Private)]
		public SectionType section;

		// Token: 0x04009AF5 RID: 39669
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerType layer;

		// Token: 0x04009AF6 RID: 39670
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04009AF7 RID: 39671
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<LayeredContent> queue = new Queue<LayeredContent>();
	}
}
