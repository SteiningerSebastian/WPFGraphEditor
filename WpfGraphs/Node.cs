using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfGraphs
{
    [Serializable]
    public class Node : INotifyPropertyChanged
    {
        public Node() { }
        private static Random random = new Random();
        private static uint _id = 0;
        private static uint _Id
        {
            get
            {
                _id++;
                return _id - 1;
            }
            set
            {
                _id = value;
            }
        }

        public static void ResetId()
        {
            _Id = 0;
        }

        public uint Id { get; set; } = Node._Id;

        public event PropertyChangedEventHandler? PropertyChanged;

        #region graphics
        private double _x = random.NextDouble()*750;
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                PropertyChange();
                PropertyChange(nameof(DisplayMargin));
            }
        }

        private double _y = random.NextDouble()*400;
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                PropertyChange();
                PropertyChange(nameof(DisplayMargin));
            }
        }

        public Thickness DisplayMargin
        {
            get => new Thickness(X - ViewModelMain.NodeSize/2, Y - ViewModelMain.NodeSize/2, 0, 0);
        }
        #endregion

        public void PropertyChange([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
