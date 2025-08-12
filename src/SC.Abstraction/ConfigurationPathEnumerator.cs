using System;
using System.Collections;
using System.Collections.Generic;

namespace SC.Abstraction;

public struct ConfigurationPathEnumerator(string path, string separator) : IEnumerator<string>, IEnumerable<string>
{
    private readonly string m_Path = !string.IsNullOrWhiteSpace(path) ? path : throw new ArgumentException(null, nameof(path));
    private int m_CurrentPosition = 0;
    private int m_NextSeparator = -1;

    public string Current { get; private set; } = null;

    readonly object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if(m_CurrentPosition >= m_Path.Length)
        {
            Current = null;
            return false;
        }

        if((m_NextSeparator = m_Path.IndexOf(separator, m_CurrentPosition)) is -1) MoveLastPart();
        else MoveNextPart();

        return true;
    }

#pragma warning disable IDE0079
#pragma warning disable IDE0057

    private void MoveLastPart()
    {
        Current = m_Path.Substring(m_CurrentPosition);
        m_CurrentPosition = m_Path.Length;
    }

    private void MoveNextPart()
    {
        Current = m_Path.Substring(m_CurrentPosition, m_NextSeparator - m_CurrentPosition);
        m_CurrentPosition = m_NextSeparator + 1;
    }

#pragma warning restore IDE0057
#pragma warning restore IDE0079

    public void Reset()
    {
        m_CurrentPosition = 0;
        m_NextSeparator = -1;
        Current = null;
    }

    public readonly void Dispose() { }

    public readonly ConfigurationPathEnumerator GetEnumerator() => this;

    readonly IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}