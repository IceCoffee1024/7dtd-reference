using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A73 RID: 6771
	public class ContentLoader
	{
		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x0600CD51 RID: 52561 RVA: 0x004AC5BA File Offset: 0x004AA7BA
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

		// Token: 0x0600CD52 RID: 52562 RVA: 0x004AC5D2 File Offset: 0x004AA7D2
		public void Start()
		{
			this.LoadQueue = new Queue<IEnumerator>();
			this.Loader = GameManager.Instance.StartCoroutine(this.Load());
		}

		// Token: 0x0600CD53 RID: 52563 RVA: 0x004AC5F5 File Offset: 0x004AA7F5
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

		// Token: 0x0600CD54 RID: 52564 RVA: 0x004AC604 File Offset: 0x004AA804
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

		// Token: 0x04009BC7 RID: 39879
		public static ContentLoader instance;

		// Token: 0x04009BC8 RID: 39880
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine Loader;

		// Token: 0x04009BC9 RID: 39881
		public Queue<IEnumerator> LoadQueue;
	}
}
