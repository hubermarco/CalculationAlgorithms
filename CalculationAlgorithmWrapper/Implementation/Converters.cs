// ========================================================================
// Sivantos GmbH
// Copyright (c) 2018
// ========================================================================

using System;

namespace CalculatorAlgorithmsWrapper
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
        internal static double Fix2Double(uint fixpoint, int nBits, int nFrac)
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
    }
}
