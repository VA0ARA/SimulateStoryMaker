using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace StoryMaker.Inspector
{
    public class test : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int N { get; set; }
    }
}
