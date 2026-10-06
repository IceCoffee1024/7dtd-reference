using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A6E RID: 6766
	[Preserve]
	public class ContentQueue
	{
		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x0600CD39 RID: 52537 RVA: 0x00020119 File Offset: 0x0001E319
		[Preserve]
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CD3A RID: 52538 RVA: 0x004AC314 File Offset: 0x004AA514
		public ContentQueue(SectionType _section, LayerType _layer)
		{
			this.section = _section;
			this.layer = _layer;
			this.count = (from e in Content.AllContent.OfType<LayeredContent>()
				where e.Section == this.section && e.Layer == this.layer
				select e).Count<LayeredContent>();
		}

		// Token: 0x0600CD3B RID: 52539 RVA: 0x004AC368 File Offset: 0x004AA568
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

		// Token: 0x0600CD3C RID: 52540 RVA: 0x004AC3EF File Offset: 0x004AA5EF
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x04009BB9 RID: 39865
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009BBA RID: 39866
		[PublicizedFrom(EAccessModifier.Private)]
		public SectionType section;

		// Token: 0x04009BBB RID: 39867
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerType layer;

		// Token: 0x04009BBC RID: 39868
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04009BBD RID: 39869
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<LayeredContent> queue = new Queue<LayeredContent>();
	}
}
