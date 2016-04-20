using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class Pair<First, Second> : IEquatable<Pair<First,Second>> {

    public readonly First first;
    public readonly Second second;

    public Pair(First fst, Second snd)
    {
        first = fst;
        second = snd;
    }

    public First Fst
    {
        get { return first; }
    }

    public Second Snd
    {
        get { return second; }
    }

    public bool Equals(Pair<First, Second> other)
    {
        if(other == null) { return false; }
        return EqualityComparer<First>.Default.Equals(Fst, other.Fst) &&
                EqualityComparer<Second>.Default.Equals(Snd, other.Snd);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Pair<First, Second>);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<First>.Default.GetHashCode(first) * 37 +
                EqualityComparer<Second>.Default.GetHashCode(second);
    }
}
