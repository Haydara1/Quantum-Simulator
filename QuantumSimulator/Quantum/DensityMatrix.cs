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
        if(!Matrix.IsDiagonalReal(amplitudes))
            throw new ArgumentException("Diagonal coefficients must be real.");
        
        // Check if the diagonal is normalized
        if(!Matrix.IsDiagonalNormalized(amplitudes))
            throw new ArgumentException("Diagonal coefficients must sum to one.");
        
        // Check if off-diagonal elements are complex conjugates of each other.
        if(!Matrix.IsHermitian(amplitudes))
            throw new ArgumentException("Matrix has to be Hermitian.");
        
        QubitCount = BitOperations.Log2((uint)amplitudes.GetLength(0));
        Amplitudes = (Complex[,])amplitudes.Clone();
    }
    
    #endregion
    
    #region Public methods

    /// <summary>
    /// Returns the purity of the density matrix, given by the trace of the square of the density matrix.
    /// </summary>
    /// <returns></returns>
    public Complex GetPurity()
        => Matrix.Trace(Matrix.Multiply(Amplitudes, Amplitudes));
    
    #endregion
}