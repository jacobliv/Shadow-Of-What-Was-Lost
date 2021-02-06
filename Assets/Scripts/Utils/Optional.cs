﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optional<T>
{
    public bool HasValue { get; private set; }
    private T value;
    public T Value
    {
        get
        {
            if (HasValue)
                return value;
            else
                throw new InvalidOperationException();
        }
    }

    public Optional(T value)
    {
        this.value = value;
        HasValue = true;
    }

    public Optional() {
        HasValue = false;
    }
    
    public static Optional<T> of(T obj) {
        return new Optional<T>(obj);
    }

    public static Optional<T> empty() {
        return new Optional<T>();
    }

    public static explicit operator T(Optional<T> optional)
    {
        return optional.Value;
    }
    public static implicit operator Optional<T>(T value)
    {
        return new Optional<T>(value);
    }

    public override bool Equals(object obj)
    {
        if (obj is Optional<T>)
            return this.Equals((Optional<T>)obj);
        else
            return false;
    }
    public bool Equals(Optional<T> other)
    {
        if (HasValue && other.HasValue)
            return object.Equals(value, other.value);
        else
            return HasValue == other.HasValue;
    }
}