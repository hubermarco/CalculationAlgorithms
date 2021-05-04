// ========================================================================
// Sivantos GmbH
// Copyright (c) 2018
// ========================================================================

using System;

namespace CalculationAlgorithmWrapper
{
    internal class Converters
    {
        // Convert fixed-point number to double representation
        //
        // function val_double = Fix2Double(fixpoint, nBits, nFrac)
        //
        // Inputs:
        // fixpoint         Fixed - point bits to be converted  in decimal representation
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

        // Convert double number to fixed-point representation
        //
        // function fix_dec = double2fix(val_double, n_bits, n_frac, mode)
        //
        // Inputs:
        // val_double      Fixed - point bits in decimal representation
        // n_bits          Overall number of bits in the fixed point format
        // n_frac          Number of fractional bits(default: n_bits - 1)
        // mode            Rounding operation mode
        // 0 = floor as IC default,
        // 1 = round mathamtical correct
        // 2 = round half up. (i.e.add 1 to lsb - 1 and
        // floor)
        // Outputs:
        // fix_dec         double value of fixed point number
        //
        // Remarks:
        // No range checking is done on the input arguments
        // but a warning is issued if clipping occurs
        // Increase n_bits by one if you need to handle unsigned numbers
        //
        // Examples:
        // fix_1 = double2fix(-1.25, 5, 2) % Treat as signed fix < 5, 2 >
        // fix_2 = double2fix(27, 6, 0) % Treat as unsigned integer ufix < 5, 0 >
        //
        // Comparison rounding / floor
        // num2strexact(double2fix([-0 - 0 - 2.^ -[23 24 25 26]], 32, 23, 0))
        // ans =
        // '0'    '4.294967295e9'    '4.294967295e9'    '4.294967295e9'    '4.294967295e9'
        //
        // num2strexact(double2fix([-0 - 0 - 2.^ -[23 24 25 26]], 32, 23, 1))
        // ans =
        // '0'    '4.294967295e9'    '4.294967295e9'    '0'    '0'
        //
        //
        // num2strexact(double2fix([-1 - 1 - 2.^ -[23 24 25 26]], 32, 23, 0))
        // ans =
        // '4.286578688e9'    '4.286578687e9'    '4.286578687e9'    '4.286578687e9'    '4.286578687e9'
        //
        // num2strexact(double2fix([-1 - 1 - 2.^ -[23 24 25 26]], 32, 23, 1))
        // ans =
        // '4.286578688e9'    '4.286578687e9'    '4.286578687e9'    '4.286578688e9'    '4.286578688e9'
        //
        //
        // num2strexact(double2fix([1 1 - 2.^ -[23 24 25 26]], 32, 23, 0))
        // ans =
        // '8.388608e6'    '8.388607e6'    '8.388607e6'    '8.388607e6'    '8.388607e6'
        //
        // num2strexact(double2fix([1 1 - 2.^ -[23 24 25 26]], 32, 23, 1))
        // ans =
        // '8.388608e6'    '8.388607e6'    '8.388608e6'    '8.388608e6'    '8.388608e6'
        //
        // Comparison rounding / half up
        // num2strexact(fix2double(double2fix( [-1.0, -0.75, -0.5, -0.25, 0, 0.25, 0.5, 0.75], 2, 1, 2), 2, 1))
        // ans =
        // '-1'    '-1'    '-0.5'    '-0.5'    '0'    '0.5'    '0.5'    '0.5'
        //
        // See also FIX2DOUBLE.
        //
        // Author: Thomas Pilgrim, Tobias Wurzbacher(rounding mode), RDSP
        //
        // $Id: double2fix.m, v 1.5 2014 / 10 / 30 15:39:40 z00134tw Exp $
        internal static uint Double2Fix(double value, int nBits, int nFrac, int mode = 0)
        {
            uint fix_dec;

            if (value >= 0)
            {
                var rawVal = Math.Abs(value) * Math.Pow(2, nFrac);

                switch (mode)
                {
                    case 0:
                        fix_dec = (uint)Math.Floor(rawVal);
                        break;
                    case 1:
                        fix_dec = (uint)Math.Round(rawVal);
                        break;
                    case 2:
                        fix_dec = (uint)Math.Floor(rawVal + 0.5);
                        break;
                    default:
                        throw new ArgumentException("Double2Fix: you gave an invalid rounding mode");
                }

                if (fix_dec > (Math.Pow(2, nBits - 1) - 1))
                {
                    //disp(['Warning: clipping from ' num2str(val_double(m, n)) ' at maximum fixpoint value to ' num2str((2 ^ (n_bits - 1) - 1))]);
                    fix_dec = (uint)Math.Pow(2, nBits - 1) - 1;
                }
            }
            else
            {
                var signed_int = 0u;
                var lowLimit = (uint)-Math.Pow(2, nBits - 1);
                // do the signed rounding
                switch (mode)
                {
                    case 0:
                        signed_int = (uint)Math.Floor(value * Math.Pow(2, nFrac)); // cut lsb - 1;
                        break;
                    case 1:
                        signed_int = (uint)Math.Floor(value * Math.Pow(2, nFrac)); // round mathmatical;
                        break;
                    case 2:
                        signed_int = (uint)Math.Floor(value * Math.Pow(2, nFrac + 0.5)); //% round half up;
                        break;
                }

                if (signed_int < lowLimit)
                {
                    //disp(['Warning: clipping from ' num2str(val_double(m, n)) ' at minimum fixpoint value to ' num2str(lowLimit)]);
                    signed_int = lowLimit;
                }

                // calculate the bit pattern with sign extension
                if (signed_int != 0) // we might have rounded to 0
                    fix_dec = (uint)Math.Pow(2, nBits) + signed_int;
                else
                    fix_dec = 0;
            }

            return fix_dec;
        }

