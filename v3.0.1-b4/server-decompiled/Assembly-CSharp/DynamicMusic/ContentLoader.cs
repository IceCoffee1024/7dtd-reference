using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A20 RID: 6688
	public class ContentLoader
	{
		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x0600CA70 RID: 51824 RVA: 0x004A1CFA File Offset: 0x0049FEFA
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

		// Token: 0x0600CA71 RID: 51825 RVA: 0x004A1D12 File Offset: 0x0049FF12
		public void Start()
		{
			this.LoadQueue = new Queue<IEnumerator>();
			this.Loader = GameManager.Instance.StartCoroutine(this.Load());
		}

		// Token: 0x0600CA72 RID: 51826 RVA: 0x004A1D35 File Offset: 0x0049FF35
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

		// Token: 0x0600CA73 RID: 51827 RVA: 0x004A1D44 File Offset: 0x0049FF44
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

		// Token: 0x040099D0 RID: 39376
		public static ContentLoader instance;

		// Token: 0x040099D1 RID: 39377
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine Loader;

		// Token: 0x040099D2 RID: 39378
		public Queue<IEnumerator> LoadQueue;
	}
}
