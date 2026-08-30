using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A47 RID: 6727
	public class ContentLoader
	{
		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x0600CBF9 RID: 52217 RVA: 0x004A80EE File Offset: 0x004A62EE
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

		// Token: 0x0600CBFA RID: 52218 RVA: 0x004A8106 File Offset: 0x004A6306
		public void Start()
		{
			this.LoadQueue = new Queue<IEnumerator>();
			this.Loader = GameManager.Instance.StartCoroutine(this.Load());
		}

		// Token: 0x0600CBFB RID: 52219 RVA: 0x004A8129 File Offset: 0x004A6329
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

		// Token: 0x0600CBFC RID: 52220 RVA: 0x004A8138 File Offset: 0x004A6338
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

		// Token: 0x04009B01 RID: 39681
		public static ContentLoader instance;

		// Token: 0x04009B02 RID: 39682
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine Loader;

		// Token: 0x04009B03 RID: 39683
		public Queue<IEnumerator> LoadQueue;
	}
}
