using System;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.WpfDesign.XamlDom;
using UnityEngine.Scripting;
using XMLData.Exceptions;

namespace XMLData.Item
{
	// Token: 0x02001649 RID: 5705
	[Preserve]
	public class PartsData : IXMLData
	{
		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x0600B3AA RID: 45994 RVA: 0x00432981 File Offset: 0x00430B81
		// (set) Token: 0x0600B3AB RID: 45995 RVA: 0x00432989 File Offset: 0x00430B89
		public DataItem<ItemClass> Stock
		{
			get
			{
				return this.pStock;
			}
			set
			{
				this.pStock = value;
			}
		}

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x0600B3AC RID: 45996 RVA: 0x00432992 File Offset: 0x00430B92
		// (set) Token: 0x0600B3AD RID: 45997 RVA: 0x0043299A File Offset: 0x00430B9A
		public DataItem<ItemClass> Receiver
		{
			get
			{
				return this.pReceiver;
			}
			set
			{
				this.pReceiver = value;
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x0600B3AE RID: 45998 RVA: 0x004329A3 File Offset: 0x00430BA3
		// (set) Token: 0x0600B3AF RID: 45999 RVA: 0x004329AB File Offset: 0x00430BAB
		public DataItem<ItemClass> Pump
		{
			get
			{
				return this.pPump;
			}
			set
			{
				this.pPump = value;
			}
		}

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x0600B3B0 RID: 46000 RVA: 0x004329B4 File Offset: 0x00430BB4
		// (set) Token: 0x0600B3B1 RID: 46001 RVA: 0x004329BC File Offset: 0x00430BBC
		public DataItem<ItemClass> Barrel
		{
			get
			{
				return this.pBarrel;
			}
			set
			{
				this.pBarrel = value;
			}
		}

		// Token: 0x0600B3B2 RID: 46002 RVA: 0x0042865B File Offset: 0x0042685B
		public List<IDataItem> GetDisplayValues(bool _recursive = true)
		{
			return new List<IDataItem>();
		}

		// Token: 0x040086DB RID: 34523
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pStock;

		// Token: 0x040086DC RID: 34524
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pReceiver;

		// Token: 0x040086DD RID: 34525
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pPump;

		// Token: 0x040086DE RID: 34526
		[PublicizedFrom(EAccessModifier.Private)]
		public DataItem<ItemClass> pBarrel;

		// Token: 0x0200164A RID: 5706
		public static class Parser
		{
			// Token: 0x0600B3B4 RID: 46004 RVA: 0x004329C8 File Offset: 0x00430BC8
			public static PartsData Parse(PositionXmlElement _elem, Dictionary<PositionXmlElement, DataItem<ItemClass>> _updateLater)
			{
				string text = (_elem.HasAttribute("class") ? _elem.GetAttribute("class") : "PartsData");
				Type type = Type.GetType(typeof(PartsData.Parser).Namespace + "." + text);
				if (type == null)
				{
					type = Type.GetType(text);
					if (type == null)
					{
						throw new InvalidValueException("Specified class \"" + text + "\" not found", _elem.LineNumber);
					}
				}
				PartsData partsData = (PartsData)Activator.CreateInstance(type);
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (object obj in _elem.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeType nodeType = xmlNode.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.Comment)
						{
							throw new UnexpectedElementException("Unknown node \"" + xmlNode.NodeType.ToString() + "\" found while parsing Parts", ((IXmlLineInfo)xmlNode).LineNumber);
						}
					}
					else
					{
						PositionXmlElement positionXmlElement = (PositionXmlElement)xmlNode;
						if (!PartsData.Parser.knownAttributesMultiplicity.ContainsKey(positionXmlElement.Name))
						{
							throw new UnexpectedElementException("Unknown element \"" + xmlNode.Name + "\" found while parsing Parts", ((IXmlLineInfo)xmlNode).LineNumber);
						}
						string text2 = positionXmlElement.Name;
						if (!(text2 == "Stock"))
						{
							if (!(text2 == "Receiver"))
							{
								if (!(text2 == "Pump"))
								{
									if (text2 == "Barrel")
									{
										ItemClass itemClass;
										try
										{
											itemClass = null;
										}
										catch (Exception ex)
										{
											throw new InvalidValueException(string.Concat(new string[]
											{
												"Could not parse attribute \"",
												positionXmlElement.Name,
												"\" value \"",
												ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
												"\""
											}), positionXmlElement.LineNumber, ex);
										}
										DataItem<ItemClass> dataItem = new DataItem<ItemClass>("Barrel", itemClass);
										_updateLater.Add(positionXmlElement, dataItem);
										partsData.pBarrel = dataItem;
									}
								}
								else
								{
									ItemClass itemClass2;
									try
									{
										itemClass2 = null;
									}
									catch (Exception ex2)
									{
										throw new InvalidValueException(string.Concat(new string[]
										{
											"Could not parse attribute \"",
											positionXmlElement.Name,
											"\" value \"",
											ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
											"\""
										}), positionXmlElement.LineNumber, ex2);
									}
									DataItem<ItemClass> dataItem2 = new DataItem<ItemClass>("Pump", itemClass2);
									_updateLater.Add(positionXmlElement, dataItem2);
									partsData.pPump = dataItem2;
								}
							}
							else
							{
								ItemClass itemClass3;
								try
								{
									itemClass3 = null;
								}
								catch (Exception ex3)
								{
									throw new InvalidValueException(string.Concat(new string[]
									{
										"Could not parse attribute \"",
										positionXmlElement.Name,
										"\" value \"",
										ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
										"\""
									}), positionXmlElement.LineNumber, ex3);
								}
								DataItem<ItemClass> dataItem3 = new DataItem<ItemClass>("Receiver", itemClass3);
								_updateLater.Add(positionXmlElement, dataItem3);
								partsData.pReceiver = dataItem3;
							}
						}
						else
						{
							ItemClass itemClass4;
							try
							{
								itemClass4 = null;
							}
							catch (Exception ex4)
							{
								throw new InvalidValueException(string.Concat(new string[]
								{
									"Could not parse attribute \"",
									positionXmlElement.Name,
									"\" value \"",
									ParserUtils.ParseStringAttribute(positionXmlElement, "value", true, null),
									"\""
								}), positionXmlElement.LineNumber, ex4);
							}
							DataItem<ItemClass> dataItem4 = new DataItem<ItemClass>("Stock", itemClass4);
							_updateLater.Add(positionXmlElement, dataItem4);
							partsData.pStock = dataItem4;
						}
						if (!dictionary.ContainsKey(positionXmlElement.Name))
						{
							dictionary[positionXmlElement.Name] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						text2 = positionXmlElement.Name;
						int num = dictionary2[text2];
						dictionary2[text2] = num + 1;
					}
				}
				foreach (KeyValuePair<string, Range<int>> keyValuePair in PartsData.Parser.knownAttributesMultiplicity)
				{
					int num2 = (dictionary.ContainsKey(keyValuePair.Key) ? dictionary[keyValuePair.Key] : 0);
					if ((keyValuePair.Value.hasMin && num2 < keyValuePair.Value.min) || (keyValuePair.Value.hasMax && num2 > keyValuePair.Value.max))
					{
						throw new IncorrectAttributeOccurrenceException(string.Concat(new string[]
						{
							"Element has incorrect number of \"",
							keyValuePair.Key,
							"\" attribute instances, found ",
							num2.ToString(),
							", expected ",
							keyValuePair.Value.ToString()
						}), _elem.LineNumber);
					}
				}
				return partsData;
			}

			// Token: 0x040086DF RID: 34527
			[PublicizedFrom(EAccessModifier.Private)]
			public static Dictionary<string, Range<int>> knownAttributesMultiplicity = new Dictionary<string, Range<int>>
			{
				{
					"Stock",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Receiver",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Pump",
					new Range<int>(true, 0, true, 1)
				},
				{
					"Barrel",
					new Range<int>(true, 0, true, 1)
				}
			};
		}
	}
}
