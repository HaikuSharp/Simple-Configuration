using System;
using System.Collections;
using System.Collections.Generic;

namespace SC.Abstraction;

public struct ConfigurationPathEnumerator(string path, string separator) : IEnumerator<string>
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

    private void MoveLastPart()
    {
#if NETFRAMEWORK
        Current = m_Path.Substring(m_CurrentPosition);
#else
        Current = m_Path[m_CurrentPosition..];
#endif
        m_CurrentPosition = m_Path.Length;
    }

    private void MoveNextPart()
    {
#if NETFRAMEWORK
        Current = m_Path.Substring(m_CurrentPosition, m_NextSeparator - m_CurrentPosition);
#else
        Current = m_Path[m_CurrentPosition..m_NextSeparator];
#endif
        m_CurrentPosition = m_NextSeparator + 1;
    }

    public void Reset()
    {
        m_CurrentPosition = 0;
        m_NextSeparator = -1;
        Current = null;
    }

    public readonly void Dispose() { }
}