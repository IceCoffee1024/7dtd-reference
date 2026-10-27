using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AA4 RID: 6820
	public class ContentLoader
	{
		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x0600CE65 RID: 52837 RVA: 0x004B00B6 File Offset: 0x004AE2B6
		public static ContentLoader Instance
		{
			get
			{
				if (ContentLoader.instance == null)
				{
					ContentLoader.instance = new ContentLoader();
				}
				return ContentLoader.instance;
			}
		}

		// Token: 0x0600CE66 RID: 52838 RVA: 0x004B00CE File Offset: 0x004AE2CE
		public void Start()
		{
			this.LoadQueue = new Queue<IEnumerator>();
			this.Loader = GameManager.Instance.StartCoroutine(this.Load());
		}

		// Token: 0x0600CE67 RID: 52839 RVA: 0x004B00F1 File Offset: 0x004AE2F1
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator Load()
		{
			for (;;)
			{
				yield return new WaitUntil(() => this.LoadQueue.Count > 0);
				IEnumerator enumerator = this.LoadQueue.Dequeue();
				yield return enumerator;
			}
			yield break;
		}

		// Token: 0x0600CE68 RID: 52840 RVA: 0x004B0100 File Offset: 0x004AE300
		public void Cleanup()
		{
			if (this.Loader != null)
			{
				GameManager.Instance.StopCoroutine(this.Loader);
			}
			if (this.LoadQueue != null)
			{
				this.LoadQueue.Clear();
			}
			this.Loader = null;
			this.LoadQueue = null;
		}

		// Token: 0x04009CB0 RID: 40112
		public static ContentLoader instance;

		// Token: 0x04009CB1 RID: 40113
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine Loader;

		// Token: 0x04009CB2 RID: 40114
		public Queue<IEnumerator> LoadQueue;
	}
}
