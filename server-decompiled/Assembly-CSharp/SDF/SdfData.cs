using System;
using System.Collections.Generic;

namespace SDF
{
	// Token: 0x02001675 RID: 5749
	public class SdfData
	{
		// Token: 0x0600B3F8 RID: 46072 RVA: 0x00432C48 File Offset: 0x00430E48
		public SdfData()
		{
			this.Nodes = new Dictionary<string, SdfTag>();
		}

		// Token: 0x0600B3F9 RID: 46073 RVA: 0x00432C5C File Offset: 0x00430E5C
		public bool Add(SdfTag sdfTag)
		{
			if (this.Nodes.ContainsKey(sdfTag.Name))
			{
				this.Nodes[sdfTag.Name].Value = sdfTag.Value;
			}
			else
			{
				this.Nodes.Add(sdfTag.Name, sdfTag);
			}
			return true;
		}

		// Token: 0x0600B3FA RID: 46074 RVA: 0x00432CAD File Offset: 0x00430EAD
		public bool Remove(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return false;
			}
			this.Nodes.Remove(tagName);
			return true;
		}

		// Token: 0x0600B3FB RID: 46075 RVA: 0x00432CD0 File Offset: 0x00430ED0
		public int? GetInt(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Int)
			{
				return null;
			}
			return new int?(Convert.ToInt32(this.Nodes[tagName].Value));
		}

		// Token: 0x0600B3FC RID: 46076 RVA: 0x00432D30 File Offset: 0x00430F30
		public float? GetFloat(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Float)
			{
				return null;
			}
			return new float?((float)this.Nodes[tagName].Value);
		}

		// Token: 0x0600B3FD RID: 46077 RVA: 0x00432D8E File Offset: 0x00430F8E
		public string GetString(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.String)
			{
				return null;
			}
			return this.Nodes[tagName].Value.ToString();
		}

		// Token: 0x0600B3FE RID: 46078 RVA: 0x00432DCC File Offset: 0x00430FCC
		public bool? GetBool(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Bool)
			{
				return null;
			}
			return new bool?((bool)this.Nodes[tagName].Value);
		}

		// Token: 0x0600B3FF RID: 46079 RVA: 0x00432E2A File Offset: 0x0043102A
		public string GetBinary(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				return null;
			}
			if (this.Nodes[tagName].TagType != SdfTagType.Binary)
			{
				return null;
			}
			return (string)this.Nodes[tagName].Value;
		}

		// Token: 0x0600B400 RID: 46080 RVA: 0x00432E68 File Offset: 0x00431068
		public byte[] GetByteArray(string tagName)
		{
			if (!this.Nodes.ContainsKey(tagName))
			{
				throw new KeyNotFoundException();
			}
			if (this.Nodes[tagName].TagType == SdfTagType.ByteArray)
			{
				throw new InvalidCastException();
			}
			return (byte[])this.Nodes[tagName].Value;
		}

		// Token: 0x040086A3 RID: 34467
		public Dictionary<string, SdfTag> Nodes;
	}
}