        // Convert real-valued input to rfloat format
        //
        // [out] = double2rfloat(in, nmant, nscal, offset)
        //
        // Convert real-valued input to rfloat format given as
        // mantissa and exponent.Input is clipped at[-1...1).
        //
        // Input arguments:
        //   in:     real - valued input signal matrix(n by m; n: time, m: vector dimension, e.g., 48)
        //   nmant:  number of bits used for matissa
        //   nscal:  number of bits used for exponent
        //   offset: offset to exponent in order to represent value > 1
        //   mode:           Rounding operation mode for the mantissa
        //                   0 = floor as IC default, 
        //                   1 = round mathmatical correct
        //                   2 = round half up. (i.e.add 1 to lsb-1 and
        //                       floor)
        //
        // Output argument:
        //   out:    output matrix of dimension n by 2*m containing mantissa
        //           and exponent for each input column
        //
        //           Note that for vector input signals(along dimension 2) the
        //           output components(mant, scal) are grouped together
        //           (e.g., [mant(1) mant(2) scal(1) scal(2)]), being
        //           compatible to the input ordering of our cpp-modules.
        //
        // See also RFLOAT_BITPATTERN2DOUBLE, DOUBLE2RFLOAT, COMPLEX2CFLOAT_BITPATTERN
        //
        // Feb. 2015, Tobias Rosenkranz, PWD SIP
        // $Id: double2rfloat.m,v 1.1 2014/01/27 14:37:23 z002y6nr Exp $
        internal static uint Double2rfloat_Bitpattern(double input, int nmant, int nscal, int offset)
        {
            // add small value to positive values since possible range of mantissa is [-1...1)
            // --> positive powers of 2(+2 ^ -scal) are left-shifted by 1 and exponent
            // is decremented by 1 in contrast to their negative counterparts(-2 ^ -scal)
            var in4scal = input;

            //get exponent
            var scal = Math.Max(Math.Min(Math.Ceiling(Log2(Math.Abs(in4scal))) * (-1.0), Math.Pow(2, nscal) - 1 - offset), -offset);

            if (in4scal * Math.Pow(2, scal) >= 1)
                scal -= 1;

            scal = Math.Max(scal, -offset);

            //get mantissa
            var mant = Double2Fix(input * Math.Pow(2, scal), nmant, nmant - 1);

            //shift to correct position in bit pattern
            scal = (scal + offset) * Math.Pow(2, nmant);

            // sum components to get int-representation of bitpattern
            var output = mant + scal;

            return (uint)output;
        }

        // Convert rfloat bitpattern to double.
        //
        // [out] = rfloat_bitpattern2double(bitpattern, nmant, nscal, offset)
        //
        // Convert rfloat format given as mantissa and exponent to real-valued double.
        //
        // Input arguments:
        //   in:     input matrix of dimension n by m containing the
        // int - representation of the rrfloat bitpattern as it would be used
        //           for storing in a register
        // nmant:  number of bits used for matissas
        // nscal:  number of bits used for exponent
        // offset: offset to exponent in order to represent value > 1
        //
        // Output argument:
        //   out:    real - valued output signal matrix(n by m; n: time, m: vector dimension, e.g., 48)
        //
        // See also: DOUBLE2RFLOAT_BITPATTERN, RFLOAT2DOUBLE, CFLOAT_BITPATTERN2COMPLEX
        //
        // Feb. 2015, Tobias Rosenkranz, PWD SIP
        // $Id: rfloat2double.m,v 1.1 2014 / 01 / 27 14:37:23 z002y6nr Exp $
        internal static double Rfloat_Bitpattern2Double(uint bitpattern, int nmant, int nscal, int offset = 0)
        {
            var scal = bitpattern & (uint)(Math.Pow(2, nmant + nscal) - Math.Pow(2, nmant));
            var mant = bitpattern & (uint)(Math.Pow(2, nmant) - Math.Pow(2, 0));

            var scal_double = Fix2Double(scal, nmant + nscal + 1, nmant);
            var mant_double = Fix2Double(mant, nmant, nmant - 1);

            var output = mant_double * Math.Pow(2, -scal_double + offset);

            return output;
        }

        internal static double Log2(double input) => Math.Log10(input) / Math.Log10(2);

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

            var doubleValue = Fix2Double((uint)fixValue, nBits, nFrac);

            return doubleValue;
        }
    }
}
