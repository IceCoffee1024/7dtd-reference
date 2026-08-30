using System;
using System.Collections.Generic;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x020016C7 RID: 5831
	public class EXRHeader
	{
		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x0600B635 RID: 46645 RVA: 0x0043BCE4 File Offset: 0x00439EE4
		// (set) Token: 0x0600B636 RID: 46646 RVA: 0x0043BCEC File Offset: 0x00439EEC
		public Dictionary<string, EXRAttribute> Attributes
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B637 RID: 46647 RVA: 0x0043BCF5 File Offset: 0x00439EF5
		public EXRHeader()
		{
			this.Attributes = new Dictionary<string, EXRAttribute>();
		}

		// Token: 0x0600B638 RID: 46648 RVA: 0x0043BD08 File Offset: 0x00439F08
		public void Read(EXRFile file, IEXRReader reader)
		{
			EXRAttribute exrattribute;
			while (EXRAttribute.Read(file, reader, out exrattribute))
			{
				this.Attributes[exrattribute.Name] = exrattribute;
			}
		}

		// Token: 0x0600B639 RID: 46649 RVA: 0x0043BD34 File Offset: 0x00439F34
		public bool TryGetAttribute<T>(string name, out T result)
		{
			EXRAttribute exrattribute;
			if (!this.Attributes.TryGetValue(name, out exrattribute))
			{
				result = default(T);
				return false;
			}
			if (exrattribute.Value == null)
			{
				result = default(T);
				return !typeof(T).IsClass && !typeof(T).IsInterface && !typeof(T).IsArray;
			}
			if (typeof(T).IsAssignableFrom(exrattribute.Value.GetType()))
			{
				result = (T)((object)exrattribute.Value);
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x0600B63A RID: 46650 RVA: 0x0043BDD7 File Offset: 0x00439FD7
		public bool IsEmpty
		{
			get
			{
				return this.Attributes.Count == 0;
			}
		}

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x0600B63B RID: 46651 RVA: 0x0043BDE8 File Offset: 0x00439FE8
		public int ChunkCount
		{
			get
			{
				int num;
				if (!this.TryGetAttribute<int>("chunkCount", out num))
				{
					throw new EXRFormatException("Invalid or corrupt EXR header: Missing chunkCount attribute.");
				}
				return num;
			}
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x0600B63C RID: 46652 RVA: 0x0043BE10 File Offset: 0x0043A010
		public Box2I DataWindow
		{
			get
			{
				Box2I box2I;
				if (!this.TryGetAttribute<Box2I>("dataWindow", out box2I))
				{
					throw new EXRFormatException("Invalid or corrupt EXR header: Missing dataWindow attribute.");
				}
				return box2I;
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x0600B63D RID: 46653 RVA: 0x0043BE38 File Offset: 0x0043A038
		public EXRCompression Compression
		{
			get
			{
				EXRCompression exrcompression;
				if (!this.TryGetAttribute<EXRCompression>("compression", out exrcompression))
				{
					throw new EXRFormatException("Invalid or corrupt EXR header: Missing compression attribute.");
				}
				return exrcompression;
			}
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x0600B63E RID: 46654 RVA: 0x0043BE60 File Offset: 0x0043A060
		public PartType Type
		{
			get
			{
				PartType partType;
				if (!this.TryGetAttribute<PartType>("type", out partType))
				{
					throw new EXRFormatException("Invalid or corrupt EXR header: Missing type attribute.");
				}
				return partType;
			}
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x0600B63F RID: 46655 RVA: 0x0043BE88 File Offset: 0x0043A088
		public ChannelList Channels
		{
			get
			{
				ChannelList channelList;
				if (!this.TryGetAttribute<ChannelList>("channels", out channelList))
				{
					throw new EXRFormatException("Invalid or corrupt EXR header: Missing channels attribute.");
				}
				return channelList;
			}
		}

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x0600B640 RID: 46656 RVA: 0x0043BEB0 File Offset: 0x0043A0B0
		public Chromaticities Chromaticities
		{
			get
			{
				foreach (EXRAttribute exrattribute in this.Attributes.Values)
				{
					if (exrattribute.Type == "chromaticities" && exrattribute.Value is Chromaticities)
					{
						return (Chromaticities)exrattribute.Value;
					}
				}
				return EXRHeader.DefaultChromaticities;
			}
		}

		// Token: 0x04008865 RID: 34917
		public static readonly Chromaticities DefaultChromaticities = new Chromaticities(0.64f, 0.33f, 0.3f, 0.6f, 0.15f, 0.06f, 0.3127f, 0.329f);
	}
}
