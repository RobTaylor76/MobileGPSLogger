using System;
using System.Text;
using System.Xml;

namespace xnatest
{
    //small struct for holding the xml attributes name and value
    public struct attr
    {
        public String attrName;
        public String attrVal;
    }

    #region delegates
    /*
     * Delegates for callback's for elements
     * used for reciving notifications/callbacks on the xml parsing.
     */
    public delegate void OnElementStartD(String name, String ns, Int32 numAttribs, Array attribs);
    public delegate void OnElementEndD(String name);
    public delegate void OnElementDataD(String name, String CData);
    #endregion

    class XmlParser
    {
        public XmlParser()
        {
            reader = null;
            fileName = "";
        }

        //returns 1 on success, 0 on failed
        public int Load(String filename)
        {
            fileName = filename;
            reader = new XmlTextReader(fileName);
            if (reader == null) return 0; //failed
            return 1;
        }

        //returns 1 on success, 0 on failed
        public int Parse()
        {
            if (reader == null) return 0;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                		string strElementName = reader.Name;
                		string strNamespaceURI = reader.NamespaceURI;
                        attr[] attrs = new attr[reader.AttributeCount];
                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                attr na = new attr();
                                na.attrName = reader.Name;
                                na.attrVal = reader.Value;
                                attrs[i] = na;
                            }
                        }
                        if (OnElementStart != null )
                        {
                        	OnElementStart(strElementName, strNamespaceURI, reader.AttributeCount, attrs);
                        }
                        break;

                    case XmlNodeType.EndElement:
                        if (OnElementEnd != null)
                        {
                        	OnElementEnd(reader.Name);
                        }
                        break;

                    case XmlNodeType.Text:
                        if (OnElementData != null)
                        {
                        	OnElementData(reader.Name, reader.Value);
                        }
                        break;
                }
            }
            return 1;
        }

        #region events
        /*
         * Event declerations for the delegates for the parser callbacks
         */
        public event OnElementStartD OnElementStart;
        public event OnElementEndD OnElementEnd;
        public event OnElementDataD OnElementData;
        #endregion

        #region privates
        //private data
        private String fileName;
        private XmlTextReader reader;
        #endregion
    }
}
