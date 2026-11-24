namespace QuantumSimulator.Quantum;

// For more information: https://en.wikipedia.org/wiki/Density_matrix

/*
 * Things to check for a density Matrix:
 * 1- Of size 2^n x 2^n, i.e 4^n coefficients.
 * 2- Diagonal numbers are real numbers that sum to 1.
 * 3- Off-diagonal elements are complex conjugates of each other.
 */

using System.Numerics;
using QMath;

public class DensityMatrix
{
    #region Attributes

    /// <summary>
    /// Coefficients of the density matrix.
    /// </summary>
    public Complex[,] Amplitudes { get; private set; }

    public int QubitCount { get; private set; }

    #endregion
    
    #region Constructors

    /// <summary>
    /// Main constructor for the Density matrix class.
    /// </summary>
    /// <param name="amplitudes"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DensityMatrix(Complex[,] amplitudes)
    {
        if (amplitudes == null)
            throw new ArgumentNullException(nameof(amplitudes));

        // Check if the lengths are power of two:
        if ((amplitudes.GetLength(0) & (amplitudes.GetLength(0) - 1)) != 0 ||
            (amplitudes.GetLength(1) & (amplitudes.GetLength(1) - 1)) != 0)
            throw new ArgumentException("Matrix dimensions must be a power of 2.");
        
        // Check if the matrix is a square matrix
        if(amplitudes.GetLength(0) != amplitudes.GetLength(1))
            throw new ArgumentException("Matrix dimensions must be the same length.");
        
        // Check if the diagonal is real:
        if(!IsDiagonalReal(amplitudes))
            throw new ArgumentException("Diagonal coefficients must be real.");
        
        // Check if the diagonal is normalized
        if(!IsDiagonalNormalized(amplitudes))
            throw new ArgumentException("Diagonal coefficients must sum to one.");
        
        // Check if off-diagonal elements are complex conjugates of each other.
        if(!IsHermitian(amplitudes))
            throw new ArgumentException("Matrix has to be Hermitian.");
        
        QubitCount = BitOperations.Log2((uint)amplitudes.GetLength(0));
        Amplitudes = amplitudes;
    }
    
    #endregion
    
    #region Private methods

    /// <summary>
    /// Checks if the sum of the diagonal coefficients is equal to 1.
    /// </summary>
    /// <param name="amplitudes"></param>
    /// <returns></returns>
    private static bool IsDiagonalNormalized(Complex[,] amplitudes)
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
    private static bool IsDiagonalReal(Complex[,] amplitudes)
    {
        for(int i = 0; i < amplitudes.GetLength(0); i++)
            if(Math.Abs(amplitudes[i, i].Imaginary) > Util.Epsilon)
                return false;
        
        return true;
    }

    private static bool IsHermitian(Complex[,] amplitudes)
    {
        // This works supposing that the amplitudes matrix is a square matrix
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
    
    #endregion
}