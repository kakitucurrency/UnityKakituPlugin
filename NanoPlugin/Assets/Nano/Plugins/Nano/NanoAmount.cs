using System;
using System.Numerics;

namespace KakituPlugin
{
  public class KakituAmount
  {
    public static BigInteger max = BigInteger.Parse("340282366920938463463374607431768211455");

    public static KakituAmount MAX_VALUE = new KakituAmount(max);

    private BigInteger rawValue;

    /**
     * Creates a KakituAmount from a given {@code raw} value.
     *
     * @param rawValue the raw value
     */
    public KakituAmount(int rawValue)
    {
      if (rawValue < 0)
        throw new ArgumentException("Raw value cannot be negative.");
    }

    /**
     * Creates a KakituAmount from a given {@code raw} value.
     *
     * @param rawValue the raw value
     */
    public KakituAmount(string rawValue)
    {
      if (KakituUtils.ValidateRaw(rawValue))
      {
        this.rawValue = BigInteger.Parse(rawValue);
      }
    }

    /**
     * Creates a KakituAmount from a given {@code raw} value.
     *
     * @param rawValue the raw value
     */
    public KakituAmount(BigInteger rawValue)
    {
      if (rawValue == null)
        throw new ArgumentException("Raw value cannot be null.");
      if (rawValue < BigInteger.Zero || rawValue > max)
        throw new ArgumentException("Raw value is outside the possible range.");
      this.rawValue = rawValue;
    }

    /**
     * Output the raw value as a string.
     */
    public override string ToString()
    {
      return rawValue.ToString();
    }

    /**
     * Returns the value of this amount in the {@code raw} unit.
     *
     * @return the value, in raw units
     */
    public BigInteger getAsRaw()
    {
      return rawValue;
    }

    /**
     * Returns the value of this amount in the standard base unit ({@link KakituUnit#BASE_UNIT}).
     *
     * @return the value, in the base unit
     */
    public string getAsKshs()
    {
      return KakituUtils.RawToKshs(ToString());
    }

    public override int GetHashCode()
    {
      return rawValue.GetHashCode();
    }

    public static KakituAmount operator +(KakituAmount a, KakituAmount b) => new KakituAmount(a.rawValue + b.rawValue);
    public static KakituAmount operator -(KakituAmount a, KakituAmount b) => new KakituAmount(a.rawValue - b.rawValue);
    public static bool operator <(KakituAmount a, KakituAmount b) => a.rawValue < b.rawValue;
    public static bool operator >(KakituAmount a, KakituAmount b) => a.rawValue > b.rawValue;
    public static bool operator <=(KakituAmount a, KakituAmount b) => a.rawValue <= b.rawValue;
    public static bool operator >=(KakituAmount a, KakituAmount b) => a.rawValue >= b.rawValue;

    public override bool Equals(Object obj)
    {
      if ((obj == null) || !this.GetType().Equals(obj.GetType()))
      {
        return false;
      }
      else
      {
        KakituAmount p = (KakituAmount)obj;
        return rawValue.Equals (p.rawValue);
      }
    }
  }
}