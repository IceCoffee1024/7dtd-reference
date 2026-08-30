using System;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001BAA RID: 7082
	public class PlatformLobbyId
	{
		// Token: 0x0600D25C RID: 53852 RVA: 0x004C23A3 File Offset: 0x004C05A3
		public PlatformLobbyId(EPlatformIdentifier _platformId, string _lobbyId)
		{
			this.PlatformIdentifier = _platformId;
			this.LobbyId = _lobbyId;
		}

		// Token: 0x0600D25D RID: 53853 RVA: 0x004C23B9 File Offset: 0x004C05B9
		public int GetWriteLength(Encoding encoding)
		{
			return 1 + this.LobbyId.GetBinaryWriterLength(encoding);
		}

		// Token: 0x0600D25E RID: 53854 RVA: 0x004C23C9 File Offset: 0x004C05C9
		public void Write(BinaryWriter _writer)
		{
			_writer.Write((byte)this.PlatformIdentifier);
			if (this.PlatformIdentifier != EPlatformIdentifier.None)
			{
				_writer.Write(this.LobbyId);
			}
		}

		// Token: 0x0600D25F RID: 53855 RVA: 0x004C23EC File Offset: 0x004C05EC
		public static PlatformLobbyId Read(BinaryReader _reader)
		{
			byte b = _reader.ReadByte();
			string text = ((b != 0) ? _reader.ReadString() : string.Empty);
			return new PlatformLobbyId((EPlatformIdentifier)b, text);
		}

		// Token: 0x0400A019 RID: 40985
		public static readonly PlatformLobbyId None = new PlatformLobbyId(EPlatformIdentifier.None, string.Empty);

		// Token: 0x0400A01A RID: 40986
		public readonly EPlatformIdentifier PlatformIdentifier;

		// Token: 0x0400A01B RID: 40987
		public readonly string LobbyId;
	}
}
