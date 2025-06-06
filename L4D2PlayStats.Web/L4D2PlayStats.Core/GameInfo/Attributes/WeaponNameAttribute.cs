﻿namespace L4D2PlayStats.Core.GameInfo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class WeaponNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}