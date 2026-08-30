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
	// Token: 0x02001A29 RID: 6697
	[Preserve]
	public abstract class LayeredContent : Content
	{
		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x0600CB48 RID: 52040 RVA: 0x004A5E95 File Offset: 0x004A4095
		// (set) Token: 0x0600CB49 RID: 52041 RVA: 0x004A5E9D File Offset: 0x004A409D
		public LayerType Layer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CB4A RID: 52042 RVA: 0x004A5EA6 File Offset: 0x004A40A6
		public LayeredContent()
		{
			this.clips = new EnumDictionary<PlacementType, IClipAdapter>();
		}

		// Token: 0x0600CB4B RID: 52043
		public abstract float GetSample(PlacementType _placement, int _idx, params float[] _params);

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x0600CB4C RID: 52044 RVA: 0x004A5EBC File Offset: 0x004A40BC
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

		// Token: 0x0600CB4D RID: 52045 RVA: 0x004A5F1C File Offset: 0x004A411C
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

		// Token: 0x0600CB4E RID: 52046 RVA: 0x004A5F2C File Offset: 0x004A412C
		public void LoadImmediate()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.LoadImmediate();
			}
		}

		// Token: 0x0600CB4F RID: 52047 RVA: 0x004A5F84 File Offset: 0x004A4184
		public override void Unload()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.Unload();
			}
		}

		// Token: 0x0600CB50 RID: 52048 RVA: 0x004A5FDC File Offset: 0x004A41DC
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.Parent.Parent.GetAttribute("name"), false);
			this.Layer = EnumUtils.Parse<LayerType>(_xmlNode.Parent.GetAttribute("name"), false);
		}

		// Token: 0x0600CB51 RID: 52049 RVA: 0x004A6038 File Offset: 0x004A4238
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

		// Token: 0x0600CB52 RID: 52050 RVA: 0x004A60AC File Offset: 0x004A42AC
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddClipAdapter(string _clipAdapterType, int _num, SectionType _section, LayerType _layer, PlacementType _placement)
		{
			IClipAdapter clipAdapter = LayeredContent.CreateClipAdapter(_clipAdapterType);
			clipAdapter.SetPaths(_num, _placement, _section, _layer, "");
			this.clips.Add(_placement, clipAdapter);
		}

		// Token: 0x0600CB53 RID: 52051 RVA: 0x004A60DF File Offset: 0x004A42DF
		[PublicizedFrom(EAccessModifier.Protected)]
		public static IClipAdapter CreateClipAdapter(string _type)
		{
			return (IClipAdapter)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600CB54 RID: 52052 RVA: 0x004A60F8 File Offset: 0x004A42F8
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

		// Token: 0x0600CB55 RID: 52053 RVA: 0x004A6180 File Offset: 0x004A4380
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

		// Token: 0x0600CB56 RID: 52054 RVA: 0x004A62B0 File Offset: 0x004A44B0
		public static void ClearQueues()
		{
			foreach (ContentQueue contentQueue in LayeredContent.queueFor.Values)
			{
				contentQueue.Clear();
			}
			LayeredContent.queueFor.Clear();
		}

		// Token: 0x04009AA9 RID: 39593
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Tuple<SectionType, LayerType>, ContentQueue> queueFor = new Dictionary<Tuple<SectionType, LayerType>, ContentQueue>();

		// Token: 0x04009AAA RID: 39594
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<PlacementType, IClipAdapter> clips;
	}
}
