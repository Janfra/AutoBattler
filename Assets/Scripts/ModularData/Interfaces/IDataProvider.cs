using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataProvider<T>
{
    public T OnProvideData();
}