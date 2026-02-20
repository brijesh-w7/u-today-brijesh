using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class MonoParent : MonoBehaviour
{

    public string ClassName
    {
        get => GetType().Name;
    }

    public static string MethodName
    {

        get => new StackTrace().GetFrame(1).GetMethod().Name;
    }

    public string ClassMethodName
    {

        get => GetType().Name + " " + new StackTrace().GetFrame(1).GetMethod().Name + "()";
    }


    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetScale(Vector3 mScale)
    {
        transform.localScale = mScale;
    }

    public void SetLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public void SetActive(bool val)
    {
        this.gameObject.SetActive(val);
    }


    protected void CallAfterSeconds(float seconds, Action callback) => StartCoroutine(RoutineCallAfterOneSeconds(seconds, callback));

    private IEnumerator RoutineCallAfterOneSeconds(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}
