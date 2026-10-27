using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001AAF RID: 6831
	[Preserve]
	public abstract class LayerMixer<ConfigType> : ILayerMixer where ConfigType : IConfiguration
	{
		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x0600CEA8 RID: 52904 RVA: 0x004B1282 File Offset: 0x004AF482
		// (set) Token: 0x0600CEA9 RID: 52905 RVA: 0x004B128A File Offset: 0x004AF48A
		public SectionType Sect { get; set; }

		// Token: 0x0600CEAA RID: 52906 RVA: 0x004B1293 File Offset: 0x004AF493
		public LayerMixer()
		{
			this.clipSetsFor = new EnumDictionary<LayerType, List<LayeredContent>>();
		}

		// Token: 0x170019E8 RID: 6632
		public abstract float this[int _idx] { get; }

		// Token: 0x0600CEAC RID: 52908 RVA: 0x004B12A6 File Offset: 0x004AF4A6
		public virtual IEnumerator Load()
		{
			Log.Out(string.Format("Loading new config for {0}...", this.Sect));
			this.config = AbstractConfiguration.Get<ConfigType>(this.Sect);
			if (this.config == null)
			{
				Log.Warning(string.Format("{0} pulled a null config", this.Sect));
			}
			this.clipSetsFor.Clear();
			yield return null;
			yield break;
		}

		// Token: 0x0600CEAD RID: 52909 RVA: 0x004B12B8 File Offset: 0x004AF4B8
		public void Unload()
		{
			this.clipSetsFor.Values.ToList<List<LayeredContent>>().ForEach(delegate(List<LayeredContent> list)
			{
				list.ToList<LayeredContent>().ForEach(delegate(LayeredContent e)
				{
					e.Unload();
				});
			});
			this.clipSetsFor.Clear();
			Log.Out(string.Format("unloaded ClipSets on {0}", this.Sect));
		}

		// Token: 0x04009CE1 RID: 40161
		[PublicizedFrom(EAccessModifier.Protected)]
		public ConfigType config;

		// Token: 0x04009CE2 RID: 40162
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<LayerType, List<LayeredContent>> clipSetsFor;
	}
}
