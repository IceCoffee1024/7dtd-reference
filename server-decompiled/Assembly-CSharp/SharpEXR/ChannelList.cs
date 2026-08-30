using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x020016C2 RID: 5826
	public class ChannelList : IEnumerable<Channel>, IEnumerable
	{
		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x0600B60D RID: 46605 RVA: 0x0043A835 File Offset: 0x00438A35
		// (set) Token: 0x0600B60E RID: 46606 RVA: 0x0043A83D File Offset: 0x00438A3D
		public List<Channel> Channels { get; set; }

		// Token: 0x0600B60F RID: 46607 RVA: 0x0043A846 File Offset: 0x00438A46
		public ChannelList()
		{
			this.Channels = new List<Channel>();
		}

		// Token: 0x0600B610 RID: 46608 RVA: 0x0043A85C File Offset: 0x00438A5C
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

		// Token: 0x0600B611 RID: 46609 RVA: 0x0043A914 File Offset: 0x00438B14
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

		// Token: 0x0600B612 RID: 46610 RVA: 0x0043A992 File Offset: 0x00438B92
		public IEnumerator<Channel> GetEnumerator()
		{
			return this.Channels.GetEnumerator();
		}

		// Token: 0x0600B613 RID: 46611 RVA: 0x0043A9A4 File Offset: 0x00438BA4
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17001636 RID: 5686
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
