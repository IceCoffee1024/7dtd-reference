using System;
using System.IO;
using UnityEngine;

// Token: 0x02001403 RID: 5123
public interface IBinaryReaderOrWriter
{
	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x0600A0D9 RID: 41177
	Stream BaseStream { get; }

	// Token: 0x0600A0DA RID: 41178
	bool ReadWrite(bool _value);

	// Token: 0x0600A0DB RID: 41179
	byte ReadWrite(byte _value);

	// Token: 0x0600A0DC RID: 41180
	sbyte ReadWrite(sbyte _value);

	// Token: 0x0600A0DD RID: 41181
	char ReadWrite(char _value);

	// Token: 0x0600A0DE RID: 41182
	short ReadWrite(short _value);

	// Token: 0x0600A0DF RID: 41183
	ushort ReadWrite(ushort _value);

	// Token: 0x0600A0E0 RID: 41184
	int ReadWrite(int _value);

	// Token: 0x0600A0E1 RID: 41185
	uint ReadWrite(uint _value);

	// Token: 0x0600A0E2 RID: 41186
	long ReadWrite(long _value);

	// Token: 0x0600A0E3 RID: 41187
	ulong ReadWrite(ulong _value);

	// Token: 0x0600A0E4 RID: 41188
	float ReadWrite(float _value);

	// Token: 0x0600A0E5 RID: 41189
	double ReadWrite(double _value);

	// Token: 0x0600A0E6 RID: 41190
	decimal ReadWrite(decimal _value);

	// Token: 0x0600A0E7 RID: 41191
	string ReadWrite(string _value);

	// Token: 0x0600A0E8 RID: 41192
	void ReadWrite(byte[] _buffer, int _index, int _count);

	// Token: 0x0600A0E9 RID: 41193
	Vector3 ReadWrite(Vector3 _value);
}
