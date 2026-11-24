namespace QuantumSimulator.Test.Quantum;

using QuantumSimulator.Quantum;
using System.Numerics;
using Xunit;

public class QuantumStateTest
{
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
        Assert.Equal(0.0, probabilityOne, 1e-10);
        Assert.Equal(1.0, 1.0 - probabilityOne, 1e-10);
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
        Assert.Equal(0.5, probabilityOne, 1e-10);
        Assert.Equal(0.5, 1.0 - probabilityOne, 1e-10);
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
        Assert.Equal(0.5, state.GetProbability(0), 1e-10);
        Assert.Equal(0.5, state.GetProbability(1), 1e-10);
    }
}