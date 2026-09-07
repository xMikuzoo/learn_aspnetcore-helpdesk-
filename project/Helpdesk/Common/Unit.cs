namespace Helpdesk.Common;

/// <summary>Wynik operacji, ktora niczego nie zwraca; generyk nie przyjmuje void.</summary>
public readonly record struct Unit
{
    public static readonly Unit Value = new();
}
