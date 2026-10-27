using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A9F RID: 6815
	[Preserve]
	public class ContentQueue
	{
		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x0600CE4D RID: 52813 RVA: 0x00020119 File Offset: 0x0001E319
		[Preserve]
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CE4E RID: 52814 RVA: 0x004AFE10 File Offset: 0x004AE010
		public ContentQueue(SectionType _section, LayerType _layer)
		{
			this.section = _section;
			this.layer = _layer;
			this.count = (from e in Content.AllContent.OfType<LayeredContent>()
				where e.Section == this.section && e.Layer == this.layer
				select e).Count<LayeredContent>();
		}

		// Token: 0x0600CE4F RID: 52815 RVA: 0x004AFE64 File Offset: 0x004AE064
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

		// Token: 0x0600CE50 RID: 52816 RVA: 0x004AFEEB File Offset: 0x004AE0EB
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x04009CA2 RID: 40098
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009CA3 RID: 40099
		[PublicizedFrom(EAccessModifier.Private)]
		public SectionType section;

		// Token: 0x04009CA4 RID: 40100
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerType layer;

		// Token: 0x04009CA5 RID: 40101
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04009CA6 RID: 40102
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<LayeredContent> queue = new Queue<LayeredContent>();
	}
}
