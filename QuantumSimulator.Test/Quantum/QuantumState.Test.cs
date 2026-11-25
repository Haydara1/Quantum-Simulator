namespace QuantumSimulator.Test.Quantum;

using QuantumSimulator.Quantum;
using QuantumSimulator.QMath;
using System.Numerics;
using Xunit;

public class QuantumStateTest
{
    #region Constructor Tests
    
    [Fact]
    public void Constructor_ValidAmplitudes_CreatesState()
    {
        // Arrange
        Complex[] amplitudes =
        {
            Complex.One,
            Complex.Zero
        };

        // Act
        var state = new QuantumState(amplitudes);

        // Assert
        Assert.Equal(1, state.QubitCount);
        Assert.Equal(2, state.Amplitudes.Count);
        Assert.Equal(Complex.One, state.Amplitudes[0]);
        Assert.Equal(Complex.Zero, state.Amplitudes[1]);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(6)]
    public void Constructor_InvalidArrayLength_ThrowsException(int length)
    {
        // Arrange
        Complex[] amplitudes = new Complex[length];

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new QuantumState(amplitudes));
    }

    [Fact]
    public void Constructor_NonNormalizedState_ThrowsException()
    {
        // Arrange
        Complex[] amplitudes =
        {
            new Complex(0.5, 0),
            new Complex(0.5, 0)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new QuantumState(amplitudes));
    }

    [Fact]
    public void Constructor_NullAmplitudes_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new QuantumState(null!));
    }

    [Fact]
    public void Constructor_NonFiniteAmplitude_ThrowsException()
    {
        // Arrange
        Complex[] amplitudes =
        {
            new Complex(double.NaN, 0),
            Complex.Zero
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new QuantumState(amplitudes));
    }
    
    
    [Fact]
    public void Constructor_ClonesInputArray()
    {
        // Arrange
        Complex[] amplitudes =
        {
            Complex.One,
            Complex.Zero
        };

        var state = new QuantumState(amplitudes);

        // Act
        amplitudes[0] = Complex.Zero;

        // Assert
        Assert.Equal(Complex.One, state.Amplitudes[0]);
    }

    #endregion
    
    #region GetProbability Tests
    
    [Fact]
    public void GetProbability_ZeroState_ReturnsZero()
    {
        // Arrange
        var state = new QuantumState(new Complex[]
        {
            Complex.One,
            Complex.Zero
        });

        // Act
        double probabilityOne = state.GetProbability(0);

        // Assert
        Assert.Equal(0.0, probabilityOne, Util.Epsilon);
        Assert.Equal(1.0, 1.0 - probabilityOne, Util.Epsilon);
    }

    [Fact]
    public void GetProbability_EqualSuperposition_ReturnsHalf()
    {
        // Arrange
        double amplitude = 1.0 / Math.Sqrt(2);

        var state = new QuantumState(new Complex[]
        {
            amplitude,
            amplitude
        });

        // Act
        double probabilityOne = state.GetProbability(0);

        // Assert
        Assert.Equal(0.5, probabilityOne, Util.Epsilon);
        Assert.Equal(0.5, 1.0 - probabilityOne, Util.Epsilon);
    }

    [Fact]
    public void GetProbability_BellState_ReturnsHalfForBothQubits()
    {
        // Arrange
        double amplitude = 1.0 / Math.Sqrt(2);

        var state = new QuantumState(new Complex[]
        {
            amplitude,
            Complex.Zero,
            Complex.Zero,
            amplitude
        });

        // Act & Assert
        Assert.Equal(0.5, state.GetProbability(0), Util.Epsilon);
        Assert.Equal(0.5, state.GetProbability(1), Util.Epsilon);
    }
    
    #endregion
    
    #region Dot product Tests

    [Fact]
    public void Multiply_RealStates_ReturnsCorrectInnerProduct()
    {
        // Arrange
        var q1 = new QuantumState(new Complex[] { 1, 0 });
        var q2 = new QuantumState(new Complex[] { 0, 1 });

        // Act
        Complex result = q1 * q2;

        // Assert
        Assert.Equal(0.0, result.Real, Util.Epsilon);
        Assert.Equal(0.0, result.Imaginary, Util.Epsilon);
    }

    [Fact]
    public void Multiply_ComplexStates_ReturnsCorrectInnerProduct()
    {
        // Arrange
        double a = 1.0 / Math.Sqrt(2);

        var q1 = new QuantumState(new Complex[]
        {
            a,
            Complex.ImaginaryOne * a
        });

        var q2 = new QuantumState(new Complex[]
        {
            a,
            a
        });

        // Act
        Complex result = q1 * q2;

        // Expected: 1/2 - i/2
        // Assert
        Assert.Equal(0.5, result.Real, Util.Epsilon);
        Assert.Equal(-0.5, result.Imaginary, Util.Epsilon);
    }

    [Fact]
    public void Multiply_StateByItself_ReturnsOne()
    {
        // Arrange
        double a = 1.0 / Math.Sqrt(2);

        var state = new QuantumState(new Complex[]
        {
            a,
            Complex.ImaginaryOne * a
        });

        // Act
        Complex result = state * state;

        // Assert
        Assert.Equal(1.0, result.Real, Util.Epsilon);
        Assert.Equal(0.0, result.Imaginary, Util.Epsilon);
    }

    [Fact]
    public void Multiply_DifferentStateSizes_ThrowsException()
    {
        // Arrange
        var q1 = new QuantumState(new Complex[] { 1, 0 });
        var q2 = new QuantumState(new Complex[]
        {
            1.0 / Math.Sqrt(2),
            0,
            0,
            1.0 / Math.Sqrt(2)
        });

        // Act & Assert
        Assert.Throws<ArgumentException>(() => q1 * q2);
    }
    
    #endregion
    
    #region Outer product Tests
    
    [Fact]
    public void OuterProduct_RealStates_ReturnsCorrectMatrix()
    {
        // Arrange
        var q1 = new QuantumState(new Complex[]
        {
            1 / Math.Sqrt(2), 1 / Math.Sqrt(2)
        });

        var q2 = new QuantumState(new Complex[]
        {
            1, 0
        });

        double expected = 1 / Math.Sqrt(2);

        // Act
        Complex[,] result = QuantumState.OuterProduct(q1, q2);

        // Assert
        Assert.True(Complex.Abs(
            result[0, 0] - new Complex(expected, 0)) < Util.Epsilon);

        Assert.True(Complex.Abs(
            result[0, 1] - Complex.Zero) < Util.Epsilon);

        Assert.True(Complex.Abs(
            result[1, 0] - new Complex(expected, 0)) < Util.Epsilon);

        Assert.True(Complex.Abs(
            result[1, 1] - Complex.Zero) < Util.Epsilon);
    }

    [Fact]
    public void OuterProduct_ComplexStates_ReturnsCorrectMatrix()
    {
        // Arrange
        var q1 = new QuantumState(new Complex[]
        {
            new Complex(1 / Math.Sqrt(2), 0),
            new Complex(0, 1 / Math.Sqrt(2))
        });

        var q2 = new QuantumState(new Complex[]
        {
            new Complex(1 / Math.Sqrt(2), 0),
            new Complex(0, -1 / Math.Sqrt(2))
        });

        // Act
        Complex[,] result = QuantumState.OuterProduct(q1, q2);

        // Assert
        double half = 0.5;

        Assert.True(Complex.Abs(
            result[0, 0] - new Complex(half, 0)) < Util.Epsilon);

        Assert.True(Complex.Abs(
            result[0, 1] - new Complex(0, half)) < Util.Epsilon);

        Assert.True(Complex.Abs(
            result[1, 0] - new Complex(0, half)) < Util.Epsilon);

        Assert.True(Complex.Abs(
            result[1, 1] - new Complex(-half, 0)) < Util.Epsilon);
    }

    [Fact]
    public void OuterProduct_DifferentDimensions_ThrowsArgumentException()
    {
        // Arrange
        var q1 = new QuantumState(new Complex[] { 1, 0 });

        var q2 = new QuantumState(new Complex[]
        {
            1, 0, 0, 0
        });

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => QuantumState.OuterProduct(q1, q2));
    }

    [Fact]
    public void OuterProduct_NullState_ThrowsArgumentNullException()
    {
        // Arrange
        var q1 = new QuantumState(new Complex[] { 1, 0 });

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => QuantumState.OuterProduct(q1, null!));

        Assert.Throws<ArgumentNullException>(
            () => QuantumState.OuterProduct(null!, q1));
    }

    [Fact]
    public void OuterProduct_ReturnsCorrectMatrixDimensions()
    {
        // Arrange
        var q1 = new QuantumState(new Complex[]
        {
            0.5, 0.5, 0.5, 0.5
        });

        // Act
        Complex[,] result = QuantumState.OuterProduct(q1, q1);

        // Assert
        Assert.Equal(4, result.GetLength(0));
        Assert.Equal(4, result.GetLength(1));
    }

    

    [Fact]
    public void OuterProduct_SameState_ReturnsHermitianMatrix()
    {
        // Arrange
        var q = new QuantumState(new Complex[]
        {
            new Complex(0.5, 0.5),
            new Complex(0.5, -0.5)
        });

        // Act
        Complex[,] result = QuantumState.OuterProduct(q, q);

        // Assert
        for (int i = 0; i < result.GetLength(0); i++)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                Assert.True(Complex.Abs(
                    result[i, j] -
                    Complex.Conjugate(result[j, i])) < Util.Epsilon);
            }
        }
    }

    #endregion
}