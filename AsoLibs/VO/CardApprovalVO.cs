using AsoLibs.POS;
using System;
using System.Collections.Generic;

namespace AsoLibs.VO
{
    [Serializable]
    public class CardApprovalVO
    {
        static public readonly int[] bytes = new int[] {
            4,      // 전문길이     : 스페이스
            1,      // 승인여부     : "O" 승인 그외 거절
            37,     // 카드번호     : 카드번호 Track2 포함    
            8,      // 승인금액     : 승인금액
            8,      // 세금         : 세금
            12,     // 승인번호     : 승인번호
            15,     // 가맹점번호   : 가맹점번호
            4,      // 발급사코드   : 발급사코드
            30,     // 발급사명     : 발급사명
            4,      // 매입사코드   : 매입사코드
            30      // 매입사명     : 매입사명
        };

        private bool trimValue = false;
        private int returnValue = 0;
        private string rawData = null;

        private List<string> fieldValues;

        private string getWordByByte(string src, int index, int count)
        {
            string result = null;

            if (src != null)
            {
                System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

                byte[] buf = myEncoding.GetBytes(src);

                if (buf.Length > index && index + count <= buf.Length)
                {
                    result = myEncoding.GetString(buf, index, count);
                }
            }

            return result;
        }


        private void SetData(string data)
        {
            try
            {
                rawData = data;

                if (data != null)
                {
                    fieldValues = new List<string>();
                    int currentPos = 0;

                    foreach (int length in bytes)
                    {
                        string item = null;

                        if (currentPos < data.Length)
                        {
                            item = getWordByByte(data, currentPos, length);
                        }
                        fieldValues.Add(item);
                        currentPos += length;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string GetValue(KOVANFieldNames fieldName)
        {
            int idx = (int)fieldName;
            return GetValue(idx);
        }

        public string GetValue(int idx)
        {
            string result = null;

            if (fieldValues != null && fieldValues.Count >= idx)
            {
                result = fieldValues[idx];
            }

            return result;
        }


    }
}
