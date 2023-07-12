using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class PlayerPrefsEncryption
{
    private static PlayerPrefsEncryption instance;
    private static readonly byte[] Key = Encoding.ASCII.GetBytes("01234567890123456789012345678901"); // 32바이트
    private static readonly byte[] IV = Encoding.ASCII.GetBytes("0123456789012345"); // 16바이트
    //private static readonly byte[] Key = Encoding.ASCII.GetBytes("01234567890123456789012345678901"); // 32바이트
    //private static readonly byte[] IV = Encoding.ASCII.GetBytes("0123456789012345"); // 16바이트

    public static PlayerPrefsEncryption Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerPrefsEncryption();
            }

            return instance;
        }
    }

    private PlayerPrefsEncryption() { }

    /// <summary>
    /// 인자로 받은 key에대해 암호화된 값을 설정, 
    /// PlayerPrefs에 저장할 Key, 암호화하여 저장할 값 value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetGeneric<T>(string key, T value)
    {
        byte[] encryptedValue = EncryptValue(value);
        string base64String = System.Convert.ToBase64String(encryptedValue);
        Debug.Log("Encrypted Value: " + base64String);

        PlayerPrefs.SetString(key, base64String);
    }

    /// <summary>
    /// 암호화된 값을 가져와서 복호화한 후 반환,
    /// defaultValue 주어진 key값이 존재하지않을때 반환할 기본값
    /// try catch로 기본값 반환 받으면됩니다
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T GetGeneric<T>(string key, T defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        string base64String = PlayerPrefs.GetString(key);
        byte[] encryptedValue = System.Convert.FromBase64String(base64String);
        Debug.Log("Encrypted Value: " + Encoding.ASCII.GetString(encryptedValue));

        T decryptedValue = DecryptValue<T>(encryptedValue);
        Debug.Log("Decrypted Value: " + decryptedValue.ToString());

        return decryptedValue;
    }

    /// <summary>
    /// 암호화 하는 메서드. 인자로 들어온 value를 직렬화 후 AES대칭키 알고리즘으로 암호화
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte[] EncryptValue<T>(T value)
    {
        // 값(value)을 직렬화하여 바이트 배열로 변환합니다.
        byte[] serializedValue = SerializeValue(value);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key; // 암호화에 사용할 대칭키(Key)를 설정합니다.
            aes.IV = IV; // 암호화에 사용할 초기화 벡터(IV)를 설정합니다.

            // 대칭키 암호화를 수행할 수 있는 ICryptoTransform 객체를 생성합니다.
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedBytes;

            using (var ms = new System.IO.MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                // 암호화된 데이터를 쓰기 위한 CryptoStream을 생성합니다.
                // 이 CryptoStream은 암호화를 수행하여 데이터를 쓸 수 있도록 합니다.
                cs.Write(serializedValue, 0, serializedValue.Length);
                cs.FlushFinalBlock();
                encryptedBytes = ms.ToArray(); // 암호화된 데이터를 바이트 배열로 변환합니다.
            }

            return encryptedBytes; // 암호화된 데이터의 바이트 배열을 반환합니다.
        }
    }
    /// <summary>
    /// 암호화된 byte배열을 AES대칭키 알고리즘으로 복호화, 복호화된 바이트 배열을 역직렬화 후 원래 데이터로 변환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="encryptedBytes"></param>
    /// <returns></returns>
    public T DecryptValue<T>(byte[] encryptedBytes)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key; // 복호화에 사용할 대칭키(Key)를 설정합니다.
            aes.IV = IV; // 복호화에 사용할 초기화 벡터(IV)를 설정합니다.

            // 대칭키 복호화를 수행할 수 있는 ICryptoTransform 객체를 생성합니다.
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decryptedBytes;

            using (var ms = new System.IO.MemoryStream(encryptedBytes))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var decryptedStream = new System.IO.MemoryStream())
            {
                // 복호화된 데이터를 읽기 위한 CryptoStream을 생성합니다.
                // 이 CryptoStream은 암호화된 데이터를 읽어서 복호화된 데이터를 생성합니다.
                cs.CopyTo(decryptedStream);
                decryptedBytes = decryptedStream.ToArray(); // 복호화된 데이터를 바이트 배열로 변환합니다.
            }

            // 복호화된 바이트 배열을 역직렬화하여 원래의 값으로 변환합니다.
            T deserializedValue = DeserializeValue<T>(decryptedBytes);
            return deserializedValue; // 복호화된 값(T)을 반환합니다.
        }
    }
    /// <summary>
    /// value값을 직렬화하고 바이트 배열로 변환(암호화)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte[] SerializeValue<T>(T value)
    {
        // 메모리에 데이터를 저장하기 위한 MemoryStream 인스턴스 생성
        using (var ms = new System.IO.MemoryStream())
        {   // 값을 직렬화하기 위한 BinaryFormatter 인스턴스 생성
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            // MemoryStream에 값(value)을 직렬화하여 저장
            formatter.Serialize(ms, value);
            // MemoryStream을 바이트 배열로 변환하여 반환
            return ms.ToArray();
        }
    }
    /// <summary>
    /// 주어진 바이트 배열을 역직렬화 하고 원래 값으로 되돌림(복호화)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public T DeserializeValue<T>(byte[] bytes)
    {    // 주어진 바이트 배열(bytes)을 읽기 위한 MemoryStream 인스턴스 생성
        using (var ms = new System.IO.MemoryStream(bytes))
        {   // 역직렬화를 위한 BinaryFormatter 인스턴스 생성
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            // MemoryStream에서 바이트 배열(bytes)을 역직렬화하여 원래의 값으로 변환
            return (T)formatter.Deserialize(ms);
        }
    }

    public void Remove(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void RemoveAll()
    {
        PlayerPrefs.DeleteAll();
    }
}