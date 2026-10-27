using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEXR
{
	// Token: 0x0200170D RID: 5901
	public class ChannelList : IEnumerable<Channel>, IEnumerable
	{
		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x0600B838 RID: 47160 RVA: 0x004417F9 File Offset: 0x0043F9F9
		// (set) Token: 0x0600B839 RID: 47161 RVA: 0x00441801 File Offset: 0x0043FA01
		public List<Channel> Channels { get; set; }

		// Token: 0x0600B83A RID: 47162 RVA: 0x0044180A File Offset: 0x0043FA0A
		public ChannelList()
		{
			this.Channels = new List<Channel>();
		}

		// Token: 0x0600B83B RID: 47163 RVA: 0x00441820 File Offset: 0x0043FA20
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

		// Token: 0x0600B83C RID: 47164 RVA: 0x004418D8 File Offset: 0x0043FAD8
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

		// Token: 0x0600B83D RID: 47165 RVA: 0x00441956 File Offset: 0x0043FB56
		public IEnumerator<Channel> GetEnumerator()
		{
			return this.Channels.GetEnumerator();
		}

		// Token: 0x0600B83E RID: 47166 RVA: 0x00441968 File Offset: 0x0043FB68
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x1700171F RID: 5919
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
