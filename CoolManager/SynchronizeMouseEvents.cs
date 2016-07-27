using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;

namespace CoolManager
{
    class SynchronizeMouseEvents : INotifyPropertyChanged
    {
        private IRange sharedXVisibleRange;

        public IRange SharedXVisibleRange
        {
            get { return sharedXVisibleRange; }
            set
            {
                if (sharedXVisibleRange == value) return;
                sharedXVisibleRange = value;
                OnPropertyChanged("SharedXVisibleRange");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
