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
	// Token: 0x02001A55 RID: 6741
	[Preserve]
	public abstract class LayeredContent : Content
	{
		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x0600CCA0 RID: 52384 RVA: 0x004AA361 File Offset: 0x004A8561
		// (set) Token: 0x0600CCA1 RID: 52385 RVA: 0x004AA369 File Offset: 0x004A8569
		public LayerType Layer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CCA2 RID: 52386 RVA: 0x004AA372 File Offset: 0x004A8572
		public LayeredContent()
		{
			this.clips = new EnumDictionary<PlacementType, IClipAdapter>();
		}

		// Token: 0x0600CCA3 RID: 52387
		public abstract float GetSample(PlacementType _placement, int _idx, params float[] _params);

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x0600CCA4 RID: 52388 RVA: 0x004AA388 File Offset: 0x004A8588
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

		// Token: 0x0600CCA5 RID: 52389 RVA: 0x004AA3E8 File Offset: 0x004A85E8
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

		// Token: 0x0600CCA6 RID: 52390 RVA: 0x004AA3F8 File Offset: 0x004A85F8
		public void LoadImmediate()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.LoadImmediate();
			}
		}

		// Token: 0x0600CCA7 RID: 52391 RVA: 0x004AA450 File Offset: 0x004A8650
		public override void Unload()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.Unload();
			}
		}

		// Token: 0x0600CCA8 RID: 52392 RVA: 0x004AA4A8 File Offset: 0x004A86A8
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.Parent.Parent.GetAttribute("name"), false);
			this.Layer = EnumUtils.Parse<LayerType>(_xmlNode.Parent.GetAttribute("name"), false);
		}

		// Token: 0x0600CCA9 RID: 52393 RVA: 0x004AA504 File Offset: 0x004A8704
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

		// Token: 0x0600CCAA RID: 52394 RVA: 0x004AA578 File Offset: 0x004A8778
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddClipAdapter(string _clipAdapterType, int _num, SectionType _section, LayerType _layer, PlacementType _placement)
		{
			IClipAdapter clipAdapter = LayeredContent.CreateClipAdapter(_clipAdapterType);
			clipAdapter.SetPaths(_num, _placement, _section, _layer, "");
			this.clips.Add(_placement, clipAdapter);
		}

		// Token: 0x0600CCAB RID: 52395 RVA: 0x004AA5AB File Offset: 0x004A87AB
		[PublicizedFrom(EAccessModifier.Protected)]
		public static IClipAdapter CreateClipAdapter(string _type)
		{
			return (IClipAdapter)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CCAC RID: 52396 RVA: 0x004AA5C4 File Offset: 0x004A87C4
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

		// Token: 0x0600CCAD RID: 52397 RVA: 0x004AA64C File Offset: 0x004A884C
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

		// Token: 0x0600CCAE RID: 52398 RVA: 0x004AA77C File Offset: 0x004A897C
		public static void ClearQueues()
		{
			foreach (ContentQueue contentQueue in LayeredContent.queueFor.Values)
			{
				contentQueue.Clear();
			}
			LayeredContent.queueFor.Clear();
		}

		// Token: 0x04009B6F RID: 39791
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Tuple<SectionType, LayerType>, ContentQueue> queueFor = new Dictionary<Tuple<SectionType, LayerType>, ContentQueue>();

		// Token: 0x04009B70 RID: 39792
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<PlacementType, IClipAdapter> clips;
	}
}
