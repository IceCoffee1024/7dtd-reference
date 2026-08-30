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
	// Token: 0x02001A02 RID: 6658
	[Preserve]
	public abstract class LayeredContent : Content
	{
		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x0600C9BF RID: 51647 RVA: 0x0049FAA1 File Offset: 0x0049DCA1
		// (set) Token: 0x0600C9C0 RID: 51648 RVA: 0x0049FAA9 File Offset: 0x0049DCA9
		public LayerType Layer
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600C9C1 RID: 51649 RVA: 0x0049FAB2 File Offset: 0x0049DCB2
		public LayeredContent()
		{
			this.clips = new EnumDictionary<PlacementType, IClipAdapter>();
		}

		// Token: 0x0600C9C2 RID: 51650
		public abstract float GetSample(PlacementType _placement, int _idx, params float[] _params);

		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x0600C9C3 RID: 51651 RVA: 0x0049FAC8 File Offset: 0x0049DCC8
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

		// Token: 0x0600C9C4 RID: 51652 RVA: 0x0049FB28 File Offset: 0x0049DD28
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

		// Token: 0x0600C9C5 RID: 51653 RVA: 0x0049FB38 File Offset: 0x0049DD38
		public void LoadImmediate()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.LoadImmediate();
			}
		}

		// Token: 0x0600C9C6 RID: 51654 RVA: 0x0049FB90 File Offset: 0x0049DD90
		public override void Unload()
		{
			foreach (IClipAdapter clipAdapter in this.clips.Values)
			{
				clipAdapter.Unload();
			}
		}

		// Token: 0x0600C9C7 RID: 51655 RVA: 0x0049FBE8 File Offset: 0x0049DDE8
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			base.Section = EnumUtils.Parse<SectionType>(_xmlNode.Parent.Parent.GetAttribute("name"), false);
			this.Layer = EnumUtils.Parse<LayerType>(_xmlNode.Parent.GetAttribute("name"), false);
		}

		// Token: 0x0600C9C8 RID: 51656 RVA: 0x0049FC44 File Offset: 0x0049DE44
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

		// Token: 0x0600C9C9 RID: 51657 RVA: 0x0049FCB8 File Offset: 0x0049DEB8
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddClipAdapter(string _clipAdapterType, int _num, SectionType _section, LayerType _layer, PlacementType _placement)
		{
			IClipAdapter clipAdapter = LayeredContent.CreateClipAdapter(_clipAdapterType);
			clipAdapter.SetPaths(_num, _placement, _section, _layer, "");
			this.clips.Add(_placement, clipAdapter);
		}

		// Token: 0x0600C9CA RID: 51658 RVA: 0x0049FCEB File Offset: 0x0049DEEB
		[PublicizedFrom(EAccessModifier.Protected)]
		public static IClipAdapter CreateClipAdapter(string _type)
		{
			return (IClipAdapter)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("DynamicMusic.", _type));
		}

		// Token: 0x0600C9CB RID: 51659 RVA: 0x0049FD04 File Offset: 0x0049DF04
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

		// Token: 0x0600C9CC RID: 51660 RVA: 0x0049FD8C File Offset: 0x0049DF8C
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

		// Token: 0x0600C9CD RID: 51661 RVA: 0x0049FEBC File Offset: 0x0049E0BC
		public static void ClearQueues()
		{
			foreach (ContentQueue contentQueue in LayeredContent.queueFor.Values)
			{
				contentQueue.Clear();
			}
			LayeredContent.queueFor.Clear();
		}

		// Token: 0x04009978 RID: 39288
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Tuple<SectionType, LayerType>, ContentQueue> queueFor = new Dictionary<Tuple<SectionType, LayerType>, ContentQueue>();

		// Token: 0x04009979 RID: 39289
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<PlacementType, IClipAdapter> clips;
	}
}
