using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KakituPlugin;

public class TestUtils : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    // Validate kshs/raw functions
    if (!KakituUtils.ValidateKshs("100.123"))
    {
      Debug.Log("Error validating kshs");
      return;
    }

    if (!KakituUtils.ValidateKshs("100,123"))
    {
      Debug.Log("Error validating kshs");
      return;
    }

    if (!KakituUtils.ValidateKshs("340282366.920938463463374607431768211455"))
    {
      Debug.Log("Error validating kshs");
      return;
    }

    if (!KakituUtils.ValidateKshs("0.1231231"))
    {
      Debug.Log("Error validating kshs");
      return;
    }

    if (!KakituUtils.ValidateKshs(".1223"))
    {
      Debug.Log("Error validating kshs");
      return;
    }

    if (!KakituUtils.ValidateKshs(",1223"))
    {
      Debug.Log("Error validating kshs");
      return;
    }

    if (KakituUtils.ValidateKshs(".122.3"))
    {
      Debug.Log("Error validating kshs, 2 decimal points");
      return;
    }

    if (!KakituUtils.ValidateRaw("100"))
    {
      Debug.Log("Error validating raw");
      return;
    }

    if (!KakituUtils.ValidateRaw("340282366920938463463374607431768211455"))
    {
      Debug.Log("Error validating raw");
      return;
    }

    if (!KakituUtils.ValidateRaw("0001"))
    {
      Debug.Log("Error validating raw");
      return;
    }

    if (KakituUtils.ValidateRaw("100.123"))
    {
      Debug.Log("Error validating raw");
      return;
    }

    if (KakituUtils.ValidateRaw("100@123"))
    {
      Debug.Log("Error validating raw");
      return;
    }

    if (KakituUtils.ValidateRaw("1111111111111111111111111111111111111111111111111111111111"))
    {
      Debug.Log("Error validating raw");
      return;
    }

    // 1 above max raw
    if (KakituUtils.ValidateRaw("340282366920938463463374607431768211456"))
    {
      Debug.Log("Error validating raw");
      return;
    }

    // Raw to kshs
    var raw = "10000000000000000000000000000000";
    var kshs = KakituUtils.RawToKshs(raw);
    if (!kshs.Equals("10.0"))
    {
      Debug.Log("Error converting raw to kshs");
      return;
    }

    raw = "1000000000000000000000000000000";
    kshs = KakituUtils.RawToKshs(raw);
    if (!kshs.Equals("1.0"))
    {
      Debug.Log("Error converting raw to kshs");
      return;
    }

    raw = "100000000000000000000000000000";
    kshs = KakituUtils.RawToKshs(raw);
    if (!kshs.Equals("0.1"))
    {
      Debug.Log("Error converting raw to kshs");
      return;
    }

    raw = "10000000000000000000000000000";
    kshs = KakituUtils.RawToKshs(raw);
    if (!kshs.Equals("0.01"))
    {
      Debug.Log("Error converting raw to kshs");
      return;
    }

    raw = "100000000000000000000000000";
    kshs = KakituUtils.RawToKshs(raw);
    if (!kshs.Equals("0.0001"))
    {
      Debug.Log("Error converting raw to kshs");
      return;
    }

    raw = "100";
    kshs = KakituUtils.RawToKshs(raw);
    if (!kshs.Equals("0.0000000000000000000000000001"))
    {
      Debug.Log("Error converting raw to kshs");
      return;
    }

    raw = "1";
    kshs = KakituUtils.RawToKshs(raw);
    if (!kshs.Equals("0.000000000000000000000000000001"))
    {
      Debug.Log("Error converting raw to kshs");
      return;
    }

    // Kshs to raw
    kshs = "10.0";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("10000000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    kshs = "10";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("10000000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    kshs = "1.0";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("1000000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    kshs = "0.1";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("100000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    kshs = ".1";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("100000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    kshs = "00000.1";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("100000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    kshs = "0.01";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("10000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    kshs = "0.000000000000000000000000000001";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("1"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    // Test localization of using a comma
    kshs = "0,01";
    raw = KakituUtils.KshsToRaw(kshs);
    if (!raw.Equals("10000000000000000000000000000"))
    {
      Debug.Log("Error converting kshs to raw");
      return;
    }

    // Test KakituAmount
    if (!((new KakituAmount("1") + new KakituAmount("2")).Equals(new KakituAmount("3")))) {
      Debug.Log("Error with adding");
      return;
    }

    if (!((new KakituAmount("2") - new KakituAmount("1")).Equals(new KakituAmount("1")))) {
      Debug.Log("Error with subtracting");
      return;
    }

    if (!(new KakituAmount("3000") > new KakituAmount("2000")))
    {
      Debug.Log("Error with greater");
      return;
    }

    if ((new KakituAmount("2000") > new KakituAmount("2000")))
    {
      Debug.Log("Error with greater");
      return;
    }

    if ((new KakituAmount("1999") > new KakituAmount("2000")))
    {
      Debug.Log("Error with greater");
      return;
    }

    if (!(new KakituAmount("3000") >= new KakituAmount("2000")))
    {
      Debug.Log("Error with greater or equal");
      return;
    }

    if (!(new KakituAmount("2000") >= new KakituAmount("2000")))
    {
      Debug.Log("Error with greater or equal");
      return;
    }

    if ((new KakituAmount("1999") > new KakituAmount("2000")))
    {
      Debug.Log("Error with greater or equal");
      return;
    }

    var amount = new KakituAmount(100);

    byte[] bytes = KakituUtils.HexStringToByteArray("E989DE925A4EDEE45447158557AD1409450315491F147F4AAA8F37DCA355354A");

    byte[] b = KakituUtils.AddressToPublicKeyByteArray("kshs_3kqdiqmqiojr1aqqj51aq8bzz5jtwnkmhb38qwf3ppngo8uhhzkdkn7up7rp");
    string s1 = KakituUtils.SignHash("E989DE925A4EDEE45447158557AD1409450315491F147F4AAA8F37DCA355354A", bytes);
    string s2 = KakituUtils.PublicKeyToAddress(bytes);
    string s3 = KakituUtils.ByteArrayToHexString(bytes);

    var prvKey = KakituUtils.GeneratePrivateKey();
    var password = "cheese_cake" + Random.value;
    var filename = "privateKey1.kshs";

    KakituUtils.SavePrivateKey(KakituUtils.ByteArrayToHexString(prvKey), filename, password);
    var originalPrivateKey = KakituUtils.ByteArrayToHexString(prvKey);
    var extractedPrivateKey = KakituUtils.LoadPrivateKey(filename, password);
    if (!originalPrivateKey.Equals(extractedPrivateKey))
    {
      Debug.Log("Error decrypting privateKey");
      return;
    }

    Debug.Log("Successfully tested Utils");
  }

  // Update is called once per frame
  void Update()
  {

  }
}
