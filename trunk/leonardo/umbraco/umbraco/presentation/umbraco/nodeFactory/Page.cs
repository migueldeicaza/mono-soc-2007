using System;
using System.Collections;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.propertytype;

namespace umbraco.presentation.nodeFactory
{
    /// <summary>
    /// Summary description for Node.
    /// </summary>

    [Serializable]
    [XmlType(Namespace = "http://umbraco.org/webservices/")]
    public class Node
    {
        private Hashtable _aliasToNames = new Hashtable();

        private bool _initialized = false;
        private Nodes _children = new Nodes();
        private Node _parent = null;
        private int _id;
        private int _template;
        private string _name;
        private string _nodeTypeAlias;
        private string _writerName;
        private string _creatorName;
        private string _path;
        private DateTime _createDate;
        private DateTime _updateDate;
        private Guid _version;
        private Properties _properties = new Properties();
        private XmlNode _pageXmlNode;
        private int _sortOrder;

        public Nodes Children
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _children;
            }
        }

        public Node Parent
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _parent;
            }
        }

        public int Id
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _id;
            }
        }

        public int template
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _template;
            }
        }

        public int SortOrder
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _sortOrder;
            }
        }

        public string Name
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _name;
            }
        }

        public string NodeTypeAlias
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _nodeTypeAlias;
            }
        }

        public string WriterName
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _writerName;
            }
        }

        public string CreatorName
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _creatorName;
            }
        }

        public string Path
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _path;
            }
        }

        public DateTime CreateDate
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _createDate;
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _updateDate;
            }
        }

        public Guid Version
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _version;
            }
        }

        public Properties Properties
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _properties;
            }
        }

        public Node()
        {
            _pageXmlNode = ((IHasXmlNode) library.GetXmlNodeCurrent().Current).GetNode();
            initializeStructure();
            initialize();
        }

        public Node(XmlNode NodeXmlNode)
        {
            _pageXmlNode = NodeXmlNode;
            initializeStructure();
            initialize();
        }

        public Node(XmlNode NodeXmlNode, bool DisableInitializing)
        {
            _pageXmlNode = NodeXmlNode;
            initializeStructure();
            if (!DisableInitializing)
                initialize();
        }

        public Node(int NodeId)
        {
            _pageXmlNode = ((IHasXmlNode) library.GetXmlNodeById(NodeId.ToString()).Current).GetNode();
            initializeStructure();
            initialize();
        }

        public Property GetProperty(string Alias)
        {
            foreach (Property p in Properties)
            {
                if (p.Alias == Alias)
                    return p;
            }
            return null;
        }

        public DataTable ChildrenAsTable()
        {
            if (Children.Count > 0)
            {
                DataTable dt = generateDataTable(Children[0]);

                string firstNodeTypeAlias = Children[0].NodeTypeAlias;

                foreach (Node n in Children)
                {
                    if (n.NodeTypeAlias == firstNodeTypeAlias)
                    {
                        DataRow dr = dt.NewRow();
                        populateRow(ref dr, n, getPropertyHeaders(n));
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            else
                return new DataTable();
        }

        public DataTable ChildrenAsTable(string nodeTypeAliasFilter)
        {
            if (Children.Count > 0)
            {
                DataTable dt = generateDataTable(Children[0]);

                foreach (Node n in Children)
                {
                    if (n.NodeTypeAlias == nodeTypeAliasFilter)
                    {
                        DataRow dr = dt.NewRow();
                        populateRow(ref dr, n, getPropertyHeaders(n));
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            else
                return new DataTable();
        }

        private DataTable generateDataTable(Node SchemaNode)
        {
            DataTable NodeAsDataTable = new DataTable(SchemaNode.NodeTypeAlias);
            string[] defaultColumns = {
                                          "Id", "NodeName", "NodeTypeAlias", "CreateDate", "UpdateDate", "CreatorName",
                                          "WriterName"
                                      };
            foreach (string s in defaultColumns)
            {
                DataColumn dc = new DataColumn(s);
                NodeAsDataTable.Columns.Add(dc);
            }

            // add properties
            Hashtable propertyHeaders = getPropertyHeaders(SchemaNode);
            IDictionaryEnumerator ide = propertyHeaders.GetEnumerator();
            while (ide.MoveNext())
            {
                DataColumn dc = new DataColumn(ide.Value.ToString());
                NodeAsDataTable.Columns.Add(dc);
            }

            return NodeAsDataTable;
        }

        private Hashtable getPropertyHeaders(Node SchemaNode)
        {
            if (_aliasToNames.ContainsKey(SchemaNode.NodeTypeAlias))
                return (Hashtable)_aliasToNames[SchemaNode.NodeTypeAlias];
            else
            {
                ContentType ct = ContentType.GetByAlias(SchemaNode.NodeTypeAlias);
                Hashtable def = new Hashtable();
                foreach (PropertyType pt in ct.PropertyTypes)
                    def.Add(pt.Alias, pt.Name);
                System.Web.HttpContext.Current.Application.Lock();
                _aliasToNames.Add(SchemaNode.NodeTypeAlias, def);
                System.Web.HttpContext.Current.Application.UnLock();

                return def;
            }
        }

        private void populateRow(ref DataRow dr, Node n, Hashtable AliasesToNames)
        {
            dr["Id"] = n.Id;
            dr["NodeName"] = n.Name;
            dr["NodeTypeAlias"] = n.NodeTypeAlias;
            dr["CreateDate"] = n.CreateDate;
            dr["UpdateDate"] = n.UpdateDate;
            dr["CreatorName"] = n.CreatorName;
            dr["WriterName"] = n.WriterName;

            int counter = 7;
            foreach (Property p in n.Properties)
            {
                if (p.Value != null)
                {
                    dr[AliasesToNames[p.Alias].ToString()] = p.Value;
                    counter++;
                }
            }
        }


        private void initializeStructure()
        {
            // Load parent if it exists and is a node
            if (_pageXmlNode.SelectSingleNode("..") != null)
                if (_pageXmlNode.SelectSingleNode("..").Name == "node")
                    _parent = new Node(_pageXmlNode.SelectSingleNode(".."), true);
        }

        private void initialize()
        {
            if (_pageXmlNode != null)
            {
                _initialized = true;
                if (_pageXmlNode.Attributes != null)
                {
                    _id = int.Parse(_pageXmlNode.Attributes.GetNamedItem("id").Value);
                    _template = int.Parse(_pageXmlNode.Attributes.GetNamedItem("template").Value);
                    _sortOrder = int.Parse(_pageXmlNode.Attributes.GetNamedItem("sortOrder").Value);
                    _name = _pageXmlNode.Attributes.GetNamedItem("nodeName").Value;
                    _writerName = _pageXmlNode.Attributes.GetNamedItem("writerName").Value;
                    // Creatorname is new in 2.1, so published xml might not have it!
                    try
                    {
                        _creatorName = _pageXmlNode.Attributes.GetNamedItem("creatorName").Value;
                    }
                    catch
                    {
                        _creatorName = _writerName;
                    }
                    _nodeTypeAlias = _pageXmlNode.Attributes.GetNamedItem("nodeTypeAlias").Value;
                    _path = _pageXmlNode.Attributes.GetNamedItem("path").Value;
                    _version = new Guid(_pageXmlNode.Attributes.GetNamedItem("version").Value);
                    _createDate = DateTime.Parse(_pageXmlNode.Attributes.GetNamedItem("createDate").Value);
                    _updateDate = DateTime.Parse(_pageXmlNode.Attributes.GetNamedItem("updateDate").Value);
                }

                // load data
                foreach (XmlNode n in _pageXmlNode.SelectNodes("./data"))
                    _properties.Add(new Property(n));

                // load children
                XPathNavigator nav = _pageXmlNode.CreateNavigator();
                XPathExpression expr = nav.Compile("./node");
                expr.AddSort("@sortOrder", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);
                XPathNodeIterator iterator = nav.Select(expr);
                while (iterator.MoveNext())
                {
                    _children.Add(
                        new Node(((IHasXmlNode) iterator.Current).GetNode(), true)
                        );
                }
            }
            else
                throw new ArgumentNullException("Node xml source is null");
        }

        public static Node GetCurrent()
        {
            XmlNode n = ((IHasXmlNode) library.GetXmlNodeCurrent().Current).GetNode();
            return new Node(int.Parse(n.Attributes.GetNamedItem("id").Value));
        }
    }

    public class Nodes : CollectionBase
    {
        public virtual void Add(Node NewNode)
        {
            List.Add(NewNode);
        }

        public virtual Node this[int Index]
        {
            get { return (Node) List[Index]; }
        }
    }

    [Serializable]
    [XmlType(Namespace = "http://umbraco.org/webservices/")]
    public class Property
    {
        private Guid _version;
        private string _alias;
        private string _value;

        public string Alias
        {
            get { return _alias; }
        }

        public string Value
        {
            get { return _value; }
        }

        public Guid Version
        {
            get { return _version; }
        }

        public Property()
        {
            
        }

        public Property(XmlNode PropertyXmlData)
        {
            if (PropertyXmlData != null)
            {
                // For backward compatibility with 2.x (the version attribute has been removed from 3.0 data nodes)
                if (PropertyXmlData.Attributes.GetNamedItem("versionID") != null)
                    _version = new Guid(PropertyXmlData.Attributes.GetNamedItem("versionID").Value);
                _alias = PropertyXmlData.Attributes.GetNamedItem("alias").Value;
                _value = xmlHelper.GetNodeValue(PropertyXmlData);
            }
            else
                throw new ArgumentNullException("Property xml source is null");
        }
    }

    public class Properties : CollectionBase
    {
        public virtual void Add(Property NewProperty)
        {
            List.Add(NewProperty);
        }

        public virtual Property this[int Index]
        {
            get { return (Property) List[Index]; }
        }
    }

 
}