using UnityEngine;
using System;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

/*inspired by "C# in Depth" by Jon Skeet on http://stackoverflow.com/questions/569903/multi-value-dictionary/569920#569920

Utility class implementing a tuple/pair, in this project primarily for use with the modules dictionary in ModuleManager 
-> goal is the ability to encode both the primary moduleID as well as the dynamically generated module subID as one key.*/

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
