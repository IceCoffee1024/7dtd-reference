using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A7E RID: 6782
	[Preserve]
	public abstract class LayerMixer<ConfigType> : ILayerMixer where ConfigType : IConfiguration
	{
		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x0600CD94 RID: 52628 RVA: 0x004AD786 File Offset: 0x004AB986
		// (set) Token: 0x0600CD95 RID: 52629 RVA: 0x004AD78E File Offset: 0x004AB98E
		public SectionType Sect { get; set; }

		// Token: 0x0600CD96 RID: 52630 RVA: 0x004AD797 File Offset: 0x004AB997
		public LayerMixer()
		{
			this.clipSetsFor = new EnumDictionary<LayerType, List<LayeredContent>>();
		}

		// Token: 0x170019A0 RID: 6560
		public abstract float this[int _idx] { get; }

		// Token: 0x0600CD98 RID: 52632 RVA: 0x004AD7AA File Offset: 0x004AB9AA
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

		// Token: 0x0600CD99 RID: 52633 RVA: 0x004AD7BC File Offset: 0x004AB9BC
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

		// Token: 0x04009BF8 RID: 39928
		[PublicizedFrom(EAccessModifier.Protected)]
		public ConfigType config;

		// Token: 0x04009BF9 RID: 39929
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<LayerType, List<LayeredContent>> clipSetsFor;
	}
}
