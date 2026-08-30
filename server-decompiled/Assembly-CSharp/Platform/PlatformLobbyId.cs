using System;
using System.IO;
using System.Text;

namespace Platform
{
	// Token: 0x02001BD1 RID: 7121
	public class PlatformLobbyId
	{
		// Token: 0x0600D3EA RID: 54250 RVA: 0x004C8797 File Offset: 0x004C6997
		public PlatformLobbyId(EPlatformIdentifier _platformId, string _lobbyId)
		{
			this.PlatformIdentifier = _platformId;
			this.LobbyId = _lobbyId;
		}

		// Token: 0x0600D3EB RID: 54251 RVA: 0x004C87AD File Offset: 0x004C69AD
		public int GetWriteLength(Encoding encoding)
		{
			return 1 + this.LobbyId.GetBinaryWriterLength(encoding);
		}

		// Token: 0x0600D3EC RID: 54252 RVA: 0x004C87BD File Offset: 0x004C69BD
		public void Write(BinaryWriter _writer)
		{
			_writer.Write((byte)this.PlatformIdentifier);
			if (this.PlatformIdentifier != EPlatformIdentifier.None)
			{
				_writer.Write(this.LobbyId);
			}
		}

		// Token: 0x0600D3ED RID: 54253 RVA: 0x004C87E0 File Offset: 0x004C69E0
		public static PlatformLobbyId Read(BinaryReader _reader)
		{
			byte b = _reader.ReadByte();
			string text = ((b != 0) ? _reader.ReadString() : string.Empty);
			return new PlatformLobbyId((EPlatformIdentifier)b, text);
		}

		// Token: 0x0400A14A RID: 41290
		public static readonly PlatformLobbyId None = new PlatformLobbyId(EPlatformIdentifier.None, string.Empty);

		// Token: 0x0400A14B RID: 41291
		public readonly EPlatformIdentifier PlatformIdentifier;

		// Token: 0x0400A14C RID: 41292
		public readonly string LobbyId;
	}
}
