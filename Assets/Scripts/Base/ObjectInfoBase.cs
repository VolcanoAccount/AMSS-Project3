using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectInfoBase : ISerializationCallbackReceiver
{
     //反序列化后调用
    public virtual void OnAfterDeserialize()
    {

    }

    //序列化前调用
    public virtual void OnBeforeSerialize()
    {
        
    }
}
