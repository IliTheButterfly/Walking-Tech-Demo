﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(0)]
[ExecuteInEditMode]
public class MainThreadDispatcher : MonoBehaviour
{
    public static MainThreadDispatcher Instance;
    public static Thread MainThread { get; private set; }
    private static float _lastDeltaTime = 0;
    public static bool IsMainThread => Thread.CurrentThread == MainThread;
    /// <summary>
    /// <inheritdoc cref="Time.deltaTime"/>
    /// Available on secondary threads
    /// </summary>
    public static float DeltaTime => IsMainThread ? Time.deltaTime : _lastDeltaTime;

    Queue<UnityAction> actions = new Queue<UnityAction>();

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        MainThread = Thread.CurrentThread;
        Instance = this;
    }

    static void LogError()
    {
        if (Application.isPlaying) Debug.LogWarning("An object must contain MainThreadDispatcher script!");
    }

    public static void Schedule(UnityAction a)
    {
        if (Instance == null)
        {
            LogError();
            return;
        }
        if (IsMainThread) a();
        else
        {
            lock(Instance.actions)
            {
                Instance.actions.Enqueue(a);
            }
        }
    }

    public static void Schedule(UnityEvent e)
    {
        if (Instance == null)
        {
            LogError();
            return;
        }
        if (IsMainThread) e.Invoke();
        else
        {
            lock (Instance.actions)
            {
                Instance.actions.Enqueue(e.Invoke);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _lastDeltaTime = Time.deltaTime;
        lock (actions)
        {
            if (actions.Count > 0)
            {
                actions.Dequeue()();
            }
        }
    }
}

public static class ActionExtentions
{
    public static void InvokeOnMainThread(this UnityAction action)
    {
        MainThreadDispatcher.Schedule(action);
    }

    public static void InvokeOnMainThread<T0>(this UnityAction<T0> action, T0 value0)
    {
        MainThreadDispatcher.Schedule(() => action(value0));
    }

    public static void InvokeOnMainThread<T0, T1>(this UnityAction<T0, T1> action, T0 value0, T1 value1)
    {
        MainThreadDispatcher.Schedule(() => action(value0, value1));
    }

    public static void InvokeOnMainThread<T0, T1, T2>(this UnityAction<T0, T1, T2> action, T0 value0, T1 value1, T2 value2)
    {
        MainThreadDispatcher.Schedule(() => action(value0, value1, value2));
    }

    public static void InvokeOnMainThread<T0, T1, T2, T3>(this UnityAction<T0, T1, T2, T3> action, T0 value0, T1 value1, T2 value2, T3 value3)
    {
        MainThreadDispatcher.Schedule(() => action(value0, value1, value2, value3));
    }

    public static void InvokeOnMainThread(this UnityEvent action)
    {
        MainThreadDispatcher.Schedule(action.Invoke);
    }

    public static void InvokeOnMainThread<T0>(this UnityEvent<T0> action, T0 value0)
    {
        MainThreadDispatcher.Schedule(() => action.Invoke(value0));
    }

    public static void InvokeOnMainThread<T0, T1>(this UnityEvent<T0, T1> action, T0 value0, T1 value1)
    {
        MainThreadDispatcher.Schedule(() => action.Invoke(value0, value1));
    }

    public static void InvokeOnMainThread<T0, T1, T2>(this UnityEvent<T0, T1, T2> action, T0 value0, T1 value1, T2 value2)
    {
        MainThreadDispatcher.Schedule(() => action.Invoke(value0, value1, value2));
    }

    public static void InvokeOnMainThread<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> action, T0 value0, T1 value1, T2 value2, T3 value3)
    {
        MainThreadDispatcher.Schedule(() => action.Invoke(value0, value1, value2, value3));
    }
}
