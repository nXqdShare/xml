using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;


namespace XML
{
    class Program
    {
        static void XmlNative()
        {
            // create xmldoc
            var doc = new XmlDocument();
            #region create
            {
                var root = doc.CreateElement("school");
                var node1 = doc.CreateElement("student");
                node1.InnerText = "Slim Shady";
                node1.SetAttribute("age", "12");
                node1.SetAttribute("class", "A1");
                root.AppendChild(node1);

                var node2 = doc.CreateElement("student");
                node2.InnerText = "Britney Spear";
                node2.SetAttribute("age", "1");
                node2.SetAttribute("class", "B1");
                root.AppendChild(node2);

                doc.AppendChild(root);
                doc.Save(Console.Out);
                Console.WriteLine("\n");
            }
            #endregion

            #region manipulate
            // select single node
            {
                var select = doc.SelectSingleNode("//student");
                Console.WriteLine(select.OuterXml);
            }
            // select multiple nodes
            {
                var selects = doc.SelectNodes("//student");
                foreach (XmlNode select in selects)
                {
                    Console.WriteLine(select.OuterXml);
                }
            }
            // change tag name
            {
                var selects = doc.SelectNodes("//student");
                foreach(var select in selects)
                {
                    var node = (XmlNode) select;
                    if (node.InnerText != "Slim Shady") continue;
                    XmlNode newChild = doc.CreateElement("teacher");
                    while (node.Attributes.Count > 0)
                    {
                        newChild.Attributes.Append(node.Attributes[0]);
                    }
                    newChild.InnerText = node.InnerText;
                    node.ParentNode.ReplaceChild(newChild,node);
                }
                Console.WriteLine("Change Slim Shady tag name : student > teacher");
                Console.WriteLine(doc.OuterXml);
            }
            // remove 
            {
                var remove = doc.SelectSingleNode("//student[@class='B1']");
                remove.ParentNode.RemoveChild(remove);
                Console.WriteLine("Remove node has class attr being B1");
                Console.WriteLine(doc.OuterXml);
            }
            // insert 
            {
                var refNode = doc.SelectSingleNode("//teacher[@class='A1']");
                var insert = doc.CreateElement("student");
                insert.InnerText = "new student";
                insert.SetAttribute("age","100");
                insert.SetAttribute("class","C1");
                if (refNode.ParentNode != null) refNode.ParentNode.InsertBefore(insert, refNode);
                Console.WriteLine(doc.OuterXml);
            }
            #endregion
        }
        static void XmlLinq()

        {
            // create xmldoc
            var doc = new XDocument();

            doc.Add(
                new XElement("school",
                    new XElement("student", "Slim Shady",
                        new XAttribute("age", "12"),
                        new XAttribute("class", "A1")),
                       new XElement("student", "Britney Spear",
                        new XAttribute("age", "1"),
                        new XAttribute("class", "B1")))
                );
            Console.WriteLine(doc.Document);
            // select single node
            {
                var name = "Britney Spear";
                foreach (var node in doc.Descendants("student") )
                {
                    Console.WriteLine(node.ToString());
                }
            }
            // change tag name
            {
                var name = "Slim Shady";
                var shady = (from node in doc.Descendants("student")
                             where node.Value == name
                             select node).FirstOrDefault();
                shady.Name = "teacher";
                Console.WriteLine(doc.Document);
            }
            // remove
            {
                var Class = "B1";
                var remove = (from node in doc.Descendants("student")
                              where node.Attribute("class").Value == Class
                              select node).FirstOrDefault();
                remove.Remove();
                Console.WriteLine(doc.Document);
            }
            // insert
            {
                doc.Element("school").Add(new XElement("student",
                    new XAttribute("age","21"),
                    new XAttribute("class","home")) {Value = "nXqd"});

                Console.WriteLine("Insert new element to xml doc");
                Console.WriteLine(doc);
            }
            // insert before
            {
                doc.Element("school").Elements("student").Where(e => (string)e.Attribute("age") == "21").Single<XElement>().AddBeforeSelf(
                    new XElement("student",
                        new XAttribute("age","-1"),
                        new XAttribute("class","abc")) {Value = "InsertBefore" }
                    );
                Console.WriteLine("insert before nXqd node");
                Console.WriteLine(doc);
            }

        }
        static void XSLT()
        {
            var xsl = new XslTransform();
            xsl.Load("test.xslt");
            xsl.Transform("test.xml", "test.html");
        }
        static void Main(string[] args)
        {
            //XmlLinq();
            //XmlNative();
            XSLT();
        }
    }
}
