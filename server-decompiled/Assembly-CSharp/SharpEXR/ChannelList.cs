using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x020016EE RID: 5870
	public class ChannelList : IEnumerable<Channel>, IEnumerable
	{
		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x0600B764 RID: 46948 RVA: 0x0043ED55 File Offset: 0x0043CF55
		// (set) Token: 0x0600B765 RID: 46949 RVA: 0x0043ED5D File Offset: 0x0043CF5D
		public List<Channel> Channels { get; set; }

		// Token: 0x0600B766 RID: 46950 RVA: 0x0043ED66 File Offset: 0x0043CF66
		public ChannelList()
		{
			this.Channels = new List<Channel>();
		}

		// Token: 0x0600B767 RID: 46951 RVA: 0x0043ED7C File Offset: 0x0043CF7C
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

		// Token: 0x0600B768 RID: 46952 RVA: 0x0043EE34 File Offset: 0x0043D034
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

		// Token: 0x0600B769 RID: 46953 RVA: 0x0043EEB2 File Offset: 0x0043D0B2
		public IEnumerator<Channel> GetEnumerator()
		{
			return this.Channels.GetEnumerator();
		}

		// Token: 0x0600B76A RID: 46954 RVA: 0x0043EEC4 File Offset: 0x0043D0C4
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170016D8 RID: 5848
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
