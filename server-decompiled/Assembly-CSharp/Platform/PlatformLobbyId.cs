using System;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001C34 RID: 7220
	public class PlatformLobbyId
	{
		// Token: 0x0600D676 RID: 54902 RVA: 0x004D0DA7 File Offset: 0x004CEFA7
		public PlatformLobbyId(EPlatformIdentifier _platformId, string _lobbyId)
		{
			this.PlatformIdentifier = _platformId;
			this.LobbyId = _lobbyId;
		}

		// Token: 0x0600D677 RID: 54903 RVA: 0x004D0DBD File Offset: 0x004CEFBD
		public int GetWriteLength(Encoding encoding)
		{
			return 1 + this.LobbyId.GetBinaryWriterLength(encoding);
		}

		// Token: 0x0600D678 RID: 54904 RVA: 0x004D0DCD File Offset: 0x004CEFCD
		public void Write(BinaryWriter _writer)
		{
			_writer.Write((byte)this.PlatformIdentifier);
			if (this.PlatformIdentifier != EPlatformIdentifier.None)
			{
				_writer.Write(this.LobbyId);
			}
		}

		// Token: 0x0600D679 RID: 54905 RVA: 0x004D0DF0 File Offset: 0x004CEFF0
		public static PlatformLobbyId Read(BinaryReader _reader)
		{
			byte b = _reader.ReadByte();
			string text = ((b != 0) ? _reader.ReadString() : string.Empty);
			return new PlatformLobbyId((EPlatformIdentifier)b, text);
		}

		// Token: 0x0400A308 RID: 41736
		public static readonly PlatformLobbyId None = new PlatformLobbyId(EPlatformIdentifier.None, string.Empty);

		// Token: 0x0400A309 RID: 41737
		public readonly EPlatformIdentifier PlatformIdentifier;

		// Token: 0x0400A30A RID: 41738
		public readonly string LobbyId;
	}
}
