using System;
using System.Reflection;
using System.Xml;

namespace ICSharpCode.WpfDesign.XamlDom
{
	// Token: 0x02001643 RID: 5699
	public class PositionXmlDocument : XmlDocument
	{
		// Token: 0x0600B2C3 RID: 45763 RVA: 0x0042B8DC File Offset: 0x00429ADC
		public PositionXmlDocument()
		{
			this.setImplementation();
		}

		// Token: 0x0600B2C4 RID: 45764 RVA: 0x0042B8EA File Offset: 0x00429AEA
		[PublicizedFrom(EAccessModifier.ProtectedInternal)]
		public PositionXmlDocument(XmlImplementation _imp)
			: base(_imp)
		{
			this.setImplementation();
		}

		// Token: 0x0600B2C5 RID: 45765 RVA: 0x0042B8F9 File Offset: 0x00429AF9
		public PositionXmlDocument(XmlNameTable _nt)
			: base(_nt)
		{
			this.setImplementation();
		}

		// Token: 0x0600B2C6 RID: 45766 RVA: 0x0042B908 File Offset: 0x00429B08
		[PublicizedFrom(EAccessModifier.Private)]
		public void setImplementation()
		{
			if (this.implementationFieldInfo == null)
			{
				this.implementationFieldInfo = typeof(XmlDocument).GetField("implementation", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			this.implementationFieldInfo.SetValue(this, new PositionXmlImplementation());
		}

		// Token: 0x0600B2C7 RID: 45767 RVA: 0x0042B945 File Offset: 0x00429B45
		public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlElement(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B2C8 RID: 45768 RVA: 0x0042B956 File Offset: 0x00429B56
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B2C9 RID: 45769 RVA: 0x0042B967 File Offset: 0x00429B67
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new PositionXmlCDataSection(data, this, this.lineInfo);
		}

		// Token: 0x0600B2CA RID: 45770 RVA: 0x0042B976 File Offset: 0x00429B76
		public override XmlComment CreateComment(string data)
		{
			return new PositionXmlComment(data, this, this.lineInfo);
		}

		// Token: 0x0600B2CB RID: 45771 RVA: 0x0042B956 File Offset: 0x00429B56
		[PublicizedFrom(EAccessModifier.Protected)]
		public override XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			return new PositionXmlAttribute(prefix, localName, namespaceURI, this, this.lineInfo);
		}

		// Token: 0x0600B2CC RID: 45772 RVA: 0x0042B985 File Offset: 0x00429B85
		public override XmlDocumentFragment CreateDocumentFragment()
		{
			return new PositionXmlDocumentFragment(this, this.lineInfo);
		}

		// Token: 0x0600B2CD RID: 45773 RVA: 0x0042B993 File Offset: 0x00429B93
		public override XmlDocumentType CreateDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new PositionXmlDocumentType(name, publicId, systemId, internalSubset, this, this.lineInfo);
		}

		// Token: 0x0600B2CE RID: 45774 RVA: 0x0042B9A6 File Offset: 0x00429BA6
		public override XmlEntityReference CreateEntityReference(string name)
		{
			return new PositionXmlEntityReference(name, this, this.lineInfo);
		}

		// Token: 0x0600B2CF RID: 45775 RVA: 0x0042B9B5 File Offset: 0x00429BB5
		public override XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE1: " + name);
			return base.CreateNode(nodeTypeString, name, namespaceURI);
		}

		// Token: 0x0600B2D0 RID: 45776 RVA: 0x0042B9D0 File Offset: 0x00429BD0
		public override XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE2: " + name);
			return base.CreateNode(type, name, namespaceURI);
		}

		// Token: 0x0600B2D1 RID: 45777 RVA: 0x0042B9EB File Offset: 0x00429BEB
		public override XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI)
		{
			Console.WriteLine("CREATING NODE3: " + name);
			return base.CreateNode(type, prefix, name, namespaceURI);
		}

		// Token: 0x0600B2D2 RID: 45778 RVA: 0x0042BA08 File Offset: 0x00429C08
		public override XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
		{
			return new PositionXmlProcessingInstruction(target, data, this, this.lineInfo);
		}

		// Token: 0x0600B2D3 RID: 45779 RVA: 0x0042BA18 File Offset: 0x00429C18
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string text)
		{
			return new PositionXmlSignificantWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B2D4 RID: 45780 RVA: 0x0042BA27 File Offset: 0x00429C27
		public override XmlText CreateTextNode(string text)
		{
			return new PositionXmlText(text, this, this.lineInfo);
		}

		// Token: 0x0600B2D5 RID: 45781 RVA: 0x0042BA36 File Offset: 0x00429C36
		public override XmlWhitespace CreateWhitespace(string text)
		{
			return new PositionXmlWhitespace(text, this, this.lineInfo);
		}

		// Token: 0x0600B2D6 RID: 45782 RVA: 0x0042BA45 File Offset: 0x00429C45
		public override XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new PositionXmlDeclaration(version, encoding, standalone, this, this.lineInfo);
		}

		// Token: 0x0600B2D7 RID: 45783 RVA: 0x0042BA58 File Offset: 0x00429C58
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

		// Token: 0x0400864C RID: 34380
		[PublicizedFrom(EAccessModifier.Private)]
		public IXmlLineInfo lineInfo;

		// Token: 0x0400864D RID: 34381
		[PublicizedFrom(EAccessModifier.Private)]
		public FieldInfo implementationFieldInfo;
	}
}
