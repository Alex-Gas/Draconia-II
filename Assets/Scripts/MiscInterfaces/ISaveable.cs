using System;
using UnityEngine;

// test interface to support game saving
public interface ISaveable<T>
{
    // request saveable class to provide an object that describes it and can be serialized
    public T Save();

    // load an object into a scene using a de-serialized object
    public void Load(T data);

}
