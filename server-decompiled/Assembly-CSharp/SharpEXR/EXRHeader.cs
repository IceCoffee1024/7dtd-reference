using System;
using System.Collections.Generic;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x02001712 RID: 5906
	public class EXRHeader
	{
		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x0600B860 RID: 47200 RVA: 0x00442CA8 File Offset: 0x00440EA8
		// (set) Token: 0x0600B861 RID: 47201 RVA: 0x00442CB0 File Offset: 0x00440EB0
		public Dictionary<string, EXRAttribute> Attributes
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B862 RID: 47202 RVA: 0x00442CB9 File Offset: 0x00440EB9
		public EXRHeader()
		{
			this.Attributes = new Dictionary<string, EXRAttribute>();
		}

		// Token: 0x0600B863 RID: 47203 RVA: 0x00442CCC File Offset: 0x00440ECC
		public void Read(EXRFile file, IEXRReader reader)
		{
			EXRAttribute exrattribute;
			while (EXRAttribute.Read(file, reader, out exrattribute))
			{
				this.Attributes[exrattribute.Name] = exrattribute;
			}
		}

		// Token: 0x0600B864 RID: 47204 RVA: 0x00442CF8 File Offset: 0x00440EF8
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

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x0600B865 RID: 47205 RVA: 0x00442D9B File Offset: 0x00440F9B
		public bool IsEmpty
		{
			get
			{
				return this.Attributes.Count == 0;
			}
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x0600B866 RID: 47206 RVA: 0x00442DAC File Offset: 0x00440FAC
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

		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x0600B867 RID: 47207 RVA: 0x00442DD4 File Offset: 0x00440FD4
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

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x0600B868 RID: 47208 RVA: 0x00442DFC File Offset: 0x00440FFC
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

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x0600B869 RID: 47209 RVA: 0x00442E24 File Offset: 0x00441024
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

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x0600B86A RID: 47210 RVA: 0x00442E4C File Offset: 0x0044104C
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

		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x0600B86B RID: 47211 RVA: 0x00442E74 File Offset: 0x00441074
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

		// Token: 0x040089DD RID: 35293
		public static readonly Chromaticities DefaultChromaticities = new Chromaticities(0.64f, 0.33f, 0.3f, 0.6f, 0.15f, 0.06f, 0.3127f, 0.329f);
	}
}
