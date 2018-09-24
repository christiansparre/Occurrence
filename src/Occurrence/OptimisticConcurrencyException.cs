using System;

namespace Occurrence
{
    public class OptimisticConcurrencyException : Exception
    {
        public int ExpectedVersion { get; }
        public int CurrentVersion { get; }

        public OptimisticConcurrencyException(int expectedVersion, int currentVersion) : base($"Attempted to append events based on an expected stream verson of {expectedVersion}, but the current stream version is {currentVersion}")
        {
            ExpectedVersion = expectedVersion;
            CurrentVersion = currentVersion;
        }
    }
}