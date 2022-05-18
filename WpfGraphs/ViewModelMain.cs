using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfGraphs
{
    public enum Tool
    {
        Node,
        Arrow,
        Arrow2
    }

    public class ViewModelMain : INotifyPropertyChanged
    {
        private static string FilePath = "./my.graph";
        public ViewModelMain()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                if(json != null && json != String.Empty)
                    MainGraph = JsonConvert.DeserializeObject<Graph>(json) ?? throw new NullReferenceException("Could not create the object!");
            }
            GraphComputeHandler.Instance.OnStart();
        }

        public static double NodeSize { get; set; } = 45;

        private Tool _toolSelected = Tool.Node;
        public Tool ToolSelected
        {
            get
            {
                return _toolSelected;
            }
            set
            {
                _toolSelected = value;
                PropertyChange();
            }
        }

        private double _mouseX = 999;

        public double MouseX
        {
            get { return _mouseX; }
            set
            {
                _mouseX = value;
                PropertyChange();
            }
        }

        private double _mouseY = 999;

        public double MouseY
        {
            get { return _mouseY; }
            set
            {
                _mouseY = value;
                PropertyChange();
            }
        }

        public Graph MainGraph { get; set; } = new Graph();

        public ObservableCollection<UIElement> UIElements { get; set; } = new ObservableCollection<UIElement>() { };

        public event PropertyChangedEventHandler? PropertyChanged;

        public Node? FirstNodeSelected { get; set; }
        public Node? SecondNodeSelected { get; set; }

        #region Commands

        private ICommand _selectNodeCommand;

        public ICommand SelectNodeCommand
        {
            get
            {
                return _selectNodeCommand ?? (_selectNodeCommand = new DelegateCommand((o) => true, (o) => { ToolSelected = Tool.Node; SelectNodeOnCanvasCommand.ChanExecuteChange(); }));
            }
        }

        private ICommand _selectArrowCommand;

        public ICommand SelectArrowCommand
        {
            get
            {
                return _selectArrowCommand ?? (_selectArrowCommand = new DelegateCommand((o) => true, (o) => { ToolSelected = Tool.Arrow; SelectNodeOnCanvasCommand.ChanExecuteChange(); }));
            }
        }

        private ICommand _selectArrow2Command;

        public ICommand SelectArrow2Command
        {
            get
            {
                return _selectArrow2Command ?? (_selectArrow2Command = new DelegateCommand((o) => true, (o) => { ToolSelected = Tool.Arrow2; SelectNodeOnCanvasCommand.ChanExecuteChange(); }));
            }
        }

        private DelegateCommand _selectNodeOnCanvasCommand;

        public DelegateCommand SelectNodeOnCanvasCommand
        {
            get
            {
                return _selectNodeOnCanvasCommand ?? (_selectNodeOnCanvasCommand = new DelegateCommand((o) =>
                {
                    return ToolSelected != Tool.Node;
                }, (o) =>
            {
                NodeClicked((uint)o);
            }));
            }
        }

        private DelegateCommand _clearCanvasCommand;

        public DelegateCommand ClearCanvasCommand
        {
            get
            {
                return _clearCanvasCommand ?? (_clearCanvasCommand = new DelegateCommand((o) =>
                {
                    return true;
                }, (o) =>
                {
                    MainGraph.Edges.Clear();
                    MainGraph.Nodes.Clear();
                    Edge.ResetId();
                    Node.ResetId();
                    PropertyChange(nameof(MainGraph));
                }));
            }
        }

        private DelegateCommand _startComputeCommand;

        public DelegateCommand StartComputeCommand
        {
            get
            {
                return _startComputeCommand ?? (_startComputeCommand = new DelegateCommand((o) =>
                {
                    return true;
                }, (o) =>
                {
                    GraphComputeHandler.Instance.Compute(MainGraph);
                }));
            }
        }

        private DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new DelegateCommand((o) =>
                {
                    return true;
                }, (o) =>
                {
                    string json = JsonConvert.SerializeObject(MainGraph);
                    File.WriteAllText(FilePath, json);
                }));
            }
        }

        #endregion

        public void NodeClicked(uint Id)
        {
            if (FirstNodeSelected == null || SecondNodeSelected == FirstNodeSelected || FirstNodeSelected.Id == Id)
            {
                FirstNodeSelected = MainGraph.GetNodeById(Id);
                SecondNodeSelected = null;
            }
            else if (FirstNodeSelected != null && SecondNodeSelected == null )
            {
                SecondNodeSelected = MainGraph.GetNodeById(Id);
            }
            else
            {
                SecondNodeSelected = null;
                FirstNodeSelected = MainGraph.GetNodeById(Id);
            }
            if(SecondNodeSelected != null && FirstNodeSelected != null && SecondNodeSelected != FirstNodeSelected)
            {
                MakeEdge(ToolSelected==Tool.Arrow?true:false);
            }
        }

        public void MakeEdge(bool isDirectional)
        {
            MainGraph.AddEdge(FirstNodeSelected??throw new NullReferenceException(), SecondNodeSelected ?? throw new NullReferenceException(), isDirectional);
        }

        public void CanvasClicked()
        {
            switch (ToolSelected)
            {
                case Tool.Node:
                    Node n = MainGraph.AddNode();
                    n.X = MouseX;
                    n.Y = MouseY;
                    PropertyChange(nameof(MainGraph));
                    break;
                case Tool.Arrow:
                    break;
                case Tool.Arrow2:
                    break;
            }
        }

        public void PropertyChange([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
