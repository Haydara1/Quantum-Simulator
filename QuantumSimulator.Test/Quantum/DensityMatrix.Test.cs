namespace QuantumSimulator.Test.Quantum;

using System.Numerics;
using Xunit;
using QuantumSimulator.Quantum;

public class DensityMatrixTests
{
    
    #region Constructor Tests
    
    [Fact]
    public void Constructor_NullMatrix_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new DensityMatrix(null!));
    }

    [Fact]
    public void Constructor_NonPowerOfTwoDimensions_ThrowsArgumentException()
    {
        // Arrange
        Complex[,] matrix = new Complex[3, 3];

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new DensityMatrix(matrix));
    }

    [Fact]
    public void Constructor_NonSquareMatrix_ThrowsArgumentException()
    {
        // Arrange
        Complex[,] matrix = new Complex[2, 4];

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new DensityMatrix(matrix));
    }

    [Fact]
    public void Constructor_ValidMatrix_SetsCorrectQubitCount()
    {
        // Arrange
        Complex[,] matrix =
        {
            { 0.5, 0 },
            { 0, 0.5 }
        };

        // Act
        var result = new DensityMatrix(matrix);

        // Assert
        Assert.Equal(1, result.QubitCount);
    }

    [Fact]
    public void Constructor_DiagonalDoesNotSumToOne_ThrowsArgumentException()
    {
        // Arrange
        Complex[,] matrix =
        {
            { 0.2, 0 },
            { 0, 0.2 }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new DensityMatrix(matrix));
    }

    [Fact]
    public void Constructor_DiagonalContainsImaginaryPart_ThrowsArgumentException()
    {
        // Arrange
        Complex[,] matrix =
        {
            { new Complex(0.5, 0.1), 0 },
            { 0, new Complex(0.5, -0.1) }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new DensityMatrix(matrix));
    }

    [Fact]
    public void Constructor_ValidHermitianMatrix_DoesNotThrow()
    {
        // Arrange
        Complex[,] matrix =
        {
            { 0.5, new Complex(0, -0.5) },
            { new Complex(0, 0.5), 0.5 }
        };

        // Act
        var exception = Record.Exception(
            () => new DensityMatrix(matrix));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_NonHermitianMatrix_ThrowsArgumentException()
    {
        // Arrange
        Complex[,] matrix =
        {
            { 0.5, new Complex(0, 0.3) },
            { new Complex(0, 0.5), 0.5 }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new DensityMatrix(matrix));
    }
    
    #endregion
}