using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KicadAutoPlacement
{
    class KicadParser
    {
        public KicadTree Tree;
        public KicadParser(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                Tree = new KicadTree();
                Tree.Head = Parse(sr, Tree.Head).Nodes[0];
                Tree.Head.prevNode = null;
                
            }
        }

        private Node Parse(StreamReader sr, Node curNode)
        {
            while (!sr.EndOfStream)
            {
                char curSymbol = (char)sr.Read();

                switch (curSymbol)
                {
                    case '(':
                        curNode.Nodes.Add(Parse(sr,new Node(curNode)));
                        break;
                    case ')':
                        return curNode;
                    case '\n':
                        break;
                    case '\r':
                        break;
                    case '"':
                        curNode.Text += curSymbol;
                        while((curSymbol = (char)sr.Read()) != '"')
                        {
                            curNode.Text += curSymbol;
                        }
                        curNode.Text += curSymbol;
                        break;
                    default:
                        curNode.Text += curSymbol;
                        break;
                }
            }
            return curNode;
        }

        public void WriteFile(string fileName, Node head)
        {
            string fileText = "";
            using (StreamWriter file = new StreamWriter(fileName,false))
            {
                fileText = Writing(fileText, head);
                file.WriteLine(fileText);
            }
        }
        private string Writing(string text, Node curNode)
        {
            text += '(' + curNode.Text;
            foreach(Node node in curNode.Nodes)
            {
                text = Writing(text, node);
            }
            text += ")\n";
            return text;
        }
        public void WriteFromPCB(PCB pcb)
        {
            int count = 0;
            foreach (Node curModule in Tree.Head.Nodes.Where(x => x.Text.Contains("module")))
            {
                string rotate = ((pcb.Modules[count].Rotate == 0) ? "" : pcb.Modules[count].Rotate.ToString());
                int padCount = 0;
                if (curModule.Text.Split(' ').ToList()[1] == (pcb.Modules[count].Name))
                {
                    foreach(Node character in curModule.Nodes)
                    {

                        List<string> values = character.Text.Split(' ').ToList();
                        switch (values[0])
                        {
                            case "at":
                                character.Text =String.Format("at {0} {1}",
                                    pcb.Modules[count].Position.ToString(),
                                    rotate);
                                break;
                            case "fp_text":
                                List<string> textPos = character.Nodes[0].Text.Split(' ').ToList();
                                character.Nodes[0].Text = String.Format("at {0} {1} {2}",
                                    textPos[1],
                                    textPos[2],
                                    rotate);
                                break;
                            case "pad":
                                character.Nodes[0].Text = String.Format("at {0} {1}",
                                    pcb.Modules[count].Pads[padCount].Position.ToString(),
                                     rotate);
                                padCount++;
                                break;
                        }
                    }
                    
                }
                count++;
            }
        }
    }
}
