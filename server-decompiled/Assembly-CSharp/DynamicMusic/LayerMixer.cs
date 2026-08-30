using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A52 RID: 6738
	[Preserve]
	public abstract class LayerMixer<ConfigType> : ILayerMixer where ConfigType : IConfiguration
	{
		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x0600CC3C RID: 52284 RVA: 0x004A92BA File Offset: 0x004A74BA
		// (set) Token: 0x0600CC3D RID: 52285 RVA: 0x004A92C2 File Offset: 0x004A74C2
		public SectionType Sect { get; set; }

		// Token: 0x0600CC3E RID: 52286 RVA: 0x004A92CB File Offset: 0x004A74CB
		public LayerMixer()
		{
			this.clipSetsFor = new EnumDictionary<LayerType, List<LayeredContent>>();
		}

		// Token: 0x170018FE RID: 6398
		public abstract float this[int _idx] { get; }

		// Token: 0x0600CC40 RID: 52288 RVA: 0x004A92DE File Offset: 0x004A74DE
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

		// Token: 0x0600CC41 RID: 52289 RVA: 0x004A92F0 File Offset: 0x004A74F0
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

		// Token: 0x04009B32 RID: 39730
		[PublicizedFrom(EAccessModifier.Protected)]
		public ConfigType config;

		// Token: 0x04009B33 RID: 39731
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<LayerType, List<LayeredContent>> clipSetsFor;
	}
}
