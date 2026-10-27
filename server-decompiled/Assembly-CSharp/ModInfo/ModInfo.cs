using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.WpfDesign.XamlDom;
using XMLData;
using XMLData.Exceptions;
using XMLData.Parsers;

namespace ModInfo
{
	// Token: 0x02001652 RID: 5714
	public class ModInfo : IXMLData
	{
		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x0600B308 RID: 45832 RVA: 0x0042D5C4 File Offset: 0x0042B7C4
		// (set) Token: 0x0600B309 RID: 45833 RVA: 0x0042D5CC File Offset: 0x0042B7CC
		public DataItem<string> Name
		{
			get
			{
				return this.pName;
			}
			set
			{
				this.pName = value;
			}
		}

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x0600B30A RID: 45834 RVA: 0x0042D5D5 File Offset: 0x0042B7D5
		// (set) Token: 0x0600B30B RID: 45835 RVA: 0x0042D5DD File Offset: 0x0042B7DD
		public DataItem<string> Description
		{
			get
			{
				return this.pDescription;
			}
			set
			{
				this.pDescription = value;
			}
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x0600B30C RID: 45836 RVA: 0x0042D5E6 File Offset: 0x0042B7E6
		// (set) Token: 0x0600B30D RID: 45837 RVA: 0x0042D5EE File Offset: 0x0042B7EE
		public DataItem<string> Author
		{
			get
			{
				return this.pAuthor;
			}
			set
			{
				this.pAuthor = value;
			}
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x0600B30E RID: 45838 RVA: 0x0042D5F7 File Offset: 0x0042B7F7
		// (set) Token: 0x0600B30F RID: 45839 RVA: 0x0042D5FF File Offset: 0x0042B7FF
		public DataItem<string> Version
		{
			get
			{
				return this.pVersion;
			}
			set
			{
				this.pVersion = value;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x0600B310 RID: 45840 RVA: 0x0042D608 File Offset: 0x0042B808
		// (set) Token: 0x0600B311 RID: 45841 RVA: 0x0042D610 File Offset: 0x0042B810
		public DataItem<string> Website
		{
			get
			{
				return this.pWebsite;
			}
			set
			{
				this.pWebsite = value;
			}
		}

		// Token: 0x04008687 RID: 34439
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pName;

		// Token: 0x04008688 RID: 34440
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pDescription;

		// Token: 0x04008689 RID: 34441
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pAuthor;

		// Token: 0x0400868A RID: 34442
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pVersion;

		// Token: 0x0400868B RID: 34443
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<string> pWebsite;

		// Token: 0x02001653 RID: 5715
		public static class Parser
		{
			// Token: 0x0600B313 RID: 45843 RVA: 0x0042D61C File Offset: 0x0042B81C
			public static ModInfo Parse(XElement _elem, Dictionary<PositionXmlElement, DataItem<ModInfo>> _updateLater)
			{
				ModInfo modInfo = new ModInfo();
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (XElement xelement in _elem.Elements())
				{
					if (!ModInfo.Parser.knownAttributesMultiplicity.ContainsKey(xelement.Name.LocalName))
					{
						string text = "Unknown element \"";
						XName name = xelement.Name;
						throw new UnexpectedElementException(text + ((name != null) ? name.ToString() : null) + "\" found while parsing ModInfo", ((IXmlLineInfo)xelement).LineNumber);
					}
					string localName = xelement.Name.LocalName;
					if (!(localName == "Name"))
					{
						if (!(localName == "Description"))
						{
							if (!(localName == "Author"))
							{
								if (!(localName == "Version"))
								{
									if (localName == "Website")
									{
										ModInfo.Parser.ParseFieldAttributeWebsite(modInfo, dictionary, xelement);
									}
								}
								else
								{
									ModInfo.Parser.ParseFieldAttributeVersion(modInfo, dictionary, xelement);
								}
							}
							else
							{
								ModInfo.Parser.ParseFieldAttributeAuthor(modInfo, dictionary, xelement);
							}
						}
						else
						{
							ModInfo.Parser.ParseFieldAttributeDescription(modInfo, dictionary, xelement);
						}
					}
					else
					{
						ModInfo.Parser.ParseFieldAttributeName(modInfo, dictionary, xelement);
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in ModInfo.Parser.knownAttributesMultiplicity)
				{
					int num = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num > keyValuePair.Value.max))
					{
						string[] array = new string[6];
						array[0] = "Element has incorrect number of \"";
						array[1] = keyValuePair.Key;
						array[2] = "\" attribute instances, found ";
						array[3] = num.ToString();
						array[4] = ", expected ";
						int num2 = 5;
						Range<int> value = keyValuePair.Value;
						array[num2] = ((value != null) ? value.ToString() : null);
						throw new IncorrectAttributeOccurrenceException(string.Concat(array), ((IXmlLineInfo)_elem).LineNumber);
					}
				}
				return modInfo;
			}

			// Token: 0x0600B314 RID: 45844 RVA: 0x0042D83C File Offset: 0x0042BA3C
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeName(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Name\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Name", text2);
				_entry.pName = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Name"))
					{
						_foundAttributes["Name"] = 0;
					}
					int num = _foundAttributes["Name"];
					_foundAttributes["Name"] = num + 1;
				}
			}

			// Token: 0x0600B315 RID: 45845 RVA: 0x0042D8E8 File Offset: 0x0042BAE8
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeDescription(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Description\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Description", text2);
				_entry.pDescription = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Description"))
					{
						_foundAttributes["Description"] = 0;
					}
					int num = _foundAttributes["Description"];
					_foundAttributes["Description"] = num + 1;
				}
			}

			// Token: 0x0600B316 RID: 45846 RVA: 0x0042D994 File Offset: 0x0042BB94
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeAuthor(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Author\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Author", text2);
				_entry.pAuthor = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Author"))
					{
						_foundAttributes["Author"] = 0;
					}
					int num = _foundAttributes["Author"];
					_foundAttributes["Author"] = num + 1;
				}
			}

			// Token: 0x0600B317 RID: 45847 RVA: 0x0042DA40 File Offset: 0x0042BC40
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeVersion(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Version\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Version", text2);
				_entry.pVersion = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Version"))
					{
						_foundAttributes["Version"] = 0;
					}
					int num = _foundAttributes["Version"];
					_foundAttributes["Version"] = num + 1;
				}
			}

			// Token: 0x0600B318 RID: 45848 RVA: 0x0042DAEC File Offset: 0x0042BCEC
			[PublicizedFrom(EAccessModifier.Private)]
			public static void ParseFieldAttributeWebsite(ModInfo _entry, Dictionary<string, int> _foundAttributes, XElement _elem)
			{
				string text = null;
				if (_elem != null)
				{
					text = ParserUtils.ParseStringAttribute(_elem, "value", true, null);
				}
				string text2;
				try
				{
					text2 = stringParser.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidValueException("Could not parse attribute \"Website\" value \"" + text + "\"", (_elem != null) ? ((IXmlLineInfo)_elem).LineNumber : (-1), ex);
				}
				DataItem<string> dataItem = new DataItem<string>("Website", text2);
				_entry.pWebsite = dataItem;
				if (_elem != null)
				{
					if (!_foundAttributes.ContainsKey("Website"))
					{
						_foundAttributes["Website"] = 0;
					}
					int num = _foundAttributes["Website"];
					_foundAttributes["Website"] = num + 1;
				}
			}

			// Token: 0x0400868C RID: 34444
			[PublicizedFrom(EAccessModifier.Private)]
			public static readonly Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Name",
					new Range<int>(true, 1, true, 1)
				},
				{
					"Description",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Author",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Version",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Website",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
