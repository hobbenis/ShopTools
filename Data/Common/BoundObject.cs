using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShopTools.Common;

public class BoundObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Dictionary<string, object?> myFields;

    protected void Set(object value, [CallerMemberName] string propName = "")
    {
        if (myFields is null)
        {
            myFields = new();
        }
            
        if (myFields.ContainsKey(propName) && myFields[propName].Equals(value))
        {
            return;
        }

        myFields[propName] = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    /*protected TGet Get<TGet>([CallerMemberName] string propName = "")
    {
        if (myFields is null)
        {
            myFields = new();
            return null;
        }

        if (!myFields.ContainsKey(propName)
            || myFields[propName] is null)
        {
            return null;
        }
            
        return (TGet)myFields[propName];
    }*/
}