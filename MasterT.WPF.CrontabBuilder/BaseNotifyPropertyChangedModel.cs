using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MasterT.WPF.CrontabBuilder
{
    public class BaseNotifyPropertyChangedModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// Event PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// This raise event 'PropertyChanged'
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a field and raise the PropertyChange event if the field value was modified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
