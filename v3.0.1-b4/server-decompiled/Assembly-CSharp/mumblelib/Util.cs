using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace mumblelib
{
	// Token: 0x02001691 RID: 5777
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Util
	{
		// Token: 0x0600B436 RID: 46134 RVA: 0x0043333D File Offset: 0x0043153D
		public unsafe static void SetVector3(float* _output, Vector3 _input)
		{
			*_output = _input.x;
			_output[1] = _input.y;
			_output[2] = _input.z;
		}

		// Token: 0x0600B437 RID: 46135 RVA: 0x00433360 File Offset: 0x00431560
		public unsafe static void SetString<[IsUnmanaged] T>(T* _output, string _input, int _max, Encoding _encoding) where T : struct, ValueType
		{
			byte[] bytes = _encoding.GetBytes(_input + "\0");
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), Math.Min(bytes.Length, _max * Marshal.SizeOf<T>()));
		}

		// Token: 0x0600B438 RID: 46136 RVA: 0x0043339C File Offset: 0x0043159C
		public unsafe static void SetContext(byte* _output, uint* _len, string _input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(_input);
			*_len = (uint)Math.Min(bytes.Length, 256);
			Marshal.Copy(bytes, 0, new IntPtr((void*)_output), (int)(*_len));
		}
	}
}
