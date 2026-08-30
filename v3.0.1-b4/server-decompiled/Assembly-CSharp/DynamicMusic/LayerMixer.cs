using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A2B RID: 6699
	[Preserve]
	public abstract class LayerMixer<ConfigType> : ILayerMixer where ConfigType : IConfiguration
	{
		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x0600CAB3 RID: 51891 RVA: 0x004A2EC6 File Offset: 0x004A10C6
		// (set) Token: 0x0600CAB4 RID: 51892 RVA: 0x004A2ECE File Offset: 0x004A10CE
		public SectionType Sect { get; set; }

		// Token: 0x0600CAB5 RID: 51893 RVA: 0x004A2ED7 File Offset: 0x004A10D7
		public LayerMixer()
		{
			this.clipSetsFor = new EnumDictionary<LayerType, List<LayeredContent>>();
		}

		// Token: 0x1700188C RID: 6284
		public abstract float this[int _idx] { get; }

		// Token: 0x0600CAB7 RID: 51895 RVA: 0x004A2EEA File Offset: 0x004A10EA
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

		// Token: 0x0600CAB8 RID: 51896 RVA: 0x004A2EFC File Offset: 0x004A10FC
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

		// Token: 0x04009A01 RID: 39425
		[PublicizedFrom(EAccessModifier.Protected)]
		public ConfigType config;

		// Token: 0x04009A02 RID: 39426
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<LayerType, List<LayeredContent>> clipSetsFor;
	}
}
