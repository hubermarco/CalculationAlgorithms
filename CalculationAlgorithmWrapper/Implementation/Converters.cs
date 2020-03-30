﻿// ========================================================================
// Sivantos GmbH
// Copyright (c) 2018
// ========================================================================

using System;

namespace CalculationAlgorithmWrapper
{
    internal class Converters
    {
        // Inputs:
        // fixpoint         Fixed - point bits to be converted  in decimal representation

        // Convert fixed-point number to double representation
        //
        // function val_double = Fix2Double(fixpoint, nBits, nFrac)
        //
        // nBits            Overall number of bits in the fixed point format
        // nFrac            Number of fractional bits(default: n_bits - 1)
        //
        // Outputs:
        // val_double       Resulting double value
        //
        // Remarks:
        // No range checking is done on the input arguments.
        // Increase n_bits by one if you need to handle unsigned numbers
        //
        // Examples:
        // double_1 = fix2double(27, 5, 2) // Treat as signed fix<5,2> -> -1.25
        // double_2 = fix2double(27, 6, 0) // Treat as unsigned integer ufix<5, 0> -> 27
        internal static double Fix2Double(ulong fixpoint, int nBits, int nFrac)
        {
            var shiftVal1 = -Math.Pow(2.0, nBits - nFrac - 1);
            var shiftVal2 = Math.Pow(2.0, -nFrac);

            var shift = nBits - 1;
            var mask = (ulong)(1 << shift);

            ulong fixDec = fixpoint;
            double valVz = (fixDec & mask) >> shift;
            fixDec &= ~mask;

            return valVz * shiftVal1 + fixDec * shiftVal2;
        }

        internal static ulong Double2Fix(double value, int nBits, int nFrac)
        {
            ulong registerValue = 0;

            var absValue = Math.Abs(value);

            for (var i = 0; i < nBits; i++)
            {
                var divisor = Math.Pow(2.0, nBits-i-nFrac-1);
                var result = (int)(absValue / divisor);

                if (result == 1)
                {
                    var mask = (ulong)(1 << nBits-i-1);
                    registerValue |= mask;
                    absValue -= divisor;
                }
            }

            if(value < 0)
            {
                // invert bits (2 complement)
                for (var i = 0; i < nBits; i++)
                {
                    var mask = (ulong)(1 << i);

                    if( (registerValue & mask) == 0)
                    {
                        registerValue += (ulong)Math.Pow(2,i);
                    }
                    else
                    {
                        registerValue -= (ulong)Math.Pow(2, i);
                    }
                }

                registerValue += 1;
            }
            
            return registerValue;
        }

        internal static ulong Fix2Bool(ulong registerValue, int nBits)
        {
            ulong boolValue = 0;

            // invert bits (2 complement)
            for (var i = 0; i < nBits; i++)
            {
                var mask = (ulong)(1 << i);

                if ((registerValue & mask) > 0)
                {
                    boolValue += (ulong)Math.Pow(10, i);
                }
            }

            return boolValue;
        }

        internal static ulong Bool2Fix(ulong boolValue)
        {
            ulong fixValue = 0;
            var stringValue = boolValue.ToString();
            var nBits = stringValue.Length;

            for (var i = nBits - 1; i >= 0; i--)
            {
                var divisor = Math.Pow(10.0, i);
                var result = (int)(boolValue / divisor);

                if (result == 1)
                {
                    fixValue += (ulong)Math.Pow(2, i);
                    boolValue -= (ulong)divisor;
                }
            }

            return fixValue;
        }

        internal static ulong Double2Bool(double value, int nBits, int nFrac)
        {
            var registerValue = Double2Fix(value, nBits, nFrac);

            var boolValue = Fix2Bool(registerValue, nBits);

            return boolValue;
        }

        internal static double Bool2Double(ulong boolValue, int nBits, int nFrac)
        {
            var fixValue = Bool2Fix(boolValue);

            var doubleValue = Fix2Double(fixValue, nBits, nFrac);

            return doubleValue;
        }
    }
}
