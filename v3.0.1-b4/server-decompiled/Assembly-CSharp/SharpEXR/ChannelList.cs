using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x0200169C RID: 5788
	public class ChannelList : IEnumerable<Channel>, IEnumerable
	{
		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x0600B48A RID: 46218 RVA: 0x00434ABD File Offset: 0x00432CBD
		// (set) Token: 0x0600B48B RID: 46219 RVA: 0x00434AC5 File Offset: 0x00432CC5
		public List<Channel> Channels { get; set; }

		// Token: 0x0600B48C RID: 46220 RVA: 0x00434ACE File Offset: 0x00432CCE
		public ChannelList()
		{
			this.Channels = new List<Channel>();
		}

		// Token: 0x0600B48D RID: 46221 RVA: 0x00434AE4 File Offset: 0x00432CE4
		public void Read(EXRFile file, IEXRReader reader, int size)
		{
			int num = 0;
			Channel channel;
			int num2;
			while (this.ReadChannel(file, reader, out channel, out num2))
			{
				this.Channels.Add(channel);
				num += num2;
				if (num > size)
				{
					throw new EXRFormatException(string.Concat(new string[]
					{
						"Read ",
						num.ToString(),
						" bytes but Size was ",
						size.ToString(),
						"."
					}));
				}
			}
			num += num2;
			if (num != size)
			{
				throw new EXRFormatException(string.Concat(new string[]
				{
					"Read ",
					num.ToString(),
					" bytes but Size was ",
					size.ToString(),
					"."
				}));
			}
		}

		// Token: 0x0600B48E RID: 46222 RVA: 0x00434B9C File Offset: 0x00432D9C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool ReadChannel(EXRFile file, IEXRReader reader, out Channel channel, out int bytesRead)
		{
			int position = reader.Position;
			string text = reader.ReadNullTerminatedString(255);
			if (text == "")
			{
				channel = null;
				bytesRead = reader.Position - position;
				return false;
			}
			channel = new Channel(text, (PixelType)reader.ReadInt32(), reader.ReadByte() > 0, reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadInt32(), reader.ReadInt32());
			bytesRead = reader.Position - position;
			return true;
		}

		// Token: 0x0600B48F RID: 46223 RVA: 0x00434C1A File Offset: 0x00432E1A
		public IEnumerator<Channel> GetEnumerator()
		{
			return this.Channels.GetEnumerator();
		}

		// Token: 0x0600B490 RID: 46224 RVA: 0x00434C2C File Offset: 0x00432E2C
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170015C4 RID: 5572
		public Channel this[int index]
		{
			get
			{
				return this.Channels[index];
			}
			set
			{
				this.Channels[index] = value;
			}
		}
	}
}
