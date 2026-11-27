namespace QuantumSimulator.QMath;

using System.Numerics;
using Quantum;

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

    /// <summary>
    /// Calculates the tensor product of two complex matrices.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static Complex[,] Tensor(Complex[,] a, Complex[,] b)
    {
        if (a == null || b == null)
            throw new ArgumentNullException();

        // Define rows and columns
        var rowsA = a.GetLength(0);
        var colsA = a.GetLength(1);

        var rowsB = b.GetLength(0);
        var colsB = b.GetLength(1);

        Complex[,] result = new Complex[
            rowsA * rowsB,
            colsA * colsB
        ];

        for (var i = 0; i < rowsA; i++)
            for (var j = 0; j < colsA; j++)
                for (var k = 0; k < rowsB; k++)
                    for (var l = 0; l < colsB; l++)
                        result[i * rowsB + k, j * colsB + l] = a[i, j] * b[k, l];
        
        return result;
    }

    /// <summary>
    /// Calculates the tensor product of two quantum states.
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static QuantumState Tensor(QuantumState q1, QuantumState q2)
    {
        if (q1 == null || q2 == null)
            throw new ArgumentNullException();
        
        // Get the complex matrices of the quantum states (column vectors)
        Complex[,] columnQ1 = TransposeRowVector((Complex[]) q1.Amplitudes);
        Complex[,] columnQ2 = TransposeRowVector((Complex[]) q2.Amplitudes);
        
        // Calculate the tensor product.
        Complex[,] columnStateVector = Tensor(columnQ1, columnQ2);
        
        // Return the quantum state.
        return new QuantumState(TransposeColumnVector(columnStateVector));
    }
    
    
    #endregion
    
    #region Private functions

    /// <summary>
    /// Transposes a row vector.
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    private static Complex[,] TransposeRowVector(Complex[] a)
    {
        // Create a column vector 
        Complex[,] output = new Complex[a.Length, 1];
        
        for(int i = 0; i < a.Length; i++)
            output[i, 0] = a[i];
        
        return output;
    }


    /// <summary>
    /// Transposes a column vector.
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    private static Complex[] TransposeColumnVector(Complex[,] a)
    {
        Complex[] result = new Complex[a.GetLength(0)];
        
        for(int i = 0; i < a.GetLength(0); i++)
            result[i] = a[i, 0];
        
        return result;
    }
    #endregion
    
}