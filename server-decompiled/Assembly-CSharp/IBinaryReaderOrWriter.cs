using System;
using System.IO;
using UnityEngine;

// Token: 0x0200141E RID: 5150
public interface IBinaryReaderOrWriter
{
	// Token: 0x170012F9 RID: 4857
	// (get) Token: 0x0600A1D6 RID: 41430
	Stream BaseStream { get; }

	// Token: 0x0600A1D7 RID: 41431
	bool ReadWrite(bool _value);

	// Token: 0x0600A1D8 RID: 41432
	byte ReadWrite(byte _value);

	// Token: 0x0600A1D9 RID: 41433
	sbyte ReadWrite(sbyte _value);

	// Token: 0x0600A1DA RID: 41434
	char ReadWrite(char _value);

	// Token: 0x0600A1DB RID: 41435
	short ReadWrite(short _value);

	// Token: 0x0600A1DC RID: 41436
	ushort ReadWrite(ushort _value);

	// Token: 0x0600A1DD RID: 41437
	int ReadWrite(int _value);

	// Token: 0x0600A1DE RID: 41438
	uint ReadWrite(uint _value);

	// Token: 0x0600A1DF RID: 41439
	long ReadWrite(long _value);

	// Token: 0x0600A1E0 RID: 41440
	ulong ReadWrite(ulong _value);

	// Token: 0x0600A1E1 RID: 41441
	float ReadWrite(float _value);

	// Token: 0x0600A1E2 RID: 41442
	double ReadWrite(double _value);

	// Token: 0x0600A1E3 RID: 41443
	decimal ReadWrite(decimal _value);

	// Token: 0x0600A1E4 RID: 41444
	string ReadWrite(string _value);

	// Token: 0x0600A1E5 RID: 41445
	void ReadWrite(byte[] _buffer, int _index, int _count);

	// Token: 0x0600A1E6 RID: 41446
	Vector3 ReadWrite(Vector3 _value);
}
