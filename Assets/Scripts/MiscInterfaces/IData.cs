using UnityEngine;

// test interface to support game saving
public interface IData<T> 
{

    public void SetData(T data);
    public void UpdateData(T behaviour);
}
