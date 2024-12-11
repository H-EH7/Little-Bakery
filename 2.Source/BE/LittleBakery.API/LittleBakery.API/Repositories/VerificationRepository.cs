using LittleBakery.API.Models;

namespace LittleBakery.API.Repositories
{
    public static class VerificationRepository
    {
        private static Dictionary<string, VerificationNumber> _verificationDic = new Dictionary<string, VerificationNumber>();

        public static void Upsert(string key, VerificationNumber verificationNumber)
        {
            KeyValuePair<string, VerificationNumber> existVerif = _verificationDic.Where(kp => kp.Key == key).FirstOrDefault();

            if (existVerif.Equals(default(KeyValuePair<string, VerificationNumber>)))
            {
                _verificationDic.Add(key, verificationNumber);
            }
            else
            {
                _verificationDic[existVerif.Key] = verificationNumber;
            }
        }

        public static void Delete(string key) => _verificationDic.Remove(key);

        public static bool Verify(string key, string number)
        {
            KeyValuePair<string, VerificationNumber> existVerif = _verificationDic.Where(kp => kp.Key == key).FirstOrDefault();

            if (existVerif.Equals(default(KeyValuePair<string, VerificationNumber>))
                || existVerif.Value.Number != number)
                return false;

            return true;
        }
    }
}
