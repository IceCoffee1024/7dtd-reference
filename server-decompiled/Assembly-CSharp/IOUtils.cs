using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using Force.Crc32;

// Token: 0x02001456 RID: 5206
public static class IOUtils
{
	// Token: 0x0600A313 RID: 41747 RVA: 0x003D2F40 File Offset: 0x003D1140
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

	// Token: 0x0600A314 RID: 41748 RVA: 0x003D2F98 File Offset: 0x003D1198
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

	// Token: 0x0600A315 RID: 41749 RVA: 0x003D2FC4 File Offset: 0x003D11C4
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

	// Token: 0x0600A316 RID: 41750 RVA: 0x003D2FE8 File Offset: 0x003D11E8
	public static uint HashUint(this Crc32Algorithm _crcInstance)
	{
		if (_crcInstance == null)
		{
			throw new ArgumentNullException("_crcInstance");
		}
		byte[] hash = _crcInstance.Hash;
		return (uint)(((int)hash[0] << 24) | ((int)hash[1] << 16) | ((int)hash[2] << 8) | (int)hash[3]);
	}

	// Token: 0x04007A78 RID: 31352
	[PublicizedFrom(EAccessModifier.Private)]
	public const int minBufferSize = 8192;

	// Token: 0x04007A79 RID: 31353
	public const int DefaultBufferSize = 32768;
}
