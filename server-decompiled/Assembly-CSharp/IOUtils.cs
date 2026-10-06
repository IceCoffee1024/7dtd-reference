using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using Force.Crc32;

// Token: 0x02001444 RID: 5188
public static class IOUtils
{
	// Token: 0x0600A2BB RID: 41659 RVA: 0x003D0B3C File Offset: 0x003CED3C
	public static byte[] CalcHashSync(string _filename, string _algorithmName = "MD5")
	{
		byte[] array;
		using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(_algorithmName))
		{
			using (Stream stream = SdFile.OpenRead(_filename))
			{
				array = hashAlgorithm.ComputeHash(stream);
			}
		}
		return array;
	}

	// Token: 0x0600A2BC RID: 41660 RVA: 0x003D0B94 File Offset: 0x003CED94
	public static IEnumerator CalcHashCoroutine(string _filename, Action<byte[]> _resultCallback, int _maxTimePerFrameMs = 5, byte[] _buffer = null, string _algorithmName = "MD5")
	{
		MicroStopwatch msw = new MicroStopwatch(true);
		using (HashAlgorithm hashAlgo = HashAlgorithm.Create(_algorithmName))
		{
			using (Stream stream = SdFile.OpenRead(_filename))
			{
				if (_buffer == null || _buffer.Length < 8192)
				{
					_buffer = new byte[32768];
				}
				int bufferSize = _buffer.Length;
				int bytesRead;
				do
				{
					bytesRead = stream.Read(_buffer, 0, bufferSize);
					if (bytesRead > 0)
					{
						hashAlgo.TransformBlock(_buffer, 0, bytesRead, null, 0);
					}
					if (msw.ElapsedMilliseconds >= (long)_maxTimePerFrameMs)
					{
						yield return null;
						msw.ResetAndRestart();
					}
				}
				while (bytesRead > 0);
				hashAlgo.TransformFinalBlock(_buffer, 0, 0);
				_resultCallback(hashAlgo.Hash);
			}
			Stream stream = null;
		}
		HashAlgorithm hashAlgo = null;
		yield break;
		yield break;
	}

	// Token: 0x0600A2BD RID: 41661 RVA: 0x003D0BC0 File Offset: 0x003CEDC0
	public static IEnumerator CalcCrcCoroutine(string _filename, Action<uint> _resultCallback, int _maxTimePerFrameMs = 5, byte[] _buffer = null)
	{
		MicroStopwatch msw = new MicroStopwatch(true);
		using (Stream stream = SdFile.OpenRead(_filename))
		{
			if (_buffer == null || _buffer.Length < 8192)
			{
				_buffer = new byte[32768];
			}
			int bufferSize = _buffer.Length;
			uint crc = 0U;
			int bytesRead;
			do
			{
				bytesRead = stream.Read(_buffer, 0, bufferSize);
				if (bytesRead > 0)
				{
					crc = Crc32Algorithm.Append(crc, _buffer, 0, bytesRead);
				}
				if (msw.ElapsedMilliseconds >= (long)_maxTimePerFrameMs)
				{
					yield return null;
					msw.ResetAndRestart();
				}
			}
			while (bytesRead > 0);
			_resultCallback(crc);
		}
		Stream stream = null;
		yield break;
		yield break;
	}

	// Token: 0x0600A2BE RID: 41662 RVA: 0x003D0BE4 File Offset: 0x003CEDE4
	public static uint HashUint(this Crc32Algorithm _crcInstance)
	{
		if (_crcInstance == null)
		{
			throw new ArgumentNullException("_crcInstance");
		}
		byte[] hash = _crcInstance.Hash;
		return (uint)(((int)hash[0] << 24) | ((int)hash[1] << 16) | ((int)hash[2] << 8) | (int)hash[3]);
	}

	// Token: 0x04007A37 RID: 31287
	[PublicizedFrom(EAccessModifier.Private)]
	public const int minBufferSize = 8192;

	// Token: 0x04007A38 RID: 31288
	public const int DefaultBufferSize = 32768;
}
