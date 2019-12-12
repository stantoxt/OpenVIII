﻿using System;
using System.Diagnostics;

namespace OpenVIII
{
    public class Slide<T>
    {
        #region Constructors

        public Slide(T start, T end, TimeSpan totalTime, Func<T, T, float, T> function)
        {
            Start = start;
            End = end;
            TotalTime = totalTime;
            Debug.Assert(TotalTime != TimeSpan.Zero);
            Function = function;
        }

        #endregion Constructors

        #region Properties

        public T Current { get; private set; }
        public TimeSpan CurrentTime { get; private set; }
        public float CurrentPercent { get; private set; }

        /// <summary>
        /// When started wait this many MS before moving.
        /// </summary>
        public TimeSpan Delay { get; set; }

        public bool Done => CurrentTime - Delay >= TotalTime;

        public T End { get; set; }
        public Func<T, T, float, T> Function { get; set; }
        public bool Reversed { get; private set; } = false;
        public TimeSpan ReversedTime { get; set; }
        public T Start { get; set; }
        public TimeSpan TotalTime { get; set; }

        #endregion Properties

        #region Methods

        public void Restart() => CurrentTime = TimeSpan.Zero;

        public void Reverse()
        {
            T tempValue = Start;
            Start = End;
            End = tempValue;
            if (ReversedTime > TimeSpan.Zero)
            {
                TimeSpan tempTime = ReversedTime;
                ReversedTime = TotalTime;
                TotalTime = tempTime;
            }
            Reversed = !Reversed;
        }

        public void ReverseRestart()
        {
            Reverse();
            Restart();
        }

        public T Update()
        {
            if (!Done && Function != null)
            {
                UpdatePercent();
                Current = Function(Start, End, CurrentPercent);
                return Current;
            }
            return End;
        }

        public float UpdatePercent()
        {
            CurrentPercent = 1f;
            if (!Done)
            {
                CurrentTime += Memory.gameTime.ElapsedGameTime;
                return CurrentPercent = CurrentTime < Delay ? 0f : (float)(Done ? 1f : (CurrentTime - Delay).TotalMilliseconds / TotalTime.TotalMilliseconds);
            }
            else
                return CurrentPercent;
        }

        #endregion Methods
    }
}