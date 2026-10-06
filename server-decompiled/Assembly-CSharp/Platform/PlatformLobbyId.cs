using System;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001C03 RID: 7171
	public class PlatformLobbyId
	{
		// Token: 0x0600D562 RID: 54626 RVA: 0x004CD27F File Offset: 0x004CB47F
		public PlatformLobbyId(EPlatformIdentifier _platformId, string _lobbyId)
		{
			this.PlatformIdentifier = _platformId;
			this.LobbyId = _lobbyId;
		}

		// Token: 0x0600D563 RID: 54627 RVA: 0x004CD295 File Offset: 0x004CB495
		public int GetWriteLength(Encoding encoding)
		{
			return 1 + this.LobbyId.GetBinaryWriterLength(encoding);
		}

		// Token: 0x0600D564 RID: 54628 RVA: 0x004CD2A5 File Offset: 0x004CB4A5
		public void Write(BinaryWriter _writer)
		{
			_writer.Write((byte)this.PlatformIdentifier);
			if (this.PlatformIdentifier != EPlatformIdentifier.None)
			{
				_writer.Write(this.LobbyId);
			}
		}

		// Token: 0x0600D565 RID: 54629 RVA: 0x004CD2C8 File Offset: 0x004CB4C8
		public static PlatformLobbyId Read(BinaryReader _reader)
		{
			byte b = _reader.ReadByte();
			string text = ((b != 0) ? _reader.ReadString() : string.Empty);
			return new PlatformLobbyId((EPlatformIdentifier)b, text);
		}

		// Token: 0x0400A21F RID: 41503
		public static readonly PlatformLobbyId None = new PlatformLobbyId(EPlatformIdentifier.None, string.Empty);

		// Token: 0x0400A220 RID: 41504
		public readonly EPlatformIdentifier PlatformIdentifier;

		// Token: 0x0400A221 RID: 41505
		public readonly string LobbyId;
	}
}
