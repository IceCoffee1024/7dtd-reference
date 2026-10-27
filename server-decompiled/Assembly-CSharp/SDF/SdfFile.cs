using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x020016DC RID: 5852
	public class SdfFile
	{
		// Token: 0x0600B77D RID: 46973 RVA: 0x0043F236 File Offset: 0x0043D436
		public SdfFile(string path)
		{
			this.data = new SdfData();
			this.filePath = path;
		}

		// Token: 0x0600B77E RID: 46974 RVA: 0x0043F250 File Offset: 0x0043D450
		public void Load()
		{
			try
			{
				this.valuesChanged = false;
				if (SdFile.Exists(this.filePath))
				{
					using (Stream stream = SdFile.Open(this.filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						this.data.Nodes = SdfReader.Read(stream);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error opening SDF file: " + ex.Message);
			}
		}

		// Token: 0x0600B77F RID: 46975 RVA: 0x0043F2D8 File Offset: 0x0043D4D8
		public void Save()
		{
			try
			{
				if (this.valuesChanged)
				{
					if (!SdDirectory.Exists(Path.GetDirectoryName(this.filePath)))
					{
						SdDirectory.CreateDirectory(Path.GetDirectoryName(this.filePath));
					}
					using (Stream stream = SdFile.Open(this.filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
					{
						SdfWriter.Write(stream, this.data.Nodes);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error opening SDF file:");
				Log.Exception(ex);
			}
		}

		// Token: 0x0600B780 RID: 46976 RVA: 0x0043F370 File Offset: 0x0043D570
		public void Set(string name, int val)
		{
			this.data.Add(new SdfInt(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B781 RID: 46977 RVA: 0x0043F38C File Offset: 0x0043D58C
		public void Set(string name, float val)
		{
			this.data.Add(new SdfFloat(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B782 RID: 46978 RVA: 0x0043F3A8 File Offset: 0x0043D5A8
		public void Set(string name, string val)
		{
			this.Set(name, val, false);
		}

		// Token: 0x0600B783 RID: 46979 RVA: 0x0043F3B3 File Offset: 0x0043D5B3
		public void Set(string name, string val, bool isBinary)
		{
			if (!isBinary)
			{
				this.data.Add(new SdfString(name, val));
			}
			else
			{
				this.data.Add(new SdfBinary(name, val));
			}
			this.valuesChanged = true;
		}

		// Token: 0x0600B784 RID: 46980 RVA: 0x0043F3E7 File Offset: 0x0043D5E7
		public void Set(string name, byte[] byteArray)
		{
			this.data.Add(new SdfByteArray(name, byteArray));
		}

		// Token: 0x0600B785 RID: 46981 RVA: 0x0043F3FC File Offset: 0x0043D5FC
		public void Set(string name, bool val)
		{
			this.data.Add(new SdfBool(name, val));
			this.valuesChanged = true;
		}

		// Token: 0x0600B786 RID: 46982 RVA: 0x0043F418 File Offset: 0x0043D618
		public float? GetFloat(string name)
		{
			return this.data.GetFloat(name);
		}

		// Token: 0x0600B787 RID: 46983 RVA: 0x0043F426 File Offset: 0x0043D626
		public int? GetInt(string name)
		{
			return this.data.GetInt(name);
		}

		// Token: 0x0600B788 RID: 46984 RVA: 0x0043F434 File Offset: 0x0043D634
		public string GetString(string name)
		{
			return this.GetString(name, false);
		}

		// Token: 0x0600B789 RID: 46985 RVA: 0x0043F43E File Offset: 0x0043D63E
		public string GetString(string name, bool isBinary)
		{
			if (!isBinary)
			{
				return this.data.GetString(name);
			}
			return Utils.FromBase64(this.data.GetString(name));
		}

		// Token: 0x0600B78A RID: 46986 RVA: 0x0043F461 File Offset: 0x0043D661
		public byte[] GetByteArray(string name)
		{
			return this.data.GetByteArray(name);
		}

		// Token: 0x0600B78B RID: 46987 RVA: 0x0043F46F File Offset: 0x0043D66F
		public bool? GetBool(string name)
		{
			return this.data.GetBool(name);
		}

		// Token: 0x0600B78C RID: 46988 RVA: 0x0043F47D File Offset: 0x0043D67D
		public void Remove(string name)
		{
			this.data.Remove(name);
			this.valuesChanged = true;
		}

		// Token: 0x0600B78D RID: 46989 RVA: 0x0043F494 File Offset: 0x0043D694
		public string[] GetKeys()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B78E RID: 46990 RVA: 0x0043F4CC File Offset: 0x0043D6CC
		public string[] GetStoredGamePrefs()
		{
			string[] array = new string[this.data.Nodes.Count];
			this.data.Nodes.CopyKeysTo<string, SdfTag>(array);
			return array;
		}

		// Token: 0x0600B78F RID: 46991 RVA: 0x0043F504 File Offset: 0x0043D704
		public void CopyFrom(SdfFile other)
		{
			foreach (KeyValuePair<string, SdfTag> keyValuePair in other.data.Nodes)
			{
				string text;
				SdfTag sdfTag;
				keyValuePair.Deconstruct(out text, out sdfTag);
				string text2 = text;
				SdfTag sdfTag2 = sdfTag;
				this.data.Nodes[text2] = sdfTag2;
			}
			this.valuesChanged = true;
		}

		// Token: 0x04008931 RID: 35121
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SdfData data;

		// Token: 0x04008932 RID: 35122
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string filePath;

		// Token: 0x04008933 RID: 35123
		[PublicizedFrom(EAccessModifier.Private)]
		public bool valuesChanged;
	}
}
