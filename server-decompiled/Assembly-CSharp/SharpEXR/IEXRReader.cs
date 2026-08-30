using System;

namespace SharpEXR
{
	// Token: 0x020016A5 RID: 5797
	public interface IEXRReader : IDisposable
	{
		// Token: 0x0600B4E1 RID: 46305
		byte ReadByte();

		// Token: 0x0600B4E2 RID: 46306
		int ReadInt32();

		// Token: 0x0600B4E3 RID: 46307
		uint ReadUInt32();

		// Token: 0x0600B4E4 RID: 46308
		Half ReadHalf();

		// Token: 0x0600B4E5 RID: 46309
		float ReadSingle();

		// Token: 0x0600B4E6 RID: 46310
		double ReadDouble();

		// Token: 0x0600B4E7 RID: 46311
		string ReadNullTerminatedString(int maxLength);

		// Token: 0x0600B4E8 RID: 46312
		string ReadString(int length);

		// Token: 0x0600B4E9 RID: 46313
		string ReadString();

		// Token: 0x0600B4EA RID: 46314
		byte[] ReadBytes(int count);

		// Token: 0x0600B4EB RID: 46315
		void CopyBytes(byte[] dest, int offset, int count);

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x0600B4EC RID: 46316
		// (set) Token: 0x0600B4ED RID: 46317
		int Position { get; set; }
	}
}
