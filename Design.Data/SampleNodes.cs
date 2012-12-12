/*
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Smart.Graphs;
using Smart.Graphs.VisualNodes;
using Smart.Classes.Bindables;
using Smart.Classes.Extensions;
using Smart.TestExtensions;


namespace DesignData
{
    public class SampleNodes:Bindable
    {
        private int _linksPerNode = 7;

        public int LinksPerNode
        {
            get { return _linksPerNode; }
            set { _linksPerNode = value;

                RaisePropertyChanged("LinkePerNode");
            }
        }

        public GraphNodesGenerator NodesGenerator { get; set; }
        public IGraphViewGenerator ViewGenerator { get; set; }


        private int _numNodes = 55;
        private VisualNode _root;

        public int NumNodes
          {
              get { return _numNodes; }
              set {
                  _numNodes = value;
                  RaisePropertyChanged("NumNodes");
              }
          }

    

    

        public virtual List<VisualNode> Generate()
        {
            var list = new List<VisualNode>();
            var nodes = new Collection<VisualNode> { this.NodesGenerator.GenerateRootNode() };
            for (var i = 1; i < NumNodes; i++) nodes.Add(this.NodesGenerator.GenerateNode(i));
            var root = nodes[0];
            while (nodes.Count > 0)
            {
                var node = nodes[0];
                //root.Properties.ContainsKey("Name").ShouldBeTrue();
                for (var i = 0; i < LinksPerNode; i++)
                {
                    var othernode = nodes.GetRandom(n => n != node && !n.HasConnection(node));
                    if (othernode == null) continue;
                    var edge = this.NodesGenerator.Connect(node, othernode);
                    //temp
                    edge.Properties.ContainsKey("text").ShouldBeTrue();
                    othernode.Properties.ContainsKey("text").ShouldBeTrue();
                    //temp
                    if (othernode.Count >= LinksPerNode) nodes.Remove(othernode);
                }
                list.Add(nodes.Shift());
            }

            Root = root;
            return list;
        }
        public VisualNode Root
        {
            get { return _root; }
            set { 
                _root = value;
                RaisePropertyChanged("NumNodes");
            }
        }

        public SampleNodes()
        {
            this.NodesGenerator = new GraphNodesGenerator();
            this.ViewGenerator = new GraphViewGenerator();
            this.Generate();
            this.Root.ItemsAdded.DoOnNext += this.OnEdgeAdded;
            

        }

        public void OnEdgeAdded(VisualEdge edge)
        {
            if(edge.View!=null) return;
            edge.View = this.ViewGenerator.MakeEdgeView(edge);
            edge.ItemsAdded.DoOnNext += OnNodeAdded;
        }

        public void OnNodeAdded(VisualNode node)
        {
            if(node.View==null) return;
            node.ItemsAdded.DoOnNext += OnEdgeAdded;
        }


    }
}
*/