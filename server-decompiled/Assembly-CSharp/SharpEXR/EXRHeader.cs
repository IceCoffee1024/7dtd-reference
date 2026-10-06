using System;
using System.Collections.Generic;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x020016F3 RID: 5875
	public class EXRHeader
	{
		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x0600B78C RID: 46988 RVA: 0x00440204 File Offset: 0x0043E404
		// (set) Token: 0x0600B78D RID: 46989 RVA: 0x0044020C File Offset: 0x0043E40C
		public Dictionary<string, EXRAttribute> Attributes
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B78E RID: 46990 RVA: 0x00440215 File Offset: 0x0043E415
		public EXRHeader()
		{
			this.Attributes = new Dictionary<string, EXRAttribute>();
		}

		// Token: 0x0600B78F RID: 46991 RVA: 0x00440228 File Offset: 0x0043E428
		public void Read(EXRFile file, IEXRReader reader)
		{
			EXRAttribute exrattribute;
			while (EXRAttribute.Read(file, reader, out exrattribute))
			{
				this.Attributes[exrattribute.Name] = exrattribute;
			}
		}

		// Token: 0x0600B790 RID: 46992 RVA: 0x00440254 File Offset: 0x0043E454
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

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x0600B791 RID: 46993 RVA: 0x004402F7 File Offset: 0x0043E4F7
		public bool IsEmpty
		{
			get
			{
				return this.Attributes.Count == 0;
			}
		}

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x0600B792 RID: 46994 RVA: 0x00440308 File Offset: 0x0043E508
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

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x0600B793 RID: 46995 RVA: 0x00440330 File Offset: 0x0043E530
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

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x0600B794 RID: 46996 RVA: 0x00440358 File Offset: 0x0043E558
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

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x0600B795 RID: 46997 RVA: 0x00440380 File Offset: 0x0043E580
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

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x0600B796 RID: 46998 RVA: 0x004403A8 File Offset: 0x0043E5A8
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

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x0600B797 RID: 46999 RVA: 0x004403D0 File Offset: 0x0043E5D0
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

		// Token: 0x0400892A RID: 35114
		public static readonly Chromaticities DefaultChromaticities = new Chromaticities(0.64f, 0.33f, 0.3f, 0.6f, 0.15f, 0.06f, 0.3127f, 0.329f);
	}
}
