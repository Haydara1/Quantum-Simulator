namespace QuantumSimulator.Test.QMath;

using QuantumSimulator.QMath;
using System.Numerics;
using Xunit;

public class MatrixTest
{
    #region Tensor product tests
    
    [Fact]
    public void Tensor_TwoScalarMatrices_ReturnsProduct()
    {
        // Arrange
        Complex[,] a = { { 2 } };
        Complex[,] b = { { 3 } };

        // Act
        var result = Matrix.Tensor(a, b);

        // Assert
        Assert.Equal(1, result.GetLength(0));
        Assert.Equal(1, result.GetLength(1));
        Assert.Equal(new Complex(6, 0), result[0, 0]);
    }

    [Fact]
    public void Tensor_NonSquareMatrices_ReturnsCorrectMatrix()
    {
        // Arrange
        Complex[,] a =
        {
            { 1, 2 }
        };

        Complex[,] b =
        {
            { 3, 4, 5 },
            { 6, 7, 8 }
        };

        Complex[,] expected =
        {
            { 3, 4, 5, 6, 8, 10 },
            { 6, 7, 8, 12, 14, 16 }
        };

        // Act
        var result = Matrix.Tensor(a, b);

        // Assert
        AssertMatrixEqual(expected, result);
    }
    
    [Fact]
    public void Tensor_ComplexMatrices_ReturnsCorrectMatrix()
    {
        // Arrange
        Complex[,] a =
        {
            { new Complex(1, 1), new Complex(2, -1) }
        };

        Complex[,] b =
        {
            { new Complex(1, 2) },
            { new Complex(3, -1) }
        };

        Complex[,] expected =
        {
            {
                new Complex(-1, 3),
                new Complex(4, 3)
            },
            {
                new Complex(4, 2),
                new Complex(5, -5)
            }
        };

        // Act
        var result = Matrix.Tensor(a, b);

        // Assert
        AssertMatrixEqual(expected, result);
    }

    [Fact]
    public void Tensor_NullFirstMatrix_ThrowsArgumentNullException()
    {
        // Arrange
        Complex[,] b = { { 1 } };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => Matrix.Tensor(null!, b)
        );
    }

    [Fact]
    public void Tensor_NullSecondMatrix_ThrowsArgumentNullException()
    {
        // Arrange
        Complex[,] a = { { 1 } };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => Matrix.Tensor(a, null!)
        );
    }

    private static void AssertMatrixEqual(
        Complex[,] expected,
        Complex[,] actual)
    {
        Assert.Equal(expected.GetLength(0), actual.GetLength(0));
        Assert.Equal(expected.GetLength(1), actual.GetLength(1));

        for (int i = 0; i < expected.GetLength(0); i++)
        {
            for (int j = 0; j < expected.GetLength(1); j++)
            {
                Assert.True(
                    Complex.Abs(expected[i, j] - actual[i, j])
                    < Util.Epsilon,
                    $"Mismatch at [{i}, {j}]: expected {expected[i, j]}, got {actual[i, j]}"
                );
            }
        }
    }
    
    #endregion
}