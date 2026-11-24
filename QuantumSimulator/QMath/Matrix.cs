namespace QuantumSimulator.QMath;

using System.Numerics;

internal static class Matrix
{
    #region Internal static functions
    
    /// <summary>
    /// Calculates the trace of a given square matrix.
    /// </summary>
    /// <param name="amplitudes"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static Complex Trace(Complex[,] amplitudes)
    {
        // Check that the matrix is square:
        if (amplitudes.GetLength(0) != amplitudes.GetLength(1))
            throw new ArgumentException("Can't calculate trace on a non-square matrix.");
        
        Complex sum = Complex.Zero;
        
        for(int i = 0; i < amplitudes.GetLength(0); i++)
            sum += amplitudes[i, i];
        
        return sum;
    }
    
    /// <summary>
    /// Checks whether a complex square matrix is Hermitian.
    /// </summary>
    /// <param name="amplitudes"></param>
    /// <returns></returns>
    internal static bool IsHermitian(Complex[,] amplitudes)
    {
        // Check that the matrix is square:
        if (amplitudes.GetLength(0) != amplitudes.GetLength(1))
            throw new ArgumentException("Hermitian character is reserved to square matrices.");
        
        for (int i = 0; i < amplitudes.GetLength(0); i++)
        {
            for (int j = 0; j < amplitudes.GetLength(1); j++)
            {
                if(Complex.Abs(amplitudes[i, j] - Complex.Conjugate(amplitudes[j, i]))
                   > Util.Epsilon)
                    return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Checks if the sum of the diagonal coefficients is equal to 1.
    /// </summary>
    /// <param name="amplitudes"></param>
    /// <returns></returns>
    internal static bool IsDiagonalNormalized(Complex[,] amplitudes)
    {
        Complex sum = Complex.Zero;
       
        // This works supposing that the amplitudes matrix is a square matrix
        for(int i = 0; i < amplitudes.GetLength(0); i++)
            sum += amplitudes[i, i];
       
        return (Math.Abs(sum.Real - 1) < Util.Epsilon) &&
               (Math.Abs(sum.Imaginary) < Util.Epsilon);
    }

    /// <summary>
    /// Checks if the diagonal coefficients of the given matrix are real numbers.
    /// </summary>
    /// <param name="amplitudes"></param>
    /// <returns></returns>
    internal static bool IsDiagonalReal(Complex[,] amplitudes)
    {
        for(int i = 0; i < amplitudes.GetLength(0); i++)
            if(Math.Abs(amplitudes[i, i].Imaginary) > Util.Epsilon)
                return false;
        
        return true;
    }
    
    #endregion
    
    #region Operaors

    /// <summary>
    /// Multiplies two complex matrices.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static Complex[,] Multiply(Complex[,] a, Complex[,] b)
    {
        if (a.GetLength(1) != b.GetLength(0))
            throw new ArgumentException("Can't multiply matrices of unmatching dimensions.");
        
        Complex[,] result = new Complex[a.GetLength(0), b.GetLength(1)];

        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < b.GetLength(1); j++)
            {
                Complex coefficient = Complex.Zero;
                
                for(int k =  0; k < a.GetLength(1); k++)
                    coefficient += a[i, k] * b[k, j];
                
                result[i, j] = coefficient;
            }
        }
        
        return result;
    }
    
    #endregion
    
}