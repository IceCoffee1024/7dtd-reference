using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A86 RID: 6790
	[Preserve]
	public abstract class LayeredContent : Content
	{
		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x0600CDB4 RID: 52660 RVA: 0x004ADE5D File Offset: 0x004AC05D
		// (set) Token: 0x0600CDB5 RID: 52661 RVA: 0x004ADE65 File Offset: 0x004AC065
		public LayerType Layer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CDB6 RID: 52662 RVA: 0x004ADE6E File Offset: 0x004AC06E
		public LayeredContent()
		{
			this.clips = new EnumDictionary<PlacementType, IClipAdapter>();
		}

		// Token: 0x0600CDB7 RID: 52663
		public abstract float GetSample(PlacementType _placement, int _idx, params float[] _params);

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600CDB8 RID: 52664 RVA: 0x004ADE84 File Offset: 0x004AC084
		public override bool IsLoaded
		{
			get
			{
				using (Dictionary<PlacementType, IClipAdapter>.ValueCollection.Enumerator enumerator = this.clips.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsLoaded)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600CDB9 RID: 52665 RVA: 0x004ADEE4 File Offset: 0x004AC0E4
		public override IEnumerator Load()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				yield return clipAdapter.Load();
			}
			Dictionary<PlacementType, IClipAdapter>.ValueCollection.Enumerator enumerator = default(Dictionary<PlacementType, IClipAdapter>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CDBA RID: 52666 RVA: 0x004ADEF4 File Offset: 0x004AC0F4
		public void LoadImmediate()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.LoadImmediate();
			}
		}

		// Token: 0x0600CDBB RID: 52667 RVA: 0x004ADF4C File Offset: 0x004AC14C
		public override void Unload()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.Unload();
			}
		}

		// Token: 0x0600CDBC RID: 52668 RVA: 0x004ADFA4 File Offset: 0x004AC1A4
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.Parent.Parent.GetAttribute("name"), false);
			this.Layer = EnumUtils.Parse<LayerType>(_xmlNode.Parent.GetAttribute("name"), false);
		}

		// Token: 0x0600CDBD RID: 52669 RVA: 0x004AE000 File Offset: 0x004AC200
		public void SetData(string _clipAdapterType, int _num, SectionType _section, LayerType _layer, bool loopOnly = false)
		{
			base.Name = _num.ToString("000") + DMSConstants.SectionAbbrvs[_section] + DMSConstants.LayerAbbrvs[_layer];
			base.Section = _section;
			this.Layer = _layer;
			this.AddClipAdapter(_clipAdapterType, _num, _section, _layer, PlacementType.Loop);
			if (!loopOnly)
			{
				this.AddClipAdapter(_clipAdapterType, _num, _section, _layer, PlacementType.Begin);
				this.AddClipAdapter(_clipAdapterType, _num, _section, _layer, PlacementType.End);
			}
		}

		// Token: 0x0600CDBE RID: 52670 RVA: 0x004AE074 File Offset: 0x004AC274
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddClipAdapter(string _clipAdapterType, int _num, SectionType _section, LayerType _layer, PlacementType _placement)
		{
			IClipAdapter clipAdapter = LayeredContent.CreateClipAdapter(_clipAdapterType);
			clipAdapter.SetPaths(_num, _placement, _section, _layer, "");
			this.clips.Add(_placement, clipAdapter);
		}

		// Token: 0x0600CDBF RID: 52671 RVA: 0x004AE0A7 File Offset: 0x004AC2A7
		[PublicizedFrom(EAccessModifier.Protected)]
		public static IClipAdapter CreateClipAdapter(string _type)
		{
			return (IClipAdapter)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CDC0 RID: 52672 RVA: 0x004AE0C0 File Offset: 0x004AC2C0
		public static T Get<T>(SectionType _section, LayerType _layer) where T : LayeredContent
		{
			(from e in Content.AllContent.OfType<T>()
				where e.Section == _section && e.Layer == _layer
				select e).ToList<T>();
			Tuple<SectionType, LayerType> tuple = new Tuple<SectionType, LayerType>(_section, _layer);
			ContentQueue contentQueue;
			if (LayeredContent.queueFor.TryGetValue(tuple, out contentQueue))
			{
				return (T)((object)contentQueue.Next());
			}
			Log.Warning(string.Format("there is no Content for {0}", tuple));
			return default(T);
		}

		// Token: 0x0600CDC1 RID: 52673 RVA: 0x004AE148 File Offset: 0x004AC348
		public static void ReadyQueuesImmediate()
		{
			LayeredContent.queueFor.Clear();
			using (List<SectionType>.Enumerator enumerator = DMSConstants.LayeredSections.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SectionType section = enumerator.Current;
					using (IEnumerator enumerator2 = Enum.GetValues(typeof(LayerType)).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							LayerType layer = (LayerType)enumerator2.Current;
							if (((ICollection<LayeredContent>)(from c in Content.AllContent.OfType<LayeredContent>()
								where c.Section == section && c.Layer == layer
								select c).ToList<LayeredContent>()).Count > 0)
							{
								ContentQueue contentQueue = new ContentQueue(section, layer);
								LayeredContent.queueFor.Add(new Tuple<SectionType, LayerType>(section, layer), contentQueue);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600CDC2 RID: 52674 RVA: 0x004AE278 File Offset: 0x004AC478
		public static void ClearQueues()
		{
			foreach (ContentQueue contentQueue in LayeredContent.queueFor.Values)
			{
				contentQueue.Clear();
			}
			LayeredContent.queueFor.Clear();
		}

		// Token: 0x04009C58 RID: 40024
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Tuple<SectionType, LayerType>, ContentQueue> queueFor = new Dictionary<Tuple<SectionType, LayerType>, ContentQueue>();

		// Token: 0x04009C59 RID: 40025
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<PlacementType, IClipAdapter> clips;
	}
}
