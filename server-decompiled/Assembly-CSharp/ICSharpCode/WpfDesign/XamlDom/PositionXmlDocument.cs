using System;
using System.Reflection;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001655 RID: 5717
	public class PositionXmlDocument : XmlDocument
	{
		// Token: 0x0600B31B RID: 45851 RVA: 0x0042DCE0 File Offset: 0x0042BEE0
		public PositionXmlDocument()
		{
			this.setImplementation();
		}

		// Token: 0x0600B31C RID: 45852 RVA: 0x0042DCEE File Offset: 0x0042BEEE
		[PublicizedFrom(EAccessModifier.ProtectedInternal)]
		public PositionXmlDocument(XmlImplementation _imp)
			: base(_imp)
		{
			this.setImplementation();
		}

		// Token: 0x0600B31D RID: 45853 RVA: 0x0042DCFD File Offset: 0x0042BEFD
		public PositionXmlDocument(XmlNameTable _nt)
			: base(_nt)
		{
			this.setImplementation();
		}

		// Token: 0x0600B31E RID: 45854 RVA: 0x0042DD0C File Offset: 0x0042BF0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImplementation()
		{
			if (this.implementationFieldInfo == null)
			{
				this.implementationFieldInfo = typeof(XmlDocument).GetField("implementation", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			this.implementationFieldInfo.SetValue(this, new PositionXmlImplementation());
		}

		// Token: 0x0600B31F RID: 45855 RVA: 0x0042DD49 File Offset: 0x0042BF49
		public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlElement(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B320 RID: 45856 RVA: 0x0042DD5A File Offset: 0x0042BF5A
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B321 RID: 45857 RVA: 0x0042DD6B File Offset: 0x0042BF6B
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new PositionXmlCDataSection(data, this, this.lineInfo);
		}

		// Token: 0x0600B322 RID: 45858 RVA: 0x0042DD7A File Offset: 0x0042BF7A
		public override XmlComment CreateComment(string data)
		{
			return new PositionXmlComment(data, this, this.lineInfo);
		}

		// Token: 0x0600B323 RID: 45859 RVA: 0x0042DD5A File Offset: 0x0042BF5A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B324 RID: 45860 RVA: 0x0042DD89 File Offset: 0x0042BF89
		public override XmlDocumentFragment CreateDocumentFragment()
		{
			return new PositionXmlDocumentFragment(this, this.lineInfo);
		}

		// Token: 0x0600B325 RID: 45861 RVA: 0x0042DD97 File Offset: 0x0042BF97
		public override XmlDocumentType CreateDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new PositionXmlDocumentType(name, publicId, systemId, internalSubset, this, this.lineInfo);
		}

		// Token: 0x0600B326 RID: 45862 RVA: 0x0042DDAA File Offset: 0x0042BFAA
		public override XmlEntityReference CreateEntityReference(string name)
		{
			return new PositionXmlEntityReference(name, this, this.lineInfo);
		}

		// Token: 0x0600B327 RID: 45863 RVA: 0x0042DDB9 File Offset: 0x0042BFB9
		public override XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE1: " + name);
			return base.CreateNode(nodeTypeString, name, namespaceURI);
		}

		// Token: 0x0600B328 RID: 45864 RVA: 0x0042DDD4 File Offset: 0x0042BFD4
		public override XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE2: " + name);
			return base.CreateNode(type, name, namespaceURI);
		}

		// Token: 0x0600B329 RID: 45865 RVA: 0x0042DDEF File Offset: 0x0042BFEF
		public override XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE3: " + name);
			return base.CreateNode(type, prefix, name, namespaceURI);
		}

		// Token: 0x0600B32A RID: 45866 RVA: 0x0042DE0C File Offset: 0x0042C00C
		public override XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
		{
			return new PositionXmlProcessingInstruction(target, data, this, this.lineInfo);
		}

		// Token: 0x0600B32B RID: 45867 RVA: 0x0042DE1C File Offset: 0x0042C01C
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string text)
		{
			return new PositionXmlSignificantWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B32C RID: 45868 RVA: 0x0042DE2B File Offset: 0x0042C02B
		public override XmlText CreateTextNode(string text)
		{
			return new PositionXmlText(text, this, this.lineInfo);
		}

		// Token: 0x0600B32D RID: 45869 RVA: 0x0042DE3A File Offset: 0x0042C03A
		public override XmlWhitespace CreateWhitespace(string text)
		{
			return new PositionXmlWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B32E RID: 45870 RVA: 0x0042DE49 File Offset: 0x0042C049
		public override XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new PositionXmlDeclaration(version, encoding, standalone, this, this.lineInfo);
		}

		// Token: 0x0600B32F RID: 45871 RVA: 0x0042DE5C File Offset: 0x0042C05C
		public override void Load(XmlReader reader)
		{
			this.lineInfo = reader as IXmlLineInfo;
			try
			{
				base.Load(reader);
			}
			finally
			{
				this.lineInfo = null;
			}
		}

		// Token: 0x0400868D RID: 34445
		[PublicizedFrom(EAccessModifier.Private)]
		public IXmlLineInfo lineInfo;

		// Token: 0x0400868E RID: 34446
		[PublicizedFrom(EAccessModifier.Private)]
		public FieldInfo implementationFieldInfo;
	}
}
