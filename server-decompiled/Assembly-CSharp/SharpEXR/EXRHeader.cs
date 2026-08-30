using System;
using System.Collections.Generic;
using SharpEXR.AttributeTypes;

namespace SharpEXR
{
	// Token: 0x020016A1 RID: 5793
	public class EXRHeader
	{
		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x0600B4B2 RID: 46258 RVA: 0x00435F6C File Offset: 0x0043416C
		// (set) Token: 0x0600B4B3 RID: 46259 RVA: 0x00435F74 File Offset: 0x00434174
		public Dictionary<string, EXRAttribute> Attributes
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600B4B4 RID: 46260 RVA: 0x00435F7D File Offset: 0x0043417D
		public EXRHeader()
		{
			this.Attributes = new Dictionary<string, EXRAttribute>();
		}

		// Token: 0x0600B4B5 RID: 46261 RVA: 0x00435F90 File Offset: 0x00434190
		public void Read(EXRFile file, IEXRReader reader)
		{
			EXRAttribute exrattribute;
			while (EXRAttribute.Read(file, reader, out exrattribute))
			{
				this.Attributes[exrattribute.Name] = exrattribute;
			}
		}

		// Token: 0x0600B4B6 RID: 46262 RVA: 0x00435FBC File Offset: 0x004341BC
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

		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x0600B4B7 RID: 46263 RVA: 0x0043605F File Offset: 0x0043425F
		public bool IsEmpty
		{
			get
			{
				return this.Attributes.Count == 0;
			}
		}

		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x0600B4B8 RID: 46264 RVA: 0x00436070 File Offset: 0x00434270
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

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x0600B4B9 RID: 46265 RVA: 0x00436098 File Offset: 0x00434298
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

		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x0600B4BA RID: 46266 RVA: 0x004360C0 File Offset: 0x004342C0
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

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x0600B4BB RID: 46267 RVA: 0x004360E8 File Offset: 0x004342E8
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

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x0600B4BC RID: 46268 RVA: 0x00436110 File Offset: 0x00434310
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

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x0600B4BD RID: 46269 RVA: 0x00436138 File Offset: 0x00434338
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

		// Token: 0x04008749 RID: 34633
		public static readonly Chromaticities DefaultChromaticities = new Chromaticities(0.64f, 0.33f, 0.3f, 0.6f, 0.15f, 0.06f, 0.3127f, 0.329f);
	}
}
